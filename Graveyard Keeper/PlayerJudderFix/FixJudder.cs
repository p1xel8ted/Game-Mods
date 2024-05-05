using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.Rendering;

namespace PlayerJudderFix;

[HarmonyPatch]
internal static class PlayerJudderFix
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerComponent), nameof(PlayerComponent.InitLocalPlayer))]
    private static void PlayerComponent_InitLocalPlayer(PlayerComponent __instance)
    {
        __instance.wgo.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.Interpolate;
    }


    [HarmonyPatch(typeof(RoundAndSortComponent), nameof(RoundAndSortComponent.DoUpdateStuff))]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codes)
    {
        var positionSetter = AccessTools.PropertySetter(typeof(Transform), nameof(Transform.position));

        return codes.Manipulator(
            i => i.Calls(positionSetter),
            _ => new[]
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                CodeInstruction.Call(typeof(PlayerJudderFix), nameof(Wrapper))
            });
    }

    private static void Wrapper(Transform transform, Vector3 position, RoundAndSortComponent roundSort)
    {
        if (roundSort != null && roundSort._world_part != null && roundSort._world_part.parent != null && roundSort._world_part.parent.is_player)
        {
            var tf = transform.GetComponentInChildren<SortingGroup>().transform;
            tf.position = new Vector3(tf.position.x, tf.position.y, position.z);
        }
        else
        {
            transform.position = position;
        }
    }
}