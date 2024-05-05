using System;
using Helper;
using UnityEngine;
using UnityEngine.Serialization;

namespace IBuildWhereIWant;

public static class Config
{
    private static Options _options;
    private static ConfigReader _con;

    public static Options GetOptions(bool external = false)
    {
        _options = new Options();
        _con = new ConfigReader(external);

        bool.TryParse(_con.Value("DisableGrid", "true"), out var disableGrid);
        _options.disableGrid = disableGrid;

        bool.TryParse(_con.Value("DisableGreyRemoveOverlay", "true"), out var disableGreyRemoveOverlay);
        _options.disableGreyRemoveOverlay = disableGreyRemoveOverlay;
        
        bool.TryParse(_con.Value("DisableBuildingCollision", "false"), out var disableBuildingCollision);
        _options.disableBuildingCollision = disableBuildingCollision;

        bool.TryParse(_con.Value("Debug", "false"), out var debug);
        _options.debug = debug;
        
        var a1 = Enum.TryParse<KeyCode>(_con.Value("ReloadConfigKeyBind", "F5"), true, out var a2);
        if (a1)
        {
            _options.reloadConfigKeyBind = a2;
            if (!external)
            {
                Debug.LogWarning($"[IBuildWhereIWant]: Parsed '{a2}' for 'ReloadConfigKeyBind'.");
            }
        }
        else
        {
            _options.reloadConfigKeyBind = KeyCode.F5;
            if (!external)
            {
                Debug.LogWarning($"[IBuildWhereIWant]: Failed to parse key for 'ReloadConfigKeyBind'. Setting to default F5.");
            }
        }
        

        var b1 = Enum.TryParse<KeyCode>(_con.Value("MenuKeyBind", "Q"), true, out var b2);
        if (b1)
        {
            _options.menuKeyBind  = b2;
            if (!external)
            {
                Debug.LogWarning($"[IBuildWhereIWant]: Parsed '{b2}' for 'MenuKeyBind'.");
            }
        }
        else
        {
            _options.menuKeyBind = KeyCode.Q;
            if (!external)
            {
                Debug.LogWarning($"[IBuildWhereIWant]: Failed to parse key for 'MenuKeyBind'. Setting to default Q");
            }
        }

        var c1 = Enum.TryParse<GamePadButton>(_con.Value("MenuControllerButton", "LB"), true, out var c2);
        if (c1)
        {
            _options.menuControllerButton  = GameButtonMap.Bindings[c2];
            if (!external)
            {
                Debug.LogWarning($"[IBuildWhereIWant]: Parsed '{c2}' for 'MenuControllerButton'.");
            }
        }
        else
        {
            _options.menuControllerButton = GameButtonMap.Bindings[GamePadButton.LB];
            if (!external)
            {
                Debug.LogWarning($"[IBuildWhereIWant]: Failed to parse key for 'MenuControllerButton'. Setting to default LB.");
            }
        }
        
        
        _con.ConfigWrite();

        return _options;
    }

    [Serializable]
    public class Options
    {
        [FormerlySerializedAs("DisableSafeMode")] public bool disableSafeMode;
        [FormerlySerializedAs("Debug")] public bool debug;
        [FormerlySerializedAs("DisableGrid")] public bool disableGrid;
        [FormerlySerializedAs("DisableGreyRemoveOverlay")] public bool disableGreyRemoveOverlay;
        [FormerlySerializedAs("DisableBuildingCollision")] public bool disableBuildingCollision;
        [FormerlySerializedAs("MenuKeyBind")] public KeyCode menuKeyBind;
        [FormerlySerializedAs("MenuControllerButton")] public int menuControllerButton;
        [FormerlySerializedAs("ReloadConfigKeyBind")] public KeyCode reloadConfigKeyBind;
    }
}