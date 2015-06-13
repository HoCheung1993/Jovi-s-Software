using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace 一键抢课_川农大专版_
{
    public partial class FrmPopUp : Form
    {
        private Point p;
        private string Info = "";//个人信息
        private int speed = 10;//弹出位移量
        public FrmPopUp(string Classname,string Info)
        {
            InitializeComponent();
            this.Classname.Text = Classname;
            this.Info = Info;
        }

        private void FrmPopUp_Load(object sender, EventArgs e)
        {
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width - this.Width, SystemInformation.WorkingArea.Height);
            p = new Point(Screen.PrimaryScreen.Bounds.Width - this.Width, SystemInformation.WorkingArea.Height - this.Height);
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            //此处进行窗口位移
            if (this.Location.Y - p.Y < speed)
                this.Location = new Point(Screen.PrimaryScreen.Bounds.Width - this.Width, SystemInformation.WorkingArea.Height - this.Height);  //位移修正
            if (p.Y < this.Location.Y)
                this.Location = new Point(Screen.PrimaryScreen.Bounds.Width - this.Width, this.Location.Y - speed);  //弹出位移
            else
                timer.Stop();
        }

        private void btn_email_Click(object sender, EventArgs e)
        {
            _126SMTP smtp;
            smtp = new _126SMTP(Info +" "+ Classname.Text);
            smtp.ShowDialog();
        }
    }
}
