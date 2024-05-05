using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace EconomyReloaded;

public static class Config
{
    private static Options _options;
    private static ConfigReader _con;

    [Serializable]
    public class Options
    {
        [FormerlySerializedAs("Debug")] public bool debug;
        [FormerlySerializedAs("OldSchoolMode")] public bool oldSchoolMode;
        [FormerlySerializedAs("DisableInflation")] public bool disableInflation;
        [FormerlySerializedAs("DisableDeflation")] public bool disableDeflation;
        [FormerlySerializedAs("ReloadConfigKeyBind")] public KeyCode reloadConfigKeyBind;
    }
        
    public static Options GetOptions(bool external = false)
    {
        _options = new Options();
        _con = new ConfigReader(external);

        bool.TryParse(_con.Value("Debug", "false"), out var debug);
        _options.debug = debug;

        bool.TryParse(_con.Value("OldSchoolMode", "false"), out var oldSchoolMode);
        _options.oldSchoolMode = oldSchoolMode;

        bool.TryParse(_con.Value("DisableInflation", "true"), out var disableInflation);
        _options.disableInflation = disableInflation;

        bool.TryParse(_con.Value("DisableDeflation", "true"), out var disableDeflation);
        _options.disableDeflation = disableDeflation;
            
        var key = Enum.TryParse<KeyCode>(_con.Value("ReloadConfigKeyBind", "F5"), true, out var b);
        if (key)
        {
            _options.reloadConfigKeyBind = b;
            if (!external)
            {
                Debug.LogWarning($"[EconomyReloaded]: Parsed '{b}' for 'ReloadConfigKeyBind'.");
            }
        }
        else
        {
            _options.reloadConfigKeyBind = KeyCode.F5;
            if (!external)
            {
                Debug.LogWarning($"[EconomyReloaded]: Failed to parse key for 'ReloadConfigKeyBind'. Setting to default F5.");
            }
        }

        _con.ConfigWrite();

        return _options;
    }
}