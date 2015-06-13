namespace WindowsFormsApplication1
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class InputVer : Form
    {
        private Button button1;
        private IContainer components = null;
        private PictureBox GetVarCode;
        private Label label1;
        private TextBox textBox1;
        public string Var = "";

        public InputVer()
        {
            this.InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((this.textBox1.Text != "") && (this.textBox1.Text.Length == 5))
            {
                this.Var = this.textBox1.Text;
                base.Hide();
            }
            else
            {
                MessageBox.Show("请输入正确的验证码。");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.label1 = new Label();
            this.GetVarCode = new PictureBox();
            this.textBox1 = new TextBox();
            this.button1 = new Button();
            ((ISupportInitialize) this.GetVarCode).BeginInit();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(13, 9);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x131, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "系统检测到您的账号登录位置变化，请输入验证码继续。";
            this.GetVarCode.Location = new Point(0xb5, 0x20);
            this.GetVarCode.Name = "GetVarCode";
            this.GetVarCode.Size = new Size(0x7b, 40);
            this.GetVarCode.TabIndex = 1;
            this.GetVarCode.TabStop = false;
            this.textBox1.Font = new Font("微软雅黑", 15f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.textBox1.Location = new Point(0x22, 0x25);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(0x65, 0x22);
            this.textBox1.TabIndex = 2;
            this.button1.Location = new Point(0x79, 0x53);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x4b, 0x17);
            this.button1.TabIndex = 3;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(this.button1_Click);
            base.AutoScaleDimensions = new SizeF(6f, 12f);
//            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(330, 0x76);
            base.Controls.Add(this.button1);
            base.Controls.Add(this.textBox1);
            base.Controls.Add(this.GetVarCode);
            base.Controls.Add(this.label1);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "InputVer";
            base.ShowInTaskbar = false;
            this.Text = "InputVer";
            base.Load += new EventHandler(this.Inputvar_Load);
            ((ISupportInitialize) this.GetVarCode).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void Inputvar_Load(object sender, EventArgs e)
        {
            base.Left = (Screen.PrimaryScreen.Bounds.Width - base.Width) / 2;
            base.Top = (Screen.PrimaryScreen.Bounds.Height - base.Height) / 2;
            this.GetVarCode.ImageLocation = @"D:\ver.png";
        }
    }
}

