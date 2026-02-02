using Lamb.UI;
using Shared;

namespace Rebirth;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("io.github.xhayper.COTL_API", "0.3.1")]
[BepInDependency("com.bepis.bepinex.configurationmanager", "18.4.1")]
[HarmonyPatch]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.cotl.rebirth";
    private const string PluginName = "Rebirth";
    private const string PluginVer = "1.1.4";

    private const string RebirthSection = "── Rebirth ──";
    private const string TokenDropsSection = "── Token Drops ──";
    private const string DungeonChestsSection = "── Dungeon Chests ──";
    private const string RefinerySection = "── Refinery ──";
    private const string MissionsSection = "── Missions ──";

    public static ManualLogSource Log { get; private set; }
    public static string PluginPath { get; private set; }
    public static InventoryItem.ITEM_TYPE RebirthItem { get; private set; }
    private CustomObjective RebirthCollectItemQuest { get; set; }
    public static readonly ModdedSaveData<List<int>> RebirthSaveData = new(PluginGuid);

    // ── Rebirth ──
    internal static ConfigEntry<bool> RebirthOldFollowers { get; private set; }
    internal static ConfigEntry<bool> PreserveUniqueFollowers { get; private set; }
    internal static ConfigEntry<int> XpPenaltyChance { get; private set; }
    internal static ConfigEntry<int> XpPenaltyMultiplier { get; private set; }
    internal static ConfigEntry<int> TokenCost { get; private set; }

    // ── Token Drops ──
    internal static ConfigEntry<int> EnemyDropRate { get; private set; }
    internal static ConfigEntry<int> DropMinQuantity { get; private set; }
    internal static ConfigEntry<int> DropMaxQuantity { get; private set; }

    // ── Dungeon Chests ──
    internal static ConfigEntry<int> ChestSpawnChance { get; private set; }
    internal static ConfigEntry<int> ChestMinAmount { get; private set; }
    internal static ConfigEntry<int> ChestMaxAmount { get; private set; }

    // ── Refinery ──
    internal static ConfigEntry<int> BoneCost { get; private set; }
    internal static ConfigEntry<int> RefineryDuration { get; private set; }

    // ── Missions ──
    internal static ConfigEntry<int> MissionRewardMin { get; private set; }
    internal static ConfigEntry<int> MissionRewardMax { get; private set; }
    internal static ConfigEntry<int> MissionBaseChance { get; private set; }

    private void Awake()
    {
        RebirthSaveData.LoadOrder = ModdedSaveLoadOrder.LOAD_AS_SOON_AS_POSSIBLE;
        ModdedSaveManager.RegisterModdedSave(RebirthSaveData);

        Log = Logger;
        PluginPath = Path.GetDirectoryName(Info.Location) ?? throw new DirectoryNotFoundException();

        BindConfig();

        CustomFollowerCommandManager.Add(new RebirthFollowerCommand());
        CustomFollowerCommandManager.Add(new RebirthSubCommand());
        RebirthItem = CustomItemManager.Add(new RebirthItem());
        CustomMissionManager.Add(new MissionItem());

        RebirthCollectItemQuest = CustomObjectiveManager.CollectItem(RebirthItem, Random.Range(15, 26), false, FollowerLocation.Dungeon1_1, 4800f);
        RebirthCollectItemQuest.InitialQuestText = $"Please leader, please! I'm {"weary of this existence".Wave()} and seek to be reborn! I will do anything for you! Can you please help me?";

        RegisterSettings();

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Helpers.PrintModLoaded(PluginName, Logger);
    }

    private void BindConfig()
    {
        // Rebirth
        RebirthOldFollowers = Config.Bind(RebirthSection, "Rebirth Old Followers", false, new ConfigDescription("Allow old followers to be reborn.", null, new ConfigurationManagerAttributes { Order = 5 }));
        PreserveUniqueFollowers = Config.Bind(RebirthSection, "Preserve Unique Followers", true, new ConfigDescription("When enabled, unique followers (Webber, Sozo, Ratau, etc.) retain their original skin and traits when reborn. Names are always randomized.", null, new ConfigurationManagerAttributes { Order = 4 }));
        XpPenaltyChance = Config.Bind(RebirthSection, "XP Penalty Chance", 20, new ConfigDescription("Chance (%) of losing XP during rebirth.", new AcceptableValueRange<int>(0, 100), new ConfigurationManagerAttributes { Order = 3 }));
        XpPenaltyMultiplier = Config.Bind(RebirthSection, "XP Kept On Penalty", 50, new ConfigDescription("Percentage of XP kept when penalty triggers.", new AcceptableValueRange<int>(10, 90), new ConfigurationManagerAttributes { Order = 2, DispName = "    └ XP Kept On Penalty" }));
        TokenCost = Config.Bind(RebirthSection, "Token Cost", 25, new ConfigDescription("Tokens required for subsequent rebirths.", new AcceptableValueRange<int>(1, 100), new ConfigurationManagerAttributes { Order = 1 }));

        // Token Drops
        EnemyDropRate = Config.Bind(TokenDropsSection, "Enemy Drop Rate", 5, new ConfigDescription("Chance (%) of tokens dropping from enemies.", new AcceptableValueRange<int>(1, 100), new ConfigurationManagerAttributes { Order = 3 }));
        DropMinQuantity = Config.Bind(TokenDropsSection, "Drop Min Quantity", 1, new ConfigDescription("Minimum tokens per drop.", new AcceptableValueRange<int>(1, 10), new ConfigurationManagerAttributes { Order = 2, DispName = "    └ Drop Min Quantity" }));
        DropMaxQuantity = Config.Bind(TokenDropsSection, "Drop Max Quantity", 2, new ConfigDescription("Maximum tokens per drop.", new AcceptableValueRange<int>(1, 20), new ConfigurationManagerAttributes { Order = 1, DispName = "    └ Drop Max Quantity" }));

        // Dungeon Chests
        ChestSpawnChance = Config.Bind(DungeonChestsSection, "Chest Spawn Chance", 5, new ConfigDescription("Chance (%) of tokens appearing in dungeon chests.", new AcceptableValueRange<int>(1, 100), new ConfigurationManagerAttributes { Order = 3 }));
        ChestMinAmount = Config.Bind(DungeonChestsSection, "Chest Min Amount", 4, new ConfigDescription("Minimum tokens per chest.", new AcceptableValueRange<int>(1, 20), new ConfigurationManagerAttributes { Order = 2, DispName = "    └ Chest Min Amount" }));
        ChestMaxAmount = Config.Bind(DungeonChestsSection, "Chest Max Amount", 7, new ConfigDescription("Maximum tokens per chest.", new AcceptableValueRange<int>(1, 50), new ConfigurationManagerAttributes { Order = 1, DispName = "    └ Chest Max Amount" }));

        // Refinery
        BoneCost = Config.Bind(RefinerySection, "Bone Cost", 15, new ConfigDescription("Bones required to refine a token.", new AcceptableValueRange<int>(1, 50), new ConfigurationManagerAttributes { Order = 2 }));
        RefineryDuration = Config.Bind(RefinerySection, "Refinery Duration", 256, new ConfigDescription("Refinery duration in seconds.", new AcceptableValueRange<int>(10, 600), new ConfigurationManagerAttributes { Order = 1 }));

        // Missions
        MissionBaseChance = Config.Bind(MissionsSection, "Mission Base Chance", 50, new ConfigDescription("Mission appearance chance (%).", new AcceptableValueRange<int>(1, 100), new ConfigurationManagerAttributes { Order = 3 }));
        MissionRewardMin = Config.Bind(MissionsSection, "Mission Reward Min", 15, new ConfigDescription("Minimum token reward from missions.", new AcceptableValueRange<int>(1, 50), new ConfigurationManagerAttributes { Order = 2, DispName = "    └ Mission Reward Min" }));
        MissionRewardMax = Config.Bind(MissionsSection, "Mission Reward Max", 25, new ConfigDescription("Maximum token reward from missions.", new AcceptableValueRange<int>(1, 100), new ConfigurationManagerAttributes { Order = 1, DispName = "    └ Mission Reward Max" }));
    }

    private static void RegisterSettings()
    {
        // 01. Rebirth
        CustomSettingsManager.AddBepInExConfig("Rebirth", "Rebirth Old Followers", RebirthOldFollowers);
        CustomSettingsManager.AddBepInExConfig("Rebirth", "Preserve Unique Followers", PreserveUniqueFollowers);
        CustomSettingsManager.AddBepInExConfig("Rebirth", "XP Penalty Chance", XpPenaltyChance, 1, MMSlider.ValueDisplayFormat.RawValue);
        CustomSettingsManager.AddBepInExConfig("Rebirth", "XP Kept On Penalty", XpPenaltyMultiplier, 5, MMSlider.ValueDisplayFormat.RawValue);
        CustomSettingsManager.AddBepInExConfig("Rebirth", "Token Cost", TokenCost, 1, MMSlider.ValueDisplayFormat.RawValue);

        // 02. Token Drops
        CustomSettingsManager.AddBepInExConfig("Rebirth - Drops", "Enemy Drop Rate", EnemyDropRate, 1, MMSlider.ValueDisplayFormat.RawValue);
        CustomSettingsManager.AddBepInExConfig("Rebirth - Drops", "Drop Min Quantity", DropMinQuantity, 1, MMSlider.ValueDisplayFormat.RawValue);
        CustomSettingsManager.AddBepInExConfig("Rebirth - Drops", "Drop Max Quantity", DropMaxQuantity, 1, MMSlider.ValueDisplayFormat.RawValue);

        // 03. Dungeon Chests
        CustomSettingsManager.AddBepInExConfig("Rebirth - Chests", "Chest Spawn Chance", ChestSpawnChance, 1, MMSlider.ValueDisplayFormat.RawValue);
        CustomSettingsManager.AddBepInExConfig("Rebirth - Chests", "Chest Min Amount", ChestMinAmount, 1, MMSlider.ValueDisplayFormat.RawValue);
        CustomSettingsManager.AddBepInExConfig("Rebirth - Chests", "Chest Max Amount", ChestMaxAmount, 1, MMSlider.ValueDisplayFormat.RawValue);

        // 04. Refinery
        CustomSettingsManager.AddBepInExConfig("Rebirth - Refinery", "Bone Cost", BoneCost, 1, MMSlider.ValueDisplayFormat.RawValue);
        CustomSettingsManager.AddBepInExConfig("Rebirth - Refinery", "Refinery Duration", RefineryDuration, 10, MMSlider.ValueDisplayFormat.RawValue);

        // 05. Missions
        CustomSettingsManager.AddBepInExConfig("Rebirth - Missions", "Mission Reward Min", MissionRewardMin, 1, MMSlider.ValueDisplayFormat.RawValue);
        CustomSettingsManager.AddBepInExConfig("Rebirth - Missions", "Mission Reward Max", MissionRewardMax, 1, MMSlider.ValueDisplayFormat.RawValue);
        CustomSettingsManager.AddBepInExConfig("Rebirth - Missions", "Mission Base Chance", MissionBaseChance, 1, MMSlider.ValueDisplayFormat.RawValue);
    }
}
