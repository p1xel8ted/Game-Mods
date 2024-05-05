using System;
using System.Globalization;
using UnityEngine;

namespace Exhaustless;

public static class Config
{
    private static Options _options;
    private static ConfigReader _con;

    public static Options GetOptions(bool external = false)
    {
        _options = new Options();
        _con = new ConfigReader(external);

        bool.TryParse(_con.Value("MakeToolsLastLonger", "true"), out var makeToolsLastLonger);
        _options.MakeToolsLastLonger = makeToolsLastLonger;

        bool.TryParse(_con.Value("SpendHalfGratitude", "true"), out var spendHalfGratitude);
        _options.SpendHalfGratitude = spendHalfGratitude;

        bool.TryParse(_con.Value("AutoEquipNewTool", "true"), out var autoEquipNewTool);
        _options.AutoEquipNewTool = autoEquipNewTool;

        bool.TryParse(_con.Value("SpeedUpSleep", "true"), out var speedUpSleep);
        _options.SpeedUpSleep = speedUpSleep;

        bool.TryParse(_con.Value("AutoWakeFromMeditation", "true"), out var autoWakeFromMeditation);
        _options.AutoWakeFromMeditation = autoWakeFromMeditation;

        bool.TryParse(_con.Value("SpendHalfSanity", "true"), out var spendHalfSanity);
        _options.SpendHalfSanity = spendHalfSanity;

        bool.TryParse(_con.Value("SpeedUpMeditation", "true"), out var speedUpMeditation);
        _options.SpeedUpMeditation = speedUpMeditation;

        bool.TryParse(_con.Value("YawnMessage", "true"), out var yawnMessage);
        _options.YawnMessage = yawnMessage;

        bool.TryParse(_con.Value("SpendHalfEnergy", "true"), out var spendHalfEnergy);
        _options.SpendHalfEnergy = spendHalfEnergy;

        int.TryParse(_con.Value("EnergySpendBeforeSleepDebuff", "1200"), NumberStyles.Number, CultureInfo.InvariantCulture, out var energySpendBeforeSleepDebuff);
        _options.EnergySpendBeforeSleepDebuff = energySpendBeforeSleepDebuff;
        
        bool.TryParse(_con.Value("Debug", "false"), out var debug);
        _options.Debug = debug;
        
        var key = Enum.TryParse<KeyCode>(_con.Value("ReloadConfigKeyBind", "F5"), true, out var b);
        if (key)
        {
            _options.ReloadConfigKeyBind = b;
            if (!external)
            {
                Debug.LogWarning($"[Exhaustless]: Parsed '{b}' for 'ReloadConfigKeyBind'.");
            }
        }
        else
        {
            _options.ReloadConfigKeyBind = KeyCode.F5;
            if (!external)
            {
                Debug.LogWarning($"[Exhaustless]: Failed to parse key for 'ReloadConfigKeyBind'. Setting to default F5.");
            }
        }
        
        _con.ConfigWrite();

        return _options;
    }

   
    public class Options
    {
     public int EnergySpendBeforeSleepDebuff;
     public bool SpeedUpSleep;
       public bool SpeedUpMeditation;
      public bool YawnMessage;
      public bool SpendHalfEnergy;
        public bool SpendHalfSanity;
        public bool AutoWakeFromMeditation;
       public bool MakeToolsLastLonger;
         public bool AutoEquipNewTool;
         public bool SpendHalfGratitude;
        public KeyCode ReloadConfigKeyBind;
        public bool Debug;
    }
}