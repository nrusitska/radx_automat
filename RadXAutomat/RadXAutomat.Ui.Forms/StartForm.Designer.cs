﻿namespace RadXAutomat.Ui.Forms
{
    partial class StartForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartForm));
            this.hackTextPanel = new System.Windows.Forms.Label();
            this.hackPanel = new System.Windows.Forms.Panel();
            this.skullPanel = new System.Windows.Forms.Panel();
            this.hackPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // hackTextPanel
            // 
            this.hackTextPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hackTextPanel.AutoSize = true;
            this.hackTextPanel.BackColor = System.Drawing.Color.Transparent;
            this.hackTextPanel.Font = new System.Drawing.Font("Courier New", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hackTextPanel.ForeColor = System.Drawing.Color.Green;
            this.hackTextPanel.Location = new System.Drawing.Point(9, 491);
            this.hackTextPanel.Margin = new System.Windows.Forms.Padding(0);
            this.hackTextPanel.Name = "hackTextPanel";
            this.hackTextPanel.Size = new System.Drawing.Size(348, 27);
            this.hackTextPanel.TabIndex = 0;
            this.hackTextPanel.Text = "this is the hacking text";
            // 
            // hackPanel
            // 
            this.hackPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hackPanel.BackColor = System.Drawing.Color.Transparent;
            this.hackPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("hackPanel.BackgroundImage")));
            this.hackPanel.Controls.Add(this.skullPanel);
            this.hackPanel.Controls.Add(this.hackTextPanel);
            this.hackPanel.ForeColor = System.Drawing.SystemColors.Control;
            this.hackPanel.Location = new System.Drawing.Point(0, 0);
            this.hackPanel.Margin = new System.Windows.Forms.Padding(0);
            this.hackPanel.Name = "hackPanel";
            this.hackPanel.Size = new System.Drawing.Size(1448, 800);
            this.hackPanel.TabIndex = 1;
            // 
            // skullPanel
            // 
            this.skullPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.skullPanel.BackColor = System.Drawing.Color.Transparent;
            this.skullPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("skullPanel.BackgroundImage")));
            this.skullPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.skullPanel.Location = new System.Drawing.Point(541, 12);
            this.skullPanel.Name = "skullPanel";
            this.skullPanel.Size = new System.Drawing.Size(357, 743);
            this.skullPanel.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(1400, 800);
            this.Controls.Add(this.hackPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Football";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.hackPanel.ResumeLayout(false);
            this.hackPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label hackTextPanel;
        private System.Windows.Forms.Panel hackPanel;
        private System.Windows.Forms.Panel skullPanel;
    }
}

