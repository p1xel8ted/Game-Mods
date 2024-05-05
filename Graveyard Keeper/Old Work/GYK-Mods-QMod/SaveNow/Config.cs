using System;
using System.Globalization;
using Helper;
using UnityEngine;
using UnityEngine.Serialization;

namespace SaveNow;

public static class Config
{
    private static Options _options;
    private static ConfigReader _con;

    public static Options GetOptions(bool external = false)
    {
        _options = new Options();
        _con = new ConfigReader(external);

        int.TryParse(_con.Value("SaveInterval", "600"), NumberStyles.Integer, CultureInfo.InvariantCulture, out var saveInterval);
        _options.saveInterval = saveInterval;

        bool.TryParse(_con.Value("AutoSave", "true"), out var autoSave);
        _options.autoSave = autoSave;

        bool.TryParse(_con.Value("NewFileOnAutoSave", "true"), out var newFileOnAutoSave);
        _options.newFileOnAutoSave = newFileOnAutoSave;

        bool.TryParse(_con.Value("NewFileOnManualSave", "true"), out var newFileOnManualSave);
        _options.newFileOnManualSave = newFileOnManualSave;

        bool.TryParse(_con.Value("BackupSavesOnSave", "true"), out var backupSavesOnSave);
        _options.backupSavesOnSave = backupSavesOnSave;

        bool.TryParse(_con.Value("TurnOffTravelMessages", "false"), out var turnOffTravelMessages);
        _options.turnOffTravelMessages = turnOffTravelMessages;

        bool.TryParse(_con.Value("TurnOffSaveGameNotificationText", "false"),
            out var turnOffSaveGameNotificationText);
        _options.turnOffSaveGameNotificationText = turnOffSaveGameNotificationText;

        bool.TryParse(_con.Value("ExitToDesktop", "false"), out var exitToDesktop);
        _options.exitToDesktop = exitToDesktop;

        bool.TryParse(_con.Value("DisableSaveOnExit", "false"), out var disableSaveOnExit);
        _options.disableSaveOnExit = disableSaveOnExit;

        int.TryParse(_con.Value("MaximumSavesVisible", "3"), out var maximumSavesVisible);
        _options.maximumSavesVisible = maximumSavesVisible;

        bool.TryParse(_con.Value("Debug", "false"), out var debug);
        _options.debug = debug;

        bool.TryParse(_con.Value("SortByRealTime", "false"), out var sortByRealTime);
        _options.sortByRealTime = sortByRealTime;

        bool.TryParse(_con.Value("AscendingSort", "false"), out var ascendingSort);
        _options.ascendingSort = ascendingSort;

        bool.TryParse(_con.Value("EnableManualSaveControllerButton", "false"), out var enableManualSaveControllerButton);
        _options.enableManualSaveControllerButton = enableManualSaveControllerButton;
        
        var a1 = Enum.TryParse<KeyCode>(_con.Value("ManualSaveKeyBind", "K"), true, out var a2);
        if (a1)
        {
            _options.manualSaveKeyBind = a2;
            if (!external)
            {
                Debug.LogWarning($"[SaveNow]: Parsed '{a2}' for 'ManualSaveKeyBind'.");
            }
        }
        else
        {
            _options.manualSaveKeyBind = KeyCode.K;
            if (!external)
            {
                Debug.LogWarning($"[SaveNow]: Failed to parse key for 'ManualSaveKeyBind'. Setting to default K.");
            }
        }

        var b1 = Enum.TryParse<KeyCode>(_con.Value("ReloadConfigKeyBind", "F5"), true, out var b2);
        if (b1)
        {
            _options.reloadConfigKeyBind = b2;
            if (!external)
            {
                Debug.LogWarning($"[SaveNow]: Parsed '{b2}' for 'ReloadConfigKeyBind'.");
            }
        }
        else
        {
            _options.reloadConfigKeyBind = KeyCode.F5;
            if (!external)
            {
                Debug.LogWarning($"[SaveNow]: Failed to parse key for 'ReloadConfigKeyBind'. Setting to default F5.");
            }
        }

        var c1 = Enum.TryParse<GamePadButton>(_con.Value("ManualSaveControllerButton", "LT"), true, out var c2);
        if (c1)
        {
            _options.manualSaveControllerButton = GameButtonMap.Bindings[c2];
            if (!external)
            {
                Debug.LogWarning($"[SaveNow]: Parsed '{c2}' for 'ManualSaveControllerButton'.");
            }
        }
        else
        {
            _options.manualSaveControllerButton = GameButtonMap.Bindings[GamePadButton.LT];
            if (!external)
            {
                Debug.LogWarning($"[SaveNow]: Failed to parse key for 'ManualSaveControllerButton'. Setting to default LT.");
            }
        }

        _con.ConfigWrite();

        return _options;
    }

    [Serializable]
    public class Options
    {
        [FormerlySerializedAs("AutoSave")] public bool autoSave = true;
        [FormerlySerializedAs("SaveInterval")] public int saveInterval = 600;

        [FormerlySerializedAs("SortByRealTime")]
        public bool sortByRealTime;

        [FormerlySerializedAs("AscendingSort")]
        public bool ascendingSort;

        [FormerlySerializedAs("NewFileOnAutoSave")]
        public bool newFileOnAutoSave;

        [FormerlySerializedAs("MaximumSavesVisible")]
        public int maximumSavesVisible = 3;

        [FormerlySerializedAs("TurnOffTravelMessages")]
        public bool turnOffTravelMessages;

        [FormerlySerializedAs("TurnOffSaveGameNotificationText")]
        public bool turnOffSaveGameNotificationText;

        [FormerlySerializedAs("ExitToDesktop")]
        public bool exitToDesktop;

        [FormerlySerializedAs("DisableSaveOnExit")]
        public bool disableSaveOnExit;

        [FormerlySerializedAs("BackupSavesOnSave")]
        public bool backupSavesOnSave;

        [FormerlySerializedAs("NewFileOnManualSave")]
        public bool newFileOnManualSave;

        [FormerlySerializedAs("EnableManualSaveControllerButton")]
        public bool enableManualSaveControllerButton = true;

        [FormerlySerializedAs("ManualSaveControllerButton")]
        public int manualSaveControllerButton;

        [FormerlySerializedAs("ManualSaveKeyBind")]
        public KeyCode manualSaveKeyBind;

        [FormerlySerializedAs("ReloadConfigKeyBind")]
        public KeyCode reloadConfigKeyBind;

        [FormerlySerializedAs("Debug")] public bool debug;
    }
}