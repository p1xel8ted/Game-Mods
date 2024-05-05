using QModReloaded;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace QModReloadedGUI
{
    public partial class FrmChecklist : Form
    {
        private readonly string _gameLocation;
        private readonly Injector _injector;
        private readonly string _modLocation;

        public FrmChecklist(Injector injector, string gameLocation, string modLocation)
        {
            _injector = injector;
            _gameLocation = gameLocation;
            _modLocation = modLocation;
            InitializeComponent();
        }

        private static bool CheckFileExists(string sFile)
        {
            var file = new FileInfo(sFile);
            return file.Exists;
        }

        private void FrmChecklist_Load(object sender, EventArgs e)
        {
            ChkModPatched.Checked = _injector.IsInjected();
            ChkGameLocation.Checked = _gameLocation != string.Empty;
            ChkModDirectoryExists.Checked = _modLocation != string.Empty;
            var allFound = 0;
            foreach (Control control in Controls)
            {
                if (control.GetType() != typeof(CheckBox)) continue;
                var c = (CheckBox)control;

                if (c.Text.Contains("exe") || c.Text.Contains("dll"))
                {
                    var found = CheckFileExists(Path.Combine(Application.StartupPath, c.Text));
                    if (found)
                    {
                        c.Checked = true;
                        allFound++;
                    }
                    else
                    {
                        c.Checked = false;
                    }
                }

                c.ForeColor = c.Checked ? Color.Green : Color.Red;
            }


            var hVersion = CheckFileExists(Path.Combine(Application.StartupPath, "0Harmony.dll"));
            if (hVersion)
            {
                var harmonyVersion =
                    FileVersionInfo.GetVersionInfo(Path.Combine(Application.StartupPath, Chk0HarmonyExists.Text));
                if (harmonyVersion.ProductVersion.StartsWith("2.2"))
                {
                    LblHarmonyVersion.Visible = true;
                    LblHarmonyVersion.Text = @"Correct, 2.2+";
                    LblHarmonyVersion.ForeColor = Color.Green;
                }
                else
                {
                    LblHarmonyVersion.Visible = true;
                    LblHarmonyVersion.Text = @"In-correct!, less than 2.2!";
                    LblHarmonyVersion.ForeColor = Color.Red;
                }
            }
            else
            {
                LblHarmonyVersion.Visible = false;
            }


            var helperPresent = CheckFileExists(Path.Combine(Application.StartupPath, @"..\..\", "QMods\\Helper.dll"));
            if (helperPresent)
            {
                allFound++;
                ChkHelper.Checked = true;
                ChkHelper.ForeColor = Color.Green;
            }
            else
            {
                ChkHelper.Checked = false;
                ChkHelper.ForeColor = Color.Red;
            }

            if (allFound == 19)
            {
                ChkPatcherLocation.Checked = true;
                ChkPatcherLocation.ForeColor = Color.Green;
            }
            else
            {
                ChkPatcherLocation.Checked = false;
                ChkPatcherLocation.ForeColor = Color.Red;
            }
        }
    }
}