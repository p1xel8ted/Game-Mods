namespace CultOfQoL.Patches.Systems;

[HarmonyPatch]
public class MiscPatches
{
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(Interaction_HarvestMeat), nameof(Interaction_HarvestMeat.Update))]
    public static IEnumerable<CodeInstruction> Interaction_HarvestMeat_Update(IEnumerable<CodeInstruction> instructions)
    {
        var debugLogMethod = AccessTools.Method(typeof(Debug), nameof(Debug.Log), [typeof(object)]);
        var codes = instructions.ToList();
        for (var i = 0; i < codes.Count; i++)
        {
            if (codes[i].Calls(debugLogMethod) && codes[i - 1].opcode == OpCodes.Box)
            {
                Plugin.Log.LogInfo("Removing Debug.Log spam from Interaction_HarvestMeat.Update");
                codes.RemoveRange(i - 2, 3);
                break;
            }
        }
        
        return codes.AsEnumerable();
    }


    //for some reason, args is being passed null, which eventually makes its way to _extraText and causes a null reference exception
    //having this here stops the null
    [HarmonyPrefix]
    [HarmonyPatch(typeof(NotificationFaith), nameof(NotificationFaith.Localize))]
    public static void NotificationFaith_Localize(ref NotificationFaith __instance)
    {
        if (__instance._extraText is not {Length: > 0})
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
    [HarmonyPatch(typeof(Debug), nameof(Debug.LogWarning), typeof(object), typeof(Object))]
    [HarmonyPatch(typeof(Debug), nameof(Debug.LogWarning), typeof(object))]
    [HarmonyPatch(typeof(Debug), nameof(Debug.Log), typeof(object), typeof(Object))]
    [HarmonyPatch(typeof(Debug), nameof(Debug.Log), typeof(object))]
    public static bool Debug_Log(object message)
    {
        //print method and declaring type that called Debug.Log or Debug.LogWarning
        // Plugin.L($"Declaring Type: {new StackTrace().GetFrame(2).GetMethod().DeclaringType}, Method: {new StackTrace().GetFrame(2).GetMethod().Name} ");
        //
        if (message is not string s) return true;
        if (s.Contains("Skeleton AnimationState")) return false;
        if (s.Contains("called during processing")) return false;
        if (s.Contains("Steam informs us the controller is a")) return false;
        if (float.TryParse(s, out _) || int.TryParse(s, out _)) return false;
        if (s.Contains("connected")) return false;
        return false;
    }

}