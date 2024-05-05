using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace ThoughtfulReminders;

public static class Config
{
    private static Options _options;
    private static ConfigReader _con;

    public static Options GetOptions(bool external = false)
    {
        _options = new Options();
        _con = new ConfigReader(external);

        bool.TryParse(_con.Value("SpeechBubbles", "true"), out var speechBubbles);
        _options.speechBubbles = speechBubbles;

        bool.TryParse(_con.Value("DaysOnly", "false"), out var daysOnly);
        _options.daysOnly = daysOnly;

        var key = Enum.TryParse<KeyCode>(_con.Value("ReloadConfigKeyBind", "F5"), true, out var a);
        if (key)
        {
            _options.reloadConfigKeyBind = a;
            if (!external)
            {
                Debug.LogWarning($"[ThoughtfulReminders]: Parsed '{a}' for 'ReloadConfigKeyBind'.");
            }
        }
        else
        {
            _options.reloadConfigKeyBind = KeyCode.F5;
            if (!external)
            {
                Debug.LogWarning($"[ThoughtfulReminders]: Failed to parse key for 'ReloadConfigKeyBind'. Setting to default F5.");
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
        [FormerlySerializedAs("ReloadConfigKeyBind")]
        public KeyCode reloadConfigKeyBind;

        [FormerlySerializedAs("Debug")] public bool debug;

        [FormerlySerializedAs("SpeechBubbles")]
        public bool speechBubbles;

        [FormerlySerializedAs("DaysOnly")] public bool daysOnly;
    }
}