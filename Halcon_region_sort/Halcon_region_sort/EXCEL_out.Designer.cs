namespace Halcon_region_sort
{
    partial class EXCEL_out
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listView1 = new System.Windows.Forms.ListView();
            this.name_baogao = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.image = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.name_baogao,
            this.image});
            this.listView1.Location = new System.Drawing.Point(12, 13);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(509, 472);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // name_baogao
            // 
            this.name_baogao.Text = "铁谱图像分析报告";
            // 
            // image
            // 
            this.image.Text = "图片名称";
            // 
            // EXCEL_out
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 505);
            this.Controls.Add(this.listView1);
            this.Name = "EXCEL_out";
            this.Text = "导出excel";
            this.Load += new System.EventHandler(this.EXCEL_out_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader name_baogao;
        private System.Windows.Forms.ColumnHeader image;
    }
}