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

        if (MainGame.me.player.is_dead)
        {
            if (Plugin.DebugEnabled && PendingReminder)
            {
                Helpers.Log($"[Update] reminder suppressed — player is dead (day {newDayOfWeek}).");
            }
            return;
        }

        if (!Application.isFocused) return;

        if (PrevDayOfWeek != newDayOfWeek)
        {
            if (Plugin.DebugEnabled)
            {
                Helpers.Log($"[Update] day changed {PrevDayOfWeek} → {newDayOfWeek} — queueing reminder.");
            }
            PendingReminder = true;
        }

        if (!PendingReminder) return;

        if (MainGame.me.player.components.character.player_controlled_by_script)
        {
            if (Plugin.DebugEnabled)
            {
                Helpers.Log($"[Update] reminder held back — player is scripted (cutscene/dialog). Will retry next frame.");
            }
            return;
        }
        if (EnvironmentEngine.me.IsTimeStopped())
        {
            if (Plugin.DebugEnabled)
            {
                Helpers.Log($"[Update] reminder held back — time is stopped (paused/menu). Will retry next frame.");
            }
            return;
        }

        Lang.Reload();

        var daysOnly = Plugin.DaysOnlyConfig.Value || !Plugin.EnableEventMessages.Value;
        if (Plugin.DebugEnabled)
        {
            Helpers.Log($"[Update] firing reminder — day={newDayOfWeek}, daysOnly={daysOnly} (DaysOnlyConfig={Plugin.DaysOnlyConfig.Value}, EnableEventMessages={Plugin.EnableEventMessages.Value}).");
        }

        if (daysOnly)
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
                    if (Plugin.DebugEnabled)
                    {
                        Helpers.Log($"[Update] unexpected day_of_week {newDayOfWeek} — falling back to 'default' translation key.");
                    }
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
                    var hasPreacher = MainGame.me.save.unlocked_perks.Contains("p_preacher");
                    if (Plugin.DebugEnabled)
                    {
                        Helpers.Log($"[Update] Pride day — preacher perk={hasPreacher}, picking {(hasPreacher ? "dhPrideSermon" : "dhPride")}.");
                    }
                    Helpers.SayMessage(hasPreacher
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
                    if (Plugin.DebugEnabled)
                    {
                        Helpers.Log($"[Update] unexpected day_of_week {newDayOfWeek} — falling back to 'default' translation key.");
                    }
                    Helpers.SayMessage(Lang.Get("default"));
                    break;
            }
        }

        PrevDayOfWeek = newDayOfWeek;
        PendingReminder = false;

        if (Plugin.DebugEnabled)
        {
            Helpers.Log($"[Update] reminder delivered — PrevDayOfWeek={PrevDayOfWeek}, queue cleared.");
        }
    }
}
