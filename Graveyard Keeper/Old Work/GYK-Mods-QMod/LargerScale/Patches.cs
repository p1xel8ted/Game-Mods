using Com.LuisPedroFonseca.ProCamera2D;
using HarmonyLib;
using Helper;
using LargerScale.lang;
using UnityEngine;
using Tools = Helper.Tools;

namespace LargerScale;

[HarmonyPatch]
public static partial class MainPatcher
{
    [HarmonyPostfix]
    [HarmonyAfter("com.p1xel8ted.graveyardkeeper.UltraWide")]
    [HarmonyPatch(typeof(ResolutionConfig), nameof(ResolutionConfig.GetResolutionConfigOrNull))]
    public static void ResolutionConfig_GetResolutionConfigOrNull(int width, int height, ref ResolutionConfig __result)
    {
        var res = new ResolutionConfig(width, height);
        if (height < 900 || width < 1280)
        {
            res.large_gui_scale = height / 900f;
        }
        
        Log($"ResolutionConfig_GetResolutionConfigOrNull: Width: {width}, Height: {height}, Pixel Size: {res.pixel_size}"); 
        __result = res;
    }

    //hooks into the time of day update and saves if the K key was pressed
    [HarmonyPrefix]
    [HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.Update))]
    public static void TimeOfDay_Update()
    {
        if (Input.GetKeyUp(_cfg.ReloadConfigKeyBind))
        {
            _cfg = Config.GetOptions();

            if (Time.time - CrossModFields.ConfigReloadTime > CrossModFields.ConfigReloadTimeLength)
            {
                Tools.ShowMessage(GetLocalizedString(strings.ConfigMessage), Vector3.zero);
                CrossModFields.ConfigReloadTime = Time.time;
            }
        }
    }
    
    // [HarmonyPrefix]
    // [HarmonyPatch(typeof(MainGame), nameof(MainGame.OnScreenSizeChanged))]
    // public static bool MainGame_OnScreenSizeChanged(ref MainGame __instance, int w = -1, int h = -1)
    // {
    //     if (w == -1)
    //     {
    //         w = Screen.width;
    //     }
    //     if (h == -1)
    //     {
    //         h = Screen.height;
    //     }
    //     var vector = new Vector2(w, h);
    //     Log($"OnScreenSizeChanged {vector}");
    //     if (GameSettings.current_resolution == null)
    //     {
    //         Log($"Unsupported resolution {w}x{h}!");
    //         GameSettings.current_resolution = new ResolutionConfig(w, h);
    //     }
    //     var vector2 = vector * 2f / GameSettings.current_resolution.pixel_size;
    //     Log($"Setting camera size: {vector2} for pixel size: {_cfg.PixelSize}");
    //     __instance.pro_camera.GetComponent<ProCamera2DPixelPerfect>().ViewportAutoScale = AutoScaleMode.None;
    //     var gameCamera = __instance.GetComponent<Camera>();
    //     gameCamera.orthographicSize = h / _cfg.PixelSize;
    //     __instance.ui_root.manualHeight = (int)vector.y / __instance.gui_pixel_zoom;
    //     if (GUIElements.me != null)
    //     {
    //         GUIElements.me.RecalcScreenResolution(w, h);
    //     }
    //     ChunkManager.RecalculateResolution(w, h);
    //     return false;
    // }
}