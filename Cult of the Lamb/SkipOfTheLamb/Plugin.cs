using BepInEx.Bootstrap;
using Shared;

namespace SkipOfTheLamb;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.cotl.skipofthelamb";
    private const string PluginName = "Skip of the Lamb";
    private const string PluginVer = "0.1.0";
    private const string CultOfQoLGuid = "p1xel8ted.cotl.CultOfQoLCollection";

    internal static ManualLogSource Log { get; private set; }
    internal static ConfigEntry<bool> SkipDevIntros { get; private set; }
    internal static ConfigEntry<bool> SkipCrownVideo { get; private set; }

    private void Awake()
    {
        Log = Logger;

        SkipDevIntros = Config.Bind("General", "Skip Intros", false,
            new ConfigDescription("Skip splash screens.", null,
                new ConfigurationManagerAttributes { Order = 2 }));

        SkipCrownVideo = Config.Bind("General", "Skip Crown Video", false,
            new ConfigDescription("Skips the video when the lamb gets given the crown.", null,
                new ConfigurationManagerAttributes { Order = 1 }));

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);

        if (Chainloader.PluginInfos.ContainsKey(CultOfQoLGuid))
        {
            Log.LogWarning("CultOfQoL detected. Its skip intro/video features will be disabled in favour of Skip of the Lamb.");
        }

        Helpers.PrintModLoaded(PluginName, Logger);
    }
}
