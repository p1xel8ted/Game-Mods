using HarmonyLib;
using UnityEngine;

namespace LifeEnergy_Regen
{
    [HarmonyPatch(typeof(EnvironmentEngine), "Update", null)]
    internal class PatchEnvironmentComponent
    {
        [HarmonyPrefix]
        public static bool Prefix(EnvironmentEngine __instance)
        {
            if (MainGame.game_starting || MainGame.paused || !MainGame.game_started || __instance.IsTimeStopped())
            {
                Config.Load();
                return true;
            }

            if (Input.GetKeyUp(Config.ReloadKey))
            {
                Config.Load();
                EffectBubblesManager.ShowImmediately(MainGame.me.player.pos3, "Config has been reloaded. ",
                    EffectBubblesManager.BubbleColor.Relation);
            }

            return true;
        }
    }
}