namespace SaveNow;

[Harmony]
public partial class Plugin
{
    private static Vector3 Pos { get; set; }
    private static string DataPath { get; set; }
    private static string SavePath { get; set; }

    private static bool CanSave { get; set; }
    private static string CurrentSave { get; set; }
    private static readonly Dictionary<string, Vector3> SaveLocationsDictionary = new();
    private static bool IsInDungeon => MainGame.me && MainGame.me.dungeon_root && MainGame.me.dungeon_root.dungeon_is_loaded_now;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.GlobalEventsCheck))]
    public static void GameSave_GlobalEventsCheck()
    {
        Log.LogInfo("[SaveNow] Player spawned — restoring location and starting timers");
        RestoreLocation();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(EnvironmentEngine), nameof(EnvironmentEngine.OnEndOfDay))]
    public static void EnvironmentEngine_OnEndOfDay()
    {
        Log.LogInfo($"[SaveNow] End of day triggered. SaveOnNewDay={SaveOnNewDay.Value}");
        if (SaveOnNewDay.Value)
        {
            PerformNewDaySave();
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(SaveSlotsMenuGUI), nameof(SaveSlotsMenuGUI.RedrawSlots))]
    public static void SaveSlotsMenuGUI_RedrawSlots(ref List<SaveSlotData> slot_datas, ref bool focus_on_first)
    {
        SortSaveGames(ref slot_datas);

        if (MaximumSavesVisible.Value > 0 && slot_datas.Count > MaximumSavesVisible.Value)
        {
            Resize(slot_datas, MaximumSavesVisible.Value);
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

        if (SortByLastModified.Value)
        {
            saveGames = AscendingSort.Value
                ? sortedList.OrderBy(e => e.lastModified).Select(e => e.data).ToList()
                : sortedList.OrderByDescending(e => e.lastModified).Select(e => e.data).ToList();
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(InGameMenuGUI), nameof(InGameMenuGUI.OnPressedSaveAndExit))]
    public static bool InGameMenuGUI_OnPressedSaveAndExit(InGameMenuGUI __instance)
    {
        if (!ExitToDesktop.Value && !SaveOnExit.Value)
        {
            return true;
        }

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
            var baseMessage = ExitToDesktop.Value ? Lang.Get("SaveAreYouSureDesktop") : Lang.Get("SaveAreYouSureMenu");
            var progressMessage = !SaveOnExit.Value || IsInDungeon
                ? Lang.Get("SaveProgressNotSaved")
                : Lang.Get("SaveProgressSaved");

            return $"{baseMessage}?\n\n{progressMessage}.";
        }

        void SaveAndExit(InGameMenuGUI instance)
        {
            if (!SaveOnExit.Value || IsInDungeon)
            {
                PerformExit(instance);
            }
            else
            {
                if (SaveLocation(true, MainGame.me.save_slot.filename_no_extension))
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
            if (ExitToDesktop.Value)
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
        SaveLocation(false, MainGame.me.save_slot.filename_no_extension);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(InGameMenuGUI), nameof(InGameMenuGUI.Open))]
    public static void InGameMenuGUI_Open(InGameMenuGUI __instance)
    {
        if (__instance == null || !ExitToDesktop.Value) return;
        Lang.Reload();
        __instance.label_save_and_exit.text = Lang.Get("ExitButtonText");
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainGame), nameof(MainGame.Update))]
    public static void MainGame_Update()
    {
        if (!MainGame.game_started) return;

        if (ManualSaveKeyBind.Value.IsUp())
        {
            Log.LogInfo("[SaveNow] Manual save triggered via keyboard keybind");
            MainGame.me.StartCoroutine(PerformManualSave());
            return;
        }

        if (EnableManualSaveControllerButton.Value && LazyInput.gamepad_active &&
            ReInput.players.GetPlayer(0).GetButtonDown(ManualSaveControllerButton.Value))
        {
            Log.LogInfo("[SaveNow] Manual save triggered via controller button");
            MainGame.me.StartCoroutine(PerformManualSave());
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(MovementComponent), nameof(MovementComponent.UpdateMovement), null)]
    public static void MovementComponent_UpdateMovement(MovementComponent __instance)
    {
        CanSave = !__instance.player_controlled_by_script;
    }
}
