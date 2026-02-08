using System.Globalization;
using System.Threading;
using BepInEx;
using BepInEx.Logging;
using ThoughtfulReminders.lang;
using UnityEngine;

namespace ThoughtfulReminders;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.thoughtfulreminders";
    private const string PluginName = "Thoughtful Reminders";
    private const string PluginVer = "2.2.8";

    private static int PrevDayOfWeek { get; set; }

    internal static ConfigEntry<bool> SpeechBubblesConfig { get; private set; }
    private static ConfigEntry<bool> DaysOnlyConfig { get; set; }

    internal static ManualLogSource LOG { get; private set; }

    private void Awake()
    {
        LOG = Logger;
        SpeechBubblesConfig = Config.Bind("01. General", "Speech Bubbles", true, new ConfigDescription("Enable or disable speech bubbles", null, new ConfigurationManagerAttributes {Order = 2}));
        DaysOnlyConfig = Config.Bind("01. General", "Days Only", false, new ConfigDescription("Enable or disable days only mode", null, new ConfigurationManagerAttributes {Order = 1}));
        StartupLogger.PrintModLoaded(PluginName, LOG);
    }

    private void Update()
    {
        if (!MainGame.game_started) return;

        var newDayOfWeek = MainGame.me.save.day_of_week;

        if (MainGame.me.player.is_dead) return;

        if (!Application.isFocused) return;

        if (PrevDayOfWeek == newDayOfWeek) return;

        var timeOfDay = TimeOfDay.me == null ? 0f : TimeOfDay.me.GetTimeK();
        if (timeOfDay is <= 0.22f or >= 0.25f) return;

        Helpers.SetUICulture();

        if (DaysOnlyConfig.Value)
        {
            switch (newDayOfWeek)
            {
                case 0:
                    Helpers.SayMessage(strings.dSloth);
                    break;

                case 1:
                    Helpers.SayMessage(strings.dPride);
                    break;

                case 2:
                    Helpers.SayMessage(strings.dLust);
                    break;

                case 3:
                    Helpers.SayMessage(strings.dGluttony);
                    break;

                case 4:
                    Helpers.SayMessage(strings.dEnvy);
                    break;

                case 5:
                    Helpers.SayMessage(strings.dWrath);
                    break;

                default:
                    Helpers.SayMessage(strings._default);
                    break;
            }
        }
        else
        {
            switch (newDayOfWeek)
            {
                case 0:
                    Helpers.SayMessage(strings.dhSloth);
                    break;

                case 1:
                    Helpers.SayMessage(MainGame.me.save.unlocked_perks.Contains("p_preacher")
                        ? strings.dhPrideSermon
                        : strings.dhPride);
                    break;

                case 2:
                    Helpers.SayMessage(strings.dhLust);
                    break;

                case 3:
                    Helpers.SayMessage(strings.dhGluttony);
                    break;

                case 4:
                    Helpers.SayMessage(strings.dhEnvy);
                    break;

                case 5:
                    Helpers.SayMessage(strings.dhWrath);
                    break;

                default:
                    Helpers.SayMessage(strings._default);
                    break;
            }
        }

        PrevDayOfWeek = newDayOfWeek;
    }
}
