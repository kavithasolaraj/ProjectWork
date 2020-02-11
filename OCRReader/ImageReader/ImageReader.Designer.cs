namespace ImageReader
{
    partial class ImageReader
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
            this.Browse = new System.Windows.Forms.Button();
            this.printBox = new System.Windows.Forms.TextBox();
            this.srcLang = new System.Windows.Forms.ComboBox();
            this.lbl_srcLang = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Browse
            // 
            this.Browse.Location = new System.Drawing.Point(474, 60);
            this.Browse.Name = "Browse";
            this.Browse.Size = new System.Drawing.Size(142, 55);
            this.Browse.TabIndex = 0;
            this.Browse.Text = "Browse";
            this.Browse.UseVisualStyleBackColor = true;
            this.Browse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // printBox
            // 
            this.printBox.Location = new System.Drawing.Point(65, 165);
            this.printBox.Multiline = true;
            this.printBox.Name = "printBox";
            this.printBox.Size = new System.Drawing.Size(680, 210);
            this.printBox.TabIndex = 1;
            // 
            // srcLang
            // 
            this.srcLang.DisplayMember = "(none)";
            this.srcLang.FormattingEnabled = true;
            this.srcLang.Items.AddRange(new object[] {
            "English",
            "Spanish"});
            this.srcLang.Location = new System.Drawing.Point(291, 85);
            this.srcLang.Name = "srcLang";
            this.srcLang.Size = new System.Drawing.Size(142, 28);
            this.srcLang.TabIndex = 2;
            this.srcLang.Text = "Select";
            this.srcLang.SelectedIndexChanged += new System.EventHandler(this.srcLang_SelectedIndexChanged);
            // 
            // lbl_srcLang
            // 
            this.lbl_srcLang.AutoSize = true;
            this.lbl_srcLang.Location = new System.Drawing.Point(161, 90);
            this.lbl_srcLang.Name = "lbl_srcLang";
            this.lbl_srcLang.Size = new System.Drawing.Size(106, 20);
            this.lbl_srcLang.TabIndex = 4;
            this.lbl_srcLang.Text = "FileLanguage";
            this.lbl_srcLang.Click += new System.EventHandler(this.label1_Click);
            // 
            // ImageReader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lbl_srcLang);
            this.Controls.Add(this.srcLang);
            this.Controls.Add(this.printBox);
            this.Controls.Add(this.Browse);
            this.Name = "ImageReader";
            this.Text = "ImageReader";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Browse;
        private System.Windows.Forms.TextBox printBox;
        private System.Windows.Forms.ComboBox srcLang;
        private System.Windows.Forms.Label lbl_srcLang;
    }
}

