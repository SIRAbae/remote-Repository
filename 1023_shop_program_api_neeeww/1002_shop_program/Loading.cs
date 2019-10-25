using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1002_shop_program
{
    
    public partial class Loading : Form
    {
        public Action Function { get; set; }

        private const int SC_CLOSE = 0xF060;
        private const int MF_ENABLED = 0x0;
        private const int MF_GRAYED = 0x1;
        private const int MF_DISABLED = 0x2;

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        private static extern int EnableMenuItem(IntPtr hMenu, int wIDEnableItem, int wEnable);

        public Loading()
        {
            InitializeComponent();
            this.Shown += new EventHandler(Form_Loaded);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            EnableMenuItem(GetSystemMenu(this.Handle, false), SC_CLOSE, MF_GRAYED);

        }

        private void Form_Loaded(object sender, EventArgs e)

        {
            

            var thread = new Thread(

                () =>

                {

                    Function?.Invoke();

                    this.Invoke(

                        (Action)(() =>

                        {

                            this.Close();

                        }));

                });

            thread.Start();

        }
    }
}
