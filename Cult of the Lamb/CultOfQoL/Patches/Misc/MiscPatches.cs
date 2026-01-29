namespace CultOfQoL.Patches.Misc;

[Harmony]
public class MiscPatches
{
    static MiscPatches()
    {
        // Game's MMJsonDataReadWriter.GetBackups crashes if the backup directory doesn't exist
        var backupDir = Path.Combine(Application.persistentDataPath, "backup");
        Directory.CreateDirectory(backupDir);
    }

    // Cached for performance
    private static readonly HashSet<string> SpamMessages = new(StringComparer.OrdinalIgnoreCase)
    {
        "Skeleton AnimationState",
        "called during processing",
        "Steam informs us the controller is a",
        "connected",
        "Fading IN transition",
        "Fading OUT transition",
        "Unable to determine ActionElementMap",
        "attempt Unlock for Avhievement",
        "Break here",
        "StoryObjectiveData must be instantiated",
        "Setup critters",
        "Using Unscaled Time",
        "Configure Node Connection Listener",
        "Configure Branch Connection Listener",
        "DLC is not installed, showing button",
        "DLC is installed, hiding button",
        "Releasing gameobject",
        "WAITING",
        "WeatherSystemController: Same system transition",
        "Max Tweens reached",
        "Follower eat stored food",
        "Required atlas size exceeds supported max",
        "MEAL_",
        "SKIP CAPTURED MOVE",
        "NO TILES AVAILALBLE",
        "Target player is already Instance",
        "Complete task!"
    };
    
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(Interaction_HarvestMeat), nameof(Interaction_HarvestMeat.Update))]
    public static IEnumerable<CodeInstruction> Interaction_HarvestMeat_Update(IEnumerable<CodeInstruction> instructions)
    {
        var original = instructions.ToList();
        try
        {
            var codes = new List<CodeInstruction>(original);
            var debugLogMethod = AccessTools.Method(typeof(Debug), nameof(Debug.Log), [typeof(object)]);
            var found = false;

            for (var i = 2; i < codes.Count; i++)
            {
                if (codes[i].Calls(debugLogMethod) && codes[i - 1].opcode == OpCodes.Box)
                {
                    codes.RemoveRange(i - 2, 3);
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Plugin.Log.LogWarning("[Transpiler] Interaction_HarvestMeat.Update: Failed to find Debug.Log call.");
                return original;
            }

            Plugin.Log.LogInfo("[Transpiler] Interaction_HarvestMeat.Update: Removed Debug.Log spam.");
            return codes;
        }
        catch (Exception ex)
        {
            Plugin.Log.LogWarning($"[Transpiler] Interaction_HarvestMeat.Update: {ex.Message}");
            return original;
        }
    }


    [HarmonyTranspiler]
    [HarmonyPatch(typeof(SimpleSpineAnimator), nameof(SimpleSpineAnimator.GetAnimationReference))]
    public static IEnumerable<CodeInstruction> SimpleSpineAnimator_GetAnimationReference_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var original = instructions.ToList();
        try
        {
            var codes = new List<CodeInstruction>(original);
            var ctor = AccessTools.Constructor(typeof(Spine.Unity.AnimationReferenceAsset));
            var createInstance = AccessTools.Method(typeof(ScriptableObject), nameof(ScriptableObject.CreateInstance), Type.EmptyTypes, [typeof(Spine.Unity.AnimationReferenceAsset)]);
            var found = false;

            for (var i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Newobj && codes[i].operand is ConstructorInfo ci && ci == ctor)
                {
                    codes[i] = new CodeInstruction(OpCodes.Call, createInstance).WithLabels(codes[i].labels);
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Plugin.Log.LogWarning("[Transpiler] SimpleSpineAnimator.GetAnimationReference: Failed to find AnimationReferenceAsset constructor.");
                return original;
            }

            Plugin.Log.LogInfo("[Transpiler] SimpleSpineAnimator.GetAnimationReference: Replaced new with ScriptableObject.CreateInstance.");
            return codes;
        }
        catch (Exception ex)
        {
            Plugin.Log.LogWarning($"[Transpiler] SimpleSpineAnimator.GetAnimationReference: {ex.Message}");
            return original;
        }
    }

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(LightingManager), nameof(LightingManager.Start))]
    public static IEnumerable<CodeInstruction> LightingManager_Start_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var original = instructions.ToList();
        try
        {
            var codes = new List<CodeInstruction>(original);
            var ctor = AccessTools.Constructor(typeof(BiomeLightingSettings));
            var createInstance = AccessTools.Method(typeof(ScriptableObject), nameof(ScriptableObject.CreateInstance), Type.EmptyTypes, [typeof(BiomeLightingSettings)]);
            var found = false;

            for (var i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Newobj && codes[i].operand is ConstructorInfo ci && ci == ctor)
                {
                    codes[i] = new CodeInstruction(OpCodes.Call, createInstance).WithLabels(codes[i].labels);
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Plugin.Log.LogWarning("[Transpiler] LightingManager.Start: Failed to find BiomeLightingSettings constructor.");
                return original;
            }

            Plugin.Log.LogInfo("[Transpiler] LightingManager.Start: Replaced new with ScriptableObject.CreateInstance.");
            return codes;
        }
        catch (Exception ex)
        {
            Plugin.Log.LogWarning($"[Transpiler] LightingManager.Start: {ex.Message}");
            return original;
        }
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
    [HarmonyPatch(typeof(Debug), nameof(Debug.LogError), typeof(object), typeof(Object))]
    [HarmonyPatch(typeof(Debug), nameof(Debug.LogError), typeof(object))]
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