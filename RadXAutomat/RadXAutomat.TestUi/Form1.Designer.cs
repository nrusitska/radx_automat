namespace RadXAutomat.TestUi
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
        protected override void Dispose(bool disposing)
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
            this.connectButton = new System.Windows.Forms.Button();
            this.tagIdLabel = new System.Windows.Forms.Label();
            this.dongleGroupBox = new System.Windows.Forms.GroupBox();
            this.radsLabel = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.dongleGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(10, 11);
            this.connectButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(56, 19);
            this.connectButton.TabIndex = 0;
            this.connectButton.Text = "connect";
            this.connectButton.UseVisualStyleBackColor = true;
            // 
            // tagIdLabel
            // 
            this.tagIdLabel.AutoSize = true;
            this.tagIdLabel.Location = new System.Drawing.Point(71, 15);
            this.tagIdLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.tagIdLabel.Name = "tagIdLabel";
            this.tagIdLabel.Size = new System.Drawing.Size(16, 13);
            this.tagIdLabel.TabIndex = 1;
            this.tagIdLabel.Text = "...";
            // 
            // dongleGroupBox
            // 
            this.dongleGroupBox.Controls.Add(this.radsLabel);
            this.dongleGroupBox.Location = new System.Drawing.Point(10, 35);
            this.dongleGroupBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dongleGroupBox.Name = "dongleGroupBox";
            this.dongleGroupBox.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dongleGroupBox.Size = new System.Drawing.Size(193, 161);
            this.dongleGroupBox.TabIndex = 2;
            this.dongleGroupBox.TabStop = false;
            this.dongleGroupBox.Text = "Dongle";
            // 
            // radsLabel
            // 
            this.radsLabel.AutoSize = true;
            this.radsLabel.Location = new System.Drawing.Point(5, 18);
            this.radsLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.radsLabel.Name = "radsLabel";
            this.radsLabel.Size = new System.Drawing.Size(35, 13);
            this.radsLabel.TabIndex = 0;
            this.radsLabel.Text = "Rads:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(125, 11);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "sound test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(212, 206);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dongleGroupBox);
            this.Controls.Add(this.tagIdLabel);
            this.Controls.Add(this.connectButton);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.dongleGroupBox.ResumeLayout(false);
            this.dongleGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Label tagIdLabel;
        private System.Windows.Forms.GroupBox dongleGroupBox;
        private System.Windows.Forms.Label radsLabel;
        private System.Windows.Forms.Button button1;
    }
}

