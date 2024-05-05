namespace NoTimeForFishing
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "p1xel8ted.sunhaven.notimeforfishing";
        private const string PluginName = "No Time For Fishing!";
        private const string PluginVersion = "0.0.8";

        public static ConfigEntry<bool> DisableCaughtFishWindow;
        public static ConfigEntry<bool> SkipFishingMiniGame;
        public static ConfigEntry<bool> AutoReel;
        public static ConfigEntry<bool> InstantAutoReel;

        internal static ConfigEntry<bool> NibblingBehaviour;

        internal static ConfigEntry<bool> ModifyFishingRodCastSpeed;
        internal static ConfigEntry<int> FishingRodCastSpeed;

        internal static ConfigEntry<bool> ModifyFishSpawnMultiplier;
        internal static ConfigEntry<int> FishSpawnMultiplier;

        internal static ConfigEntry<bool> ModifyFishSpawnLimit;
        internal static ConfigEntry<int> FishSpawnLimit;

        internal static ConfigEntry<bool> ModifyMiniGameSpeed;
        internal static ConfigEntry<float> MiniGameMaxSpeed;


        internal static ConfigEntry<bool> ModifyBubbleSpell;
        internal static ConfigEntry<bool> IncreaseBubbleSpellCap;

        internal static ConfigEntry<bool> ModifyMiniGameWinAreaMultiplier;
        internal static ConfigEntry<float> MiniGameWinAreaMultiplier;
        internal static ConfigEntry<bool> DoubleBaseFishSwimSpeed;

        internal static ConfigEntry<bool> DoubleBaseBobberAttractionRadius;
        internal static ConfigEntry<bool> EnhanceBaseCastLength;

        internal static ConfigEntry<bool> InstantAttraction;

        internal static ConfigEntry<bool> ModifyFishOdds;
        internal static ConfigEntry<float> FishOddsMultiplier;
        internal static ConfigEntry<bool> MaximizeFishOdds;
        
        internal static ConfigEntry<bool> IncreaseMuseumFishChance;
        internal static ConfigEntry<float> IncreaseMuseumFishChanceValue;
        
        
        public static ConfigEntry<bool> Debug;

        private static bool _showConfirmationDialog;

        internal static ManualLogSource LOG { get; set; }


        private void Awake()
        {
            LOG = new ManualLogSource(PluginName);
            BepInEx.Logging.Logger.Sources.Add(LOG);

            // 01. Bobber Dynamics
            InstantAttraction = Config.Bind("01. Bobber Dynamics", "Immediate Fish Attraction", true,
                new ConfigDescription("Ensures that fish are instantly attracted to the bobber.", null,
                    new ConfigurationManagerAttributes {Order = 100}));

            DoubleBaseBobberAttractionRadius = Config.Bind("01. Bobber Dynamics", "Extended Bobber Attraction Radius",
                true,
                new ConfigDescription(
                    "Enhances the base attraction radius of the bobber. Talent bonuses are applied afterward.",
                    null, new ConfigurationManagerAttributes {Order = 99}));

            // 02. Fish Behavior & Spawning
            NibblingBehaviour = Config.Bind("02. Fish Behavior & Spawning", "Nibbling Behavior", false,
                new ConfigDescription("Prevents fish from nibbling and fleeing.", null,
                    new ConfigurationManagerAttributes {Order = 98}));

            DoubleBaseFishSwimSpeed = Config.Bind("02. Fish Behavior & Spawning", "Enhanced Fish Swim Speed", true,
                new ConfigDescription(
                    "Boosts the base speed of fish movement towards the bobber. Talent bonuses are applied afterward.",
                    null, new ConfigurationManagerAttributes {Order = 97}));

            ModifyFishSpawnLimit = Config.Bind("02. Fish Behavior & Spawning", "Adjustable Fish Population Cap", true,
                new ConfigDescription("Toggle the ability to set a maximum fish population.", null,
                    new ConfigurationManagerAttributes {Order = 96}));

            FishSpawnLimit = Config.Bind("02. Fish Behavior & Spawning", "Fish Population Limit", 1500,
                new ConfigDescription("Determine the maximum fish population in the game environment.",
                    new AcceptableValueRange<int>(1, 1500), new ConfigurationManagerAttributes {Order = 95}));

            ModifyFishSpawnMultiplier = Config.Bind("02. Fish Behavior & Spawning", "Fish Spawn Rate Modifier", true,
                new ConfigDescription("Toggle the ability to adjust the fish spawn rate.", null,
                    new ConfigurationManagerAttributes {Order = 94}));

            FishSpawnMultiplier = Config.Bind("02. Fish Behavior & Spawning", "Fish Spawn Rate", 1500,
                new ConfigDescription("Set the rate at which fish spawn in the environment.",
                    new AcceptableValueRange<int>(1, 1500), new ConfigurationManagerAttributes {Order = 93}));
            
            ModifyFishOdds = Config.Bind("02. Fish Behavior & Spawning", "Adjustable Fish Odds", true,
                new ConfigDescription("Toggle the ability to adjust the fish odds. Changing any of these settings will only apply to newly spawned fish.", null,
                    new ConfigurationManagerAttributes {Order = 92}));
            MaximizeFishOdds = Config.Bind("02. Fish Behavior & Spawning", "Maximize Fish Odds", false,
                new ConfigDescription("Maximize the chance of getting higher odds. Changing any of these settings will only apply to newly spawned fish.",
                    null, new ConfigurationManagerAttributes {Order = 91}));
            FishOddsMultiplier = Config.Bind("02. Fish Behavior & Spawning", "Fish Odds Multiplier", 1.50f,
                new ConfigDescription("Apply a multiplier to the fish odds. Changing any of these settings will only apply to newly spawned fish.",
                    new AcceptableValueRange<float>(1.1f, 10f), new ConfigurationManagerAttributes {Order = 91}));

            IncreaseMuseumFishChance = Config.Bind("02. Fish Behavior & Spawning", "Increase Museum Fish Chance", true,
                new ConfigDescription("Increase the chance of getting a select few museum items when catching fish.",
                    null, new ConfigurationManagerAttributes {Order = 90}));
            IncreaseMuseumFishChanceValue = Config.Bind("02. Fish Behavior & Spawning", "Increase Museum Fish Chance Value", 0.10f,           
                new ConfigDescription("Set the chance of getting a select few museum items when catching fish. Game default is 0.04",
                    new AcceptableValueRange<float>(0.1f, 0.25f), new ConfigurationManagerAttributes {Order = 89}));
            
            
            // 03. Rod Capabilities
            ModifyFishingRodCastSpeed = Config.Bind("03. Rod Capabilities", "Adjustable Rod Casting Speed", true,
                new ConfigDescription("Toggle the ability to modify the fishing rod's casting speed.", null,
                    new ConfigurationManagerAttributes {Order = 92}));

            FishingRodCastSpeed = Config.Bind("03. Rod Capabilities", "Rod Casting Speed", 5,
                new ConfigDescription(
                    "Set the base casting speed of the fishing rod. Talent bonuses are applied afterward.",
                    new AcceptableValueRange<int>(1, 10), new ConfigurationManagerAttributes {Order = 91}));

            EnhanceBaseCastLength = Config.Bind("03. Rod Capabilities", "Extended Rod Cast Length", true,
                new ConfigDescription(
                    "Boosts the base casting distance of the fishing rod. Talent bonuses are applied afterward.",
                    null, new ConfigurationManagerAttributes {Order = 90}));

            InstantAutoReel = Config.Bind("03. Rod Capabilities", "Instant Automatic Reeling", true,
                new ConfigDescription("Ensures fish are reeled in immediately upon biting. The reeling animation will not play.", null,
                    new ConfigurationManagerAttributes {Order = 89}));
            InstantAutoReel.SettingChanged += (_, _) =>
            {
                if (InstantAutoReel.Value)
                {
                    AutoReel.Value = false;
                }
            };

            AutoReel = Config.Bind("03. Rod Capabilities", "Automatic Reeling", true,
                new ConfigDescription("The rod will automatically reel in fish upon a successful bite. The reeling animation will play.", null,
                    new ConfigurationManagerAttributes {Order = 87}));
            AutoReel.SettingChanged += (_, _) =>
            {
                if (AutoReel.Value)
                {
                    InstantAutoReel.Value = false;
                }
            };

            // 04. Mini-Game Adjustments
            SkipFishingMiniGame = Config.Bind("04. Mini-Game Adjustments", "Bypass Fishing Mini-Game", true,
                new ConfigDescription("Omits the fishing mini-game entirely for quicker fishing.", null,
                    new ConfigurationManagerAttributes {Order = 86}));

            ModifyMiniGameSpeed = Config.Bind("04. Mini-Game Adjustments", "Adjustable Mini-Game Speed", true,
                new ConfigDescription("Toggle the ability to modify the fishing mini-game's speed.", null,
                    new ConfigurationManagerAttributes {Order = 85}));

            MiniGameMaxSpeed = Config.Bind("04. Mini-Game Adjustments", "Mini-Game Maximum Speed", 0.1f,
                new ConfigDescription("Set the maximum playable speed for the fishing mini-game.",
                    new AcceptableValueRange<float>(0.1f, 1f), new ConfigurationManagerAttributes {Order = 84}));

            ModifyMiniGameWinAreaMultiplier = Config.Bind("04. Mini-Game Adjustments", "Adjustable Winning Zone Size",
                true,
                new ConfigDescription(
                    "Toggle the ability to adjust the size of the successful catch zone in the mini-game.", null,
                    new ConfigurationManagerAttributes {Order = 83}));

            MiniGameWinAreaMultiplier = Config.Bind("04. Mini-Game Adjustments", "Winning Zone Size in Mini-Game", 20f,
                new ConfigDescription("Determine the size of the successful catch zone in the mini-game.",
                    new AcceptableValueRange<float>(1, 20), new ConfigurationManagerAttributes {Order = 82}));

            // 05. Spell Adjustments
            ModifyBubbleSpell = Config.Bind("05. Spell Adjustments", "Customize Bubble Spell", true,
                new ConfigDescription(
                    "Modify the bubble spell to cap the number of fish the bubble can contain based on your skill level. Check Bubble Spell tooltip for details.",
                    null,
                    new ConfigurationManagerAttributes {Order = 81}));
            IncreaseBubbleSpellCap = Config.Bind("05. Spell Adjustments", "Increase Bubble Spell Cap", true,
                new ConfigDescription(
                    "Increase the bubble spell's cap. Check Bubble Spell tooltip for details.",
                    null,
                    new ConfigurationManagerAttributes {Order = 80}));


            // 06. General Settings
            DisableCaughtFishWindow = Config.Bind("06. General Settings", "Disable Fish Catch Display", true,
                new ConfigDescription(
                    "Deactivates the pop-up window displaying information about the fish you've caught.", null,
                    new ConfigurationManagerAttributes {Order = 79}));

            Debug = Config.Bind("06. General Settings", "Enable Debug Logging", false,
                new ConfigDescription("Activate debugging for more detailed logs and insights.", null,
                    new ConfigurationManagerAttributes {Order = 78}));

            Config.Bind("06. General Settings", "Revert to Default Settings", true,
                new ConfigDescription("Reset all configurations to the mod's default settings.", null,
                    new ConfigurationManagerAttributes
                        {Order = 77, HideDefaultButton = true, CustomDrawer = RecommendedButtonDrawer}));


            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
            LOG.LogInfo($"Plugin {PluginName} is loaded!");
        }

        private void OnDisable()
        {
            LOG.LogError($"{PluginName} has been disabled!");
        }

        private void OnDestroy()
        {
            LOG.LogError($"{PluginName} has been destroyed!");
        }

        private static void DisplayConfirmationDialog()
        {
            GUILayout.Label("Are you sure you want to reset to default settings?");

            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Yes", GUILayout.ExpandWidth(true)))
                {
                    RecommendedSettingsAction();
                    _showConfirmationDialog = false;
                }

                if (GUILayout.Button("No", GUILayout.ExpandWidth(true)))
                {
                    _showConfirmationDialog = false;
                }
            }
            GUILayout.EndHorizontal();
        }

        private static void RecommendedButtonDrawer(ConfigEntryBase entry)
        {
            if (_showConfirmationDialog)
            {
                DisplayConfirmationDialog();
            }
            else
            {
                var button = GUILayout.Button("Recommended Settings", GUILayout.ExpandWidth(true));
                if (button)
                {
                    _showConfirmationDialog = true;
                }
            }
        }

        private static void RecommendedSettingsAction()
        {
            // 01. Bobber Dynamics
            DoubleBaseBobberAttractionRadius.Value = true;
            InstantAttraction.Value = true;

            // 02. Fish Behavior & Spawning
            NibblingBehaviour.Value = false;
            DoubleBaseFishSwimSpeed.Value = true;
            ModifyFishSpawnLimit.Value = true;
            FishSpawnLimit.Value = 1500;
            ModifyFishSpawnMultiplier.Value = true;
            FishSpawnMultiplier.Value = 1500;
            ModifyFishOdds.Value = true;
            FishOddsMultiplier.Value = 1.50f;
            MaximizeFishOdds.Value = false;
            IncreaseMuseumFishChance.Value = true;
            IncreaseMuseumFishChanceValue.Value = 0.10f;
            

            // 03. Rod Capabilities
            AutoReel.Value = false;
            InstantAutoReel.Value = true;
            EnhanceBaseCastLength.Value = true;
            ModifyFishingRodCastSpeed.Value = true;
            FishingRodCastSpeed.Value = 5;

            // 04. Mini-Game Adjustments
            SkipFishingMiniGame.Value = true;
            ModifyMiniGameSpeed.Value = true;
            MiniGameMaxSpeed.Value = 0.1f;
            ModifyMiniGameWinAreaMultiplier.Value = true;
            MiniGameWinAreaMultiplier.Value = 20f;

            // 05. Bubble Spell Adjustments
            ModifyBubbleSpell.Value = true;

            // 06. General Settings
            DisableCaughtFishWindow.Value = true;
            Debug.Value = false;
        }
    }
}