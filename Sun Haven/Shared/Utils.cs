using UnityEngine;
using UnityEngine.UI;
using Wish;

namespace Shared;

public static class Utils
{
    public static float BaseAspect => 16f / 9f;
    public static float CurrentAspect => (float) Screen.width / Screen.height;
    public static bool LargerAspect => CurrentAspect > BaseAspect;
    public static float PositiveScaleFactor => CurrentAspect / BaseAspect;
    public static float NegativeScaleFactor => 1f / PositiveScaleFactor;


    public static bool CanUse(ToolData toolData)
    {
        return SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.Professions[toolData.profession].level >= toolData.requiredLevel;
    }
    
    public static void DestroyChildren(Transform transform)
    {
        foreach (Transform child in transform)
        {
            UnityEngine.Object.Destroy(child.gameObject);
        }
    }

    public static void ConfigureCanvasScaler(CanvasScaler canvasScaler, CanvasScaler.ScaleMode scaleMode, float scaleFactor)
    {
        if (canvasScaler is null)
        {
            // Plugin.LOG.LogWarning($"ConfigureCanvasScaler: canvasScaler is null!");
            return;
        }

        canvasScaler.uiScaleMode = scaleMode;
        canvasScaler.scaleFactor = scaleFactor;
    }
    
    public static void SendNotification(string message)
    {
        if (NotificationStack.Instance is not null)
        {
            SingletonBehaviour<NotificationStack>.Instance.SendNotification(message);
        }
    }

}