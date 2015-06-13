using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data;

namespace 空课表生成工具
{
	class BLL
	{
		public static string GetStrCon(string Filename) //获取连接字符串
		{
			return "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Filename + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=0'";
		}

		public static void ShowExcel(DataGridView dgv, object item)  //显示表格
		{
			string strCon = GetStrCon(item.ToString());
			OleDbConnection myConn = new OleDbConnection(strCon);
			string strCom = "SELECT * FROM [Sheet1$]";
			myConn.Open();
			OleDbDataAdapter myDataAdapter = new OleDbDataAdapter(strCom, myConn);
			DataSet myDataSet = new DataSet();
			myDataAdapter.Fill(myDataSet, "[Sheet1$]");
			myConn.Close();
			DataTable dt = myDataSet.Tables[0]; //初始化DataTable实例
			dgv.DataSource = myDataSet.Tables[0].DefaultView; //显示到datagridview
		}

		public static bool CheckRepeatItem(ListBox lb,object item)  //检查item是否重复,重复返回true
		{
			for (int i = 0; i < lb.Items.Count; i++)
			{
				if (lb.Items[i].ToString().ToLower() == item.ToString().ToLower())
				{
					lb.SetSelected(i, true);  //重复选定Item
					return true;
				}
			}
			return false;
		}
	}
}
