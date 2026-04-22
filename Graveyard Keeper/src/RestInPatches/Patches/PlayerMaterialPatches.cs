namespace RestInPatches.Patches;

// PlayerComponent.Update writes player_material.SetColor("_AdditionalColour", ...) every
// frame with a string key. The string lookup hashes into Shader's property table each
// call, and the native material write fires even when the colour hasn't changed. Most
// of the time player_additional_color stays at Color.black for the whole session.
//
// This patch transpiles the SetColor call site: replace the string-keyed overload with
// the int-keyed overload using a cached Shader.PropertyToID, and skip the write when the
// value matches what we wrote last.
[Harmony]
public static class PlayerMaterialPatches
{
    private static readonly int AdditionalColourId = Shader.PropertyToID("_AdditionalColour");

    private static readonly ConditionalWeakTable<Material, ColorBox> LastColors = new();

    private sealed class ColorBox
    {
        public Color Value;
        public bool Set;
    }

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(PlayerComponent), nameof(PlayerComponent.Update))]
    public static IEnumerable<CodeInstruction> PlayerComponent_Update_Transpiler(IEnumerable<CodeInstruction> codes)
    {
        var setColorStr = AccessTools.Method(typeof(Material), nameof(Material.SetColor), new[] { typeof(string), typeof(Color) });
        var replacement = AccessTools.Method(typeof(PlayerMaterialPatches), nameof(SetAdditionalColourCached));

        foreach (var code in codes)
        {
            if (code.Calls(setColorStr))
            {
                // Stack entering the original call: [Material, string "_AdditionalColour", Color].
                // Our replacement signature is (Material, string, Color) -> ignores the string arg
                // and uses the cached property ID. This keeps the transpiler minimally invasive —
                // we accept the string push that the original emits and just drop it in the helper.
                yield return new CodeInstruction(OpCodes.Call, replacement);
            }
            else
            {
                yield return code;
            }
        }
    }

    private static void SetAdditionalColourCached(Material material, string _, Color color)
    {
        if (material == null)
        {
            return;
        }

        var box = LastColors.GetOrCreateValue(material);
        if (box.Set && box.Value == color)
        {
            return;
        }

        box.Value = color;
        box.Set = true;
        material.SetColor(AdditionalColourId, color);
    }
}
