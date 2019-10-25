using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1002_shop_program
{
    public partial class Setting : Form
    {
        Form1 form;

        public bool MoneyMode { get; private set; }
        public int Money { get; private set; }
        public int Display { get; private set; }

        public Setting(Form1 form1)
        {
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            InitializeComponent();
            form = form1;
            MoneyMode = false;
            Money = 0;
            Display = 10;
            
        }

        private void Setting_Load(object sender, EventArgs e)
        {
            
            checkBox1.Checked = MoneyMode;
            textBox1.Text = Money.ToString();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (textBox1.ReadOnly == true)
                textBox1.ReadOnly = false;
            else
                textBox1.ReadOnly = true;     
        }

        //확인
        private void button2_Click(object sender, EventArgs e)
        {
            MoneyMode = checkBox1.Checked;
            Money = int.Parse(textBox1.Text);

            if (comboBox1.Text == "")
            {
                Display = 10;
            }
            else
                Display = int.Parse(comboBox1.Text);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
