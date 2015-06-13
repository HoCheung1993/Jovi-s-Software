
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

/*使用说明*/
//返回版本更新的信息需实现IVersionInfo接口
//直接调用属性 GetUpdateVersionInfo可以得到版本信息
//传入委托Callback为版本获取函数,需要实现IVersionInfo Function(string Respond),返回值为版本信息,Respond为待处理数据流
/*Demo*/
//class MyVersionInfo : IVersionInfo
        //{
        //    private string version;//版本号
        //    public string LatestVersion
        //    {
        //        get { return version; }
        //    }

        //    private string downloadUrl;//版本下载地址
        //    public string LatestDownloadUrl
        //    {
        //        get { return downloadUrl; }
        //    }

        //    private string dt; //版本时间
        //    public string LatestDt
        //    {
        //        get { return dt; }
        //    }

        //    public MyVersionInfo(string version, string downloadUrl, string dt)
        //    {
        //        this.version = version;
        //        this.downloadUrl = downloadUrl;
        //        this.dt = dt;
        //    }
        //}
        //public IVersionInfo GetVersion(string respond)  //进行数据流的处理获取版本信息
        //{
        //    Match temp ;
        //    try
        //    {
        //        temp = (new Regex(@"版本号:\[.*\]").Match(respond));
        //        string versionid = temp.Groups[0].Value.ToString();
        //        versionid = versionid.Substring(5, versionid.Length - 6);

        //        temp = (new Regex(@"下载地址:\[.*\]").Match(respond));
        //        string downUrl = temp.Groups[0].Value.ToString();
        //        downUrl = downUrl.Substring(6, downUrl.Length - 7);

        //        temp = (new Regex(@"更新日期:\[.*\]").Match(respond));
        //        string dt = temp.Groups[0].Value.ToString();
        //        dt = dt.Substring(6, dt.Length - 7);

        //        return new MyVersionInfo(versionid, downUrl, dt);
        //    }
        //    catch
        //    {
        //        MessageBox.Show("获取服务器信息失败，请检查网络连接！", "检查更新失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        Process.GetCurrentProcess().Kill();
        //        return null;
        //    } 
        //}
 
  //          GetUpdateInfo info = new GetUpdateInfo("http://blog.sina.com.cn/s/blog_a62456420101vkbb.html", "Get", null, GetVersion); //GetVersion是处理数据返回版本的回调函数
  //          IVersionInfo versionInfo = info.GetUpdateVersionInfo;  //返回版本信息的方法


namespace 一键抢课_川农大专版_
{
    class GetUpdateInfo //获取更新信息,实现接口IVersionInfo
    {
        public delegate IVersionInfo MyDeleGetInfo(string Respond); //处理方法委托，返回需版本信息类

        private MyDeleGetInfo MyCallBack; //传入字符处理方法
        private string Respond; //存储返回数据


        public IVersionInfo GetUpdateVersionInfo //返回版本信息类
        {
            get { return MyCallBack(Respond); }
        }

        public GetUpdateInfo(string Url,string Method,string PostData,MyDeleGetInfo Callback) 
        {
            if (PostData == null)
                PostData = string.Empty;  //提高效率？
            Respond = HttpHelper.SendPostData(Url, "text/html, application/xhtml+xml, */*", "1", "text/html", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)", Method, PostData, new CookieContainer());
            MyCallBack = Callback; //存储处理方法  
        }
    }
    class HttpHelper
    {
        public static string SendPostData(string strUrl, string Accept, string Referer, string ContentType, string UserAgent, string Method, string PostData, CookieContainer cc)
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
                if (hwrQuest.Method.ToUpper()=="POST")  //当不为POST方法的时候不需发送数据流
                {
                    ASCIIEncoding ae = new ASCIIEncoding();
                    byte[] by = ae.GetBytes(PostData);

                    hwrQuest.ContentLength = by.Length;

                    Stream MyStream = hwrQuest.GetRequestStream();
                    MyStream.Write(by, 0, by.Length);
                    MyStream.Close();
                }

                //实例化HttpWebResponse
                HttpWebResponse hwrp = (HttpWebResponse)hwrQuest.GetResponse();
                StreamReader MyStreamR = new StreamReader(hwrp.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                string result = MyStreamR.ReadToEnd();
                MyStreamR.Close();
                return result;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
    }
    public interface IVersionInfo //版本信息接口
    {
        string LatestVersion
        {
            get;
        }
        string LatestDownloadUrl
        {
            get;
        }
        string LatestDt
        {
            get;
        }
        string Tips
        {
            get;
        }
        string Stopusing
        {
            get;
        }
        string Stoptips
        {
            get;
        }
    }
}
