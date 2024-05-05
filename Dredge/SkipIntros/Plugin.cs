using System.Reflection;
using BepInEx;
using HarmonyLib;

namespace SkipIntros;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.dredge.skipintros";
    private const string PluginName = "Skip Intros";
    private const string PluginVersion = "0.1.0";

    private void Awake()
    {
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Logger.LogInfo($"Plugin {PluginGuid} is loaded!");
    }
}