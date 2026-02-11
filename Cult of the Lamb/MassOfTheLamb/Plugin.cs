namespace MassOfTheLamb;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("com.bepis.bepinex.configurationmanager", "18.4.1")]
public partial class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.cotl.MassOfTheLamb";
    internal const string PluginName = "Mass of the Lamb";
    private const string PluginVer = "0.1.0";

    private const string RestartGameMessage = "You must restart the game for these changes to take effect, as in totally exit to desktop and restart the game.\n\n** indicates a restart is required if the setting is changed.";

    private const string MassActionCostsSection = "── Mass Action Costs ──";
    private const string MassAnimalSection = "── Mass Animal ──";
    private const string MassCollectSection = "── Mass Collect ──";
    private const string MassFarmSection = "── Mass Farm ──";
    private const string MassFollowerSection = "── Mass Follower ──";
    private const string MassFillSection = "── Mass Fill (Structures) ──";

    internal static ManualLogSource Log { get; private set; }
    internal static PopupManager PopupManager { get; private set; }

    private void Awake()
    {
        Log = Logger;

        PopupManager = gameObject.AddComponent<PopupManager>();
        PopupManager.Title = PluginName;

        // ── Mass Animal ──
        MassPetAnimals = Config.Bind(MassAnimalSection, "Mass Pet Animals", false, new ConfigDescription("When petting a farm animal, all farm animals are petted at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Pet Animals**", Order = 6
        }));
        MassPetAnimals.SettingChanged += (_, _) => ShowRestartMessage();
        MassCleanAnimals = Config.Bind(MassAnimalSection, "Mass Clean Animals", false, new ConfigDescription("When cleaning a stinky animal, all stinky animals are cleaned at once.", null, new ConfigurationManagerAttributes
        {
            Order = 5
        }));
        MassFeedAnimals = Config.Bind(MassAnimalSection, "Mass Feed Animals", false, new ConfigDescription("When feeding an animal, all hungry animals are fed the same food at once (consumes one item per animal).", null, new ConfigurationManagerAttributes
        {
            Order = 4
        }));
        MassMilkAnimals = Config.Bind(MassAnimalSection, "Mass Milk Animals", false, new ConfigDescription("When milking an animal, all animals ready for milking are milked at once.", null, new ConfigurationManagerAttributes
        {
            Order = 3
        }));
        MassShearAnimals = Config.Bind(MassAnimalSection, "Mass Shear Animals", false, new ConfigDescription("When shearing an animal, all animals ready for shearing are sheared at once.", null, new ConfigurationManagerAttributes
        {
            Order = 2
        }));
        FillTroughToCapacity = Config.Bind(MassAnimalSection, "Fill Trough to Capacity", false, new ConfigDescription("When adding food to a trough, fills it to capacity in one action instead of adding one item at a time.", null, new ConfigurationManagerAttributes
        {
            Order = 1
        }));
        MassFillTroughs = Config.Bind(MassAnimalSection, "Mass Fill Troughs", false, new ConfigDescription("When filling a trough, all non-full troughs are filled with the same food.", null, new ConfigurationManagerAttributes
        {
            Order = 0, DispName = "    \u2514 Mass Fill Troughs"
        }));

        // ── Mass Action Costs ──
        MassActionCostModeEntry = Config.Bind(MassActionCostsSection, "Cost Mode", MassActionCostMode.PerObject, new ConfigDescription("How costs are calculated. Per Mass Action = flat fee regardless of count. Per Object = cost multiplied by number of objects affected.", null, new ConfigurationManagerAttributes
        {
            Order = 5
        }));
        ShowMassActionCostPreview = Config.Bind(MassActionCostsSection, "Show Cost Preview", false, new ConfigDescription("Show the estimated cost in the interaction label when highlighting a mass action. Only visible when Cost Mode is set to Per Object.", null, new ConfigurationManagerAttributes
        {
            Order = 4
        }));
        MassActionGoldCost = Config.Bind(MassActionCostsSection, "Gold Cost", 0f, new ConfigDescription("Gold deducted for a mass action. Set to 0 for free. If you can't afford the total, the mass action is skipped but the original single interaction still works.", new AcceptableValueRange<float>(0f, 50f), new ConfigurationManagerAttributes
        {
            Order = 3
        }));
        MassActionGoldCost.SettingChanged += (_, _) => MassActionGoldCost.Value = RoundToQuarter(MassActionGoldCost.Value);
        MassActionTimeCost = Config.Bind(MassActionCostsSection, "Time Cost (Game Minutes)", 0f, new ConfigDescription("Game minutes that pass for a mass action. Set to 0 for no time cost. 240 minutes = 1 game phase.", new AcceptableValueRange<float>(0f, 120f), new ConfigurationManagerAttributes
        {
            Order = 2
        }));
        MassActionTimeCost.SettingChanged += (_, _) => MassActionTimeCost.Value = RoundToQuarter(MassActionTimeCost.Value);
        MassFaithReduction = Config.Bind(MassActionCostsSection, "Faith Reduction (%)", 0, new ConfigDescription("Reduces faith gained per follower from mass Bless and Inspire. 0 = full faith, 50 = half faith, 100 = no faith. The original single interaction always gives full faith.", new AcceptableValueRange<int>(0, 100), new ConfigurationManagerAttributes
        {
            Order = 1
        }));

        // ── Mass Collect ──
        CollectAllGodTearsAtOnce = Config.Bind(MassCollectSection, "Collect All God Tears At Once", false, new ConfigDescription("When collecting god tears from the shrine, collect all available at once instead of one per interaction.", null, new ConfigurationManagerAttributes
        {
            Order = 7
        }));
        MassCollectFromBeds = Config.Bind(MassCollectSection, "Mass Collect From Beds", false, new ConfigDescription("When collecting souls from a bed, all beds are collected from at once. Also speeds up the per-soul drain to 2x.", null, new ConfigurationManagerAttributes
        {
            Order = 6
        }));
        MassCollectFromOuthouses = Config.Bind(MassCollectSection, "Mass Collect From Outhouses", false, new ConfigDescription("When collecting resources from an outhouse, all outhouses are collected from at once.", null, new ConfigurationManagerAttributes
        {
            Order = 5
        }));
        MassCollectFromOfferingShrines = Config.Bind(MassCollectSection, "Mass Collect From Offering Shrines", false, new ConfigDescription("When collecting resources from an offering shrine, all offering shrines are collected from at once.", null, new ConfigurationManagerAttributes
        {
            Order = 4
        }));
        MassCollectFromPassiveShrines = Config.Bind(MassCollectSection, "Mass Collect From Passive Shrines", false, new ConfigDescription("When collecting resources from a passive shrine, all passive shrines are collected from at once.", null, new ConfigurationManagerAttributes
        {
            Order = 3
        }));
        MassCollectFromCompost = Config.Bind(MassCollectSection, "Mass Collect From Compost", false, new ConfigDescription("When collecting resources from a compost, all composts are collected from at once.", null, new ConfigurationManagerAttributes
        {
            Order = 2
        }));
        MassCollectFromHarvestTotems = Config.Bind(MassCollectSection, "Mass Collect From Harvest Totems", false, new ConfigDescription("When collecting resources from a harvest totem, all harvest totems are collected from at once.", null, new ConfigurationManagerAttributes
        {
            Order = 1
        }));
        MassCleanPoop = Config.Bind(MassCollectSection, "Mass Clean Poop", false, new ConfigDescription("When cleaning a poop pile, all poop piles are cleaned at once.", null, new ConfigurationManagerAttributes
        {
            Order = 0
        }));
        MassCleanVomit = Config.Bind(MassCollectSection, "Mass Clean Vomit", false, new ConfigDescription("When cleaning a vomit puddle, all vomit puddles are cleaned at once.", null, new ConfigurationManagerAttributes
        {
            Order = -1
        }));

        // ── Mass Farm ──
        MassPlantSeeds = Config.Bind(MassFarmSection, "Mass Plant Seeds", false, new ConfigDescription("When planting a seed in a farm plot, all other empty farm plots are planted with the same seed.", null, new ConfigurationManagerAttributes
        {
            Order = 14
        }));
        MassFertilize = Config.Bind(MassFarmSection, "Mass Fertilize", false, new ConfigDescription("When fertilizing a plot, all farm plots are fertilized at once.", null, new ConfigurationManagerAttributes
        {
            Order = 13
        }));
        MassWater = Config.Bind(MassFarmSection, "Mass Water", false, new ConfigDescription("When watering a plot, all farm plots are watered at once.", null, new ConfigurationManagerAttributes
        {
            Order = 12
        }));
        FillToolshedToCapacity = Config.Bind(MassFarmSection, "Fill Carpentry Station to Capacity", false, new ConfigDescription("When depositing materials into a Carpentry Station, fills it to capacity in one action instead of adding one item at a time.", null, new ConfigurationManagerAttributes
        {
            Order = 11
        }));
        MassFillToolsheds = Config.Bind(MassFarmSection, "Mass Fill Carpentry Stations", false, new ConfigDescription("When filling a Carpentry Station, all non-full Carpentry Stations are filled with the same material.", null, new ConfigurationManagerAttributes
        {
            Order = 10, DispName = "    \u2514 Mass Fill Carpentry Stations"
        }));
        FillMedicToCapacity = Config.Bind(MassFarmSection, "Fill Medic Station to Capacity", false, new ConfigDescription("When depositing supplies into a Medic Station, fills it to capacity in one action instead of adding one item at a time.", null, new ConfigurationManagerAttributes
        {
            Order = 9
        }));
        MassFillMedicStations = Config.Bind(MassFarmSection, "Mass Fill Medic Stations", false, new ConfigDescription("When filling a Medic Station, all non-full Medic Stations are filled with the same supply.", null, new ConfigurationManagerAttributes
        {
            Order = 8, DispName = "    \u2514 Mass Fill Medic Stations"
        }));
        FillSeedSiloToCapacity = Config.Bind(MassFarmSection, "Fill Seed Silo to Capacity", false, new ConfigDescription("When depositing seeds into a Seed Silo, fills it to capacity in one action instead of adding one item at a time.", null, new ConfigurationManagerAttributes
        {
            Order = 7
        }));
        MassFillSeedSilos = Config.Bind(MassFarmSection, "Mass Fill Seed Silos", false, new ConfigDescription("When filling a Seed Silo, all non-full Seed Silos are filled with the same seed.", null, new ConfigurationManagerAttributes
        {
            Order = 6, DispName = "    \u2514 Mass Fill Seed Silos"
        }));
        FillFertilizerSiloToCapacity = Config.Bind(MassFarmSection, "Fill Fertiliser Silo to Capacity", false, new ConfigDescription("When depositing fertiliser into a Fertiliser Silo, fills it to capacity in one action instead of adding one item at a time.", null, new ConfigurationManagerAttributes
        {
            Order = 5
        }));
        MassFillFertilizerSilos = Config.Bind(MassFarmSection, "Mass Fill Fertiliser Silos", false, new ConfigDescription("When filling a Fertiliser Silo, all non-full Fertiliser Silos are filled with the same fertiliser.", null, new ConfigurationManagerAttributes
        {
            Order = 4, DispName = "    \u2514 Mass Fill Fertiliser Silos"
        }));
        MassOpenScarecrows = Config.Bind(MassFarmSection, "Mass Open Scarecrows", false, new ConfigDescription("When opening a scarecrow trap, all scarecrow traps with caught birds are opened at once.", null, new ConfigurationManagerAttributes
        {
            Order = 3
        }));
        MassWolfTraps = Config.Bind(MassFarmSection, "Mass Wolf Traps", MassWolfTrapMode.Disabled, new ConfigDescription("Fill Only: Fill all empty traps with the same bait. Collect Only: Collect from all traps with caught wolves. Both: Do both actions.", null, new ConfigurationManagerAttributes
        {
            Order = 2
        }));

        // ── Mass Follower ──
        MassNotificationThreshold = Config.Bind(MassFollowerSection, "Mass Notification Threshold", 3, new ConfigDescription("When a mass action affects more than this many followers, show a single summary notification instead of one per follower. Set to 0 to always show a summary.", new AcceptableValueRange<int>(0, 50), new ConfigurationManagerAttributes
        {
            Order = 14
        }));
        MassBribe = Config.Bind(MassFollowerSection, "Mass Bribe", false, new ConfigDescription("When bribing a follower, all followers are bribed at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Bribe**", Order = 13
        }));
        MassBribe.SettingChanged += (_, _) => ShowRestartMessage();
        MassBless = Config.Bind(MassFollowerSection, "Mass Bless", false, new ConfigDescription("When blessing a follower, all followers are blessed at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Bless**", Order = 12
        }));
        MassBless.SettingChanged += (_, _) => ShowRestartMessage();
        MassExtort = Config.Bind(MassFollowerSection, "Mass Extort", false, new ConfigDescription("When extorting a follower, all followers are extorted at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Extort**", Order = 11
        }));
        MassExtort.SettingChanged += (_, _) => ShowRestartMessage();
        MassIntimidate = Config.Bind(MassFollowerSection, "Mass Intimidate", false, new ConfigDescription("When intimidating a follower, all followers are intimidated at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Intimidate**", Order = 10
        }));
        MassIntimidate.SettingChanged += (_, _) => ShowRestartMessage();
        MassIntimidateScareAll = Config.Bind(MassFollowerSection, "Mass Intimidate Scare All", false, new ConfigDescription("When Mass Intimidate is enabled, apply the 5% Scared trait roll to all intimidated followers instead of only the original.", null, new ConfigurationManagerAttributes
        {
            Order = 9, DispName = "    \u2514 Mass Intimidate Scare All"
        }));
        MassIntimidateScareAll.SettingChanged += (_, _) => ShowRestartMessage();
        MassInspire = Config.Bind(MassFollowerSection, "Mass Inspire", false, new ConfigDescription("When inspiring a follower, all followers are inspired at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Inspire**", Order = 8
        }));
        MassInspire.SettingChanged += (_, _) => ShowRestartMessage();
        MassRomance = Config.Bind(MassFollowerSection, "Mass Romance", false, new ConfigDescription("When romancing a follower, all followers are romanced at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Romance**", Order = 7
        }));
        MassRomance.SettingChanged += (_, _) => ShowRestartMessage();
        MassBully = Config.Bind(MassFollowerSection, "Mass Bully", false, new ConfigDescription("When bullying a follower, all followers are bullied at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Bully**", Order = 6
        }));
        MassBully.SettingChanged += (_, _) => ShowRestartMessage();
        MassReassure = Config.Bind(MassFollowerSection, "Mass Reassure", false, new ConfigDescription("When reassuring a follower, all followers are reassured at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Reassure**", Order = 5
        }));
        MassReassure.SettingChanged += (_, _) => ShowRestartMessage();
        MassReeducate = Config.Bind(MassFollowerSection, "Mass Reeducate", false, new ConfigDescription("When reeducating a follower, all followers are reeducated at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Reeducate**", Order = 4
        }));
        MassReeducate.SettingChanged += (_, _) => ShowRestartMessage();
        MassLevelUp = Config.Bind(MassFollowerSection, "Mass Level Up", false, new ConfigDescription("When leveling up a follower, all eligible followers are leveled up at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Level Up**", Order = 3
        }));
        MassLevelUp.SettingChanged += (_, _) => ShowRestartMessage();
        MassLevelUpInstantSouls = Config.Bind(MassFollowerSection, "Mass Level Up Instant Souls", true, new ConfigDescription("Instantly collect souls during mass level up instead of having them fly to you.", null, new ConfigurationManagerAttributes
        {
            Order = 2, DispName = "    \u2514 Mass Level Up Instant Souls"
        }));
        MassPetFollower = Config.Bind(MassFollowerSection, "Mass Pet Follower", false, new ConfigDescription("When petting a follower, all eligible followers are petted at once. Which followers qualify depends on the Mass Pet All Followers setting below.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Pet Follower**", Order = 1
        }));
        MassPetFollower.SettingChanged += (_, _) => ShowRestartMessage();
        MassPetAllFollowers = Config.Bind(MassFollowerSection, "Mass Pet All Followers", false, new ConfigDescription("When enabled, mass pet applies to all followers regardless of the Pettable trait. When disabled, only followers with the Pettable trait or Dog/Poppy skin are petted.", null, new ConfigurationManagerAttributes
        {
            Order = 0, DispName = "    \u2514 Mass Pet All Followers"
        }));
        MassSinExtract = Config.Bind(MassFollowerSection, "Mass Sin Extract", false, new ConfigDescription("When extracting sin from a follower, all eligible followers have their sin extracted at once.", null, new ConfigurationManagerAttributes
        {
            DispName = "Mass Sin Extract**", Order = -1
        }));
        MassSinExtract.SettingChanged += (_, _) => ShowRestartMessage();

        // ── Mass Fill (Structures) ──
        RefineryMassFill = Config.Bind(MassFillSection, "Refinery Mass Fill", false, new ConfigDescription("When adding an item to the refinery queue, automatically fill all available slots with that item.", null, new ConfigurationManagerAttributes { Order = 4 }));
        CookingFireMassFill = Config.Bind(MassFillSection, "Cooking Fire Mass Fill", false, new ConfigDescription("When adding a meal to the cooking fire queue, automatically fill all available slots with that meal.", null, new ConfigurationManagerAttributes { Order = 3 }));
        KitchenMassFill = Config.Bind(MassFillSection, "Kitchen Mass Fill", false, new ConfigDescription("When adding a meal to the follower kitchen queue, automatically fill all available slots with that meal.", null, new ConfigurationManagerAttributes { Order = 2 }));
        PubMassFill = Config.Bind(MassFillSection, "Pub Mass Fill", false, new ConfigDescription("When adding a drink to the pub queue, automatically fill all available slots with that drink.", null, new ConfigurationManagerAttributes { Order = 1 }));

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);

        Helpers.PrintModLoaded(PluginName, Logger);
    }

    public enum LogType
    {
        Error,
        Info,
        Warning
    }

    public static void WriteLog(string message, LogType type = LogType.Info)
    {
        switch (type)
        {
            case LogType.Error:
                Log.LogError(message);
                break;
            case LogType.Info:
                Log.LogInfo(message);
                break;
            case LogType.Warning:
                Log.LogWarning(message);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    internal static float RoundToQuarter(float value) => (float)(Math.Round(value * 4.0) / 4.0);

    private void ShowRestartMessage()
    {
        if (!PopupManager.ShowPopup)
        {
            PopupManager.ShowPopupDlg(RestartGameMessage, true);
        }
    }
}
