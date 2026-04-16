namespace ThoughtfulReminders;

public static class Helpers
{
    internal static void SayMessage(string msg)
    {
        if (GJL.IsEastern())
        {
            if (Plugin.DebugEnabled) Log($"[SayMessage] eastern-language speech bubble: \"{msg}\"");
            MainGame.me.player.Say(msg, null, false, SpeechBubbleGUI.SpeechBubbleType.Think,
                SmartSpeechEngine.VoiceID.None, true);
        }
        else if (Plugin.SpeechBubblesConfig.Value)
        {
            if (Plugin.DebugEnabled) Log($"[SayMessage] speech bubble on: \"{msg}\"");
            MainGame.me.player.Say(msg, null, false, SpeechBubbleGUI.SpeechBubbleType.Think,
                SmartSpeechEngine.VoiceID.None, true);
        }
        else
        {
            if (Plugin.DebugEnabled) Log($"[SayMessage] speech bubble off, floating label: \"{msg}\"");
            var pos = MainGame.me.player.pos3;
            pos.y += 125f;
            EffectBubblesManager.ShowImmediately(pos, msg,
                EffectBubblesManager.BubbleColor.Red,
                true, 4f);
        }
    }

    internal static void ShowDebugWarningOnce()
    {
        if (!Plugin.DebugEnabled || Plugin.DebugDialogShown) return;
        Plugin.DebugDialogShown = true;
        Lang.Reload();
        GUIElements.me.dialog.OpenOK("Thoughtful Reminders", null, Lang.Get("DebugWarning"), true, string.Empty);
    }

    internal static void Log(string message, bool error = false)
    {
        if (error)
        {
            LogHelper.Error(message);
        }
        else
        {
            LogHelper.Info(message);
        }
    }
}
