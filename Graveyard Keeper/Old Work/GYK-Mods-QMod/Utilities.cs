[HarmonyPatch(typeof(ResolutionConfig), "GetResolutionConfigOrNull")]
public static class PrintOutIlCode
{
    [HarmonyTranspiler]
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        string file = null;
        try
        {

            var codes = new List<CodeInstruction>(instructions);
            for (var i = 0; i < codes.Count; i++)
            {
                var code = codes[i];
                file += (i + " : " + code.opcode + " : " + code.operand + "\n");
            }
            File.WriteAllText("il-code.txt", file, Encoding.Default);
            return codes.AsEnumerable();
        }
        catch (System.Exception ex)
        {
            File.WriteAllText("il-code.txt", ex.Message + " : " + ex.Source + " : " + ex.StackTrace, Encoding.Default);
        }

        return null;
    }
}