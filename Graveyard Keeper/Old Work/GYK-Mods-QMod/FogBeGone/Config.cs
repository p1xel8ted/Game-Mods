using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace FogBeGone;

public static class Config
{
    private static Options _options;
    private static ConfigReader _con;

    public static Options GetOptions(bool external = false)
    {
        _options = new Options();
        _con = new ConfigReader(external);

        bool.TryParse(_con.Value("Debug", "false"), out var debug);
        _options.debug = debug;
        
        bool.TryParse(_con.Value("EnableFog", "false"), out var enableFog);
        _options.enableFog = enableFog;

        var key = Enum.TryParse<KeyCode>(_con.Value("ReloadConfigKeyBind", "F5"), true, out var b);
        if (key)
        {
            _options.reloadConfigKeyBind = b;
            if (!external)
            {
                Debug.LogWarning($"[FogBeGone]: Parsed '{b}' for 'ReloadConfigKeyBind'.");
            }
        }
        else
        {
            _options.reloadConfigKeyBind = KeyCode.F5;
            if (!external)
            {
                Debug.LogWarning($"[FogBeGone]: Failed to parse key for 'ReloadConfigKeyBind'. Setting to default F5.");
            }
        }

        _con.ConfigWrite();

        return _options;
    }

    [Serializable]
    public class Options
    {
        
        [FormerlySerializedAs("ReloadConfigKeyBind")] public KeyCode reloadConfigKeyBind;
        [FormerlySerializedAs("Debug")] public bool debug;
        [FormerlySerializedAs("EnableFog")] public bool enableFog;

    }
}