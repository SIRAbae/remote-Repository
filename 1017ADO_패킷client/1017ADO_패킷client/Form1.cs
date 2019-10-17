using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1017ADO_패킷client
{
    public partial class Form1 : Form
    {
        Form2 IM = new Form2();
        NewMember newf = new NewMember();

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
            string msg1 = Packet.Intance.GetMessage(msg);

            client.Send(msg);

        }

        public void RecvPrint(string data)
        {
            Ui.LogPrint(listBox2, data);
        }

        private void 회원가입ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (newf.ShowDialog() == DialogResult.OK)
            {
                string msg = Packet.Intance.GetNewMember(newf.ID, newf.PW, newf.NAME, newf.AGE);
                client.Send(msg);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            newf.ID = textBox1.Text;
            string msg = Packet.Intance.GetLogout(newf.ID);
            client.Send(msg);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            newf.ID = textBox1.Text;
            string msg = Packet.Intance.GetDelMember(newf.ID);
            client.Send(msg);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            newf.ID = textBox1.Text;
            newf.PW = textBox2.Text;
            string msg = Packet.Intance.GetLogin(newf.ID, newf.PW);
            client.Send(msg);
        }
    }
}
