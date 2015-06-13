using System;
using System.Collections.Generic;

namespace 一键抢课_川农大专版_
{
    class User  //用户信息
    {
        private string userid;
        private string username;
        public string Userid
        {
            get { return userid; }
        } //学号
        public string Username
        {
            get { return username; }
        }  //姓名

        public User(string Userid,string Username)
        {
            userid = Userid;
            username = Username;
        }
    }

    class LoginInfo  //登陆信息处理
    {
        private string userid;   //学号
        private string dcode;   //原密码
        private string dcode2;   //抓取前台dcode2
        private string dcode1;  //加密后的密码
        private string code;  //gb2312密码

        public string Userid
        {
            get { return userid; }
        }
        public string Code
        {
            get { return code; }
        }

        public LoginInfo(string Dcode,string Userid,string Dcode2)
        {
            userid = Userid;
            dcode = Dcode;
            dcode2 = Dcode2;
            dcode1 = Getdcode1(dcode, dcode2);
            code = Getcode(dcode1);
        }
        public string Getdcode1(object dcode, object dcode2)  //得到加密后密码
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
        public string Getcode(string dcode1) //得到gb2312密码
        {
            string code=dcode1;
			code = code.Replace("%", "%25");
			code = code.Replace("!", "%21");
			code = code.Replace("#", "%23");
			code = code.Replace("$", "%24");
			code = code.Replace("&", "%26");
			code = code.Replace("'", "%27");
			code = code.Replace("(", "%28");
			code = code.Replace(")", "%29");
			//code = code.Replace("*", "%2A");
			code = code.Replace("+", "%2B");
			code = code.Replace(",", "%2C");
			code = code.Replace("/", "%2F");
			code = code.Replace(":", "%3A");
			code = code.Replace(";", "%3B");
			code = code.Replace("=", "%3D");
			code = code.Replace("?", "%3F");
			code = code.Replace("@", "%40");
			code = code.Replace("[", "%5B");
			code = code.Replace("]", "%5D");
            return code;
        }
    }

	class KcInfo //课程信息
	{
		private string kcid; //课程编号
		private string kcmc;
		private string kclx;  //课程类型
		private string kclxid = "";  //课程类型代码
		public bool haschoosed = false;  //选课成功标志

		public string Kclxid
		{
			get
			{
				return kclxid;
			}
		}
		public string Kcid
		{
			get
			{
				return kcid;
			}
		}
		public string Kcmc
		{
			get
			{
				return kcmc;
			}
		}
		public string Kclx
		{
			get
			{
				return kclx;
			}
		}
		public KcInfo(string kcid, string kcmc, string kclx)
		{
			this.kcid = kcid;
			this.kcmc = kcmc;
			this.kclx = kclx;
			//以下赋值课程类型代码
			if (this.kclx == "公共选修课")
				kclxid = @"%B9%AB%B9%B2%D1%A1%D0%DE%BF%CE";
			if (this.kclx == "必修")
				kclxid = @"%B1%D8%D0%DE";
			if (this.kclx == "推荐选修课")
				kclxid = @"%CD%C6%BC%F6%D1%A1%D0%DE%BF%CE";
			if (this.kclx == "其他选修课")
				kclxid = @"%C6%E4%CB%FB%D1%A1%D0%DE%BF%CE";
			if (this.kclx == "实践教学")
				kclxid = @"%CA%B5%BC%F9%BD%CC%D1%A7";
			if (this.kclx == "专业方向")
				kclxid = @"%D7%A8%D2%B5%B7%BD%CF%F2";
		}
	}

	class SkcInfo //等待刷取的课程信息
	{
		private string skcid;
		public string Skcid
		{
			get
			{
				return skcid;
			}
		}

		public SkcInfo(string skcid)
		{
			this.skcid = skcid;
		}
	}


    class YxkcInfo //已选课程信息
    {
        private string yxKcmc;
        private string yxKclx;
        private string yxKcjs;
        private string ncount1; //计划人数
        private string ncount2; //已选人数
        private string locked;  //是否锁定

