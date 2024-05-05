using Il2CppSystem;
using UnityEngine.UI;
using Type = System.Type;

namespace Wanderful;

[Harmony]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class Patches
{
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(TimeManager), nameof(TimeManager.Initialize))]
    public static void TimeManager_Initialize(ref TimeManager __instance)
    {
        __instance.CurrentDay = 0;
        Plugin.Logger.LogWarning("TimeManager Initialize");
       
    }
    
    // [HarmonyPostfix]
    // [HarmonyPatch(typeof(TimeManager), nameof(TimeManager.Backward))]
    // public static void TimeManager_Backward(ref TimeManager __instance)
    // {
    //   
    //     __instance.CurrentDay = 0;
    //     Plugin.Logger.LogWarning("TimeManager Backward");
    //    
    // }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(TimeManager), nameof(TimeManager.Forward), new Type[]{})]
    public static void TimeManager_Forward_One(ref TimeManager __instance)
    {
        __instance.CurrentDay = 0;
        Plugin.Logger.LogWarning("TimeManager Forward");
       
    }
    [HarmonyPostfix]
    [HarmonyPatch(typeof(TimeManager), nameof(TimeManager.Forward), typeof(GridPosition3D))]
    public static void TimeManager_Forward(ref TimeManager __instance)
    {
        __instance.CurrentDay = 0;
        Plugin.Logger.LogWarning("TimeManager Forward GridPosition3D");
       
    }

    
    // [HarmonyPrefix]
    // [HarmonyPatch(typeof(ScoreManager), nameof(ScoreManager.ChangeScore))]
    // [HarmonyPatch(typeof(ScoreManager), nameof(ScoreManager.SetScore))]
    // public static void ScoreManager_ChangeScore(ref ScoreManager __instance)
    // {
    //     __instance.daysPassed = 0;
    //     Plugin.Logger.LogWarning("ScoreManager OnDayPassed");
    //     
    // }
    //
    
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(GUIDemo), nameof(GUIDemo.OnDayPassed))]
    public static void GUIDemo_OnDayPassed(ref GUIDemo __instance)
    {
        __instance._sessionService.Session.TimeManager.CurrentDay = 0;
        __instance._sessionService.Session.TimeManager._CurrentDay_k__BackingField = 0;
        Plugin.Logger.LogWarning("GUIDemo OnDayPassed");
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Session), nameof(Session.End))]
    public static void Session_End(ref Session __instance)
    {
        Plugin.Logger.LogWarning("Session End");
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(GUIDemo), nameof(GUIDemo.Start))]
    public static void GUIDemo_Start(GUIDemo __instance)
    {
        ConfigureGUIDemo(__instance);
        Plugin.Logger.LogWarning("GUIDemo Start");
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GUIDemo), nameof(GUIDemo.OnSessionStart))]
    public static void GUIDemo_OnSessionStart(GUIDemo __instance)
    {
        ConfigureGUIDemo(__instance);
        Plugin.Logger.LogWarning("GUIDemo OnSessionStart");
    }

    private static void ConfigureGUIDemo(GUIDemo instance)
    {
        instance.maxDays = int.MaxValue;
        instance.active = false;
        instance.daysElement.SetActive(false);
        instance.daysLabel.gameObject.SetActive(false);
    }
    
    private const string Sinai = "sinai";
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(CanvasScaler), nameof(CanvasScaler.OnEnable))]
    public static void CanvasScaler_OnEnable(ref CanvasScaler __instance)
    {
        if (__instance.name.ToLowerInvariant().Contains(Sinai)) return;
        __instance.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
        __instance.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
        var newScale = Utils.GetNewScale(__instance.referenceResolution.y);
        __instance.scaleFactor = newScale;
    }
    
}