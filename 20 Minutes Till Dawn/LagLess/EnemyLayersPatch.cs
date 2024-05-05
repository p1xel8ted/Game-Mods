using flanne;
using HarmonyLib;

namespace LagLess;

[Harmony]
public static class EnemyLayersPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(AIComponent), nameof(AIComponent.Start))]
    private static void AIComponent_Start(ref AIComponent __instance)
    {
        var enemy = __instance.gameObject.tag.StartsWith("Enemy");
        if (enemy)
        {
            __instance.gameObject.layer = LLLayers.enemyLayer;
        }
    }

}