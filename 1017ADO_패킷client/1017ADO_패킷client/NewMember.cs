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
    public partial class NewMember : Form
    {

        public string ID { get;  set; }
        public string PW { get;  set; }
        public string NAME { get; private set; }
        public int AGE { get; private set; }
        public NewMember()
        {
            InitializeComponent();
        }

        #region 버튼 핸들러
       

       

        private void button1_Click_1(object sender, EventArgs e)
        {
            ID = textBox1.Text;
            PW = textBox2.Text;
            NAME = textBox3.Text;
            AGE = int.Parse(textBox4.Text);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
        #endregion
    }
}
