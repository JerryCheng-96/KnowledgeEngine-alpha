using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4j.Driver.V1;

namespace KE_Angular.Models
{
    public class Neo4jInterface : IDisposable
    {
        private readonly IDriver _driver;

        public Neo4jInterface(string uri, string username, string password)
        {
            _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(username, password));
        }

        public ISession GetSession()
        {
            return _driver.Session();
        }

        public void AddJPaper(Paper thePaper)
        {
            Dictionary<String, object> thePaperDict = thePaper.GetDictionary();
            var result = _driver.Session().Run("MATCH (a:Paper { doi:{doi} }) RETURN a", new Dictionary<String, object> { { "doi", thePaperDict["doi"] } });
            if (result.Count() == 0)
            {
                _driver.Session().Run("CREATE (a:Paper {title: {title}, volume: {volume}, msId: {msId}, refAbout: {refAbout}, startPage: {startPage}, endPage: {endPage}, pubDate: {pubDate}, doi: {doi}, abstract: {Abstract}, note: {Note} })", thePaperDict);
            }

            foreach (var item in thePaper.authorsList)
            {
                AddAuthor(item);
                _driver.Session().Run(@"MATCH (p:Paper { doi:{doi} }), (a:Author {refAbout:{refAbout} })  MERGE (a)-[:isAuthorOf]->(p)", new Dictionary<String, object> { { "doi", thePaper.doi }, { "refAbout", item.refAbout.ToString() } });
            }

            foreach (var item in thePaper.Keywords)
            {
                AddKeywords(item);
                _driver.Session().Run(@"MATCH (p:Paper { doi:{doi} }), (k:Keyword {name:{item}})  MERGE (k)-[:isKeywordOf]->(p)", new Dictionary<String, object> { { "doi", thePaper.doi }, { "item", item } });
            }

            AddJounal(thePaper.journal);
            _driver.Session().Run(@"MATCH (p:Paper { doi:{doi} }), (j:Journal {refAbout:{refAbout} })  MERGE (p)-[v:isPartOf]->(j)", new Dictionary<String, object> { { "doi", thePaper.doi }, { "refAbout", thePaper.journal.refAbout.ToString() } });
        }


        public void ModPaperWithRef(Paper thePaper)
        {
            Dictionary<String, object> thePaperDict = thePaper.GetDictionary();

            _driver.Session().Run(@"MATCH (p:Paper { doi:{doi} }) SET p.abstract={abstract}", new Dictionary<String, object> { { "doi", thePaper.doi }, { "abstract", thePaper.Abstract } });

            foreach (var item in thePaper.Keywords)
            {
                AddKeywords(item);
                _driver.Session().Run(@"MATCH (p:Paper { doi:{doi} }), (k:Keyword {name:{item}})  MERGE (k)-[:isKeywordOf]->(p)", new Dictionary<String, object> { { "doi", thePaper.doi }, { "item", item } });
            }

            try
            {
                foreach (var item in thePaper.refDoiList)
                {
                    AddJPaper(DOI_CrossRef.GetPaper(item));
                    _driver.Session().Run(@"MATCH (p:Paper { doi:{doi} }), (r:Paper { doi:{item}}) MERGE (p)-[:references]->(r)", new Dictionary<String, object> { { "doi", thePaper.doi }, { "item", item } });
                }

            }
            catch (NullReferenceException)
            {
                ;
            }

        }

        private void AddKeywords(string item)
        {
            var result = _driver.Session().Run("MATCH (k:Keyword) WHERE k.name = {item} RETURN k.name AS name", new Dictionary<String, object> { { "item", item } });
            if (result.Count() == 0)
            {
                _driver.Session().Run("CREATE (k:Keyword { name:{item} })", new Dictionary<String, object> { { "item", item } });
            }
        }

        public void AddAuthor(Author theAuthor)
        {
            Dictionary<String, object> theAuthorDict = theAuthor.GetDictionary();
            var result = _driver.Session().Run("MATCH (a:Author) WHERE a.refAbout = {refAbout} RETURN a.refAbout AS refAbout", new Dictionary<String, object> { { "refAbout", theAuthorDict["refAbout"] } });
            if (result.Count() == 0)
            {
                _driver.Session().Run("CREATE (a:Author {familyName: {familyName}, givenName: {givenName}, refAbout: {refAbout}, msId: {msId}})", theAuthorDict);
            }
        }

