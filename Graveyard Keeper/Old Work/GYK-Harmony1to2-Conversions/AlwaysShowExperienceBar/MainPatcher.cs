using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;

namespace AlwaysShowExperienceBar
{
    public static class MainPatcher
    {
        public static FieldInfo FieldStayShownTime;

        public static void Patch()
        {
            try
            {
                var harmony = new Harmony("com.graveyardkeeper.urbanvibes.alwaysshowexperiencebar");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
                FieldStayShownTime = typeof(AnimatedGUIPanel).GetField("stay_shown_time", AccessTools.all);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[AlwaysShowExperienceBar (Harmony2)]: {ex.Message}, {ex.Source}, {ex.StackTrace}");
            }
        }
    }
}