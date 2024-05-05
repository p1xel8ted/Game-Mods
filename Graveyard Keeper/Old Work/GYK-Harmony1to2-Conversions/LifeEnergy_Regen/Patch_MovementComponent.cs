using HarmonyLib;

namespace LifeEnergy_Regen
{
    [HarmonyPatch(typeof(MovementComponent), "UpdateMovement", null)]
    internal class PatchMovementComponent
    {
        [HarmonyPrefix]
        public static bool Prefix(MovementComponent __instance)
        {
            if (!__instance.wgo.is_player || __instance.player_controlled_by_script || __instance.wgo.is_dead)
                return true;
            Config.IsStandingStill = __instance.wgo.GetParam("speed_buff") <= 0.0;
            return true;
        }
    }
}