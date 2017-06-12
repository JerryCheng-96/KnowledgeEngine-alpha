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
            //HttpResponseMessage hrm = new HttpResponseMessage(HttpStatusCode.OK)
            //{
            //    Content = new StringContent(Paper.GetListJson(Paper.GetItemList(neo4jIf.GetPaperList())))
            //};
            //hrm.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            //return hrm;
            return Content(Paper.GetListJson(Paper.GetItemList(neo4jIf.GetPaperList())), "application/json");
        }

        public ContentResult GetPaper(String doi)
        {
            Neo4jInterface neo4jIf = new Neo4jInterface("bolt://localhost:7687", "neo4j", "19961117JC");
            return Content(Paper.GetPaperJsonFull(doi, neo4jIf), "application/json");
        }

        public ContentResult AddItem(String addStr)
        {
            Paper thePaper = DOI_CrossRef.GetPaper(addStr);
            Neo4jInterface neo4jIf = new Neo4jInterface("bolt://localhost:7687", "neo4j", "19961117JC");
            neo4jIf.AddJPaper(thePaper);
            return Content(Paper.GetPaperJsonFull(thePaper.doi, neo4jIf), "application/json"); ;
        }

        public void DeleteItem(String doi)
        {
            Neo4jInterface neo4jIf = new Neo4jInterface("bolt://localhost:7687", "neo4j", "19961117JC");
            neo4jIf.DeletePaper(doi);
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
