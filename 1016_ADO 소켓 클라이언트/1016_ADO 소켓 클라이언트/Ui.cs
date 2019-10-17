using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1016_ADO_소켓_클라이언트
{
    class Ui
    {

        public static void FillDrawing(Control c, Color color)
        {
            c.BackColor = color;
        }

        public static void LabelState(Control c, bool b)
        {
            if(b)
            {
                c.Text = "서버 실행중...";
                c.BackColor = Color.Blue;
            }
            else
            {
                c.Text = "서버 정지....";
                c.BackColor = Color.Red;
            }
        }

        public static void LogPrint(ListBox c, string msg)
        {
            //msg = [연결].....성공
            msg += DateTime.Now.ToString();
            c.Items.Add(msg);
        }
    }
}
