using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using FlowCanvas.Nodes;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace GYKHelper;

[HarmonyPriority(1)]
internal class MiscFixes
{
    //stops unnecessary log spam due to spawning clone objects that don't have matching sprites
    [HarmonyPriority(1)]
    [HarmonyPrefix]
    [HarmonyPatch(typeof(EasySpritesCollection), nameof(EasySpritesCollection.GetSprite))]
    public static void EasySpritesCollection_GetSprite(ref bool not_found_is_valid)
    {
        not_found_is_valid = true;
    }


    //stops unnecessary log spam due to wheres ma storage inventory changes

    [HarmonyPriority(1)]
    [HarmonyPrefix]
    [HarmonyPatch(typeof(MultiInventory), nameof(MultiInventory.IsEmpty))]
    public static void MultiInventory_IsEmpty(ref bool print_log)
    {
        print_log = false;
    }


    //stops unnecessary path finding failed log spam

    // [HarmonyPriority(1)]
    // [HarmonyTranspiler]
    // [HarmonyPatch(typeof(MovementComponent), nameof(MovementComponent.OnPathFailed))]
    // private static IEnumerable<CodeInstruction>? MovementComponent_OnPathFailed_Transpiler(IEnumerable<CodeInstruction> instructions)
    // {
    //     var instructionsList = new List<CodeInstruction>(instructions);
    //     instructionsList.RemoveRange(0, 9);
    //     return instructionsList.AsEnumerable();
    // }
        
    // [HarmonyPrefix]
    // [HarmonyPatch(typeof(WorldMap), nameof(WorldMap.GetWorldGameObjectsByComparator))]
    // public static void WorldMap_GetWorldGameObjectsByComparator(ref bool log_if_not_found)
    // {
    //     log_if_not_found = false;
    // }
    //
    // [HarmonyPriority(1)]
    // [HarmonyTranspiler]
    // [HarmonyPatch(typeof(WorldMap), nameof(WorldMap.GetWorldGameObjectByComparator))]
    // private static IEnumerable<CodeInstruction> WorldMap_GetWorldGameObjectByComparator_Transpiler(IEnumerable<CodeInstruction> instructions)
    // {
    //     var instructionsList = new List<CodeInstruction>(instructions);
    //     
    //     instructionsList[92].opcode = OpCodes.Nop;
    //     instructionsList[93].opcode = OpCodes.Nop;
    //     instructionsList[94].opcode = OpCodes.Nop;
    //     instructionsList[95].opcode = OpCodes.Nop;
    //     
    //     instructionsList[102].opcode = OpCodes.Nop;
    //     instructionsList[103].opcode = OpCodes.Nop;
    //     instructionsList[104].opcode = OpCodes.Nop;
    //     instructionsList[105].opcode = OpCodes.Nop;
    //     instructionsList[106].opcode = OpCodes.Nop;
    //     instructionsList[107].opcode = OpCodes.Nop;
    //     instructionsList[108].opcode = OpCodes.Nop;
    //     instructionsList[109].opcode = OpCodes.Nop;
    //
    //     return instructionsList.AsEnumerable();
    // }


    //stops unnecessary carrying item spr null errors


    // [HarmonyPriority(1)]
    // [HarmonyTranspiler]
    // [CanBeNull]
    // [HarmonyPatch(typeof(BaseCharacterComponent), nameof(BaseCharacterComponent.SetCarryingItem), typeof(Item))]
    // private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    // {
    //     var instructionsList = new List<CodeInstruction>(instructions);
    //     instructionsList.RemoveRange(28, 2);
    //     return instructionsList.AsEnumerable();
    // }
        

    // //stops unnecessary no animator spam
    // [HarmonyPriority(1)]
    // [HarmonyTranspiler]
    // [CanBeNull]
    // [HarmonyPatch(typeof(CustomNetworkAnimatorSync), nameof(CustomNetworkAnimatorSync.Init), typeof(GameObject))]
    // private static IEnumerable<CodeInstruction> CustomNetworkAnimatorSync_Init_Transpiler(IEnumerable<CodeInstruction> instructions)
    // {
    //     var instructionsList = new List<CodeInstruction>(instructions);
    //     instructionsList.RemoveRange(19, 6);
    //     return instructionsList.AsEnumerable();
    // }
    //

