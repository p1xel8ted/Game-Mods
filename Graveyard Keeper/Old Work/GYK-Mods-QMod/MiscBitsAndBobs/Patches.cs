using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HarmonyLib;
using Helper;
using MiscBitsAndBobs.lang;
using UnityEngine;
using Tools = Helper.Tools;

namespace MiscBitsAndBobs;

[HarmonyPatch]
public static partial class MainPatcher
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(MovementComponent), nameof(MovementComponent.UpdateMovement), typeof(Vector2), typeof(float))]
    public static void MovementComponent_UpdateMovement(ref MovementComponent __instance)
    {
        if (__instance.wgo.is_dead || __instance.player_controlled_by_script) return;
        //Log($"[MoveSpeed]: Instance: {__instance.wgo.obj_id}, Speed: {__instance.wgo.data.GetParam("speed")}");

        if (__instance.wgo.is_player && !_sprint)
        {
            var speed = __instance.wgo.data.GetParam("speed");
            if (speed > 0)
            {
                speed = LazyConsts.PLAYER_SPEED + __instance.wgo.data.GetParam("speed_buff");
            }

            if (_cfg.modifyPlayerMovementSpeed)
            {
                __instance.SetSpeed(speed * _cfg.playerMovementSpeed);
            }
            else
            {
                __instance.SetSpeed(speed);
            }
        }

        if (__instance.wgo.IsWorker() && WorkerHasBackpack(__instance.wgo))
        {
            //1 and 0 = the same speed in game for zombies
            if (_cfg.modifyPorterMovementSpeed)
            {
                __instance.SetSpeed(_cfg.porterMovementSpeed);
            }
            else
            {
                __instance.SetSpeed(0);
            }
        }
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.Update))]
    public static void TimeOfDay_Update()
    {
        if (!MainGame.game_started) return;

        if (MainGame.game_started && !_sprintMsgShown && _sprint && _cfg.modifyPlayerMovementSpeed)
        {
            Tools.ShowAlertDialog(GetLocalizedString(strings.Title), GetLocalizedString(strings.Content), separateWithStars: true);
            _sprintMsgShown = true;
        }

        Application.runInBackground = _cfg.keepGamingRunningInBackground;

        if (Input.GetKeyUp(_cfg.reloadConfigKeyBind))
        {
            _cfg = Config.GetOptions();

            if (Time.time - CrossModFields.ConfigReloadTime > CrossModFields.ConfigReloadTimeLength)
            {
                Tools.ShowMessage(GetLocalizedString(strings.ConfigMessage), Vector3.zero);
                CrossModFields.ConfigReloadTime = Time.time;
            }
        }
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(CameraTools), nameof(CameraTools.TweenLetterbox))]
    public static void CameraTools_TweenLetterbox(ref bool show)
    {
        if (_cfg.disableCinematicLetterboxing)
        {
            show = false;
        }
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(Intro), nameof(Intro.ShowIntro))]
    public static void Intro_ShowIntro()
    {
        if (_cfg.skipIntroVideoOnNewGame)
        {
            Intro.need_show_first_intro = false;
        }
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.Interact))]
    public static void WorldGameObject_Interact(WorldGameObject __instance, WorldGameObject other_obj)
    {
        if (!MainGame.game_started || __instance == null) return;
        if (other_obj == MainGame.me.player)
        {
            _wgo = __instance;
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(CraftComponent), nameof(CraftComponent.End))]
    public static void CraftComponent_End(ref CraftComponent __instance)
    {
        if (!MainGame.game_started || __instance == null) return;

        if (!_cfg.kitsuneKitoMode) return;

        if (__instance.last_craft_id.Equals("set_grave_bot_wd_1"))
        {
            TechPointsDrop.Drop(_wgo.pos3, 0, 0, 1);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(GameGUI), nameof(GameGUI.Open))]
    public static void GameGUI_Open()
    {
        if (_cfg.quietMusicInGui) SmartAudioEngine.me.SetDullMusicMode();
    }


    [HarmonyPrefix]
    [HarmonyAfter("p1xel8ted.GraveyardKeeper.QModHelper")]
    [HarmonyPatch(typeof(MainMenuGUI), nameof(MainMenuGUI.Open))]
    public static void MainMenuGUI_Open_Prefix(ref MainMenuGUI __instance)
    {
        if (!_cfg.hideCreditsButtonOnMainMenu) return;
        if (__instance == null) return;

        foreach (var comp in __instance.GetComponentsInChildren<UIButton>()
                     .Where(x => x.name.Contains("credits")))
        {
            comp.SetState(UIButtonColor.State.Disabled, true);
            comp.SetActive(false);
        }
    }

    [HarmonyPostfix]
    [HarmonyAfter("p1xel8ted.GraveyardKeeper.QModHelper")]
    [HarmonyPatch(typeof(MainMenuGUI), nameof(MainMenuGUI.Open))]
    public static void MainMenuGUI_Open_Postfix()
    {
        _sprintTools = Tools.ModLoaded("", "SprintReloaded.dll", "Sprint Reloaded");
        _sprintHarmony = Harmony.HasAnyPatches("mugen.GraveyardKeeper.SprintReloaded");
        _sprint = _sprintTools || _sprintHarmony;

        Log($"[MBB]: Sprint Detected via Tools: {_sprintTools}");

        Log($"[MBB]: Sprint Detected via Harmony: {_sprintHarmony}");
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
        if (!__instance._inited || !MainGame.game_started || !_cfg.condenseXpBar) return;

        var r = MainGame.me.player.GetParam("r");
        var g = MainGame.me.player.GetParam("g");
        var b = MainGame.me.player.GetParam("b");

        string red;
        if (r >= 1000)
        {
            r /= 1000f;
            var nSplit = r.ToString(CultureInfo.InvariantCulture).Split('.');
            red = nSplit[1].StartsWith("0") ? $"(r){r:0}K" : $"(r){r:0.0}K";
        }
        else
        {
            red = $"(r){r}";
        }

        string green;
        if (g >= 1000)
        {
            g /= 1000f;
            var nSplit = g.ToString(CultureInfo.InvariantCulture).Split('.');
            green = nSplit[1].StartsWith("0") ? $"(g){g:0}K" : $"(g){g:0.0}K";
        }
        else
        {
            green = $"(g){g}";
        }

        string blue;
        if (b >= 1000)
        {
            b /= 1000f;
            var nSplit = b.ToString(CultureInfo.InvariantCulture).Split('.');
            blue = nSplit[1].StartsWith("0") ? $"(b){b:0}K" : $"(b){b:0.0}K";
        }
        else
        {
            blue = $"(b){b}";
        }

        foreach (var comp in GUIElements.me.hud.tech_points_bar.GetComponentsInChildren<UILabel>())
            comp.text = $"{red} {green} {blue}";
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(GameGUI), nameof(GameGUI.Hide))]
    public static void GameGUI_Hide()
    {
        if (_cfg.quietMusicInGui) SmartAudioEngine.me.SetDullMusicMode(false);
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.GlobalEventsCheck))]
    private static bool GameSave_GlobalEventsCheck()
    {
        var year = DateTime.Now.Year;
        foreach (var globalEventBase in new List<GlobalEventBase>
                 {
                     new("halloween", _cfg.halloweenNow ? DateTime.Now : new DateTime(year, 10, 29), new TimeSpan(14, 0, 0, 0))
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
        if (!_cfg.removePrayerOnUse) return;
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
                Log($"Removed 1x {__instance._selected_item.id} from {inv._obj_id}.");
                return;
            }
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(LeaveTrailComponent), nameof(LeaveTrailComponent.LeaveTrail))]
    public static void LeaveTrailComponent_LeaveTrail(ref LeaveTrailComponent __instance)
    {
        if (!_cfg.lessenFootprintImpact) return;
        var byType = __instance._trail_definition.GetByType(__instance._trail_type);
        if (LeaveTrailComponent._all_trails.Count <= 0) return;
        var trailObject = LeaveTrailComponent._all_trails[LeaveTrailComponent._all_trails.Count - 1];
        trailObject.SetColor(byType.color, __instance._dirty_amount * 0.5f);
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameBalance), nameof(GameBalance.LoadGameBalance))]
    public static void GameBalance_LoadGameBalance()
    {
        if (_cfg.addCoalToTavernOven)
        {
            var coal = GameBalance.me.GetData<CraftDefinition>("mf_furnace_0_fuel_coal");
            coal?.craft_in.Add("tavern_oven");
        }

        if (_cfg.addZombiesToPyreAndCrematorium)
        {
            var mfPyre = GameBalance.me.GetData<ObjectDefinition>("mf_pyre");
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