namespace 空课表生成工具
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripButton_open = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButton_delete = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButton_about = new System.Windows.Forms.ToolStripButton();
			this.listBox = new System.Windows.Forms.ListBox();
			this.dataGridView = new System.Windows.Forms.DataGridView();
			this.groupBox = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btn_save = new System.Windows.Forms.Button();
			this.btn_operate = new System.Windows.Forms.Button();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
			this.toolStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
			this.groupBox.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_open,
            this.toolStripSeparator1,
            this.toolStripButton_delete,
            this.toolStripSeparator2,
            this.toolStripButton_about});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(1136, 25);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripButton_open
			// 
			this.toolStripButton_open.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_open.Image")));
			this.toolStripButton_open.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton_open.Name = "toolStripButton_open";
			this.toolStripButton_open.Size = new System.Drawing.Size(75, 22);
			this.toolStripButton_open.Text = "添加表格";
			this.toolStripButton_open.Click += new System.EventHandler(this.toolStripButton_open_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButton_delete
			// 
			this.toolStripButton_delete.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_delete.Image")));
			this.toolStripButton_delete.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton_delete.Name = "toolStripButton_delete";
			this.toolStripButton_delete.Size = new System.Drawing.Size(75, 22);
			this.toolStripButton_delete.Text = "删除表格";
			this.toolStripButton_delete.Click += new System.EventHandler(this.toolStripButton_delete_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButton_about
			// 
			this.toolStripButton_about.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_about.Image")));
			this.toolStripButton_about.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton_about.Name = "toolStripButton_about";
			this.toolStripButton_about.Size = new System.Drawing.Size(51, 22);
			this.toolStripButton_about.Text = "关于";
			// 
			// listBox
			// 
			this.listBox.FormattingEnabled = true;
			this.listBox.ItemHeight = 12;
			this.listBox.Location = new System.Drawing.Point(6, 20);
			this.listBox.Name = "listBox";
			this.listBox.Size = new System.Drawing.Size(120, 268);
			this.listBox.TabIndex = 1;
			this.listBox.SelectedIndexChanged += new System.EventHandler(this.listBox_SelectedIndexChanged);
			// 
			// dataGridView
			// 
			this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView.Cursor = System.Windows.Forms.Cursors.Hand;
			this.dataGridView.Location = new System.Drawing.Point(158, 29);
			this.dataGridView.Name = "dataGridView";
			this.dataGridView.RowTemplate.Height = 23;
			this.dataGridView.Size = new System.Drawing.Size(963, 551);
			this.dataGridView.TabIndex = 3;
			// 
			// groupBox
			// 
			this.groupBox.Controls.Add(this.label1);
			this.groupBox.Controls.Add(this.btn_save);
			this.groupBox.Controls.Add(this.btn_operate);
			this.groupBox.Controls.Add(this.listBox);
			this.groupBox.Location = new System.Drawing.Point(12, 66);
			this.groupBox.Name = "groupBox";
			this.groupBox.Size = new System.Drawing.Size(134, 481);
			this.groupBox.TabIndex = 4;
			this.groupBox.TabStop = false;
			this.groupBox.Text = "待处理列表";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.ForeColor = System.Drawing.Color.Red;
			this.label1.Location = new System.Drawing.Point(6, 295);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(113, 96);
			this.label1.TabIndex = 4;
			this.label1.Text = "\r\nTips:\r\n\r\n① 添加 Excel课表\r\n\r\n② 点击 一键制作 \r\n\r\n③ 点击 保存到... \r\n";
			// 
			// btn_save
			// 
			this.btn_save.Location = new System.Drawing.Point(27, 443);
			this.btn_save.Name = "btn_save";
			this.btn_save.Size = new System.Drawing.Size(75, 23);
			this.btn_save.TabIndex = 3;
			this.btn_save.Text = "保存到....";
			this.btn_save.UseVisualStyleBackColor = true;
			// 
			// btn_operate
			// 
			this.btn_operate.Location = new System.Drawing.Point(27, 414);
			this.btn_operate.Name = "btn_operate";
			this.btn_operate.Size = new System.Drawing.Size(75, 23);
			this.btn_operate.TabIndex = 2;
			this.btn_operate.Text = "一键制作";
			this.btn_operate.UseVisualStyleBackColor = true;
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1});
			this.statusStrip1.Location = new System.Drawing.Point(0, 591);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(1136, 22);
			this.statusStrip1.TabIndex = 5;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripProgressBar1
			// 
			this.toolStripProgressBar1.Name = "toolStripProgressBar1";
			this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1136, 613);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.groupBox);
			this.Controls.Add(this.dataGridView);
			this.Controls.Add(this.toolStrip1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Main";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "空课表制作工具";
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
			this.groupBox.ResumeLayout(false);
			this.groupBox.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton toolStripButton_open;
		private System.Windows.Forms.ToolStripButton toolStripButton_delete;
		private System.Windows.Forms.ListBox listBox;
		private System.Windows.Forms.DataGridView dataGridView;
		private System.Windows.Forms.GroupBox groupBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btn_save;
		private System.Windows.Forms.Button btn_operate;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton toolStripButton_about;
	}
}

