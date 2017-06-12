using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.IO;
using System.Drawing;

//using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

using Neo4j.Driver.V1;
using System.Threading;

namespace KE_Angular.Models
{
    class WebPageParser
    {

        //使用mercury解析网页内容
        public static String WebPageParseMercury(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"https://mercury.postlight.com/parser?url=" + url);
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Headers.Add("x-api-key: WHDMnQZU46DMgOYW0rXNr5YovnSrysyqgAPoTnlL");
                //request.Headers.Add("Accept-Charset: utf-8");

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader strRdr = new StreamReader(response.GetResponseStream());

                return strRdr.ReadToEnd();
            }
            catch (System.Net.WebException e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
        //去除内容中的尖括号部分
        public static string GetContent_DeTag(String content)
        {
            if (content == null)
            {
                return null;
            }
            int trigger = 0;
            StringBuilder s = new StringBuilder();
            foreach (char c in content)
            {
                if (trigger == 0)
                {
                    if (c.Equals('<'))
                    {
                        trigger = 1;
                        continue;
                    }
                    s.Append(c);
                }
                else
                {
                    if (trigger == 1)
                    {
                        if (c.Equals('>'))
                        {
                            trigger = 0;
                        }
                    }
                }
            }
            return s.ToString();
        }
        //获取特定html标签中特定属性的值
        public static List<string> GetHtmlAttr(string html, string tag, string attr)
        {
            Regex re = new Regex(@"(<" + tag + @"[\w\W].+?>)");
            MatchCollection imgreg = re.Matches(html);
            List<string> m_Attributes = new List<string>();
            Regex attrReg = new Regex(@"([a-zA-Z1-9_-]+)\s*=\s*(\x27|\x22)([^\x27\x22]*)(\x27|\x22)", RegexOptions.IgnoreCase);
            for (int i = 0; i < imgreg.Count; i++)
            {
                MatchCollection matchs = attrReg.Matches(imgreg[i].ToString());
                for (int j = 0; j < matchs.Count; j++)
                {
                    GroupCollection groups = matchs[j].Groups;
                    if (attr.ToUpper() == groups[1].Value.ToUpper())
                    {
                        m_Attributes.Add(groups[3].Value);
                        break;
                    }
                }
            }
            return m_Attributes;
        }
        //计算机视觉API获取图片描述
        static async Task<string> MakeImageAnalysisRequest(string url)
        {
            try
            {
                var client = new HttpClient();
                //HttpUtility引用自System.Web.dll
                var queryString = HttpUtility.ParseQueryString(string.Empty);

                //请求头
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "016fb77b43cf45f4b09ae8c904437a59");

                //请求参数
                queryString["visualFeatures"] = "Description";
                queryString["details"] = "Celebrities";
                queryString["language"] = "en";
                var uri = "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0/analyze?" + queryString;

                HttpResponseMessage response;

                //请求主体
                byte[] byteData = Encoding.UTF8.GetBytes("{\"url\":\"" + url + "\"}");

                using (var content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    response = await client.PostAsync(uri, content);
                }

                StreamReader strRdr = new StreamReader(await response.Content.ReadAsStreamAsync());
                return strRdr.ReadToEnd();
            }
            catch (System.Net.WebException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static string MakeImageAnalysisRequest_notasync(string url)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["visualFeatures"] = "Description";
            queryString["details"] = "Celebrities";
            queryString["language"] = "en";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"https://westcentralus.api.cognitive.microsoft.com/vision/v1.0/analyze?" + queryString);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("Ocp-Apim-Subscription-Key: 016fb77b43cf45f4b09ae8c904437a59");
            Stream requeststream = request.GetRequestStream();
            StreamWriter sw = new StreamWriter(requeststream, Encoding.GetEncoding("utf-8"));
            sw.Write("{\"url\":\"" + url + "\"}");
            sw.Close();
            //request.Headers.Add("Accept-Charset: utf-8");

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader strRdr = new StreamReader(response.GetResponseStream());

            return strRdr.ReadToEnd();
        }

