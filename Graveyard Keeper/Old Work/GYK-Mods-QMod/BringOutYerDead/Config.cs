using System;
using System.Globalization;
using UnityEngine;

namespace BringOutYerDead;

public static class Config
{
    private static Options _options;
    private static ConfigReader _con;


    public static Options GetOptions(bool external = false)
    {
        _options = new Options();
        _con = new ConfigReader(external);

        bool.TryParse(_con.Value("MorningDelivery", "true"), out var morningDelivery);
        _options.MorningDelivery = morningDelivery;

        bool.TryParse(_con.Value("DayDelivery", "false"), out var dayDelivery);
        _options.DayDelivery = dayDelivery;

        bool.TryParse(_con.Value("NightDelivery", "false"), out var nightDelivery);
        _options.NightDelivery = nightDelivery;

        bool.TryParse(_con.Value("EveningDelivery", "true"), out var eveningDelivery);
        _options.EveningDelivery = eveningDelivery;
        
        float.TryParse(_con.Value("DonkeySpeed", "2"), NumberStyles.Float, CultureInfo.InvariantCulture, out var donkeySpeed);
        _options.DonkeySpeed = donkeySpeed;
        if (donkeySpeed < 2.0f)
        {
            _options.DonkeySpeed = 2f;
        }
       

        var key = Enum.TryParse<KeyCode>(_con.Value("ReloadConfigKeyBind", "F5"), true, out var b);
        if (key)
        {
            _options.ReloadConfigKeyBind = b;
            if (!external)
            {
                Debug.LogWarning($"[BringOutYerDead]: Parsed '{b}' for 'ReloadConfigKeyBind'.");
            }
        }
        else
        {
            _options.ReloadConfigKeyBind = KeyCode.F5;
            if (!external)
            {
                Debug.LogWarning($"[BringOutYerDead]: Failed to parse key for 'ReloadConfigKeyBind'. Setting to default F5.");
            }
        }

        bool.TryParse(_con.Value("Debug", "false"), out var debug);
        _options.Debug = debug;

        _con.ConfigWrite();

        return _options;
    }


    public class Options
    {
        public bool MorningDelivery;
        public bool DayDelivery;
        public bool EveningDelivery;
        public float DonkeySpeed;
        public bool NightDelivery;
        public KeyCode ReloadConfigKeyBind;
        public bool Debug;
    }
}