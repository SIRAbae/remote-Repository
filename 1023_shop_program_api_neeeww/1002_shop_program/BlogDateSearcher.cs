using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace _1002_shop_program
{
    class BlogDateSearcher
    {
        public List<BlogDate> blogdateList = new List<BlogDate>();
        public String ImageName { get; private set; }
        public String XmlString { get; private set; }

        XmlDocument doc = null;

        public void SearchBlog(string str)
        {
            blogdateList.Clear();

            XmlString = Find(str);
            doc = new XmlDocument();
            doc.LoadXml(XmlString);
            //doc.Save("Search.xml");
            //===========================
            XmlNode node = doc.SelectSingleNode("rss");
            XmlNode n = node.SelectSingleNode("channel");
            BlogDate simage = null;

            foreach (XmlNode el in n.SelectNodes("item"))
            {
                simage = BlogDate.MakeBlogData(el);

                blogdateList.Add(simage);
            }

        }

        public string Find(string str)
        {
            string sort = string.Empty;

            string query = str + "&display=10";
            string url = "https://openapi.naver.com/v1/search/blog.xml?query=" + query;  // 결과가 XML 포맷
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("X-Naver-Client-Id", "DUpc4UQEaEldMKEEFis4"); // 클라이언트 아이디
            request.Headers.Add("X-Naver-Client-Secret", "XrbBj14B1E");       // 클라이언트 시크릿
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string status = response.StatusCode.ToString();
            if (status == "OK")
            {
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                string text = reader.ReadToEnd();
                return text;
            }
            else
            {
                return string.Format("Error발생={0}", status);
            }
        }
    }
}
