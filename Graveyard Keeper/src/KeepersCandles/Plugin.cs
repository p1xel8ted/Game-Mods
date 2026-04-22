namespace KeepersCandles;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal const string Souls = "souls";
    internal const string Candelabrum = "candelabrum";
    internal const string Incense = "incense";
    internal const string Column = "column";
    internal const string Church = "CHURCH";

    // Section names: plain "── Name ──" style, rendered in CM in bind-call order (Advanced first).
    // Legacy section names get rewritten to these by MigrateConfig() on first launch
    // so existing user customisations are preserved.
    private const string AdvancedSection = "── Advanced ──";
    private const string CandlesSection  = "── Candles & Incenses ──";
    private const string ChurchSection   = "── Church ──";
    private const string ControlsSection = "── Controls ──";
    private const string UpdatesSection  = "── Updates ──";

    private static readonly Dictionary<string, string> SectionRenames = new()
    {
        ["00. Advanced"]     = AdvancedSection,
        ["01. Distance"]     = CandlesSection,
        ["02. Features"]     = ChurchSection,
        ["03. Keybinds"]     = ControlsSection,
        ["── 1. Advanced ──"] = AdvancedSection,
        ["── 2. Candles ──"]  = CandlesSection,
        ["── 3. Church ──"]   = ChurchSection,
        ["── 4. Controls ──"] = ControlsSection,
        ["── 5. Updates ──"]  = UpdatesSection,
    };

    // Key-name renames inside [── Controls ──]. Old name on the left of a "key = value"
    // line gets rewritten to the new name so the user's value survives the rename.
    private static readonly Dictionary<string, string> ControlKeyRenames = new()
    {
        ["Extinguish Keybind"]           = "Extinguish Candle Keybind",
        ["Extinguish Controller Button"] = "Extinguish Candle Controller Button",
    };

    internal static readonly List<GameObject> ChurchColumnsList = [];

    internal static ManualLogSource Log { get; private set; }
    internal static bool DebugEnabled;

    internal static ConfigEntry<bool> Debug { get; private set; }
    internal static ConfigEntry<float> ExtinguishDistance { get; private set; }
    internal static ConfigEntry<bool> DirectionalArrow { get; private set; }
    internal static ConfigEntry<bool> ChurchColumns { get; private set; }
    internal static ConfigEntry<KeyboardShortcut> ExtinguishCandleKeyBind { get; private set; }
    internal static ConfigEntry<string> ExtinguishCandleControllerButton { get; private set; }
    internal static ConfigEntry<KeyboardShortcut> ExtinguishIncenseKeyBind { get; private set; }
    internal static ConfigEntry<string> ExtinguishIncenseControllerButton { get; private set; }
    internal static ConfigEntry<bool> CheckForUpdates { get; private set; }

    internal static Vector2 PlayerPosition => MainGame.me.player.grid_pos;

    private void Awake()
    {
        Log = Logger;
        LogHelper.Log = Logger;
        MigrateConfig();
        InitConfiguration();
        Lang.Init(Assembly.GetExecutingAssembly(), Log);
        SceneManager.sceneLoaded += (_, _) => Patches.OnGameBalanceLoaded();
        UpdateChecker.Register(Info, CheckForUpdates);
        DebugWarningDialog.Register(MyPluginInfo.PLUGIN_NAME, () => DebugEnabled);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);
    }

    // Rewrites legacy section headers (e.g. "[01. Distance]" → "[── Candles & Incenses ──]")
    // and renames legacy key names in [── Controls ──] (e.g. "Extinguish Keybind" →
    // "Extinguish Candle Keybind") so existing user values survive. Idempotent — re-running
    // on an already-migrated file is a no-op.
    private void MigrateConfig()
    {
        var path = Config.ConfigFilePath;
        if (!File.Exists(path)) return;

        string[] lines;
        try
        {
            lines = File.ReadAllLines(path);
        }
        catch (Exception ex)
        {
            Log.LogWarning($"[Migration] Could not read {path}: {ex.Message}");
            return;
        }

        var sectionsRenamed = 0;
        var keysRenamed = 0;
        var inControlsSection = false;

        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            var trimmed = line.TrimStart();

            if (trimmed.StartsWith("[") && trimmed.Contains("]"))
            {
                var end = trimmed.IndexOf(']');
                var header = trimmed.Substring(1, end - 1);
                if (SectionRenames.TryGetValue(header, out var newName))
                {
                    lines[i] = $"[{newName}]";
                    header = newName;
                    sectionsRenamed++;
                }
                inControlsSection = header == ControlsSection;
                continue;
            }

            if (!inControlsSection) continue;
            if (trimmed.StartsWith("#") || trimmed.Length == 0) continue;

            var eq = line.IndexOf('=');
            if (eq <= 0) continue;

            var keyName = line.Substring(0, eq).TrimEnd();
            if (!ControlKeyRenames.TryGetValue(keyName, out var newKey)) continue;

            lines[i] = $"{newKey} {line.Substring(eq)}";
            keysRenamed++;
        }

        if (sectionsRenamed == 0 && keysRenamed == 0) return;

        try
        {
            File.WriteAllLines(path, lines);
        }
        catch (Exception ex)
        {
            Log.LogWarning($"[Migration] Could not write {path}: {ex.Message}");
            return;
        }

        Log.LogInfo($"[Migration] Renamed {sectionsRenamed} section header(s) and {keysRenamed} key name(s). Existing user values preserved.");
        Config.Reload();
    }

    private void InitConfiguration()
    {
        // ── 1. Advanced ──
        Debug = Config.Bind(AdvancedSection, "Debug Logging", false,
            new ConfigDescription("Write detailed diagnostic info to the BepInEx log while you play. Turn this on before reporting a bug so the log has the context I need to help.", null,
                new ConfigurationManagerAttributes {Order = 100}));
        DebugEnabled = Debug.Value;
        Debug.SettingChanged += (_, _) => DebugEnabled = Debug.Value;

        // ── 2. Candles ──
        ExtinguishDistance = Config.Bind(CandlesSection, "Extinguish Distance", 1f,
            new ConfigDescription("How close you need to stand to a lit candle before the extinguish key picks it up. Higher values let you grab candles from further away. Snaps to quarter-unit increments.",
                new AcceptableValueRange<float>(1, 5),
                new ConfigurationManagerAttributes {Order = 100}));
        ExtinguishDistance.SettingChanged += (_, _) =>
        {
            ExtinguishDistance.Value = Mathf.Round(ExtinguishDistance.Value * 4) / 4;
        };

        DirectionalArrow = Config.Bind(CandlesSection, "Directional Arrow", true,
            new ConfigDescription("On: when a lit candle is nearby but out of reach, a pointer arrow appears above it to guide you in. Off: no arrow — you'll only see a speech bubble when you're too far.", null,
                new ConfigurationManagerAttributes {Order = 99}));
        DirectionalArrow.SettingChanged += (_, _) => Patches.ResetArrow();

        // ── 3. Church ──
        ChurchColumns = Config.Bind(ChurchSection, "Church Columns", true,
            new ConfigDescription("On: the stone columns running down the middle of the church are visible as normal. Off: the columns are hidden, giving you an unobstructed view of the altar and candles.", null,
                new ConfigurationManagerAttributes {Order = 100}));
        ChurchColumns.SettingChanged += (_, _) => Patches.ChurchColumnsToggle();

        // ── 4. Controls ──
        ExtinguishCandleKeyBind = Config.Bind(ControlsSection, "Extinguish Candle Keybind", new KeyboardShortcut(KeyCode.C),
            new ConfigDescription("Keyboard key you press to extinguish the nearest lit candle when you're in range.", null,
                new ConfigurationManagerAttributes {Order = 100}));

        ExtinguishCandleControllerButton = Config.Bind(ControlsSection, "Extinguish Candle Controller Button",
            Enum.GetName(typeof(GamePadButton), GamePadButton.DUp),
            new ConfigDescription("Controller button you press to extinguish the nearest lit candle when you're in range.",
                new AcceptableValueList<string>(Enum.GetNames(typeof(GamePadButton))),
                new ConfigurationManagerAttributes {Order = 99}));

        ExtinguishIncenseKeyBind = Config.Bind(ControlsSection, "Extinguish Incense Keybind", new KeyboardShortcut(KeyCode.None),
            new ConfigDescription("Keyboard key you press to extinguish the nearest lit incense when you're in range. Unbound by default — set a key here if you want to extinguish incenses. Leave unbound to only extinguish candles.", null,
                new ConfigurationManagerAttributes {Order = 98}));

        ExtinguishIncenseControllerButton = Config.Bind(ControlsSection, "Extinguish Incense Controller Button",
            Enum.GetName(typeof(GamePadButton), GamePadButton.None),
            new ConfigDescription("Controller button you press to extinguish the nearest lit incense when you're in range. Unbound by default — set a button here if you want to extinguish incenses on a controller.",
                new AcceptableValueList<string>(Enum.GetNames(typeof(GamePadButton))),
                new ConfigurationManagerAttributes {Order = 97}));

        // ── 5. Updates ──
        CheckForUpdates = Config.Bind(UpdatesSection, "Check for Updates", true,
            new ConfigDescription(
                "Show a notice on the main menu when a newer version of this mod is available on NexusMods. Click the notice to open the mod's page.",
                null,
                new ConfigurationManagerAttributes {Order = 100}));
    }

    internal static bool CanFindCandles()
    {
        return MainGame.game_started &&
               !MainGame.me.player.is_dead &&
               !MainGame.me.player.IsDisabled() &&
               !MainGame.paused &&
               BaseGUI.all_guis_closed;
    }

    internal static bool ShouldProcess(string id)
    {
        return !id.Contains(Souls) && (id.Contains(Candelabrum) || id.Contains(Incense));
    }

    internal static bool MatchesKeyword(string id, string keyword)
    {
        return !id.Contains(Souls) && id.Contains(keyword);
    }

    internal static string GetUnlitCandle(string input)
    {
        if (!input.Contains("_to_")) return string.Empty;

        var parts = input.Split(["_to_"], StringSplitOptions.None);
        return parts[0];
    }

    internal static List<WorldGameObject> GetCandles()  => GetLitBurners(Candelabrum);
    internal static List<WorldGameObject> GetIncenses() => GetLitBurners(Incense);

    private static List<WorldGameObject> GetLitBurners(string keyword)
    {
        var zone = MainGame.me.player.GetMyWorldZone();

        var all = zone
            ? zone.GetZoneWGOs().Where(wgo => MatchesKeyword(wgo.obj_id, keyword) || MatchesKeyword(wgo.obj_def.id, keyword)).ToList()
            : WorldMap._objs.Where(wgo => MatchesKeyword(wgo.obj_id, keyword) || MatchesKeyword(wgo.obj_def.id, keyword)).ToList();

        return all.Where(wgo => wgo.components.craft.is_crafting).ToList();
    }

    internal static string GetPath(Transform transform)
    {
        var path = transform.name;
        while (transform.parent)
        {
            transform = transform.parent;
            path = $"{transform.name}/{path}";
        }
        return path;
    }
}
