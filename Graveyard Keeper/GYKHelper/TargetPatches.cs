using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;

namespace GYKHelper;

[HarmonyPatch]
public static class DataNotFoundPatch
{
    [HarmonyTargetMethods]
    public static IEnumerable<MethodBase> TargetMethods()
    {
        yield return AccessTools.Method(typeof(GameBalanceBase), nameof(GameBalanceBase.DataNotFound)).MakeGenericMethod(typeof(ObjectCraftDefinition));
    }

    [HarmonyPrefix]
    public static bool ObjectCraftDefinition_DataNotFound()
    {
        return false;
    }
}