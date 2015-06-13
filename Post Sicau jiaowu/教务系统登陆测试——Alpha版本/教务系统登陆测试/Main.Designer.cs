namespace 教务系统登陆测试
{
	partial class Main
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tb_cookies = new System.Windows.Forms.TextBox();
			this.btn_getcookies = new System.Windows.Forms.Button();
			this.tb_sicauUrl = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.grpEmulateLogin = new System.Windows.Forms.GroupBox();
			this.button1 = new System.Windows.Forms.Button();
			this.rb_result = new System.Windows.Forms.RichTextBox();
			this.lblEmulateLoginResult = new System.Windows.Forms.Label();
			this.tb_pwd = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.tb_uid = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.btn_Login = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.tb_dcode2 = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.tb_mm = new System.Windows.Forms.TextBox();
			this.rtbExtractedHtml = new System.Windows.Forms.RichTextBox();
			this.btnExtractInfo = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.tb_IOstreamPwd = new System.Windows.Forms.TextBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label8 = new System.Windows.Forms.Label();
			this.comboBox_xq = new System.Windows.Forms.ComboBox();
			this.rb_xkresult = new System.Windows.Forms.RichTextBox();
			this.bt_start = new System.Windows.Forms.Button();
			this.btn_dele = new System.Windows.Forms.Button();
			this.btn_modifybh = new System.Windows.Forms.Button();
			this.btn_addbh = new System.Windows.Forms.Button();
			this.label7 = new System.Windows.Forms.Label();
			this.tb_bh = new System.Windows.Forms.TextBox();
			this.listBox_bh = new System.Windows.Forms.ListBox();
			this.label9 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.grpEmulateLogin.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.tb_cookies);
			this.groupBox1.Controls.Add(this.btn_getcookies);
			this.groupBox1.Controls.Add(this.tb_sicauUrl);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(411, 130);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "获取ASPSESSIONID";
			// 
			// tb_cookies
			// 
			this.tb_cookies.BackColor = System.Drawing.SystemColors.Info;
			this.tb_cookies.Location = new System.Drawing.Point(23, 70);
			this.tb_cookies.Multiline = true;
			this.tb_cookies.Name = "tb_cookies";
			this.tb_cookies.Size = new System.Drawing.Size(372, 47);
			this.tb_cookies.TabIndex = 3;
			// 
			// btn_getcookies
			// 
			this.btn_getcookies.Location = new System.Drawing.Point(76, 41);
			this.btn_getcookies.Name = "btn_getcookies";
			this.btn_getcookies.Size = new System.Drawing.Size(244, 23);
			this.btn_getcookies.TabIndex = 2;
			this.btn_getcookies.Text = "获取ASPSESSIONID";
			this.btn_getcookies.UseVisualStyleBackColor = true;
			this.btn_getcookies.Click += new System.EventHandler(this.btn_getcookies_Click);
			// 
			// tb_sicauUrl
			// 
			this.tb_sicauUrl.Location = new System.Drawing.Point(116, 14);
			this.tb_sicauUrl.Name = "tb_sicauUrl";
			this.tb_sicauUrl.ReadOnly = true;
			this.tb_sicauUrl.Size = new System.Drawing.Size(279, 21);
			this.tb_sicauUrl.TabIndex = 1;
			this.tb_sicauUrl.Text = "http://jiaowu.sicau.edu.cn/web/web/web/index.asp";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(21, 17);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(89, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "教务系统首页：";
			// 
			// grpEmulateLogin
			// 
			this.grpEmulateLogin.Controls.Add(this.button1);
			this.grpEmulateLogin.Controls.Add(this.rb_result);
			this.grpEmulateLogin.Controls.Add(this.lblEmulateLoginResult);
			this.grpEmulateLogin.Controls.Add(this.tb_pwd);
			this.grpEmulateLogin.Controls.Add(this.label2);
			this.grpEmulateLogin.Controls.Add(this.tb_uid);
			this.grpEmulateLogin.Controls.Add(this.label3);
			this.grpEmulateLogin.Controls.Add(this.btn_Login);
			this.grpEmulateLogin.Location = new System.Drawing.Point(12, 427);
			this.grpEmulateLogin.Name = "grpEmulateLogin";
			this.grpEmulateLogin.Size = new System.Drawing.Size(411, 192);
			this.grpEmulateLogin.TabIndex = 3;
			this.grpEmulateLogin.TabStop = false;
			this.grpEmulateLogin.Text = "模拟登陆";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(41, 107);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 13;
			this.button1.Text = "抓取姓名";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// rb_result
			// 
			this.rb_result.BackColor = System.Drawing.SystemColors.Info;
			this.rb_result.DetectUrls = false;
			this.rb_result.HideSelection = false;
			this.rb_result.Location = new System.Drawing.Point(129, 107);
			this.rb_result.Name = "rb_result";
			this.rb_result.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
			this.rb_result.Size = new System.Drawing.Size(276, 85);
			this.rb_result.TabIndex = 12;
			this.rb_result.Text = "";
			// 
			// lblEmulateLoginResult
			// 
			this.lblEmulateLoginResult.AutoSize = true;
			this.lblEmulateLoginResult.Location = new System.Drawing.Point(3, 144);
			this.lblEmulateLoginResult.Name = "lblEmulateLoginResult";
			this.lblEmulateLoginResult.Size = new System.Drawing.Size(101, 12);
			this.lblEmulateLoginResult.TabIndex = 11;
			this.lblEmulateLoginResult.Text = "模拟登录的结果：";
			// 
			// tb_pwd
			// 
			this.tb_pwd.BackColor = System.Drawing.SystemColors.Info;
			this.tb_pwd.Location = new System.Drawing.Point(157, 46);
			this.tb_pwd.Name = "tb_pwd";
			this.tb_pwd.Size = new System.Drawing.Size(248, 21);
			this.tb_pwd.TabIndex = 10;
			this.tb_pwd.Text = "19930807";
			this.tb_pwd.UseSystemPasswordChar = true;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(84, 46);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(41, 12);
			this.label2.TabIndex = 9;
			this.label2.Text = "密码：";
			// 
			// tb_uid
			// 
			this.tb_uid.BackColor = System.Drawing.SystemColors.Info;
			this.tb_uid.Location = new System.Drawing.Point(157, 22);
			this.tb_uid.Name = "tb_uid";
			this.tb_uid.Size = new System.Drawing.Size(248, 21);
			this.tb_uid.TabIndex = 8;
			this.tb_uid.Text = "20123025";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(84, 22);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(41, 12);
			this.label3.TabIndex = 7;
			this.label3.Text = "学号：";
			// 
			// btn_Login
			// 
			this.btn_Login.Location = new System.Drawing.Point(97, 70);
			this.btn_Login.Name = "btn_Login";
			this.btn_Login.Size = new System.Drawing.Size(200, 24);
			this.btn_Login.TabIndex = 6;
			this.btn_Login.Text = "模拟登陆";
			this.btn_Login.UseVisualStyleBackColor = true;
			this.btn_Login.Click += new System.EventHandler(this.btn_Login_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(71, 163);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(101, 12);
			this.label4.TabIndex = 4;
			this.label4.Text = "获取的dcode2值：";
			// 
			// tb_dcode2
			// 
			this.tb_dcode2.BackColor = System.Drawing.SystemColors.Info;
			this.tb_dcode2.Location = new System.Drawing.Point(178, 154);
			this.tb_dcode2.Name = "tb_dcode2";
			this.tb_dcode2.Size = new System.Drawing.Size(173, 21);
			this.tb_dcode2.TabIndex = 5;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(71, 195);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(113, 12);
			this.label5.TabIndex = 6;
			this.label5.Text = "进行加密后的密码：";
			// 
			// tb_mm
			// 
			this.tb_mm.BackColor = System.Drawing.SystemColors.Info;
			this.tb_mm.Location = new System.Drawing.Point(178, 192);
			this.tb_mm.Name = "tb_mm";
			this.tb_mm.Size = new System.Drawing.Size(173, 21);
			this.tb_mm.TabIndex = 7;
			// 
			// rtbExtractedHtml
			// 
			this.rtbExtractedHtml.BackColor = System.Drawing.SystemColors.Info;
			this.rtbExtractedHtml.DetectUrls = false;
			this.rtbExtractedHtml.HideSelection = false;
			this.rtbExtractedHtml.Location = new System.Drawing.Point(53, 312);
			this.rtbExtractedHtml.Name = "rtbExtractedHtml";
			this.rtbExtractedHtml.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
			this.rtbExtractedHtml.Size = new System.Drawing.Size(315, 85);
			this.rtbExtractedHtml.TabIndex = 9;
			this.rtbExtractedHtml.Text = "";
			// 
			// btnExtractInfo
			// 
			this.btnExtractInfo.Location = new System.Drawing.Point(128, 264);
			this.btnExtractInfo.Name = "btnExtractInfo";
			this.btnExtractInfo.Size = new System.Drawing.Size(143, 42);
			this.btnExtractInfo.TabIndex = 10;
			this.btnExtractInfo.Text = "提取所需的信息";
			this.btnExtractInfo.UseVisualStyleBackColor = true;
			this.btnExtractInfo.Click += new System.EventHandler(this.btnExtractInfo_Click);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(71, 231);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(77, 12);
			this.label6.TabIndex = 11;
			this.label6.Text = "传输流密码：";
			// 
			// tb_IOstreamPwd
			// 
			this.tb_IOstreamPwd.BackColor = System.Drawing.SystemColors.Info;
			this.tb_IOstreamPwd.Location = new System.Drawing.Point(178, 228);
			this.tb_IOstreamPwd.Name = "tb_IOstreamPwd";
			this.tb_IOstreamPwd.Size = new System.Drawing.Size(173, 21);
			this.tb_IOstreamPwd.TabIndex = 12;
			// 
			// groupBox2
			// 
			this.groupBox2.Location = new System.Drawing.Point(12, 144);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(411, 277);
			this.groupBox2.TabIndex = 13;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "提取信息";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.label8);
			this.groupBox3.Controls.Add(this.comboBox_xq);
			this.groupBox3.Controls.Add(this.rb_xkresult);
			this.groupBox3.Controls.Add(this.bt_start);
			this.groupBox3.Controls.Add(this.btn_dele);
			this.groupBox3.Controls.Add(this.btn_modifybh);
			this.groupBox3.Controls.Add(this.btn_addbh);
			this.groupBox3.Controls.Add(this.label7);
			this.groupBox3.Controls.Add(this.tb_bh);
			this.groupBox3.Controls.Add(this.listBox_bh);
			this.groupBox3.Location = new System.Drawing.Point(440, 13);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(302, 437);
			this.groupBox3.TabIndex = 14;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "网上选课";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(146, 155);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(65, 12);
			this.label8.TabIndex = 12;
			this.label8.Text = "选择学期：";
			// 
			// comboBox_xq
			// 
			this.comboBox_xq.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox_xq.FormattingEnabled = true;
			this.comboBox_xq.Items.AddRange(new object[] {
            "2013-2014-1",
            "2013-2014-2",
            "2014-2015-1",
            "2014-2015-2",
            "2015-2016-1",
            "2015-2016-2"});
			this.comboBox_xq.Location = new System.Drawing.Point(159, 179);
			this.comboBox_xq.Name = "comboBox_xq";
			this.comboBox_xq.Size = new System.Drawing.Size(121, 20);
			this.comboBox_xq.TabIndex = 11;
			// 
			// rb_xkresult
			// 
			this.rb_xkresult.BackColor = System.Drawing.SystemColors.Info;
			this.rb_xkresult.DetectUrls = false;
			this.rb_xkresult.HideSelection = false;
			this.rb_xkresult.Location = new System.Drawing.Point(15, 243);
			this.rb_xkresult.Name = "rb_xkresult";
			this.rb_xkresult.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
			this.rb_xkresult.Size = new System.Drawing.Size(276, 182);
			this.rb_xkresult.TabIndex = 10;
			this.rb_xkresult.Text = "";
			// 
			// bt_start
			// 
			this.bt_start.Location = new System.Drawing.Point(64, 213);
			this.bt_start.Name = "bt_start";
			this.bt_start.Size = new System.Drawing.Size(159, 23);
			this.bt_start.TabIndex = 6;
			this.bt_start.Text = "一键抢课";
			this.bt_start.UseVisualStyleBackColor = true;
			this.bt_start.Click += new System.EventHandler(this.bt_start_Click);
			// 
			// btn_dele
			// 
			this.btn_dele.Location = new System.Drawing.Point(191, 122);
			this.btn_dele.Name = "btn_dele";
			this.btn_dele.Size = new System.Drawing.Size(75, 23);
			this.btn_dele.TabIndex = 5;
			this.btn_dele.Text = "删除";
			this.btn_dele.UseVisualStyleBackColor = true;
			// 
			// btn_modifybh
			// 
			this.btn_modifybh.Location = new System.Drawing.Point(191, 93);
			this.btn_modifybh.Name = "btn_modifybh";
			this.btn_modifybh.Size = new System.Drawing.Size(75, 23);
			this.btn_modifybh.TabIndex = 4;
			this.btn_modifybh.Text = "修改";
			this.btn_modifybh.UseVisualStyleBackColor = true;
			// 
			// btn_addbh
			// 
			this.btn_addbh.Location = new System.Drawing.Point(191, 64);
			this.btn_addbh.Name = "btn_addbh";
			this.btn_addbh.Size = new System.Drawing.Size(75, 23);
			this.btn_addbh.TabIndex = 3;
			this.btn_addbh.Text = "添加";
			this.btn_addbh.UseVisualStyleBackColor = true;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(146, 22);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(65, 12);
			this.label7.TabIndex = 2;
			this.label7.Text = "课程编号：";
			// 
			// tb_bh
			// 
			this.tb_bh.BackColor = System.Drawing.SystemColors.Info;
			this.tb_bh.Location = new System.Drawing.Point(191, 37);
			this.tb_bh.Name = "tb_bh";
			this.tb_bh.Size = new System.Drawing.Size(100, 21);
			this.tb_bh.TabIndex = 1;
			// 
			// listBox_bh
			// 
			this.listBox_bh.BackColor = System.Drawing.SystemColors.Info;
			this.listBox_bh.FormattingEnabled = true;
			this.listBox_bh.ItemHeight = 12;
			this.listBox_bh.Items.AddRange(new object[] {
            "300091663",
            "300091656"});
			this.listBox_bh.Location = new System.Drawing.Point(20, 22);
			this.listBox_bh.Name = "listBox_bh";
			this.listBox_bh.Size = new System.Drawing.Size(120, 160);
			this.listBox_bh.TabIndex = 0;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(438, 473);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(311, 132);
			this.label9.TabIndex = 15;
			this.label9.Text = "抢课工具（川农大专版）\r\n\r\n      即将开发定时选课功能，为解决选课人不在或者\r\n\r\n浏览器不给力问题。\r\n\r\n      此Alpha版本不代表最终版本，" +
    "基础功能已经基\r\n\r\n本全部实现。\r\n\r\n                                  Jovi ©  2014     ";
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(760, 632);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.tb_IOstreamPwd);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.btnExtractInfo);
			this.Controls.Add(this.rtbExtractedHtml);
			this.Controls.Add(this.tb_mm);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.tb_dcode2);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.grpEmulateLogin);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupBox2);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.MaximizeBox = false;
			this.Name = "Main";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "抢课工具（四川农业大学专版)——Alpha";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.grpEmulateLogin.ResumeLayout(false);
			this.grpEmulateLogin.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox tb_cookies;
		private System.Windows.Forms.Button btn_getcookies;
		private System.Windows.Forms.TextBox tb_sicauUrl;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox grpEmulateLogin;
		private System.Windows.Forms.Label lblEmulateLoginResult;
		private System.Windows.Forms.TextBox tb_pwd;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tb_uid;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btn_Login;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox tb_dcode2;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox tb_mm;
		private System.Windows.Forms.RichTextBox rtbExtractedHtml;
		private System.Windows.Forms.Button btnExtractInfo;
		private System.Windows.Forms.RichTextBox rb_result;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox tb_IOstreamPwd;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.RichTextBox rb_xkresult;
		private System.Windows.Forms.Button bt_start;
		private System.Windows.Forms.Button btn_dele;
		private System.Windows.Forms.Button btn_modifybh;
		private System.Windows.Forms.Button btn_addbh;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox tb_bh;
		private System.Windows.Forms.ListBox listBox_bh;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ComboBox comboBox_xq;
		private System.Windows.Forms.Label label9;
	}
}

