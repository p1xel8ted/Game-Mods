using System.Threading;
using GYKHelper;
using UnityEngine;

namespace ThoughtfulReminders;

public static class Helpers
{
    internal static string GetLocalizedString(string content)
    {
        Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;
        return content;
    }

    internal static void SayMessage(string msg)
    {
        if (Plugin.SpeechBubblesConfig.Value)
        {
            Tools.ShowMessage(GetLocalizedString(msg), Vector3.zero, sayAsPlayer: true);
        }
        else
        {
            Tools.ShowMessage(GetLocalizedString(msg), Vector3.zero, sayAsPlayer: false, color: EffectBubblesManager.BubbleColor.Red, time: 4f);
        }
    }
}