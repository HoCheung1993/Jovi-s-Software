using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;


/*Demo*/
 //UpdateSever us = new UpdateSever("http://blog.sina.com.cn/s/blog_a62456420101vkbb.html", "Get", null, GetVersion, "2014年5月12日");
 //           us.Updating();
//Updating()更新检测 CanUseOld决定能否不更新使用

namespace 一键抢课_川农大专版_
{
    class UpdateSever:GetUpdateInfo
    {
        private string oldDt;
        IVersionInfo version;
        bool needUpdate ; //更新标志
        bool CanUseOld; //不更新能否使用

        public bool NeedUpdate
        {
            get { return needUpdate; }
        }

        public UpdateSever(string Url,string Method,string PostData,MyDeleGetInfo Callback,string OldDt,bool Canuseold):base(Url,Method,PostData,Callback)
        {
            oldDt = OldDt;
            CanUseOld = Canuseold;
            version = base.GetUpdateVersionInfo;
            TimeSpan ts=new TimeSpan (DateTime.Parse(version.LatestDt).Ticks-DateTime.Parse(oldDt).Ticks);
            if (ts.Ticks > 0)
                needUpdate = true;
            else
                needUpdate = false;
        }
        public void Updating()  //发现新版本进行更新,不更新可以选择是否不能使用
        {
            if(version.Stopusing=="是")
            {
                MessageBox.Show("管理员被火星人带走了!\n" + version.Stoptips, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Process.Start("http://weibo.com/HKJOVI");
                Process.GetCurrentProcess().Kill();
            }
            if (version.Stopusing == "否")
            {
                MessageBox.Show(version.Stoptips, "公告", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (!needUpdate)
                return ;
            DialogResult result = MessageBox.Show("版本号：" + version.LatestVersion + "\n更新时间：" + version.LatestDt + "\n更新说明：" + version.Tips  + "\n是否更新？", "发现新版本", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Process.Start( version.LatestDownloadUrl);
                Process.GetCurrentProcess().Kill();
            }
            if (!CanUseOld)
            {
                //MessageBox.Show("不更新不能使用！","请更新", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Process.GetCurrentProcess().Kill();
            }
        }

    }
}
