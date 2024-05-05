using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace LongerDays;

public static class Config
{
    private static Options _options;
    private static ConfigReader _con;

    public static Options GetOptions(bool external = false)
    {
        _options = new Options();
        _con = new ConfigReader(external);

        bool.TryParse(_con.Value("DoubleLengthDays", "false"), out var doubleLengthDays);
        _options.doubleLengthDays = doubleLengthDays;

        bool.TryParse(_con.Value("EvenLongerDays", "false"), out var evenLongerDays);
        _options.evenLongerDays = evenLongerDays;

        bool.TryParse(_con.Value("Madness", "false"), out var madness);
        _options.madness = madness;

        var key = Enum.TryParse<KeyCode>(_con.Value("ReloadConfigKeyBind", "F5"), true, out var b);
        if (key)
        {
            _options.reloadConfigKeyBind = b;
            if (!external)
            {
                Debug.LogWarning($"[LongerDays]: Parsed '{b}' for 'ReloadConfigKeyBind'.");
            }
        }
        else
        {
            _options.reloadConfigKeyBind = KeyCode.F5;
            if (!external)
            {
                Debug.LogWarning($"[LongerDays]: Failed to parse key for 'ReloadConfigKeyBind'. Setting to default F5.");
            }
        }
        
        bool.TryParse(_con.Value("Debug", "false"), out var debug);
        _options.debug = debug;
        
        _con.ConfigWrite();

        return _options;
    }

    [Serializable]
    public class Options
    {
        [FormerlySerializedAs("DoubleLengthDays")] public bool doubleLengthDays;
        [FormerlySerializedAs("EvenLongerDays")] public bool evenLongerDays;
        [FormerlySerializedAs("Madness")] public bool madness;
        [FormerlySerializedAs("ReloadConfigKeyBind")] public KeyCode reloadConfigKeyBind;
        [FormerlySerializedAs("Debug")] public bool debug;
    }
}