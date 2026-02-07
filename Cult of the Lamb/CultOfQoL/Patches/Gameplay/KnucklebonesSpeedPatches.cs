using CultOfQoL.Core;
using DG.Tweening;
using KnuckleBones;

namespace CultOfQoL.Patches.Gameplay;

[Harmony]
public static class KnucklebonesSpeedPatches
{
    private static float SpeedMultiplier => ConfigCache.GetCachedValue(ConfigCache.Keys.KnucklebonesSpeedMultiplier, () => Plugin.KnucklebonesSpeedMultiplier.Value);

    private static readonly FieldInfo WaitSecondsField = AccessTools.Field(typeof(WaitForSecondsRealtime), "<waitTime>k__BackingField");

    /// <summary>
    /// Returns Time.unscaledDeltaTime multiplied by the speed multiplier.
    /// Called by transpiler-injected code to make time pass faster in dice animations.
    /// </summary>
    public static float GetScaledUnscaledDeltaTime() => Time.unscaledDeltaTime * SpeedMultiplier;

    private static readonly MethodInfo GetUnscaledDeltaTime = AccessTools.PropertyGetter(typeof(Time), nameof(Time.unscaledDeltaTime));
    private static readonly MethodInfo GetScaledUnscaledDeltaTimeMethod = AccessTools.Method(typeof(KnucklebonesSpeedPatches), nameof(GetScaledUnscaledDeltaTime));

    /// <summary>
    /// Wraps an enumerator and scales all WaitForSecondsRealtime yields by the speed multiplier.
    /// </summary>
    private static IEnumerator ScaleWaitTimes(IEnumerator original, float speedMultiplier)
    {
        while (original.MoveNext())
        {
            var current = original.Current;

            if (current is WaitForSecondsRealtime waitRealtime && WaitSecondsField != null)
            {
                var seconds = (float)WaitSecondsField.GetValue(waitRealtime);
                WaitSecondsField.SetValue(waitRealtime, seconds / speedMultiplier);
            }

            yield return current;
        }
    }

    // ═══════════════════════════════════════════════════════════════════════════
    // KBGameScreen coroutine patches - main game loop delays
    // ═══════════════════════════════════════════════════════════════════════════

