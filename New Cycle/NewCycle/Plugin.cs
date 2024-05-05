using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace NewCycle;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BasePlugin
{
    private const string PluginGuid = "p1xel8ted.newcycle.tweaks";
    private const string PluginName = "New Cycle Tweaks (IL2CPP)";
    internal const string PluginVersion = "0.1.0";

    internal static ManualLogSource Logger { get; private set; }


    public override void Load()
    {
        Logger = Log;

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);

        Logger.LogInfo($"Plugin {PluginGuid} is loaded!");
    }


}