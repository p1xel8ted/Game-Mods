using System.Globalization;
using System.Threading;
using UnityEngine;

namespace ThoughtfulReminders;

public static class Helpers
{
    private static CultureInfo GameCulture =>
        CultureInfo.GetCultureInfo(GameSettings.me.language.Replace('_', '-').ToLower(CultureInfo.InvariantCulture).Trim());

    internal static void SetUICulture()
    {
        Thread.CurrentThread.CurrentUICulture = GameCulture;
    }

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
