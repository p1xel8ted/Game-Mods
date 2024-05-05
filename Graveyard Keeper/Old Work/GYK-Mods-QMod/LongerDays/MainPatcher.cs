using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;

namespace LongerDays;

[HarmonyPatch]
public static partial class MainPatcher
{
    private const float MadnessSeconds = 1350f; //3 bodies in total
    private const float EvenLongerSeconds = 1125f; //2 bodies in total
    private const float DoubleLengthSeconds = 900f; //2 bodies in total
    private const float DefaultIncreaseSeconds = 675f; //1 body in total
    private static Config.Options _cfg;

    private static float _seconds;

    public static float GetTime()
    {
        var adj = GetTimeMulti();
        var time = Time.deltaTime;
        var newTime = time / adj;
        return newTime;
    }

    public static void Patch()
    {
        try
        {
            _cfg = Config.GetOptions();

            var harmony = new Harmony("p1xel8ted.GraveyardKeeper.LongerDays");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            RefreshConfig();
        }
        catch (Exception ex)
        {
            Log($"{ex.Message}, {ex.Source}, {ex.StackTrace}", true);
        }
    }

    private static void RefreshConfig()
    {
        if (_cfg.madness)
        {
            _seconds = MadnessSeconds;
            Log("Madness mode enabled: 1350 seconds.");
        }
        else if (_cfg.evenLongerDays)
        {
            _seconds = EvenLongerSeconds;
            Log("Even longer days enabled: 1125 seconds.");
        }
        else if (_cfg.doubleLengthDays)
        {
            _seconds = DoubleLengthSeconds;
            Log("Double length days enabled: 900 seconds.");
        }
        else
        {
            _seconds = DefaultIncreaseSeconds;
            Log("Default increase enabled: 675 seconds.");
        }
    }

    private static float GetTimeMulti()
    {
        var num = _seconds switch
        {
            DefaultIncreaseSeconds => 1.5f,
            DoubleLengthSeconds => 2f,
            EvenLongerSeconds => 2.5f,
            MadnessSeconds => 3f,
            _ => 1f
        };
        return num;
    }
}