using System;
using UnityEngine;

namespace PrayTheDayAway;

public static class Config
{
    private static Options _options;
    private static ConfigReader _con;

    public static Options GetOptions(bool external = false)
    {
        _options = new Options();
        _con = new ConfigReader(external);

        bool.TryParse(_con.Value("EverydayIsSermonDay", "true"), out var everydayIsSermonDay);
        _options.EverydayIsSermonDay = everydayIsSermonDay;

        bool.TryParse(_con.Value("SermonOverAndOver", "false"), out var sermonOverAndOver);
        _options.SermonOverAndOver = sermonOverAndOver;

        bool.TryParse(_con.Value("NotifyOnPrayerLoss", "true"), out var notifyOnPrayerLoss);
        _options.NotifyOnPrayerLoss = notifyOnPrayerLoss;
        
        bool.TryParse(_con.Value("AlternateMode", "true"), out var alternateMode);
        _options.AlternateMode = alternateMode;
        
        bool.TryParse(_con.Value("RandomlyUpgradeBasicPrayer", "true"), out var randomlyUpgradeBasicPrayer);
        _options.RandomlyUpgradeBasicPrayer = randomlyUpgradeBasicPrayer;

        var key = Enum.TryParse<KeyCode>(_con.Value("ReloadConfigKeyBind", "F5"), true, out var b);
        if (key)
        {
            _options.ReloadConfigKeyBind = b;
            if (!external)
            {
                Debug.LogWarning($"[PrayTheDayAway]: Parsed '{b}' for 'ReloadConfigKeyBind'.");
            }
        }
        else
        {
            _options.ReloadConfigKeyBind = KeyCode.F5;
            if (!external)
            {
                Debug.LogWarning($"[PrayTheDayAway]: Failed to parse key for 'ReloadConfigKeyBind'. Setting to default F5.");
            }
        }

        bool.TryParse(_con.Value("Debug", "false"), out var debug);
        _options.Debug = debug;

        _con.ConfigWrite();

        return _options;
    }

    public class Options
    {
        public bool NotifyOnPrayerLoss;
        public KeyCode ReloadConfigKeyBind;
        public bool EverydayIsSermonDay;
        public bool SermonOverAndOver;
        public bool Debug;
        public bool AlternateMode;
        public bool RandomlyUpgradeBasicPrayer;
    }
}