// using I2.Loc;
//
// namespace Seedify;
//
// [Harmony]
// public static class DescPatches
// {
//     [HarmonyTargetMethods]
//     public static IEnumerable<MethodBase> TargetMethods()
//     {
//         var methods = AccessTools.GetDeclaredProperties(typeof(ScriptLocalization));
//         foreach (var method in methods)
//         {
//             if (method.Name.Contains("seed_", StringComparison.OrdinalIgnoreCase) || method.Name.Contains("seeds_", StringComparison.OrdinalIgnoreCase))
//             {
//                 var meth = method.GetGetMethod();
//                 if (meth != null)
//                 {
//                     Plugin.Log.LogWarning($"Found method {meth.Name} in {meth.DeclaringType}");
//                     yield return meth;
//                 }
//             }
//         }
//     }
//
//     [HarmonyPostfix]
//     public static void ScriptLocalization_GetSeedDescription(ref string __result)
//     {
//         __result = "THIS IS A TEST!";
//     }
// }