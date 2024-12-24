namespace GraveChangesRedux;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("p1xel8ted.gyk.gykhelper", "3.1.0")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.gravechangesredux";
    private const string PluginName = "Grave Changes Redux";
    private const string PluginVer = "0.1.1";

    private static readonly Dictionary<string,float> ItemDefBackups = new();
    private static readonly Dictionary<string,SmartExpression> ObjDefBackups = new();

    private static readonly string[] SkipThese = ["grave_empty", "_place", "place_", "grave_corp", "grave_exhume", "grave_ground"];
    private static ManualLogSource LOG { get; set; }
    private static ConfigEntry<bool> ModifyGraves { get; set; }
    private static ConfigEntry<bool> ModifyObjects { get; set; }
    private static SmartExpression MaxQuality
    {
        get
        {
            var se = SmartExpression.ParseExpression("30");
            se.default_value = 30f;
            se._simpified_float = 30f;
            return se;
        }
    }

    private void Awake()
    {
        ModifyGraves = Config.Bind("01. Changes", "Modify Graves", true, new ConfigDescription("Max out the quality of grave items.", null, new ConfigurationManagerAttributes {Order = 2}));
        ModifyGraves.SettingChanged += (_, _) => GameBalanceLoad();
        ModifyObjects = Config.Bind("01. Changes", "Modify Decorations", false, new ConfigDescription("Max out the quality of decorative items.", null, new ConfigurationManagerAttributes {Order = 1}));
        ModifyObjects.SettingChanged += (_, _) => GameBalanceLoad();
        Actions.GameBalanceLoad += GameBalanceLoad;
        LOG = Logger;
        LOG.LogInfo($"Plugin {PluginName} is loaded!");
    }

    private static void GameBalanceLoad()
    {
        foreach (var itemDef in GameBalance.me.items_data.Where(itemDef => itemDef.id.StartsWith("grave", StringComparison.OrdinalIgnoreCase) && itemDef.quality_type is not ItemDefinition.QualityType.Stars))
        {
            if (SkipThese.Any(a => itemDef.id.Contains(a)))
            {
                LOG.LogInfo($"ITEM: Skipping {itemDef.id} - {itemDef.quality}");
                continue;
            }

            if (!ItemDefBackups.ContainsKey(itemDef.id))
            {
                ItemDefBackups.Add(itemDef.id, itemDef.quality);
            }

            itemDef.quality = ModifyGraves.Value ? MaxQuality._simpified_float : ItemDefBackups[itemDef.id];
            
            LOG.LogInfo($"ITEM: Set quality of {itemDef.id} to {MaxQuality._simpified_float}");
        }

        foreach (var objDef in GameBalance.me.objs_data.Where(a => a.quality_type == ObjectDefinition.QualityType.Grave))
        {
            if (SkipThese.Any(a => objDef.id.Contains(a)))
            {
                LOG.LogInfo($"OBJECT: Skipping {objDef.id} - {objDef.quality.GetRawExpressionString()}");
                continue;
            }
            
            if(!ObjDefBackups.ContainsKey(objDef.id))
            {
                ObjDefBackups.Add(objDef.id, objDef.quality);
            }
            
            objDef.quality = ModifyObjects.Value ? MaxQuality : ObjDefBackups[objDef.id];
            
            LOG.LogInfo($"OBJECT: Set quality of {objDef.id} to {MaxQuality._simpified_float}");
        }
    }
}