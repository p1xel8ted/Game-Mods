using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;
using static QModReloadedGUI.Utilities;

namespace QModReloadedGUI
{
    public partial class FrmNexus : Form
    {
        private readonly string _homePath;
        private readonly ToolStripLabel _lblNexusRequests;
        private readonly Settings _settings;

        public FrmNexus(string homePath, ref ToolStripLabel lblNexusRequests, ref Settings settings)
        {
            _settings = settings;
            _lblNexusRequests = lblNexusRequests;
            _homePath = homePath;
            InitializeComponent();
        }

        private void BtnValidate_Click(object sender, EventArgs e)
        {
            try
            {
                var apiKey = TxtApi.Text.Trim();
                var validateKey = new WebClient();
                validateKey.DownloadStringCompleted += ValidateKeyCompleted;
                validateKey.Headers.Add("apiKey", apiKey);
                validateKey.Headers.Add("Application-Version", Assembly.GetExecutingAssembly().GetName().Version.ToString());
                validateKey.Headers.Add("Application-Name", "QMod-Manager-Reloaded");
                validateKey.Headers.Add("User-Agent", $"QMod-Manager-Reloaded/{Assembly.GetExecutingAssembly().GetName().Version} {Environment.OSVersion}");

                validateKey.DownloadStringAsync(new Uri($"https://api.nexusmods.com/v1/users/validate"));

                void ValidateKeyCompleted(object sender, DownloadStringCompletedEventArgs args)
                {
                    _lblNexusRequests.Text = UpdateRequestCounts(validateKey.ResponseHeaders, _settings.UserName,_settings.IsPremium);
                    var validate = JsonSerializer.Deserialize<Validate>(args.Result);
                    if (validate != null)
                    {
                        _settings.UserName = validate.Name;
                        _settings.IsPremium = validate.IsPremium;
                        LblValidated.Text = _settings.IsPremium ? @$"Profile: {_settings.UserName} (Premium)" : @$"Profile: {_settings.UserName} (Standard)";
                        if (_settings.UserName.Length > 0)
                        {
                            Text = _settings.IsPremium
                                ? @$"Nexus API Key - Profile: {_settings.UserName} (Premium)"
                                : @$"Profile: {_settings.UserName} (Standard)";
                        }
                        LblValidated.ForeColor = _settings.IsPremium ? Color.Green : Color.Black;

                        var obscurer = new Obscure();
                        _settings.ApiKey = Obscure.Encrypt(TxtApi.Text.Trim(), obscurer.Key, obscurer.Vector);
                        _settings.Save();
                        File.Delete(_homePath);
                        File.WriteAllText(_homePath, JsonSerializer.Serialize(new PairedKeys { Lock = obscurer.Key, Vector = obscurer.Vector }, new JsonSerializerOptions() { WriteIndented = true }), Encoding.Default);
                        File.SetAttributes(_homePath, FileAttributes.Hidden);
                    }
                    else
                    {
                        MessageBox.Show(@"Sorry, that key could not be validated by NexusMods.", @"Typo?",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch
            {
                //
            }
        }

        private void FrmNexus_Load(object sender, EventArgs e)
        {
            try
            {
                var pairedKeys = JsonSerializer.Deserialize<PairedKeys>(File.ReadAllText(_homePath), new JsonSerializerOptions { AllowTrailingCommas = true });
                if (pairedKeys?.Vector is null)
                {
                    return;
                }
                TxtApi.Text = Obscure.Decrypt(_settings.ApiKey, pairedKeys.Lock, pairedKeys.Vector);
                LblValidated.Text = _settings.IsPremium ? @$"Profile: {_settings.UserName} (Premium)" : @$"Profile: {_settings.UserName} (Standard)";
                if (_settings.UserName.Length > 0)
                {
                    Text = _settings.IsPremium
                        ? @$"Nexus API Key - Profile: {_settings.UserName} (Premium)"
                        : @$"Profile: {_settings.UserName} (Standard)";
                }

                LblValidated.ForeColor = _settings.IsPremium ? Color.Green : Color.Black;
            }
            catch
            {
                //
            }
        }

        private void LblValidated_Click(object sender, EventArgs e)
        {

        }
    }
}