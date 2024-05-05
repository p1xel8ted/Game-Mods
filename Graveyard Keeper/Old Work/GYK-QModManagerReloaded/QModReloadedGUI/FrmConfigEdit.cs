using QModReloaded;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace QModReloadedGUI
{
    public partial class FrmConfigEdit : Form
    {
        private readonly DataGridView _dgvLog;
        private readonly QMod _foundMod;
        private readonly string _gameLocation;
        private readonly string _path;
        private string _contents;

        public FrmConfigEdit(ref QMod mod, ref DataGridView dgvLog, string gameLocation)
        {
            InitializeComponent();
            _foundMod = mod;
            _path = Path.Combine(_foundMod.ModAssemblyPath, _foundMod.Config);
            _dgvLog = dgvLog;
            _gameLocation = gameLocation;
        }

        private void FrmConfigEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                File.WriteAllText(_path, TxtConfig.Text, Encoding.Default);
                WriteLog($"Saved changes to {_foundMod.Config} for {_foundMod.DisplayName}.");
            }
            catch (Exception ex)
            {
                WriteLog($"Error saving changes to {_foundMod.Config} for {_foundMod.DisplayName}. Message: {ex.Message}", true);
            }
        }

        private void FrmConfigEdit_Load(object sender, EventArgs e)
        {
            try
            {
                Text = @$"Editing {_foundMod.Config} for {_foundMod.DisplayName}";
                _contents = File.ReadAllText(_path);
                TxtConfig.Text = _contents;
                for (var i = 0; i < TxtConfig.Lines.Length; i++)
                {
                    var trim = TxtConfig.Lines[i].Trim();
                    TxtConfig.Lines[i] = trim;
                }
                WriteLog($"Loaded {_foundMod.Config} for {_foundMod.DisplayName}.");
            }
            catch (Exception ex)
            {
                WriteLog($"Error loading {_foundMod.Config} for {_foundMod.DisplayName}. Message: {ex.Message}", true);
            }
        }

        private void WriteLog(string message, bool error = false)
        {
            var dt = DateTime.Now;
            var rowIndex = _dgvLog.Rows.Add(dt.ToLongTimeString(), message);
            var row = _dgvLog.Rows[rowIndex];
            if (error)
            {
                row.DefaultCellStyle.BackColor = Color.LightCoral;
            }

            string logMessage;
            if (error)
            {
                logMessage = "-----------------------------------------\n";
                logMessage += dt.ToShortDateString() + " " + dt.ToLongTimeString() + " : [ERROR] : " + message + "\n";
                logMessage += "-----------------------------------------";
            }
            else
            {
                logMessage = dt.ToShortDateString() + " " + dt.ToLongTimeString() + " : " + message;
            }
            _dgvLog.FirstDisplayedScrollingRowIndex = _dgvLog.RowCount - 1;
            Utilities.WriteLog(logMessage, _gameLocation);
        }

        private void TxtConfig_TextChanged(object sender, EventArgs e)
        {
            TxtConfig.Font = TxtConfig.Font;
        }
    }
}