using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace AddStraightToTable;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.addstraighttotable";
    private const string PluginName = "Add Straight to Table!";
    private const string PluginVer = "2.4.8";
    internal static ManualLogSource LOG { get; set; }
    
    private void Awake()
    {
        LOG = Logger;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        GYKHelper.StartupLogger.PrintModLoaded(PluginName, LOG);
    }

}