using System.Net.Mime;
using System.Reflection;
using System.Threading;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using GYKHelper;
using HarmonyLib;
using ThoughtfulReminders.lang;
using UnityEngine;

namespace ThoughtfulReminders;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("p1xel8ted.gyk.gykhelper", "3.0.5")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.thoughtfulreminders";
    private const string PluginName = "Thoughtful Reminders";
    private const string PluginVer = "2.2.6";

    private static int PrevDayOfWeek { get; set; }
    
    internal static ConfigEntry<bool> SpeechBubblesConfig { get; private set; }
    private static ConfigEntry<bool> DaysOnlyConfig { get; set; }

    internal static ManualLogSource LOG { get; private set; }

    private void Awake()
    {
        LOG = Logger;
        SpeechBubblesConfig = Config.Bind("01. General", "Speech Bubbles", true, new ConfigDescription("Enable or disable speech bubbles", null, new ConfigurationManagerAttributes {Order = 2}));
        DaysOnlyConfig = Config.Bind("01. General", "Days Only", false, new ConfigDescription("Enable or disable days only mode", null, new ConfigurationManagerAttributes {Order = 1}));
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        LOG.LogInfo($"Plugin {PluginName} is loaded!");
    }

    private void Update()
    {

        if (!MainGame.game_started)
        {
            return;
        }
        var newDayOfWeek = MainGame.me.save.day_of_week;
        if (MainGame.me.player.is_dead)
        {
            return;
        }

        if (!Application.isFocused)
        {
            return;
        }

        if (PrevDayOfWeek == newDayOfWeek)
        {
            return;
        }

        if (CrossModFields.TimeOfDayFloat is <= 0.22f or >= 0.25f) return;
        if (DaysOnlyConfig.Value)
        {
            switch (newDayOfWeek)
            {
                case 0: //day of Sloth
                    Helpers.SayMessage(Helpers.GetLocalizedString(strings.dSloth));
                    break;

                case 1: //day of Pride
                    Helpers.SayMessage(Helpers.GetLocalizedString(strings.dPride));
                    break;

                case 2: //day of Lust
                    Helpers.SayMessage(Helpers.GetLocalizedString(strings.dLust));
                    break;

                case 3: //day of Gluttony
                    Helpers.SayMessage(Helpers.GetLocalizedString(strings.dGluttony));
                    break;

                case 4: //day of Envy
                    Helpers.SayMessage(Helpers.GetLocalizedString(strings.dEnvy));
                    break;

                case 5: //day of Wrath
                    Helpers.SayMessage(Helpers.GetLocalizedString(strings.dWrath));
                    break;

                default:
                    Helpers.SayMessage(Helpers.GetLocalizedString(strings._default));
                    break;
            }
        }
        else
        {
            Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;
            switch (newDayOfWeek)
            {
                case 0: //day of Sloth
                    Helpers.SayMessage(Helpers.GetLocalizedString(strings.dhSloth));
                    break;

                case 1: //day of Pride
                    Helpers.SayMessage(MainGame.me.save.unlocked_perks.Contains("p_preacher")
                        ? Helpers.GetLocalizedString(strings.dhPrideSermon)
                        : Helpers.GetLocalizedString(strings.dhPride));
                    break;

                case 2: //day of Lust
                    Helpers.SayMessage(Helpers.GetLocalizedString(strings.dhLust));
                    break;

                case 3: //day of Gluttony
                    Helpers.SayMessage(Helpers.GetLocalizedString(strings.dhGluttony));
                    break;

                case 4: //day of Envy
                    Helpers.SayMessage(Helpers.GetLocalizedString(strings.dhEnvy));
                    break;

                case 5: //day of Wrath
                    Helpers.SayMessage(Helpers.GetLocalizedString(strings.dhWrath));
                    break;

                default:
                    Helpers.SayMessage(Helpers.GetLocalizedString(strings._default));
                    break;
            }
        }

        PrevDayOfWeek = newDayOfWeek;
    }
}