using System;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using GYKHelper;
using HarmonyLib;

namespace WheresMaPoints;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("p1xel8ted.gyk.gykhelper", "3.0.1")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.wheresmapoints";
    private const string PluginName = "Where's Ma' Points!";
    private const string PluginVer = "0.2.6";

    internal static ConfigEntry<bool> ShowPointGainAboveKeeper { get; private set; }
    internal static ConfigEntry<bool> StillPlayCollectAudio { get; private set; }

    internal static ConfigEntry<bool> AlwaysShowXpBar { get; private set; }

    private static ManualLogSource Log { get; set; }
    private static Harmony Harmony { get; set; }

    private static  ConfigEntry<bool> ModEnabled { get; set; }

    private void Awake()
    {
            
        Log = Logger;
        Harmony = new Harmony(PluginGuid);

            
        InitConfiguration();

        ApplyPatches(this, null);
    }

    private void InitConfiguration()
    {
        ModEnabled = Config.Bind("1. General", "Enabled", true, new ConfigDescription($"Toggle {PluginName}", null, new ConfigurationManagerAttributes {Order = 0}));
        ModEnabled.SettingChanged += ApplyPatches;

        AlwaysShowXpBar = Config.Bind("2. User Interface", "Always Show XP Bar", true, new ConfigDescription("Display the experience bar constantly, even without active experience gain.", null, new ConfigurationManagerAttributes {Order = 6}));
        ShowPointGainAboveKeeper = Config.Bind("3. Visual Feedback", "Show Point Gain Above Keeper", true, new ConfigDescription("Display the points earned above the keeper's head.", null, new ConfigurationManagerAttributes {Order = 5}));
        StillPlayCollectAudio = Config.Bind("4. Audio Feedback", "Still Play Collect Audio", false, new ConfigDescription("Keep playing the collect audio when point gain is displayed above the keeper's head.", null, new ConfigurationManagerAttributes {Order = 4}));
        

    }


    private static void ApplyPatches(object sender, EventArgs eventArgs)
    {
        if (ModEnabled.Value)
        {
            Log.LogInfo($"Applying patches for {PluginName}");
            Harmony.PatchAll(Assembly.GetExecutingAssembly());   
        }
        else
        {
            Log.LogInfo($"Removing patches for {PluginName}");
            Harmony.UnpatchSelf(); 
        }
    }
}