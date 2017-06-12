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
                _driver.Session().Run("CREATE (a:Paper {title: {title}, volume: {volume}, msId: {msId}, refAbout: {refAbout}, startPage: {startPage}, endPage: {endPage}, pubDate: {pubDate}, doi: {doi} })", thePaperDict);
            }

            foreach (var item in thePaper.authorsList)
            {
                AddAuthor(item);
                _driver.Session().Run(@"MATCH (p:Paper { doi:{doi} }), (a:Author {refAbout:{refAbout} })  MERGE (a)-[:isAuthorOf]->(p)", new Dictionary<String, object> { { "doi", thePaper.doi }, { "refAbout", item.refAbout.ToString() } });
            }

            AddJounal(thePaper.journal);
            _driver.Session().Run(@"MATCH (p:Paper { doi:{doi} }), (j:Journal {refAbout:{refAbout} })  MERGE (p)-[v:isPartOf]->(j)", new Dictionary<String, object> { { "doi", thePaper.doi }, { "refAbout", thePaper.journal.refAbout.ToString() } });
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

        public Paper GetPaper(String doi)
        {
            var paperRes = _driver.Session().Run("MATCH (p:Paper) WHERE p.doi = {doi} RETURN p.title AS title, p.volume AS volume, p.msId AS msId, p.refAbout AS refAbout, p.startPage AS startPage, p.endPage AS endPage, p.pubDate AS pubDate", new Dictionary<String, object> { { "doi", doi } });
            var authorRes = _driver.Session().Run("MATCH (p:Paper)<-[:isAuthorOf]-(a:Author) WHERE p.doi = {doi} RETURN a.familyName AS familyName, a.givenName AS givenName, a.refAbout AS refAbout, a.msId AS msId", new Dictionary<String, object> { { "doi", doi } });
            var journalRes = _driver.Session().Run("MATCH (p:Paper)-[:isPartOf]->(j:Journal) WHERE p.doi = {doi} RETURN j.name AS name, j.refAbout AS refAbout, j.msId AS msId", new Dictionary<String, object> { { "doi", doi } });

            List<Author> authorsList = new List<Author>();

            foreach (var record in authorRes)
            {
                authorsList.Add(new Author(record));
            }

            Paper thePaper = new Paper(paperRes.First());
            thePaper.doi = doi;
            thePaper.journal = new Journal(journalRes.First());
            thePaper.authorsList = authorsList;

            return thePaper;
        }

        public void DeletePaper(String doi)
        {
            _driver.Session().Run("MATCH (p:Paper) WHERE p.doi = {doi} DETACH DELETE p", new Dictionary<String, object> { { "doi", doi } });
        }

        public void Dispose()
        {
            _driver?.Dispose();
        }
    }
}
