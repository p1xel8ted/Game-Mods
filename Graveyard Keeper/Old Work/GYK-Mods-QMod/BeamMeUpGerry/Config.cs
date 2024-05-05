using System;
using Helper;
using UnityEngine;
using UnityEngine.Serialization;

namespace BeamMeUpGerry;

public static class Config
{
    private static Options _options;
    private static ConfigReader _con;

    [Serializable]
    public class Options
    {
        [FormerlySerializedAs("FadeForCustomLocations")]
        public bool fadeForCustomLocations;

        [FormerlySerializedAs("IncreaseMenuAnimationSpeed")]
        public bool increaseMenuAnimationSpeed;

        [FormerlySerializedAs("EnableListExpansion")]
        public bool enableListExpansion;

        [FormerlySerializedAs("DisableCost")] public bool disableCost;
        [FormerlySerializedAs("DisableGerry")] public bool disableGerry;
        [FormerlySerializedAs("Debug")] public bool debug;

        [FormerlySerializedAs("TeleportMenuKeyBind")]
        public KeyCode teleportMenuKeyBind;

        [FormerlySerializedAs("ReloadConfigKeyBind")]
        public KeyCode reloadConfigKeyBind;

        [FormerlySerializedAs("TeleportMenuControllerButton")]
        public int teleportMenuControllerButton;
    }

    public static Options GetOptions(bool external = false)
    {
        _options = new Options();
        _con = new ConfigReader(external);

        bool.TryParse(_con.Value("IncreaseMenuAnimationSpeed", "true"), out var increaseMenuAnimationSpeed);
        _options.increaseMenuAnimationSpeed = increaseMenuAnimationSpeed;

        bool.TryParse(_con.Value("FadeForCustomLocations", "true"), out var fadeForCustomLocations);
        _options.fadeForCustomLocations = fadeForCustomLocations;

        bool.TryParse(_con.Value("EnableListExpansion", "true"), out var enableListExpansion);
        _options.enableListExpansion = enableListExpansion;

        bool.TryParse(_con.Value("DisableGerry", "false"), out var disableGerry);
        _options.disableGerry = disableGerry;

        bool.TryParse(_con.Value("DisableCost", "false"), out var disableCost);
        _options.disableCost = disableCost;

        bool.TryParse(_con.Value("Debug", "false"), out var debug);
        _options.debug = debug;

        _options.teleportMenuKeyBind = Enum.TryParse<KeyCode>(_con.Value("TeleportMenuKeyBind", "Z"), true,out var a) ? a : KeyCode.Z;
        var a1 = Enum.TryParse<KeyCode>(_con.Value("TeleportMenuKeyBind", "Alpha6"), true, out var a2);
        if (a1)
        {
            _options.teleportMenuKeyBind = a2;
            if (!external)
            {
                Debug.LogWarning($"[BeamMeUpGerry]: Parsed '{a2}' for 'TeleportMenuKeyBind'.");
            }
        }
        else
        {
            _options.teleportMenuKeyBind = KeyCode.Z;
            if (!external)
            {
                Debug.LogWarning($"[BeamMeUpGerry]: Failed to parse key for 'TeleportMenuKeyBind'. Setting to default Z.");
            }
        }

        _options.reloadConfigKeyBind = Enum.TryParse<KeyCode>(_con.Value("ReloadConfigKeyBind", "F5"), true,out var b) ? b : KeyCode.F5;
        var b1 = Enum.TryParse<KeyCode>(_con.Value("ReloadConfigKeyBind", "F5"), true, out var b2);
        if (b1)
        {
            _options.reloadConfigKeyBind = b2;
            if (!external)
            {
                Debug.LogWarning($"[BeamMeUpGerry]: Parsed '{b2}' for 'ReloadConfigKeyBind'.");
            }
        }
        else
        {
            _options.reloadConfigKeyBind = KeyCode.F5;
            if (!external)
            {
                Debug.LogWarning($"[BeamMeUpGerry]: Failed to parse key for 'ReloadConfigKeyBind'. Setting to default F5.");
            }
        }
        
        var c1 = Enum.TryParse<GamePadButton>(_con.Value("TeleportMenuControllerButton", "RB"), true, out var c2);
        if (c1)
        {
            _options.teleportMenuControllerButton = GameButtonMap.Bindings[c2];
            if (!external)
            {
                Debug.LogWarning($"[BeamMeUpGerry]: Parsed '{c2}' for 'TeleportMenuControllerButton'.");
            }
        }
        else
        {
            _options.teleportMenuControllerButton = GameButtonMap.Bindings[GamePadButton.RB];
            if (!external)
            {
                Debug.LogWarning($"[BeamMeUpGerry]: Failed to parse key for 'TeleportMenuControllerButton'. Setting to default RB.");
            }
        }
        
        _con.ConfigWrite();

        return _options;
    }
}