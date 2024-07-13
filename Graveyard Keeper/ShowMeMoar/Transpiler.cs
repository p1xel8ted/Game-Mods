namespace ShowMeMoar;

/// <summary>
/// A Harmony patch to intercept Unity's Screen.resolutions method and add custom resolutions.
/// </summary>
[HarmonyPatch]
public static class ScreenResolutionsPatch
{
    /// <summary>
    /// Provides a list of resolutions, including a custom one based on the main display.
    /// This method is meant to replace <see cref="Screen.resolutions"/>.
    /// </summary>
    /// <returns>A list of resolutions, including the custom one.</returns>
    public static Resolution[] MyResolutions()
    {
        Plugin.Log.LogWarning("Unity Screen.resolutions intercepted!");
        var newRes = new Resolution
        {
            height = Display.main.systemHeight,
            width = Display.main.systemWidth,
            refreshRate = Screen.resolutions.Max(a => a.refreshRate)
        };
        var availableResolutions = Screen.resolutions.ToList();
        availableResolutions.Add(newRes);
        return availableResolutions.ToArray();
    }
    
    /// <summary>
    /// Replaces calls to <see cref="Screen.resolutions"/> with calls to <see cref="MyResolutions"/> in the target methods.
    /// </summary>
    /// <param name="instructions">The original IL instructions of the method.</param>
    /// <returns>The modified IL instructions.</returns>
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(ResolutionConfig), nameof(ResolutionConfig.InitResolutions))]
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        if (!Plugin.Ultrawide.Value)
        {
            Plugin.Log.LogWarning("Transpiler: Screen.resolutions transpiler not applied!");
            return instructions.AsEnumerable();
        }
        var originalInstructions = instructions.ToList();
        var getResolutionsMethod = AccessTools.Property(typeof(Screen), nameof(Screen.resolutions))?.GetGetMethod();
        var myResolutionsMethod = AccessTools.Method(typeof(ScreenResolutionsPatch), nameof(MyResolutions));

        foreach (var t in originalInstructions.Where(t => t.Calls(getResolutionsMethod)))
        {
            t.operand = myResolutionsMethod;
        }

        Plugin.Log.LogWarning("Screen.resolutions transpiler applied!");
        return originalInstructions.AsEnumerable();
    }
}