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
            this.dongleGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(13, 13);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 23);
            this.connectButton.TabIndex = 0;
            this.connectButton.Text = "connect";
            this.connectButton.UseVisualStyleBackColor = true;
            // 
            // tagIdLabel
            // 
            this.tagIdLabel.AutoSize = true;
            this.tagIdLabel.Location = new System.Drawing.Point(95, 18);
            this.tagIdLabel.Name = "tagIdLabel";
            this.tagIdLabel.Size = new System.Drawing.Size(20, 17);
            this.tagIdLabel.TabIndex = 1;
            this.tagIdLabel.Text = "...";
            // 
            // dongleGroupBox
            // 
            this.dongleGroupBox.Controls.Add(this.radsLabel);
            this.dongleGroupBox.Location = new System.Drawing.Point(13, 43);
            this.dongleGroupBox.Name = "dongleGroupBox";
            this.dongleGroupBox.Size = new System.Drawing.Size(257, 198);
            this.dongleGroupBox.TabIndex = 2;
            this.dongleGroupBox.TabStop = false;
            this.dongleGroupBox.Text = "Dongle";
            // 
            // radsLabel
            // 
            this.radsLabel.AutoSize = true;
            this.radsLabel.Location = new System.Drawing.Point(7, 22);
            this.radsLabel.Name = "radsLabel";
            this.radsLabel.Size = new System.Drawing.Size(45, 17);
            this.radsLabel.TabIndex = 0;
            this.radsLabel.Text = "Rads:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Controls.Add(this.dongleGroupBox);
            this.Controls.Add(this.tagIdLabel);
            this.Controls.Add(this.connectButton);
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
    }
}

