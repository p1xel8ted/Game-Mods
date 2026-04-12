namespace RestInPatches.Patches;

[Harmony]
public static class PhysicsPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerComponent), nameof(PlayerComponent.InitLocalPlayer))]
    public static void PlayerComponent_InitLocalPlayer(PlayerComponent __instance)
    {
        __instance.wgo.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(RoundAndSortComponent), nameof(RoundAndSortComponent.DoUpdateStuff))]
    public static IEnumerable<CodeInstruction> RoundAndSortComponent_DoUpdateStuff(IEnumerable<CodeInstruction> codes)
    {
        var positionSetter = AccessTools.PropertySetter(typeof(Transform), nameof(Transform.position));

        foreach (var code in codes)
        {
            if (code.Calls(positionSetter))
            {
                yield return new CodeInstruction(OpCodes.Ldarg_0);
                yield return CodeInstruction.Call(typeof(PhysicsPatches), nameof(PositionWrapper));
            }
            else
            {
                yield return code;
            }
        }
    }

    private static void PositionWrapper(Transform transform, Vector3 position, RoundAndSortComponent roundSort)
    {
        if (roundSort._world_part?.parent?.is_player ?? false)
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
