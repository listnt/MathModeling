using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace лаба_1ПММ
{
    public partial class Form1 : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();



        public Form1()
        {
            InitializeComponent();
        }


        private void userControl11_Load(object sender, EventArgs e)
        {

        }


        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }

       
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            clicked_button.InOfClBu = 0;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            clicked_button.InOfClBu = 1;
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.L)
            {
                MessageBox.Show("hello");
            }
        }
    }
    class clicked_button
    {
        private static int  indexOfClickedButton=0;
        public static int InOfClBu
        {
            get
            {
                return indexOfClickedButton;
            }
            set
            {
                indexOfClickedButton = value;
            }
        }
    }
}
