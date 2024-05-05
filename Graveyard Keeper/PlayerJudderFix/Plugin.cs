using System.Reflection;
using BepInEx;
using HarmonyLib;

namespace PlayerJudderFix;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "aze.graveyardkeeper.playerjudderfix";
    private const string PluginName = "PlayerJudderFix";
    private const string PluginVer = "0.0.2";

    private static readonly Harmony Harmony = new(PluginGuid);

    private void Awake()
    {
        Harmony.PatchAll(Assembly.GetExecutingAssembly());
    }
}