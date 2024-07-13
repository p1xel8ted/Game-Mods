using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace AddStraightToTable;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("p1xel8ted.gyk.gykhelper", "3.0.5 ")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.addstraighttotable";
    private const string PluginName = "Add Straight to Table!";
    private const string PluginVer = "2.4.5";
    private static ManualLogSource LOG { get; set; }
    
    private void Awake()
    {
        LOG = Logger;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        LOG.LogInfo($"Plugin {PluginName} is loaded!");
    }

}