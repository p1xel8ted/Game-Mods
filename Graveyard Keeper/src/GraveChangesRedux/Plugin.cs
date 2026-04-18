namespace GraveChangesRedux;

[Harmony]
[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private const string AdvancedSection = "── Advanced ──";
    private const string ChangesSection  = "── Changes ──";
    private const string UpdatesSection  = "── Updates ──";

    private static readonly Dictionary<string, string> SectionRenames = new()
    {
        ["00. Advanced"] = AdvancedSection,
        ["01. Changes"]  = ChangesSection,
    };

    private const float MaxQualityValue = 30f;
    private static readonly SmartExpression MaxQualityExpression = SmartExpression.ParseExpression("30");

    private static readonly Dictionary<string,float> ItemDefBackups = new();
    private static readonly Dictionary<string,SmartExpression> ObjDefBackups = new();

    private static readonly string[] SkipThese = ["grave_empty", "_place", "place_", "grave_corp", "grave_exhume", "grave_ground"];
    private static ManualLogSource LOG { get; set; }
    private static ConfigEntry<bool> Debug { get; set; }
    internal static bool DebugEnabled;
    private static ConfigEntry<bool> ModifyGraves { get; set; }
    private static ConfigEntry<bool> ModifyObjects { get; set; }
    internal static ConfigEntry<bool> CheckForUpdates { get; private set; }

    private void Awake()
    {
        LOG = Logger;
        LogHelper.Log = Logger;
        MigrateRenamedSections();

        Debug = Config.Bind(AdvancedSection, "Debug Logging", false,
            new ConfigDescription("Write verbose grave/decoration quality-change diagnostics to the BepInEx console. Leave off for normal play.", null,
                new ConfigurationManagerAttributes {Order = 0}));
        DebugEnabled = Debug.Value;
        Debug.SettingChanged += (_, _) => DebugEnabled = Debug.Value;

        DebugWarningDialog.Register(MyPluginInfo.PLUGIN_NAME, () => DebugEnabled);

        ModifyGraves = Config.Bind(ChangesSection, "Modify Graves", true,
            new ConfigDescription("Max out the base quality of every grave-category item so your graveyard score is capped from the start.", null,
                new ConfigurationManagerAttributes {Order = 2}));
        ModifyGraves.SettingChanged += (_, _) => GameBalanceLoad();

        ModifyObjects = Config.Bind(ChangesSection, "Modify Decorations", false,
            new ConfigDescription("Max out the base quality of grave-quality decoration objects too — not just the gravestones/fences themselves.", null,
                new ConfigurationManagerAttributes {Order = 1}));
        ModifyObjects.SettingChanged += (_, _) => GameBalanceLoad();

        CheckForUpdates = Config.Bind(UpdatesSection, "Check for Updates", true, new ConfigDescription(
            "Show a notice on the main menu when a newer version of this mod is available on NexusMods. Click the notice to open the mod's page.",
            null,
            new ConfigurationManagerAttributes { Order = 0 }));

        UpdateChecker.Register(Info, CheckForUpdates);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);
    }

    // Rewrites old numbered section headers to the new "── Name ──" style so existing
    // user values survive the rename. Idempotent.
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
            LOG.LogWarning($"[Migration] Could not read {path} for section rename: {ex.Message}");
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
            LOG.LogWarning($"[Migration] Could not write {path} after section rename: {ex.Message}");
            return;
        }

        LOG.LogInfo($"[Migration] Renamed {renamed} legacy config section header(s) to the '── Name ──' style. Existing user values preserved.");
        Config.Reload();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameBalance), nameof(GameBalance.LoadGameBalance))]
    public static void GameBalanceLoad()
    {
        foreach (var itemDef in GameBalance.me.items_data.Where(itemDef => itemDef.id.StartsWith("grave", StringComparison.OrdinalIgnoreCase) && itemDef.quality_type is not ItemDefinition.QualityType.Stars))
        {
            if (SkipThese.Any(a => itemDef.id.Contains(a)))
            {
                if (DebugEnabled)
                {
                    LOG.LogInfo($"ITEM: Skipping {itemDef.id} - {itemDef.quality}");
                }
                continue;
            }

            TryAdd(ItemDefBackups, itemDef.id, itemDef.quality);

            if (ModifyGraves.Value)
            {
                itemDef.quality = MaxQualityValue;
                if (DebugEnabled)
                {
                    LOG.LogInfo($"ITEM: Set quality of {itemDef.id} to {MaxQualityValue}");
                }
            }
            else
            {
                itemDef.quality = ItemDefBackups[itemDef.id];
            }
        }

        foreach (var objDef in GameBalance.me.objs_data.Where(a => a.quality_type == ObjectDefinition.QualityType.Grave))
        {
            if (SkipThese.Any(a => objDef.id.Contains(a)))
            {
                if (DebugEnabled)
                {
                    LOG.LogInfo($"OBJECT: Skipping {objDef.id} - {objDef.quality.GetRawExpressionString()}");
                }
                continue;
            }

            TryAdd(ObjDefBackups, objDef.id, objDef.quality);

            if (ModifyObjects.Value)
            {
                objDef.quality = MaxQualityExpression;
                if (DebugEnabled)
                {
                    LOG.LogInfo($"OBJECT: Set quality of {objDef.id} to {MaxQualityValue}");
                }
            }
            else
            {
                objDef.quality = ObjDefBackups[objDef.id];
            }
        }
    }

    private static bool TryAdd<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        if (dictionary.ContainsKey(key)) return false;
        dictionary.Add(key, value);
        return true;
    }
}
