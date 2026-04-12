namespace RestInPatches.Patches;

public static class FootprintPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(LeaveTrailComponent), nameof(LeaveTrailComponent.LeaveTrail))]
    public static void LeaveTrailComponent_LeaveTrail(LeaveTrailComponent __instance)
    {
        var max = Plugin.MaxFootprints.Value;
        if (max <= 0 || LeaveTrailComponent._all_trails.Count <= max)
        {
            return;
        }

        var oldest = LeaveTrailComponent._all_trails.First();
        LeaveTrailComponent.OnTrailObjectDestroyed(oldest);
        if (!oldest.GetComponent<SpriteRenderer>().isVisible)
        {
            UnityEngine.Object.Destroy(oldest.gameObject);
        }
        else
        {
            oldest._degrading = false;
            oldest.gameObject.AddComponent<DestroyWhenInvisible>();
        }
    }

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(TrailObject), nameof(TrailObject.Update))]
    public static IEnumerable<CodeInstruction> TrailObject_Update(IEnumerable<CodeInstruction> codes)
    {
        var colorGetter = AccessTools.PropertyGetter(typeof(SpriteRenderer), nameof(SpriteRenderer.color));
        var setAlphaMethod = AccessTools.Method(typeof(ExtentionTools), nameof(ExtentionTools.SetAlpha));

        var matcher = new CodeMatcher(codes)
            .SearchForward(i => i.Calls(colorGetter))
            .RemoveInstruction()
            .SearchForward(i => i.Calls(setAlphaMethod))
            .SetInstruction(Transpilers.EmitDelegate<Action<SpriteRenderer, float>>((spr, alpha) =>
            {
                var color = spr.color;
                color.a = alpha;
                spr.color = color;
            }));

        return matcher.InstructionEnumeration();
    }
}
