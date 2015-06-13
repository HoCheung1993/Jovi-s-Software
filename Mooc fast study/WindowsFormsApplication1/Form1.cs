namespace WindowsFormsApplication1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.Security;
    using System.Windows.Forms;
    using System.Threading;

    public class Form1 : Form
    {
        public string[,] _c;
        public int _ERROR = 0;
        private bool _error_now = true;
        public string[,] _s;
        private bool _se_c_ok = false;
        private Label APass;
        private Button button1;
        private IContainer components = null;
        public CookieContainer Cookie = new CookieContainer();
        private ComboBox Courese_Iteml;
        private ListView Course_Data;
        private Label Course_Name;
        private Label Course_Num;
        private System.Windows.Forms.Timer ERROR;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private ColumnHeader ID;
        public bool IsLogin = false;
        private ColumnHeader IsOk;
        private Label Lab_Name;
        private Label Lab_School;
        private Label Lab_Tip;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label15;
        private Label label16;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Button Login;
        private Button M_Study;
        private Label NPass;
        private GroupBox Operate;
        private ColumnHeader S_ID;
        private Button Study_Now;
        private Label Sub_Num;
        private ColumnHeader Title;
        private TextBox Txtb_SchoolID;
        private TextBox Txtb_UserName;
        private TextBox Txtb_UserPwd;
        public string userName = "";
        private string UserName;
        private string UserPwd;
        private Button btn_fast_study;
        private BackgroundWorker backgroundWorker_faststudy;
        private string UserSchool;

        public Form1()
        {
            this.InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Designed by: SunGod \n Modified by Jovi ");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public static bool DowloadCheckImg(string Url, ref CookieContainer cookCon, string savePath)
        {
            bool flag = true;
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(Url);
            request.AllowWriteStreamBuffering = true;
            request.Credentials = CredentialCache.DefaultCredentials;
            request.MaximumResponseHeadersLength = -1;
            request.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Maxthon; .NET CLR 1.1.4322)";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "GET";
            request.Headers.Add("Accept-Language", "zh-cn");
            request.Headers.Add("Accept-Encoding", "gzip,deflate");
            request.KeepAlive = true;
            request.CookieContainer = cookCon;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        int num;
                        bool flag3;
                        List<byte> list = new List<byte>();
                        goto Label_00DC;
                    Label_00B7:
                        num = stream.ReadByte();
                        if (num == -1)
                        {
                            goto Label_00E1;
                        }
                        list.Add((byte) num);
                    Label_00DC:
                        flag3 = true;
                        goto Label_00B7;
                    Label_00E1:
                        System.IO.File.WriteAllBytes(savePath, list.ToArray());
                    }
                    return flag;
                }
            }
            catch (WebException)
            {
                flag = false;
            }
            catch (Exception)
            {
                flag = false;
            }
            return flag;
        }

        private void ERROR_Tick(object sender, EventArgs e)
        {
            if ((this._ERROR >= 20) && this._error_now)
            {
                this._error_now = !this._error_now;
                MessageBox.Show("操作错误太多，程序即将关闭~！");
                base.Close();
            }
        }

        private void Fast_Study_Click(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            base.Left = (Screen.PrimaryScreen.Bounds.Width - base.Width) / 2;
            base.Top = (Screen.PrimaryScreen.Bounds.Height - base.Height) / 2;
            if (!this.IsConnectedToInternet())
            {
                MessageBox.Show("很抱歉，你的网络不正常。\n程序即将关闭！");
                base.Close();
            }
            else
            {
                string url = "http://tieba.baidu.com/p/3731413155";
                if (this.SendDataByGET(url, "", ref this.Cookie).IndexOf("fjdu34321dfuh&") > 0)
                {
                    MessageBox.Show("很抱歉，服务已关闭。\n程序即将关闭！");
                    base.Close();
                }
            }
            MessageBox.Show("欢迎使用，本版本为一键学习增强版！","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private string[,] GetCourse()
        {
            string url = "http://mooc1.chaoxing.com/visit/courses";
            string str2 = this.SendDataByGET(url, "", ref this.Cookie);
            string[,] strArray = new string[10, 3];
            for (int i = 1; i <= this.GetCourseNum(); i++)
            {
                strArray[i, 0] = this.StrMid(str2, "<h3><a href=\"/mycourse/studentcourse?courseId=", "&clazzid");
                strArray[i, 1] = this.StrMid(str2, "<h3><a href=\"/mycourse/studentcourse?courseId=" + strArray[i, 0] + "&clazzid=", "\"  target=\"_blank\">");
                strArray[i, 2] = this.StrMid(str2, "<h3><a href=\"/mycourse/studentcourse?courseId=" + strArray[i, 0] + "&clazzid=" + strArray[i, 1] + "\"  target=\"_blank\">", "</a></h3>");
                str2 = this.StrMid(str2, "<h3><a href=\"/mycourse/studentcourse?courseId=" + strArray[i, 0] + "&clazzid=" + strArray[i, 1] + "\"  target=\"_blank\">", "</body>");
            }
            return strArray;
        }

        private int GetCourseAPassSubNum(string courseId, string clazzid)
        {
            string url = "http://mooc1.chaoxing.com/mycourse/studentcourse";
            string postDataStr = "courseId=" + courseId + "&clazzid=" + clazzid;
            string str = this.SendDataByGET(url, postDataStr, ref this.Cookie);
            string substring = "<span class=\"zadd_s_percent\">100%</span></span>";
            return SubstringCount(str, substring);
        }

        private int GetCourseNum()
        {
            string url = "http://mooc1.chaoxing.com/visit/courses";
            string str = this.SendDataByGET(url, "", ref this.Cookie);
            string substring = "<p class=\"Mconrightp1\">";
            return SubstringCount(str, substring);
        }

        private int GetCourseSubNum(string courseId, string clazzid)
        {
            string url = "http://mooc1.chaoxing.com/mycourse/studentcourse";
            string postDataStr = "courseId=" + courseId + "&clazzid=" + clazzid;
            string str = this.SendDataByGET(url, postDataStr, ref this.Cookie);
            string substring = "<h3 class=\"clearfix\">";
            return SubstringCount(str, substring);
        }

        private int getDuration(string objid)
        {
            string url = "http://ptr.chaoxing.com/ananas/status/" + objid;
            string str2 = this.SendDataByGET(url, "", ref this.Cookie);
            return Convert.ToInt32(this.StrMid(str2, "duration\":", ",\"filename"));
        }

        private void GetIsLogin(string BackSite)
        {
            this.label13.Text = "数据准备中...";
            this.Text = "[已登陆]超级超星系统 绿色\x00b7安全\x00b7健康\x00b7有效 的学习";
            this.Txtb_UserName.Enabled = false;
            this.Txtb_UserPwd.Enabled = false;
            this.Txtb_SchoolID.Enabled = false;
            this.Txtb_UserName.BackColor = Color.White;
            this.Txtb_UserPwd.BackColor = Color.White;
            this.Lab_Tip.Text = "登陆成功";
            this.Login.Text = "退出登录";
            this.IsLogin = true;
            this.groupBox2.Enabled = true;
            this.userName = this.StrMid(this.SendDataByGET("http://i.mooc.chaoxing.com/settings/info", "", ref this.Cookie), "update-fid?id=", "&enc=");
            this.Lab_Name.Text = BackSite.Substring(BackSite.IndexOf("text-overflow:ellipsis;\">") + 0x19, 20).Trim();
            Match match = new Regex(@"(?m)<title[^>]*>(?<title>(?:\w|\W)*?)</title[^>]*>", RegexOptions.Multiline | RegexOptions.IgnoreCase).Match(BackSite);
            if (match.Success)
            {
                this.Lab_School.Text = match.Groups["title"].Value.Trim();
            }
            this.Operate.Enabled = true;
            int courseNum = this.GetCourseNum();
            this.Course_Num.Text = courseNum.ToString();
            this._c = this.GetCourse();
            for (int i = 1; i <= courseNum; i++)
            {
                this.Courese_Iteml.Items.Add(this._c[i, 2]);
            }
            this.label13.Text = "数据准备完毕";
        }

        private void GetLogin(string UserName, string UserPwd, string UserSchool)
        {
            int index = 0;
            string backSite = "";
            this.Lab_Tip.Text = "Logining...";
            this.Login.Text = "Logining";
            string url = "http://passport2.chaoxing.com/login";
            string postDataStr = "pid=-1&pidName=&fid=" + UserSchool + "&fidName=&uname=" + UserName + "&password=" + UserPwd;
            backSite = this.SendDataByPost(url, postDataStr, ref this.Cookie);
            if (backSite.IndexOf("系统检测") > 0)
            {
                bool flag = false;
                do
                {
                    bool bflag = false;
                    DowloadCheckImg("http://passport2.chaoxing.com/img/code?" + GetTimeStamp(bflag), ref this.Cookie, @"D:\ver.png");
                    InputVer ver = new InputVer();
                    ver.ShowDialog();
                    string str5 = "http://passport2.chaoxing.com/pwd/do_check_vercode";
                    string var = ver.Var;
                    ver.Close();
                    if (var == "")
                    {
                        index = -2;
                        break;
                    }
                    string str7 = backSite.Substring(backSite.IndexOf("uid\" value=\"") + 12, backSite.Substring(backSite.IndexOf("uid\" value=\"") + 12).IndexOf("\">")).Trim();
                    string str8 = backSite.Substring(backSite.IndexOf("enc\" value=\"") + 12, backSite.Substring(backSite.IndexOf("enc\" value=\"") + 12).IndexOf("\">")).Trim();
                    string str9 = backSite.Substring(backSite.IndexOf("refer\" value=\"") + 14, backSite.Substring(backSite.IndexOf("refer\" value=\"") + 14).IndexOf("\"/>")).Trim();
                    string str10 = backSite.Substring(backSite.IndexOf("ip\" value=\"") + 11, backSite.Substring(backSite.IndexOf("ip\" value=\"") + 11).IndexOf("\">")).Trim();
                    string str11 = "uid=" + str7 + "&enc=" + str8 + "&refer=" + str9 + "&ip=" + str10 + "&vercode=" + var;
                    backSite = this.SendDataByPost(str5, str11, ref this.Cookie);
                    flag = backSite.IndexOf("验证码错误") > 0;
                }
                while (flag);
            }
            if (index != -2)
            {
                index = backSite.IndexOf("设置");
            }
            if (index > 0)
            {
                this.GetIsLogin(backSite);
            }
            else if (backSite.IndexOf("密码错误") > 0)
            {
                this.GetLogo();
                this.TipUserPwd("\x00d7检查密码");
                this._ERROR++;
            }
            else if (backSite.IndexOf("用户不存在") > 0)
            {
                this.GetLogo();
                this.TipUserName("\x00d7检查用户名");
                this._ERROR++;
            }
            switch (index)
            {
                case 1:
                    this.Lab_Tip.Text = "服务器错误";
                    this._ERROR++;
                    break;

                case -2:
                    this.Login.Text = "重新登录";
                    this.Lab_Tip.Text = "登陆失败";
                    break;
            }
        }

        private void GetLogo()
        {
            this.Txtb_UserName.Enabled = true;
            this.Txtb_UserPwd.Enabled = true;
            this.Txtb_SchoolID.Enabled = true;
            this.Login.Text = "尝试登录";
            this.Lab_Tip.Text = "退出登录";
            this.IsLogin = false;
            this.Operate.Enabled = false;
            this.groupBox2.Enabled = false;
            this.Text = "[未登陆]超级超星系统 绿色\x00b7安全\x00b7健康\x00b7有效 的学习";
            this.Course_Num.Text = "[Course_Num]";
            this.Lab_Name.Text = "[Name]";
            this.Lab_School.Text = "[SchoolName]";
            this.Cookie = new CookieContainer();
            this.Courese_Iteml.Items.Clear();
        }

        private string GetMd5(string strPwd)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(strPwd, "MD5").ToLower();
        }

        private string GetPwdWord(string classId, string userid, string jobid, string objectId, int playtime)
        {
            string str2 = "[" + classId + "][" + userid + "][" + jobid + "][" + objectId + "][";
            string str3 = (playtime * 0x3e8).ToString();
            string str4 = "][d_yHJ!$pdA~5]";
            return this.GetMd5(str2 + str3 + str4);
        }

        private string[,] GetSubCourse(string courseId, string clazzid)
        {
            string url = "http://mooc1.chaoxing.com/mycourse/studentcourse";
            string postDataStr = "courseId=" + courseId + "&clazzid=" + clazzid + "&ut=s";
            string str3 = this.SendDataByGET(url, postDataStr, ref this.Cookie);
            str3 = this.StrMid(str3, "<body>", "</body>");
            string[,] strArray = new string[100, 3];
            for (int i = 1; i <= Convert.ToInt32(this.Sub_Num.Text); i++)
            {
                strArray[i, 0] = this.StrMid(str3, "<h3 class=\"clearfix\"><a class=\"zadd_schedule\" target=\"_blank\" href=\"/moocAnalysis/nodeStatisticByUser?flag=1&courseId=" + courseId + "&classId=" + clazzid + "&chapterId=", "&chapterName");
                strArray[i, 1] = this.StrMid(str3, "<h3 class=\"clearfix\"><a class=\"zadd_schedule\" target=\"_blank\" href=\"/moocAnalysis/nodeStatisticByUser?flag=1&courseId=" + courseId + "&classId=" + clazzid + "&chapterId=" + strArray[i, 0] + "&chapterName=", "&totalStudent");
                str3 = str3.Substring(str3.IndexOf(strArray[i, 0]));
                strArray[i, 2] = this.StrMid(str3, "<span class=\"zadd_s_percent\">", "</span></span>");
            }
            return strArray;
        }

        public static string GetTimeStamp(bool bflag = true)
        {
            TimeSpan span = (TimeSpan) (DateTime.UtcNow - new DateTime(0x7b2, 1, 1, 8, 0, 0, 0));
            if (bflag)
            {
                return Convert.ToInt64(span.TotalSeconds).ToString();
            }
            return Convert.ToInt64(span.TotalMilliseconds).ToString();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Lab_Tip = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Login = new System.Windows.Forms.Button();
            this.Txtb_SchoolID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Txtb_UserPwd = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Txtb_UserName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Operate = new System.Windows.Forms.GroupBox();
            this.btn_fast_study = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.M_Study = new System.Windows.Forms.Button();
            this.Course_Data = new System.Windows.Forms.ListView();
            this.ID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.S_ID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Title = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.IsOk = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label13 = new System.Windows.Forms.Label();
            this.APass = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.NPass = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.Sub_Num = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.Courese_Iteml = new System.Windows.Forms.ComboBox();
            this.Course_Name = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.Study_Now = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Course_Num = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Lab_School = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.Lab_Name = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ERROR = new System.Windows.Forms.Timer(this.components);
            this.label16 = new System.Windows.Forms.Label();
            this.backgroundWorker_faststudy = new System.ComponentModel.BackgroundWorker();
            this.groupBox1.SuspendLayout();
            this.Operate.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Lab_Tip);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.Login);
            this.groupBox1.Controls.Add(this.Txtb_SchoolID);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.Txtb_UserPwd);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.Txtb_UserName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(651, 56);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "登录到系统";
            // 
            // Lab_Tip
            // 
            this.Lab_Tip.AutoSize = true;
            this.Lab_Tip.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Lab_Tip.ForeColor = System.Drawing.Color.DarkViolet;
            this.Lab_Tip.Location = new System.Drawing.Point(479, 25);
            this.Lab_Tip.Name = "Lab_Tip";
            this.Lab_Tip.Size = new System.Drawing.Size(17, 12);
            this.Lab_Tip.TabIndex = 8;
            this.Lab_Tip.Text = "  ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(488, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 12);
            this.label4.TabIndex = 7;
            // 
            // Login
            // 
            this.Login.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Login.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Login.Location = new System.Drawing.Point(572, 19);
            this.Login.Name = "Login";
            this.Login.Size = new System.Drawing.Size(75, 23);
            this.Login.TabIndex = 6;
            this.Login.Text = "尝试登陆";
            this.Login.UseVisualStyleBackColor = true;
            this.Login.Click += new System.EventHandler(this.Login_Click);
            // 
            // Txtb_SchoolID
            // 
            this.Txtb_SchoolID.ForeColor = System.Drawing.Color.Red;
            this.Txtb_SchoolID.Location = new System.Drawing.Point(383, 21);
            this.Txtb_SchoolID.Name = "Txtb_SchoolID";
            this.Txtb_SchoolID.Size = new System.Drawing.Size(90, 21);
            this.Txtb_SchoolID.TabIndex = 5;
            this.Txtb_SchoolID.Text = "2142";
            this.Txtb_SchoolID.TextChanged += new System.EventHandler(this.Txtb_SchoolID_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Cursor = System.Windows.Forms.Cursors.Help;
            this.label3.Location = new System.Drawing.Point(323, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "学校代码";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // Txtb_UserPwd
            // 
            this.Txtb_UserPwd.ForeColor = System.Drawing.Color.Red;
            this.Txtb_UserPwd.Location = new System.Drawing.Point(218, 20);
            this.Txtb_UserPwd.Name = "Txtb_UserPwd";
            this.Txtb_UserPwd.PasswordChar = '*';
            this.Txtb_UserPwd.Size = new System.Drawing.Size(90, 21);
            this.Txtb_UserPwd.TabIndex = 3;
            this.Txtb_UserPwd.Tag = "";
            this.Txtb_UserPwd.TextChanged += new System.EventHandler(this.Txtb_UserPwd_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(183, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "密码";
            // 
            // Txtb_UserName
            // 
            this.Txtb_UserName.BackColor = System.Drawing.SystemColors.Window;
            this.Txtb_UserName.ForeColor = System.Drawing.Color.Red;
            this.Txtb_UserName.Location = new System.Drawing.Point(77, 21);
            this.Txtb_UserName.Name = "Txtb_UserName";
            this.Txtb_UserName.Size = new System.Drawing.Size(90, 21);
            this.Txtb_UserName.TabIndex = 1;
            this.Txtb_UserName.TextChanged += new System.EventHandler(this.Txtb_UserName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "学号/账号";
            // 
            // Operate
            // 
            this.Operate.Controls.Add(this.btn_fast_study);
            this.Operate.Controls.Add(this.button1);
            this.Operate.Controls.Add(this.label15);
            this.Operate.Controls.Add(this.M_Study);
            this.Operate.Controls.Add(this.Course_Data);
            this.Operate.Controls.Add(this.label13);
            this.Operate.Controls.Add(this.APass);
            this.Operate.Controls.Add(this.label12);
            this.Operate.Controls.Add(this.NPass);
            this.Operate.Controls.Add(this.label11);
            this.Operate.Controls.Add(this.Sub_Num);
            this.Operate.Controls.Add(this.label10);
            this.Operate.Controls.Add(this.Courese_Iteml);
            this.Operate.Controls.Add(this.Course_Name);
            this.Operate.Controls.Add(this.label9);
            this.Operate.Controls.Add(this.Study_Now);
            this.Operate.Controls.Add(this.label8);
            this.Operate.Enabled = false;
            this.Operate.Location = new System.Drawing.Point(3, 141);
            this.Operate.Name = "Operate";
            this.Operate.Size = new System.Drawing.Size(651, 327);
            this.Operate.TabIndex = 1;
            this.Operate.TabStop = false;
            this.Operate.Text = "开始认真的学习";
            // 
            // btn_fast_study
            // 
            this.btn_fast_study.Enabled = false;
            this.btn_fast_study.Location = new System.Drawing.Point(544, 207);
            this.btn_fast_study.Name = "btn_fast_study";
            this.btn_fast_study.Size = new System.Drawing.Size(100, 36);
            this.btn_fast_study.TabIndex = 19;
            this.btn_fast_study.Text = "一键学习";
            this.btn_fast_study.UseVisualStyleBackColor = true;
            this.btn_fast_study.Click += new System.EventHandler(this.btn_fast_study_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(557, 298);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 18;
            this.button1.Text = "关于";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(537, 85);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(107, 12);
            this.label15.TabIndex = 17;
            this.label15.Text = " 选择一个ID来学习";
            // 
            // M_Study
            // 
            this.M_Study.Enabled = false;
            this.M_Study.Location = new System.Drawing.Point(544, 100);
            this.M_Study.Name = "M_Study";
            this.M_Study.Size = new System.Drawing.Size(100, 36);
            this.M_Study.TabIndex = 16;
            this.M_Study.Text = "标准学习";
            this.M_Study.UseVisualStyleBackColor = true;
            this.M_Study.Click += new System.EventHandler(this.M_Study_Click);
            // 
            // Course_Data
            // 
            this.Course_Data.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ID,
            this.S_ID,
            this.Title,
            this.IsOk});
            this.Course_Data.FullRowSelect = true;
            this.Course_Data.Location = new System.Drawing.Point(8, 85);
            this.Course_Data.MultiSelect = false;
            this.Course_Data.Name = "Course_Data";
            this.Course_Data.Size = new System.Drawing.Size(515, 236);
            this.Course_Data.TabIndex = 14;
            this.Course_Data.UseCompatibleStateImageBehavior = false;
            this.Course_Data.View = System.Windows.Forms.View.Details;
            // 
            // ID
            // 
            this.ID.Text = "ID";
            // 
            // S_ID
            // 
            this.S_ID.Text = "小节ID";
            this.S_ID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.S_ID.Width = 65;
            // 
            // Title
            // 
            this.Title.Text = "标题";
            this.Title.Width = 290;
            // 
            // IsOk
            // 
            this.IsOk.Text = "状态";
            this.IsOk.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.IsOk.Width = 70;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.DarkViolet;
            this.label13.Location = new System.Drawing.Point(346, 26);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 13;
            this.label13.Text = "准备就绪";
            // 
            // APass
            // 
            this.APass.AutoSize = true;
            this.APass.ForeColor = System.Drawing.Color.Green;
            this.APass.Location = new System.Drawing.Point(465, 61);
            this.APass.Name = "APass";
            this.APass.Size = new System.Drawing.Size(47, 12);
            this.APass.TabIndex = 11;
            this.APass.Text = "[APass]";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(424, 61);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 12);
            this.label12.TabIndex = 10;
            this.label12.Text = "已通过";
            // 
            // NPass
            // 
            this.NPass.AutoSize = true;
            this.NPass.ForeColor = System.Drawing.Color.Red;
            this.NPass.Location = new System.Drawing.Point(346, 61);
            this.NPass.Name = "NPass";
            this.NPass.Size = new System.Drawing.Size(47, 12);
            this.NPass.TabIndex = 9;
            this.NPass.Text = "[NPass]";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(306, 61);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 12);
            this.label11.TabIndex = 8;
            this.label11.Text = "未通过";
            // 
            // Sub_Num
            // 
            this.Sub_Num.AutoSize = true;
            this.Sub_Num.ForeColor = System.Drawing.Color.Red;
            this.Sub_Num.Location = new System.Drawing.Point(221, 61);
            this.Sub_Num.Name = "Sub_Num";
            this.Sub_Num.Size = new System.Drawing.Size(59, 12);
            this.Sub_Num.TabIndex = 7;
            this.Sub_Num.Text = "[Sub_Num]";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(180, 61);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 12);
            this.label10.TabIndex = 6;
            this.label10.Text = "小节数";
            // 
            // Courese_Iteml
            // 
            this.Courese_Iteml.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Courese_Iteml.FormattingEnabled = true;
            this.Courese_Iteml.Location = new System.Drawing.Point(149, 23);
            this.Courese_Iteml.Name = "Courese_Iteml";
            this.Courese_Iteml.Size = new System.Drawing.Size(110, 20);
            this.Courese_Iteml.TabIndex = 5;
            // 
            // Course_Name
            // 
            this.Course_Name.AutoSize = true;
            this.Course_Name.ForeColor = System.Drawing.Color.Red;
            this.Course_Name.Location = new System.Drawing.Point(47, 61);
            this.Course_Name.Name = "Course_Name";
            this.Course_Name.Size = new System.Drawing.Size(83, 12);
            this.Course_Name.TabIndex = 4;
            this.Course_Name.Text = "[Course_Name]";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 61);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 3;
            this.label9.Text = "课程名";
            // 
            // Study_Now
            // 
            this.Study_Now.Location = new System.Drawing.Point(265, 21);
            this.Study_Now.Name = "Study_Now";
            this.Study_Now.Size = new System.Drawing.Size(75, 23);
            this.Study_Now.TabIndex = 2;
            this.Study_Now.Text = "准备学习";
            this.Study_Now.UseVisualStyleBackColor = true;
            this.Study_Now.Click += new System.EventHandler(this.Study_Now_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 26);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(137, 12);
            this.label8.TabIndex = 1;
            this.label8.Text = "请先从这里选择一个课程";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Course_Num);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.Lab_School);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.Lab_Name);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new System.Drawing.Point(3, 74);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(651, 52);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "学生信息";
            // 
            // Course_Num
            // 
            this.Course_Num.AutoSize = true;
            this.Course_Num.ForeColor = System.Drawing.Color.Red;
            this.Course_Num.Location = new System.Drawing.Point(435, 23);
            this.Course_Num.Name = "Course_Num";
            this.Course_Num.Size = new System.Drawing.Size(77, 12);
            this.Course_Num.TabIndex = 5;
            this.Course_Num.Text = "[Course_Num]";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(383, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 4;
            this.label6.Text = "已选课程";
            // 
            // Lab_School
            // 
            this.Lab_School.AutoSize = true;
            this.Lab_School.ForeColor = System.Drawing.Color.Red;
            this.Lab_School.Location = new System.Drawing.Point(131, 23);
            this.Lab_School.Name = "Lab_School";
            this.Lab_School.Size = new System.Drawing.Size(77, 12);
            this.Lab_School.TabIndex = 3;
            this.Lab_School.Text = "[SchoolName]";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(103, 23);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 2;
            this.label7.Text = "高校";
            // 
            // Lab_Name
            // 
            this.Lab_Name.AutoSize = true;
            this.Lab_Name.ForeColor = System.Drawing.Color.Red;
            this.Lab_Name.Location = new System.Drawing.Point(40, 23);
            this.Lab_Name.Name = "Lab_Name";
            this.Lab_Name.Size = new System.Drawing.Size(41, 12);
            this.Lab_Name.TabIndex = 1;
            this.Lab_Name.Text = "[Name]";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "姓名";
            // 
            // ERROR
            // 
            this.ERROR.Enabled = true;
            this.ERROR.Interval = 1000;
            this.ERROR.Tick += new System.EventHandler(this.ERROR_Tick);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.ForeColor = System.Drawing.Color.Red;
            this.label16.Location = new System.Drawing.Point(12, 471);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(269, 12);
            this.label16.TabIndex = 18;
            this.label16.Text = "提示：合理的时间间隔进行点击，成功率会增大。";
            // 
            // backgroundWorker_faststudy
            // 
            this.backgroundWorker_faststudy.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_faststudy_DoWork);
            this.backgroundWorker_faststudy.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_faststudy_RunWorkerCompleted);
            // 
            // Form1
            // 
            this.AcceptButton = this.Login;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(661, 488);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.Operate);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "[未登陆]超级超星系统 绿色·安全·健康·有效 的学习";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.Operate.ResumeLayout(false);
            this.Operate.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private bool IsConnectedToInternet()
        {
            Ping ping = new Ping();
            PingOptions options = new PingOptions {
                DontFragment = true
            };
            string s = string.Empty;
            byte[] bytes = Encoding.ASCII.GetBytes(s);
            int timeout = 0x4b0;
            try
            {
                return (ping.Send(IPAddress.Parse("220.181.57.217"), timeout, bytes, options).Status == IPStatus.Success);
            }
            catch
            {
                return false;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("What is the school code? I can't tell you! ");
        }

        private void Login_Click(object sender, EventArgs e)
        {
            this.UserName = this.Txtb_UserName.Text;
            this.UserPwd = this.Txtb_UserPwd.Text;
            this.UserSchool = this.Txtb_SchoolID.Text;
            if (!this.IsLogin)
            {
                if (this.UserName == "")
                {
                    this.TipUserName("\x00d7检查用户名");
                    this._ERROR++;
                }
                else if (this.UserPwd == "")
                {
                    this.TipUserPwd("\x00d7检查密码");
                    this._ERROR++;
                }
                else if (this.UserSchool == "")
                {
                    this.TipUserSchool("\x00d7检查学校代码");
                    this._ERROR++;
                }
                else
                {
                    this.GetLogin(this.UserName, this.UserPwd, this.UserSchool);
                }
            }
            else
            {
                this.GetLogo();
            }
        }

        private void M_Study_Click(object sender, EventArgs e)
        {
            string chapterId = "";
            try
            {
                chapterId = this.Course_Data.SelectedItems[0].SubItems[1].Text;
            }
            catch
            {
            }
            if (chapterId == "")
            {
                MessageBox.Show("你什么都没选！");
                this._ERROR++;
            }
            else
            {
                this.Course_Name.Text = this.Courese_Iteml.SelectedItem.ToString();
                int num = 1;
                string courseId = "";
                string clazzid = "";
                while (true)
                {
                    if (this._c[num, 2] == this.Course_Name.Text)
                    {
                        courseId = this._c[num, 0];
                        clazzid = this._c[num, 1];
                        try
                        {
                            if (this.passAChapter(courseId, clazzid, chapterId))
                            {
                                MessageBox.Show("视频[" + this.Course_Data.SelectedItems[0].SubItems[2].Text + "]学习完成！");
                            }
                            else
                            {
                                MessageBox.Show("系统错误，学习失败！");
                            }
                        }
                        catch
                        {
                            MessageBox.Show("系统错误，学习失败！");
                        }                       
                        return;
                    }
                }
            }
        }

        private bool passAChapter(string courseId = "80031966", string clazzid = "171519", string chapterId = "845134")
        {
            bool bflag = false;
            string timeStamp = GetTimeStamp(bflag);
            bool flag2 = false;
            string url = "http://mooc.chaoxing.com/knowledge/cards";
            string postDataStr = "clazzid=" + clazzid + "&courseid=" + courseId + "&knowledgeid=" + chapterId + "&num=0&v=20140815";
            string str4 = this.SendDataByGET(url, postDataStr, ref this.Cookie);
            string str5 = this.StrMid(str4, "try{", "};");
            string jobid = this.StrMid(str5, "jobid\":\"", "\",\"objectId");
            string objid = this.StrMid(str5, jobid + "\",\"objectId\":\"", "\"");
            string str8 = this.StrMid(str5, "otherInfo\":\"", "\",\"property");
            int num = this.getDuration(objid);
            string str9 = this.GetPwdWord(clazzid, this.userName, jobid, objid, num - 1);
            this.SendDataByGET("http://mooc1.chaoxing.com/mycourse/studentstudy", "chapterId=" + chapterId + "&courseId=" + courseId + "&clazzid=" + clazzid, ref this.Cookie);
            string str10 = "http://mooc1.chaoxing.com/multimedia/log";
            object[] objArray = new object[] { 
                "otherInfo=nodeId_", chapterId, "&clipTime=0_", num, "&jobid=", jobid, "&rt=0.9&clazzId=", clazzid, "&dtype=Video&duration=", num.ToString(), "&userid=", this.userName, "&objectId=", objid, "&view=pc&playingTime=", (num - 1).ToString(), 
                "&isdrag=3&enc=", str9
             };
            string str11 = string.Concat(objArray);
            if (this.StudyGET(str10, str11, ref this.Cookie).IndexOf("true") > 0)
            {
                flag2 = true;
            }
            return flag2;
        }

        public string SendDataByGET(string Url, string postDataStr, ref CookieContainer cookie)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(Url + ((postDataStr == "") ? "" : "?") + postDataStr);
            if (cookie.Count == 0)
            {
                request.CookieContainer = new CookieContainer();
                cookie = request.CookieContainer;
            }
            else
            {
                request.CookieContainer = cookie;
            }
            string str = "";
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            try
            {
                Stream responseStream = ((HttpWebResponse) request.GetResponse()).GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                str = reader.ReadToEnd();
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                foreach (System.Net.Cookie cookie2 in response.Cookies)
                {
                    this.Cookie.Add(cookie2);
                }
                reader.Close();
                responseStream.Close();
            }
            catch
            {
                str = "";
            }
            return str;
        }

        public string SendDataByPost(string Url, string postDataStr, ref CookieContainer cookie)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(Url);
            if (cookie.Count == 0)
            {
                request.CookieContainer = new CookieContainer();
                cookie = request.CookieContainer;
            }
            else
            {
                request.CookieContainer = cookie;
            }
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postDataStr.Length;
            StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.GetEncoding("gb2312"));
            writer.Write(postDataStr);
            writer.Close();
            Stream responseStream = ((HttpWebResponse) request.GetResponse()).GetResponseStream();
            StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
            string str = reader.ReadToEnd();
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            foreach (System.Net.Cookie cookie2 in response.Cookies)
            {
                this.Cookie.Add(cookie2);
            }
            response.Close();
            reader.Close();
            responseStream.Close();
            return str;
        }

        private void SetN(string _str = "准备数据中...")
        {
            this.label13.Text = _str;
        }

        private string StrMid(string _str, string start, string end)
        {
            string str = "";
            int startIndex = _str.IndexOf(start) + start.Length;
            int index = _str.Substring(startIndex).IndexOf(end);
            if ((index >= 0) && (startIndex >= 0))
            {
                str = _str.Substring(startIndex, index).Trim();
            }
            return str;
        }

        private void Study_Now_Click(object sender, EventArgs e)
        {
            this.SetN("准备数据中...");
            if (this._se_c_ok)
            {
                this.Study_Now.Text = "准备学习";
                this.label13.Text = "就绪";
                this.Course_Name.Text = "[Course_Name]";
                this.Courese_Iteml.Enabled = true;
                this.Sub_Num.Text = "[Sub_Num]";
                this.APass.Text = "[APass]";
                this.NPass.Text = "[NPass]";
                this._se_c_ok = !this._se_c_ok;
                this.Login.Enabled = true;
                this.Course_Data.Items.Clear();
                this.M_Study.Enabled = false;
                this.btn_fast_study.Enabled = false;
            }
            else
            {
                bool flag = false;
                try
                {
                    this.Course_Name.Text = this.Courese_Iteml.SelectedItem.ToString();
                    this.Courese_Iteml.Enabled = false;
                    flag = !flag;
                }
                catch
                {
                    MessageBox.Show("你什么都没选！！！");
                    this._ERROR++;
                }
                if (flag)
                {
                    this._se_c_ok = !this._se_c_ok;
                    this.Study_Now.Text = "准备数据中...";
                    int num = 1;
                    string courseId = "";
                    string clazzid = "";
                    while (true)
                    {
                        if (this._c[num, 2] == this.Course_Name.Text)
                        {
                            courseId = this._c[num, 0];
                            clazzid = this._c[num, 1];
                            this.Sub_Num.Text = this.GetCourseSubNum(courseId, clazzid).ToString();
                            this.APass.Text = this.GetCourseAPassSubNum(courseId, clazzid).ToString();
                            this.NPass.Text = (Convert.ToInt32(this.Sub_Num.Text) - Convert.ToInt32(this.APass.Text)).ToString();
                            string[,] subCourse = this.GetSubCourse(courseId, clazzid);
                            this.Course_Data.BeginUpdate();
                            for (int i = 1; i <= Convert.ToInt32(this.Sub_Num.Text); i++)
                            {
                                ListViewItem item = new ListViewItem {
                                    Text = i.ToString()
                                };
                                item.SubItems.Add(subCourse[i, 0]);
                                item.SubItems.Add(WebUtility.UrlDecode(subCourse[i, 1]));
                                item.SubItems.Add(subCourse[i, 2]);
                                this.Course_Data.Items.Add(item);
                            }
                            this.Course_Data.EndUpdate();
                            this.SetN("数据准备完毕ok");
                            this.Study_Now.Text = "取消选择";
                            this.Login.Enabled = false;
                            this.M_Study.Enabled = true;
                            this.btn_fast_study.Enabled = true;
                            return;
                        }
                    }
                }
            }
        }

        public string StudyGET(string Url, string postDataStr, ref CookieContainer cookie)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(Url + ((postDataStr == "") ? "" : "?") + postDataStr);
            if (cookie.Count == 0)
            {
                request.CookieContainer = new CookieContainer();
                cookie = request.CookieContainer;
            }
            else
            {
                request.CookieContainer = cookie;
            }
            string str = "";
            request.Method = "GET";
            request.Referer = "http://mooc1.chaoxing.com/ananas/modules/video/index.html?v=20150402";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.90 Safari/537.36";
            try
            {
                Stream responseStream = ((HttpWebResponse) request.GetResponse()).GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                str = reader.ReadToEnd();
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                foreach (System.Net.Cookie cookie2 in response.Cookies)
                {
                    this.Cookie.Add(cookie2);
                }
                reader.Close();
                responseStream.Close();
            }
            catch
            {
                str = "";
            }
            return str;
        }

        private static int SubstringCount(string str, string substring)
        {
            int num = 0;
            if (str.Contains(substring))
            {
                string str2 = str.Replace(substring, "");
                num = (str.Length - str2.Length) / substring.Length;
            }
            return num;
        }

        private void TipUserName(string _msg = "\x00d7检查用户名")
        {
            this.Txtb_UserName.BackColor = Color.Red;
            this.Lab_Tip.Text = _msg;
        }

        private void TipUserPwd(string _msg = "\x00d7检查密码")
        {
            this.Txtb_UserPwd.BackColor = Color.Red;
            this.Lab_Tip.Text = _msg;
        }

        private void TipUserSchool(string _msg = "\x00d7检查学校代码")
        {
            this.Txtb_SchoolID.BackColor = Color.Red;
            this.Lab_Tip.Text = _msg;
        }

        private void Txtb_SchoolID_TextChanged(object sender, EventArgs e)
        {
            this.Txtb_SchoolID.BackColor = Color.White;
        }

        private void Txtb_UserName_TextChanged(object sender, EventArgs e)
        {
            this.Txtb_UserName.BackColor = Color.White;
        }

        private void Txtb_UserPwd_TextChanged(object sender, EventArgs e)
        {
            this.Txtb_UserPwd.BackColor = Color.White;
        }

        private void btn_fast_study_Click(object sender, EventArgs e)
        {
            if(backgroundWorker_faststudy.IsBusy)
            {
                MessageBox.Show("正在一键学习中！");
                return;
            }
            backgroundWorker_faststudy.RunWorkerAsync();
        }

        private void backgroundWorker_faststudy_DoWork(object sender, DoWorkEventArgs e)
        {
            Course_Data.HideSelection = false;
            for (int i = 0; i < Course_Data.Items.Count; i++)
            {
                if (Course_Data.Items[i].SubItems[3].Text != "")
                {
                    continue;
                }
                Course_Data.Items[i].Selected = true;
                string chapterId = "";
                try
                {
                    chapterId = this.Course_Data.Items[i].SubItems[1].Text;
                }
                catch
                {
                }
                if (chapterId == "")
                {
                    MessageBox.Show("你什么都没选！");
                    this._ERROR++;
                }
                else
                {
                    this.Course_Name.Text = this.Courese_Iteml.SelectedItem.ToString();
                    int num = 1;
                    string courseId = "";
                    string clazzid = "";
                    while (true)
                    {
                        if (this._c[num, 2] == this.Course_Name.Text)
                        {
                            courseId = this._c[num, 0];
                            clazzid = this._c[num, 1];
                            try
                            {
                                if (!this.passAChapter(courseId, clazzid, chapterId))
                                {
                                    MessageBox.Show("视频[" + this.Course_Data.Items[i].SubItems[2].Text + "]学习失败！");
                                }
                            }
                            catch
                            {
                                MessageBox.Show("视频[" + this.Course_Data.Items[i].SubItems[2].Text + "]学习失败！");
                            }
                            Thread.Sleep(2000);
                            break;
                        }
                    }
                }
            }
        }

        private void backgroundWorker_faststudy_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("一键学习完成！");
        }
    }
}

