using System;
using System.Windows.Forms;

namespace QModReloadedGUI
{
    public partial class FrmOptions : Form
    {
        private readonly Settings _settings;
        public FrmOptions(ref Settings settings)
        {
            _settings = settings;
            InitializeComponent();
        }

        private void FrmOptions_Load(object sender, EventArgs e)
        {
            ChkLaunchExeDirectly.Checked = _settings.LaunchDirectly;
            ChkUpdateOnStartup.Checked = _settings.UpdateOnStartup;
            ChkOverride.Checked = _settings.OverrideConfigEditor;
            ChkRemoveDownloadedFile.Checked = _settings.DeleteUpdates;
            ChkMinimizeTray.Checked = _settings.MinToSysTray;
            ChkEditor.Checked = _settings.UsePreferredEditor;
            ChkDownloadDirectory.Checked = _settings.UseCustomDownloadDir;
            ChkRedownload.Checked = _settings.AlwaysRedownload;
            ChkHideDisabled.Checked = _settings.HideDisabledMods;

            TxtEditor.Text = _settings.PreferredEditor;
            TxtDownloadDir.Text = _settings.CustomDownloadDir;
        }

        private void ChkUpdateOnStartup_CheckedChanged(object sender, EventArgs e)
        {
           _settings.UpdateOnStartup = ChkUpdateOnStartup.Checked;
            _settings.Save();
        }

        private void ChkLaunchExeDirectly_CheckedChanged(object sender, EventArgs e)
        {
            _settings.LaunchDirectly = ChkLaunchExeDirectly.Checked;
            _settings.Save();
        }

        private void ChkEditor_CheckedChanged(object sender, EventArgs e)
        {
           _settings.UsePreferredEditor = ChkEditor.Checked;
           _settings.Save();
        }

        private void ChkOverride_CheckedChanged(object sender, EventArgs e)
        {
            _settings.OverrideConfigEditor = ChkOverride.Checked;
            _settings.Save();
        }


        private void FrmOptions_MouseEnter(object sender, EventArgs e)
        {
            LblInfo.Text = "";
        }

        private void ChkMinimizeTray_CheckedChanged(object sender, EventArgs e)
        {
             _settings.MinToSysTray = ChkMinimizeTray.Checked;
             _settings.Save();
        }

        private void ChkRemoveDownloadedFile_CheckedChanged(object sender, EventArgs e)
        {
            _settings.DeleteUpdates = ChkRemoveDownloadedFile.Checked;
            _settings.Save();
        }

        private void ChkRemoveDownloadedFile_MouseHover(object sender, EventArgs e)
        {
            LblInfo.Text = @"Removed downloaded update files after successful extraction.";
        }

        private void ChkMinimizeTray_MouseHover(object sender, EventArgs e)
        {
            LblInfo.Text = @"Go to system tray when minimized.";
        }

        private void ChkLaunchExeDirectly_MouseHover(object sender, EventArgs e)
        {
            LblInfo.Text = @"Tick this to launch the EXE directly.";
        }

        private void ChkEditor_MouseEnter(object sender, EventArgs e)
        {
            LblInfo.Text = @"Select a preferred editor to view log/config files.";
        }

        private void ChkOverride_MouseHover(object sender, EventArgs e)
        {
            LblInfo.Text = @"Override the built-in config editor with the editor of your choice.";
        }

        private void ChkUpdateOnStartup_MouseHover(object sender, EventArgs e)
        {
            LblInfo.Text = @"Check for mod updates from NexusMods when QMR loads.";
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            var dlgResult = DlgEditor.ShowDialog(this);
            if (dlgResult != DialogResult.OK) return;
            TxtEditor.Text = DlgEditor.FileName;
            _settings.PreferredEditor = TxtEditor.Text;
            _settings.Save();
        }

        private void ChkDownloadDirectory_CheckedChanged(object sender, EventArgs e)
        {
            _settings.UseCustomDownloadDir = ChkDownloadDirectory.Checked;
            _settings.Save();
        }

        private void ChkDownloadDirectory_MouseHover(object sender, EventArgs e)
        {
            LblInfo.Text = @"Download updates to the specified directory.";
        }

        private void BtnBrowseUpdate_Click(object sender, EventArgs e)
        {
            var dlgResult = DlgDownload.ShowDialog(this);
            if (dlgResult != DialogResult.OK) return;
            TxtDownloadDir.Text = DlgDownload.SelectedPath;
            _settings.CustomDownloadDir = TxtDownloadDir.Text;
            _settings.Save();
        }

        private void FrmOptions_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((ChkOverride.Checked || ChkEditor.Checked) && TxtEditor.Text.Length <= 0)
            {
                e.Cancel = true;
            }
            if (ChkDownloadDirectory.Checked && TxtDownloadDir.Text.Length <= 0)
            {
                e.Cancel = true;
            }

            if(e.Cancel)
             MessageBox.Show(
                @"You have selected options that require more configuration. i.e. custom download directory/use preferred editor. Please resolve or un-tick the options to continue.", @"Hmmm", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        private void ChkRedownload_MouseHover(object sender, EventArgs e)
        {
            LblInfo.Text = @"Always re-download updates even if the file already exists.";
        }

        private void ChkRedownload_CheckedChanged(object sender, EventArgs e)
        {
             _settings.AlwaysRedownload = ChkRedownload.Checked;
             _settings.Save();
        }

        private void ChkHideDisabled_CheckedChanged(object sender, EventArgs e)
        {
            _settings.HideDisabledMods = ChkHideDisabled.Checked;
            _settings.Save();
        }

        private void ChkHideDisabled_MouseHover(object sender, EventArgs e)
        {
            LblInfo.Text = @"Enabling hiding of disabled mods by default.";
        }
    }
}
