namespace GYKResPatcher
{
    partial class FrmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.BtnApplyResPatch = new System.Windows.Forms.Button();
            this.BtnPatchIntros = new System.Windows.Forms.Button();
            this.LblCW = new System.Windows.Forms.Label();
            this.LblCH = new System.Windows.Forms.Label();
            this.TxtCW = new System.Windows.Forms.TextBox();
            this.TxtCH = new System.Windows.Forms.TextBox();
            this.TxtRH = new System.Windows.Forms.TextBox();
            this.TxtRW = new System.Windows.Forms.TextBox();
            this.LblRH = new System.Windows.Forms.Label();
            this.LblRW = new System.Windows.Forms.Label();
            this.PicBox = new System.Windows.Forms.PictureBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.LblUrl = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.PicBox)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnApplyResPatch
            // 
            this.BtnApplyResPatch.Location = new System.Drawing.Point(12, 116);
            this.BtnApplyResPatch.Name = "BtnApplyResPatch";
            this.BtnApplyResPatch.Size = new System.Drawing.Size(75, 23);
            this.BtnApplyResPatch.TabIndex = 0;
            this.BtnApplyResPatch.Text = "Patch &Res";
            this.BtnApplyResPatch.UseVisualStyleBackColor = true;
            this.BtnApplyResPatch.Click += new System.EventHandler(this.BtnApplyResPatch_Click);
            // 
            // BtnPatchIntros
            // 
            this.BtnPatchIntros.Location = new System.Drawing.Point(93, 116);
            this.BtnPatchIntros.Name = "BtnPatchIntros";
            this.BtnPatchIntros.Size = new System.Drawing.Size(97, 23);
            this.BtnPatchIntros.TabIndex = 1;
            this.BtnPatchIntros.Text = "Patch &Intros";
            this.BtnPatchIntros.UseVisualStyleBackColor = true;
            this.BtnPatchIntros.Click += new System.EventHandler(this.BtnPatchIntros_Click);
            // 
            // LblCW
            // 
            this.LblCW.AutoSize = true;
            this.LblCW.Location = new System.Drawing.Point(12, 9);
            this.LblCW.Name = "LblCW";
            this.LblCW.Size = new System.Drawing.Size(82, 15);
            this.LblCW.TabIndex = 2;
            this.LblCW.Text = "Current Width";
            // 
            // LblCH
            // 
            this.LblCH.AutoSize = true;
            this.LblCH.Location = new System.Drawing.Point(118, 9);
            this.LblCH.Name = "LblCH";
            this.LblCH.Size = new System.Drawing.Size(86, 15);
            this.LblCH.TabIndex = 3;
            this.LblCH.Text = "Current Height";
            // 
            // TxtCW
            // 
            this.TxtCW.Location = new System.Drawing.Point(12, 27);
            this.TxtCW.Name = "TxtCW";
            this.TxtCW.ReadOnly = true;
            this.TxtCW.Size = new System.Drawing.Size(100, 23);
            this.TxtCW.TabIndex = 4;
            // 
            // TxtCH
            // 
            this.TxtCH.Location = new System.Drawing.Point(118, 27);
            this.TxtCH.Name = "TxtCH";
            this.TxtCH.ReadOnly = true;
            this.TxtCH.Size = new System.Drawing.Size(100, 23);
            this.TxtCH.TabIndex = 5;
            // 
            // TxtRH
            // 
            this.TxtRH.Location = new System.Drawing.Point(118, 76);
            this.TxtRH.Name = "TxtRH";
            this.TxtRH.Size = new System.Drawing.Size(100, 23);
            this.TxtRH.TabIndex = 9;
            // 
            // TxtRW
            // 
            this.TxtRW.Location = new System.Drawing.Point(12, 76);
            this.TxtRW.Name = "TxtRW";
            this.TxtRW.Size = new System.Drawing.Size(100, 23);
            this.TxtRW.TabIndex = 8;
            // 
            // LblRH
            // 
            this.LblRH.AutoSize = true;
            this.LblRH.Location = new System.Drawing.Point(118, 58);
            this.LblRH.Name = "LblRH";
            this.LblRH.Size = new System.Drawing.Size(101, 15);
            this.LblRH.TabIndex = 7;
            this.LblRH.Text = "Requested Height";
            // 
            // LblRW
            // 
            this.LblRW.AutoSize = true;
            this.LblRW.Location = new System.Drawing.Point(12, 58);
            this.LblRW.Name = "LblRW";
            this.LblRW.Size = new System.Drawing.Size(97, 15);
            this.LblRW.TabIndex = 6;
            this.LblRW.Text = "Requested Width";
            // 
            // PicBox
            // 
            this.PicBox.Image = ((System.Drawing.Image)(resources.GetObject("PicBox.Image")));
            this.PicBox.Location = new System.Drawing.Point(224, 9);
            this.PicBox.Name = "PicBox";
            this.PicBox.Size = new System.Drawing.Size(130, 130);
            this.PicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PicBox.TabIndex = 10;
            this.PicBox.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LblUrl});
            this.statusStrip1.Location = new System.Drawing.Point(0, 157);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(365, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // LblUrl
            // 
            this.LblUrl.IsLink = true;
            this.LblUrl.Name = "LblUrl";
            this.LblUrl.Size = new System.Drawing.Size(259, 17);
            this.LblUrl.Text = "https://github.com/p1xel8ted/GraveyardKeeper";
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 179);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.PicBox);
            this.Controls.Add(this.TxtRH);
            this.Controls.Add(this.TxtRW);
            this.Controls.Add(this.LblRH);
            this.Controls.Add(this.LblRW);
            this.Controls.Add(this.TxtCH);
            this.Controls.Add(this.TxtCW);
            this.Controls.Add(this.LblCH);
            this.Controls.Add(this.LblCW);
            this.Controls.Add(this.BtnPatchIntros);
            this.Controls.Add(this.BtnApplyResPatch);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmMain";
            this.Text = "GYK Res/Intro Patcher";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PicBox)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button BtnApplyResPatch;
        private Button BtnPatchIntros;
        private Label LblCW;
        private Label LblCH;
        private TextBox TxtCW;
        private TextBox TxtCH;
        private TextBox TxtRH;
        private TextBox TxtRW;
        private Label LblRH;
        private Label LblRW;
        private PictureBox PicBox;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel LblUrl;
    }
}