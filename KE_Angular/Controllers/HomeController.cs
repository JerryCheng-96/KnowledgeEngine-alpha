using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using KE_Angular.Models;
using System.Net.Http;
using System.Text;
using System.Net;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KE_Angular.Controllers
{
    [System.Web.Mvc.Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ContentResult List()
        {
            Neo4jInterface neo4jIf = new Neo4jInterface("bolt://localhost:7687", "neo4j", "19961117JC");
            return Content(Paper.GetListJson(Paper.GetItemList(neo4jIf.GetPaperList())), "application/json");
        }

        public ContentResult WebpageList()
        {
            Neo4jInterface neo4jIf = new Neo4jInterface("bolt://localhost:7687", "neo4j", "19961117JC");
            return Content(Neo4jDatabase.GetWebPageListJson(Neo4jDatabase.GetAllEntries()), "application/json");
        }


        public ContentResult RefList(String doi)
        {
            Neo4jInterface neo4jIf = new Neo4jInterface("bolt://localhost:7687", "neo4j", "19961117JC");
            return Content(Paper.GetListJson(Paper.GetItemList(neo4jIf.GetRefPaperList(doi))), "application/json");
        }

        public ContentResult CiteList(String doi)
        {
            Neo4jInterface neo4jIf = new Neo4jInterface("bolt://localhost:7687", "neo4j", "19961117JC");
            return Content(Paper.GetListJson(Paper.GetItemList(neo4jIf.GetCitePaperList(doi))), "application/json");
        }

        public ContentResult GoMSAcademic(string doi)
        {
            Neo4jInterface neo4jIf = new Neo4jInterface("bolt://localhost:7687", "neo4j", "19961117JC");
            neo4jIf.AmendPaper(MSCognitive.GetMSAcademic(neo4jIf.GetPaper(doi)));
            return Content("done!", "text/plain");
        }

        public ContentResult GoPartMSAcademic(string doi)
        {
            Neo4jInterface neo4jIf = new Neo4jInterface("bolt://localhost:7687", "neo4j", "19961117JC");
            neo4jIf.AmendPaper(MSCognitive.GetPartMSAcademic(neo4jIf.GetPaper(doi)));
            return Content("done!", "text/plain");
        }

        public ContentResult AddWebpage(string url)
        {
            Neo4jDatabase.CreateWebPageEntry(url);
            return Content("done!", "text/plain");
        }

        public ContentResult GetPaper(String doi)
        {
            Neo4jInterface neo4jIf = new Neo4jInterface("bolt://localhost:7687", "neo4j", "19961117JC");
            return Content(Paper.GetPaperJsonFull(doi, neo4jIf), "application/json");
        }

        public ContentResult GetPage(String url)
        {
            return Content(Neo4jDatabase.GetWebpageJsonFull(Neo4jDatabase.GetWebPageEntry(url)), "application/json");
        }


        public ContentResult AddItem(String addStr)
        {
            Regex doiRegex = new Regex(@"10\.\d{4,}\/[^\s]*$");
            if (doiRegex.IsMatch(addStr))
            {
                Paper thePaper = DOI_CrossRef.GetPaper(addStr);
                Neo4jInterface neo4jIf = new Neo4jInterface("bolt://localhost:7687", "neo4j", "19961117JC");
                neo4jIf.AddJPaper(thePaper);
                return Content(Paper.GetPaperJsonFull(thePaper.doi, neo4jIf), "application/json"); ;
            }
            else
            {
                Neo4jDatabase.CreateWebPageEntry(addStr);
                return Content("done!", "text/plain");
            }
        }

        public void DeleteItem(String doi)
        {
            Neo4jInterface neo4jIf = new Neo4jInterface("bolt://localhost:7687", "neo4j", "19961117JC");
            neo4jIf.DeletePaper(doi);
        }

        public ContentResult DeletePage(String uri)
        {
            Neo4jDatabase.DeleteWebPageEntry(uri);
            return Content("done!", "text/plain");
        }

        public ContentResult AddNoteToPaper(String newNote, string doi)
        {
            Neo4jInterface neo4jIf = new Neo4jInterface("bolt://localhost:7687", "neo4j", "19961117JC");
            neo4jIf.ChangeNoteToPaper(newNote, doi);
            return Content("done!", "text/plain");
        }


        [System.Web.Mvc.HttpPost]
        public object Add([FromBody]JObject theData)
        {
            if (theData == null)
            {
                throw new HttpRequestException();
            }

            return theData;
        }

    }
}
