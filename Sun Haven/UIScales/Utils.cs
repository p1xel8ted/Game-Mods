namespace UIScales;

public static class Utils
{

    internal static void UpdateUiScale(bool isMainMenu)
    {
        float scaleAdjustment = 0;

        if (Plugin.UIKeyboardShortcutIncrease.Value.IsUp())
        {
            scaleAdjustment = 0.25f;
        }
        else if (Plugin.UIKeyboardShortcutDecrease.Value.IsUp())
        {
            scaleAdjustment = -0.25f;
        }

        if (scaleAdjustment != 0)
        {
            if (isMainMenu)
            {
                Plugin.MainMenuUiScale.Value += scaleAdjustment;
                Plugin.MainMenuUiScale.Value = Mathf.Max(Mathf.Round(Plugin.MainMenuUiScale.Value / 0.25f) * 0.25f, 0.5f);
            }

            Plugin.MainHudScale.Value += scaleAdjustment;
            Plugin.MainHudScale.Value = Mathf.Max(Mathf.Round(Plugin.MainHudScale.Value / 0.25f) * 0.25f, 0.5f);

            if (Plugin.Notifications.Value && NotificationStack.Instance is not null)
            {
                SingletonBehaviour<NotificationStack>.Instance.SendNotification("UI Scale: " + Plugin.MainHudScale.Value);
            }
        }
    }

    internal static void UpdateZoomLevel()
    {
        if (Plugin.ZoomKeyboardShortcutIncrease.Value.IsUp() || Plugin.ZoomKeyboardShortcutDecrease.Value.IsUp())
        {
            // ZoomNeedsUpdating = true;
            var zoomAdjustment = Plugin.ZoomKeyboardShortcutIncrease.Value.IsUp() ? 0.25f : -0.25f;
            Plugin.ZoomLevel.Value += zoomAdjustment;
            Plugin.ZoomLevel.Value = Mathf.Max(Mathf.Round(Plugin.ZoomLevel.Value / 0.25f) * 0.25f, 0.5f);

            if (Player.Instance is not null)
            {
                Player.Instance.OverrideCameraZoomLevel = false;
                Player.Instance.SetZoom(Plugin.ZoomLevel.Value, true);
            }

            if (Plugin.Notifications.Value && NotificationStack.Instance is not null)
            {
                SingletonBehaviour<NotificationStack>.Instance.SendNotification("Zoom Level: " + Plugin.ZoomLevel.Value);
            }
        }
        // }
        // internal static void UpdateCanvasScaleFactors()
        // {
        //     if (!Plugin.ScaleAdjustments.Value) return;
        //     Shared.Utils.ConfigureCanvasScaler(Plugin.MainMenuCanvas, CanvasScaler.ScaleMode.ConstantPixelSize, Plugin.MainMenuUiScale.Value);
        //     Shared.Utils.ConfigureCanvasScaler(Plugin.UIOneCanvas, CanvasScaler.ScaleMode.ConstantPixelSize, Plugin.InGameUiScale.Value);
        //     Shared.Utils.ConfigureCanvasScaler(Plugin.UITwoCanvas, CanvasScaler.ScaleMode.ConstantPixelSize, Plugin.InGameUiScale.Value);
        //     Shared.Utils.ConfigureCanvasScaler(Plugin.QuantumCanvas, CanvasScaler.ScaleMode.ConstantPixelSize, Plugin.CheatConsoleScale.Value);
        // }
        //
        // internal static void ResetCanvasScaleFactors()
        // {
        //     if (Plugin.ScaleAdjustments.Value) return;
        //     Shared.Utils.ConfigureCanvasScaler(Plugin.MainMenuCanvas, CanvasScaler.ScaleMode.ScaleWithScreenSize, 3);
        //     Shared.Utils.ConfigureCanvasScaler(Plugin.UIOneCanvas, CanvasScaler.ScaleMode.ScaleWithScreenSize, 2);
        //     Shared.Utils.ConfigureCanvasScaler(Plugin.UITwoCanvas, CanvasScaler.ScaleMode.ScaleWithScreenSize, 2);
        //     Shared.Utils.ConfigureCanvasScaler(Plugin.QuantumCanvas, CanvasScaler.ScaleMode.ScaleWithScreenSize, 2);
        // }
    }
}