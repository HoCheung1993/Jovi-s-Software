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

namespace FileCleaner_CSharp
{
    public partial class Form1 : Form
    {
        [DllImport("FileCleanerDll.dll")]
        public static extern void CreateCleanerDlg();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CreateCleanerDlg();
        }
    }
}
