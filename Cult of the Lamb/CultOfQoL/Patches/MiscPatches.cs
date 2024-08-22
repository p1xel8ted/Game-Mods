using Socket.Newtonsoft.Json.Utilities.LinqBridge;
using Debug = UnityEngine.Debug;
using Debugger = DG.Tweening.Core.Debugger;

namespace CultOfQoL.Patches;

[HarmonyPatch]
public class MiscPatches
{
    //for some reason, args is being passed null, which eventually makes its way to _extraText and causes a null reference exception
    //having this here stops the null
    [HarmonyPrefix]
    [HarmonyPatch(typeof(NotificationFaith), nameof(NotificationFaith.Localize))]
    public static void NotificationFaith_Localize(ref NotificationFaith __instance)
    {
        if (__instance._extraText is {Length: > 0})
        {
            // foreach (var s in __instance._extraText)
            // {
            //     Plugin.L($"ExtraText: {s}");
            // }
        }
        else
        {
            __instance._extraText ??= [];
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Debugger), nameof(Debugger.LogInvalidTween))]
    public static bool Debugger_LogInvalidTween(Tween t)
    {
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Debug), nameof(Debug.LogWarning), typeof(object),  typeof(Object))]
    [HarmonyPatch(typeof(Debug), nameof(Debug.LogWarning), typeof(object))]
    [HarmonyPatch(typeof(Debug), nameof(Debug.Log), typeof(object),  typeof(Object))]
    [HarmonyPatch(typeof(Debug), nameof(Debug.Log), typeof(object))]
    public static bool Debug_Log(object message)
    {
        if (message is not string s) return true;
        if (s.Contains("Skeleton AnimationState")) return false;
        if (s.Contains("called during processing")) return false;
        if (s.Contains("Steam informs us the controller is a")) return false;
        if (float.TryParse(s, out _) || int.TryParse(s, out _)) return false;
        if (s.Contains("connected")) return false;
        return false;
    }

}