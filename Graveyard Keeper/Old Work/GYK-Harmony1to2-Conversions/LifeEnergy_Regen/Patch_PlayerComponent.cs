using HarmonyLib;
using System;
using UnityEngine;

namespace LifeEnergy_Regen
{
    [HarmonyPatch(typeof(PlayerComponent), "Update", null)]
    internal class PatchPlayerComponent
    {
        private static float _delay = Config.RegenDelay;

        [HarmonyPrefix]
        public static bool Prefix()
        {
            if (_delay == 0.0)
            {
                if (MainGame.me.player.energy < (double)MainGame.me.save.max_energy)
                    MainGame.me.player.energy += Math.Abs(Config.EnergyRegen);
                if (MainGame.me.player.hp < (double)MainGame.me.save.max_hp)
                    MainGame.me.player.hp += Math.Abs(Config.LifeRegen);
                _delay = Config.RegenDelay;
            }
            else
                _delay = _delay <= 0.0 ? 0.0f : _delay - Time.deltaTime;

            return true;
        }
    }
}