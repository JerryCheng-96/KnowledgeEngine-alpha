using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace KE_Angular.Models
{
    public static class MSCognitive
    {
        enum AcademicAIQueryType { Evaluate, Intepret };
        static String academicAISubKey = @"f2dcf7453d114f488017604e636c39cf";

        public static async Task<String> AcademicAIEvalRequestAsync(string expr, string attr, string count = "", string offset = "", string orderby = "", string model = "")
        {
            var client = new HttpClient();
            var queryStringDict = GenQueryString();

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", academicAISubKey);

            // Request parameters
            queryStringDict["expr"] = expr;
            queryStringDict["model"] = model;
            queryStringDict["count"] = count;
            queryStringDict["offset"] = offset;
            queryStringDict["orderby"] = orderby;
            queryStringDict["attributes"] = attr;
            var uri = "https://westus.api.cognitive.microsoft.com/academic/v1.0/evaluate?" + QueryToString(queryStringDict, AcademicAIQueryType.Evaluate);

            var response = await client.GetStringAsync(uri);
            return response;
        }

        public static async Task<String> AcademicAIEvalRequestAsync(int msId, string attr, string count = "", string offset = "", string orderby = "", string model = "")
        {
            var client = new HttpClient();
            var queryStringDict = GenQueryString();

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", academicAISubKey);

            // Request parameters
            queryStringDict["expr"] = "Id=" + msId.ToString();
            queryStringDict["model"] = model;
            queryStringDict["count"] = count;
            queryStringDict["offset"] = offset;
            queryStringDict["orderby"] = orderby;
            queryStringDict["attributes"] = attr;
            var uri = "https://westus.api.cognitive.microsoft.com/academic/v1.0/evaluate?" + QueryToString(queryStringDict, AcademicAIQueryType.Evaluate);

            var response = await client.GetStringAsync(uri);
            return response;
        }

        private static string QueryToString(Dictionary<string, string> queryStringDict, AcademicAIQueryType queryType)
        {
            String res = "";

            foreach (KeyValuePair<string, string> item in queryStringDict)
            {
                if (item.Key == "expr")
                {
                    if (item.Value == "")
                    {
                        return null;
                    }
                    else
                    {
                        res += item.Key + "=" + item.Value;
                    }
                    continue;
                }
                if (item.Value != "")
                {
                    res += @"&";
                    res += item.Key + "=" + item.Value;
                }
            }

            return res;
        }

        private static Dictionary<String, String> GenQueryString()
        {
            Dictionary<String, String> queryDict = new Dictionary<string, string>
            {
                { "expr", "" },
                { "model", "" },
                { "count", "" },
                { "offset", "" },
                { "orderby", "" },
                { "attributes", "" }
            };
            return queryDict;
        }

        public static String FormatTitle(String oriTitle)
        {
            char[] resChar = new char[oriTitle.Length];
            char[] oriChar = oriTitle.ToLower().ToCharArray();
            for (int i = 0, j = 0; i < oriTitle.Length; i++)
            {
                if ((oriChar[i] <= 'z' && oriChar[i] >= 'a') || (oriChar[i] <= '9' && oriChar[i] >= '0'))
                {
                    resChar[j] = oriChar[i];
                    j++;
                }
                else if (resChar[j - 1] != ' ')
                {
                    resChar[j] = ' ';
                    j++;
                }
            }

            return new string(resChar).TrimEnd('\0');
        }

        public static JObject ParseAcademicJson(string jsonReturned)
        {
            JObject resJObj = new JObject();
            resJObj = JObject.Parse(jsonReturned);
            resJObj["entities"][0]["E"] = JObject.Parse(resJObj["entities"][0]["E"].Value<String>());
            return resJObj;
        }

        public static List<string> GetRefDoiList(Paper thePaper)
        {
            string jsonStr = AcademicAIEvalRequest("Ti=\'" + FormatTitle(thePaper.title) + "\'", "Id,Ti,Y,D,CC,RId,AA.AuN,AA.AuId,AA.AfId,AA.AfN,AA.S,F.FN,F.FId,J.JN,J.JId,W,E");
            var jsonRes = ParseAcademicJson(jsonStr);
            var Rid = jsonRes["entities"][0]["RId"];
            JObject refJson;
            List<string> refDoiList = new List<string>();
            int i = 0;

            foreach (var item in Rid)
            {
                refJson = ParseAcademicJson(AcademicAIEvalRequest(long.Parse(item.ToString()), @"Id,E"));
                try
                {
                    refDoiList.Add(refJson["entities"][0]["E"]["DOI"].ToString());
                }
                catch (NullReferenceException)
                {
                    continue;
                }
                Console.WriteLine(refJson["entities"][0]["E"]["DOI"].ToString());
                Thread.Sleep(500);
                i++;
                if (i > 3)
                {
                    break;
                }
            }

            return refDoiList;
        }

        public static Paper GetMSAcademic(Paper thePaper)
        {
            string jsonStr = AcademicAIEvalRequest("Ti=\'" + FormatTitle(thePaper.title) + "\'", "Id,Ti,Y,D,CC,RId,AA.AuN,AA.AuId,AA.AfId,AA.AfN,AA.S,F.FN,F.FId,J.JN,J.JId,W,E");
            var jsonRes = ParseAcademicJson(jsonStr);
            foreach (var item in jsonRes["entities"][0]["W"])
            {
                thePaper.Keywords.Add(item.ToString());
            }
            string jsonAbstract = "";
            try
            {
                foreach (var item in (JObject)jsonRes["entities"][0]["E"]["IA"]["InvertedIndex"])
                {
                    jsonAbstract += item.Key;
                    jsonAbstract += " ";
                }

            }
            catch (NullReferenceException)
            {
                ;
            }
            thePaper.Abstract = jsonAbstract;
            thePaper.refDoiList = GetRefDoiList(thePaper);
            return thePaper;
        }

        public static Paper GetPartMSAcademic(Paper thePaper)
        {
            string jsonStr = AcademicAIEvalRequest("Ti=\'" + FormatTitle(thePaper.title) + "\'", "Id,Ti,Y,D,CC,RId,AA.AuN,AA.AuId,AA.AfId,AA.AfN,AA.S,F.FN,F.FId,J.JN,J.JId,W,E");
            var jsonRes = ParseAcademicJson(jsonStr);
            foreach (var item in jsonRes["entities"][0]["W"])
            {
                thePaper.Keywords.Add(item.ToString());
            }
            string jsonAbstract = "";
            foreach (var item in (JObject)jsonRes["entities"][0]["E"]["IA"]["InvertedIndex"])
            {
                jsonAbstract += item.Key;
                jsonAbstract += " ";
            }
            thePaper.Abstract = jsonAbstract;
            return thePaper;
        }


        public static String AcademicAIEvalRequest(long msId, string attr, string count = "", string offset = "", string orderby = "", string model = "")
        {
            var queryStringDict = GenQueryString();

            // Request parameters
            queryStringDict["expr"] = "Id=" + msId.ToString();
            queryStringDict["model"] = model;
            queryStringDict["count"] = count;
            queryStringDict["offset"] = offset;
            queryStringDict["orderby"] = orderby;
            queryStringDict["attributes"] = attr;
            var uri = "https://westus.api.cognitive.microsoft.com/academic/v1.0/evaluate?" + QueryToString(queryStringDict, AcademicAIQueryType.Evaluate);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "GET";
            request.Headers.Add("Ocp-Apim-Subscription-Key: " + academicAISubKey);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader strRdr = new StreamReader(response.GetResponseStream());

            return strRdr.ReadToEnd();
        }

        public static String AcademicAIEvalRequest(string expr, string attr, string count = "", string offset = "", string orderby = "", string model = "")
        {
            var queryStringDict = GenQueryString();

            // Request parameters
            queryStringDict["expr"] = expr;
            queryStringDict["model"] = model;
            queryStringDict["count"] = count;
            queryStringDict["offset"] = offset;
            queryStringDict["orderby"] = orderby;
            queryStringDict["attributes"] = attr;
            var uri = "https://westus.api.cognitive.microsoft.com/academic/v1.0/evaluate?" + QueryToString(queryStringDict, AcademicAIQueryType.Evaluate);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "GET";
            request.Headers.Add("Ocp-Apim-Subscription-Key: " + academicAISubKey);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader strRdr = new StreamReader(response.GetResponseStream());

            return strRdr.ReadToEnd();
        }
    }

}
