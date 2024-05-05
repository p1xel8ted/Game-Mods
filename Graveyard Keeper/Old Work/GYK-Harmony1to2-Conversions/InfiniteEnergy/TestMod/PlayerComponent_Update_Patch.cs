using HarmonyLib;
using UnityEngine;

namespace TestMod
{
    [HarmonyPatch(typeof(PlayerComponent))]
    [HarmonyPatch("Update")]
    internal class PlayerComponentUpdatePatch
    {
        private static bool _infiniteEnabled = true;

        [HarmonyPrefix]
        public static bool Prefix()
        {
            if (Input.GetKey((KeyCode)108))
                _infiniteEnabled = !_infiniteEnabled;
            if (_infiniteEnabled)
                MainGame.me.player.energy = MainGame.me.save.max_energy - 1f;
            return true;
        }
    }
}