        public string YxKcmc
        {
            get { return yxKcmc; }
        }
        public string YxKclx
        {
            get { return yxKclx; }
        }
        public string YxKcjs
        {
            get { return yxKcjs; }
        }
        public string Ncount1
        {
            get { return ncount1; }
        }
        public string Ncount2
        {
            get { return ncount2; }
        }
        public string Locked
        {
            get { return locked; }
        }
        public YxkcInfo(string yxkcmc,string yxkclx,string yxkcjs,string ncount1,string ncount2,string locked)
        {
            this.yxKcmc = yxkcmc;
            this.yxKclx = yxkclx;
            this.yxKcjs = yxkcjs;
            this.ncount1 = ncount1;
            this.ncount2 = ncount2;
            this.locked = locked;
        }
    }

    class BLL
    {
        public static bool IsFull(string respHtml) //处理课程是否满的字段
        {
            string temp = respHtml;
            int nCount1 = 0, nCount2 = 0;  //计划人数，可选人数
            int start = -1,end = -1 ,pos = -1; //学号位置

            //抓字段比较空位
            start = respHtml.LastIndexOf("<td  class=g_body     width=50>");
            start = respHtml.LastIndexOf("<td  class=g_body     width=50>", start);
            start = respHtml.LastIndexOf("<td  class=g_body     width=50>", start);
            temp = temp.Substring(start);
            end = temp.IndexOf(">") + 1;
            pos = temp.IndexOf("</td>");
            temp = temp.Substring(end, pos - end);
            nCount2 = int.Parse(temp);
            end = start;
            start = respHtml.LastIndexOf("<td  class=g_body     width=50>", start);
            temp = respHtml.Substring(start);
            end = temp.IndexOf(">") + 1;
            pos = temp.IndexOf("</td>");
            temp = temp.Substring(end, pos - end);
            nCount1 = int.Parse(temp);

            return nCount1>nCount2? false :true;
        }
        public static List<YxkcInfo> GetYxkcInfo(string respHtml)  //返回已选课表
        {
            List<YxkcInfo> YxkcInfos = new List<YxkcInfo> ();  //保存已选课程列表
            string kcmc,kclx,kcjs,count1,count2,locked;
            string temp = respHtml;
            int end = respHtml.Length;
            int start = temp.LastIndexOf("<td width=40>", end);
            if( start == -1)
                return null;
            while(start != -1)
            {
                start = temp.LastIndexOf("<td width=40>", start) ;
                locked = (temp.Substring(start, end - start).Contains("是")) ? "是" : "否"; //课程是否锁定

                start = temp.LastIndexOf("<td width=50>", end);
                start = temp.IndexOf(">", start) + 1;
                end = temp.IndexOf("&nbsp", start);
                count2 = temp.Substring(start, end - start);//已选人数

                start = temp.LastIndexOf("<td width=50>", end);
                start = temp.LastIndexOf("<td width=50>", start);
                start = temp.IndexOf(">", start) + 1;
                end = temp.IndexOf("&nbsp", start);
                count1 = temp.Substring(start, end - start); //计划人数

                start = temp.LastIndexOf("jiaoshishow.asp?xingming=", end);
                start = temp.IndexOf("=", start) + 1;
                end = temp.IndexOf("\"", start);
                kcjs = temp.Substring(start, end - start); //课程教师

                start = temp.LastIndexOf("jianjie.asp?ke=", end);
                start = temp.IndexOf("<td width=\"100\">", start);
                start = temp.IndexOf(">",start) + 1;
                end = temp.IndexOf("&nbsp", start);
                kclx = temp.Substring(start, end - start); //课程类型

                start = temp.LastIndexOf("jianjie.asp?ke=", end);
                start = temp.IndexOf("=", start) + 1;
                end = temp.IndexOf("\"", start);
                kcmc = temp.Substring(start, end - start); //课程名称

                start = temp.LastIndexOf("<td width=40>", end);
            
                YxkcInfos.Add(new YxkcInfo(kcmc,kclx,kcjs,count1,count2,locked));
            }
            return YxkcInfos;
        }
    }
}
