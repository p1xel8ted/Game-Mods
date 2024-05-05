using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;

namespace UltraWide;

public static partial class MainPatcher
{
    private static float _newValue;
    private static float _otherNewValue;

    public static void Patch()
    {
        try
        {
            _newValue = Screen.width > 3440 ? 72f : 48f;
            _otherNewValue = Screen.width > 3440 ? 12f : 8f;

            var harmony = new Harmony("p1xel8ted.GraveyardKeeper.UltraWide");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
        catch (Exception ex)
        {
            Log($"{ex.Message}, {ex.Source}, {ex.StackTrace}", true);
        }
    }
}