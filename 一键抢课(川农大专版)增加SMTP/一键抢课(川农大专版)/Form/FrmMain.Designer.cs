namespace 一键抢课_川农大专版_
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
            this.panel_login = new System.Windows.Forms.Panel();
            this.btn_login = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_pwd = new System.Windows.Forms.TextBox();
            this.tb_user = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel_main = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lb_username = new System.Windows.Forms.Label();
            this.lb_userid = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox_photo = new System.Windows.Forms.PictureBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.comboBox_type = new System.Windows.Forms.ComboBox();
            this.comboBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btn_stop = new System.Windows.Forms.Button();
            this.btn_qk = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_del = new System.Windows.Forms.Button();
            this.btn_add = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_id = new System.Windows.Forms.TextBox();
            this.listBox = new System.Windows.Forms.ListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.Menu = new System.Windows.Forms.ToolStripMenuItem();
            this.exit = new System.Windows.Forms.ToolStripMenuItem();
            this.Contact = new System.Windows.Forms.ToolStripMenuItem();
            this.Weibo = new System.Windows.Forms.ToolStripMenuItem();
            this.About = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.status = new System.Windows.Forms.ToolStripStatusLabel();
            this.listBox_log = new System.Windows.Forms.ListBox();
            this.panel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_login2 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.Column_kcmc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_kclx = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_js = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_nCount1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_nCount2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_locked = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lb_count = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker_chkUpdate = new System.ComponentModel.BackgroundWorker();
            this.timer_chkUpdate = new System.Windows.Forms.Timer(this.components);
            this.timer_hk = new System.Windows.Forms.Timer(this.components);
            this.panel_login.SuspendLayout();
            this.panel_main.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_photo)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.panel.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_login
            // 
            this.panel_login.BackColor = System.Drawing.SystemColors.Control;
            this.panel_login.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_login.Controls.Add(this.btn_login);
            this.panel_login.Controls.Add(this.label3);
            this.panel_login.Controls.Add(this.tb_pwd);
            this.panel_login.Controls.Add(this.tb_user);
            this.panel_login.Controls.Add(this.label2);
            this.panel_login.Controls.Add(this.label1);
            this.panel_login.Location = new System.Drawing.Point(168, 46);
            this.panel_login.Name = "panel_login";
            this.panel_login.Size = new System.Drawing.Size(257, 183);
            this.panel_login.TabIndex = 0;
            // 
            // btn_login
            // 
            this.btn_login.Location = new System.Drawing.Point(88, 132);
            this.btn_login.Name = "btn_login";
            this.btn_login.Size = new System.Drawing.Size(75, 23);
            this.btn_login.TabIndex = 3;
            this.btn_login.TabStop = false;
            this.btn_login.Text = "登录";
            this.btn_login.UseVisualStyleBackColor = true;
            this.btn_login.Click += new System.EventHandler(this.btn_login2_Click);
            this.btn_login.MouseEnter += new System.EventHandler(this.btn_login_MouseEnter);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(89, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 22);
            this.label3.TabIndex = 4;
            this.label3.Text = "用户登录";
            // 
            // tb_pwd
            // 
            this.tb_pwd.Location = new System.Drawing.Point(76, 88);
            this.tb_pwd.Name = "tb_pwd";
            this.tb_pwd.Size = new System.Drawing.Size(147, 21);
            this.tb_pwd.TabIndex = 2;
            this.tb_pwd.UseSystemPasswordChar = true;
            // 
            // tb_user
            // 
            this.tb_user.Location = new System.Drawing.Point(76, 51);
            this.tb_user.Name = "tb_user";
            this.tb_user.Size = new System.Drawing.Size(147, 21);
            this.tb_user.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "密码：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "学号：";
            // 
            // panel_main
            // 
            this.panel_main.Controls.Add(this.groupBox1);
            this.panel_main.Controls.Add(this.groupBox3);
            this.panel_main.Controls.Add(this.groupBox2);
            this.panel_main.Location = new System.Drawing.Point(3, 1);
            this.panel_main.Name = "panel_main";
            this.panel_main.Size = new System.Drawing.Size(410, 224);
            this.panel_main.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lb_username);
            this.groupBox1.Controls.Add(this.lb_userid);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.pictureBox_photo);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(112, 217);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "用户信息";
            // 
            // lb_username
            // 
            this.lb_username.AutoSize = true;
            this.lb_username.Location = new System.Drawing.Point(4, 192);
            this.lb_username.Name = "lb_username";
            this.lb_username.Size = new System.Drawing.Size(47, 12);
            this.lb_username.TabIndex = 3;
            this.lb_username.Text = "姓 名：";
            // 
            // lb_userid
            // 
            this.lb_userid.AutoSize = true;
            this.lb_userid.Location = new System.Drawing.Point(4, 171);
            this.lb_userid.Name = "lb_userid";
            this.lb_userid.Size = new System.Drawing.Size(47, 12);
            this.lb_userid.TabIndex = 2;
            this.lb_userid.Text = "学 号：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(4, 149);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "欢迎使用!";
            // 
            // pictureBox_photo
            // 
            this.pictureBox_photo.BackColor = System.Drawing.Color.White;
            this.pictureBox_photo.ImageLocation = "";
            this.pictureBox_photo.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox_photo.InitialImage")));
            this.pictureBox_photo.Location = new System.Drawing.Point(6, 17);
            this.pictureBox_photo.Name = "pictureBox_photo";
            this.pictureBox_photo.Size = new System.Drawing.Size(98, 126);
            this.pictureBox_photo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_photo.TabIndex = 0;
            this.pictureBox_photo.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.comboBox_type);
            this.groupBox3.Controls.Add(this.comboBox);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.btn_stop);
            this.groupBox3.Controls.Add(this.btn_qk);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Location = new System.Drawing.Point(121, 129);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(286, 91);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "定时选课设定";
            // 
            // comboBox_type
            // 
            this.comboBox_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_type.FormattingEnabled = true;
            this.comboBox_type.Items.AddRange(new object[] {
            "选课模式",
            "刷课模式",
            "换课模式"});
            this.comboBox_type.Location = new System.Drawing.Point(6, 29);
            this.comboBox_type.Name = "comboBox_type";
            this.comboBox_type.Size = new System.Drawing.Size(121, 20);
            this.comboBox_type.TabIndex = 9;
            this.comboBox_type.TabStop = false;
            this.comboBox_type.SelectedIndexChanged += new System.EventHandler(this.comboBox_type_SelectedIndexChanged);
            // 
            // comboBox
            // 
            this.comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox.FormattingEnabled = true;
            this.comboBox.Location = new System.Drawing.Point(6, 65);
            this.comboBox.Name = "comboBox";
            this.comboBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.comboBox.Size = new System.Drawing.Size(121, 20);
            this.comboBox.TabIndex = 4;
            this.comboBox.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(4, 51);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 8;
            this.label6.Text = "间隔时间：";
            // 
            // btn_stop
            // 
            this.btn_stop.Location = new System.Drawing.Point(161, 57);
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(98, 32);
            this.btn_stop.TabIndex = 6;
            this.btn_stop.TabStop = false;
            this.btn_stop.Text = "停止";
            this.btn_stop.UseVisualStyleBackColor = true;
            this.btn_stop.Click += new System.EventHandler(this.btn_stop_Click);
            // 
            // btn_qk
            // 
            this.btn_qk.Location = new System.Drawing.Point(161, 19);
            this.btn_qk.Name = "btn_qk";
            this.btn_qk.Size = new System.Drawing.Size(98, 32);
            this.btn_qk.TabIndex = 5;
            this.btn_qk.TabStop = false;
            this.btn_qk.Text = "开始";
            this.btn_qk.UseVisualStyleBackColor = true;
            this.btn_qk.Click += new System.EventHandler(this.btn_qk_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label7.Location = new System.Drawing.Point(6, 15);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 5;
            this.label7.Text = "选课模式：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_del);
            this.groupBox2.Controls.Add(this.btn_add);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.tb_id);
            this.groupBox2.Controls.Add(this.listBox);
            this.groupBox2.Location = new System.Drawing.Point(121, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(286, 121);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "待选课信息";
            // 
            // btn_del
            // 
            this.btn_del.Location = new System.Drawing.Point(143, 89);
            this.btn_del.Name = "btn_del";
            this.btn_del.Size = new System.Drawing.Size(88, 23);
            this.btn_del.TabIndex = 3;
            this.btn_del.TabStop = false;
            this.btn_del.Text = "删除";
            this.btn_del.UseVisualStyleBackColor = true;
            this.btn_del.Click += new System.EventHandler(this.btn_del_Click);
            // 
            // btn_add
            // 
            this.btn_add.Location = new System.Drawing.Point(143, 62);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(88, 23);
            this.btn_add.TabIndex = 2;
            this.btn_add.TabStop = false;
            this.btn_add.Text = "添加";
            this.btn_add.UseVisualStyleBackColor = true;
            this.btn_add.Click += new System.EventHandler(this.btn_add_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label5.Location = new System.Drawing.Point(120, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "课程编号：";
            // 
            // tb_id
            // 
            this.tb_id.Location = new System.Drawing.Point(122, 32);
            this.tb_id.Name = "tb_id";
            this.tb_id.Size = new System.Drawing.Size(137, 21);
            this.tb_id.TabIndex = 1;
            // 
            // listBox
            // 
            this.listBox.FormattingEnabled = true;
            this.listBox.ItemHeight = 12;
            this.listBox.Location = new System.Drawing.Point(6, 17);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(100, 100);
            this.listBox.TabIndex = 0;
            this.listBox.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.White;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu,
            this.Contact,
            this.Weibo,
            this.About});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(602, 25);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // Menu
            // 
            this.Menu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exit});
            this.Menu.Name = "Menu";
            this.Menu.Size = new System.Drawing.Size(44, 21);
            this.Menu.Text = "菜单";
            // 
            // exit
            // 
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(100, 22);
            this.exit.Text = "退出";
            this.exit.Click += new System.EventHandler(this.exit_Click);
            // 
            // Contact
            // 
            this.Contact.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Contact.Name = "Contact";
            this.Contact.Size = new System.Drawing.Size(68, 21);
            this.Contact.Text = "给我留言";
            this.Contact.Click += new System.EventHandler(this.Contact_Click);
            // 
            // Weibo
            // 
            this.Weibo.ForeColor = System.Drawing.Color.Red;
            this.Weibo.Name = "Weibo";
            this.Weibo.Size = new System.Drawing.Size(72, 21);
            this.Weibo.Text = "找找Jovi?";
            this.Weibo.Click += new System.EventHandler(this.Weibo_Click);
            // 
            // About
            // 
            this.About.Name = "About";
            this.About.Size = new System.Drawing.Size(44, 21);
            this.About.Text = "关于";
            this.About.Click += new System.EventHandler(this.About_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.status});
            this.statusStrip.Location = new System.Drawing.Point(0, 387);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(602, 22);
            this.statusStrip.TabIndex = 3;
            this.statusStrip.Text = "statusStrip1";
            // 
            // status
            // 
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(113, 17);
            this.status.Text = "我只是个选课助手~";
            // 
            // listBox_log
            // 
            this.listBox_log.ForeColor = System.Drawing.Color.OrangeRed;
            this.listBox_log.FormattingEnabled = true;
            this.listBox_log.ItemHeight = 12;
            this.listBox_log.Location = new System.Drawing.Point(419, 2);
            this.listBox_log.Name = "listBox_log";
            this.listBox_log.Size = new System.Drawing.Size(180, 220);
            this.listBox_log.TabIndex = 4;
            this.listBox_log.TabStop = false;
            // 
            // panel
            // 
            this.panel.Controls.Add(this.panel1);
            this.panel.Controls.Add(this.label8);
            this.panel.Controls.Add(this.panel_main);
            this.panel.Controls.Add(this.listBox_log);
            this.panel.Controls.Add(this.dataGridView);
            this.panel.Controls.Add(this.lb_count);
            this.panel.Location = new System.Drawing.Point(0, 26);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(602, 358);
            this.panel.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btn_login2);
            this.panel1.Controls.Add(this.panel_login);
            this.panel1.Location = new System.Drawing.Point(0, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(602, 357);
            this.panel1.TabIndex = 6;
            // 
            // btn_login2
            // 
            this.btn_login2.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btn_login2.FlatAppearance.BorderSize = 0;
            this.btn_login2.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.btn_login2.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.btn_login2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_login2.Location = new System.Drawing.Point(3, 340);
            this.btn_login2.Name = "btn_login2";
            this.btn_login2.Size = new System.Drawing.Size(10, 14);
            this.btn_login2.TabIndex = 5;
            this.btn_login2.TabStop = false;
            this.btn_login2.UseVisualStyleBackColor = true;
            this.btn_login2.Click += new System.EventHandler(this.btn_login_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.OrangeRed;
            this.label8.Location = new System.Drawing.Point(4, 227);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 12);
            this.label8.TabIndex = 8;
            this.label8.Text = "已选课程:";
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToResizeRows = false;
            this.dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_kcmc,
            this.Column_kclx,
            this.Column_js,
            this.Column_nCount1,
            this.Column_nCount2,
            this.Column_locked});
            this.dataGridView.Location = new System.Drawing.Point(2, 242);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.RowHeadersWidth = 20;
            this.dataGridView.RowTemplate.Height = 23;
            this.dataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.Size = new System.Drawing.Size(602, 115);
            this.dataGridView.TabIndex = 7;
            this.dataGridView.TabStop = false;
            // 
            // Column_kcmc
            // 
            this.Column_kcmc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column_kcmc.HeaderText = "课程名称";
            this.Column_kcmc.Name = "Column_kcmc";
            this.Column_kcmc.ReadOnly = true;
            this.Column_kcmc.Width = 160;
            // 
            // Column_kclx
            // 
            this.Column_kclx.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column_kclx.HeaderText = "课程性质";
            this.Column_kclx.Name = "Column_kclx";
            this.Column_kclx.ReadOnly = true;
            this.Column_kclx.Width = 140;
            // 
            // Column_js
            // 
            this.Column_js.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column_js.HeaderText = "教师";
            this.Column_js.Name = "Column_js";
            this.Column_js.ReadOnly = true;
            this.Column_js.Width = 85;
            // 
            // Column_nCount1
            // 
            this.Column_nCount1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column_nCount1.HeaderText = "计划人数";
            this.Column_nCount1.Name = "Column_nCount1";
            this.Column_nCount1.ReadOnly = true;
            this.Column_nCount1.Width = 78;
            // 
            // Column_nCount2
            // 
            this.Column_nCount2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column_nCount2.HeaderText = "已选人数";
            this.Column_nCount2.Name = "Column_nCount2";
            this.Column_nCount2.ReadOnly = true;
            this.Column_nCount2.Width = 78;
            // 
            // Column_locked
            // 
            this.Column_locked.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column_locked.HeaderText = "锁定";
            this.Column_locked.Name = "Column_locked";
            this.Column_locked.ReadOnly = true;
            this.Column_locked.Width = 60;
            // 
            // lb_count
            // 
            this.lb_count.AutoSize = true;
            this.lb_count.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lb_count.Location = new System.Drawing.Point(69, 227);
            this.lb_count.Name = "lb_count";
            this.lb_count.Size = new System.Drawing.Size(29, 12);
            this.lb_count.TabIndex = 9;
            this.lb_count.Text = "0 门";
            // 
            // timer
            // 
            this.timer.Interval = 4000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            // 
            // backgroundWorker_chkUpdate
            // 
            this.backgroundWorker_chkUpdate.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_chkUpdate_DoWork);
            // 
            // timer_chkUpdate
            // 
            this.timer_chkUpdate.Interval = 1800000;
            this.timer_chkUpdate.Tick += new System.EventHandler(this.timer_chkUpdate_Tick);
            // 
            // timer_hk
            // 
            this.timer_hk.Interval = 30000;
            this.timer_hk.Tick += new System.EventHandler(this.timer_hk_Tick);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 409);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "川农选课助手(萌萌哒)";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.panel_login.ResumeLayout(false);
            this.panel_login.PerformLayout();
            this.panel_main.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_photo)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel_login;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_login;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_pwd;
        private System.Windows.Forms.TextBox tb_user;
        private System.Windows.Forms.Panel panel_main;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem Menu;
        private System.Windows.Forms.ToolStripMenuItem About;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lb_userid;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox_photo;
        private System.Windows.Forms.Label lb_username;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel status;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.Button btn_del;
        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_id;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btn_qk;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button btn_stop;
		private System.Windows.Forms.ListBox listBox_log;
		private System.Windows.Forms.Panel panel;
		private System.Windows.Forms.Timer timer;
		private System.Windows.Forms.ComboBox comboBox;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ToolStripMenuItem exit;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.ToolStripMenuItem Contact;
        private System.Windows.Forms.ToolStripMenuItem Weibo;
        private System.ComponentModel.BackgroundWorker backgroundWorker_chkUpdate;
        private System.Windows.Forms.Timer timer_chkUpdate;
        private System.Windows.Forms.Button btn_login2;
        private System.Windows.Forms.ComboBox comboBox_type;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_kcmc;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_kclx;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_js;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_nCount1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_nCount2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_locked;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lb_count;
        private System.Windows.Forms.Timer timer_hk;

        public System.Windows.Forms.ToolStripMenuItem contact
        {
            get { return Contact; }
            set { Contact = value; }
        }
    }
}