    [HarmonyFinalizer]
    [HarmonyPatch(typeof(DynamicLights), nameof(DynamicLights.SearchForLightsInNewObject), typeof(GameObject))]
    public static Exception DynamicLights_SearchForLightsInNewObject_Finalizer()
    {
        return null;
    }
 
    [HarmonyPrefix]
    [HarmonyPatch(typeof(DynamicLights), nameof(DynamicLights.SearchForLightsInNewObject), typeof(GameObject))]
    public static bool DynamicLights_SearchForLightsInNewObject(ref GameObject go,ref List<Light> ____lights_ground,
        ref List<float> ____lights_ground_k,
        ref List<DynamicSpritePreset> ____dyn_lights_ground,
        ref List<Light> ____lights_default ,
        ref List<DynamicLight> ____dyn_lights_default,
        ref List<float> ____lights_default_k)
    {
            
        var list = ____lights_ground;
        var k = ____lights_ground_k;
        var ground = ____dyn_lights_ground;
        var lights = ____lights_default;
        var @default = ____dyn_lights_default;
        var floats = ____lights_default_k;
        
        Parallel.ForEach(go.GetComponentsInChildren<Light>(true), light =>
        {
            var groundLight = light.gameObject.GetComponentInParent<GroundLight>();
            if (groundLight == null)
            {
                groundLight = light.gameObject.transform.parent.GetComponent<GroundLight>();
            }

            if (groundLight == null)
            {
                groundLight = light.gameObject.GetComponentInChildren<GroundLight>();
            }

            if (groundLight != null)
            {
                list.Add(light);
                k.Add(groundLight.intensity_k);
                ground.Add(groundLight.intensity_preset);
            }
            else
            {
                var dynamicLight = light.gameObject.GetComponentInParent<DynamicLight>() ?? light.gameObject.transform.parent.GetComponent<DynamicLight>();
                if (dynamicLight == null)
                {
                    dynamicLight = light.gameObject.GetComponentInChildren<DynamicLight>();
                }

                if (dynamicLight != null)
                {

                    lights.Add(light);
                    @default.Add(dynamicLight);
                    floats.Add(dynamicLight.intensity_k);
                }
            }
        });

        return false;
    }


    // [CanBeNull]
    // [HarmonyTranspiler]
    // [HarmonyDebug]
    // [HarmonyPriority(1)]
    // [HarmonyPatch(typeof(DynamicLights), "SearchForLightsInNewObject")]
    // private static IEnumerable<CodeInstruction> DynamicLights_SearchForLightsInNewObject_Transpiler(IEnumerable<CodeInstruction> instructions)
    // {
    //     var instructionsList = new List<CodeInstruction>(instructions);
    //     instructionsList.RemoveRange(73, 6);
    //     return instructionsList.AsEnumerable();
    // }

    //stops unnecessary duplicate objects spam from spawning clone vendors
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(Flow_TryFreeIdlePoint))]
    public static class FlowTryFreeIdlePointRegisterPortsPatch
    {
        internal static IEnumerable<MethodBase> TargetMethods()
        {
            var inner = typeof(Flow_TryFreeIdlePoint).GetNestedType("<>c__DisplayClass0_0", AccessTools.all)
                        ?? throw new Exception("Inner Not Found");

            foreach (var method in inner.GetMethods(AccessTools.all))
            {
                if (method.Name.Contains("<RegisterPorts>") && method.GetParameters().Length == 1)
                {
                    yield return method;
                }
            }
        }

        [HarmonyTranspiler]
        [CanBeNull]
        private static IEnumerable<CodeInstruction> Flow_TryFreeIdlePoint_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var instructionsList = new List<CodeInstruction>(instructions);
            for (var index = 0; index < instructionsList.Count; index++)
            {
                if (instructionsList[index].opcode == OpCodes.Ldstr && instructionsList[index].operand.ToString().ToLowerInvariant().StartsWith("can") && instructionsList[index + 1].opcode == OpCodes.Call)
                {
                    instructionsList[index].opcode = OpCodes.Nop;
                    instructionsList[index + 1].opcode = OpCodes.Nop;
                }
            }

            return instructionsList.AsEnumerable();
        }
    }
}