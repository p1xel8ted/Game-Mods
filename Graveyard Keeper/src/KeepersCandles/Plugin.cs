namespace KeepersCandles;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal const string Souls = "souls";
    internal const string Candelabrum = "candelabrum";
    internal const string Column = "column";
    internal const string Church = "CHURCH";

    // Section names. New scheme: ── N. Name ── (bind-order driven — Advanced first).
    // Legacy section names get rewritten to these by MigrateRenamedSections() on first launch
    // of the new version, so existing user customisations are preserved.
    private const string AdvancedSection = "── 1. Advanced ──";
    private const string CandlesSection  = "── 2. Candles ──";
    private const string ChurchSection   = "── 3. Church ──";
    private const string ControlsSection = "── 4. Controls ──";
    private const string UpdatesSection  = "── 5. Updates ──";

    private static readonly Dictionary<string, string> SectionRenames = new()
    {
        ["00. Advanced"] = AdvancedSection,
        ["01. Distance"] = CandlesSection,
        ["02. Features"] = ChurchSection,
        ["03. Keybinds"] = ControlsSection,
    };

    internal static readonly List<GameObject> ChurchColumnsList = [];

    internal static ManualLogSource Log { get; private set; }
    internal static bool DebugEnabled;
    internal static bool DebugDialogShown;

    internal static ConfigEntry<bool> Debug { get; private set; }
    internal static ConfigEntry<float> ExtinguishDistance { get; private set; }
    internal static ConfigEntry<bool> DirectionalArrow { get; private set; }
    internal static ConfigEntry<bool> ChurchColumns { get; private set; }
    internal static ConfigEntry<KeyboardShortcut> ExtinguishKeyBind { get; private set; }
    internal static ConfigEntry<string> ExtinguishControllerButton { get; private set; }
    internal static ConfigEntry<bool> CheckForUpdates { get; private set; }

    internal static Vector2 PlayerPosition => MainGame.me.player.grid_pos;

    private void Awake()
    {
        Log = Logger;
        LogHelper.Log = Logger;
        MigrateRenamedSections();
        InitConfiguration();
        Lang.Init(Assembly.GetExecutingAssembly(), Log);
        SceneManager.sceneLoaded += (_, _) => Patches.OnGameBalanceLoaded();
        UpdateChecker.Register(Info, CheckForUpdates);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);
    }

    // Rewrites old "[01. Distance]" style headers to "[── 2. Candles ──]" in the .cfg file
    // so existing user values survive the section rename. Idempotent — re-running on an
    // already-migrated file is a no-op (no old headers left to find).
    private void MigrateRenamedSections()
    {
        var path = Config.ConfigFilePath;
        if (!File.Exists(path)) return;

        string content;
        try
        {
            content = File.ReadAllText(path);
        }
        catch (Exception ex)
        {
            Log.LogWarning($"[Migration] Could not read {path} for section rename: {ex.Message}");
            return;
        }

        var renamed = 0;
        foreach (var kv in SectionRenames)
        {
            var oldHeader = $"[{kv.Key}]";
            var newHeader = $"[{kv.Value}]";
            if (!content.Contains(oldHeader)) continue;
            content = content.Replace(oldHeader, newHeader);
            renamed++;
        }
        if (renamed == 0) return;

        try
        {
            File.WriteAllText(path, content);
        }
        catch (Exception ex)
        {
            Log.LogWarning($"[Migration] Could not write {path} after section rename: {ex.Message}");
            return;
        }

        Log.LogInfo($"[Migration] Renamed {renamed} legacy config section header(s) to the '── N. Name ──' style. Existing user values preserved.");
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
        ExtinguishKeyBind = Config.Bind(ControlsSection, "Extinguish Keybind", new KeyboardShortcut(KeyCode.C),
            new ConfigDescription("Keyboard key you press to extinguish the nearest lit candle when you're in range.", null,
                new ConfigurationManagerAttributes {Order = 100}));

        ExtinguishControllerButton = Config.Bind(ControlsSection, "Extinguish Controller Button",
            Enum.GetName(typeof(GamePadButton), GamePadButton.DUp),
            new ConfigDescription("Controller button you press to extinguish the nearest lit candle when you're in range.",
                new AcceptableValueList<string>(Enum.GetNames(typeof(GamePadButton))),
                new ConfigurationManagerAttributes {Order = 99}));

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
        return !id.Contains(Souls) && id.Contains(Candelabrum);
    }

    internal static string GetUnlitCandle(string input)
    {
        if (!input.Contains("_to_")) return string.Empty;

        var parts = input.Split(["_to_"], StringSplitOptions.None);
        return parts[0];
    }

    internal static List<WorldGameObject> GetCandles()
    {
        var zone = MainGame.me.player.GetMyWorldZone();

        var allCandles = zone
            ? zone.GetZoneWGOs().Where(wgo => ShouldProcess(wgo.obj_id) || ShouldProcess(wgo.obj_def.id)).ToList()
            : WorldMap._objs.Where(wgo => ShouldProcess(wgo.obj_id) || ShouldProcess(wgo.obj_def.id)).ToList();

        return allCandles.Where(wgo => wgo.components.craft.is_crafting).ToList();
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
