namespace SaveNow;

[Harmony]
public static class Patches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.GlobalEventsCheck))]
    public static void GameSave_GlobalEventsCheck()
    {
        if (Plugin.DebugEnabled) Plugin.WriteLog("[SaveNow] Player spawned — restoring location and starting timers");
        Plugin.ShowDebugWarningOnce();
        Plugin.RestoreLocation();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(EnvironmentEngine), nameof(EnvironmentEngine.OnEndOfDay))]
    public static void EnvironmentEngine_OnEndOfDay()
    {
        if (Plugin.DebugEnabled) Plugin.WriteLog($"[SaveNow] End of day triggered. SaveOnNewDay={Plugin.SaveOnNewDay.Value}");
        if (Plugin.SaveOnNewDay.Value)
        {
            Plugin.PerformNewDaySave();
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(SaveSlotsMenuGUI), nameof(SaveSlotsMenuGUI.RedrawSlots))]
    public static void SaveSlotsMenuGUI_RedrawSlots(ref List<SaveSlotData> slot_datas, ref bool focus_on_first)
    {
        var originalCount = slot_datas.Count;
        SortSaveGames(ref slot_datas);

        if (Plugin.MaximumSavesVisible.Value > 0 && slot_datas.Count > Plugin.MaximumSavesVisible.Value)
        {
            Plugin.Resize(slot_datas, Plugin.MaximumSavesVisible.Value);
            if (Plugin.DebugEnabled) Plugin.WriteLog($"[RedrawSlots] resized: {originalCount} → {slot_datas.Count} (cap={Plugin.MaximumSavesVisible.Value})");
        }
        else if (Plugin.DebugEnabled)
        {
            Plugin.WriteLog($"[RedrawSlots] no resize (count={originalCount}, cap={Plugin.MaximumSavesVisible.Value})");
        }

        focus_on_first = true;
    }

    private static void SortSaveGames(ref List<SaveSlotData> saveGames)
    {
        var path = PlatformSpecific.GetSaveFolder();
        var files = Directory.GetFiles(path, "*.dat", SearchOption.TopDirectoryOnly);

        var sortedList = new List<(DateTime lastModified, SaveSlotData data)>();

        foreach (var save in saveGames)
        {
            var saveFile = files.FirstOrDefault(f => f.Contains(save.filename_no_extension));
            if (saveFile == null) continue;
            var lastModified = File.GetLastWriteTime(saveFile);
            sortedList.Add((lastModified, save));
        }

        if (Plugin.SortByLastModified.Value)
        {
            saveGames = Plugin.AscendingSort.Value
                ? sortedList.OrderBy(e => e.lastModified).Select(e => e.data).ToList()
                : sortedList.OrderByDescending(e => e.lastModified).Select(e => e.data).ToList();
            if (Plugin.DebugEnabled) Plugin.WriteLog($"[SortSaveGames] sorted by time ({(Plugin.AscendingSort.Value ? "asc" : "desc")}), {sortedList.Count} entries");
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(InGameMenuGUI), nameof(InGameMenuGUI.OnPressedSaveAndExit))]
    public static bool InGameMenuGUI_OnPressedSaveAndExit(InGameMenuGUI __instance)
    {
        if (!Plugin.ExitToDesktop.Value && !Plugin.SaveOnExit.Value)
        {
            if (Plugin.DebugEnabled) Plugin.WriteLog("[SaveAndExit] skip (ExitToDesktop=false, SaveOnExit=false)");
            return true;
        }

        if (Plugin.DebugEnabled) Plugin.WriteLog($"[SaveAndExit] intercepting (ExitToDesktop={Plugin.ExitToDesktop.Value}, SaveOnExit={Plugin.SaveOnExit.Value}, InDungeon={Plugin.IsInDungeon})");

        Lang.Reload();

        __instance._stored_focus = null;
        __instance.SetControllsActive(false);
        __instance.OnClosePressed();

        var messageText = CreateMessageText();

        var dialog = GUIElements.me.dialog;
        var gui = __instance;
        dialog.OpenYesNo(messageText, delegate
        {
            SaveAndExit(gui);
        }, null, delegate
        {
            gui.SetControllsActive(true);
        });

        return false;

        string CreateMessageText()
        {
            var baseMessage = Plugin.ExitToDesktop.Value ? Lang.Get("SaveAreYouSureDesktop") : Lang.Get("SaveAreYouSureMenu");
            var progressMessage = !Plugin.SaveOnExit.Value || Plugin.IsInDungeon
                ? Lang.Get("SaveProgressNotSaved")
                : Lang.Get("SaveProgressSaved");

            return $"{baseMessage}?\n\n{progressMessage}.";
        }

        void SaveAndExit(InGameMenuGUI instance)
        {
            if (!Plugin.SaveOnExit.Value || Plugin.IsInDungeon)
            {
                PerformExit(instance);
            }
            else
            {
                if (Plugin.SaveLocation(true, MainGame.me.save_slot.filename_no_extension))
                {
                    PlatformSpecific.SaveGame(MainGame.me.save_slot, MainGame.me.save,
                        delegate
                        {
                            PerformExit(instance);
                        });
                }
            }
        }

        void PerformExit(InGameMenuGUI instance)
        {
            if (Plugin.ExitToDesktop.Value)
            {
                GC.Collect();
                Resources.UnloadUnusedAssets();
                Application.Quit();
            }
            else
            {
                LoadingGUI.Show(instance.ReturnToMainMenu);
            }
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(SleepGUI), nameof(SleepGUI.WakeUp))]
    public static void SleepGUI_WakeUp()
    {
        if (Plugin.DebugEnabled) Plugin.WriteLog("[SleepGUI.WakeUp] persisting player location");
        Plugin.SaveLocation(false, MainGame.me.save_slot.filename_no_extension);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(InGameMenuGUI), nameof(InGameMenuGUI.Open))]
    public static void InGameMenuGUI_Open(InGameMenuGUI __instance)
    {
        if (__instance == null || !Plugin.ExitToDesktop.Value) return;
        Lang.Reload();
        __instance.label_save_and_exit.text = Lang.Get("ExitButtonText");
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainGame), nameof(MainGame.Update))]
    public static void MainGame_Update()
    {
        if (!MainGame.game_started) return;

        if (Plugin.ManualSaveKeyBind.Value.IsUp())
        {
            if (Plugin.DebugEnabled) Plugin.WriteLog("[SaveNow] Manual save triggered via keyboard keybind");
            MainGame.me.StartCoroutine(Plugin.PerformManualSave());
            return;
        }

        if (Plugin.EnableManualSaveControllerButton.Value && LazyInput.gamepad_active &&
            ReInput.players.GetPlayer(0).GetButtonDown(Plugin.ManualSaveControllerButton.Value))
        {
            if (Plugin.DebugEnabled) Plugin.WriteLog("[SaveNow] Manual save triggered via controller button");
            MainGame.me.StartCoroutine(Plugin.PerformManualSave());
        }
    }

    private static bool _prevCanSave = true;
    [HarmonyPostfix]
    [HarmonyPatch(typeof(MovementComponent), nameof(MovementComponent.UpdateMovement), null)]
    public static void MovementComponent_UpdateMovement(MovementComponent __instance)
    {
        var next = !__instance.player_controlled_by_script;
        if (next != _prevCanSave)
        {
            _prevCanSave = next;
            if (Plugin.DebugEnabled) Plugin.WriteLog($"[CanSave] transition → {next} (player_controlled_by_script={__instance.player_controlled_by_script})");
        }
        Plugin.CanSave = next;
    }
}
