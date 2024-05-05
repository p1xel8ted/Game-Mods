using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Serialization;

namespace MiscBitsAndBobs;

public static class Config
{
    private static Options _options;
    private static ConfigReader _con;

    public static Options GetOptions(bool external = false)
    {
        _options = new Options();
        _con = new ConfigReader(external);

        bool.TryParse(_con.Value("QuietMusicInGUI", "true"), out var quietMusicInGui);
        _options.quietMusicInGui = quietMusicInGui;

        bool.TryParse(_con.Value("CondenseXpBar", "true"), out var condenseXpBar);
        _options.condenseXpBar = condenseXpBar;

        bool.TryParse(_con.Value("ModifyPlayerMovementSpeed", "true"), out var modifyPlayerMovementSpeed);
        _options.modifyPlayerMovementSpeed = modifyPlayerMovementSpeed;

        var playerMs = float.TryParse(_con.Value("PlayerMovementSpeed", "1.0"), NumberStyles.Number, CultureInfo.InvariantCulture, out var playerMovementSpeed);
        if (playerMs)
        {
            _options.playerMovementSpeed = playerMovementSpeed < 1 ? 1.0f : playerMovementSpeed;
        }

        bool.TryParse(_con.Value("ModifyPorterMovementSpeed", "true"), out var modifyPorterMovementSpeed);
        _options.modifyPorterMovementSpeed = modifyPorterMovementSpeed;

        var porterMs = float.TryParse(_con.Value("PorterMovementSpeed", "1.0"), out var porterMovementSpeed);
        if (porterMs)
        {
            _options.porterMovementSpeed = porterMovementSpeed < 1 ? 1.0f : porterMovementSpeed;
        }

        bool.TryParse(_con.Value("HalloweenNow", "false"), out var halloweenNow);
        _options.halloweenNow = halloweenNow;

        bool.TryParse(_con.Value("HideCreditsButtonOnMainMenu", "true"), out var hideCreditsButtonOnMainMenu);
        _options.hideCreditsButtonOnMainMenu = hideCreditsButtonOnMainMenu;

        bool.TryParse(_con.Value("SkipIntroVideoOnNewGame", "false"), out var skipIntroVideoOnNewGame);
        _options.skipIntroVideoOnNewGame = skipIntroVideoOnNewGame;

        bool.TryParse(_con.Value("DisableCinematicLetterboxing", "true"), out var disableCinematicLetterboxing);
        _options.disableCinematicLetterboxing = disableCinematicLetterboxing;

        bool.TryParse(_con.Value("KitsuneKitoMode", "false"), out var kitsuneKitoMode);
        _options.kitsuneKitoMode = kitsuneKitoMode;

        bool.TryParse(_con.Value("LessenFootprintImpact", "false"), out var lessenFootprintImpact);
        _options.lessenFootprintImpact = lessenFootprintImpact;

        bool.TryParse(_con.Value("RemovePrayerOnUse", "false"), out var removePrayerOnUse);
        _options.removePrayerOnUse = removePrayerOnUse;

        bool.TryParse(_con.Value("AddCoalToTavernOven", "true"), out var addCoalToTavernOven);
        _options.addCoalToTavernOven = addCoalToTavernOven;

        bool.TryParse(_con.Value("AddZombiesToPyreAndCrematorium", "true"), out var addZombiesToPyreAndCrematorium);
        _options.addZombiesToPyreAndCrematorium = addZombiesToPyreAndCrematorium;

        bool.TryParse(_con.Value("KeepGamingRunningInBackground", "true"), out var keepGamingRunningInBackground);
        _options.keepGamingRunningInBackground = keepGamingRunningInBackground;

        var key = Enum.TryParse<KeyCode>(_con.Value("ReloadConfigKeyBind", "F5"), true, out var b);
        if (key)
        {
            _options.reloadConfigKeyBind = b;
            if (!external)
            {
                Debug.LogWarning($"[MiscBitsAndBobs]: Parsed '{b}' for 'ReloadConfigKeyBind'.");
            }
        }
        else
        {
            _options.reloadConfigKeyBind = KeyCode.F5;
            if (!external)
            {
                Debug.LogWarning($"[MiscBitsAndBobs]: Failed to parse key for 'ReloadConfigKeyBind'. Setting to default F5.");
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

        [FormerlySerializedAs("KeepGamingRunningInBackground")]
        public bool keepGamingRunningInBackground;

        [FormerlySerializedAs("ModifyPlayerMovementSpeed")]
        public bool modifyPlayerMovementSpeed;

        [FormerlySerializedAs("PlayerMovementSpeed")]
        public float playerMovementSpeed = 1.0f;

        [FormerlySerializedAs("ModifyPorterMovementSpeed")]
        public bool modifyPorterMovementSpeed;

        [FormerlySerializedAs("PorterMovementSpeed")]
        public float porterMovementSpeed = 1.0f;

        [FormerlySerializedAs("QuietMusicInGui")]
        public bool quietMusicInGui;

        [FormerlySerializedAs("HalloweenNow")] public bool halloweenNow;

        [FormerlySerializedAs("HideCreditsButtonOnMainMenu")]
        public bool hideCreditsButtonOnMainMenu;

        [FormerlySerializedAs("CondenseXpBar")]
        public bool condenseXpBar;

        [FormerlySerializedAs("SkipIntroVideoOnNewGame")]
        public bool skipIntroVideoOnNewGame;

        [FormerlySerializedAs("DisableCinematicLetterboxing")]
        public bool disableCinematicLetterboxing;

        [FormerlySerializedAs("KitsuneKitoMode")]
        public bool kitsuneKitoMode;

        [FormerlySerializedAs("LessenFootprintImpact")]
        public bool lessenFootprintImpact;

        [FormerlySerializedAs("RemovePrayerOnUse")]
        public bool removePrayerOnUse;

        [FormerlySerializedAs("AddCoalToTavernOven")]
        public bool addCoalToTavernOven;

        [FormerlySerializedAs("AddZombiesToPyreAndCrematorium")]
        public bool addZombiesToPyreAndCrematorium;
    }
}