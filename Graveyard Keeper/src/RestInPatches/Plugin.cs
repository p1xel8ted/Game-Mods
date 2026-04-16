namespace RestInPatches;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.restinpatches";
    private const string PluginName = "Rest In Patches";
    private const string PluginVer = "0.1.1";

    private static ManualLogSource Log { get; set; }
    internal static Sprite ArrowLeftSprite { get; private set; }
    private static ConfigEntry<bool> KeepRunningInBackground { get; set; }
    private static ConfigEntry<bool> MuteWhenUnfocused { get; set; }
    internal static ConfigEntry<int> MaxFootprints { get; private set; }

    private void Awake()
    {
        Log = Logger;
        InitConfiguration();
        ArrowLeftSprite = LoadEmbeddedSprite("RestInPatches.Resources.ui_btn_arrow_left.png", "ui_btn_arrow_left");
        if (ArrowLeftSprite == null)
        {
            Log.LogError("Failed to load embedded arrow sprite; craft arrow fix will be inactive.");
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        Application.focusChanged += OnFocusChanged;

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
    }

    private void InitConfiguration()
    {
        KeepRunningInBackground = Config.Bind("01. Application", "Keep Running In Background", true,
            "Keeps the game simulating when its window isn't focused.");
        KeepRunningInBackground.SettingChanged += (_, _) =>
        {
            Application.runInBackground = KeepRunningInBackground.Value;
        };

        MuteWhenUnfocused = Config.Bind("01. Application", "Mute When Unfocused", true,
            "Mutes all audio while the game window isn't focused.");
        MuteWhenUnfocused.SettingChanged += (_, _) =>
        {
            AudioListener.volume = MuteWhenUnfocused.Value && !Application.isFocused ? 0f : 1f;
        };

        MaxFootprints = Config.Bind("02. Footprints", "Max Footprints", 1000,
            new ConfigDescription("Maximum number of footprints kept in the world before the oldest are removed. 0 disables the cap.",
                new AcceptableValueRange<int>(0, 10000)));
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
