namespace MiscBitsAndBobs;

[HarmonyPatch]
public static class Patches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(SpeechBubbleGUI), nameof(SpeechBubbleGUI.SpeechText))]
    public static void SpeechBubbleGUI_SpeechText(string s, ref string __result)
    {
        if (!Plugin.OldEnglishThrowback.Value) return;
        if (!s.Equals("Our church is great!")) return;
        if (!GJL.GetCurLng().Equals("en")) return;
        if (Plugin.DebugEnabled) Helpers.Log("[OldEnglish] Replaced sermon line 'Our church is great!' → 'Our church great!'.");
        __result = "Our church great!";
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(MovementComponent), nameof(MovementComponent.UpdateMovement), typeof(Vector2), typeof(float))]
    public static void MovementComponent_UpdateMovement(MovementComponent __instance)
    {
        if (IsMovementUpdateAllowed(__instance)) return;

        if (__instance.wgo.is_player)
        {
            UpdatePlayerMovementSpeed(__instance);
        }
        else if (__instance.wgo.IsWorker() && __instance.wgo.data.inventory.Any(backpack => backpack.id == "porter_backpack"))
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
        if (show && Plugin.CinematicLetterboxingConfig.Value)
        {
            if (Plugin.DebugEnabled) Helpers.Log("[Letterbox] Suppressing cinematic letterbox (show=true → false).");
            show = false;
        }
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(Intro), nameof(Intro.ShowIntro))]
    public static void Intro_ShowIntro()
    {
        if (Intro.need_show_first_intro && Plugin.SkipIntroVideoOnNewGameConfig.Value)
        {
            if (Plugin.DebugEnabled) Helpers.Log("[Intro] need_show_first_intro=true and Skip Intro is on — cancelling intro.");
            Intro.need_show_first_intro = false;
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(CraftComponent), nameof(CraftComponent.End))]
    public static void CraftComponent_End(CraftComponent __instance)
    {
        if (!MainGame.game_started || __instance == null) return;

        if (!Plugin.KitsuneKitoModeConfig.Value) return;

        if (__instance.last_craft_id.Equals("set_grave_bot_wd_1"))
        {
            if (Plugin.DebugEnabled) Helpers.Log("[KitsuneKito] Wooden grave fence placed — dropping 1 blue XP orb.");
            TechPointsDrop.Drop(MainGame.me.player.pos3, 0, 0, 1);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(GameGUI), nameof(GameGUI.Open))]
    public static void GameGUI_Open()
    {
        if (Plugin.QuietMusicInGuiConfig.Value)
        {
            if (Plugin.DebugEnabled) Helpers.Log("[Music] GameGUI opened — enabling dull music mode.");
            SmartAudioEngine.me.SetDullMusicMode();
        }
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(MainMenuGUI), nameof(MainMenuGUI.Open))]
    public static void MainMenuGUI_Open_Prefix(MainMenuGUI __instance)
    {
        var enabled = Plugin.HideCreditsButtonOnMainMenuConfig.Value;
        if (__instance == null) return;

        var hiddenCount = 0;
        foreach (var comp in __instance.GetComponentsInChildren<UIButton>()
                     .Where(x => x.name.Contains("credits")))
        {
            comp.SetState(enabled ? UIButtonColor.State.Normal : UIButtonColor.State.Disabled, true);
            comp.SetActive(!enabled);
            hiddenCount++;
        }

        if (Plugin.DebugEnabled) Helpers.Log($"[MainMenu] Credits buttons processed: count={hiddenCount} hidden={enabled}.");
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainMenuGUI), nameof(MainMenuGUI.Open))]
    public static void MainMenuGUI_Open_Postfix()
    {
        Helpers.Sprint = Harmony.HasAnyPatches("mugen.GraveyardKeeper.SprintReloaded");
        if (Plugin.DebugEnabled) Helpers.Log($"[MainMenu] Sprint Reloaded detected via Harmony: {Helpers.Sprint}.");
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(HUD), nameof(HUD.Update))]
    public static void HUD_Update(HUD __instance)
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


    [HarmonyPrefix]
    [HarmonyPatch(typeof(GameGUI), nameof(GameGUI.Hide))]
    public static void GameGUI_Hide()
    {
        if (Plugin.QuietMusicInGuiConfig.Value)
        {
            if (Plugin.DebugEnabled) Helpers.Log("[Music] GameGUI hidden — restoring normal music volume.");
            SmartAudioEngine.me.SetDullMusicMode(false);
        }
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.GlobalEventsCheck))]
    private static bool GameSave_GlobalEventsCheck()
    {
        var year = DateTime.Now.Year;
        var halloweenStart = Plugin.HalloweenNowConfig.Value ? DateTime.Now : new DateTime(year, 10, 29);
        if (Plugin.DebugEnabled) Helpers.Log($"[Halloween] GlobalEventsCheck processing Halloween event: start={halloweenStart:yyyy-MM-dd} (Halloween Now={Plugin.HalloweenNowConfig.Value}).");
        foreach (var globalEventBase in new List<GlobalEventBase>
                 {
                     new("halloween", halloweenStart, new TimeSpan(14, 0, 0, 0))
                     {
                         on_start_script = new Scene1100_To_SceneHelloween(),
                         on_finish_script = new SceneHelloween_To_Scene1100()
                     }
                 })
            globalEventBase.Process();

        return false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.GlobalEventsCheck))]
    private static void GameSave_GlobalEventsCheck_Postfix()
    {
        Helpers.ActionsOnSpawnPlayer();
    }

    [HarmonyPostfix]
    [HarmonyAfter("p1xel8ted.gyk.praythedayaway")]
    [HarmonyPatch(typeof(PrayCraftGUI), nameof(PrayCraftGUI.OnPrayButtonPressed))]
    public static void PrayCraftGUI_OnPrayButtonPressed(PrayCraftGUI __instance)
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
                if (Plugin.DebugEnabled) Helpers.Log($"[Prayer] Removed 1x {__instance._selected_item.id} from inventory '{inv._obj_id}' after pray.");
                return;
            }
        }
        if (Plugin.DebugEnabled) Helpers.Log($"[Prayer] No matching item found in any inventory for selected prayer {__instance._selected_item?.id}.");
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(LeaveTrailComponent), nameof(LeaveTrailComponent.LeaveTrail))]
    public static void LeaveTrailComponent_LeaveTrail(LeaveTrailComponent __instance)
    {
        if (!Plugin.LessenFootprintImpactConfig.Value) return;
        if (LeaveTrailComponent._all_trails.Count <= 0) return;

        var byType = __instance._trail_definition.GetByType(__instance._trail_type);
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
            if (coal != null)
            {
                coal.craft_in.Add("tavern_oven");
                if (Plugin.DebugEnabled) Helpers.Log("[GameBalance] Added 'tavern_oven' to coal fuel craft_in list.");
            }
            else
            {
                if (Plugin.DebugEnabled) Helpers.Log("[GameBalance] Skipped coal→tavern_oven wiring: CraftDefinition 'mf_furnace_0_fuel_coal' missing.");
            }
        }

        if (Plugin.AddZombiesToPyreAndCrematoriumConfig.Value)
        {
            var mfPyre = GameBalance.me.GetData<ObjectDefinition>("mf_pyre");
            if (mfPyre != null)
            {
                mfPyre.can_insert_items.Add("working_zombie_on_ground_1");
                mfPyre.can_insert_items.Add("working_zombie_pseudoitem_1");
                mfPyre.can_insert_zombie = true;
                if (Plugin.DebugEnabled) Helpers.Log("[GameBalance] Added zombie inputs to mf_pyre.");

                var mfCrematorium = GameBalance.me.GetData<ObjectDefinition>("mf_crematorium");
                mfCrematorium.can_insert_items.Add("working_zombie_on_ground_1");
                mfCrematorium.can_insert_items.Add("working_zombie_pseudoitem_1");
                mfCrematorium.can_insert_items.Add("body");
                mfCrematorium.can_insert_items.Add("body_guard");
                mfCrematorium.can_insert_zombie = true;
                if (Plugin.DebugEnabled) Helpers.Log("[GameBalance] Added zombie + body inputs to mf_crematorium.");

                var mfCrematoriumCorp = GameBalance.me.GetData<ObjectDefinition>("mf_crematorium_corp");
                mfCrematoriumCorp.can_insert_items.Add("working_zombie_on_ground_1");
                mfCrematoriumCorp.can_insert_items.Add("working_zombie_pseudoitem_1");
                mfCrematoriumCorp.can_insert_items.Add("body");
                mfCrematoriumCorp.can_insert_items.Add("body_guard");
                mfCrematoriumCorp.can_insert_zombie = true;
                if (Plugin.DebugEnabled) Helpers.Log("[GameBalance] Added zombie + body inputs to mf_crematorium_corp.");
            }
            else
            {
                if (Plugin.DebugEnabled) Helpers.Log("[GameBalance] Skipped zombie/pyre wiring: ObjectDefinition 'mf_pyre' missing.");
            }
        }
    }
}
