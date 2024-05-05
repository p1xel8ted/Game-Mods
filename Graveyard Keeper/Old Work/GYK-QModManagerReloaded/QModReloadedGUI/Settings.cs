using System;
using System.IO;
using System.Text;
using System.Text.Json;

namespace QModReloadedGUI
{
    public class Settings
    {
        private static readonly string Path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "QMR", "settings.json");
        public byte[] ApiKey { get; set; }
        public string GamePath { get; set; }
        public bool IsPremium { get; set; }
        public bool LaunchDirectly { get; set; }
        public bool UpdateOnStartup { get; set; } = true;
        public string UserName { get; set; }
        public bool UsePreferredEditor { get; set; }
        public string PreferredEditor { get; set; }
        public bool OverrideConfigEditor { get; set; }
        public bool MinToSysTray { get; set; } = true;
        public bool DeleteUpdates { get; set; } = true;
        public bool UseCustomDownloadDir { get; set; }
        public string CustomDownloadDir { get; set; }
        public bool AlwaysRedownload { get; set; }
        public bool HideDisabledMods { get; set; }
        public bool IntroShown { get; set; }

        public static Settings FromJsonFile()
        {
            if (!File.Exists(Path)) return new Settings();
            var value = File.ReadAllText(Path, Encoding.Default);
            return JsonSerializer.Deserialize<Settings>(value);
        }

        public void Save()
        {
            File.WriteAllText(Path, JsonSerializer.Serialize(this, new JsonSerializerOptions() { WriteIndented = true }), Encoding.Default);
        }
    }
}