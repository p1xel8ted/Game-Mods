using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace NoIntros;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.nointros";
    private const string PluginName = "No Intros!";
    private const string PluginVer = "2.2.3";
    private static ManualLogSource Log { get; set; }

    private void Awake()
    {
        Log = Logger;
        Log.LogInfo($"Applying patches for {PluginName}");
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
    }
}