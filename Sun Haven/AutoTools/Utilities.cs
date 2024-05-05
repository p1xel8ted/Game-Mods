namespace AutoTools;

public static class Utilities
{
    private const float TimeBetweenNotifications = 5f;
    private static float LastNotificationTime { get; set; }
    private static string PreviousMessage { get; set; }

 

    internal static void Notify(string message, int id = 0, bool error = false)
    {
        if (message == PreviousMessage && Time.time - LastNotificationTime < TimeBetweenNotifications) return;
        LastNotificationTime = Time.time;
        PreviousMessage = message;
        SingletonBehaviour<NotificationStack>.Instance.SendNotification(message, id, 0, error);
    }

    internal static void SetActionBar(int index)
    {
        PlayerInput.AllowChangeActionBarItem = true;
        Player.Instance.PlayerInventory.SetActionBarSlot(index);
        Player.Instance.PlayerInventory.SetIndex(index);
    }

    internal static float GetDistance(EnemyAI enemyAI)
    {
        return !Player.Instance ? float.MaxValue : ((Vector2) enemyAI.transform.position - Player.Instance.ExactGraphicsPosition).magnitude;
    }

    internal static bool IsInFarmTile()
    {
        return TileManager.Instance.HasTileOrFarmingTile(Player.Instance.Position, ScenePortalManager.ActiveSceneIndex);
    }
}