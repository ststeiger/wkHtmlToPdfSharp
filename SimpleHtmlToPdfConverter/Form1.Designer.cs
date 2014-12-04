namespace SimpleHtmlToPdfConverter
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtHtml = new System.Windows.Forms.TextBox();
            this.btnConvert = new System.Windows.Forms.Button();
            this.pbRectangle = new System.Windows.Forms.PictureBox();
            this.txtDimX = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDimY = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbRectangle)).BeginInit();
            this.SuspendLayout();
            // 
            // txtHtml
            // 
            this.txtHtml.Location = new System.Drawing.Point(12, 12);
            this.txtHtml.Multiline = true;
            this.txtHtml.Name = "txtHtml";
            this.txtHtml.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtHtml.Size = new System.Drawing.Size(565, 356);
            this.txtHtml.TabIndex = 0;
            this.txtHtml.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtHtml_KeyDown);
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(12, 379);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(101, 23);
            this.btnConvert.TabIndex = 1;
            this.btnConvert.Text = "Convert to PDF";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // pbRectangle
            // 
            this.pbRectangle.Location = new System.Drawing.Point(669, 48);
            this.pbRectangle.Name = "pbRectangle";
            this.pbRectangle.Size = new System.Drawing.Size(260, 226);
            this.pbRectangle.TabIndex = 3;
            this.pbRectangle.TabStop = false;
            // 
            // txtDimX
            // 
            this.txtDimX.Location = new System.Drawing.Point(226, 381);
            this.txtDimX.Name = "txtDimX";
            this.txtDimX.Size = new System.Drawing.Size(100, 20);
            this.txtDimX.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(185, 384);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Dim-X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(347, 386);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Dim-Y";
            // 
            // txtDimY
            // 
            this.txtDimY.Location = new System.Drawing.Point(388, 381);
            this.txtDimY.Name = "txtDimY";
            this.txtDimY.Size = new System.Drawing.Size(100, 20);
            this.txtDimY.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 414);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDimY);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDimX);
            this.Controls.Add(this.pbRectangle);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.txtHtml);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SimpleHtmlToPdfConverter";
            ((System.ComponentModel.ISupportInitialize)(this.pbRectangle)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtHtml;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.PictureBox pbRectangle;
        private System.Windows.Forms.TextBox txtDimX;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDimY;
    }
}

