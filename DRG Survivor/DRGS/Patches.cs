namespace DRGS;

[Harmony]
public static class Patches
{
    private static DropPod DropPodInstance { get; set; }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(DropPod), nameof(DropPod.Awake))]
    public static void DropPod_Awake(DropPod __instance)
    {
        DropPodInstance = __instance;
        UpdateDropPodTimer();

    }
    internal static void UpdateDropPodTimer()
    {
        if (!DropPodInstance) return;
        DropPodInstance.secondsToTimeOut = (int) (Plugin.DropPodTime.Value * 60);
    }
    
}