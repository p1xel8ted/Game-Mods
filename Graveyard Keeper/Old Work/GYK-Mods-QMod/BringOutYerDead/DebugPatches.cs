// using FlowCanvas;
// using HarmonyLib;
// using UnityEngine;
//
// namespace BringOutYerDead;
//
// [HarmonyPatch]
// public static partial class MainPatcher
// {
//     [HarmonyPrefix]
//     [HarmonyPatch(typeof(FlowScriptEngine), nameof(FlowScriptEngine.SendEvent), typeof(string))]
//     public static bool FlowScriptEngine_SendEvent(FlowScriptEngine __instance, FlowScriptEngine ____me, FlowScriptController[] ____scripts, string event_name)
//     {
//         if (!event_name.Contains("donkey")) return true;
//         
//         Debug.Log("FlowScriptEngine.SendEvent: " + event_name);
//         FlowScriptController[] scripts = ____scripts;
//         for (int i = 0; i < scripts.Length; i++)
//         {
//             scripts[i].SendEvent(event_name);
//         }
//         
//         return false;
//     }
// }