using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GYKHelper;
using HarmonyLib;
using UnityEngine;

namespace MiscBitsAndBobs;

[HarmonyPatch]
public static class Patches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(SpeechBubbleGUI), nameof(SpeechBubbleGUI.SpeechText))]
    public static void SpeechBubbleGUI_SpeechText(ref string s, ref string __result)
    {
        if (!Plugin.OldEnglishThrowback.Value) return;
        if (!s.Equals("Our church is great!")) return;
        if (!CrossModFields.Lang.Equals("en")) return;
        __result = "Our church great!";
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(MovementComponent), nameof(MovementComponent.UpdateMovement), typeof(Vector2), typeof(float))]
    public static void MovementComponent_UpdateMovement(ref MovementComponent __instance)
    {
        if (IsMovementUpdateAllowed(__instance)) return;

        if (__instance.wgo.is_player)
        {
            UpdatePlayerMovementSpeed(__instance);
        }
        else if (__instance.wgo.IsWorker() && Tools.WorkerHasBackpack(__instance.wgo))
        {
            UpdatePorterMovementSpeed(__instance);
        }
    }

    private static bool IsMovementUpdateAllowed(MovementComponent instance)
    {
        return instance.wgo.is_dead || instance.player_controlled_by_script;
    }

    private static void UpdatePlayerMovementSpeed(MovementComponent instance)
    {
        var speed = instance.wgo.data.GetParam("speed");
        if (speed > 0)
        {
            speed = LazyConsts.PLAYER_SPEED + instance.wgo.data.GetParam("speed_buff");
        }

        if (Plugin.ModifyPlayerMovementSpeedConfig.Value)
        {
            instance.SetSpeed(speed * Plugin.PlayerMovementSpeedConfig.Value);
        }
        else
        {
            instance.SetSpeed(speed);
        }
    }

    private static void UpdatePorterMovementSpeed(MovementComponent instance)
    {
        if (Plugin.ModifyPorterMovementSpeedConfig.Value)
        {
            instance.SetSpeed(Plugin.PorterMovementSpeedConfig.Value);
        }
        else
        {
            instance.SetSpeed(0);
        }
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(CameraTools), nameof(CameraTools.TweenLetterbox))]
    public static void CameraTools_TweenLetterbox(ref bool show)
    {
        if (CrossModFields.GerryCinematicPlaying) return;
        if (show && !Plugin.CinematicLetterboxingConfig.Value)
        {
            show = false;
        }
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(Intro), nameof(Intro.ShowIntro))]
    public static void Intro_ShowIntro()
    {
        if (Intro.need_show_first_intro && Plugin.SkipIntroVideoOnNewGameConfig.Value)
        {
            Intro.need_show_first_intro = false;
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(CraftComponent), nameof(CraftComponent.End))]
    public static void CraftComponent_End(ref CraftComponent __instance)
    {
        if (!MainGame.game_started || __instance == null) return;

        if (!Plugin.KitsuneKitoModeConfig.Value) return;

        if (__instance.last_craft_id.Equals("set_grave_bot_wd_1"))
        {
            TechPointsDrop.Drop(MainGame.me.player.pos3, 0, 0, 1);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(GameGUI), nameof(GameGUI.Open))]
    public static void GameGUI_Open()
    {
        if (Plugin.QuietMusicInGuiConfig.Value)
        {
            SmartAudioEngine.me.SetDullMusicMode();
        }
    }


    [HarmonyPrefix]
    [HarmonyAfter("p1xel8ted.gyk.gykhelper")]
    [HarmonyPatch(typeof(MainMenuGUI), nameof(MainMenuGUI.Open))]
    public static void MainMenuGUI_Open_Prefix(ref MainMenuGUI __instance)
    {
        var enabled = Plugin.HideCreditsButtonOnMainMenuConfig.Value;
        if (__instance == null) return;

        foreach (var comp in __instance.GetComponentsInChildren<UIButton>()
                     .Where(x => x.name.Contains("credits")))
        {
            comp.SetState(enabled ? UIButtonColor.State.Normal : UIButtonColor.State.Disabled, true);
            comp.SetActive(!enabled);
        }
    }

    [HarmonyPostfix]
    [HarmonyAfter("pp1xel8ted.gyk.gykhelper")]
    [HarmonyPatch(typeof(MainMenuGUI), nameof(MainMenuGUI.Open))]
    public static void MainMenuGUI_Open_Postfix()
    {
        Helpers.SprintTools = Tools.ModLoaded("", "SprintReloaded.dll", "Sprint Reloaded");
        Helpers.SprintHarmony = Harmony.HasAnyPatches("mugen.GraveyardKeeper.SprintReloaded");
        Helpers.Sprint = Helpers.SprintTools || Helpers.SprintHarmony;

        Plugin.Log.LogInfo($"[MBB]: Sprint Detected via Tools: {Helpers.SprintTools}");

        Plugin.Log.LogInfo($"[MBB]: Sprint Detected via Harmony: {Helpers.SprintHarmony}");
    }

    [HarmonyFinalizer]
    [HarmonyPatch(typeof(HUD), nameof(HUD.Update))]
    public static Exception Finalizer(ref Exception __exception)
    {
        return __exception is IndexOutOfRangeException ? null : __exception;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(HUD), nameof(HUD.Update))]
    public static void HUD_Update(ref HUD __instance)
    {
        if (!__instance._inited || !MainGame.game_started || !Plugin.CondenseXpBarConfig.Value) return;

        var r = MainGame.me.player.GetParam("r");
        var g = MainGame.me.player.GetParam("g");
        var b = MainGame.me.player.GetParam("b");

        var red = ProcessColorValue(r, "(r)");
        var green = ProcessColorValue(g, "(g)");
        var blue = ProcessColorValue(b, "(b)");

        foreach (var comp in GUIElements.me.hud.tech_points_bar.GetComponentsInChildren<UILabel>())
            comp.text = $"{red} {green} {blue}";
    }

    private static string ProcessColorValue(float value, string prefix)
    {
        if (!(value >= 1000)) return $"{prefix}{value}";
        value /= 1000f;
        var nSplit = value.ToString(CultureInfo.InvariantCulture).Split('.');

        if (nSplit.Length > 1)
        {
            return nSplit[1].StartsWith("0") ? $"{prefix}{value:0}K" : $"{prefix}{value:0.0}K";
        }

        return $"{prefix}{value}";
    }


    // [HarmonyPostfix]
    // [HarmonyPatch(typeof(HUD), nameof(HUD.Update))]
    // public static void HUD_Update(ref HUD __instance)
    // {
    //     if (!__instance._inited || !MainGame.game_started || !Plugin.CondenseXpBarConfig.Value) return;
    //
    //     var r = MainGame.me.player.GetParam("r");
    //     var g = MainGame.me.player.GetParam("g");
    //     var b = MainGame.me.player.GetParam("b");
    //
    //     string red;
    //     if (r >= 1000)
    //     {
    //         r /= 1000f;
    //         var nSplit = r.ToString(CultureInfo.InvariantCulture).Split('.');
    //         red = nSplit[1].StartsWith("0") ? $"(r){r:0}K" : $"(r){r:0.0}K";
    //     }
    //     else
    //     {
    //         red = $"(r){r}";
    //     }
    //
    //     string green;
    //     if (g >= 1000)
    //     {
    //         g /= 1000f;
    //         var nSplit = g.ToString(CultureInfo.InvariantCulture).Split('.');
    //         green = nSplit[1].StartsWith("0") ? $"(g){g:0}K" : $"(g){g:0.0}K";
    //     }
    //     else
    //     {
    //         green = $"(g){g}";
    //     }
    //
    //     string blue;
    //     if (b >= 1000)
    //     {
    //         b /= 1000f;
    //         var nSplit = b.ToString(CultureInfo.InvariantCulture).Split('.');
    //         blue = nSplit[1].StartsWith("0") ? $"(b){b:0}K" : $"(b){b:0.0}K";
    //     }
    //     else
    //     {
    //         blue = $"(b){b}";
    //     }
    //
    //     foreach (var comp in GUIElements.me.hud.tech_points_bar.GetComponentsInChildren<UILabel>())
    //         comp.text = $"{red} {green} {blue}";
    // }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(GameGUI), nameof(GameGUI.Hide))]
    public static void GameGUI_Hide()
    {
        if (Plugin.QuietMusicInGuiConfig.Value)
        {
            SmartAudioEngine.me.SetDullMusicMode(false);
        }
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.GlobalEventsCheck))]
    private static bool GameSave_GlobalEventsCheck()
    {
        var year = DateTime.Now.Year;
        foreach (var globalEventBase in new List<GlobalEventBase>
                 {
                     new("halloween", Plugin.HalloweenNowConfig.Value ? DateTime.Now : new DateTime(year, 10, 29), new TimeSpan(14, 0, 0, 0))
                     {
                         on_start_script = new Scene1100_To_SceneHelloween(),
                         on_finish_script = new SceneHelloween_To_Scene1100()
                     }
                 })
            globalEventBase.Process();

        return false;
    }

    [HarmonyPostfix]
    [HarmonyAfter("p1xel8ted.GraveyardKeeper.PrayTheDayAway")]
    [HarmonyPatch(typeof(PrayCraftGUI), nameof(PrayCraftGUI.OnPrayButtonPressed))]
    public static void PrayCraftGUI_OnPrayButtonPressed(ref PrayCraftGUI __instance)
    {
        if (!Plugin.RemovePrayerOnUseConfig.Value) return;
        if (__instance == null) return;
        var playerInv = MainGame.me.player.GetMultiInventory(exceptions: null, force_world_zone: "",
            player_mi: MultiInventory.PlayerMultiInventory.IncludePlayer, include_toolbelt: true,
            include_bags: true, sortWGOS: true);
        foreach (var inv in playerInv.all)
        {
            foreach (var item in inv.data.inventory)
            {
                if (item != __instance._selected_item) continue;
                inv.data.RemoveItemNoCheck(item, 1);
                Plugin.Log.LogMessage($"Removed 1x {__instance._selected_item.id} from {inv._obj_id}.");
                return;
            }
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(LeaveTrailComponent), nameof(LeaveTrailComponent.LeaveTrail))]
    public static void LeaveTrailComponent_LeaveTrail(ref LeaveTrailComponent __instance)
    {
        if (!Plugin.LessenFootprintImpactConfig.Value) return;
        var byType = __instance._trail_definition.GetByType(__instance._trail_type);
        if (LeaveTrailComponent._all_trails.Count <= 0) return;
        var trailObject = LeaveTrailComponent._all_trails[LeaveTrailComponent._all_trails.Count - 1];
        trailObject.SetColor(byType.color, __instance._dirty_amount * 0.5f);
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameBalance), nameof(GameBalance.LoadGameBalance))]
    public static void GameBalance_LoadGameBalance()
    {
        if (Plugin.AddCoalToTavernOvenConfig.Value)
        {
            var coal = GameBalance.me.GetData<CraftDefinition>("mf_furnace_0_fuel_coal");
            coal?.craft_in.Add("tavern_oven");
        }

        if (Plugin.AddZombiesToPyreAndCrematoriumConfig.Value)
        {
            var mfPyre = GameBalance.me.GetData<ObjectDefinition>("mf_pyre");
            if (mfPyre != null)
            {
                mfPyre.can_insert_items.Add("working_zombie_on_ground_1");
                mfPyre.can_insert_items.Add("working_zombie_pseudoitem_1");
                mfPyre.can_insert_zombie = true;

                var mfCrematorium = GameBalance.me.GetData<ObjectDefinition>("mf_crematorium");
                mfCrematorium.can_insert_items.Add("working_zombie_on_ground_1");
                mfCrematorium.can_insert_items.Add("working_zombie_pseudoitem_1");
                mfCrematorium.can_insert_items.Add("body");
                mfCrematorium.can_insert_items.Add("body_guard");
                mfCrematorium.can_insert_zombie = true;

                var mfCrematoriumCorp = GameBalance.me.GetData<ObjectDefinition>("mf_crematorium_corp");
                mfCrematoriumCorp.can_insert_items.Add("working_zombie_on_ground_1");
                mfCrematoriumCorp.can_insert_items.Add("working_zombie_pseudoitem_1");
                mfCrematoriumCorp.can_insert_items.Add("body");
                mfCrematoriumCorp.can_insert_items.Add("body_guard");
                mfCrematoriumCorp.can_insert_zombie = true;
            }
        }
    }
}