using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace _1002_shop_program
{
    class BlogDate
    {
        #region 프로퍼티=====================
        public string Title { get; private set; }
        public string Link { get; private set; }
        #endregion

        #region 파서=====================
        static public BlogDate MakeBlogData(XmlNode xn)
        {
            string title = string.Empty;
            string link = string.Empty;

            XmlNode title_node = xn.SelectSingleNode("title");
            title = ConvertString(title_node.InnerText);

            XmlNode link_node = xn.SelectSingleNode("link");
            link = ConvertString(link_node.InnerText);

            return new BlogDate(title, link);
        }
        static private string ConvertString(string str)
        {
            int sindex = 0;
            int eindex = 0;
            while (true)
            {
                sindex = str.IndexOf('<');
                if (sindex == -1)
                    break;
                eindex = str.IndexOf('>');
                str = str.Remove(sindex, eindex - sindex + 1);
            }
            return str;
        }
        #endregion

        public BlogDate(string t, string l)
        {
            Title = t;
            Link = l;
           
        }
    }
}
