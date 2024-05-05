namespace QModReloadedGUI
{
    partial class FrmConfigEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmConfigEdit));
            this.TxtConfig = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // TxtConfig
            // 
            this.TxtConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtConfig.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtConfig.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtConfig.Location = new System.Drawing.Point(12, 12);
            this.TxtConfig.Name = "TxtConfig";
            this.TxtConfig.Size = new System.Drawing.Size(776, 426);
            this.TxtConfig.TabIndex = 0;
            this.TxtConfig.Text = "";
            this.TxtConfig.TextChanged += new System.EventHandler(this.TxtConfig_TextChanged);
            // 
            // FrmConfigEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.TxtConfig);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmConfigEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Config Edit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmConfigEdit_FormClosing);
            this.Load += new System.EventHandler(this.FrmConfigEdit_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox TxtConfig;
    }
}