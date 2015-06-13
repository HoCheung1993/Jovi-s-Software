using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;

namespace 空课表生成工具
{
	public partial class Main : Form
	{

		public Main()
		{
			InitializeComponent();
		}

		private void toolStripButton_open_Click(object sender, EventArgs e)  //打开
		{
			string FilePath;
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.RestoreDirectory = true;
			ofd.Filter = "Excel表格(*.xls/*.xlsx)|*.xls;*.xlsx";
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				FilePath = ofd.FileName;
				if (!BLL.CheckRepeatItem(listBox, FilePath))
					listBox.Items.Add(FilePath);
			}
		}

		private void toolStripButton_delete_Click(object sender, EventArgs e)  //删除
		{
			listBox.Items.Remove(listBox.SelectedItem);
			listBox.SelectedIndex = -1;  //设定不选定，否者会有SelectedIndexChanged
			dataGridView.DataSource = null;
		}

		private void listBox_SelectedIndexChanged(object sender, EventArgs e)  //点击listbox显示
		{
			if(listBox.SelectedIndex!=-1)  //防止删除的时候错误
			BLL.ShowExcel(dataGridView, listBox.SelectedItem);
		}


	}
}
