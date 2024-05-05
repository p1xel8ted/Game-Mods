using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Serialization;

namespace GiveMeMoar;

public static class Config
{
    private static Options _options;
    private static ConfigReader _con;

    [Serializable]
    public class Options
    {
        [FormerlySerializedAs("GratitudeMultiplier")]
        public float gratitudeMultiplier;

        [FormerlySerializedAs("SinShardMultiplier")]
        public float sinShardMultiplier;

        [FormerlySerializedAs("DonationMultiplier")]
        public float donationMultiplier;

        [FormerlySerializedAs("RedTechPointMultiplier")]
        public float redTechPointMultiplier;

        [FormerlySerializedAs("BlueTechPointMultiplier")]
        public float blueTechPointMultiplier;

        [FormerlySerializedAs("GreenTechPointMultiplier")]
        public float greenTechPointMultiplier;

        [FormerlySerializedAs("HappinessMultiplier")]
        public float happinessMultiplier;

        [FormerlySerializedAs("ResourceMultiplier")]
        public float resourceMultiplier;

        [FormerlySerializedAs("FaithMultiplier")]
        public float faithMultiplier;

        [FormerlySerializedAs("Debug")] public bool debug;

        [FormerlySerializedAs("DisableSticks")] public bool disableSticks;
        
        [FormerlySerializedAs("ReloadConfigKeyBind")]
        public KeyCode reloadConfigKeyBind;
    }

    public static Options GetOptions(bool external = false)
    {
        _options = new Options();
        _con = new ConfigReader(external);

        float.TryParse(_con.Value("FaithMultiplier", "1"), NumberStyles.Number, CultureInfo.InvariantCulture, out var faithMultiplier);
        _options.faithMultiplier = faithMultiplier;

        float.TryParse(_con.Value("ResourceMultiplier", "1"), NumberStyles.Number, CultureInfo.InvariantCulture, out var resourceMultiplier);
        _options.resourceMultiplier = resourceMultiplier;

        float.TryParse(_con.Value("GratitudeMultiplier", "1"), NumberStyles.Number, CultureInfo.InvariantCulture, out var gratitudeMultiplier);
        _options.gratitudeMultiplier = gratitudeMultiplier;

        float.TryParse(_con.Value("SinShardMultiplier", "1"), NumberStyles.Number, CultureInfo.InvariantCulture, out var sinShardMultiplier);
        _options.sinShardMultiplier = sinShardMultiplier;

        float.TryParse(_con.Value("DonationMultiplier", "1"), NumberStyles.Number, CultureInfo.InvariantCulture, out var donationMultiplier);
        _options.donationMultiplier = donationMultiplier;

        float.TryParse(_con.Value("BlueTechPointMultiplier", "1"), NumberStyles.Number, CultureInfo.InvariantCulture, out var blueTechPointMultiplier);
        _options.blueTechPointMultiplier = blueTechPointMultiplier;

        float.TryParse(_con.Value("GreenTechPointMultiplier", "1"), NumberStyles.Number, CultureInfo.InvariantCulture, out var greenTechPointMultiplier);
        _options.greenTechPointMultiplier = greenTechPointMultiplier;

        float.TryParse(_con.Value("RedTechPointMultiplier", "1"), NumberStyles.Number, CultureInfo.InvariantCulture, out var redTechPointMultiplier);
        _options.redTechPointMultiplier = redTechPointMultiplier;

        float.TryParse(_con.Value("HappinessMultiplier", "1"), NumberStyles.Number, CultureInfo.InvariantCulture, out var happinessMultiplier);
        _options.happinessMultiplier = happinessMultiplier;

        bool.TryParse(_con.Value("Debug", "false"), out var debug);
        _options.debug = debug;
        
        bool.TryParse(_con.Value("DisableSticks", "false"), out var disableSticks);
        _options.disableSticks = disableSticks;

        var key = Enum.TryParse<KeyCode>(_con.Value("ReloadConfigKeyBind", "F5"), true, out var b);
        if (key)
        {
            _options.reloadConfigKeyBind = b;
            if (!external)
            {
                Debug.LogWarning($"[GiveMeMoar]: Parsed '{b}' for 'ReloadConfigKeyBind'.");
            }
        }
        else
        {
            _options.reloadConfigKeyBind = KeyCode.F5;
            if (!external)
            {
                Debug.LogWarning($"[GiveMeMoar]: Failed to parse key for 'ReloadConfigKeyBind'. Setting to default F5.");
            }
        }

        _con.ConfigWrite();

        return _options;
    }
}