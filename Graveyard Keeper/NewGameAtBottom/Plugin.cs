using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace NewGameAtBottom;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("p1xel8ted.gyk.gykhelper", "3.1.0")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.newgameatbottom";
    private const string PluginName = "New Game at Bottom!";
    private const string PluginVer = "2.2.5";

    private static ManualLogSource Log { get; set; }

    private void Awake()
    {
        Log = Logger;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Log.LogInfo($"Plugin {PluginName} is loaded!");
    }
}