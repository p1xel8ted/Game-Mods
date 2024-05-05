using System.ComponentModel;
using System.Windows.Forms;

namespace QModReloadedGUI
{
    partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.TxtGameLocation = new System.Windows.Forms.TextBox();
            this.TxtModFolderLocation = new System.Windows.Forms.TextBox();
            this.LblGameLocation = new System.Windows.Forms.Label();
            this.LblModFolderLocation = new System.Windows.Forms.Label();
            this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.BtnRestore = new System.Windows.Forms.Button();
            this.BtnOpenLog = new System.Windows.Forms.Button();
            this.BtnOpenModDir = new System.Windows.Forms.Button();
            this.BtnOpenGameDir = new System.Windows.Forms.Button();
            this.BtnRemoveIntros = new System.Windows.Forms.Button();
            this.BtnRefresh = new System.Windows.Forms.Button();
            this.BtnRemovePatch = new System.Windows.Forms.Button();
            this.BtnRunGame = new System.Windows.Forms.PictureBox();
            this.BtnRemove = new System.Windows.Forms.Button();
            this.BtnAddMod = new System.Windows.Forms.Button();
            this.BtnPatch = new System.Windows.Forms.Button();
            this.DgvMods = new System.Windows.Forms.DataGridView();
            this.ChEnabledBox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ChOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChMod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChAuthor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChConfig = new System.Windows.Forms.DataGridViewLinkColumn();
            this.ChID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modListCtxMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ModMenuName = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.openConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeModToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UpdateDivider = new System.Windows.Forms.ToolStripSeparator();
            this.ModMenuUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.ModMenuUpdateAll = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checklistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStrip = new System.Windows.Forms.ToolStrip();
            this.LblPatched = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.LblIntroPatched = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.LblHelper = new System.Windows.Forms.ToolStripLabel();
            this.ErrorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.LblErrors = new System.Windows.Forms.ToolStripLabel();
            this.UpdateProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.LblNexusRequests = new System.Windows.Forms.ToolStripLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.nexusPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.openSaveDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.checklistToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.modifyResolutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BtnLaunchModless = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.firstRunInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.nexusAPIKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DlgFile = new System.Windows.Forms.OpenFileDialog();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayIconCtxMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.restoreWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.launchGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openmModDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openGameDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.ChkToggleMods = new System.Windows.Forms.CheckBox();
            this.DgvLog = new System.Windows.Forms.DataGridView();
            this.ChTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TxtFilter = new System.Windows.Forms.TextBox();
            this.LblFilter = new System.Windows.Forms.Label();
            this.ChkHideDisabledMods = new System.Windows.Forms.CheckBox();
            this.LblLaunch = new System.Windows.Forms.Label();
            this.BtnKofi = new System.Windows.Forms.PictureBox();
            this.LblGameVersion = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            ((System.ComponentModel.ISupportInitialize)(this.BtnRunGame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMods)).BeginInit();
            this.modListCtxMenu.SuspendLayout();
            this.ToolStrip.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.trayIconCtxMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BtnKofi)).BeginInit();
            this.SuspendLayout();
            // 
            // TxtGameLocation
            // 
            this.TxtGameLocation.Location = new System.Drawing.Point(12, 37);
            this.TxtGameLocation.Name = "TxtGameLocation";
            this.TxtGameLocation.ReadOnly = true;
            this.TxtGameLocation.Size = new System.Drawing.Size(916, 20);
            this.TxtGameLocation.TabIndex = 0;
            this.ToolTip.SetToolTip(this.TxtGameLocation, "Directory that contains Graveyard Keeper.exe");
            // 
            // TxtModFolderLocation
            // 
            this.TxtModFolderLocation.Location = new System.Drawing.Point(354, 64);
            this.TxtModFolderLocation.Name = "TxtModFolderLocation";
            this.TxtModFolderLocation.ReadOnly = true;
            this.TxtModFolderLocation.Size = new System.Drawing.Size(471, 20);
            this.TxtModFolderLocation.TabIndex = 1;
            this.ToolTip.SetToolTip(this.TxtModFolderLocation, "This cannot be changed due to the nature of QMods.");
            // 
            // LblGameLocation
            // 
            this.LblGameLocation.AutoSize = true;
            this.LblGameLocation.Location = new System.Drawing.Point(12, 13);
            this.LblGameLocation.Name = "LblGameLocation";
            this.LblGameLocation.Size = new System.Drawing.Size(81, 17);
            this.LblGameLocation.TabIndex = 3;
            this.LblGameLocation.Text = "Game Location";
            this.LblGameLocation.UseCompatibleTextRendering = true;
            // 
            // LblModFolderLocation
            // 
            this.LblModFolderLocation.AutoSize = true;
            this.LblModFolderLocation.Location = new System.Drawing.Point(241, 69);
            this.LblModFolderLocation.Name = "LblModFolderLocation";
            this.LblModFolderLocation.Size = new System.Drawing.Size(107, 17);
            this.LblModFolderLocation.TabIndex = 4;
            this.LblModFolderLocation.Text = "Mod Folder Location";
            this.LblModFolderLocation.UseCompatibleTextRendering = true;
            // 
            // BtnRestore
            // 
            this.BtnRestore.Image = global::QModReloadedGUI.Properties.Resources.save;
            this.BtnRestore.Location = new System.Drawing.Point(819, 478);
            this.BtnRestore.Name = "BtnRestore";
            this.BtnRestore.Size = new System.Drawing.Size(120, 25);
            this.BtnRestore.TabIndex = 36;
            this.BtnRestore.Text = "Restore Backup";
            this.BtnRestore.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnRestore.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ToolTip.SetToolTip(this.BtnRestore, "Restores backed up Assembly-CSharp.dll if it exists.");
            this.BtnRestore.UseCompatibleTextRendering = true;
            this.BtnRestore.UseVisualStyleBackColor = true;
            this.BtnRestore.Click += new System.EventHandler(this.BtnRestore_Click);
            // 
            // BtnOpenLog
            // 
            this.BtnOpenLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnOpenLog.Image = global::QModReloadedGUI.Properties.Resources.comments;
            this.BtnOpenLog.Location = new System.Drawing.Point(722, 478);
            this.BtnOpenLog.Name = "BtnOpenLog";
            this.BtnOpenLog.Size = new System.Drawing.Size(91, 25);
            this.BtnOpenLog.TabIndex = 29;
            this.BtnOpenLog.Text = "Open &Logs";
            this.BtnOpenLog.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnOpenLog.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ToolTip.SetToolTip(this.BtnOpenLog, "Open the log file in your default editor.");
            this.BtnOpenLog.UseVisualStyleBackColor = true;
            this.BtnOpenLog.Click += new System.EventHandler(this.BtnOpenLog_Click);
            // 
            // BtnOpenModDir
            // 
            this.BtnOpenModDir.Image = global::QModReloadedGUI.Properties.Resources.folder_files;
            this.BtnOpenModDir.Location = new System.Drawing.Point(831, 61);
            this.BtnOpenModDir.Name = "BtnOpenModDir";
            this.BtnOpenModDir.Size = new System.Drawing.Size(108, 25);
            this.BtnOpenModDir.TabIndex = 28;
            this.BtnOpenModDir.Text = "Open M&od Dir";
            this.BtnOpenModDir.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnOpenModDir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ToolTip.SetToolTip(this.BtnOpenModDir, "Open the mod directory in Explorer");
            this.BtnOpenModDir.UseCompatibleTextRendering = true;
            this.BtnOpenModDir.UseVisualStyleBackColor = true;
            this.BtnOpenModDir.Click += new System.EventHandler(this.BtnOpenModDir_Click);
            // 
            // BtnOpenGameDir
            // 
            this.BtnOpenGameDir.Image = global::QModReloadedGUI.Properties.Resources.folder_open;
            this.BtnOpenGameDir.Location = new System.Drawing.Point(941, 61);
            this.BtnOpenGameDir.Name = "BtnOpenGameDir";
            this.BtnOpenGameDir.Size = new System.Drawing.Size(121, 25);
            this.BtnOpenGameDir.TabIndex = 27;
            this.BtnOpenGameDir.Text = "Ope&n Game Dir";
            this.BtnOpenGameDir.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnOpenGameDir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ToolTip.SetToolTip(this.BtnOpenGameDir, "Open the game directory in Explorer");
            this.BtnOpenGameDir.UseVisualStyleBackColor = true;
            this.BtnOpenGameDir.Click += new System.EventHandler(this.BtnOpenGameDir_Click);
            // 
            // BtnRemoveIntros
            // 
            this.BtnRemoveIntros.Image = global::QModReloadedGUI.Properties.Resources.application;
            this.BtnRemoveIntros.Location = new System.Drawing.Point(945, 478);
            this.BtnRemoveIntros.Name = "BtnRemoveIntros";
            this.BtnRemoveIntros.Size = new System.Drawing.Size(117, 25);
            this.BtnRemoveIntros.TabIndex = 20;
            this.BtnRemoveIntros.Text = "Remove &Intros";
            this.BtnRemoveIntros.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnRemoveIntros.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ToolTip.SetToolTip(this.BtnRemoveIntros, "Removes intros (permanently).");
            this.BtnRemoveIntros.UseCompatibleTextRendering = true;
            this.BtnRemoveIntros.UseVisualStyleBackColor = true;
            this.BtnRemoveIntros.Click += new System.EventHandler(this.BtnRemoveIntros_Click);
            // 
            // BtnRefresh
            // 
            this.BtnRefresh.Image = global::QModReloadedGUI.Properties.Resources.search;
            this.BtnRefresh.Location = new System.Drawing.Point(201, 478);
            this.BtnRefresh.Name = "BtnRefresh";
            this.BtnRefresh.Size = new System.Drawing.Size(86, 25);
            this.BtnRefresh.TabIndex = 18;
            this.BtnRefresh.Text = "Re&fresh";
            this.BtnRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ToolTip.SetToolTip(this.BtnRefresh, "Click if you\'ve installed mods externally.");
            this.BtnRefresh.UseCompatibleTextRendering = true;
            this.BtnRefresh.UseVisualStyleBackColor = true;
            this.BtnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // BtnRemovePatch
            // 
            this.BtnRemovePatch.Image = global::QModReloadedGUI.Properties.Resources.minimize;
            this.BtnRemovePatch.Location = new System.Drawing.Point(608, 478);
            this.BtnRemovePatch.Margin = new System.Windows.Forms.Padding(1);
            this.BtnRemovePatch.Name = "BtnRemovePatch";
            this.BtnRemovePatch.Size = new System.Drawing.Size(110, 25);
            this.BtnRemovePatch.TabIndex = 17;
            this.BtnRemovePatch.Text = "Remove Pa&tch";
            this.BtnRemovePatch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnRemovePatch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ToolTip.SetToolTip(this.BtnRemovePatch, "Removes the mod patch only.");
            this.BtnRemovePatch.UseCompatibleTextRendering = true;
            this.BtnRemovePatch.UseVisualStyleBackColor = true;
            this.BtnRemovePatch.Click += new System.EventHandler(this.BtnRemovePatch_Click);
            // 
            // BtnRunGame
            // 
            this.BtnRunGame.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnRunGame.Image = ((System.Drawing.Image)(resources.GetObject("BtnRunGame.Image")));
            this.BtnRunGame.Location = new System.Drawing.Point(905, 509);
            this.BtnRunGame.Name = "BtnRunGame";
            this.BtnRunGame.Size = new System.Drawing.Size(157, 142);
            this.BtnRunGame.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.BtnRunGame.TabIndex = 16;
            this.BtnRunGame.TabStop = false;
            this.ToolTip.SetToolTip(this.BtnRunGame, "Click to launch Graveyard Keeper. Launches via Steam first, then by the EXE direc" +
        "tly if Steam fails for whatever reason.");
            this.BtnRunGame.Click += new System.EventHandler(this.BtnRunGame_Click);
            // 
            // BtnRemove
            // 
            this.BtnRemove.Image = global::QModReloadedGUI.Properties.Resources.action_delete;
            this.BtnRemove.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnRemove.Location = new System.Drawing.Point(95, 478);
            this.BtnRemove.Name = "BtnRemove";
            this.BtnRemove.Size = new System.Drawing.Size(100, 25);
            this.BtnRemove.TabIndex = 14;
            this.BtnRemove.Text = "&Remove Mod";
            this.BtnRemove.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnRemove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ToolTip.SetToolTip(this.BtnRemove, "Removes the selected mod(s).");
            this.BtnRemove.UseCompatibleTextRendering = true;
            this.BtnRemove.UseVisualStyleBackColor = true;
            this.BtnRemove.Click += new System.EventHandler(this.BtnRemove_Click);
            // 
            // BtnAddMod
            // 
            this.BtnAddMod.Image = global::QModReloadedGUI.Properties.Resources.action_add;
            this.BtnAddMod.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnAddMod.Location = new System.Drawing.Point(12, 478);
            this.BtnAddMod.Name = "BtnAddMod";
            this.BtnAddMod.Size = new System.Drawing.Size(77, 25);
            this.BtnAddMod.TabIndex = 13;
            this.BtnAddMod.Text = "A&dd Mod";
            this.BtnAddMod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnAddMod.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ToolTip.SetToolTip(this.BtnAddMod, "Adds a new mod.");
            this.BtnAddMod.UseCompatibleTextRendering = true;
            this.BtnAddMod.UseVisualStyleBackColor = true;
            this.BtnAddMod.Click += new System.EventHandler(this.BtnAddMod_Click);
            // 
            // BtnPatch
            // 
            this.BtnPatch.Image = global::QModReloadedGUI.Properties.Resources.maximize;
            this.BtnPatch.Location = new System.Drawing.Point(508, 478);
            this.BtnPatch.Margin = new System.Windows.Forms.Padding(1);
            this.BtnPatch.Name = "BtnPatch";
            this.BtnPatch.Size = new System.Drawing.Size(98, 25);
            this.BtnPatch.TabIndex = 7;
            this.BtnPatch.Text = "&Apply Patch";
            this.BtnPatch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnPatch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ToolTip.SetToolTip(this.BtnPatch, "Applies the mod patch.");
            this.BtnPatch.UseCompatibleTextRendering = true;
            this.BtnPatch.UseVisualStyleBackColor = true;
            this.BtnPatch.Click += new System.EventHandler(this.BtnPatch_Click);
            // 
            // DgvMods
            // 
            this.DgvMods.AllowDrop = true;
            this.DgvMods.AllowUserToAddRows = false;
            this.DgvMods.AllowUserToDeleteRows = false;
            this.DgvMods.BackgroundColor = System.Drawing.SystemColors.Control;
            this.DgvMods.CausesValidation = false;
            this.DgvMods.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvMods.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ChEnabledBox,
            this.ChOrder,
            this.ChMod,
            this.ChDesc,
            this.ChVersion,
            this.ChAuthor,
            this.ChConfig,
            this.ChID});
            this.DgvMods.ContextMenuStrip = this.modListCtxMenu;
            this.DgvMods.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.DgvMods.GridColor = System.Drawing.SystemColors.Control;
            this.DgvMods.Location = new System.Drawing.Point(12, 95);
            this.DgvMods.Name = "DgvMods";
            this.DgvMods.ReadOnly = true;
            this.DgvMods.RowHeadersVisible = false;
            this.DgvMods.ShowEditingIcon = false;
            this.DgvMods.Size = new System.Drawing.Size(1050, 382);
            this.DgvMods.TabIndex = 35;
            this.DgvMods.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvMods_CellContentClick);
            this.DgvMods.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DgvMods_CellMouseDown);
            this.DgvMods.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvMods_RowEnter);
            this.DgvMods.RowLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvMods_RowLeave);
            this.DgvMods.DragDrop += new System.Windows.Forms.DragEventHandler(this.DgvMods_DragDrop);
            this.DgvMods.DragOver += new System.Windows.Forms.DragEventHandler(this.DgvMods_DragOver);
            this.DgvMods.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.DgvMods_MouseDoubleClick);
            this.DgvMods.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DgvMods_MouseDown);
            this.DgvMods.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DgvMods_MouseMove);
            // 
            // ChEnabledBox
            // 
            this.ChEnabledBox.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ChEnabledBox.DataPropertyName = "Enabled";
            this.ChEnabledBox.HeaderText = "Enabled";
            this.ChEnabledBox.Name = "ChEnabledBox";
            this.ChEnabledBox.ReadOnly = true;
            this.ChEnabledBox.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ChEnabledBox.Width = 52;
            // 
            // ChOrder
            // 
            this.ChOrder.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ChOrder.DataPropertyName = "LoadOrder";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ChOrder.DefaultCellStyle = dataGridViewCellStyle1;
            this.ChOrder.HeaderText = "Order";
            this.ChOrder.Name = "ChOrder";
            this.ChOrder.ReadOnly = true;
            this.ChOrder.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ChOrder.Width = 58;
            // 
            // ChMod
            // 
            this.ChMod.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ChMod.DataPropertyName = "DisplayName";
            this.ChMod.HeaderText = "Mod";
            this.ChMod.Name = "ChMod";
            this.ChMod.ReadOnly = true;
            this.ChMod.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ChMod.Width = 53;
            // 
            // ChDesc
            // 
            this.ChDesc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ChDesc.DataPropertyName = "Description";
            this.ChDesc.HeaderText = "Description";
            this.ChDesc.Name = "ChDesc";
            this.ChDesc.ReadOnly = true;
            this.ChDesc.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // ChVersion
            // 
            this.ChVersion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ChVersion.DataPropertyName = "Version";
            this.ChVersion.HeaderText = "Version";
            this.ChVersion.Name = "ChVersion";
            this.ChVersion.ReadOnly = true;
            this.ChVersion.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ChVersion.Width = 67;
            // 
            // ChAuthor
            // 
            this.ChAuthor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ChAuthor.DataPropertyName = "Author";
            this.ChAuthor.HeaderText = "Author";
            this.ChAuthor.Name = "ChAuthor";
            this.ChAuthor.ReadOnly = true;
            this.ChAuthor.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ChAuthor.Width = 63;
            // 
            // ChConfig
            // 
            this.ChConfig.ActiveLinkColor = System.Drawing.Color.DarkBlue;
            this.ChConfig.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ChConfig.HeaderText = "Config";
            this.ChConfig.LinkColor = System.Drawing.Color.DarkBlue;
            this.ChConfig.Name = "ChConfig";
            this.ChConfig.ReadOnly = true;
            this.ChConfig.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ChConfig.VisitedLinkColor = System.Drawing.Color.DarkBlue;
            this.ChConfig.Width = 62;
            // 
            // ChID
            // 
            this.ChID.DataPropertyName = "AssemblyName";
            this.ChID.HeaderText = "ID";
            this.ChID.Name = "ChID";
            this.ChID.ReadOnly = true;
            this.ChID.Visible = false;
            // 
            // modListCtxMenu
            // 
            this.modListCtxMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ModMenuName,
            this.toolStripMenuItem4,
            this.openConfigToolStripMenuItem,
            this.removeModToolStripMenuItem,
            this.UpdateDivider,
            this.ModMenuUpdate});
            this.modListCtxMenu.Name = "modListCtxMenu";
            this.modListCtxMenu.Size = new System.Drawing.Size(147, 104);
            this.modListCtxMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ModListCtxMenu_Opening);
            // 
            // ModMenuName
            // 
            this.ModMenuName.Image = global::QModReloadedGUI.Properties.Resources.nexus_mod_manager_icon_256x256;
            this.ModMenuName.Name = "ModMenuName";
            this.ModMenuName.Size = new System.Drawing.Size(146, 22);
            this.ModMenuName.Text = "--";
            this.ModMenuName.Click += new System.EventHandler(this.ModMenuName_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(143, 6);
            // 
            // openConfigToolStripMenuItem
            // 
            this.openConfigToolStripMenuItem.Image = global::QModReloadedGUI.Properties.Resources.file;
            this.openConfigToolStripMenuItem.Name = "openConfigToolStripMenuItem";
            this.openConfigToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.openConfigToolStripMenuItem.Text = "Open Config";
            this.openConfigToolStripMenuItem.Click += new System.EventHandler(this.OpenConfigToolStripMenuItem_Click);
            // 
            // removeModToolStripMenuItem
            // 
            this.removeModToolStripMenuItem.Image = global::QModReloadedGUI.Properties.Resources.action_delete;
            this.removeModToolStripMenuItem.Name = "removeModToolStripMenuItem";
            this.removeModToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.removeModToolStripMenuItem.Text = "Remove Mod";
            this.removeModToolStripMenuItem.Click += new System.EventHandler(this.RemoveModToolStripMenuItem_Click);
            // 
            // UpdateDivider
            // 
            this.UpdateDivider.Name = "UpdateDivider";
            this.UpdateDivider.Size = new System.Drawing.Size(143, 6);
            // 
            // ModMenuUpdate
            // 
            this.ModMenuUpdate.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ModMenuUpdateAll});
            this.ModMenuUpdate.Image = global::QModReloadedGUI.Properties.Resources.save;
            this.ModMenuUpdate.Name = "ModMenuUpdate";
            this.ModMenuUpdate.Size = new System.Drawing.Size(146, 22);
            this.ModMenuUpdate.Text = "Install &Update";
            this.ModMenuUpdate.Click += new System.EventHandler(this.ModMenuUpdate_Click);
            // 
            // ModMenuUpdateAll
            // 
            this.ModMenuUpdateAll.Image = global::QModReloadedGUI.Properties.Resources.arrow_top;
            this.ModMenuUpdateAll.Name = "ModMenuUpdateAll";
            this.ModMenuUpdateAll.Size = new System.Drawing.Size(129, 22);
            this.ModMenuUpdateAll.Text = "Update &All";
            this.ModMenuUpdateAll.Click += new System.EventHandler(this.UpdateAllToolStripMenuItem_Click);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(67, 22);
            // 
            // checklistToolStripMenuItem
            // 
            this.checklistToolStripMenuItem.Name = "checklistToolStripMenuItem";
            this.checklistToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "&About";
            // 
            // ToolStrip
            // 
            this.ToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LblPatched,
            this.toolStripSeparator2,
            this.LblIntroPatched,
            this.toolStripSeparator1,
            this.LblHelper,
            this.ErrorSeparator,
            this.LblErrors,
            this.UpdateProgress,
            this.LblNexusRequests,
            this.toolStripSeparator3,
            this.LblGameVersion});
            this.ToolStrip.Location = new System.Drawing.Point(0, 680);
            this.ToolStrip.Name = "ToolStrip";
            this.ToolStrip.Size = new System.Drawing.Size(1074, 25);
            this.ToolStrip.TabIndex = 25;
            this.ToolStrip.Text = "toolStrip1";
            // 
            // LblPatched
            // 
            this.LblPatched.Name = "LblPatched";
            this.LblPatched.Size = new System.Drawing.Size(86, 22);
            this.LblPatched.Text = "toolStripLabel1";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // LblIntroPatched
            // 
            this.LblIntroPatched.Name = "LblIntroPatched";
            this.LblIntroPatched.Size = new System.Drawing.Size(86, 22);
            this.LblIntroPatched.Text = "toolStripLabel2";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // LblHelper
            // 
            this.LblHelper.Name = "LblHelper";
            this.LblHelper.Size = new System.Drawing.Size(86, 22);
            this.LblHelper.Text = "toolStripLabel1";
            // 
            // ErrorSeparator
            // 
            this.ErrorSeparator.Name = "ErrorSeparator";
            this.ErrorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // LblErrors
            // 
            this.LblErrors.Name = "LblErrors";
            this.LblErrors.Size = new System.Drawing.Size(40, 22);
            this.LblErrors.Text = "Errors:";
            // 
            // UpdateProgress
            // 
            this.UpdateProgress.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.UpdateProgress.Name = "UpdateProgress";
            this.UpdateProgress.Padding = new System.Windows.Forms.Padding(2);
            this.UpdateProgress.Size = new System.Drawing.Size(104, 22);
            this.UpdateProgress.Visible = false;
            // 
            // LblNexusRequests
            // 
            this.LblNexusRequests.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.LblNexusRequests.Name = "LblNexusRequests";
            this.LblNexusRequests.Size = new System.Drawing.Size(0, 22);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem1,
            this.checklistToolStripMenuItem1,
            this.modifyResolutionToolStripMenuItem,
            this.BtnLaunchModless,
            this.optionsToolStripMenuItem,
            this.firstRunInfoToolStripMenuItem,
            this.aboutToolStripMenuItem1,
            this.nexusAPIKeyToolStripMenuItem,
            this.updatesToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1074, 24);
            this.menuStrip1.TabIndex = 26;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem1
            // 
            this.fileToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nexusPageToolStripMenuItem,
            this.toolStripMenuItem5,
            this.openSaveDirectoryToolStripMenuItem,
            this.toolStripMenuItem3,
            this.exitToolStripMenuItem1});
            this.fileToolStripMenuItem1.Image = global::QModReloadedGUI.Properties.Resources.arrow_down;
            this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
            this.fileToolStripMenuItem1.Size = new System.Drawing.Size(53, 20);
            this.fileToolStripMenuItem1.Text = "F&ile";
            // 
            // nexusPageToolStripMenuItem
            // 
            this.nexusPageToolStripMenuItem.Image = global::QModReloadedGUI.Properties.Resources.nexus_mod_manager_icon_256x256;
            this.nexusPageToolStripMenuItem.Name = "nexusPageToolStripMenuItem";
            this.nexusPageToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.nexusPageToolStripMenuItem.Text = "Nexus Page";
            this.nexusPageToolStripMenuItem.Click += new System.EventHandler(this.NexusPageToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(178, 6);
            // 
            // openSaveDirectoryToolStripMenuItem
            // 
            this.openSaveDirectoryToolStripMenuItem.Image = global::QModReloadedGUI.Properties.Resources.folder_files;
            this.openSaveDirectoryToolStripMenuItem.Name = "openSaveDirectoryToolStripMenuItem";
            this.openSaveDirectoryToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.openSaveDirectoryToolStripMenuItem.Text = "Open &Save Directory";
            this.openSaveDirectoryToolStripMenuItem.Click += new System.EventHandler(this.OpenSaveDirectoryToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(178, 6);
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Image = global::QModReloadedGUI.Properties.Resources.stop;
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            this.exitToolStripMenuItem1.Size = new System.Drawing.Size(181, 22);
            this.exitToolStripMenuItem1.Text = "E&xit";
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.ExitToolStripMenuItem1_Click);
            // 
            // checklistToolStripMenuItem1
            // 
            this.checklistToolStripMenuItem1.Image = global::QModReloadedGUI.Properties.Resources.action_check;
            this.checklistToolStripMenuItem1.Name = "checklistToolStripMenuItem1";
            this.checklistToolStripMenuItem1.Size = new System.Drawing.Size(83, 20);
            this.checklistToolStripMenuItem1.Text = "C&hecklist";
            this.checklistToolStripMenuItem1.ToolTipText = "Click to see if your installation is valid for mods to function.";
            this.checklistToolStripMenuItem1.Click += new System.EventHandler(this.ChecklistToolStripMenuItem1_Click);
            // 
            // modifyResolutionToolStripMenuItem
            // 
            this.modifyResolutionToolStripMenuItem.Image = global::QModReloadedGUI.Properties.Resources.login;
            this.modifyResolutionToolStripMenuItem.Name = "modifyResolutionToolStripMenuItem";
            this.modifyResolutionToolStripMenuItem.Size = new System.Drawing.Size(132, 20);
            this.modifyResolutionToolStripMenuItem.Text = "&Modify Resolution";
            this.modifyResolutionToolStripMenuItem.Click += new System.EventHandler(this.ModifyResolutionToolStripMenuItem_Click);
            // 
            // BtnLaunchModless
            // 
            this.BtnLaunchModless.Image = global::QModReloadedGUI.Properties.Resources.play;
            this.BtnLaunchModless.Name = "BtnLaunchModless";
            this.BtnLaunchModless.Size = new System.Drawing.Size(121, 20);
            this.BtnLaunchModless.Text = "&Launch Modless";
            this.BtnLaunchModless.ToolTipText = "This launches the game WITHOUT mods. Click the skeleton to launch with mods.";
            this.BtnLaunchModless.Click += new System.EventHandler(this.BtnLaunchModless_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Image = global::QModReloadedGUI.Properties.Resources.settings;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.optionsToolStripMenuItem.Text = "&Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.OptionsToolStripMenuItem_Click);
            // 
            // firstRunInfoToolStripMenuItem
            // 
            this.firstRunInfoToolStripMenuItem.Image = global::QModReloadedGUI.Properties.Resources.letter;
            this.firstRunInfoToolStripMenuItem.Name = "firstRunInfoToolStripMenuItem";
            this.firstRunInfoToolStripMenuItem.Size = new System.Drawing.Size(105, 20);
            this.firstRunInfoToolStripMenuItem.Text = "First Run Info";
            this.firstRunInfoToolStripMenuItem.Click += new System.EventHandler(this.FirstRunInfoToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem1
            // 
            this.aboutToolStripMenuItem1.Image = global::QModReloadedGUI.Properties.Resources.question_mark;
            this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(68, 20);
            this.aboutToolStripMenuItem1.Text = "A&bout";
            this.aboutToolStripMenuItem1.Click += new System.EventHandler(this.AboutToolStripMenuItem1_Click);
            // 
            // nexusAPIKeyToolStripMenuItem
            // 
            this.nexusAPIKeyToolStripMenuItem.Enabled = false;
            this.nexusAPIKeyToolStripMenuItem.Image = global::QModReloadedGUI.Properties.Resources.nexus_mod_manager_icon_256x256;
            this.nexusAPIKeyToolStripMenuItem.Name = "nexusAPIKeyToolStripMenuItem";
            this.nexusAPIKeyToolStripMenuItem.Size = new System.Drawing.Size(111, 20);
            this.nexusAPIKeyToolStripMenuItem.Text = "&Nexus API Key";
            this.nexusAPIKeyToolStripMenuItem.Visible = false;
            this.nexusAPIKeyToolStripMenuItem.Click += new System.EventHandler(this.NexusAPIKeyToolStripMenuItem_Click);
            // 
            // updatesToolStripMenuItem
            // 
            this.updatesToolStripMenuItem.Enabled = false;
            this.updatesToolStripMenuItem.Image = global::QModReloadedGUI.Properties.Resources.planet_earth_5056;
            this.updatesToolStripMenuItem.Name = "updatesToolStripMenuItem";
            this.updatesToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
            this.updatesToolStripMenuItem.Text = "&Updates";
            this.updatesToolStripMenuItem.Visible = false;
            this.updatesToolStripMenuItem.Click += new System.EventHandler(this.UpdatesToolStripMenuItem_Click);
            // 
            // DlgFile
            // 
            this.DlgFile.Filter = "ZIP Files|*.zip";
            this.DlgFile.Multiselect = true;
            this.DlgFile.Title = "Select ZIP file( s)";
            // 
            // trayIcon
            // 
            this.trayIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.trayIcon.BalloonTipTitle = "QMod Manager Reloaded";
            this.trayIcon.ContextMenuStrip = this.trayIconCtxMenu;
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = "QMod Manager Reloaded";
            this.trayIcon.Visible = true;
            this.trayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TrayIcon_MouseDoubleClick);
            // 
            // trayIconCtxMenu
            // 
            this.trayIconCtxMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.restoreWindowToolStripMenuItem,
            this.toolStripMenuItem2,
            this.launchGameToolStripMenuItem,
            this.openmModDirectoryToolStripMenuItem,
            this.openGameDirectoryToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem2});
            this.trayIconCtxMenu.Name = "trayIconCtxMenu";
            this.trayIconCtxMenu.Size = new System.Drawing.Size(189, 126);
            // 
            // restoreWindowToolStripMenuItem
            // 
            this.restoreWindowToolStripMenuItem.Image = global::QModReloadedGUI.Properties.Resources.arrow_top;
            this.restoreWindowToolStripMenuItem.Name = "restoreWindowToolStripMenuItem";
            this.restoreWindowToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.restoreWindowToolStripMenuItem.Text = "&Restore Window";
            this.restoreWindowToolStripMenuItem.Click += new System.EventHandler(this.RestoreWindowToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(185, 6);
            // 
            // launchGameToolStripMenuItem
            // 
            this.launchGameToolStripMenuItem.Image = global::QModReloadedGUI.Properties.Resources.play;
            this.launchGameToolStripMenuItem.Name = "launchGameToolStripMenuItem";
            this.launchGameToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.launchGameToolStripMenuItem.Text = "&Launch Game";
            this.launchGameToolStripMenuItem.Click += new System.EventHandler(this.LaunchGameToolStripMenuItem_Click);
            // 
            // openmModDirectoryToolStripMenuItem
            // 
            this.openmModDirectoryToolStripMenuItem.Image = global::QModReloadedGUI.Properties.Resources.folder_files1;
            this.openmModDirectoryToolStripMenuItem.Name = "openmModDirectoryToolStripMenuItem";
            this.openmModDirectoryToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.openmModDirectoryToolStripMenuItem.Text = "Open &Mod Directory";
            this.openmModDirectoryToolStripMenuItem.Click += new System.EventHandler(this.OpenmModDirectoryToolStripMenuItem_Click);
            // 
            // openGameDirectoryToolStripMenuItem
            // 
            this.openGameDirectoryToolStripMenuItem.Image = global::QModReloadedGUI.Properties.Resources.folder_open1;
            this.openGameDirectoryToolStripMenuItem.Name = "openGameDirectoryToolStripMenuItem";
            this.openGameDirectoryToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.openGameDirectoryToolStripMenuItem.Text = "Open &Game Directory";
            this.openGameDirectoryToolStripMenuItem.Click += new System.EventHandler(this.OpenGameDirectoryToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(185, 6);
            // 
            // exitToolStripMenuItem2
            // 
            this.exitToolStripMenuItem2.Image = global::QModReloadedGUI.Properties.Resources.stop;
            this.exitToolStripMenuItem2.Name = "exitToolStripMenuItem2";
            this.exitToolStripMenuItem2.Size = new System.Drawing.Size(188, 22);
            this.exitToolStripMenuItem2.Text = "E&xit";
            this.exitToolStripMenuItem2.Click += new System.EventHandler(this.ExitToolStripMenuItem2_Click);
            // 
            // ChkToggleMods
            // 
            this.ChkToggleMods.AutoSize = true;
            this.ChkToggleMods.Location = new System.Drawing.Point(33, 67);
            this.ChkToggleMods.Name = "ChkToggleMods";
            this.ChkToggleMods.Size = new System.Drawing.Size(15, 14);
            this.ChkToggleMods.TabIndex = 37;
            this.ChkToggleMods.UseVisualStyleBackColor = true;
            this.ChkToggleMods.Click += new System.EventHandler(this.ChkToggleMods_Click);
            // 
            // DgvLog
            // 
            this.DgvLog.AllowDrop = true;
            this.DgvLog.AllowUserToAddRows = false;
            this.DgvLog.AllowUserToDeleteRows = false;
            this.DgvLog.BackgroundColor = System.Drawing.SystemColors.Control;
            this.DgvLog.CausesValidation = false;
            this.DgvLog.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.DgvLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ChTime,
            this.dataGridViewTextBoxColumn1});
            this.DgvLog.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.DgvLog.GridColor = System.Drawing.SystemColors.Control;
            this.DgvLog.Location = new System.Drawing.Point(12, 509);
            this.DgvLog.MultiSelect = false;
            this.DgvLog.Name = "DgvLog";
            this.DgvLog.ReadOnly = true;
            this.DgvLog.RowHeadersVisible = false;
            this.DgvLog.ShowEditingIcon = false;
            this.DgvLog.Size = new System.Drawing.Size(887, 158);
            this.DgvLog.TabIndex = 39;
            // 
            // ChTime
            // 
            this.ChTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ChTime.HeaderText = "Time";
            this.ChTime.Name = "ChTime";
            this.ChTime.ReadOnly = true;
            this.ChTime.Width = 55;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.HeaderText = "Log";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // TxtFilter
            // 
            this.TxtFilter.Location = new System.Drawing.Point(111, 64);
            this.TxtFilter.Name = "TxtFilter";
            this.TxtFilter.Size = new System.Drawing.Size(124, 20);
            this.TxtFilter.TabIndex = 40;
            this.TxtFilter.TextChanged += new System.EventHandler(this.TxtFilter_TextChanged);
            // 
            // LblFilter
            // 
            this.LblFilter.AutoSize = true;
            this.LblFilter.Location = new System.Drawing.Point(73, 69);
            this.LblFilter.Name = "LblFilter";
            this.LblFilter.Size = new System.Drawing.Size(32, 13);
            this.LblFilter.TabIndex = 41;
            this.LblFilter.Text = "Filter:";
            // 
            // ChkHideDisabledMods
            // 
            this.ChkHideDisabledMods.AutoSize = true;
            this.ChkHideDisabledMods.Location = new System.Drawing.Point(293, 483);
            this.ChkHideDisabledMods.Name = "ChkHideDisabledMods";
            this.ChkHideDisabledMods.Size = new System.Drawing.Size(118, 17);
            this.ChkHideDisabledMods.TabIndex = 42;
            this.ChkHideDisabledMods.Text = "Hide disabled mods";
            this.ChkHideDisabledMods.UseVisualStyleBackColor = true;
            this.ChkHideDisabledMods.CheckedChanged += new System.EventHandler(this.ChkHideDisabledMods_CheckedChanged);
            // 
            // LblLaunch
            // 
            this.LblLaunch.AutoSize = true;
            this.LblLaunch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblLaunch.ForeColor = System.Drawing.Color.SeaGreen;
            this.LblLaunch.Location = new System.Drawing.Point(902, 654);
            this.LblLaunch.Name = "LblLaunch";
            this.LblLaunch.Size = new System.Drawing.Size(160, 13);
            this.LblLaunch.TabIndex = 43;
            this.LblLaunch.Text = "Launch Game With Mods ^";
            // 
            // BtnKofi
            // 
            this.BtnKofi.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnKofi.Image = global::QModReloadedGUI.Properties.Resources._61e11d6ea0473a3528b575b4_Button_3_p_5001;
            this.BtnKofi.Location = new System.Drawing.Point(934, 37);
            this.BtnKofi.Name = "BtnKofi";
            this.BtnKofi.Size = new System.Drawing.Size(128, 20);
            this.BtnKofi.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.BtnKofi.TabIndex = 44;
            this.BtnKofi.TabStop = false;
            this.BtnKofi.Click += new System.EventHandler(this.BtnKofi_Click);
            // 
            // LblGameVersion
            // 
            this.LblGameVersion.Name = "LblGameVersion";
            this.LblGameVersion.Size = new System.Drawing.Size(91, 22);
            this.LblGameVersion.Text = "Game Version: 0";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1074, 705);
            this.Controls.Add(this.LblLaunch);
            this.Controls.Add(this.ChkHideDisabledMods);
            this.Controls.Add(this.TxtFilter);
            this.Controls.Add(this.LblFilter);
            this.Controls.Add(this.DgvLog);
            this.Controls.Add(this.ChkToggleMods);
            this.Controls.Add(this.BtnRestore);
            this.Controls.Add(this.DgvMods);
            this.Controls.Add(this.BtnOpenLog);
            this.Controls.Add(this.BtnOpenModDir);
            this.Controls.Add(this.BtnOpenGameDir);
            this.Controls.Add(this.ToolStrip);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.BtnRemoveIntros);
            this.Controls.Add(this.BtnRefresh);
            this.Controls.Add(this.BtnRemovePatch);
            this.Controls.Add(this.BtnRunGame);
            this.Controls.Add(this.BtnRemove);
            this.Controls.Add(this.BtnAddMod);
            this.Controls.Add(this.BtnPatch);
            this.Controls.Add(this.LblModFolderLocation);
            this.Controls.Add(this.LblGameLocation);
            this.Controls.Add(this.TxtModFolderLocation);
            this.Controls.Add(this.TxtGameLocation);
            this.Controls.Add(this.BtnKofi);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QMod Manager Reloaded";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.Resize += new System.EventHandler(this.FrmMain_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.BtnRunGame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMods)).EndInit();
            this.modListCtxMenu.ResumeLayout(false);
            this.ToolStrip.ResumeLayout(false);
            this.ToolStrip.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.trayIconCtxMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DgvLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BtnKofi)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox TxtGameLocation;
        private TextBox TxtModFolderLocation;
        private Label LblGameLocation;
        private Label LblModFolderLocation;
        private Button BtnPatch;
        private Button BtnAddMod;
        private Button BtnRemove;
        private PictureBox BtnRunGame;
        private ToolTip ToolTip;
        private Button BtnRemovePatch;
        private Button BtnRefresh;
        private Button BtnRemoveIntros;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem checklistToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStrip ToolStrip;
        private ToolStripLabel LblPatched;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripLabel LblIntroPatched;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem1;
        private ToolStripMenuItem exitToolStripMenuItem1;
        private ToolStripMenuItem checklistToolStripMenuItem1;
        private ToolStripMenuItem aboutToolStripMenuItem1;
        private OpenFileDialog DlgFile;
        private Button BtnOpenGameDir;
        private Button BtnOpenModDir;
        private Button BtnOpenLog;
        private ToolStripMenuItem modifyResolutionToolStripMenuItem;
        private DataGridView DgvMods;
        private NotifyIcon trayIcon;
        private ContextMenuStrip trayIconCtxMenu;
        private ToolStripMenuItem restoreWindowToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem launchGameToolStripMenuItem;
        private ToolStripMenuItem openmModDirectoryToolStripMenuItem;
        private ToolStripMenuItem openGameDirectoryToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem exitToolStripMenuItem2;
        private Button BtnRestore;
        private CheckBox ChkToggleMods;
        private ToolStripMenuItem BtnLaunchModless;
        public DataGridView DgvLog;
        private DataGridViewTextBoxColumn ChTime;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private ToolStripSeparator toolStripMenuItem3;
        private ToolStripMenuItem openSaveDirectoryToolStripMenuItem;
        private ContextMenuStrip modListCtxMenu;
        private ToolStripMenuItem openConfigToolStripMenuItem;
        private ToolStripMenuItem removeModToolStripMenuItem;
        private ToolStripSeparator ErrorSeparator;
        private ToolStripLabel LblErrors;
        private ToolStripMenuItem ModMenuName;
        private ToolStripSeparator toolStripMenuItem4;
        private ToolStripMenuItem nexusAPIKeyToolStripMenuItem;
        private TextBox TxtFilter;
        private Label LblFilter;
        private ToolStripMenuItem updatesToolStripMenuItem;
        private ToolStripProgressBar UpdateProgress;
        private ToolStripLabel LblNexusRequests;
        private ToolStripMenuItem nexusPageToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem5;
        private ToolStripSeparator UpdateDivider;
        private ToolStripMenuItem ModMenuUpdate;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem ModMenuUpdateAll;
        private CheckBox ChkHideDisabledMods;
        private DataGridViewCheckBoxColumn ChEnabledBox;
        private DataGridViewTextBoxColumn ChOrder;
        private DataGridViewTextBoxColumn ChMod;
        private DataGridViewTextBoxColumn ChDesc;
        private DataGridViewTextBoxColumn ChVersion;
        private DataGridViewTextBoxColumn ChAuthor;
        private DataGridViewLinkColumn ChConfig;
        private DataGridViewTextBoxColumn ChID;
        private Label LblLaunch;
        private PictureBox BtnKofi;
        private ToolStripMenuItem firstRunInfoToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripLabel LblHelper;
        private ToolStripSeparator toolStripSeparator3;
        public ToolStripLabel LblGameVersion;
    }
}

