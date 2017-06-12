using Neo4j.Driver.V1;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KE_Angular.Models
{
    public class Paper
    {
        public String title;
        public List<Author> authorsList;
        public Journal journal;
        public String doi;
        public String volume;
        public String startPage;
        public String endPage;
        public String pubDate;
        public Uri refAbout;
        public int msId;

        public Paper()
        {
            title = "";
            authorsList = new List<Author>();
            journal = null;
            doi = "";
            volume = "";
            startPage = "";
            endPage = "";
            pubDate = "";
            refAbout = null;
            msId = -1;
        }

        public Paper(String inDoi)
        {
            title = "";
            authorsList = new List<Author>();
            journal = null;
            volume = "";
            startPage = "";
            endPage = "";
            pubDate = "";
            refAbout = null;
            msId = -1;

            doi = inDoi;
        }
        
        public Paper(Dictionary<String, object> thePaperDict)
        {
            title = (String)thePaperDict["title"];
            authorsList = new List<Author>();
            journal = null;
            volume = (String)thePaperDict["volume"];
            startPage = (String)thePaperDict["startPage"];
            endPage = (String)thePaperDict["endPage"];
            pubDate = (String)thePaperDict["pubDate"];
            refAbout = new Uri((String)thePaperDict["refAbout"]);
            msId = int.Parse((String)thePaperDict["startPage"]);
        }

        public Paper(IRecord thePaperDict)
        {
            title = (String)thePaperDict["title"];
            authorsList = new List<Author>();
            journal = null;
            volume = (String)thePaperDict["volume"];
            startPage = (String)thePaperDict["startPage"];
            endPage =  (String)thePaperDict["endPage"];
            pubDate = (String)thePaperDict["pubDate"];
            refAbout = new Uri((String)thePaperDict["refAbout"]);
            msId = int.Parse((String)thePaperDict["msId"]);
        }

        public Dictionary<String, object> GetDictionary()
        {
            return new Dictionary<string, object> {
                { "title", title },
                { "volume", volume.ToString() },
                { "startPage", startPage.ToString() },
                { "endPage", endPage.ToString() },
                { "pubDate", pubDate },
                { "refAbout", refAbout.ToString() },
                { "doi", doi },
                { "msId", msId.ToString() },
            };
        }

        public static List<PaperListItem> GetItemList(List<Paper> paperList)
        {
            List<PaperListItem> itemList = new List<PaperListItem>();

            foreach (var item in paperList)
            {
                itemList.Add(new PaperListItem(item));
            }

            return itemList;
        }

        public static String GetListJson(List<PaperListItem> itemList)
        {
            return JsonConvert.SerializeObject(itemList);
        }

        public static String GetPaperJsonFull(String paperDoi, Neo4jInterface neo4j)
        {
            return JsonConvert.SerializeObject(neo4j.GetPaper(paperDoi));
        }
    }

    public class PaperListItem
    {
        public String title;
        public String journalName;
        public List<Author> authorsList;
        public bool isOnlyAuthor;
        public int year;
        public String doi;
        public String refAbout;

        public PaperListItem(Paper thePaper)
        {
            title = thePaper.title;
            journalName = thePaper.journal.name;
            authorsList = thePaper.authorsList;
            isOnlyAuthor = thePaper.authorsList.Count == 1;
            DateTime dt;
            try
            {
                dt = DateTime.Parse(thePaper.pubDate);
                year = dt.Year;
            }
            catch (System.FormatException)
            {
                year = int.Parse(thePaper.pubDate); 
            }
            doi = thePaper.doi;
            refAbout = thePaper.refAbout.ToString();
        }
    }

    public class Author
    {
        public string familyName;
        public string givenName;
        public Uri refAbout;
        public int msId;

        public Author()
        {
            familyName = "";
            givenName = "";
            refAbout = null;
            msId = -1;
        }
        
        public Author(Dictionary<String, object> theAuthorDict)
        {
            familyName = (String)theAuthorDict["familyName"];
            givenName = (String)theAuthorDict["givenName"];
            refAbout = new Uri((String)theAuthorDict["refAbout"]);
            msId = int.Parse((String)theAuthorDict["msId"]);
        }
        
        public Author(IRecord theAuthorDict)
        {
            familyName = (String)theAuthorDict["familyName"];
            givenName = (String)theAuthorDict["givenName"];
            refAbout = new Uri((String)theAuthorDict["refAbout"]);
            msId = int.Parse((String)theAuthorDict["msId"]);
        }

        public Dictionary<String, object> GetDictionary()
        {
            return new Dictionary<string, object>
            {
                { "familyName", familyName },
                { "givenName", givenName },
                { "refAbout", refAbout.ToString() },
                { "msId", msId.ToString() }
            };
        }
    }

    public class Journal
    {
        public String name;
        public Uri refAbout;
        public int msId;

        public Journal()
        {
            name = "";
            refAbout = null;
            msId = -1;
        }

        public Journal(Dictionary<String, object> theJournalDict)
        {
            name = (String)theJournalDict["Name"];
            refAbout = new Uri((String)theJournalDict["refAbout"]);
            msId = int.Parse((String)theJournalDict["msId"]);
        }

        public Journal(IRecord theJournalDict)
        {
            name = (String)theJournalDict["name"];
            refAbout = new Uri((String)theJournalDict["refAbout"]);
            msId = int.Parse((String)theJournalDict["msId"]);
        }
 
        public Dictionary<String, object> GetDictionary()
        {
            return new Dictionary<string, object>
            {
                { "name", name },
                { "refAbout", refAbout.ToString() },
                { "msId", msId.ToString() }
            };
        }
    }
}
