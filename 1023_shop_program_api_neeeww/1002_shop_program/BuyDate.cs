using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace _1002_shop_program
{
    class BuyDate
    {
        #region 프로퍼티=====================
        public string Title { get; private set; }
        public string Link { get; private set; }
        public string Image { get; private set; }
        public int Lprice { get; private set; }
        public string Lprice_s { get; private set; }
        public int Hprice { get; private set; }
        public int ProductType { get; private set; }
        public int kind { get; private set; }
        #endregion

        #region 파서=====================
        static public BuyDate MakeBuyData(XmlNode xn)
        {
            string title = string.Empty;
            string link = string.Empty;
            string image = string.Empty;
            int lprice =0;
            int hprice =0;
            int productType =0;



            XmlNode title_node = xn.SelectSingleNode("title");
            title = ConvertString(title_node.InnerText);

            XmlNode link_node = xn.SelectSingleNode("link");
            link = ConvertString(link_node.InnerText);

            XmlNode imagel_node = xn.SelectSingleNode("image");
            image = ConvertString(imagel_node.InnerText);

            XmlNode lprice_node = xn.SelectSingleNode("lprice");
            lprice = int.Parse(ConvertString(lprice_node.InnerText));

            XmlNode hprice_node = xn.SelectSingleNode("hprice");
            hprice = int.Parse(ConvertString(hprice_node.InnerText));

            XmlNode productType_node = xn.SelectSingleNode("productType");
            productType = int.Parse(ConvertString(productType_node.InnerText));



            return new BuyDate(title,link,image,lprice,hprice,productType);
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

        public BuyDate(string t, string l, string i, int lp, int hp, int pt)
        {
            Title = t;
            Link = l;
            Image = i;
            Lprice = lp;
            Lprice_s = String.Format("{0:#,###}원", Lprice);
            Hprice = hp;
            ProductType = pt;
        }

        public BuyDate(string t, int lp, string l)
        {
            Title = t;
            Link = l;
            Lprice = lp;
        }
    }
}
