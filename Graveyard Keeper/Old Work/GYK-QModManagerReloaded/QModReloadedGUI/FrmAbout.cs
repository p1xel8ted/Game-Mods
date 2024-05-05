using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace QModReloadedGUI
{
    public partial class FrmAbout : Form
    {
        public FrmAbout()
        {
            InitializeComponent();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmAbout_Load(object sender, EventArgs e)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            var buildDate = new DateTime(2000, 1, 1)
                .AddDays(version.Build).AddSeconds(version.Revision * 2);
            var displayableVersion = $"{version} ({buildDate})";
            TxtVersion.Text = displayableVersion;
        }

        private void LblCreditsUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(LblCreditsUrl.Text);
        }

        private void LblMyUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(LblMyUrl.Text);
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("https://www.nexusmods.com/graveyardkeeper/mods/40?tab=files");
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}