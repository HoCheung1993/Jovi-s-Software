using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace 一键抢课_川农大专版_
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool flag;  //禁止多开
            System.Threading.Mutex mutex = new System.Threading.Mutex(true, Application.ProductName, out flag);
            if (flag)
            {
                // 启用应用程序的可视样式
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // 处理当前在消息队列中的所有 Windows 消息
                Application.DoEvents();
                Application.Run(new FrmMain());

                // 释放 System.Threading.Mutex 一次
                mutex.ReleaseMutex();
            }
            else
            {
                MessageBox.Show(null, "萌萌哒已经不能分身了！!", "萌萌哒提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
        }
    }
}
