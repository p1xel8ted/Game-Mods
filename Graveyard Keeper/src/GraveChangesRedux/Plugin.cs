namespace GraveChangesRedux;

[Harmony]
[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.gravechangesredux";
    private const string PluginName = "Grave Changes Redux";
    private const string PluginVer = "0.1.3";

    private const float MaxQualityValue = 30f;
    private static readonly SmartExpression MaxQualityExpression = SmartExpression.ParseExpression("30");

    private static readonly Dictionary<string,float> ItemDefBackups = new();
    private static readonly Dictionary<string,SmartExpression> ObjDefBackups = new();

    private static readonly string[] SkipThese = ["grave_empty", "_place", "place_", "grave_corp", "grave_exhume", "grave_ground"];
    private static ManualLogSource LOG { get; set; }
    private static ConfigEntry<bool> ModifyGraves { get; set; }
    private static ConfigEntry<bool> ModifyObjects { get; set; }

    private void Awake()
    {
        ModifyGraves = Config.Bind("01. Changes", "Modify Graves", true, new ConfigDescription("Max out the quality of grave items.", null, new ConfigurationManagerAttributes {Order = 2}));
        ModifyGraves.SettingChanged += (_, _) => GameBalanceLoad();
        ModifyObjects = Config.Bind("01. Changes", "Modify Decorations", false, new ConfigDescription("Max out the quality of decorative items.", null, new ConfigurationManagerAttributes {Order = 1}));
        ModifyObjects.SettingChanged += (_, _) => GameBalanceLoad();
        LOG = Logger;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        StartupLogger.PrintModLoaded(PluginName, LOG);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameBalance), nameof(GameBalance.LoadGameBalance))]
    public static void GameBalanceLoad()
    {
        foreach (var itemDef in GameBalance.me.items_data.Where(itemDef => itemDef.id.StartsWith("grave", StringComparison.OrdinalIgnoreCase) && itemDef.quality_type is not ItemDefinition.QualityType.Stars))
        {
            if (SkipThese.Any(a => itemDef.id.Contains(a)))
            {
                LOG.LogInfo($"ITEM: Skipping {itemDef.id} - {itemDef.quality}");
                continue;
            }

            TryAdd(ItemDefBackups, itemDef.id, itemDef.quality);

            if (ModifyGraves.Value)
            {
                itemDef.quality = MaxQualityValue;
                LOG.LogInfo($"ITEM: Set quality of {itemDef.id} to {MaxQualityValue}");
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
                LOG.LogInfo($"OBJECT: Skipping {objDef.id} - {objDef.quality.GetRawExpressionString()}");
                continue;
            }

            TryAdd(ObjDefBackups, objDef.id, objDef.quality);

            if (ModifyObjects.Value)
            {
                objDef.quality = MaxQualityExpression;
                LOG.LogInfo($"OBJECT: Set quality of {objDef.id} to {MaxQualityValue}");
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
