using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;

namespace GKInflationMod
{
    internal class MainPatcher
    {
        public static void Patch()
        {
            try
            {
                var harmony = new Harmony("com.fluffiest.graveyardkeeper.inflation.mod");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Debug.LogError($"[GKInflationMod (Harmony2)]: {ex.Message}, {ex.Source}, {ex.StackTrace}"); ;
            }
        }
    }
}