using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1002_shop_program
{
    public partial class shopingItem : UserControl
    {
        public string title;
        public TextBox num;
        public Form1 form;

        public shopingItem(Image img, string name, string price, string url, Form1 f)
        {
            InitializeComponent();
            pictureBox1.Image = img;
            label4.Text = name;
            label5.Text = price;
            linkLabel1.Text = url;

            num = this.textBox3;
            title = label4.Text;
            form = f;
        }

        //장바구니
        private void button2_Click(object sender, EventArgs e)
        {
            form.AddList(int.Parse(textBox3.Text));
        }
     
        //URL 연결
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            form.GoBrowser(linkLabel1.Text);
        }

        //키워드 검색
        private void button1_Click_1(object sender, EventArgs e)
        {
            form.SearchBrowser(title);
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            form.PrintSearchTxt(label4.Text);
        }
    }
}
