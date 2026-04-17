namespace GraveChangesRedux;

[Harmony]
[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private const float MaxQualityValue = 30f;
    private static readonly SmartExpression MaxQualityExpression = SmartExpression.ParseExpression("30");

    private static readonly Dictionary<string,float> ItemDefBackups = new();
    private static readonly Dictionary<string,SmartExpression> ObjDefBackups = new();

    private static readonly string[] SkipThese = ["grave_empty", "_place", "place_", "grave_corp", "grave_exhume", "grave_ground"];
    private static ManualLogSource LOG { get; set; }
    private static ConfigEntry<bool> DebugEnabled { get; set; }
    private static ConfigEntry<bool> ModifyGraves { get; set; }
    private static ConfigEntry<bool> ModifyObjects { get; set; }
    internal static ConfigEntry<bool> CheckForUpdates { get; private set; }

    private void Awake()
    {
        DebugEnabled = Config.Bind("00. Advanced", "Debug Logging", false, new ConfigDescription("Toggle debug logging on or off.", null, new ConfigurationManagerAttributes {Order = 0}));
        ModifyGraves = Config.Bind("01. Changes", "Modify Graves", true, new ConfigDescription("Max out the quality of grave items.", null, new ConfigurationManagerAttributes {Order = 2}));
        ModifyGraves.SettingChanged += (_, _) => GameBalanceLoad();
        ModifyObjects = Config.Bind("01. Changes", "Modify Decorations", false, new ConfigDescription("Max out the quality of decorative items.", null, new ConfigurationManagerAttributes {Order = 1}));
        ModifyObjects.SettingChanged += (_, _) => GameBalanceLoad();

        CheckForUpdates = Config.Bind("── Updates ──", "Check for Updates", true, new ConfigDescription(
            "Show a notice on the main menu when a newer version of this mod is available on NexusMods. Click the notice to open the mod's page.",
            null,
            new ConfigurationManagerAttributes { Order = 0 }));

        LOG = Logger;
        UpdateChecker.Register(Info, CheckForUpdates);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameBalance), nameof(GameBalance.LoadGameBalance))]
    public static void GameBalanceLoad()
    {
        foreach (var itemDef in GameBalance.me.items_data.Where(itemDef => itemDef.id.StartsWith("grave", StringComparison.OrdinalIgnoreCase) && itemDef.quality_type is not ItemDefinition.QualityType.Stars))
        {
            if (SkipThese.Any(a => itemDef.id.Contains(a)))
            {
                if (DebugEnabled.Value)
                {
                    LOG.LogInfo($"ITEM: Skipping {itemDef.id} - {itemDef.quality}");
                }
                continue;
            }

            TryAdd(ItemDefBackups, itemDef.id, itemDef.quality);

            if (ModifyGraves.Value)
            {
                itemDef.quality = MaxQualityValue;
                if (DebugEnabled.Value)
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
                if (DebugEnabled.Value)
                {
                    LOG.LogInfo($"OBJECT: Skipping {objDef.id} - {objDef.quality.GetRawExpressionString()}");
                }
                continue;
            }

            TryAdd(ObjDefBackups, objDef.id, objDef.quality);

            if (ModifyObjects.Value)
            {
                objDef.quality = MaxQualityExpression;
                if (DebugEnabled.Value)
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
