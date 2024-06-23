namespace NoTimeToStopAndEat;

[Harmony]
public static class Patches
{
    private const string EatFoodRoutineA = "<EatFoodRoutine>b__20_0";
    private const string EatFoodRoutineB = "<EatFoodRoutine>b__20_1";
    private const string EatFoodRoutineC = "<EatFoodRoutine>b__20_2";

    /// <summary>
    /// Used to filter out certain types from an enumerator. For example, <see cref="WaitForSeconds"/> is used in the <see cref="Food.EatFoodRoutine"/> enumerator.
    /// </summary>
    /// <param name="original"></param>
    /// <param name="typeToRemove"></param>
    private static IEnumerator FilterEnumerator(IEnumerator original, Type typeToRemove)
    {
        while (original.MoveNext())
        {
            var current = original.Current;
            if (current != null && current.GetType() != typeToRemove)
            {
                yield return current;
            }
        }
    }

    /// <summary>
    /// Remove the <see cref="WaitForSeconds"/> from the <see cref="Food.EatFoodRoutine"/> enumerator.
    /// </summary>
    /// <param name="__result"></param>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Food), nameof(Food.EatFoodRoutine))]
    private static void Food_EatFoodRoutine(ref IEnumerator __result)
    {
        __result = FilterEnumerator(__result, typeof(WaitForSeconds));
    }

    /// <summary>
    /// Kills the numerous tweens that are used to animate the food item, cancels any remotes and enables walking animation. Also hides the food item if <see cref="Plugin.HideFoodItemWhenEating"/> is true.
    /// </summary>
    /// <param name="__instance"></param>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Food), nameof(Food.EatFoodRoutine), MethodType.Enumerator)]
    private static void Food_EatFoodRoutine(ref Food __instance)
    {
        if (!__instance) return;

        var itemGraphics = __instance._itemGraphics;
        if (itemGraphics)
        {
            if (Plugin.HideFoodItemWhenEating.Value)
            {
                var spriteRenderer = itemGraphics.GetComponent<SpriteRenderer>();
                if (spriteRenderer && spriteRenderer.sprite)
                {
                    spriteRenderer.sprite = null;
                }
            }
            DOTween.Kill(itemGraphics);
        }


        if (!Player.Instance) return;
        Player.Instance._emoteTween?.Kill();
        Player.Instance.FreezeWalkAnimations = false;
    }


    /// <summary>
    /// Sets the food item's position and scale to the default values (in front of the player) if <see cref="Plugin.HideFoodItemWhenEating"/> is false.
    /// </summary>
    /// <param name="__instance"></param>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Food), nameof(Food.Update))]
    private static void Food_Update(ref Food __instance)
    {
        if (__instance == null || __instance._itemGraphics == null) return;

        var spriteRenderer = __instance._itemGraphics.GetComponent<SpriteRenderer>();
        if (Plugin.HideFoodItemWhenEating.Value)
        {
            if (spriteRenderer.sprite != null)
            {
                spriteRenderer.sprite = null;
            }
            return;
        }

        var transform = __instance._itemGraphics.transform;
        if (transform.localPosition != new Vector3(0, -0.7f, -1f))
        {
            transform.localPosition = new Vector3(0, -0.7f, -1f);
        }
        if (transform.localScale != new Vector3(1f, 1f, 1f))
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }


    /// <summary>
    /// Stops the player from emoting/slowing down while eating food.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Food), EatFoodRoutineA)]
    [HarmonyPatch(typeof(Food), EatFoodRoutineB)]
    [HarmonyPatch(typeof(Food), EatFoodRoutineC)]
    private static bool Food_EatFoodRoutine_Prefix()
    {
        return false;
    }
}