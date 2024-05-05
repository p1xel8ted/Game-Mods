namespace QModReloadedGUI
{
    partial class FrmOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmOptions));
            this.ChkUpdateOnStartup = new System.Windows.Forms.CheckBox();
            this.ChkLaunchExeDirectly = new System.Windows.Forms.CheckBox();
            this.GrpMisc = new System.Windows.Forms.GroupBox();
            this.ChkHideDisabled = new System.Windows.Forms.CheckBox();
            this.ChkMinimizeTray = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ChkOverride = new System.Windows.Forms.CheckBox();
            this.BtnBrowse = new System.Windows.Forms.Button();
            this.ChkEditor = new System.Windows.Forms.CheckBox();
            this.TxtEditor = new System.Windows.Forms.TextBox();
            this.DlgEditor = new System.Windows.Forms.OpenFileDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.LblInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.GrpUpdates = new System.Windows.Forms.GroupBox();
            this.ChkRedownload = new System.Windows.Forms.CheckBox();
            this.ChkDownloadDirectory = new System.Windows.Forms.CheckBox();
            this.BtnBrowseUpdate = new System.Windows.Forms.Button();
            this.TxtDownloadDir = new System.Windows.Forms.TextBox();
            this.ChkRemoveDownloadedFile = new System.Windows.Forms.CheckBox();
            this.DlgDownload = new System.Windows.Forms.FolderBrowserDialog();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.GrpMisc.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.GrpUpdates.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // ChkUpdateOnStartup
            // 
            this.ChkUpdateOnStartup.AutoSize = true;
            this.ChkUpdateOnStartup.Enabled = false;
            this.ChkUpdateOnStartup.Location = new System.Drawing.Point(6, 19);
            this.ChkUpdateOnStartup.Name = "ChkUpdateOnStartup";
            this.ChkUpdateOnStartup.Size = new System.Drawing.Size(163, 17);
            this.ChkUpdateOnStartup.TabIndex = 0;
            this.ChkUpdateOnStartup.Text = "Check for updates on startup";
            this.ChkUpdateOnStartup.UseVisualStyleBackColor = true;
            this.ChkUpdateOnStartup.CheckedChanged += new System.EventHandler(this.ChkUpdateOnStartup_CheckedChanged);
            this.ChkUpdateOnStartup.MouseHover += new System.EventHandler(this.ChkUpdateOnStartup_MouseHover);
            // 
            // ChkLaunchExeDirectly
            // 
            this.ChkLaunchExeDirectly.AutoSize = true;
            this.ChkLaunchExeDirectly.Location = new System.Drawing.Point(6, 42);
            this.ChkLaunchExeDirectly.Name = "ChkLaunchExeDirectly";
            this.ChkLaunchExeDirectly.Size = new System.Drawing.Size(131, 18);
            this.ChkLaunchExeDirectly.TabIndex = 39;
            this.ChkLaunchExeDirectly.Text = "Launch game directly";
            this.ChkLaunchExeDirectly.UseCompatibleTextRendering = true;
            this.ChkLaunchExeDirectly.UseVisualStyleBackColor = true;
            this.ChkLaunchExeDirectly.CheckedChanged += new System.EventHandler(this.ChkLaunchExeDirectly_CheckedChanged);
            this.ChkLaunchExeDirectly.MouseHover += new System.EventHandler(this.ChkLaunchExeDirectly_MouseHover);
            // 
            // GrpMisc
            // 
            this.GrpMisc.Controls.Add(this.ChkHideDisabled);
            this.GrpMisc.Controls.Add(this.ChkMinimizeTray);
            this.GrpMisc.Controls.Add(this.ChkLaunchExeDirectly);
            this.GrpMisc.Location = new System.Drawing.Point(12, 12);
            this.GrpMisc.Name = "GrpMisc";
            this.GrpMisc.Size = new System.Drawing.Size(319, 74);
            this.GrpMisc.TabIndex = 40;
            this.GrpMisc.TabStop = false;
            this.GrpMisc.Text = "Misc. Options";
            // 
            // ChkHideDisabled
            // 
            this.ChkHideDisabled.AutoSize = true;
            this.ChkHideDisabled.Location = new System.Drawing.Point(140, 19);
            this.ChkHideDisabled.Name = "ChkHideDisabled";
            this.ChkHideDisabled.Size = new System.Drawing.Size(167, 17);
            this.ChkHideDisabled.TabIndex = 40;
            this.ChkHideDisabled.Text = "Hide disabled mods by default";
            this.ChkHideDisabled.UseVisualStyleBackColor = true;
            this.ChkHideDisabled.CheckedChanged += new System.EventHandler(this.ChkHideDisabled_CheckedChanged);
            this.ChkHideDisabled.MouseHover += new System.EventHandler(this.ChkHideDisabled_MouseHover);
            // 
            // ChkMinimizeTray
            // 
            this.ChkMinimizeTray.AutoSize = true;
            this.ChkMinimizeTray.Location = new System.Drawing.Point(6, 19);
            this.ChkMinimizeTray.Name = "ChkMinimizeTray";
            this.ChkMinimizeTray.Size = new System.Drawing.Size(98, 17);
            this.ChkMinimizeTray.TabIndex = 1;
            this.ChkMinimizeTray.Text = "Minimize to tray";
            this.ChkMinimizeTray.UseVisualStyleBackColor = true;
            this.ChkMinimizeTray.CheckedChanged += new System.EventHandler(this.ChkMinimizeTray_CheckedChanged);
            this.ChkMinimizeTray.MouseHover += new System.EventHandler(this.ChkMinimizeTray_MouseHover);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ChkOverride);
            this.groupBox2.Controls.Add(this.BtnBrowse);
            this.groupBox2.Controls.Add(this.ChkEditor);
            this.groupBox2.Controls.Add(this.TxtEditor);
            this.groupBox2.Location = new System.Drawing.Point(12, 92);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(395, 94);
            this.groupBox2.TabIndex = 41;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "File Viewer";
            // 
            // ChkOverride
            // 
            this.ChkOverride.AutoSize = true;
            this.ChkOverride.Location = new System.Drawing.Point(6, 42);
            this.ChkOverride.Name = "ChkOverride";
            this.ChkOverride.Size = new System.Drawing.Size(160, 17);
            this.ChkOverride.TabIndex = 3;
            this.ChkOverride.Text = "Override built-in config editor";
            this.ChkOverride.UseVisualStyleBackColor = true;
            this.ChkOverride.CheckedChanged += new System.EventHandler(this.ChkOverride_CheckedChanged);
            this.ChkOverride.MouseHover += new System.EventHandler(this.ChkOverride_MouseHover);
            // 
            // BtnBrowse
            // 
            this.BtnBrowse.Image = global::QModReloadedGUI.Properties.Resources.folder_open;
            this.BtnBrowse.Location = new System.Drawing.Point(314, 36);
            this.BtnBrowse.Name = "BtnBrowse";
            this.BtnBrowse.Size = new System.Drawing.Size(75, 23);
            this.BtnBrowse.TabIndex = 2;
            this.BtnBrowse.Text = "&Browse";
            this.BtnBrowse.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnBrowse.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnBrowse.UseVisualStyleBackColor = true;
            this.BtnBrowse.Click += new System.EventHandler(this.BtnBrowse_Click);
            // 
            // ChkEditor
            // 
            this.ChkEditor.AutoSize = true;
            this.ChkEditor.Location = new System.Drawing.Point(6, 19);
            this.ChkEditor.Name = "ChkEditor";
            this.ChkEditor.Size = new System.Drawing.Size(190, 17);
            this.ChkEditor.TabIndex = 1;
            this.ChkEditor.Text = "Use preferred editor for log viewing";
            this.ChkEditor.UseVisualStyleBackColor = true;
            this.ChkEditor.CheckedChanged += new System.EventHandler(this.ChkEditor_CheckedChanged);
            this.ChkEditor.MouseEnter += new System.EventHandler(this.ChkEditor_MouseEnter);
            // 
            // TxtEditor
            // 
            this.TxtEditor.Location = new System.Drawing.Point(6, 65);
            this.TxtEditor.Name = "TxtEditor";
            this.TxtEditor.ReadOnly = true;
            this.TxtEditor.Size = new System.Drawing.Size(383, 20);
            this.TxtEditor.TabIndex = 0;
            // 
            // DlgEditor
            // 
            this.DlgEditor.Filter = "Executables|*.exe";
            this.DlgEditor.Title = "Choose Preferred Editor Executable";
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LblInfo});
            this.statusStrip1.Location = new System.Drawing.Point(0, 341);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(418, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.Stretch = false;
            this.statusStrip1.TabIndex = 42;
            // 
            // LblInfo
            // 
            this.LblInfo.Name = "LblInfo";
            this.LblInfo.Size = new System.Drawing.Size(0, 17);
            // 
            // GrpUpdates
            // 
            this.GrpUpdates.Controls.Add(this.ChkRedownload);
            this.GrpUpdates.Controls.Add(this.ChkDownloadDirectory);
            this.GrpUpdates.Controls.Add(this.BtnBrowseUpdate);
            this.GrpUpdates.Controls.Add(this.TxtDownloadDir);
            this.GrpUpdates.Controls.Add(this.ChkRemoveDownloadedFile);
            this.GrpUpdates.Controls.Add(this.ChkUpdateOnStartup);
            this.GrpUpdates.Location = new System.Drawing.Point(12, 192);
            this.GrpUpdates.Name = "GrpUpdates";
            this.GrpUpdates.Size = new System.Drawing.Size(395, 141);
            this.GrpUpdates.TabIndex = 43;
            this.GrpUpdates.TabStop = false;
            this.GrpUpdates.Text = "Updates";
            // 
            // ChkRedownload
            // 
            this.ChkRedownload.AutoSize = true;
            this.ChkRedownload.Enabled = false;
            this.ChkRedownload.Location = new System.Drawing.Point(6, 88);
            this.ChkRedownload.Name = "ChkRedownload";
            this.ChkRedownload.Size = new System.Drawing.Size(161, 17);
            this.ChkRedownload.TabIndex = 5;
            this.ChkRedownload.Text = "Always re-download updates";
            this.ChkRedownload.UseVisualStyleBackColor = true;
            this.ChkRedownload.CheckedChanged += new System.EventHandler(this.ChkRedownload_CheckedChanged);
            this.ChkRedownload.MouseHover += new System.EventHandler(this.ChkRedownload_MouseHover);
            // 
            // ChkDownloadDirectory
            // 
            this.ChkDownloadDirectory.AutoSize = true;
            this.ChkDownloadDirectory.Enabled = false;
            this.ChkDownloadDirectory.Location = new System.Drawing.Point(6, 65);
            this.ChkDownloadDirectory.Name = "ChkDownloadDirectory";
            this.ChkDownloadDirectory.Size = new System.Drawing.Size(174, 17);
            this.ChkDownloadDirectory.TabIndex = 4;
            this.ChkDownloadDirectory.Text = "Use custom download directory";
            this.ChkDownloadDirectory.UseVisualStyleBackColor = true;
            this.ChkDownloadDirectory.CheckedChanged += new System.EventHandler(this.ChkDownloadDirectory_CheckedChanged);
            this.ChkDownloadDirectory.MouseHover += new System.EventHandler(this.ChkDownloadDirectory_MouseHover);
            // 
            // BtnBrowseUpdate
            // 
            this.BtnBrowseUpdate.Enabled = false;
            this.BtnBrowseUpdate.Image = global::QModReloadedGUI.Properties.Resources.folder_open;
            this.BtnBrowseUpdate.Location = new System.Drawing.Point(314, 84);
            this.BtnBrowseUpdate.Name = "BtnBrowseUpdate";
            this.BtnBrowseUpdate.Size = new System.Drawing.Size(75, 23);
            this.BtnBrowseUpdate.TabIndex = 3;
            this.BtnBrowseUpdate.Text = "&Browse";
            this.BtnBrowseUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnBrowseUpdate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnBrowseUpdate.UseVisualStyleBackColor = true;
            this.BtnBrowseUpdate.Click += new System.EventHandler(this.BtnBrowseUpdate_Click);
            // 
            // TxtDownloadDir
            // 
            this.TxtDownloadDir.Enabled = false;
            this.TxtDownloadDir.Location = new System.Drawing.Point(6, 111);
            this.TxtDownloadDir.Name = "TxtDownloadDir";
            this.TxtDownloadDir.ReadOnly = true;
            this.TxtDownloadDir.Size = new System.Drawing.Size(383, 20);
            this.TxtDownloadDir.TabIndex = 2;
            // 
            // ChkRemoveDownloadedFile
            // 
            this.ChkRemoveDownloadedFile.AutoSize = true;
            this.ChkRemoveDownloadedFile.Enabled = false;
            this.ChkRemoveDownloadedFile.Location = new System.Drawing.Point(6, 42);
            this.ChkRemoveDownloadedFile.Name = "ChkRemoveDownloadedFile";
            this.ChkRemoveDownloadedFile.Size = new System.Drawing.Size(159, 17);
            this.ChkRemoveDownloadedFile.TabIndex = 1;
            this.ChkRemoveDownloadedFile.Text = "Delete downloaded updates";
            this.ChkRemoveDownloadedFile.UseVisualStyleBackColor = true;
            this.ChkRemoveDownloadedFile.CheckedChanged += new System.EventHandler(this.ChkRemoveDownloadedFile_CheckedChanged);
            this.ChkRemoveDownloadedFile.MouseHover += new System.EventHandler(this.ChkRemoveDownloadedFile_MouseHover);
            // 
            // DlgDownload
            // 
            this.DlgDownload.Description = "Select a location to store mod updates";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::QModReloadedGUI.Properties.Resources.gerry;
            this.pictureBox1.Location = new System.Drawing.Point(337, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(70, 75);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 44;
            this.pictureBox1.TabStop = false;
            // 
            // FrmOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 363);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.GrpUpdates);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.GrpMisc);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmOptions";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmOptions_FormClosing);
            this.Load += new System.EventHandler(this.FrmOptions_Load);
            this.MouseEnter += new System.EventHandler(this.FrmOptions_MouseEnter);
            this.GrpMisc.ResumeLayout(false);
            this.GrpMisc.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.GrpUpdates.ResumeLayout(false);
            this.GrpUpdates.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox ChkUpdateOnStartup;
        private System.Windows.Forms.CheckBox ChkLaunchExeDirectly;
        private System.Windows.Forms.GroupBox GrpMisc;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button BtnBrowse;
        private System.Windows.Forms.CheckBox ChkEditor;
        private System.Windows.Forms.TextBox TxtEditor;
        private System.Windows.Forms.CheckBox ChkOverride;
        private System.Windows.Forms.OpenFileDialog DlgEditor;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel LblInfo;
        private System.Windows.Forms.GroupBox GrpUpdates;
        private System.Windows.Forms.CheckBox ChkMinimizeTray;
        private System.Windows.Forms.CheckBox ChkRemoveDownloadedFile;
        private System.Windows.Forms.CheckBox ChkDownloadDirectory;
        private System.Windows.Forms.Button BtnBrowseUpdate;
        private System.Windows.Forms.TextBox TxtDownloadDir;
        private System.Windows.Forms.FolderBrowserDialog DlgDownload;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox ChkRedownload;
        private System.Windows.Forms.CheckBox ChkHideDisabled;
    }
}