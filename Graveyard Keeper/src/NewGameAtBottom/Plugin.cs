using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using Shared;

namespace NewGameAtBottom;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private static ManualLogSource Log { get; set; }
    internal static ConfigEntry<bool> CheckForUpdates { get; private set; }

    private void Awake()
    {
        Log = Logger;
        CheckForUpdates = Config.Bind("── Updates ──", "Check for Updates", true,
            "Show a notice on the main menu when a newer version of this mod is available on NexusMods. Click the notice to open the mod's page.");
        UpdateChecker.Register(Info, CheckForUpdates);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);
    }
}
