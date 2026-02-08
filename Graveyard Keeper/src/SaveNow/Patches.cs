namespace SaveNow;

[HarmonyPatch]
public partial class Plugin
{
    private static Vector3 Pos { get; set; }
    private static string DataPath { get; set; }
    private static string SavePath { get; set; }

    private static bool CanSave { get; set; }
    private static string CurrentSave { get; set; }
    private static readonly Dictionary<string, Vector3> SaveLocationsDictionary = new();
    internal static bool IsInDungeon { get; set; }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.Interact))]
    public static void WorldGameObject_Interact_Prefix(WorldGameObject __instance)
    {
        IsInDungeon = __instance.obj_id.ToLowerInvariant().Contains("dungeon_enter");
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.GlobalEventsCheck))]
    public static void GameSave_GlobalEventsCheck()
    {
        RestoreLocation();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(EnvironmentEngine), nameof(EnvironmentEngine.OnEndOfDay))]
    public static void EnvironmentEngine_OnEndOfDay()
    {
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

        if (slot_datas.Count > MaximumSavesVisible.Value)
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

    [HarmonyPatch(typeof(BaseMenuGUI), nameof(BaseMenuGUI.SetControllsActive))]
    [HarmonyReversePatch]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void BaseMenuGUI_SetControllsActive(BaseMenuGUI instance, bool active)
    {
        WriteLog("BaseMenuGUI_SetControllsActive: Setting controls active to " + active);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(InGameMenuGUI), nameof(InGameMenuGUI.OnPressedSaveAndExit))]
    public static bool InGameMenuGUI_OnPressedSaveAndExit(InGameMenuGUI __instance)
    {
        if (!ExitToDesktop.Value && !SaveOnExit.Value)
        {
            WriteLog("Exit to desktop and save on exit are both disabled. Letting original method run.");
            return true;
        }

        if (!TutorialDone()) return true;
        Thread.CurrentThread.CurrentUICulture = GameCulture;

        __instance._stored_focus = null;
        BaseMenuGUI_SetControllsActive(__instance, false);
        __instance.OnClosePressed();

        var messageText = CreateMessageText();

        var dialog = GUIElements.me.dialog;
        var gui = __instance;
        dialog.OpenYesNo(messageText, delegate
        {
            SaveAndExit(gui);
        }, null, delegate
        {
            WriteLog("Save Cancelled");
            BaseMenuGUI_SetControllsActive(gui, true);
        });

        return false;

        string CreateMessageText()
        {
            var baseMessage = ExitToDesktop.Value ? strings.SaveAreYouSureDesktop : strings.SaveAreYouSureMenu;
            var progressMessage = !SaveOnExit.Value || IsInDungeon
                ? strings.SaveProgressNotSaved
                : strings.SaveProgressSaved;

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
        __instance.label_save_and_exit.text = strings.ExitButtonText;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(MovementComponent), nameof(MovementComponent.UpdateMovement), null)]
    public static void MovementComponent_UpdateMovement(MovementComponent __instance)
    {
        CanSave = !__instance.player_controlled_by_script;
    }
}
