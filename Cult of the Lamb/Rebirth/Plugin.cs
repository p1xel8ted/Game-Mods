using MonoMod.Utils;
using Shared;

namespace Rebirth;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("io.github.xhayper.COTL_API", "0.2.8")]
[BepInDependency("com.bepis.bepinex.configurationmanager", "18.3")]
[HarmonyPatch]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.cotl.rebirth";
    private const string PluginName = "Rebirth";
    private const string PluginVer = "1.1.1";

    public static ManualLogSource Log { get; private set; }
    public static string PluginPath { get; private set; }
    internal static ConfigEntry<bool> RebirthOldFollowers { get; private set; }
    public static InventoryItem.ITEM_TYPE RebirthItem { get; private set; }
    private CustomObjective RebirthCollectItemQuest { get; set; }
    internal static RebirthItem RebirthItemInstance { get; private set; }
    
    public static readonly ModdedSaveData<List<int>> RebirthSaveData = new(PluginGuid);

    private void Awake()
    {
          
        RebirthSaveData.LoadOrder = ModdedSaveLoadOrder.LOAD_AS_SOON_AS_POSSIBLE;
        ModdedSaveManager.RegisterModdedSave(RebirthSaveData);
        
        RebirthOldFollowers = Config.Bind("General", "Rebirth Old Followers", false, "Allow old followers to be reborn.");
        Log = Logger;

        PluginPath = Path.GetDirectoryName(Info.Location) ?? throw new DirectoryNotFoundException();

        CustomFollowerCommandManager.Add(new RebirthFollowerCommand());
        CustomFollowerCommandManager.Add(new RebirthSubCommand());
        RebirthItem = CustomItemManager.Add(new RebirthItem());
        CustomMissionManager.Add(new MissionItem());

        RebirthItemInstance = new RebirthItem();

        RebirthCollectItemQuest = CustomObjectiveManager.CollectItem(RebirthItem, Random.Range(15, 26), false, FollowerLocation.Dungeon1_1, 4800f);
        RebirthCollectItemQuest.InitialQuestText = $"Please leader, please! I'm {"weary of this existence".Wave()} and seek to be reborn! I will do anything for you! Can you please help me?";
        CustomSettingsManager.AddBepInExConfig("Rebirth", "Rebirth Old Followers",RebirthOldFollowers, b =>
        {
            Log.LogInfo("Setting 'Rebirth Old Followers' to " + b);
        });

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Helpers.PrintModLoaded(PluginName, Logger);

    }
}