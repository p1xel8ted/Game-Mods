namespace RestInPatches.Patches;

// HUD.Update runs every frame. The original reassigns UILabel.text for day/r/g/b/hint_x
// using freshly-allocated strings from "day " + N.ToString() and N.ToString() — even when
// the underlying int hasn't changed. NGUI's UILabel setter short-circuits on content
// equality, but the string allocation happens before the setter is called, so the garbage
// is already produced.
//
// This patch fully replaces HUD.Update: it tracks the last-seen values in a per-HUD
// side table and only rebuilds the label text when the value actually changed. The
// bar_hp / bar_energy / bar_sanity / RedrawTime calls are preserved verbatim so the
// HP/energy/sanity sliders and the clock still tick every frame.
//
// Note: ResiliencePatches already attaches a [HarmonyFinalizer] to HUD.Update for
// IndexOutOfRangeException swallowing — finalizers still run after a Prefix that returns
// false, so that safety net is preserved.
[Harmony]
public static class HudStringPatches
{
    private sealed class State
    {
        public int Day = int.MinValue;
        public int R = int.MinValue;
        public int G = int.MinValue;
        public int B = int.MinValue;
        public bool HintInit;
        public bool HintHasOverhead;
    }

    private static readonly ConditionalWeakTable<HUD, State> States = new();

    [HarmonyPrefix]
    [HarmonyPatch(typeof(HUD), nameof(HUD.Update))]
    public static bool HUD_Update_Prefix(HUD __instance)
    {
        if (!__instance._inited || !MainGame.game_started)
        {
            return false;
        }

        var save = MainGame.me.save;
        var player = MainGame.me.player;

        __instance.bar_hp.value = save.GetHPPercentage();
        __instance.bar_energy.value = player.energy / (float)save.max_energy;
        __instance.bar_sanity.value = player.sanity / (float)save.max_sanity;

        var state = States.GetOrCreateValue(__instance);

        var day = save.day;
        if (day != state.Day)
        {
            state.Day = day;
            __instance.day_label.text = "day " + day;
        }

        var r = Mathf.RoundToInt(player.GetParam("r"));
        if (r != state.R)
        {
            state.R = r;
            __instance.r_label.text = r.ToString();
        }

        var g = Mathf.RoundToInt(player.GetParam("g"));
        if (g != state.G)
        {
            state.G = g;
            __instance.g_label.text = g.ToString();
        }

        var b = Mathf.RoundToInt(player.GetParam("b"));
        if (b != state.B)
        {
            state.B = b;
            __instance.b_label.text = b.ToString();
        }

        if (__instance.hint_x != null)
        {
            var hasOverhead = MainGame.me.player_char.has_overhead;
            if (!state.HintInit || hasOverhead != state.HintHasOverhead)
            {
                state.HintInit = true;
                state.HintHasOverhead = hasOverhead;
                __instance.hint_x.text = !hasOverhead ? "(X) - attack/use tool" : "(X) - drop";
            }
        }

        __instance.RedrawTime(TimeOfDay.me.GetTimeK());
        return false;
    }
}
