using System.Threading;
using HarmonyLib;
using Helper;
using ThoughtfulReminders.lang;
using UnityEngine;

namespace ThoughtfulReminders;

[HarmonyPatch]
public static partial class MainPatcher
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainGame), nameof(MainGame.Update))]
    public static void MainGame_Update()
    {
        if (!MainGame.game_started) return;
        var newDayOfWeek = MainGame.me.save.day_of_week;
        if (MainGame.me.player.is_dead) return;

        if (!Application.isFocused) return;

        if (_prevDayOfWeek == newDayOfWeek) return;

        if (CrossModFields.TimeOfDayFloat is <= 0.22f or >= 0.25f) return;

        if (_cfg.daysOnly)
        {
            switch (newDayOfWeek)
            {
                case 0: //day of Sloth
                    SayMessage(GetLocalizedString(strings.dSloth));
                    break;

                case 1: //day of Pride
                    SayMessage(GetLocalizedString(strings.dPride));
                    break;

                case 2: //day of Lust
                    SayMessage(GetLocalizedString(strings.dLust));
                    break;

                case 3: //day of Gluttony
                    SayMessage(GetLocalizedString(strings.dGluttony));
                    break;

                case 4: //day of Envy
                    SayMessage(GetLocalizedString(strings.dEnvy));
                    break;

                case 5: //day of Wrath
                    SayMessage(GetLocalizedString(strings.dWrath));
                    break;

                default:
                    SayMessage(GetLocalizedString(strings._default));
                    break;
            }
        }
        else
        {
            Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;
            switch (newDayOfWeek)
            {
                case 0: //day of Sloth
                    SayMessage(GetLocalizedString(strings.dhSloth));
                    break;

                case 1: //day of Pride
                    SayMessage(MainGame.me.save.unlocked_perks.Contains("p_preacher")
                        ? GetLocalizedString(strings.dhPrideSermon)
                        : GetLocalizedString(strings.dhPride));
                    break;

                case 2: //day of Lust
                    SayMessage(GetLocalizedString(strings.dhLust));
                    break;

                case 3: //day of Gluttony
                    SayMessage(GetLocalizedString(strings.dhGluttony));
                    break;

                case 4: //day of Envy
                    SayMessage(GetLocalizedString(strings.dhEnvy));
                    break;

                case 5: //day of Wrath
                    SayMessage(GetLocalizedString(strings.dhWrath));
                    break;

                default:
                    SayMessage(GetLocalizedString(strings._default));
                    break;
            }
        }

        _prevDayOfWeek = newDayOfWeek;
    }
}