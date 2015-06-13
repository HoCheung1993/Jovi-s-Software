using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Diagnostics;
using System.Drawing;
namespace 一键抢课_川农大专版_
{
    public partial class FrmMain : Form
    {
        int nRandom = 0;  //按键随机次数
        User user = null;  //保存登录信息
        string xq = "2014-2015-2"; //学期
        string Version = "2015年1月12日";  //软件版本时间
        CookieCollection curCookies = null;
        bool gotCookie;
        List<KcInfo> kc = new List<KcInfo> { }; //课程信息
        List<SkcInfo> skc = new List<SkcInfo> { };  //等待刷取的课程
        class MyVersionInfo : IVersionInfo
        {
            private string version;//版本号
            public string LatestVersion
            {
                get { return version; }
            }

            private string downloadUrl;//版本下载地址
            public string LatestDownloadUrl
            {
                get { return downloadUrl; }
            }

            private string dt; //版本时间
            public string LatestDt
            {
                get { return dt; }
            }

            private string tips; //公告消息
            public string Tips
            {
                get { return tips; }
            }

            private string stopusing; //停用标志
            public string Stopusing
            {
                get { return stopusing; }
            }

            private string stoptips; //停用说明
            public string Stoptips
            {
                get { return stoptips; }
            }

            public MyVersionInfo(string version, string downloadUrl, string dt, string tips, string stopusing,string stoptips)
            {
                this.version = version;
                this.downloadUrl = downloadUrl;
                this.dt = dt;
                this.tips = tips;
                this.stopusing = stopusing;
                this.stoptips = stoptips;
            }
        }
        public IVersionInfo GetVersion(string respond)  //进行数据流的处理获取版本信息
        {
            Match temp;
            try
            {
                temp = (new Regex(@"版本号:\[.*\]").Match(respond));
                string versionid = temp.Groups[0].Value.ToString();
                versionid = versionid.Substring(5, versionid.Length - 6);

                temp = (new Regex(@"下载地址:\[.*\]").Match(respond));
                string downUrl = temp.Groups[0].Value.ToString();
                downUrl = downUrl.Substring(6, downUrl.Length - 7);

                temp = (new Regex(@"更新日期:\[.*\]").Match(respond));
                string dt = temp.Groups[0].Value.ToString();
                dt = dt.Substring(6, dt.Length - 7);

                temp = (new Regex(@"更新说明:\[.*\]").Match(respond));
                string tips = temp.Groups[0].Value.ToString();
                tips = tips.Substring(6, tips.Length - 7);

                temp = (new Regex(@"是否停用:\[.*\]").Match(respond));
                string stopusing= temp.Groups[0].Value.ToString();
                stopusing = stopusing.Substring(6, stopusing.Length - 7);

                temp = (new Regex(@"停用说明:\[.*\]").Match(respond));
                string stoptips = temp.Groups[0].Value.ToString();
                stoptips = stoptips.Substring(6, stoptips.Length - 7);

                return new MyVersionInfo(versionid, downUrl, dt, tips, stopusing, stoptips);
            }
            catch
            {
                MessageBox.Show("我想确定我是最新版本哟！", "检查更新失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Process.GetCurrentProcess().Kill();
                return null;
            }
        }
     
        //PopUp委托
        private delegate void ShowPopUpCallBack(string kcmc);

        //委托方法
        private void ShowPopup(string kcmc)
        {
            FrmPopUp PopUp = new FrmPopUp(kcmc, user.Userid + user.Username);
            PopUp.Show();
        }

        private void PopupShow(string kcmc)  //UI线程弹出窗体方法
        {
            //创建委托
            ShowPopUpCallBack wt = new ShowPopUpCallBack(ShowPopup);
            //这段代码在主窗体类里面写着，this指主窗体
            this.Invoke(wt, new Object[] { kcmc });
        }

        public FrmMain()
        {
			CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            CheckConnection();   //检查网络连接
            UpdateSever us = new UpdateSever("http://blog.sina.com.cn/s/blog_a62456420101vkbb.html", "Get", null, GetVersion, Version, false);
            us.Updating();
            timer_chkUpdate.Start();  //版本检测计时器
        }  

        private void btn_login_Click(object sender, EventArgs e)
        {
            try
            {
                curCookies = new CookieCollection();
                string username = "";
                if (tb_user.Text == "" || tb_pwd.Text == "")
                {
                    MessageBox.Show("学号，密码！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                #region 网站登陆
                string sicauUrl = "http://jiaowu.sicau.edu.cn/web/web/web/index.asp";
                HttpWebResponse resp = getUrlResponse(sicauUrl);

                string urlToCrawl = "http://jiaowu.sicau.edu.cn/web/web/web/index.asp";
                //generate http request
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(urlToCrawl);
                //use GET method to get url's html
                req.Method = "GET";
                //use request to get response
                //HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                string htmlCharset = "GBK";
                //use songtaste's html's charset GB2312 to decode html
                //otherwise will return messy code
                Encoding htmlEncoding = Encoding.GetEncoding(htmlCharset);
                StreamReader sr = new StreamReader(resp.GetResponseStream(), htmlEncoding);
                //read out the returned html
                string respHtml = sr.ReadToEnd();
                string cookies = "";
                foreach (Cookie ck in resp.Cookies)
                {
                    cookies += "[" + ck.Name + "]=" + ck.Value;
                    if (cookies != "")
                    {
                        gotCookie = true;
                    }
                }

                if (gotCookie)
                {
                    //store cookies
                    curCookies = resp.Cookies;
                }
                else
                {
                    MessageBox.Show("登陆失败，请重试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string dcode2 = "";
                string Rex_dcode2 = @"(dcode2=)(\d{2,})";  //查找dcode2
                Match Founddcode2 = (new Regex(Rex_dcode2)).Match(respHtml);
                if (Founddcode2.Success)
                {
                    dcode2 = Founddcode2.Groups[0].Value.Substring(7);
                }
                else
                {
                    MessageBox.Show("登陆失败，请重试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                LoginInfo logininfo = new LoginInfo(tb_pwd.Text, tb_user.Text, dcode2);
                //init post dict info
                Dictionary<string, string> postDict = new Dictionary<string, string>();
                Dictionary<string, string> headerDict = new Dictionary<string, string>();
                headerDict.Add("Referer", "http://jiaowu.sicau.edu.cn/web/web/web/index.asp");  //服务器的JS会判断这个字段来判断是否跨页面提交
                postDict.Add("lb", "S");
                postDict.Add("submit", "");
                postDict.Add("user", logininfo.Userid);
                postDict.Add("pwd", logininfo.Code);


                string SicauMainLoginUrl = "http://jiaowu.sicau.edu.cn/jiaoshi/bangong/check.asp";
                string loginSicauRespHtml = getUrlRespHtml(SicauMainLoginUrl, headerDict, "gb2312", postDict);
                //check whether got all expected cookies
                //Dictionary<string, bool> cookieCheckDict = new Dictionary<string, bool>();
                //string[] cookiesNameList = { "ASPSESSIONIDQQRRAQAD" };
                //foreach (String cookieToCheck in cookiesNameList)
                //{
                //	cookieCheckDict.Add(cookieToCheck, false);
                //}

                //foreach (Cookie singleCookie in curCookies)
                //{
                //	if (cookieCheckDict.ContainsKey(singleCookie.Name))
                //	{
                //		cookieCheckDict[singleCookie.Name] = true;
                //	}
                //}
                //foreach (bool foundCurCookie in cookieCheckDict.Values)
                //{
                //	allCookiesFound = allCookiesFound && foundCurCookie;
                //}
                string Regex_xm = @"<td\swidth=""99""\salign=""left"">([\u4E00-\u9FA5].+)</td>";
                Match foundxm = (new Regex(Regex_xm).Match(loginSicauRespHtml));
                if (foundxm.Success)
                {
                    username = foundxm.Groups[1].Value.ToString();
                }
                else
                {
                    MessageBox.Show("用户或者密码错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion

                //设置相关信息
                user = new User(logininfo.Userid, username);
                lb_userid.Text += user.Userid;
                lb_username.Text += user.Username;
                pictureBox_photo.ImageLocation = "http://jiaowu.sicau.edu.cn/photo/" + user.Userid + ".jpg";
                panel1.Visible = false;
                AddLog("登陆成功: " + user.Userid);

                string xuan_RespHtml = getUrlRespHtml("http://jiaowu.sicau.edu.cn/xuesheng/gongxuan/gongxuan/xszhinan.asp?xueqi=" + xq, headerDict, "gb2312", null);
                xuan_RespHtml = getUrlRespHtml("http://jiaowu.sicau.edu.cn/xuesheng/gongxuan/gongxuan/xuankeshow.asp", headerDict, "gb2312", null);
                Show_DataGridView(BLL.GetYxkcInfo(xuan_RespHtml));
            }
            catch(SystemException ex)
            {
                if (DialogResult.Yes == MessageBox.Show("小霸王服务器登录出现问题！上报管理员？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
                {
                    _126SMTP.SendEmail(user.Userid + user.Username + " 软件抛出异常", ex.ToString());
                }
                Process.GetCurrentProcess().Kill();
            }
        }

		#region HttpHelper
		public HttpWebResponse getUrlResponse(string url,
								Dictionary<string, string> headerDict,
								Dictionary<string, string> postDict,
								int timeout,
								string postDataStr)
		{
			//CookieCollection parsedCookies;

			HttpWebResponse resp = null;

			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

			req.AllowAutoRedirect = true;
			req.Accept = "*/*";

			//const string gAcceptLanguage = "en-US"; // zh-CN/en-US
			//req.Headers["Accept-Language"] = gAcceptLanguage;

			req.KeepAlive = true;

			//IE8
			//const string gUserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; InfoPath.3; .NET4.0C; .NET4.0E";
			//IE9
			//const string gUserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)"; // x64
			const string gUserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)"; // x86
			//Chrome
			//const string gUserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.99 Safari/533.4";
			//Mozilla Firefox
			//const string gUserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; rv:1.9.2.6) Gecko/20100625 Firefox/3.6.6";
			req.UserAgent = gUserAgent;

			req.Headers["Accept-Encoding"] = "gzip, deflate";
			req.AutomaticDecompression = DecompressionMethods.GZip;

			req.Proxy = null;

			if (timeout > 0)
			{
				req.Timeout = timeout;
			}

			if (curCookies != null)
			{
				req.CookieContainer = new CookieContainer();
				req.CookieContainer.PerDomainCapacity = 40; // following will exceed max default 20 cookie per domain
				req.CookieContainer.Add(curCookies);
			}

			if (headerDict != null)
			{
				foreach (string header in headerDict.Keys)
				{
					string headerValue = "";
					if (headerDict.TryGetValue(header, out headerValue))
					{
						// following are allow the caller overwrite the default header setting
						if (header.ToLower() == "referer")
						{
							req.Referer = headerValue;
						}
						else if (header.ToLower() == "allowautoredirect")
						{
							bool isAllow = false;
							if (bool.TryParse(headerValue, out isAllow))
							{
								req.AllowAutoRedirect = isAllow;
							}
						}
						else if (header.ToLower() == "accept")
						{
							req.Accept = headerValue;
						}
						else if (header.ToLower() == "keepalive")
						{
							bool isKeepAlive = false;
							if (bool.TryParse(headerValue, out isKeepAlive))
							{
								req.KeepAlive = isKeepAlive;
							}
						}
						else if (header.ToLower() == "accept-language")
						{
							req.Headers["Accept-Language"] = headerValue;
						}
						else if (header.ToLower() == "useragent")
						{
							req.UserAgent = headerValue;
						}
						else
						{
							req.Headers[header] = headerValue;
						}
					}
					else
					{
						break;
					}
				}
			}

			if (postDict != null || postDataStr != "")
			{
				req.Method = "POST";
				req.ContentType = "application/x-www-form-urlencoded";

				if (postDict != null)
				{
					postDataStr = quoteParas(postDict);
				}

				//byte[] postBytes = Encoding.GetEncoding("utf-8").GetBytes(postData);
				byte[] postBytes = Encoding.UTF8.GetBytes(postDataStr);
				req.ContentLength = postBytes.Length;

				Stream postDataStream = req.GetRequestStream();
				postDataStream.Write(postBytes, 0, postBytes.Length);
				postDataStream.Close();
			}
			else
			{
				req.Method = "GET";
			}

			//may timeout, has fixed in:
			//http://www.crifan.com/fixed_problem_sometime_httpwebrequest_getresponse_timeout/
			resp = (HttpWebResponse)req.GetResponse();

			updateLocalCookies(resp.Cookies, ref curCookies);

			return resp;
		}

		public HttpWebResponse getUrlResponse(string url,
									Dictionary<string, string> headerDict,
									Dictionary<string, string> postDict)
		{
			return getUrlResponse(url, headerDict, postDict, 0, "");
		}

		public HttpWebResponse getUrlResponse(string url)
		{
			return getUrlResponse(url, null, null, 0, "");
		}

		public string quoteParas(Dictionary<string, string> paras)
		{
			string quotedParas = "";
			bool isFirst = true;
			string val = "";
			foreach (string para in paras.Keys)
			{
				if (paras.TryGetValue(para, out val))
				{
					if (isFirst)
					{
						isFirst = false;
						quotedParas += para + "=" + HttpUtility.UrlPathEncode(val);
					}
					else
					{
						quotedParas += "&" + para + "=" + HttpUtility.UrlPathEncode(val);
					}
				}
				else
				{
					break;
				}
			}

			return quotedParas;
		}

		public void addCookieToCookies(Cookie toAdd, ref CookieCollection cookies, bool overwriteDomain)
		{
			bool found = false;

			if (cookies.Count > 0)
			{
				foreach (Cookie originalCookie in cookies)
				{
					if (originalCookie.Name == toAdd.Name)
					{
						// !!! for different domain, cookie is not same,
						// so should not set the cookie value here while their domains is not same
						// only if it explictly need overwrite domain
						if ((originalCookie.Domain == toAdd.Domain) ||
							((originalCookie.Domain != toAdd.Domain) && overwriteDomain))
						{
							//here can not force convert CookieCollection to HttpCookieCollection,
							//then use .remove to remove this cookie then add
							// so no good way to copy all field value
							originalCookie.Value = toAdd.Value;

							originalCookie.Domain = toAdd.Domain;

							originalCookie.Expires = toAdd.Expires;
							originalCookie.Version = toAdd.Version;
							originalCookie.Path = toAdd.Path;

							//following fields seems should not change
							//originalCookie.HttpOnly = toAdd.HttpOnly;
							//originalCookie.Secure = toAdd.Secure;

							found = true;
							break;
						}
					}
				}
			}

			if (!found)
			{
				if (toAdd.Domain != "")
				{
					// if add the null domain, will lead to follow req.CookieContainer.Add(cookies) failed !!!
					cookies.Add(toAdd);
				}
			}

		}//addCookieToCookies

		//add singel cookie to cookies, default no overwrite domain
		public void addCookieToCookies(Cookie toAdd, ref CookieCollection cookies)
		{
			addCookieToCookies(toAdd, ref cookies, false);
		}

		//check whether the cookies contains the ckToCheck cookie
		//support:
		//ckTocheck is Cookie/string
		//cookies is Cookie/string/CookieCollection/string[]
		public bool isContainCookie(object ckToCheck, object cookies)
		{
			bool isContain = false;

			if ((ckToCheck != null) && (cookies != null))
			{
				string ckName = "";
				Type type = ckToCheck.GetType();

				//string typeStr = ckType.ToString();

				//if (ckType.FullName == "System.string")
				if (type.Name.ToLower() == "string")
				{
					ckName = (string)ckToCheck;
				}
				else if (type.Name == "Cookie")
				{
					ckName = ((Cookie)ckToCheck).Name;
				}

				if (ckName != "")
				{
					type = cookies.GetType();

					// is single Cookie
					if (type.Name == "Cookie")
					{
						if (ckName == ((Cookie)cookies).Name)
						{
							isContain = true;
						}
					}
					// is CookieCollection
					else if (type.Name == "CookieCollection")
					{
						foreach (Cookie ck in (CookieCollection)cookies)
						{
							if (ckName == ck.Name)
							{
								isContain = true;
								break;
							}
						}
					}
					// is single cookie name string
					else if (type.Name.ToLower() == "string")
					{
						if (ckName == (string)cookies)
						{
							isContain = true;
						}
					}
					// is cookie name string[]
					else if (type.Name.ToLower() == "string[]")
					{
						foreach (string name in ((string[])cookies))
						{
							if (ckName == name)
							{
								isContain = true;
								break;
							}
						}
					}
				}
			}

			return isContain;
		}//isContainCookie

		// update cookiesToUpdate to localCookies
		// if omitUpdateCookies designated, then omit cookies of omitUpdateCookies in cookiesToUpdate
		public void updateLocalCookies(CookieCollection cookiesToUpdate, ref CookieCollection localCookies, object omitUpdateCookies)
		{
			if (cookiesToUpdate.Count > 0)
			{
				if (localCookies == null)
				{
					localCookies = cookiesToUpdate;
				}
				else
				{
					foreach (Cookie newCookie in cookiesToUpdate)
					{
						if (isContainCookie(newCookie, omitUpdateCookies))
						{
							// need omit process this
						}
						else
						{
							addCookieToCookies(newCookie, ref localCookies);
						}
					}
				}
			}
		}//updateLocalCookies

		//update cookiesToUpdate to localCookies
		public void updateLocalCookies(CookieCollection cookiesToUpdate, ref CookieCollection localCookies)
		{
			updateLocalCookies(cookiesToUpdate, ref localCookies, null);
		}

		public string getUrlRespHtml(string url,
									  Dictionary<string, string> headerDict,
									  string charset,
									  Dictionary<string, string> postDict,
									  int timeout,
									  string postDataStr)
		{
			string respHtml = "";

			//HttpWebResponse resp = getUrlResponse(url, headerDict, postDict, timeout);
			HttpWebResponse resp = getUrlResponse(url, headerDict, postDict, timeout, postDataStr);

			//long realRespLen = resp.ContentLength;

			StreamReader sr;
			if ((charset != null) && (charset != ""))
			{
				Encoding htmlEncoding = Encoding.GetEncoding(charset);
				sr = new StreamReader(resp.GetResponseStream(), htmlEncoding);
			}
			else
			{
				sr = new StreamReader(resp.GetResponseStream());
			}
			respHtml = sr.ReadToEnd();

			return respHtml;
		}

		public string getUrlRespHtml(string url, Dictionary<string, string> headerDict, string charset, Dictionary<string, string> postDict, string postDataStr)
		{
			return getUrlRespHtml(url, headerDict, charset, postDict, 0, postDataStr);
		}

		public string getUrlRespHtml(string url, Dictionary<string, string> headerDict, string charset, Dictionary<string, string> postDict)
		{
			return getUrlRespHtml(url, headerDict, charset, postDict, 0, "");
		}

		public string getUrlRespHtml(string url, Dictionary<string, string> headerDict, Dictionary<string, string> postDict)
		{
			return getUrlRespHtml(url, headerDict, "", postDict, "");
		}

		public string getUrlRespHtml(string url, Dictionary<string, string> headerDict)
		{
			return getUrlRespHtml(url, headerDict, null);
		}

		public string getUrlRespHtml(string url, string charset, int timeout)
		{
			return getUrlRespHtml(url, null, charset, null, timeout, "");
		}

		public string getUrlRespHtml(string url, string charset)
		{
			return getUrlRespHtml(url, charset, 0);
		}

		public string getUrlRespHtml(string url)
		{
			return getUrlRespHtml(url, "");
		}
		#endregion

		private void btn_add_Click(object sender, EventArgs e)
		{
			if (tb_id.Text == "")
				return;
			string Rex_id = @"^[0-9]*[1-9][0-9]*$";
			Match checkid = new Regex(Rex_id).Match(tb_id.Text);
			if (!checkid.Success)
			{
				MessageBox.Show("课程编号好像不对啊！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			foreach (SkcInfo temp in skc)  //检查重复
			{
				if (temp.Skcid == tb_id.Text)
					return;
			}
			skc.Add(new SkcInfo(tb_id.Text));  
			listBox.Items.Add(tb_id.Text);
			AddLog("已添加  " + tb_id.Text);
		}

		private void btn_del_Click(object sender, EventArgs e)
		{
			if (listBox.SelectedIndex == -1)
				return;
			DialogResult result = MessageBox.Show("你确定要删除"+listBox.SelectedItem.ToString()+"吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
			if (result == DialogResult.No)
				return;
			foreach (SkcInfo temp in skc)
			{
				if (temp.Skcid == listBox.SelectedItem.ToString())
				{
					skc.Remove(skc[skc.IndexOf(temp)]);
					break;
				}
			}
			AddLog("已删除  " + listBox.SelectedItem.ToString());
			listBox.Items.Remove(listBox.SelectedItem);
			listBox.Refresh();
		}

		private void btn_qk_Click(object sender, EventArgs e)
		{
            if (listBox.Items.Count == 0)
            {
                MessageBox.Show("一个选课目录都不给我！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (comboBox_type.SelectedIndex == -1)
            {
                MessageBox.Show("模式还没有设定呢！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
			btn_add.Enabled = false;
			btn_del.Enabled = false;
			btn_qk.Enabled = false;
			comboBox.Enabled = false;
            comboBox_type.Enabled = false;
			status.Text = "努力中....";
            timer.Interval =   Convert.ToInt32(decimal.Parse(comboBox.SelectedItem.ToString()) * 1000);
            timer.Start();
            if(comboBox_type.SelectedIndex==2) //换课模式
            {
                timer_hk.Start();  //换课计时器
                MessageBox.Show("萌萌哒已开启30s的换课模式！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
		}

		private void btn_stop_Click(object sender, EventArgs e)
		{

			timer.Stop();
            timer_hk.Stop();
            if (comboBox_type.SelectedIndex == 2)  //刷课模式
            {
                MessageBox.Show("萌萌哒换课模式已停止！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
			btn_add.Enabled = true;
			btn_del.Enabled = true;
			btn_qk.Enabled = true;
			comboBox.Enabled = true;
            comboBox_type.Enabled = true;
            if(status.Text == "努力中....")
			status.Text = "已停止";
		}	

		private void timer_Tick(object sender, EventArgs e)
		{
			if (backgroundWorker.IsBusy)
            {
                return;
            }
			backgroundWorker.RunWorkerAsync();
		}

		private void About_Click(object sender, EventArgs e)
		{
			FrmAbout about = new FrmAbout();
			about.ShowDialog();
		}

		private void exit_Click(object sender, EventArgs e)
		{
			Application.Exit();
			this.Dispose();
		}

		private void AddLog(string info)  //添加操作日志
		{
			if (listBox_log.Items.Count >= 40)
				listBox_log.Items.Clear();
			listBox_log.Items.Add(info);
			listBox_log.Refresh();
			listBox_log.SelectedIndex = listBox_log.Items.Count - 1;
		}

		private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			dobackwork();
		}

		private void dobackwork()  //后台线程抢课操作
		{
            try
            {
                System.Media.SoundPlayer sndPlayer = new System.Media.SoundPlayer(Properties.Resources.Sound1);    //wav格式的铃声 
				for (int i = 0; i < skc.Count; i++)
				{
					SkcInfo temp = skc[i];
					bool Repeated = false;
					string kcid = "";
					string kcmc = "";
					string kclx = "";
					Match Foundkc = null;
                    string strtemp = "";
					Dictionary<string, string> headerDict = new Dictionary<string, string>();
					Dictionary<string, string> postDict = new Dictionary<string, string>();
                    Dictionary<string, string> headerDict_shua = new Dictionary<string, string>();  //刷课的header
                    Dictionary<string, string> postDict_shua = new Dictionary<string, string>();  //刷课的post包
					foreach (KcInfo temp2 in kc)  //去除重复选课
					{
						if (temp2.Kcid == temp.Skcid)
						{
							Repeated = true;
							break;
						}
					}
					if (Repeated)
						continue;  //重复跳出添加循环

                    if (comboBox_type.SelectedIndex > 0) //刷课换课模式
                    {
                        //刷课header和post data
                        headerDict_shua.Add("Referer", "http://jiaowu.sicau.edu.cn/xuesheng/gongxuan/gongxuan/xszhinan.asp?xueqi=" + xq);
                        postDict_shua.Add("selw", "%C8%AB%B2%BF"); //url代码意思是 全部
                        postDict_shua.Add("ww", temp.Skcid);  //课程编号
                        postDict_shua.Add("picha", "yes");
                        postDict_shua.Add("kl", "");
                        postDict_shua.Add("zh", "");
                        postDict_shua.Add("jsj", "");
                        postDict_shua.Add("ku", "");
                        postDict_shua.Add("o", "");
                        postDict_shua.Add("id", "0");
                        postDict_shua.Add("y", "1");
                        postDict_shua.Add("xuangai", "");
                        postDict_shua.Add("act", "");
                        postDict_shua.Add("dizhi", "%2Fxuesheng%2Fgongxuan%2Fgongxuan%2Fkaike.asp%3Fkai%3D%BF%CE%B3%CC%BB%E3%D7%DC");
                        postDict_shua.Add("w1", "%D1%A7%C6%DA%3D%27" + xq + "%27++and+%C5%C5%BF%CE%C0%E0%B1%F0%3C%3E%27%BB%EC%B0%E0%27+and+%CA%C7%B7%F1%B3%B7%B0%E0%3C%3E%27%CA%C7%27+and+%CA%C7%B7%F1%B7%A2%B2%BC%3D%27%CA%C7%27+and+%D0%A3%C7%F8%3D%27%B3%C9%B6%BC%27");
                        postDict_shua.Add("w2", "");
                        postDict_shua.Add("sw1", "");
                        postDict_shua.Add("p", "20");
                        postDict_shua.Add("twid", "250");
                        postDict_shua.Add("wid", "100%2C100%2C100%2C150%2C80%2C80%2C80%2C120%2C80%2C50%2C50%2C50%2C50%2C50%2C100%2C50%2C50%2C50%2C100%2C100%2C50%2C50%2C80");
                        postDict_shua.Add("vrul", "y%2Cy%2Cn%2Cy%2Cy%2Cy%2Cy%2Cy%2Cy%2Cy%2Cy%2Cy%2Cy%2Cy%2Cy%2Cy%2Cy%2Cy%2Cy%2Cy%2Cy%2Cy%2Cy");
                        postDict_shua.Add("m", "");
                        postDict_shua.Add("rul", "n%2Cn%2Cn%2Cn%2Cn%2Cn%2Cn%2Cn%2Cn%2Cn%2Cn%2Cn%2Cn%2Cn%2Cn%2Cn%2Cn%2Cn%2Cn%2Cn%2Cn%2C%CE%C4%2C%CE%C4");
                        postDict_shua.Add("h", "%BF%CE%B3%CC%BB%E3%D7%DC%BF%AA%BF%CE%C4%BF%C2%BC");
                        postDict_shua.Add("rig", "");
                        postDict_shua.Add("bh", "8910");
                        string shua_RespHtml = getUrlRespHtml("http://jiaowu.sicau.edu.cn/xuesheng/gongxuan/gongxuan/xszhinan.asp?xueqi=" + xq, headerDict_shua, "gb2312", postDict_shua);
                        shua_RespHtml = getUrlRespHtml("http://jiaowu.sicau.edu.cn/xuesheng/gongxuan/gongxuan/kaike.asp?kai=%BF%CE%B3%CC%BB%E3%D7%DC", headerDict_shua, "gb2312", postDict_shua);

                        //判断课程是否满了
                        if (BLL.IsFull(shua_RespHtml))
                        {
                            AddLog(temp.Skcid + "满满满，刷刷刷....");
                            continue;
                        }
                        //未满执行抢课
                    }
                    //抢课代码
                    headerDict.Add("Referer", "http://jiaowu.sicau.edu.cn/xuesheng/gongxuan/gongxuan/xszhinan.asp?xueqi=" + xq);                    
                    postDict.Add("bianhao", temp.Skcid);
					string Url = "http://jiaowu.sicau.edu.cn/xuesheng/gongxuan/gongxuan/xuan.asp?bianhao=" + temp.Skcid;
					string RespHtml = getUrlRespHtml(Url, headerDict, "gb2312", postDict);


					if (RespHtml.Contains("未开放"))
					{
						AddLog("选课系统好像尚未开放噢！");
                        //btn_stop_Click(this, new EventArgs());
                        continue;
					}
                    if (RespHtml.Contains("锁定"))
                    {
                        AddLog("此课程已被火星人锁定！");
                        //btn_stop_Click(this, new EventArgs());
                        continue;
                    }
					if (RespHtml.Contains("门"))
					{                    
						AddLog("已达到选课上限啦！");
                        //btn_stop_Click(this, new EventArgs());
                        continue;
					}
					if (RespHtml.Contains("间隔"))
					{
						Thread.Sleep(5000);   //教务处限定5秒的冷却
                        continue;
					}
                    if (RespHtml.Contains("不能代替"))
                    {
                        AddLog(temp.Skcid + "此课程不能选择!");
                        continue;
                    }

                    if (RespHtml.Contains("课程限制"))
                    {
                        AddLog(temp.Skcid + "此课程不能选择!");
                        continue;
                    }

					if (RespHtml.Contains("满"))
					{
						AddLog(temp.Skcid + "满满满，刷刷刷....");
						continue;
					}

                    
                    
                    if(!RespHtml.Contains(@"<input name=""xingzhi"" type=""hidden"" value="""))
                    continue;

                    string Rex_kc = @"<input name=""xingzhi"" type=""hidden"" value=""..?.?.?.?.?.?.?."" />";
                    Foundkc = (new Regex(Rex_kc)).Match(RespHtml);
                    strtemp = Foundkc.Groups[0].Value.ToString();
                    int last = strtemp.LastIndexOf("\"")   ;
                    strtemp = strtemp.Substring(0, last );
                    int start = strtemp.LastIndexOf("\"") +1 ;
                    strtemp = strtemp.Substring(start, last - start);
                    kclx = strtemp;

                    Rex_kc = @"<input name=""bianhao"" type=""hidden"" value="".?.?.?.?.?.?.?.?.?.?.?.?.?.?"" />";
                    Foundkc = (new Regex(Rex_kc)).Match(RespHtml);
                    strtemp = Foundkc.Groups[0].Value.ToString();
                    last = strtemp.LastIndexOf("\"") ;
                    strtemp = strtemp.Substring(0, last);
                    start = strtemp.LastIndexOf("\"") + 1;
                    strtemp = strtemp.Substring(start, last - start );
                    kcid = strtemp;

                    Rex_kc = @"<input name=""kcmc"" type=""hidden"" value="".?.?.?.?.?.?.?.?.?.?.?.?.?.?"" />";
                    Foundkc = (new Regex(Rex_kc)).Match(RespHtml);
                    strtemp = Foundkc.Groups[0].Value.ToString();
                    last = strtemp.LastIndexOf("\"") ;
                    strtemp = strtemp.Substring(0, last);
                    start = strtemp.LastIndexOf("\"") + 1;
                    strtemp = strtemp.Substring(start, last - start);
                    kcmc = strtemp;
                    
					if (kcmc == "" || kcid == "" || kclx == "")
					{
						continue;
					}
					kc.Add(new KcInfo(kcid, kcmc, kclx));
					listBox.Items.Remove(kcid);
					listBox.Items.Add(kcid + kcmc);
					listBox.Refresh();
					AddLog("获取信息  " + kcmc);
				}
				for (int i = 0; i < kc.Count; i++)  //进行抢课
				{
					KcInfo temp = kc[i];
					if (temp.Kclxid == "")
						return;
					if (temp.haschoosed)
					{
						continue;
					}
					Dictionary<string, string> headerDict = new Dictionary<string, string>();
					Dictionary<string, string> postDict = new Dictionary<string, string>();
					postDict.Add("jiaocai", @"%B7%F1");  //不要教材
					postDict.Add("bianhao", temp.Kcid);
					postDict.Add("xingzhi", temp.Kclxid);
                    headerDict.Add("Referer", "http://jiaowu.sicau.edu.cn/xuesheng/gongxuan/gongxuan/kai.asp?xueqi="+xq);
					string Url = "http://jiaowu.sicau.edu.cn/xuesheng/gongxuan/gongxuan/xuansave.asp";
					string RespHtml = getUrlRespHtml(Url, headerDict, "gb2312", postDict);
					if (RespHtml.Contains("成功") || RespHtml.Contains("重复"))
					{
						temp.haschoosed = true;                       
						AddLog("抢课成功  " + temp.Kcmc);
                        PopupShow(temp.Kcmc);
                        string xuan_RespHtml = getUrlRespHtml("http://jiaowu.sicau.edu.cn/xuesheng/gongxuan/gongxuan/xszhinan.asp?xueqi=" + xq, headerDict, "gb2312", null);
                        xuan_RespHtml = getUrlRespHtml("http://jiaowu.sicau.edu.cn/xuesheng/gongxuan/gongxuan/xuankeshow.asp", headerDict, "gb2312", null);
                        Show_DataGridView(BLL.GetYxkcInfo(xuan_RespHtml));
                        sndPlayer.Play(); 
					}
				}
				int Choosedcount = 0;   //抢课成功停止
				for (int i = 0; i < kc.Count; i++)
				{
					if (kc[i].haschoosed)
					{
						Choosedcount++;
					}
				}
				if (Choosedcount == skc.Count)
				{
					btn_stop_Click(this, new EventArgs());
					MessageBox.Show("不辞劳累，快去看看已选课程吧！\n萌萌哒！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
            }
            catch (SystemException ex)
            {
                AddLog("我出异常啦！");
//              btn_stop_Click(this, new EventArgs());
                if (ex.ToString().Contains("System.Net.WebException"))  //网络连接问题
                {
                    return;
                }
                if (DialogResult.Yes == MessageBox.Show("我又调皮出BUG啦！是否告诉管理员？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
                {
                    _126SMTP.SendEmail(user.Userid + user.Username + " 软件抛出异常", ex.ToString());
                }
            }
		}

		private void CheckConnection()  //检测连接性
		{
			try
			{
				string RespHtml = getUrlRespHtml("http://jiaowu.sicau.edu.cn/web/web/web/index.asp");
                RespHtml = getUrlRespHtml("http://blog.sina.com.cn/s/blog_a62456420101vkbb.html");
			}
			catch
			{
				MessageBox.Show("小霸王服务器去哪了呢？\n再点我一次嘛！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Process.GetCurrentProcess().Kill();
			}
		}

        //留言点击事件
        public void Contact_Click(object sender, EventArgs e)
        {
            _126SMTP smtp;
            if (user != null)
                smtp = new _126SMTP(user.Userid + user.Username);
            else
                smtp = new _126SMTP();
            smtp.ShowDialog();
        }

        private void Weibo_Click(object sender, EventArgs e)
        {
            MessageBox.Show("我是低调的 HK Jovi");
            Process.Start("http://weibo.com/HKJOVI");
        }

        public delegate void MessageBoxHandler();  //交回主线程的委托

       

        private void backgroundWorker_chkUpdate_DoWork(object sender, DoWorkEventArgs e)  //检查版本后台线程
        {
            UpdateSever us = new UpdateSever("http://blog.sina.com.cn/s/blog_a62456420101vkbb.html", "Get", null, GetVersion, Version, false);
            if (us.GetUpdateVersionInfo.Stopusing == "否")
            {
                return;
            }
            else
            {
                //主线程出现模式对话框
                this.Invoke(new MessageBoxHandler(delegate()
                {
                    btn_stop_Click(this, new EventArgs());
                    MessageBox.Show("管理员被师姐拉走了.....\n萌萌哒下次再见！","提示",MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Process.Start("http://weibo.com/HKJOVI");
                    Process.GetCurrentProcess().Kill();
                }));
            }
        }

        private void timer_chkUpdate_Tick(object sender, EventArgs e)   //实时监测版本
        {
              if (backgroundWorker_chkUpdate.IsBusy)
                return;
              backgroundWorker_chkUpdate.RunWorkerAsync();
        }

        private void btn_login_MouseEnter(object sender, EventArgs e)  //登录按键逃脱效果
        {
            Random rad = new Random();
            int x = rad.Next(0, 181);
            int y = rad.Next(112,  157);
            this.btn_login.Location = new Point(x, y);
        }

        private void btn_login2_Click(object sender, EventArgs e)   //隐藏登录键
        {
            nRandom++;
            if (nRandom == 1)
            {
                MessageBox.Show("多抓我几次就让你进去！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (nRandom == 2)
            {
                MessageBox.Show("芝麻开门！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (nRandom == 3)
            {
                MessageBox.Show("妈咪妈咪哄！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (nRandom == 4)
            {
                MessageBox.Show("妈咪妈咪妈咪哄！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (nRandom == 5)
            {
                MessageBox.Show("为何不按回车呢！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (nRandom == 6)
            {
                MessageBox.Show("怎么不私信问问Jovi呢！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (nRandom == 7)
            {
                MessageBox.Show("或者你抓住我99次我就告诉你如何登录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (nRandom == 99)
            {
                MessageBox.Show("辛苦辛苦，回车回车。。，点左下角[我]字上面空白就能登陆啦！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void comboBox_type_SelectedIndexChanged(object sender, EventArgs e)  //选课模式选择
        {
            comboBox.Items.Clear();  //清除所有
            comboBox.Items.Insert(0, "30");
            comboBox.Items.Insert(0, "15");
            comboBox.Items.Insert(0, "10");
            comboBox.Items.Insert(0, "5");
            if (comboBox_type.SelectedIndex == 1)  //刷课模式
            {
                comboBox.Items.Insert(0, "4");
                comboBox.Items.Insert(0, "3");
                comboBox.Items.Insert(0, "2");               
            }
            if (comboBox_type.SelectedIndex == 2)  //换课模式
            {
                comboBox.Items.Clear();  //清除所有
                comboBox.Items.Insert(0, "1");
                comboBox.Items.Insert(0, "0.5");
            }
            comboBox.SelectedIndex = 0;  //默认选中第一个
        }  

        private void Show_DataGridView(List<YxkcInfo> yxkcinfo)
        {
            dataGridView.Rows.Clear();
            if (yxkcinfo == null)
                return;
            for (int i =  yxkcinfo.Count - 1 ; i >= 0; i--)
            {
                dataGridView.Rows.Add(yxkcinfo[i].YxKcmc, yxkcinfo[i].YxKclx, yxkcinfo[i].YxKcjs, yxkcinfo[i].Ncount1, yxkcinfo[i].Ncount2, yxkcinfo[i].Locked);
            }
            lb_count.Text = yxkcinfo.Count + "门";
        }

        private void timer_hk_Tick(object sender, EventArgs e)  //换课模式计时器
        {
            btn_stop_Click(this,new EventArgs());           
        }
    }
}
