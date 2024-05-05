using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace Silence;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BasePlugin
{
    private const string PluginGuid = "p1xel8ted.godsworn.silence";
    private const string PluginName = "Silence!";
    internal const string PluginVersion = "0.1.0";
   
    internal static ConfigEntry<int> VoicePlayChance { get; private set; }
    
    internal static ConfigEntry<bool> ControlVoicePlayChance { get; private set; }
    
    internal static ConfigEntry<bool> DontPlayIfJustPlayed { get; private set; }
    internal static ManualLogSource Logger { get; private set; }


    public override void Load()
    {
        Logger = Log;
        
        ControlVoicePlayChance = Config.Bind("01. Voice", "Control Voice Play Chance", false, new ConfigDescription("Control voice play chance", null));
        DontPlayIfJustPlayed = Config.Bind("01. Voice", "Dont Play If Just Played", false, new ConfigDescription("Don't play line if the the same line just played.", null));
        VoicePlayChance = Config.Bind("01. Voice", "Voice Play Chance", 75, new ConfigDescription("Chance of voice playing on move, attack etc.", new AcceptableValueRange<int>(0, 100)));
        
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Logger.LogInfo($"Plugin {PluginGuid} is loaded!");
    }

}