using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;

namespace GKInflationModReverse
{
    internal class MainPatcher
    {
        public static void Patch()
        {
            try
            {
                var harmony = new Harmony("com.ithinkandicode.graveyardkeeper.inflationreverse.mod");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Debug.LogError($"[GKInflationModReverse (Harmony2)]: {ex.Message}, {ex.Source}, {ex.StackTrace}");
            }
        }
    }
}