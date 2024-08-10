using UnityEngine.SceneManagement;

namespace MiscBitsAndBobs;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("p1xel8ted.gyk.gykhelper", "3.1.0")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.miscbitsandbobs";
    private const string PluginName = "Misc. Bits & Bobs";
    private const string PluginVer = "2.2.9";

    internal static ConfigEntry<bool> Debug { get; set; }
    internal static ConfigEntry<bool> QuietMusicInGuiConfig { get; private set; }
    internal static ConfigEntry<bool> CondenseXpBarConfig { get; private set; }
    internal static ConfigEntry<bool> ModifyPlayerMovementSpeedConfig { get; private set; }
    internal static ConfigEntry<float> PlayerMovementSpeedConfig { get; private set; }
    internal static ConfigEntry<bool> ModifyPorterMovementSpeedConfig { get; private set; }
    internal static ConfigEntry<float> PorterMovementSpeedConfig { get; private set; }
    internal static ConfigEntry<bool> HalloweenNowConfig { get; private set; }
    internal static ConfigEntry<bool> HideCreditsButtonOnMainMenuConfig { get; private set; }
    internal static ConfigEntry<bool> SkipIntroVideoOnNewGameConfig { get; private set; }
    internal static ConfigEntry<bool> CinematicLetterboxingConfig { get; private set; }
    internal static ConfigEntry<bool> KitsuneKitoModeConfig { get; private set; }
    internal static ConfigEntry<bool> LessenFootprintImpactConfig { get; private set; }
    internal static ConfigEntry<bool> RemovePrayerOnUseConfig { get; private set; }
    internal static ConfigEntry<bool> AddCoalToTavernOvenConfig { get; private set; }
    internal static ConfigEntry<bool> AddZombiesToPyreAndCrematoriumConfig { get; private set; }
    private static ConfigEntry<bool> KeepGamingRunningInBackgroundConfig { get; set; }
    
    private static ConfigEntry<bool> MuteWhenRunningInBackgroundConfig { get; set; }
    internal static ConfigEntry<bool> OldEnglishThrowback { get; private set; }
    
    internal static ManualLogSource Log { get; private set; }


    private void Awake()
    {
        Log = Logger;
        InitConfiguration();
        SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
        Application.focusChanged += ApplicationOnFocusChanged;
        Actions.GameStartedPlaying += Helpers.ActionsOnSpawnPlayer;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Log.LogInfo($"Plugin {PluginName} is loaded!");
    }
    private static void ApplicationOnFocusChanged(bool value)
    {
        Application.runInBackground = KeepGamingRunningInBackgroundConfig.Value;
        if (MuteWhenRunningInBackgroundConfig.Value)
        {
            AudioListener.volume = Application.isFocused ? 1 : 0;
        }
        else
        {
            AudioListener.volume = 1;
        }
    }
    private static void SceneManagerOnSceneLoaded(Scene __arg0, LoadSceneMode __arg1)
    {
        ApplicationOnFocusChanged(true);
    }
    
    private void InitConfiguration()
    {
        KeepGamingRunningInBackgroundConfig = Config.Bind("01. General", "Keep Game Running In Background", true, new ConfigDescription("Keep the game running when it is in the background.", null, new ConfigurationManagerAttributes {Order = 30}));
        KeepGamingRunningInBackgroundConfig.SettingChanged += (_, _) =>
        {
            Application.runInBackground = KeepGamingRunningInBackgroundConfig.Value;
        };
        
        MuteWhenRunningInBackgroundConfig = Config.Bind("01. General", "Mute When Running In Background", true, new ConfigDescription("Mute the game when it is in the background.", null, new ConfigurationManagerAttributes {Order = 31}));
        MuteWhenRunningInBackgroundConfig.SettingChanged += (_, _) =>
        {
            if (MuteWhenRunningInBackgroundConfig.Value)
            {
                AudioListener.volume = Application.isFocused ? 1 : 0;
            }
            else
            {
                AudioListener.volume = 1;
            }
        };
        QuietMusicInGuiConfig = Config.Bind("02. Audio", "Quiet Music In GUI", true, new ConfigDescription("Lower the music volume when in-game menus are open.", null, new ConfigurationManagerAttributes {Order = 29}));

        CondenseXpBarConfig = Config.Bind("03. UI", "Condense XP Bar", true, new ConfigDescription("Reduce the size of the XP bar in the user interface.", null, new ConfigurationManagerAttributes {Order = 28}));
        HideCreditsButtonOnMainMenuConfig = Config.Bind("03. UI", "Hide Credits Button On Main Menu", true, new ConfigDescription("Remove the credits button from the main menu.", null, new ConfigurationManagerAttributes {Order = 27}));
        CinematicLetterboxingConfig = Config.Bind("03. UI", "Remove Cinematic Letterboxing", true, new ConfigDescription("Remove black bars during cinematic cutscenes.", null, new ConfigurationManagerAttributes {Order = 25}));

        HalloweenNowConfig = Config.Bind("04. Gameplay", "Halloween Now", false, new ConfigDescription("Activate Halloween mode at any time.", null, new ConfigurationManagerAttributes {Order = 24}));
        SkipIntroVideoOnNewGameConfig = Config.Bind("04. Gameplay", "Skip Intro Video On New Game", false, new ConfigDescription("Skip the intro video when starting a new game.", null, new ConfigurationManagerAttributes {Order = 23}));
        LessenFootprintImpactConfig = Config.Bind("04. Gameplay", "Lessen Footprint Impact", false, new ConfigDescription("Reduce the impact of footprints on the environment.", null, new ConfigurationManagerAttributes {Order = 22}));
        RemovePrayerOnUseConfig = Config.Bind("04. Gameplay", "Remove Prayer On Use", false, new ConfigDescription("Prayers are removed after use.", null, new ConfigurationManagerAttributes {Order = 21}));
        AddCoalToTavernOvenConfig = Config.Bind("04. Gameplay", "Add Coal To Tavern Oven", true, new ConfigDescription("Allow coal to be used as fuel in the tavern oven.", null, new ConfigurationManagerAttributes {Order = 20}));
        AddZombiesToPyreAndCrematoriumConfig = Config.Bind("04. Gameplay", "Add Zombies To Pyre And Crematorium", true, new ConfigDescription("Enable the option to burn zombies at the pyre and crematorium.", null, new ConfigurationManagerAttributes {Order = 19}));

        ModifyPlayerMovementSpeedConfig = Config.Bind("05. Movement", "Modify Player Movement Speed", true, new ConfigDescription("Allow modification of the player's movement speed.", null, new ConfigurationManagerAttributes {Order = 18}));
        PlayerMovementSpeedConfig = Config.Bind("05. Movement", "Player Movement Speed", 1.0f, new ConfigDescription("Set the player's movement speed.", new AcceptableValueRange<float>(1.0f, 100f), new ConfigurationManagerAttributes {Order = 17}));
        ModifyPorterMovementSpeedConfig = Config.Bind("05. Movement", "Modify Porter Movement Speed", true, new ConfigDescription("Allow modification of the porter's movement speed.", null, new ConfigurationManagerAttributes {Order = 16}));
        PorterMovementSpeedConfig = Config.Bind("05. Movement", "Porter Movement Speed", 1.0f, new ConfigDescription("Set the porter's movement speed.", new AcceptableValueRange<float>(1.0f, 100f), new ConfigurationManagerAttributes {Order = 15}));
        KitsuneKitoModeConfig = Config.Bind("06. Misc", "KitsuneKito Mode", false, new ConfigDescription("Discord user request. Drops a blue xp point when adding a basic fence to a grave.", null, new ConfigurationManagerAttributes {Order = 14}));
        OldEnglishThrowback = Config.Bind("06. Misc", "Old English Throwback", false, new ConfigDescription("Discord user request. Modifies a sermon sentence.", null, new ConfigurationManagerAttributes {Order = 13}));
        Debug = Config.Bind("00. Advanced", "Debug Logging", false, new ConfigDescription("Enable or disable debug logging.", null, new ConfigurationManagerAttributes {IsAdvanced = true, Order = 12}));
    }
}