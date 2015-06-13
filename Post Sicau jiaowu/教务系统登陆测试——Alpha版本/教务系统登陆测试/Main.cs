using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace 教务系统登陆测试
{
	public partial class Main : Form
	{
		CookieCollection curCookies = null;
		bool gotCookie, loginOk;

		public Main()
		{
			InitializeComponent();
			curCookies = new CookieCollection();
		}

		private void btn_getcookies_Click(object sender, EventArgs e)
		{
			string sicauUrl = tb_sicauUrl.Text;
			HttpWebResponse resp = getUrlResponse(sicauUrl);

			string urlToCrawl = tb_sicauUrl.Text;
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
			rtbExtractedHtml.Text = respHtml;


			tb_cookies.Text = "";
			foreach (Cookie ck in resp.Cookies)
			{
				tb_cookies.Text += "[" + ck.Name + "]=" + ck.Value;
				if (tb_cookies.Text!=null)
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
				MessageBox.Show("错误：没有找到cookie ！");
			}
		}


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

		private void btn_Login_Click(object sender, EventArgs e)
		{
			if (gotCookie)
			{

				//init post dict info
				Dictionary<string, string> postDict = new Dictionary<string, string>();
				Dictionary<string, string> headerDict = new Dictionary<string, string>();
				headerDict.Add("Referer", "http://jiaowu.sicau.edu.cn/web/web/web/index.asp");  //服务器的JS会判断这个字段来判断是否跨页面提交
				//postDict.Add("ppui_logintime", "");
				//postDict.Add("charset", "utf-8");
				//postDict.Add("codestring", "");
				//postDict.Add("u", "");
				//postDict.Add("safeflg", "0");
				//postDict.Add("staticpage", staticpage);
				//postDict.Add("loginType", "1");
				postDict.Add("submit", "");
				postDict.Add("user", tb_uid.Text);
				postDict.Add("pwd", tb_pwd.Text);
				//postDict.Add("verifycode", "");
				//postDict.Add("mem_pass", "on");

				string baiduMainLoginUrl = "http://jiaowu.sicau.edu.cn/jiaoshi/bangong/check.asp";
				string loginBaiduRespHtml = getUrlRespHtml(baiduMainLoginUrl, headerDict,"gb2312",postDict);

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

				bool allCookiesFound = true;
				//foreach (bool foundCurCookie in cookieCheckDict.Values)
				//{
				//	allCookiesFound = allCookiesFound && foundCurCookie;
				//}


				loginOk = allCookiesFound;
				if (loginOk)
				{
					rb_result.Text = "成功模拟登陆！";
					rb_result.Text += Environment.NewLine + "所返回的HTML源码为：";
					rb_result.Text += Environment.NewLine + loginBaiduRespHtml;
				}
				else
				{
					rb_result.Text = "模拟登陆失败！";
					rb_result.Text += Environment.NewLine + "所返回的HTML源码为：";
					rb_result.Text += Environment.NewLine + loginBaiduRespHtml;
				}
			}
			else
			{
				MessageBox.Show("错误：没有正确获得Cookie！");
			}
		}

		private void btnExtractInfo_Click(object sender, EventArgs e)
		{
			string dcode2 = @"(dcode2=)(\d{2,})";
			Match founddcode2 = (new Regex(dcode2)).Match(rtbExtractedHtml.Text);
			if (founddcode2.Success)
			{
				//extracted the expected h1user's value
				tb_dcode2.Text = founddcode2.Groups[0].Value.Substring(7);
				tb_mm.Text = GetMM(tb_pwd.Text, tb_dcode2.Text);
				tb_IOstreamPwd.Text = StringURI(tb_mm.Text);
				tb_pwd.Text = StringURI(tb_mm.Text);
			}
			else
			{
				tb_dcode2.Text = "Not found !";
			}
		}


		public static string GetMM(object dcode, object dcode2)
		{
			int length = dcode.ToString().Length;
			byte[] b;
			int result;
			string tmpstr;
			string dcode1 = "";
			dcode2 = Convert.ToInt64(dcode2) * 137;
			for (int i = 1; i <= length; i++)
			{
				tmpstr = dcode.ToString().Substring(i - 1, 1);
				b = System.Text.Encoding.Unicode.GetBytes(tmpstr.Substring(0, 1));
				result = Convert.ToInt32(string.Format("{0}", b[0])) - i - Convert.ToInt32(dcode2.ToString().Substring(i - 1, 1));
				dcode1 += (char)result;
			}
			return dcode1;
		}

		public string StringURI(string dcode1)
		{
			dcode1 = dcode1.Replace("%", "%25");
			dcode1 = dcode1.Replace("!", "%21");
			dcode1 = dcode1.Replace("#", "%23");
			dcode1 = dcode1.Replace("$", "%24");
			dcode1 = dcode1.Replace("&", "%26");
			dcode1 = dcode1.Replace("'", "%27");
			dcode1 = dcode1.Replace("(", "%28");
			dcode1 = dcode1.Replace(")", "%29");
			//dcode1 = dcode1.Replace("*", "%2A");
			dcode1 = dcode1.Replace("+", "%2B");
			dcode1 = dcode1.Replace(",", "%2C");
			dcode1 = dcode1.Replace("/", "%2F");
			dcode1 = dcode1.Replace(":", "%3A");
			dcode1 = dcode1.Replace(";", "%3B");
			dcode1 = dcode1.Replace("=", "%3D");
			dcode1 = dcode1.Replace("?", "%3F");
			dcode1 = dcode1.Replace("@", "%40");
			dcode1 = dcode1.Replace("[", "%5B");
			dcode1 = dcode1.Replace("]", "%5D");
			return dcode1;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			string Regex_xm = @"<td\swidth=""99""\salign=""left"">([\u4E00-\u9FA5].+)</td>";
			Match foundxm=(new Regex(Regex_xm).Match(rb_result.Text));
			if(foundxm.Success)
			{
				string xm=foundxm.Groups[1].Value.ToString();
				string str="登陆姓名为："+xm;
				MessageBox.Show(str);
			}
			else
			{
				MessageBox.Show("获取失败！");
			}
		}

		private void bt_start_Click(object sender, EventArgs e)
		{
			Dictionary<string, string> headerDict = new Dictionary<string, string>();
			Dictionary<string, string> postDict = new Dictionary<string, string>();
			postDict.Add("jiaocai", @"%B7%F1");
			postDict.Add("bianhao", listBox_bh.SelectedItem.ToString());
			postDict.Add("xingzhi", "%C7%C0%BF%CE,%BB%B9%D3%D0%CB%AD!");
			headerDict.Add("Referer", "http://jiaowu.sicau.edu.cn/xuesheng/gongxuan/gongxuan/kai.asp?xueqi=1" );
			string Url = "http://jiaowu.sicau.edu.cn/xuesheng/gongxuan/gongxuan/xuansave.asp";
			string RespHtml = getUrlRespHtml(Url, headerDict, "gb2312", postDict);
			rb_xkresult.Text = RespHtml;
		}
	}
}
