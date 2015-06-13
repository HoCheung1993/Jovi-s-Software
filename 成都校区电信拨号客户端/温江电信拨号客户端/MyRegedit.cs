using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace 温江电信拨号客户端
{
    class CreateIPSec
    {
        public CreateIPSec()
        {
        string[] SubkeysNames;
        string Value="ProhibitIPSec"; //保存原键值名称
        bool HasExist = false; //是否存在键值
        RegistryKey reg = Registry.LocalMachine;
        reg = reg.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\RasMan\Parameters", true); //重新定位
        SubkeysNames = reg.GetValueNames();
        foreach (string keyName in SubkeysNames) //遍历查询键值是否存在
        {
            if (keyName.ToLower() == "prohibitipsec")
            {
                HasExist = true;
                Value = keyName;
            }
        }
        if (HasExist)  //存在键值进行删除
        {
            reg.DeleteValue(Value);
        }
        reg.SetValue("ProhibitIPSec", "1" , RegistryValueKind.DWord); //创建键值
        reg.Close();
        }
    }

    class u_SupplicantHelper
    {
        private string path;
        public string Path  //存储路径
        {
            get { return path;}
        }
        public u_SupplicantHelper()
        {
            RegistryKey reg = Registry.LocalMachine;
            reg = reg.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\四川农业大学校园网认证客户端");
            string [] paths = reg.GetValue("UninstallString").ToString().Split(' ');
            path = paths[0].Substring(0, paths[0].Length - 10);  //获取红蝴蝶路径
        }
    }
}
