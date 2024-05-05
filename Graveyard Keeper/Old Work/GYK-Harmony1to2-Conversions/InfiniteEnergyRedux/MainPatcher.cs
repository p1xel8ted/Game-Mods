using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;

namespace com.deathpax.mods.GraveyardKeeper.InfiniteEnergyRedux
{
    public class MainPatcher
    {
        public static void Patch()
        {
            try
            {
                var harmony = new Harmony("com.deathpax.mods.graveyardkeeper.infiniteenergyredux");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Debug.LogError($"[InfiniteEnergyRedux (Harmony2)]: {ex.Message}, {ex.Source}, {ex.StackTrace}");
            }
        }
    }
}