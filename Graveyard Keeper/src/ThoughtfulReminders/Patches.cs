namespace ThoughtfulReminders;

[Harmony]
public static class Patches
{
    private static int PrevDayOfWeek { get; set; }
    private static bool PendingReminder { get; set; }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainGame), nameof(MainGame.Update))]
    public static void MainGame_Update()
    {
        if (!MainGame.game_started) return;

        var newDayOfWeek = MainGame.me.save.day_of_week;

        if (MainGame.me.player.is_dead) return;

        if (!Application.isFocused) return;

        if (PrevDayOfWeek != newDayOfWeek)
        {
            PendingReminder = true;
        }

        if (!PendingReminder) return;

        if (MainGame.me.player.components.character.player_controlled_by_script) return;
        if (EnvironmentEngine.me.IsTimeStopped()) return;

        Lang.Reload();

        if (Plugin.DaysOnlyConfig.Value || !Plugin.EnableEventMessages.Value)
        {
            switch (newDayOfWeek)
            {
                case 0:
                    Helpers.SayMessage(Lang.Get("dSloth"));
                    break;

                case 1:
                    Helpers.SayMessage(Lang.Get("dPride"));
                    break;

                case 2:
                    Helpers.SayMessage(Lang.Get("dLust"));
                    break;

                case 3:
                    Helpers.SayMessage(Lang.Get("dGluttony"));
                    break;

                case 4:
                    Helpers.SayMessage(Lang.Get("dEnvy"));
                    break;

                case 5:
                    Helpers.SayMessage(Lang.Get("dWrath"));
                    break;

                default:
                    Helpers.SayMessage(Lang.Get("default"));
                    break;
            }
        }
        else
        {
            switch (newDayOfWeek)
            {
                case 0:
                    Helpers.SayMessage(Lang.Get("dhSloth"));
                    break;

                case 1:
                    Helpers.SayMessage(MainGame.me.save.unlocked_perks.Contains("p_preacher")
                        ? Lang.Get("dhPrideSermon")
                        : Lang.Get("dhPride"));
                    break;

                case 2:
                    Helpers.SayMessage(Lang.Get("dhLust"));
                    break;

                case 3:
                    Helpers.SayMessage(Lang.Get("dhGluttony"));
                    break;

                case 4:
                    Helpers.SayMessage(Lang.Get("dhEnvy"));
                    break;

                case 5:
                    Helpers.SayMessage(Lang.Get("dhWrath"));
                    break;

                default:
                    Helpers.SayMessage(Lang.Get("default"));
                    break;
            }
        }

        PrevDayOfWeek = newDayOfWeek;
        PendingReminder = false;
    }
}
