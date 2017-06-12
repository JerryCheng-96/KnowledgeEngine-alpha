using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KE_Angular.Models
{
    class DOI_CrossRef
    {
        public static GroupCollection FindPotentialDOI(String input)
        {
            Regex doiRegex = new Regex(@"10\.\d{4,}\/[^\s]*\s{1,}");
            var doiMatch = doiRegex.Match(input).Groups;

            return doiMatch;
        }

        public static XElement GetCrossRefInfo(String doi)
        {
            var client = new HttpClient();
            var uri = @"https://api.crossref.org/works/" + doi.Trim('/') + @"/transform/application/rdf+xml";

            return XElement.Load(HTTPHelper.HttpGetXml(uri));
        }

        public static Paper GetPaper(String doi)
        {
            Paper thePaper = new Paper(doi);
            XElement xe = GetCrossRefInfo(doi);

            XNamespace rdf = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
            XNamespace j0 = "http://purl.org/dc/terms/";
            XNamespace j1 = "http://prismstandard.org/namespaces/basic/2.1/";
            XNamespace owl = "http://www.w3.org/2002/07/owl#";
            XNamespace j2 = "http://purl.org/ontology/bibo/";
            XNamespace j3 = "http://xmlns.com/foaf/0.1/";

            IEnumerable<XElement> authorNodes = from target in xe.Descendants(j0 + @"creator")
                                                select target;
            foreach (var item in authorNodes)
            {
                Author currAuthor = new Author();
                var res1 = from target in item.Descendants(j3 + "Person")
                           select target.Attribute(rdf + "about").Value;
                currAuthor.refAbout = new Uri(res1.First());
                res1 = from target in item.Descendants(j3 + "familyName")
                       select target.Value;
                currAuthor.familyName = res1.First();
                res1 = from target in item.Descendants(j3 + "givenName")
                       select target.Value;
                currAuthor.givenName = res1.First();
                thePaper.authorsList.Add(currAuthor);
            }

            var res = from target in xe.Descendants(j1 + "volume")
                      select target.Value;
            thePaper.volume = (res.Count() == 0 ? "" : res.First());

            res = from target in xe.Descendants(j2 + "pageStart")
                  select target.Value;
            thePaper.startPage = (res.First());

            res = from target in xe.Descendants(j2 + "pageEnd")
                  select target.Value;
            thePaper.endPage = (res.First());

            res = from target in xe.Descendants(j0 + "date")
                  select target.Value;
            thePaper.pubDate = res.First();

            thePaper.journal = new Journal();
            var resNode = from target in xe.Descendants(j2 + "Journal")
                          select target;
            thePaper.journal.refAbout = new Uri(resNode.First().FirstAttribute.Value);
            var resNode2 = from target in resNode.Descendants(j0 + "title")
                           select target;
            thePaper.journal.name = resNode2.First().Value;

            res = from t in ((XElement)xe.Nodes().First()).Nodes()
                           where ((XElement)t).Name == j0 + "title"
                           select ((XElement)t).Value;
            thePaper.title = res.First();

            thePaper.refAbout = new Uri(((XElement)xe.Nodes().First()).FirstAttribute.Value); 

            return thePaper;
        }
    }
}
