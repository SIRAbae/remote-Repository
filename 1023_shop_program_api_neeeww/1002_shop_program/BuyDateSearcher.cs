using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace _1002_shop_program
{
    class BuyDateSearcher
    {
        public List<BuyDate> buydateList = new List<BuyDate>();
        public String ImageName { get; private set; }
        public String XmlString { get; private set; }

        XmlDocument doc = null;

        public void SearchBuy(string str, string sort, string start, string display)
        {
            buydateList.Clear();

            XmlString = Find(str, sort, start,display);
            doc = new XmlDocument();
            doc.LoadXml(XmlString);
            //doc.Save("Search.xml");
            //===========================
            XmlNode node = doc.SelectSingleNode("rss");
            XmlNode n = node.SelectSingleNode("channel");
            BuyDate simage = null;

            foreach (XmlNode el in n.SelectNodes("item"))
            {
                simage = BuyDate.MakeBuyData(el);
               
                buydateList.Add(simage);
            }

        }

        public string Find(string str, string sor, string start, string display)
        {
            string sort = string.Empty;

            if (sor == "낮은 최저가")
                sort += "&sort=asc";
            else if(sor == "높은 최저가")
                sort += "&sort=dsc";

            string query = str + "&display="+ display + " &start=" + start + sort; // 검색할 문자열
            //string url = "https://openapi.naver.com/v1/search/shop?query=" + query; // 결과가 JSON 포맷
            string url = "https://openapi.naver.com/v1/search/shop.xml?query=" + query;  // 결과가 XML 포맷
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
