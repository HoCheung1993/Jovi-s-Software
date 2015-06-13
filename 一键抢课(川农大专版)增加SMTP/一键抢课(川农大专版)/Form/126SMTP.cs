using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net.Mail;
using System.Text;
using System.Windows.Forms;
//邮箱 JoviSoftWare@126.com   hexiang1993
namespace 一键抢课_川农大专版_
{
    public partial class _126SMTP : Form
    {
        string EmailSubject = "匿名抢课软件留言信息";
        public _126SMTP()
        {
            InitializeComponent();
        }

        public _126SMTP(string info)
        {
            InitializeComponent();
            EmailSubject = info +"抢课软件留言信息";
        }  //带信息的邮件投递

        private void btn_OK_Click(object sender, EventArgs e)
        {
            SendEmail(EmailSubject, txtContent.Text);
            this.Close();
        }

        public static void SendEmail(string emailSubject,string emailContent)
        {
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();

            msg.To.Add("378967910@qq.com");
            //这个地方可以发送给多人，但是没有实现，可以用“，”分割，之后取得每个收件人的地址。
            //也可以抄送给多人。
            msg.Subject = emailSubject;
            msg.From = new MailAddress("JoviSoftWare@126.com");
            msg.SubjectEncoding = System.Text.Encoding.UTF8;//邮件标题编码
            msg.Body = emailContent;
            msg.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码
            msg.IsBodyHtml = false;
            msg.Priority = MailPriority.High;
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("JoviSoftWare@126.com", "hexiang1993");
            //client.Host = "localhost";//这个发布出去啊。应该是当做了垃圾邮件了。
            client.Host = "smtp.126.com";
            object userState = msg;
            if (msg.Body == "")
            {
                MessageBox.Show("您的留言不能为空哟！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                client.SendAsync(msg, userState);
                MessageBox.Show("提交成功！谢谢您的支持！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("提交出错！请检查网络连接！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
