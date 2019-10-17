using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1016_ADO_소켓_클라이언트
{
    public partial class Form1 : Form
    {
        Form2 IM = new Form2();

        WbClient client = new WbClient();

        Ui ui = new Ui();

        public Form1()
        {
            InitializeComponent();
            client.ParentInfo(this);
        }

        #region  소켓에서 보낸 정보
        public void LogMessage(string str)
        {
            str += "(" + DateTime.Now.ToString() + ")";
            Ui.LogPrint(listBox1, str);
        }

        public void ShortMessage(string str)
        {
            str += "(" + DateTime.Now.ToString() + ")";
            Ui.LogPrint(listBox2, str);
        }
        #endregion

        private void 프로그램종료ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 서버연결ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if (IM.ShowDialog() == DialogResult.OK)
            {
                //IM.IP;
                if (client.ClientRun(IM.IP, IM.Port) == true)
                {
                    Ui.FillDrawing(panel1, Color.Blue);
                    Ui.LabelState(label1, true);

                    String temp = String.Format("[서버실행]{0}:{1} 성공",
                        client.ClientIp, client.ClientPort);
                    Ui.LogPrint(listBox1, temp);
                }
            }
        }

        private void 서버연결해제ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ui.FillDrawing(panel1, Color.Red);
            Ui.LabelState(label1, false);

            String temp = String.Format("[서버종료]{0}:{1} 성공",
                        client.ClientIp, client.ClientPort);
            Ui.LogPrint(listBox1, temp);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            string msg = textBox2.Text;
            string temp = string.Format("[{0}] : {1}", name, msg);
            byte[] data = Encoding.Default.GetBytes(temp);

            client.SendData(data);


           // Ui.LogPrint(listBox2,temp);
        }
    
        public void RecvPrint(string data)
        {
            Ui.LogPrint(listBox2, data);
        }
    }
}
