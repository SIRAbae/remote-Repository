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
    public partial class Form2 : Form
    {
        public string IP { get; private set; }
        public int Port { get; private set; }
        public Form2()
        {
            InitializeComponent();
        }

        #region 버튼 핸들러
        private void button1_Click(object sender, EventArgs e)
        {
            IP = textBox1.Text;
            Port = int.Parse(textBox2.Text);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
        #endregion
    }
}
