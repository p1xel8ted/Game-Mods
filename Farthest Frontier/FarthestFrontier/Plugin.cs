using JetBrains.Annotations;
using MelonLoader;

[assembly: MelonInfo(typeof(FarthestFrontier.Plugin), "Farthest Frontier Tweaks", "0.1.0", "p1xel8ted")]

namespace FarthestFrontier;

public class Plugin : MelonMod
{
    public override void OnInitializeMelon()
    {
        base.OnInitializeMelon();
        MelonLogger.Msg("Farthest Frontier Tweaks Loaded");
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        base.OnSceneWasLoaded(buildIndex, sceneName);
        MelonLogger.Msg($"Scene Loaded: {sceneName}");
    }
}