        static async Task<Stream> MakeThumbNailRequest(string url)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "016fb77b43cf45f4b09ae8c904437a59");

            // Request parameters
            queryString["width"] = "400";
            queryString["height"] = "400";
            queryString["smartCropping"] = "true";
            var uri = "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0/generateThumbnail?" + queryString;

            HttpResponseMessage response;

            // Request body
            byte[] byteData = Encoding.UTF8.GetBytes("{\"url\":\"" + url + "\"}");

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(uri, content);
                return await response.Content.ReadAsStreamAsync();
            }

        }

        public static async Task<double> MakeSimilarityRequest(string s1, string s2)
        {
            var client = new HttpClient();

            // GET请求串
            //var queryString = HttpUtility.ParseQueryString(string.Empty);

            // 请求头
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "b93aeababbdc449494a2f5a6227dcfaa");

            // GET请求内容参数
            /*
            queryString["s1"] = s1;
            queryString["s2"] = s2;
            */

            var uri = "https://westus.api.cognitive.microsoft.com/academic/v1.0/similarity?";

            // POST方法
            client.BaseAddress = new Uri(uri);
            var queryContent = new FormUrlEncodedContent(
                new Dictionary<string, string>()
                {
                    {"s1",s1 },
                    {"s2",s2 }
                });

            var response = await client.PostAsync(uri, queryContent);

            //GET方法
            //var response = await client.GetAsync(uri + queryString);

            StreamReader strRdr = new StreamReader(await response.Content.ReadAsStreamAsync());
            return Double.Parse(strRdr.ReadToEnd());
        }
        /*
        static async Task<string> MakeEntityLinkingRequest(string text)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "816e72934be04d1eafb02cdbc4e2ffcb");

            // Request parameters
            //queryString["selection"] = "";
            //queryString["offset"] = "";
            var uri = "https://westus.api.cognitive.microsoft.com/entitylinking/v1.0/link?" + queryString;

            HttpResponseMessage response;

            // Request body
            byte[] byteData = Encoding.UTF8.GetBytes(text);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                response = await client.PostAsync(uri, content);

                StreamReader strRdr = new StreamReader(await response.Content.ReadAsStreamAsync());
                return strRdr.ReadToEnd();
            }
            
        }
        */
        public static string MakeEntityLinkingRequest(string text)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"https://westus.api.cognitive.microsoft.com/entitylinking/v1.0/link?");
                request.Method = "POST";
                request.ContentType = "text/plain";
                request.Headers.Add("Ocp-Apim-Subscription-Key: 816e72934be04d1eafb02cdbc4e2ffcb");
                Stream requeststream = request.GetRequestStream();
                StreamWriter sw = new StreamWriter(requeststream, Encoding.GetEncoding("utf-8"));
                sw.Write(text);
                sw.Close();
                //request.Headers.Add("Accept-Charset: utf-8");

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader strRdr = new StreamReader(response.GetResponseStream());

                return strRdr.ReadToEnd();
            }
            catch (System.Net.WebException e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
        //取得特定网络图片的描述
        public static async Task<string> GetImageAnalysis(string imageUrl)
        {
            string r = await MakeImageAnalysisRequest(imageUrl);

            /*
            JObject jo = JObject.Parse(r);
            //获取名称
            List<string> name = new List<string>();
            JArray categories = JArray.Parse(jo["categories"].ToString());
            foreach (JObject j in categories)
            {
                name.Add(j["name"].ToString());
            }
            */

            JObject jo_r = (JObject)JsonConvert.DeserializeObject(r);
            string name = jo_r["categories"][0]["name"].ToString();
            string tags = "";
            foreach (JValue jv in jo_r["description"]["tags"])
            {
                tags = tags + jv.ToString() + ";";
            }
            string text = jo_r["description"]["captions"][0]["text"].ToString();


            return "(A picture of" + name + ", describing " + text + ".\n" + "Contanining elements of " + tags + ")";
        }

        public static void CreateIllustEntry(string imgurl, string pageurl, IDriver d)
        {
            var session = d.Session();
            string r = MakeImageAnalysisRequest_notasync(imgurl);
            JObject jo = (JObject)JsonConvert.DeserializeObject(r);
            List<string> tags = new List<string>();
            foreach (JValue jv in jo["description"]["tags"])
            {
                tags.Add(jv.ToString());
            }
            session.Run("MATCH (p:WebPage) WHERE p.url={url} CREATE (p)-[:Contains]->(i:Illustration{imgurl:{imgurl},tags:{tags},format:{format},width:{width},height{height}})",
                        new Dictionary<string, object> { { "url", pageurl }, { "imgurl", imgurl }, { "tags", tags }, { "format", jo["metadata"]["format"].ToString() }, { "width", int.Parse(jo["metadata"]["width"].ToString()) }, { "height", int.Parse(jo["metadata"]["height"].ToString()) } });
        }

        //取得特定网页的全部内容，包括文本和图片描述
        public static async Task<string> GetWebPageFullText(string url)
        {
            //获取网页解析结果
            String response_mercury = WebPageParseMercury(url);
            //响应错误退出
            if (response_mercury == null)
            {
                return null;
            }
            //获取文本标题、去除正文html标签
            JObject jo = JObject.Parse(response_mercury);
            string title = jo["title"].ToString();
            string content = jo["content"].ToString();
            string content_detag = GetContent_DeTag(content);
            //提取正文图片url，获取图片内容描述
            List<string> imgurl = GetHtmlAttr(content, "img", "src");
            List<string> description = new List<string>();
            foreach (string s in imgurl)
            {
                description.Add(await GetImageAnalysis(s));
            }
            //拼接为网页全文
            string fulltext = title + "\n" + content_detag + "\n";
            /*
            foreach (string s in description)
            {
                fulltext = fulltext + s + "\n";
            }
            */
            return fulltext + "\n";
        }

        public static List<string> GetWebPageTags(string content)
        {
            string response = MakeEntityLinkingRequest(content);
            JObject jo = (JObject)JsonConvert.DeserializeObject(response);
            JArray ja = JArray.Parse(jo["entities"].ToString());
            List<string> tags = new List<string>();
            foreach (JObject s_jo in ja)
            {
                tags.Add(s_jo["wikipediaId"].ToString());
            }
            return tags;
        }

        public static async Task<WebPage> GenerateWebPageAsync(string url)
        {
            WebPage p = new WebPage();
            string response_mercury = WebPageParseMercury(url);
            p.attr = JsonConvert.DeserializeObject<WebPageAttr>(response_mercury);
            p.content_detag = GetContent_DeTag(p.attr.content);
            string EntityLinkingQuery = p.content_detag.Substring(0, 1999);
            List<string> tags = GetWebPageTags(EntityLinkingQuery);
            List<string> illusts = GetHtmlAttr(p.attr.content, "img", "src");
            foreach (string s in illusts)
            {
                p.illusts.Add(new Illustration(s));
            }
            foreach (string s in tags)
            {
                p.tags.Add(s);
            }
            return p;
        }

        public static async void SaveAsThumbNail(string url, string directory)
        {
            Stream s = await MakeThumbNailRequest(url);

            var img = new Bitmap(s);
            img.Save(directory, System.Drawing.Imaging.ImageFormat.Jpeg);
            s.Close();

        }

        public static async void tester(string url)
        {
            /*
            WebPage p = new WebPage();
            p.attr.url = "testurl2";
            //p.attr.content = "testcontent";
            p.attr.title = "testtitle";
            p.illusts.Add(new Illustration("testimgurl1"));
            p.illusts.Add(new Illustration("testimgurl2"));
            p.tags.Add(new WikiTag("id1"));
            p.tags.Add(new WikiTag("id2"));
            var driver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "123456"));
            //driver.Session().Run(@"match(p:WebPage) delete p");
            try
            {
                Neo4jDatabase.CreateWebPageEntry(p, driver);
            }
            catch(Neo4j.Driver.V1.ClientException e)
            {
                Console.WriteLine(e.Message);
            }
            */

            /*
            WebPage p = await GenerateWebPage(url);
            foreach(WikiTag t in p.tags)
            {
                Console.WriteLine(t.wikipediaid);
            }
            var driver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "123456"));
            try
            {
                Neo4jDatabase.CreateWebPageEntry(p, driver);
            }
            catch (Neo4j.Driver.V1.ClientException e)
            {
                Console.WriteLine(e.Message);
            }
            */
            url = Console.ReadLine();
            var driver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "123456"));
            Neo4jDatabase.CreateWebPageEntry(url);
            Console.WriteLine("done");
            Console.ReadLine();
        }
    }

    public class WebPage
    {
        public WebPageAttr attr;

        public string content_detag;
        public List<Illustration> illusts;
        public List<string> tags;

        public WebPage()
        {
            attr = new WebPageAttr();
            illusts = new List<Illustration>();
            tags = new List<string>();
        }
    }

    public class WebPageAttr
    {
        public string title;
        public string content;
        public string author;
        public string date_published;
        public string lead_image_url;
        public string dek;
        public string next_page_url;
        public string url;
        public string domain;
        public string excerpt;
        public int word_count;
        public string direction;
        public int total_pages;
        public int rendered_pages;
    }

    public class Illustration
    {
        public string imgurl;
        public List<string> tags;
        public string format;
        public int width;
        public int height;

        public Illustration(string url)
        {
            imgurl = url;
            tags = null;
            format = null;
            width = 0;
            height = 0;
        }
    }

    public class Neo4jDatabase
    {

        public static void ConnectDatabase(IDriver driver)
        {
            //using (var driver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "123456")))

            using (var session = driver.Session())
            {
                //session.Run("CREATE (a:Person {name: {name}, title: {title}})",
                //            new Dictionary<string, object> { { "name", "Arthur" }, { "title", "King" } });

                var result = session.Run("MATCH (a:Person) WHERE a.name = {name} " +
                                         "RETURN a.name AS name, a.title AS title",
                                         new Dictionary<string, object> { { "name", "Arthur" } });

                foreach (var record in result)
                {
                    Console.WriteLine($"{record["title"].As<string>()} {record["name"].As<string>()}");
                }
            }
        }

        public static void AddConstraint(IDriver d)
        {
            var session = d.Session();
            try
            {
                session.Run("CREATE CONSTRAINT ON (p:WebPage) ASSERT p.url IS UNIQUE");
            }
            catch (Neo4j.Driver.V1.ClientException e)
            {
                ;
            }
            try
            {
                session.Run("CREATE CONSTRAINT ON (t:WikiTag) ASSERT t.WikiPediaID IS UNIQUE");
            }
            catch (Neo4j.Driver.V1.ClientException e)
            {
                ;
            }
            try
            {
                session.Run("CREATE CONSTRAINT ON (i:Illustration) ASSERT i.imgurl IS UNIQUE");
            }
            catch (Neo4j.Driver.V1.ClientException e)
            {
                return;
            }
        }
        /*
        public static void CreateWebPageEntry(WebPage p,IDriver d)
        {
            using (var session = d.Session())
            {
                string url = p.attr.url;
                var result = session.Run("CREATE (p:WebPage {url: {url}, htmlsource: {htmlsource}, title: {title}}) RETURN p",
                            new Dictionary<string, object> { { "url", p.attr.url }, { "htmlsource", p.attr.content }, { "title", p.attr.title } });
                foreach (Illustration i in p.illusts)
                {
                    session.Run("MATCH (p:WebPage) WHERE p.url={url} CREATE (p)-[:Contains]->(i:Illustration{imgurl:{imgurl}})",
                        new Dictionary<string, object> { { "url",url}, { "imgurl",i.imgurl} });
                }
                
                foreach (string t in p.tags)
                {
                    session.Run("MATCH (p:WebPage) WHERE p.url={url} CREATE (p)-[:tag]->(t:WikiTag{WikiPediaID:{WikiPediaID}})",
                        new Dictionary<string, object> { { "url", url }, { "WikiPediaID", t } });
                }
                
            }
        }
        */

        public static List<WebPage> GetAllEntries()
        {
            var d = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "19961117JC"));
            List<WebPage> entries = new List<WebPage>();
            var session = d.Session();
            var tx = session.BeginTransaction();
            try
            {
                var result_pages = tx.Run("MATCH (p:WebPage) RETURN p");
                foreach (var p_record in result_pages)
                {
                    WebPage p = new WebPage();
                    p.attr = JsonConvert.DeserializeObject<WebPageAttr>(((INode)p_record["p"])["attr_json"].As<string>());
                    p.content_detag = WebPageParser.GetContent_DeTag(p.attr.content);
                    var result_tags = tx.Run("MATCH (p:WebPage)-[:tag]->(t:WikiTag) WHERE p.url={url} return t",
                                        new Dictionary<string, object> { { "url", p.attr.url } });
                    foreach (var t_record in result_tags)
                    {
                        p.tags.Add(((INode)t_record["t"])["WikiPediaID"].As<string>());
                    }

                    entries.Add(p);
                    /*
                    var result_illusts = tx.Run("MATCH (p:WebPage)-[:contains]->(i:Illustration) WHERE p.url={url} RETURN i",
                                        new Dictionary<string, object> { { "url",p.attr.url } });
                    foreach (var i_record in result_illusts)
                    {
                        Illustration i = new Illustration(i_record["imgurl"].As<string>());
                    }
                    */
                }
                tx.Success();
            }
            finally
            {
                tx.Dispose();
            }
            return entries;
        }

        public static void CreateWebPageEntry(string url)
        {
            var d = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "19961117JC"));
            string attr_json = WebPageParser.WebPageParseMercury(url);
            JObject jo1 = (JObject)JsonConvert.DeserializeObject(attr_json);
            List<string> img_urls = WebPageParser.GetHtmlAttr(jo1["content"].ToString(), "img", "src");
            List<string> tags;
            try
            {
                tags = WebPageParser.GetWebPageTags(WebPageParser.GetContent_DeTag(jo1["content"].ToString()).Substring(0, 1999));
            }
            catch (ArgumentOutOfRangeException)
            {
                tags = WebPageParser.GetWebPageTags(WebPageParser.GetContent_DeTag(jo1["content"].ToString()));
            }
            tags = tags.Distinct<string>().ToList<string>();
            using (var session = d.Session())
            {
                var tx = session.BeginTransaction();
                try
                {
                    tx.Run("CREATE (p:WebPage {url: {url}, title: {title},attr_json:{attr_json}}) RETURN p",
                                    new Dictionary<string, object> { { "url", url }, { "title", jo1["title"].ToString() }, { "attr_json", attr_json } });
                    foreach (string i in img_urls)
                    {
                        string r = "";
                        try
                        {
                            r = WebPageParser.MakeImageAnalysisRequest_notasync(i);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                        JObject jo2 = (JObject)JsonConvert.DeserializeObject(r);
                        List<string> i_tags = new List<string>();
                        foreach (JValue jv in jo2["description"]["tags"])
                        {
                            i_tags.Add(jv.ToString());
                        }
                        tx.Run("MATCH (p:WebPage) WHERE p.url={url} CREATE (p)-[:contains]->(i:Illustration{imgurl:{imgurl},tags:{tags},format:{format},width:{width},height:{height}})",
                                    new Dictionary<string, object> { { "url", url }, { "imgurl", i }, { "tags", i_tags }, { "format", jo2["metadata"]["format"].ToString() }, { "width", int.Parse(jo2["metadata"]["width"].ToString()) }, { "height", int.Parse(jo2["metadata"]["height"].ToString()) } });
                    }

                    foreach (string t in tags)
                    {
                        var result = tx.Run("MATCH (t:WikiTag) WHERE t.WikiPediaID={WikiPediaID} return t",
                                            new Dictionary<string, object> { { "WikiPediaID", t } });
                        int r_count = 0;
                        foreach (var record in result)
                        {
                            r_count++;
                        }
                        if (r_count == 0)
                        {
                            tx.Run("MATCH (p:WebPage) WHERE p.url={url} CREATE (p)-[:tag]->(t:WikiTag{WikiPediaID:{WikiPediaID}})",
                                  new Dictionary<string, object> { { "url", url }, { "WikiPediaID", t } });
                        }
                        else
                        {
                            tx.Run("MATCH (p:WebPage),(t:WikiTag) WHERE p.url={url} AND t.WikiPediaID={WikiPediaID} CREATE (p)-[:tag]->(t)",
                                  new Dictionary<string, object> { { "url", url }, { "WikiPediaID", t } });
                        }
                    }

                    tx.Success();
                }
                finally
                {
                    tx.Dispose();
                }

            }
        }

        public static void CreateIllustEntry(Illustration i, IDriver d)
        {
            using (var session = d.Session())
            {
                session.Run("CREATE (i:Illustration)");
            }
        }

        public static String GetWebPageListJson(List<WebPage> itemList)
        {
            return JsonConvert.SerializeObject(itemList);
        }

        public static WebPage GetWebPageEntry(string url)
        {
            var d = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "19961117JC"));
            List<WebPage> entries = new List<WebPage>();
            var session = d.Session();
            var tx = session.BeginTransaction();
            try
            {
                var result_pages = tx.Run("MATCH (p:WebPage) WHERE p.url={url} RETURN p",
                                        new Dictionary<string, object> { { "url", url } });
                foreach (var p_record in result_pages)
                {
                    WebPage p = new WebPage();
                    p.attr = JsonConvert.DeserializeObject<WebPageAttr>(((INode)p_record["p"])["attr_json"].As<string>());
                    p.content_detag = WebPageParser.GetContent_DeTag(p.attr.content);
                    var result_tags = tx.Run("MATCH (p:WebPage)-[:tag]->(t:WikiTag) WHERE p.url={url} return t",
                                        new Dictionary<string, object> { { "url", p.attr.url } });
                    foreach (var t_record in result_tags)
                    {
                        p.tags.Add(((INode)t_record["t"])["WikiPediaID"].As<string>());
                    }
                    entries.Add(p);
                }
                tx.Success();
            }
            finally
            {
                tx.Dispose();
            }
            return entries.First();
        }

        public static String GetWebpageJsonFull(WebPage thePage)
        {
            return JsonConvert.SerializeObject(thePage);
        }

        public static void DeleteWebPageEntry(string url)
        {
            var d = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "19961117JC"));
            var session = d.Session();
            var tx = session.BeginTransaction();
            try
            {
                tx.Run("MATCH (p:WebPage)-[r:contains]->(i:Illustration) WHERE p.url={url} DELETE r,i",
                       new Dictionary<string, object> { { "url", url } });
                tx.Run("MATCH (p:WebPage)-[r]->() WHERE p.url={url} DELETE r,p",
                        new Dictionary<string, object> { { "url", url } });
                tx.Run("MATCH (p:WebPage) WHERE p.url={url} DELETE p",
                        new Dictionary<string, object> { { "url", url } });
 
                tx.Success();
            }
            finally
            {
                tx.Dispose();
            }
        }

    }

    //public class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        /*
    //        using (var driver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "123456")))

    //        using (var session = driver.Session())
    //        {
    //            //session.Run("CREATE (a:Person {name: {name}, title: {title}})",
    //            //            new Dictionary<string, object> { { "name", "Arthur" }, { "title", "King" } });

    //            session.Run("MATCH (a:Person) WHERE a.name=$name DELETE a",new { name="Arthur"});
    //            var result = session.Run("MATCH (a:Person) WHERE a.name = {name} " +
    //                                     "RETURN a.name AS name, a.title AS title",
    //                                     new Dictionary<string, object> { { "name", "Arthur" } });

    //            foreach (var record in result)
    //            {
    //                Console.WriteLine($"{record["title"].As<string>()} {record["name"].As<string>()}");
    //            }
    //        }
    //        */
    //        WebPageParser.tester("");
    //    }
    //}
    /*
    public class Rootobject
    {
        public Category[] categories { get; set; }
        public Description description { get; set; }
        public string requestId { get; set; }
        public Metadata metadata { get; set; }
    }

    public class Description
    {
        public string[] tags { get; set; }
        public Caption[] captions { get; set; }
    }

    public class Caption
    {
        public string text { get; set; }
        public float confidence { get; set; }
    }

    public class Metadata
    {
        public int width { get; set; }
        public int height { get; set; }
        public string format { get; set; }
    }

    public class Category
    {
        public string name { get; set; }
        public float score { get; set; }
        public Detail detail { get; set; }
    }

    public class Detail
    {
        public object[] celebrities { get; set; }
    }
    */
}
