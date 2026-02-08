namespace PrayTheDayAway;

public partial class Plugin
{
    private static CultureInfo GameCulture =>
        CultureInfo.GetCultureInfo(GameSettings.me.language.Replace('_', '-').ToLower(CultureInfo.InvariantCulture).Trim());

    private static string GetLocalizedString(string content)
    {
        Thread.CurrentThread.CurrentUICulture = GameCulture;
        return content;
    }
    private static void WriteLog(string message, bool error = false)
    {
        if (error)
        {
            Log.LogError($"{message}");
        }
        else
        {
            if (Debug.Value)
            {
                Log.LogInfo($"{message}");
            }
        }
    }
}