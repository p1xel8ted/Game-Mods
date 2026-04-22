namespace RestInPatches;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private const string ApplicationSection = "── Application ──";
    private const string FootprintsSection  = "── Footprints ──";
    private const string UpdatesSection     = "── Updates ──";

    private static readonly Dictionary<string, string> SectionRenames = new()
    {
        ["01. Application"] = ApplicationSection,
        ["02. Footprints"]  = FootprintsSection,
    };

    private static ManualLogSource Log { get; set; }
    internal static Sprite ArrowLeftSprite { get; private set; }
    internal static Sprite ArrowUpSprite { get; private set; }
    internal static Sprite ArrowDownSprite { get; private set; }
    private static ConfigEntry<bool> KeepRunningInBackground { get; set; }
    private static ConfigEntry<bool> MuteWhenUnfocused { get; set; }
    internal static ConfigEntry<int> MaxFootprints { get; private set; }
    internal static ConfigEntry<bool> CheckForUpdates { get; private set; }

    private void Awake()
    {
        Log = Logger;
        MigrateRenamedSections();
        InitConfiguration();
        ArrowLeftSprite = LoadEmbeddedSprite("RestInPatches.Resources.ui_btn_arrow_left.png", "ui_btn_arrow_left");
        if (ArrowLeftSprite == null)
        {
            Log.LogError("Failed to load embedded left arrow sprite; amount-button arrow fix will be inactive.");
        }

        ArrowUpSprite = LoadEmbeddedSprite("RestInPatches.Resources.ui_btn_arrow_up.png", "ui_btn_arrow_up");
        if (ArrowUpSprite == null)
        {
            Log.LogError("Failed to load embedded up arrow sprite; expanded details-toggle arrow fix will be inactive.");
        }

        ArrowDownSprite = LoadEmbeddedSprite("RestInPatches.Resources.ui_btn_arrow_dn.png", "ui_btn_arrow_dn");
        if (ArrowDownSprite == null)
        {
            Log.LogError("Failed to load embedded down arrow sprite; collapsed details-toggle arrow fix will be inactive.");
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        Application.focusChanged += OnFocusChanged;

        UpdateChecker.Register(Info, CheckForUpdates);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);
    }

    // Rewrites legacy numbered section headers to the plain "── Name ──" style so existing
    // user values survive the rename. Idempotent.
    private void MigrateRenamedSections()
    {
        var path = Config.ConfigFilePath;
        if (!File.Exists(path)) return;

        string content;
        try { content = File.ReadAllText(path); }
        catch (Exception ex) { Log.LogWarning($"[Migration] Could not read {path}: {ex.Message}"); return; }

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

        try { File.WriteAllText(path, content); }
        catch (Exception ex) { Log.LogWarning($"[Migration] Could not write {path}: {ex.Message}"); return; }

        Log.LogInfo($"[Migration] Renamed {renamed} legacy config section header(s) to the '── Name ──' style. Existing user values preserved.");
        Config.Reload();
    }

    private void InitConfiguration()
    {
        KeepRunningInBackground = Config.Bind(ApplicationSection, "Keep Running In Background", true,
            "Keeps the game simulating when its window isn't focused.");
        KeepRunningInBackground.SettingChanged += (_, _) =>
        {
            Application.runInBackground = KeepRunningInBackground.Value;
        };

        MuteWhenUnfocused = Config.Bind(ApplicationSection, "Mute When Unfocused", true,
            "Mutes all audio while the game window isn't focused.");
        MuteWhenUnfocused.SettingChanged += (_, _) =>
        {
            AudioListener.volume = MuteWhenUnfocused.Value && !Application.isFocused ? 0f : 1f;
        };

        MaxFootprints = Config.Bind(FootprintsSection, "Max Footprints", 1000,
            new ConfigDescription("Maximum number of footprints kept in the world before the oldest are removed. 0 disables the cap.",
                new AcceptableValueRange<int>(0, 10000)));

        CheckForUpdates = Config.Bind(UpdatesSection, "Check for Updates", true,
            "Show a notice on the main menu when a newer version of this mod is available on NexusMods. Click the notice to open the mod's page.");
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        OnFocusChanged(true);
    }

    private static void OnFocusChanged(bool focused)
    {
        Application.runInBackground = KeepRunningInBackground.Value;
        AudioListener.volume = MuteWhenUnfocused.Value && !focused ? 0f : 1f;
    }

    private static Sprite LoadEmbeddedSprite(string resourceName, string spriteName)
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            return null;
        }

        var bytes = new byte[stream.Length];
        _ = stream.Read(bytes, 0, bytes.Length);

        var tex = new Texture2D(2, 2, TextureFormat.RGBA32, false)
        {
            name = spriteName,
            filterMode = FilterMode.Bilinear,
            wrapMode = TextureWrapMode.Clamp
        };
        tex.LoadImage(bytes);

        var sprite = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 1f);
        sprite.name = spriteName;
        return sprite;
    }
}
