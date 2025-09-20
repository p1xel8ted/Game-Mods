namespace CultOfQoL.Patches.Systems;

[HarmonyPatch]
public class MiscPatches
{
    // Cached for performance
    private static readonly HashSet<string> SpamMessages = new(StringComparer.OrdinalIgnoreCase)
    {
        "Skeleton AnimationState",
        "called during processing",
        "Steam informs us the controller is a",
        "connected"
        
    };
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(Interaction_HarvestMeat), nameof(Interaction_HarvestMeat.Update))]
    public static IEnumerable<CodeInstruction> Interaction_HarvestMeat_Update(IEnumerable<CodeInstruction> instructions)
    {
        var debugLogMethod = AccessTools.Method(typeof(Debug), nameof(Debug.Log), [typeof(object)]);
        var codes = instructions.ToList();
        
        for (var i = 2; i < codes.Count; i++) // Start at 2 to avoid index out of bounds
        {
            if (codes[i].Calls(debugLogMethod) && codes[i - 1].opcode == OpCodes.Box)
            {
                Plugin.Log.LogInfo("Removing Debug.Log spam from Interaction_HarvestMeat.Update");
                codes.RemoveRange(i - 2, 3);
                break;
            }
        }
        
        return codes;
    }


    //for some reason, args is being passed null, which eventually makes its way to _extraText and causes a null reference exception
    //having this here stops the null
    // Prevents null reference exception when _extraText is null
    [HarmonyPrefix]
    [HarmonyPatch(typeof(NotificationFaith), nameof(NotificationFaith.Localize))]
    public static void NotificationFaith_Localize(ref NotificationFaith __instance)
    {
        __instance._extraText ??= [];
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Debugger), nameof(Debugger.LogInvalidTween))]
    public static bool Debugger_LogInvalidTween(Tween t)
    {
        return false; // Suppress invalid tween logging
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Debug), nameof(Debug.LogWarning), typeof(object), typeof(Object))]
    [HarmonyPatch(typeof(Debug), nameof(Debug.LogWarning), typeof(object))]
    [HarmonyPatch(typeof(Debug), nameof(Debug.Log), typeof(object), typeof(Object))]
    [HarmonyPatch(typeof(Debug), nameof(Debug.Log), typeof(object))]
    public static bool Debug_Log(object message)
    {
       
        if (message is not string s) return true;
        
        // Block numeric-only logs
        if (s.All(c => char.IsDigit(c) || c == '.' || c == '-'))
            return false;
        
        // Block known spam messages
        return !SpamMessages.Any(spam => s.Contains(spam, StringComparison.OrdinalIgnoreCase));
    }

}