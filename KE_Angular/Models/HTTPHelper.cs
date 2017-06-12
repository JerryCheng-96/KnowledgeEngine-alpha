using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace KE_Angular.Models
{
    class HTTPHelper
    {
        public static Stream HttpGetXml(string url, string param = "")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "?" + param);
            request.Method = "GET";
            request.ContentType = "text/xml;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            return response.GetResponseStream();
        }

        public static String WebPageParseMercury(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"https://mercury.postlight.com/parser?url=" + url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Headers.Add("x-api-key: kYev2U1p4O4SyuXDE7kJkYGviWEcE3LD4UMLbsXb");

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader strRdr = new StreamReader(response.GetResponseStream());

            return strRdr.ReadToEnd();
        }

        public static String HttpGetHtmlString(string url)
        {
            WebRequest request = WebRequest.Create(url);
            request.Timeout = 30000;
            request.Headers.Set("Pragma", "no-cache");
            WebResponse response = request.GetResponse();
            Stream streamReceive = response.GetResponseStream();
            Encoding encoding = Encoding.GetEncoding("GB2312");
            StreamReader streamReader = new StreamReader(streamReceive, encoding);
            String str = streamReader.ReadToEnd();
            streamReader.Close();

            return str;
        }
    }
}
