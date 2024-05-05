using System;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using GYKHelper;
using HarmonyLib;

namespace GerryFixer;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("p1xel8ted.gyk.gykhelper", "3.0.1")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.gerryfixer";
    private const string PluginName = "Gerry Fixer...";
    private const string PluginVer = "0.1.4";

    internal static ConfigEntry<bool> Debug;
    internal static ConfigEntry<bool> AttemptToFixCutsceneGerrys { get; private set; }
    internal static ConfigEntry<bool> SpawnTavernCellarGerry { get; private set; }
    internal static ConfigEntry<bool> SpawnTavernMorgueGerry { get; private set; }
    internal static ManualLogSource Log { get; private set; }
    private static Harmony Harmony { get; set; }
    private static ConfigEntry<bool> ModEnabled { get; set; }

    private void Awake()
    {
        Log = Logger;
        Harmony = new Harmony(PluginGuid);
        InitConfiguration();
        ApplyPatches(this, null);
    }

    private void InitConfiguration()
    {
        ModEnabled = Config.Bind("1. General", "Enabled", true, new ConfigDescription($"Toggle {PluginName}", null, new ConfigurationManagerAttributes {Order = 5}));
        ModEnabled.SettingChanged += ApplyPatches;
    
        Debug = Config.Bind("2. Advanced", "Debug Logging", false, new ConfigDescription("Toggle debug logging on or off", null, new ConfigurationManagerAttributes {IsAdvanced = true, Order = 4}));

        AttemptToFixCutsceneGerrys = Config.Bind("3. Gerry", "Attempt To Fix Cutscene Gerrys", false, new ConfigDescription("Attempt to fix cutscene freezes/crashes due to Gerry not appearing", null, new ConfigurationManagerAttributes {Order = 3}));
    
        SpawnTavernCellarGerry = Config.Bind("3. Gerry", "Spawn Tavern Cellar Gerry", false, new ConfigDescription("Choose whether to spawn Gerry in the tavern cellar (if he does not exist)", null, new ConfigurationManagerAttributes {Order = 2}));
    
        SpawnTavernMorgueGerry = Config.Bind("3. Gerry", "Spawn Tavern Morgue Gerry", false, new ConfigDescription("Choose whether to spawn Gerry in the tavern morgue (if he does not exist)", null, new ConfigurationManagerAttributes {Order = 1}));
    }

        
    private static void ApplyPatches(object sender, EventArgs eventArgs)
    {
        if (ModEnabled.Value)
        {
            Actions.GameStartedPlaying += Patches.FixGerry;
            Log.LogInfo($"Applying patches for {PluginName}");
            Harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
        else
        {
            Actions.GameStartedPlaying -= Patches.FixGerry;
            Log.LogInfo($"Removing patches for {PluginName}");
            Harmony.UnpatchSelf();
        }
    }
}