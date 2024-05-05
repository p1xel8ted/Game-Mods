using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;

namespace InfiniteEnergy
{
    public class MainPatcher
    {
        public static void Patch()
        {
            try
            {
                var harmony = new Harmony("com.glibfire.graveyardkeeper.infiniteenergy.mod");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Debug.LogError($"[GKSleepMod (Harmony2)]: {ex.Message}, {ex.Source}, {ex.StackTrace}");
            }
        }
    }
}