using System.Linq;
using UnityEngine.UI;

namespace Wanderful;

public static class Utils
{
    internal static void WriteLog(string message)
    {
       
            Plugin.Logger.LogInfo(message);
        
    }

    internal static float GetNewScale(float reference)
    {
        var displayHeight = Display.main.systemHeight;
        var scale = 1f / (reference / displayHeight);
        return scale;
    }

    internal static void UpdateScalers()
    {
        Resources.FindObjectsOfTypeAll<CanvasScaler>().ToList().ForEach(a =>
        {
            if (a.name.ToLowerInvariant().Contains("sinai")) return;
            a.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
            a.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
            var newScale = GetNewScale(a.referenceResolution.y);
            a.scaleFactor = newScale;
        });
    }

    internal static int FindLowestFrameRateMultipleAboveFifty(int originalRate)
    {
        // Start from half of the original rate and decrement by one to find the highest multiple above 50.
        for (var rate = originalRate / 2; rate > 50; rate--)
        {
            if (originalRate % rate == 0)
            {
                return rate;
            }
        }

        // Fallback, though this scenario is unlikely with standard monitor refresh rates
        return originalRate;
    }

}