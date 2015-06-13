namespace 温江电信拨号客户端
{
	partial class FrmMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_user = new System.Windows.Forms.TextBox();
            this.tb_pwd = new System.Windows.Forms.TextBox();
            this.btn_CreateRAS = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.AllUsersPhoneBook = new DotRas.RasPhoneBook(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.status = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.菜单ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exit = new System.Windows.Forms.ToolStripMenuItem();
            this.help = new System.Windows.Forms.ToolStripMenuItem();
            this.about = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_hzly = new System.Windows.Forms.Button();
            this.btn_CreateIPSec = new System.Windows.Forms.Button();
            this.btn_uSHelper = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "账号：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(52, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "密码：";
            // 
            // tb_user
            // 
            this.tb_user.Location = new System.Drawing.Point(97, 35);
            this.tb_user.Name = "tb_user";
            this.tb_user.Size = new System.Drawing.Size(131, 21);
            this.tb_user.TabIndex = 4;
            // 
            // tb_pwd
            // 
            this.tb_pwd.Location = new System.Drawing.Point(97, 68);
            this.tb_pwd.Name = "tb_pwd";
            this.tb_pwd.Size = new System.Drawing.Size(131, 21);
            this.tb_pwd.TabIndex = 5;
            this.tb_pwd.UseSystemPasswordChar = true;
            // 
            // btn_CreateRAS
            // 
            this.btn_CreateRAS.Location = new System.Drawing.Point(12, 91);
            this.btn_CreateRAS.Name = "btn_CreateRAS";
            this.btn_CreateRAS.Size = new System.Drawing.Size(111, 23);
            this.btn_CreateRAS.TabIndex = 6;
            this.btn_CreateRAS.Text = "创建拨号器";
            this.btn_CreateRAS.UseVisualStyleBackColor = true;
            this.btn_CreateRAS.Click += new System.EventHandler(this.btn_CreateRAS_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(5, 147);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(155, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "默认只需点击： 创建拨号器\r\n";
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.White;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.status});
            this.statusStrip1.Location = new System.Drawing.Point(0, 209);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(288, 22);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // status
            // 
            this.status.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(65, 17);
            this.status.Text = "欢迎使用...";
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.White;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.菜单ToolStripMenuItem,
            this.help,
            this.about});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(288, 25);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 菜单ToolStripMenuItem
            // 
            this.菜单ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exit});
            this.菜单ToolStripMenuItem.Name = "菜单ToolStripMenuItem";
            this.菜单ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.菜单ToolStripMenuItem.Text = "菜单";
            // 
            // exit
            // 
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(100, 22);
            this.exit.Text = "退出";
            this.exit.Click += new System.EventHandler(this.exit_Click);
            // 
            // help
            // 
            this.help.Name = "help";
            this.help.Size = new System.Drawing.Size(44, 21);
            this.help.Text = "帮助";
            this.help.Click += new System.EventHandler(this.help_Click);
            // 
            // about
            // 
            this.about.Name = "about";
            this.about.Size = new System.Drawing.Size(44, 21);
            this.about.Text = "关于";
            this.about.Click += new System.EventHandler(this.about_Click);
            // 
            // btn_hzly
            // 
            this.btn_hzly.ForeColor = System.Drawing.Color.Red;
            this.btn_hzly.Location = new System.Drawing.Point(129, 91);
            this.btn_hzly.Name = "btn_hzly";
            this.btn_hzly.Size = new System.Drawing.Size(147, 23);
            this.btn_hzly.TabIndex = 10;
            this.btn_hzly.Text = "添加路由";
            this.btn_hzly.UseVisualStyleBackColor = true;
            this.btn_hzly.Click += new System.EventHandler(this.btn_hzly_Click);
            // 
            // btn_CreateIPSec
            // 
            this.btn_CreateIPSec.ForeColor = System.Drawing.Color.DarkOrange;
            this.btn_CreateIPSec.Location = new System.Drawing.Point(12, 118);
            this.btn_CreateIPSec.Name = "btn_CreateIPSec";
            this.btn_CreateIPSec.Size = new System.Drawing.Size(111, 23);
            this.btn_CreateIPSec.TabIndex = 12;
            this.btn_CreateIPSec.Text = "添加IPSec";
            this.btn_CreateIPSec.UseVisualStyleBackColor = true;
            this.btn_CreateIPSec.Click += new System.EventHandler(this.btn_CreateIPSec_Click);
            // 
            // btn_uSHelper
            // 
            this.btn_uSHelper.ForeColor = System.Drawing.Color.Blue;
            this.btn_uSHelper.Location = new System.Drawing.Point(129, 118);
            this.btn_uSHelper.Name = "btn_uSHelper";
            this.btn_uSHelper.Size = new System.Drawing.Size(147, 23);
            this.btn_uSHelper.TabIndex = 13;
            this.btn_uSHelper.Text = "卸载Supplicant Helper";
            this.btn_uSHelper.UseVisualStyleBackColor = true;
            this.btn_uSHelper.Click += new System.EventHandler(this.btn_uSHelper_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(4, 163);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(167, 12);
            this.label4.TabIndex = 14;
            this.label4.Text = "无法访问内网点击： 添加路由";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Location = new System.Drawing.Point(4, 180);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(269, 12);
            this.label5.TabIndex = 15;
            this.label5.Text = "VPN连接成功无Internet访问或者VPN错误 789,800";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.DarkOrange;
            this.label6.Location = new System.Drawing.Point(4, 195);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(245, 12);
            this.label6.TabIndex = 16;
            this.label6.Text = "点击  添加IPSec 和 卸载Supplicant Helper";
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 231);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btn_uSHelper);
            this.Controls.Add(this.btn_CreateIPSec);
            this.Controls.Add(this.btn_hzly);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btn_CreateRAS);
            this.Controls.Add(this.tb_pwd);
            this.Controls.Add(this.tb_user);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "成都校区电信拨号客户端";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMain_FormClosed);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion


		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tb_user;
		private System.Windows.Forms.TextBox tb_pwd;
		private System.Windows.Forms.Button btn_CreateRAS;
		private System.Windows.Forms.Label label3;
		private DotRas.RasPhoneBook AllUsersPhoneBook;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel status;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem 菜单ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exit;
		private System.Windows.Forms.ToolStripMenuItem help;
		private System.Windows.Forms.ToolStripMenuItem about;
        private System.Windows.Forms.Button btn_hzly;
        private System.Windows.Forms.Button btn_CreateIPSec;
        private System.Windows.Forms.Button btn_uSHelper;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
	}
}

