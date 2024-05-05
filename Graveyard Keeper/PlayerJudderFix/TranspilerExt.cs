using System;
using System.Collections.Generic;
using HarmonyLib;

namespace PlayerJudderFix;

public static class TranspilerExt
{
    public static IEnumerable<CodeInstruction> Manipulator(this IEnumerable<CodeInstruction> codes, Func<CodeInstruction, bool> predicate,
        Func<CodeInstruction, IEnumerable<CodeInstruction>> function)
    {
        foreach (var c in codes)
        {
            if (predicate(c))
                foreach (var i in function(c))
                    yield return i;
            else
                yield return c;
        }
    }
}