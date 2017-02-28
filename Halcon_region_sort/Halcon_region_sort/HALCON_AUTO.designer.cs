namespace Halcon_region_sort
{
    partial class halcon_form
    {
        HDevelopExport hd = new HDevelopExport();
        //string imagePayh;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(halcon_form));
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.read_button = new System.Windows.Forms.Button();
            this.button_prcoessing = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.AllowDrop = true;
            this.hWindowControl1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(-15, -20, 2590, 1960);
            this.hWindowControl1.Location = new System.Drawing.Point(158, 12);
            this.hWindowControl1.Name = "hWindowControl1";
            this.hWindowControl1.Size = new System.Drawing.Size(1095, 739);
            this.hWindowControl1.TabIndex = 2;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(1095, 739);
            this.hWindowControl1.HMouseMove += new HalconDotNet.HMouseEventHandler(this.hWindowControl1_HMouseMove);
            // 
            // read_button
            // 
            this.read_button.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.read_button.Font = new System.Drawing.Font("隶书", 42F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.read_button.Location = new System.Drawing.Point(12, 385);
            this.read_button.Name = "read_button";
            this.read_button.Size = new System.Drawing.Size(130, 367);
            this.read_button.TabIndex = 0;
            this.read_button.Text = "图像处理";
            this.read_button.UseVisualStyleBackColor = false;
            this.read_button.Click += new System.EventHandler(this.read_button_Click);
            // 
            // button_prcoessing
            // 
            this.button_prcoessing.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.button_prcoessing.Font = new System.Drawing.Font("隶书", 42F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_prcoessing.Location = new System.Drawing.Point(12, 12);
            this.button_prcoessing.Name = "button_prcoessing";
            this.button_prcoessing.Size = new System.Drawing.Size(130, 367);
            this.button_prcoessing.TabIndex = 1;
            this.button_prcoessing.Text = "读取图片";
            this.button_prcoessing.UseVisualStyleBackColor = false;
            this.button_prcoessing.Click += new System.EventHandler(this.button_prcoessing_Click);
            // 
            // halcon_form
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1278, 751);
            this.Controls.Add(this.hWindowControl1);
            this.Controls.Add(this.button_prcoessing);
            this.Controls.Add(this.read_button);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "halcon_form";
            this.Text = "自动处理";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.halcon_form_FormClosing);
            this.Load += new System.EventHandler(this.halcon_form_Load);
            this.Click += new System.EventHandler(this.halcon_form_Click);
            this.ResumeLayout(false);

        }

        #endregion

        private HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.Button read_button;
        private System.Windows.Forms.Button button_prcoessing;
    }
}

