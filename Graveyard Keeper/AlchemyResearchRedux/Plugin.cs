using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace AlchemyResearchRedux;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("p1xel8ted.gyk.gykhelper", "3.0.9")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.alchemyresearchredux";
    private const string PluginName = "Alchemy Research Redux!";
    private const string PluginVer = "0.1.0";
    private static ManualLogSource LOG { get; set; }
    
    private void Awake()
    {
        LOG = Logger;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        LOG.LogInfo($"Plugin {PluginName} is loaded!");
    }

}