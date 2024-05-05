using HarmonyLib;
using Helper;
using System;
using System.Reflection;
using UnityEngine;

namespace ThoughtfulReminders;

[HarmonyPatch]
public static partial class MainPatcher
{
    private static int _prevDayOfWeek;
    private static Config.Options _cfg;

    public static void Patch()
    {
        try
        {
            var harmony = new Harmony("p1xel8ted.GraveyardKeeper.ThoughtfulReminders");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            _cfg = Config.GetOptions();
        }
        catch (Exception ex)
        {
            Log($"{ex.Message}, {ex.Source}, {ex.StackTrace}", true);
        }
    }

    private static void SayMessage(string msg)
    {
        if (_cfg.speechBubbles)
        {
            Tools.ShowMessage(GetLocalizedString(msg), Vector3.zero, sayAsPlayer: true);
        }
        else
        {
            Tools.ShowMessage(GetLocalizedString(msg), Vector3.zero, sayAsPlayer: false, color: EffectBubblesManager.BubbleColor.Red, time: 4f);
        }
    }
}