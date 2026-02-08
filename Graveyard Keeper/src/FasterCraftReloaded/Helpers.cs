namespace FasterCraftReloaded;

public static class Helpers
{
    internal static void Log(string message, bool error = false)
    {
        if (error)
        {
            Plugin.Log.LogError($"{message}");
        }
        else
        {
            if (Plugin.Debug.Value)
            {
                Plugin.Log.LogInfo($"{message}");
            }
        }
    }
}