    [HarmonyPostfix]
    [HarmonyPatch(typeof(KBGameScreen), nameof(KBGameScreen.DoGameLoop))]
    public static void KBGameScreen_DoGameLoop_Postfix(ref IEnumerator __result)
    {
        if (Mathf.Approximately(SpeedMultiplier, 1f)) return;
        __result = ScaleWaitTimes(__result, SpeedMultiplier);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(KBGameScreen), nameof(KBGameScreen.PerformTurn))]
    public static void KBGameScreen_PerformTurn_Postfix(ref IEnumerator __result)
    {
        if (Mathf.Approximately(SpeedMultiplier, 1f)) return;
        __result = ScaleWaitTimes(__result, SpeedMultiplier);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(KBGameScreen), nameof(KBGameScreen.DoShowAnimation))]
    public static void KBGameScreen_DoShowAnimation_Postfix(ref IEnumerator __result)
    {
        if (Mathf.Approximately(SpeedMultiplier, 1f)) return;
        __result = ScaleWaitTimes(__result, SpeedMultiplier);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(KBGameScreen), nameof(KBGameScreen.DoHideAnimation))]
    public static void KBGameScreen_DoHideAnimation_Postfix(ref IEnumerator __result)
    {
        if (Mathf.Approximately(SpeedMultiplier, 1f)) return;
        __result = ScaleWaitTimes(__result, SpeedMultiplier);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(KBGameScreen), nameof(KBGameScreen.DoEndGame))]
    public static void KBGameScreen_DoEndGame_Postfix(ref IEnumerator __result)
    {
        if (Mathf.Approximately(SpeedMultiplier, 1f)) return;
        __result = ScaleWaitTimes(__result, SpeedMultiplier);
    }

    // ═══════════════════════════════════════════════════════════════════════════
    // KBPlayerBase / KBOpponent patches - tub selection delays
    // ═══════════════════════════════════════════════════════════════════════════

    [HarmonyPostfix]
    [HarmonyPatch(typeof(KBPlayerBase), nameof(KBPlayerBase.FinishTubSelection))]
    public static void KBPlayerBase_FinishTubSelection_Postfix(ref IEnumerator __result)
    {
        if (Mathf.Approximately(SpeedMultiplier, 1f)) return;
        __result = ScaleWaitTimes(__result, SpeedMultiplier);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(KBOpponent), nameof(KBOpponent.FinishTubSelection))]
    public static void KBOpponent_FinishTubSelection_Postfix(ref IEnumerator __result)
    {
        if (Mathf.Approximately(SpeedMultiplier, 1f)) return;
        __result = ScaleWaitTimes(__result, SpeedMultiplier);
    }

    // ═══════════════════════════════════════════════════════════════════════════
    // Dice patches - rolling and movement animations
    // ═══════════════════════════════════════════════════════════════════════════

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Dice), nameof(Dice.RollRoutine))]
    public static void Dice_RollRoutine_Postfix(ref IEnumerator __result)
    {
        if (Mathf.Approximately(SpeedMultiplier, 1f)) return;
        __result = ScaleWaitTimes(__result, SpeedMultiplier);
    }

    // ═══════════════════════════════════════════════════════════════════════════
    // Dice DOTween cleanup - kill any running tweens before movement starts
    // RollRoutine uses a 1-second DOMove tween. When we speed up the routine's
    // WaitForSecondsRealtime, it finishes before the DOTween. GoToLocationRoutine
    // then fights with the still-running DOTween, causing the dice to vanish.
    // ═══════════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Kills any running DOTween on the dice's rectTransform before GoToLocationRoutine starts.
    /// This prevents the RollRoutine's DOMove from fighting with the manual position setting.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Dice), nameof(Dice.GoToLocationRoutine))]
    private static void Dice_GoToLocationRoutine_KillTweens(Dice __instance)
    {
        if (Mathf.Approximately(SpeedMultiplier, 1f)) return;

        // Kill any DOTween animations on this dice's rectTransform
        __instance.rectTransform.DOKill();
        Plugin.WriteLog($"[Knucklebones] Killed DOTween on dice before GoToLocationRoutine (speed: {SpeedMultiplier}x)");
    }

    // ═══════════════════════════════════════════════════════════════════════════
    // Dice deltaTime transpilers - scale Time.unscaledDeltaTime reads
    // Instead of modifying duration constants, we multiply the deltaTime value
    // so time passes faster from the routine's perspective.
    // ═══════════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Transpiler for GoToLocationRoutine - replaces Time.unscaledDeltaTime with scaled version.
    /// Original: Progress += Time.unscaledDeltaTime
    /// Patched:  Progress += GetScaledUnscaledDeltaTime()
    /// </summary>
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(Dice), nameof(Dice.GoToLocationRoutine), MethodType.Enumerator)]
    private static IEnumerable<CodeInstruction> Dice_GoToLocationRoutine_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return PatchUnscaledDeltaTime(instructions, "Dice.GoToLocationRoutine");
    }

    /// <summary>
    /// Transpiler for ShakeRoutine - replaces Time.unscaledDeltaTime with scaled version.
    /// Original: Shake -= Time.unscaledDeltaTime
    /// Patched:  Shake -= GetScaledUnscaledDeltaTime()
    /// </summary>
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(Dice), nameof(Dice.ShakeRoutine), MethodType.Enumerator)]
    private static IEnumerable<CodeInstruction> Dice_ShakeRoutine_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return PatchUnscaledDeltaTime(instructions, "Dice.ShakeRoutine");
    }

    /// <summary>
    /// Transpiler for ScaleRoutine - replaces Time.unscaledDeltaTime with scaled version.
    /// Original: Progress += Time.unscaledDeltaTime
    /// Patched:  Progress += GetScaledUnscaledDeltaTime()
    /// </summary>
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(Dice), nameof(Dice.ScaleRoutine), MethodType.Enumerator)]
    private static IEnumerable<CodeInstruction> Dice_ScaleRoutine_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return PatchUnscaledDeltaTime(instructions, "Dice.ScaleRoutine");
    }

    /// <summary>
    /// Shared transpiler logic: replaces calls to Time.get_unscaledDeltaTime() with GetScaledUnscaledDeltaTime().
    /// </summary>
    private static IEnumerable<CodeInstruction> PatchUnscaledDeltaTime(IEnumerable<CodeInstruction> instructions, string methodName)
    {
        var codes = new List<CodeInstruction>(instructions);
        var patched = 0;

        for (var i = 0; i < codes.Count; i++)
        {
            if (codes[i].Calls(GetUnscaledDeltaTime))
            {
                codes[i] = new CodeInstruction(OpCodes.Call, GetScaledUnscaledDeltaTimeMethod).WithLabels(codes[i].labels);
                patched++;
            }
        }

        if (patched > 0)
        {
            Plugin.Log.LogInfo($"[Transpiler] {methodName}: Replaced {patched} Time.unscaledDeltaTime call(s) with scaled version.");
        }
        else
        {
            Plugin.Log.LogWarning($"[Transpiler] {methodName}: Failed to find Time.unscaledDeltaTime calls.");
        }

        return codes;
    }

    // ═══════════════════════════════════════════════════════════════════════════
    // KBDiceTub patches - dice placement
    // ═══════════════════════════════════════════════════════════════════════════

    [HarmonyPostfix]
    [HarmonyPatch(typeof(KBDiceTub), nameof(KBDiceTub.AddDice))]
    public static void KBDiceTub_AddDice_Postfix(ref IEnumerator __result)
    {
        if (Mathf.Approximately(SpeedMultiplier, 1f)) return;
        __result = ScaleWaitTimes(__result, SpeedMultiplier);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(KBDiceTub), nameof(KBDiceTub.CheckDice))]
    public static void KBDiceTub_CheckDice_Postfix(ref IEnumerator __result)
    {
        if (Mathf.Approximately(SpeedMultiplier, 1f)) return;
        __result = ScaleWaitTimes(__result, SpeedMultiplier);
    }
}
