using UnityEngine;
using UnityEngine.UI;

namespace HavenPark;

public static class Utils
{
    internal static float GetNewScale(float reference)
    {
        var displayHeight = Display.main.systemHeight;
        var scale = 1f / (reference / displayHeight);
        return scale;
    }

    internal static void UpdateCamera()
    {
        var fov = Plugin.ConfigFoV.Value;
        if (Camera.main != null)
        {
            Camera.main.fieldOfView = fov;
        }
    }

    internal static void UpdateScaler(CanvasScaler scaler)
    {
        if (scaler == null) return;
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
        scaler.scaleFactor = Utils.GetNewScale(scaler.referenceResolution.y) * Plugin.ConfigScale.Value;
    }
}