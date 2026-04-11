namespace ThoughtfulReminders;

public static class Helpers
{
    internal static void SayMessage(string msg)
    {
        if (GJL.IsEastern())
        {
            MainGame.me.player.Say(msg, null, false, SpeechBubbleGUI.SpeechBubbleType.Think,
                SmartSpeechEngine.VoiceID.None, true);
        }
        else if (Plugin.SpeechBubblesConfig.Value)
        {
            MainGame.me.player.Say(msg, null, false, SpeechBubbleGUI.SpeechBubbleType.Think,
                SmartSpeechEngine.VoiceID.None, true);
        }
        else
        {
            var pos = MainGame.me.player.pos3;
            pos.y += 125f;
            EffectBubblesManager.ShowImmediately(pos, msg,
                EffectBubblesManager.BubbleColor.Red,
                true, 4f);
        }
    }
}
