// using System.Collections.Generic;
// using System.Diagnostics.CodeAnalysis;
// using System.Linq;
// using System.Reflection;
// using AudioEnum;
// using HarmonyLib;
// using Sirenix.Utilities;
//
// namespace AnAlchemicalCollection;
//
// [Harmony]
// [SuppressMessage("ReSharper", "InconsistentNaming")]
// public static class TargetMethodPatches
// {
//     
//     [HarmonyTargetMethods]
//     public static IEnumerable<MethodBase> TargetMethods()
//     {
//         return AccessTools.GetDeclaredMethods(typeof(Chemist_SFX)).Where(a=>a.GetParamsNames().Contains("id"));
//     }
//
//     [HarmonyPrefix]
//     public static void Prefix(MethodBase __originalMethod, ref SFXID id)
//     {
//         Plugin.L($"Playing {id.ToString()} with {__originalMethod.Name}");
//     }
//     
// }