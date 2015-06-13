using System;
using System.Net;
using System.Windows.Forms;
using DotRas;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace 温江电信拨号客户端
{
    public partial class FrmMain : Form
    {
        public enum VersionInfo  //版本号信息枚举
        {
            win8 = 62,
            win81 = 63
        }
        private const string EntryName = "Ras_vpn";
        private const string IP = "1.1.5.127";
        public const int SW_SHOWNORMAL = 1;
        bool HasCeatedIPSec = false;

        public FrmMain()
        {
            InitializeComponent();
            MessageBox.Show("新版本解决Win8连接以后无Internet访问问题\n如有建议欢迎电邮作者：onlyperfectyou@vip.qq.com\n谢谢支持！", "欢迎使用", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Version ver = System.Environment.OSVersion.Version;  //获取版本信息，决定是否添加ProhibitIPSec
            bool Iswin8 = Enum.IsDefined(typeof(VersionInfo), (ver.Major * 10 + ver.Minor));  //检查版本是否为枚举版本
            if (Iswin8)
            {
                DialogResult result = MessageBox.Show("已检测您为Win8或以上系统,是否为您添加IPSec。", "系统检测", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                    btn_CreateIPSec_Click(this, new EventArgs());
            }
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void about_Click(object sender, EventArgs e)
        {
            FrmAbout about = new FrmAbout();
            about.ShowDialog();
        }

        private void btn_CreateRAS_Click(object sender, EventArgs e)
        {
            try
            {
                Userinfo userinfo = new Userinfo(tb_user.Text, tb_pwd.Text);
                this.AllUsersPhoneBook.Open();
                RasEntry entry = RasEntry.CreateVpnEntry(EntryName, IP, RasVpnStrategy.L2tpOnly,
                    RasDevice.GetDeviceByName("(PPTP)", RasDeviceType.Vpn));
                entry.Options.DoNotNegotiateMultilink = false;  //多链路
                entry.EncryptionType = RasEncryptionType.None;  //允许未加密密码
                entry.Options.RequireEncryptedPassword = false;
                entry.NetworkProtocols.IPv6 = false;  //取消IPV6服务
                entry.Options.CacheCredentials = true;  //win8以上记住密码
                DialogResult result = MessageBox.Show("你确定要创建账号为 " + userinfo.User + " 密码为 " + userinfo.Pwd + " 的拨号程序吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.No)
                    return;
                foreach (RasEntry temp in this.AllUsersPhoneBook.Entries)
                    if (temp.Name == entry.Name)
                    {
                        MessageBox.Show("创建失败，已经存在拨号连接 " + entry.Name, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                this.status.Text = "创建中...";
                this.AllUsersPhoneBook.Entries.Add(entry);
                if (tb_user.Text != "" && tb_pwd.Text != "")
                {
                    NetworkCredential credential = new NetworkCredential(tb_user.Text, tb_pwd.Text);
                    entry.UpdateCredentials(credential);
                }
                MessageBox.Show("创建成功！请点击右下角网络连接处的 " + entry.Name + " \n为您添加路由ing...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btn_hzly_Click(this, new EventArgs());
                this.status.Text = "创建成功！";
            }
            catch
            {
                MessageBox.Show("创建失败，请设定防火墙允许！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.GetCurrentProcess().Kill();
            }
        }

        private void help_Click(object sender, EventArgs e)
        {
            FrmHelp help = new FrmHelp();
            help.ShowDialog();
        }

        private void btn_hzly_Click(object sender, EventArgs e)
        {
            string result = Execute("for /f tokens^=2delims^=^\" %a in ('wmic Path Win32_NetworkAdapterConfiguration where \"IPEnabled='True'\" get DefaultIPGateway /value') do set \"gwip=%a\"&route add 10.0.0.0 mask 255.0.0.0 %a&route add 113.54.0.0 mask 255.255.240.0 %a&route add 202.115.176.0 mask 255.255.240.0 %a", 1);
            if (result.Contains("请求的操作需要提升"))
            {
                MessageBox.Show("添加回指路由失败!\n请设定防火墙允许！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.GetCurrentProcess().Kill();
            }
            if (result.Contains("route add"))
                MessageBox.Show("添加回指路由成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("添加回指路由失败，请检查是否接入川农内网！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static string Execute(string command, int seconds)
        {
            string output = ""; //输出字符串  
            if (command != null && !command.Equals(""))
            {
                Process process = new Process();//创建进程对象  
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "cmd.exe";//设定需要执行的命令  
                startInfo.Arguments = "/C " + command;//“/C”表示执行完命令后马上退出  
                startInfo.UseShellExecute = false;//不使用系统外壳程序启动  
                startInfo.RedirectStandardInput = false;//不重定向输入  
                startInfo.RedirectStandardOutput = true; //重定向输出  
                startInfo.CreateNoWindow = true;//不创建窗口  
                process.StartInfo = startInfo;
                try
                {
                    if (process.Start())//开始进程  
                    {
                        if (seconds == 0)
                        {
                            process.WaitForExit();//这里无限等待进程结束  
                        }
                        else
                        {
                            process.WaitForExit(seconds); //等待进程结束，等待时间为指定的毫秒  
                        }
                        output = process.StandardOutput.ReadToEnd();//读取进程的输出  
                    }
                }
                catch
                {
                }
                finally
                {
                    if (process != null)
                        process.Close();
                }
            }
            return output;
        }

        private void btn_CreateIPSec_Click(object sender, EventArgs e)
        {
            try
            {
                CreateIPSec IPSec = new CreateIPSec();
                HasCeatedIPSec = true;
                MessageBox.Show("添加IPSec成功,重启生效!\n如果连接成功但无Internet访问。\n请卸载红蝴蝶Supplicant helper！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("添加IPSec失败!\n请设定防火墙允许！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.GetCurrentProcess().Kill();
            }
        }

        private void btn_uSHelper_Click(object sender, EventArgs e)
        {
            try
            {
                u_SupplicantHelper u_SHelper = new u_SupplicantHelper();
                MessageBox.Show("即将启动驱动工具，请点击 卸载驱动 !", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ProcessStartInfo psi = new ProcessStartInfo();
                Process p = new Process();
                psi.WindowStyle = ProcessWindowStyle.Hidden;//设置隐藏
                psi.FileName = u_SHelper.Path + "Supplicant驱动修复工具.exe";
                p.StartInfo = psi;
                p.Start();
            }
            catch
            {
                MessageBox.Show("未检测到红蝴蝶,请安装最新版红蝴蝶或者手动启动Supplicant驱动修复工具！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Process.Start(@"http://ietc.sicau.edu.cn/?p=38&a=view&r=27"); //跳转下载页
            }
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (HasCeatedIPSec)
            {
                DialogResult result = MessageBox.Show("部分设置重启生效,是否重启？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                    Shutdown.Reboot();
            }
        }
    }
    class Userinfo
    {
        private string user = "";
        private string pwd = "";

        public string Pwd
        {
            get
            {
                if (pwd != "")
                    return pwd;
                else
                    return "空";
            }
        }

        public string User
        {
            get
            {
                if (user != "")
                    return user;
                else
                    return "空";
            }
        }

        public Userinfo(string user, string pwd)
        {
            this.user = user;
            this.pwd = pwd;
        }
    }
}
