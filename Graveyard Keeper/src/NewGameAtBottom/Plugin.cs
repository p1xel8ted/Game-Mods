using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace NewGameAtBottom;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.newgameatbottom";
    private const string PluginName = "New Game at Bottom!";
    private const string PluginVer = "2.2.6";

    private static ManualLogSource Log { get; set; }

    private void Awake()
    {
        Log = Logger;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        GYKHelper.StartupLogger.PrintModLoaded(PluginName, Log);
    }
}