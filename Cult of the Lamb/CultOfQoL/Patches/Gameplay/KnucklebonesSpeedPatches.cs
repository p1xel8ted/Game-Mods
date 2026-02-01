using CultOfQoL.Core;
using KnuckleBones;

namespace CultOfQoL.Patches.Gameplay;

[Harmony]
public static class KnucklebonesSpeedPatches
{
    private static float SpeedMultiplier => ConfigCache.GetCachedValue(ConfigCache.Keys.KnucklebonesSpeedMultiplier, () => Plugin.KnucklebonesSpeedMultiplier.Value);

    private static readonly FieldInfo WaitSecondsField = AccessTools.Field(typeof(WaitForSecondsRealtime), "<waitTime>k__BackingField");

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

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Dice), nameof(Dice.GoToLocationRoutine))]
    public static void Dice_GoToLocationRoutine_Postfix(ref IEnumerator __result)
    {
        if (Mathf.Approximately(SpeedMultiplier, 1f)) return;
        __result = ScaleDeltaTimeLoop(__result, SpeedMultiplier);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Dice), nameof(Dice.ShakeRoutine))]
    public static void Dice_ShakeRoutine_Postfix(ref IEnumerator __result)
    {
        if (Mathf.Approximately(SpeedMultiplier, 1f)) return;
        __result = ScaleDeltaTimeLoop(__result, SpeedMultiplier);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Dice), nameof(Dice.ScaleRoutine))]
    public static void Dice_ScaleRoutine_Postfix(ref IEnumerator __result)
    {
        if (Mathf.Approximately(SpeedMultiplier, 1f)) return;
        __result = ScaleDeltaTimeLoop(__result, SpeedMultiplier);
    }

    /// <summary>
    /// For coroutines that use unscaledDeltaTime with a fixed duration, we can't easily
    /// modify the duration constant. Instead, we wrap the enumerator to advance it multiple
    /// times per frame based on the speed multiplier.
    /// </summary>
    private static IEnumerator ScaleDeltaTimeLoop(IEnumerator original, float speedMultiplier)
    {
        // For loops using unscaledDeltaTime, we need to run multiple iterations per frame
        // to effectively speed them up. The original loop does:
        //   while (progress < duration) { progress += Time.unscaledDeltaTime; yield return null; }
        //
        // We can't change the duration constant, but we can skip frames or advance faster
        // by running multiple MoveNext calls per yield.

        var frameSkipAccumulator = 0f;

        while (original.MoveNext())
        {
            var current = original.Current;

            // For null yields (frame waits), potentially skip frames based on speed
            if (current == null && speedMultiplier > 1f)
            {
                frameSkipAccumulator += speedMultiplier - 1f;

                // Advance the original enumerator extra times to speed up
                while (frameSkipAccumulator >= 1f)
                {
                    frameSkipAccumulator -= 1f;
                    if (!original.MoveNext())
                    {
                        yield break;
                    }
                }
            }

            yield return current;
        }
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
