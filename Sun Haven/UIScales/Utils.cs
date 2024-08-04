namespace UIScales;

public static class Utils
{
    internal static void UpdateZoomLevel()
    {
        if (Plugin.ZoomKeyboardShortcutIncrease.Value.IsUp() || Plugin.ZoomKeyboardShortcutDecrease.Value.IsUp())
        {
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
    }
}