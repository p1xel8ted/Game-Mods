using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;

namespace LifeEnergy_Regen
{
    public class Main
    {
        public static void Patch()
        {
            try
            {
                var harmony = new Harmony("com.eurion.lifeenergyregen");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Debug.LogError($"[LifeEnergy_Regen (Harmony2)]: {ex.Message}, {ex.Source}, {ex.StackTrace}");
            }
        }
    }
}