        public void AddJounal(Journal theJournal)
        {
            Dictionary<String, object> theJournalDict = theJournal.GetDictionary();
            var result = _driver.Session().Run("MATCH (a:Journal) WHERE a.refAbout = {refAbout} RETURN a.refAbout AS refAbout", new Dictionary<String, object> { { "refAbout", theJournalDict["refAbout"] } });
            if (result.Count() == 0)
            {
                _driver.Session().Run("CREATE (a:Journal {name: {name}, refAbout: {refAbout}, msId: {msId}})", theJournalDict);
            }
        }

        public List<Paper> GetPaperList()
        {
            List<Paper> paperList = new List<Paper>();
            var paperRes = _driver.Session().Run("MATCH (p:Paper) RETURN p.doi AS doi");

            foreach (var item in paperRes)
            {
                paperList.Add(GetPaper((String)item["doi"]));
            }

            return paperList;
        }

        public List<Paper> GetRefPaperList(String doi)
        {
            List<Paper> paperList = new List<Paper>();
            var paperRes = _driver.Session().Run("MATCH (p:Paper {doi:{doi}})-[:references]->(n:Paper) RETURN n.doi AS doi", new Dictionary<string, object> { { "doi", doi } });

            foreach (var item in paperRes)
            {
                paperList.Add(GetPaper((String)item["doi"]));
            }

            return paperList;
        }

        public List<Paper> GetCitePaperList(String doi)
        {
            List<Paper> paperList = new List<Paper>();
            var paperRes = _driver.Session().Run("MATCH (p:Paper {doi:{doi}})<-[:references]-(n:Paper) RETURN n.doi AS doi", new Dictionary<string, object> { { "doi", doi } });

            foreach (var item in paperRes)
            {
                paperList.Add(GetPaper((String)item["doi"]));
            }

            return paperList;
        }

        public List<Paper> GetAllRelativePaper(String doi)
        {
            List<Paper> paperList = new List<Paper>();
            var paperRes = _driver.Session().Run("MATCH (p:Paper {doi:{doi}})-[]-(n:Paper) RETURN n.doi AS doi", new Dictionary<string, object> { { "doi", doi } });

            foreach (var item in paperRes)
            {
                paperList.Add(GetPaper((String)item["doi"]));
            }

            return paperList;
        }

        public Paper GetPaper(String doi)
        {
            var paperRes = _driver.Session().Run("MATCH (p:Paper) WHERE p.doi = {doi} RETURN p.title AS title, p.volume AS volume, p.msId AS msId, p.refAbout AS refAbout, p.startPage AS startPage, p.endPage AS endPage, p.pubDate AS pubDate, p.abstract AS abstract, p.Note AS note", new Dictionary<String, object> { { "doi", doi } });
            var authorRes = _driver.Session().Run("MATCH (p:Paper)<-[:isAuthorOf]-(a:Author) WHERE p.doi = {doi} RETURN a.familyName AS familyName, a.givenName AS givenName, a.refAbout AS refAbout, a.msId AS msId", new Dictionary<String, object> { { "doi", doi } });
            var kwdRes = _driver.Session().Run("MATCH (p:Paper)<-[:isKeywordOf]-(k:Keyword) WHERE p.doi = {doi} RETURN k.name AS name", new Dictionary<String, object> { { "doi", doi } });
            var journalRes = _driver.Session().Run("MATCH (p:Paper)-[:isPartOf]->(j:Journal) WHERE p.doi = {doi} RETURN j.name AS name, j.refAbout AS refAbout, j.msId AS msId", new Dictionary<String, object> { { "doi", doi } });

            List<Author> authorsList = new List<Author>();
            List<string> keywordList = new List<string>();

            foreach (var record in authorRes)
            {
                authorsList.Add(new Author(record));
            }

            foreach (var item in kwdRes)
            {
                keywordList.Add((String)item["name"]);
            }

            Paper thePaper = new Paper(paperRes.First());
            thePaper.doi = doi;
            thePaper.journal = new Journal(journalRes.First());
            thePaper.authorsList = authorsList;
            thePaper.Keywords = keywordList;

            return thePaper;
        }

        public void DeletePaper(String doi)
        {
            _driver.Session().Run("MATCH (p:Paper) WHERE p.doi = {doi} DETACH DELETE p", new Dictionary<String, object> { { "doi", doi } });
        }

        public void AmendPaper(Paper thePaper)
        {
            ModPaperWithRef(thePaper);
        }

        public void ChangeNoteToPaper(string newNote, string doi)
        {
            _driver.Session().Run("MATCH (p:Paper) WHERE p.doi = {doi} SET p.Note = {note}", new Dictionary<String, object> { { "doi", doi }, { "note", newNote} });
        }

        public void Dispose()
        {
            _driver?.Dispose();
        }
    }
}
