namespace KeepersCandles;

[Harmony]
public static class Patches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainGame), nameof(MainGame.Update))]
    public static void MainGame_Update()
    {
        if (!Plugin.CanFindCandles()) return;

        TryExtinguish(
            "candle",
            Plugin.ExtinguishCandleKeyBind.Value,
            Plugin.ExtinguishCandleControllerButton.Value,
            Plugin.GetCandles,
            "TooFar",
            "NoneFound");

        TryExtinguish(
            "incense",
            Plugin.ExtinguishIncenseKeyBind.Value,
            Plugin.ExtinguishIncenseControllerButton.Value,
            Plugin.GetIncenses,
            "TooFarIncense",
            "NoneFoundIncense");
    }

    private static void TryExtinguish(string typeLabel, KeyboardShortcut keybind, string controllerButtonName,
        Func<List<WorldGameObject>> getBurners, string tooFarLangKey, string noneFoundLangKey)
    {
        var keyUp = keybind.MainKey != KeyCode.None && keybind.IsUp();
        var gamepadDown = LazyInput.gamepad_active
                          && !string.IsNullOrEmpty(controllerButtonName)
                          && controllerButtonName != nameof(GamePadButton.None)
                          && ReInput.players.GetPlayer(0).GetButtonDown(controllerButtonName);
        if (!gamepadDown && !keyUp) return;

        if (Plugin.DebugEnabled)
        {
            Helpers.Log($"[Extinguish:{typeLabel}] trigger: key={keyUp} gamepad={gamepadDown} zone={MainGame.me.player.GetMyWorldZoneId()}");
        }

        WorldGameObject closest = null;
        var closestDistance = float.MaxValue;
        var count = 0;

        foreach (var burner in getBurners())
        {
            count++;
            var distance = Vector3.Distance(burner.grid_pos, Plugin.PlayerPosition);

            if (!(distance < closestDistance)) continue;

            closestDistance = distance;
            closest = burner;
        }

        if (Plugin.DebugEnabled)
        {
            Helpers.Log($"[Extinguish:{typeLabel}] scanned {count} lit {typeLabel}(s) in zone; closest={(closest ? closest.obj_id : "none")} distance={closestDistance:F2} limit={Plugin.ExtinguishDistance.Value}");
        }

        if (closest)
        {
            if (closestDistance <= Plugin.ExtinguishDistance.Value)
            {
                var unlit = Plugin.GetUnlitCandle(closest.components.craft.last_craft_id);
                if (unlit.IsNullOrWhiteSpace())
                {
                    Helpers.Log($"Could not find unlit {typeLabel} for {closest.obj_id}. Last craft ID: {closest.components.craft.last_craft_id}. Please report this!", true);
                    ResetArrow();
                    return;
                }
                if (Plugin.DebugEnabled)
                {
                    Helpers.Log($"[Extinguish:{typeLabel}] extinguishing {closest.obj_id} → {unlit} (last_craft_id={closest.components.craft.last_craft_id})");
                }
                ResetArrow();
                ReplaceAndDrop(closest, unlit);
            }
            else
            {
                if (Plugin.DebugEnabled)
                {
                    Helpers.Log($"[Extinguish:{typeLabel}] too far — pointing arrow at {closest.obj_id} ({closestDistance:F2} > {Plugin.ExtinguishDistance.Value})");
                }
                SetArrow(closest);
                MainGame.me.player.Say(Lang.Get(tooFarLangKey), null, false, SpeechBubbleGUI.SpeechBubbleType.Think, SmartSpeechEngine.VoiceID.None, true);
            }
        }
        else
        {
            if (Plugin.DebugEnabled)
            {
                Helpers.Log($"[Extinguish:{typeLabel}] no lit {typeLabel}s in current zone");
            }
            MainGame.me.player.Say(Lang.Get(noneFoundLangKey), null, false, SpeechBubbleGUI.SpeechBubbleType.Think, SmartSpeechEngine.VoiceID.None, true);
        }
    }

    internal static void ChurchColumnsToggle()
    {
        var toggled = 0;
        foreach (var column in Plugin.ChurchColumnsList.ToList().Where(column => column))
        {
            column.gameObject.SetActive(Plugin.ChurchColumns.Value);
            toggled++;
        }

        if (Plugin.DebugEnabled)
        {
            Helpers.Log($"[ChurchColumns] visibility={Plugin.ChurchColumns.Value}; toggled {toggled}/{Plugin.ChurchColumnsList.Count} column(s)");
        }
    }

    internal static void ResetArrow()
    {
        GUIElements.me.tutorial_arrow.SetActive(false);
        GUIElements.me.tutorial_arrow._attached_wgo = null;
        GUIElements.me.tutorial_arrow._visible = false;
    }

    private static void SetArrow(WorldGameObject wgo)
    {
        if (!Plugin.DirectionalArrow.Value)
        {
            if (Plugin.DebugEnabled)
            {
                Helpers.Log("[SetArrow] skipped — DirectionalArrow disabled");
            }
            ResetArrow();
            return;
        }

        GUIElements.me.tutorial_arrow.Init();
        GUIElements.me.tutorial_arrow.AttachToWGO(wgo);
        GUIElements.me.tutorial_arrow._visible = true;
    }

    internal static void OnGameBalanceLoaded()
    {
        try
        {
            var craftUpdates = 0;
            foreach (var obj in GameBalance._instance.craft_data.Where(obj => Plugin.ShouldProcess(obj.id)))
            {
                obj.dur_needs_item = 0f;
                obj.dur_parameter = 0f;
                obj.can_craft_always = true;
                craftUpdates++;
            }

            var defUpdates = 0;
            foreach (var obj in GameBalance._instance.objs_data.Where(obj => Plugin.ShouldProcess(obj.id)))
            {
                obj.durability_modificator = 0f;
                obj.always_active = true;
                defUpdates++;
            }

            var wgoUpdates = 0;
            foreach (var wgo in WorldMap._objs.Where(wgo => Plugin.ShouldProcess(wgo.obj_id) || Plugin.ShouldProcess(wgo.obj_def.id)))
            {
                wgo.obj_def.durability_modificator = 0f;
                wgo.obj_def.always_active = true;
                wgoUpdates++;
            }

            if (Plugin.DebugEnabled)
            {
                Helpers.Log($"[OnGameBalanceLoaded] candelabrum/incense updates — crafts={craftUpdates}, obj_defs={defUpdates}, live wgos={wgoUpdates}");
            }

            FixCandles();
            ChurchColumnsToggle();
        }
        catch (Exception ex)
        {
            if (Plugin.DebugEnabled)
            {
                Helpers.Log($"[OnGameBalanceLoaded] swallowed exception (expected during early scene loads): {ex.Message}");
            }
        }
    }

    private static void ReplaceAndDrop(WorldGameObject wgo, string unlitCandle)
    {
        wgo.ReplaceWithObject(unlitCandle, true);
        wgo.components.craft.is_crafting = false;

        var candleItem = wgo.components.craft._cur_craft_items_used.FirstOrDefault();
        if (candleItem != null)
        {
            if (Plugin.DebugEnabled)
            {
                Helpers.Log($"[ReplaceAndDrop] dropping recovered candle item={candleItem.id} qty={candleItem.value} at {wgo.tf.position}");
            }
            DropResGameObject.Drop(wgo.tf.position, candleItem, wgo.tf.parent, Direction.ToPlayer, 3f, Random.Range(0, 2), force_stacked_drop: true);
        }
        else
        {
            Helpers.Log($"Could not find candle item used for {wgo.obj_id}. Please report this!", true);
        }
    }

    private static void FixCandles()
    {
        var fixedCount = 0;
        foreach (var wgo in WorldMap._objs.Where(wgo => Plugin.ShouldProcess(wgo.obj_id) || Plugin.ShouldProcess(wgo.obj_def.id)))
        {
            var keyword = wgo.obj_id.Contains(Plugin.Candelabrum) ? Plugin.Candelabrum : Plugin.Incense;
            var split = wgo.obj_id.Split([keyword], StringSplitOptions.None);
            var postfix = split.Last();
            var underscoreCount = postfix.Count(c => c == '_');
            if (underscoreCount < 2) continue;
            if (Plugin.DebugEnabled)
            {
                Helpers.Log($"[FixCandles] correcting '{wgo.obj_id}' crafting status → true");
            }
            wgo.components.craft.is_crafting = true;
            fixedCount++;
        }

        if (Plugin.DebugEnabled)
        {
            Helpers.Log($"[FixCandles] corrected {fixedCount} burner(s)");
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ChunkedGameObject), nameof(ChunkedGameObject.Init))]
    public static void ChunkedGameObject_Init(ChunkedGameObject __instance)
    {
        var name = __instance.name;
        var path = Plugin.GetPath(__instance.transform);
        if (name.Contains(Plugin.Column) && path.Contains(Plugin.Church))
        {
            Plugin.ChurchColumnsList.Add(__instance.gameObject);
            if (Plugin.DebugEnabled)
            {
                Helpers.Log($"[ChunkedGameObject.Init] registered church column '{name}' (total={Plugin.ChurchColumnsList.Count})");
            }
        }

        ChurchColumnsToggle();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameBalance), nameof(GameBalance.LoadGameBalance))]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.GlobalEventsCheck))]
    [HarmonyPatch(typeof(ChunkManager), nameof(ChunkManager.RescanAllObjects))]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.LateSaveFixer))]
    public static void OnGameBalanceLoaded_Postfix()
    {
        OnGameBalanceLoaded();
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CraftComponent), nameof(CraftComponent.ReallyUpdateComponent))]
    public static bool CraftComponent_ReallyUpdateComponent(CraftComponent __instance)
    {
        var blocked = Plugin.ShouldProcess(__instance.wgo.obj_id);
        if (blocked && Plugin.DebugEnabled)
        {
            Helpers.Log($"[CraftComponent.ReallyUpdateComponent] skipping craft tick for '{__instance.wgo.obj_id}' (keeps candle/incense lit indefinitely)");
        }
        return !blocked;
    }
}
