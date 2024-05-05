using System.Reflection;
using Harmony12;
using Pathea.IntroduceNs;
using UnityEngine.SceneManagement;
using UnityModManagerNet;

namespace SkipIntros;

[Harmony]
internal static class Main
{
    private static void Load()
    {
        var harmony = HarmonyInstance.Create("p1xel8ted.portia.skipintros");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
        SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
        SceneManager.sceneUnloaded += SceneManagerOnSceneUnloaded;
        UnityModManagerNet.UnityModManager.Logger.Log($"Skip Intros Loaded", "Skip Intros");
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Splash), nameof(Splash.Start))]
    public static bool Splash_Start(ref Splash __instance)
    {
        UnityModManager.Logger.Log("Splash.Start", "Skip Intros");
        SceneManager.LoadScene("Game");
        return false;
    }

    private static void SceneManagerOnSceneUnloaded(Scene arg0)
    {
        UnityModManagerNet.UnityModManager.Logger.Log($"Scene unloaded: {arg0.name}", "Skip Intros");
    }

    private static void SceneManagerOnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        UnityModManagerNet.UnityModManager.Logger.Log($"Scene loaded: {arg0.name}", "Skip Intros");
    }
}