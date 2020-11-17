namespace пмм_6
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.glControl1 = new OpenTK.GLControl();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.glControl2 = new OpenTK.GLControl();
            this.glControl3 = new OpenTK.GLControl();
            this.SuspendLayout();
            // 
            // glControl1
            // 
            this.glControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Location = new System.Drawing.Point(0, 0);
            this.glControl1.Margin = new System.Windows.Forms.Padding(0);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(386, 600);
            this.glControl1.TabIndex = 0;
            this.glControl1.VSync = false;
            this.glControl1.Load += new System.EventHandler(this.glControl1_Load);
            this.glControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl1_Paint);
            this.glControl1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.glControl1_KeyDown);
            this.glControl1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseClick);
            this.glControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseMove);
            this.glControl1.Resize += new System.EventHandler(this.glControl1_Resize);
            // 
            // glControl2
            // 
            this.glControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.glControl2.BackColor = System.Drawing.Color.Black;
            this.glControl2.Location = new System.Drawing.Point(386, 0);
            this.glControl2.Margin = new System.Windows.Forms.Padding(0);
            this.glControl2.Name = "glControl2";
            this.glControl2.Size = new System.Drawing.Size(398, 300);
            this.glControl2.TabIndex = 1;
            this.glControl2.VSync = false;
            this.glControl2.Load += new System.EventHandler(this.glControl2_Load);
            this.glControl2.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl2_Paint);
            this.glControl2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.glControl2_KeyDown);
            this.glControl2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.glControl2_MouseClick);
            this.glControl2.Resize += new System.EventHandler(this.glControl2_Resize);
            // 
            // glControl3
            // 
            this.glControl3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.glControl3.BackColor = System.Drawing.Color.Black;
            this.glControl3.Location = new System.Drawing.Point(386, 300);
            this.glControl3.Margin = new System.Windows.Forms.Padding(0);
            this.glControl3.Name = "glControl3";
            this.glControl3.Size = new System.Drawing.Size(398, 253);
            this.glControl3.TabIndex = 2;
            this.glControl3.VSync = false;
            this.glControl3.Load += new System.EventHandler(this.glControl3_Load);
            this.glControl3.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl3_Paint);
            this.glControl3.KeyDown += new System.Windows.Forms.KeyEventHandler(this.glControl3_KeyDown);
            this.glControl3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.glControl3_MouseClick);
            this.glControl3.Resize += new System.EventHandler(this.glControl3_Resize);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 553);
            this.Controls.Add(this.glControl3);
            this.Controls.Add(this.glControl2);
            this.Controls.Add(this.glControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private OpenTK.GLControl glControl1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private OpenTK.GLControl glControl2;
        private OpenTK.GLControl glControl3;
    }
}

