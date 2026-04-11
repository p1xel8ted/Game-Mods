namespace MiscBitsAndBobs;

public class DestroyWhenInvisible : MonoBehaviour
{
    private void Start()
    {
        if (!GetComponent<SpriteRenderer>().isVisible)
        {
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible() => Destroy(gameObject);
}

public static class Helpers
{
    private static bool _sprintMsgShown;
    internal static bool Sprint;

    internal static void ActionsOnSpawnPlayer()
    {
        Plugin.Log.LogInfo($"Running MiscBitsAndBobs ActionsOnSpawnPlayer as Player has spawned in.");
        if (MainGame.game_started && !_sprintMsgShown && Sprint && Plugin.ModifyPlayerMovementSpeedConfig.Value)
        {
            Lang.Reload();
            GUIElements.me.dialog.OpenOK(Lang.Get("Title"), null, Lang.Get("Content"), true);
            _sprintMsgShown = true;
        }
    }
}