namespace SaveNow;

[Harmony]
public partial class Plugin
{
    private static Vector3 Pos { get; set; }
    private static string DataPath { get; set; }
    private static string SavePath { get; set; }
    private readonly static List<SaveSlotData> AllSaveGames = [];
    private static List<SaveSlotData> SortedTrimmedSaveGames { get; set; } = [];
    private static bool CanSave { get; set; }
    private static string CurrentSave { get; set; }
    private readonly static Dictionary<string, Vector3> SaveLocationsDictionary = new();


    [HarmonyPrefix]
    [HarmonyPatch(typeof(SaveSlotsMenuGUI), nameof(SaveSlotsMenuGUI.RedrawSlots))]
    public static void SaveSlotsMenuGUI_RedrawSlots(ref List<SaveSlotData> slot_datas, ref bool focus_on_first)
    {
       
        AllSaveGames.Clear();
        SortedTrimmedSaveGames.Clear();

        LoadSaveGames();
        
        if (AllSaveGames.Count <= 0) return;
        
        slot_datas.Clear();

        SortedTrimmedSaveGames = SortSaveGames();

        if (SortedTrimmedSaveGames.Count > MaximumSavesVisible.Value)
        {
            Resize(SortedTrimmedSaveGames, MaximumSavesVisible.Value);
        }

        slot_datas = SortedTrimmedSaveGames;
        focus_on_first = true;
    }

    private static void LoadSaveGames()
    {
        var saveFiles = Directory.GetFiles(PlatformSpecific.GetSaveFolder(), "*.info",
            SearchOption.TopDirectoryOnly);

        foreach (var text in saveFiles)
        {
            var data = SaveSlotData.FromJSON(File.ReadAllText(text));
            if (data == null) continue;
            data.filename_no_extension = Path.GetFileNameWithoutExtension(text);
            AllSaveGames.Add(data);
        }
    }

    private static List<SaveSlotData> SortSaveGames()
    {
        return SortByRealTime.Value
            ? (AscendingSort.Value
                ? AllSaveGames.OrderBy(o => o.real_time).ToList()
                : AllSaveGames.OrderByDescending(o => o.real_time).ToList())
            : (AscendingSort.Value
                ? AllSaveGames.OrderBy(o => o.game_time).ToList()
                : AllSaveGames.OrderByDescending(o => o.game_time).ToList());
    }

    [HarmonyPatch(typeof(BaseMenuGUI), nameof(BaseMenuGUI.SetControllsActive))]
    [HarmonyReversePatch]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void BaseMenuGUI_SetControllsActive(BaseMenuGUI instance, bool active)
    {
        WriteLog("BaseMenuGUI_SetControllsActive: Setting controls active to " + active);
    }

    [HarmonyPatch(typeof(BaseMenuGUI), nameof(BaseMenuGUI.SetControllsActive))]
    [HarmonyReversePatch]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void BaseMenuGUI_SetControllsActive(DialogGUI instance, bool active)
    {
        WriteLog("BaseMenuGUI_SetControllsActive: Setting controls active to " + active);
    }

    // [HarmonyPostfix]
    // [HarmonyPatch(typeof(DialogGUI), nameof(DialogGUI.Open))]
    // public static void DialogGUI_Open(ref DialogGUI __instance, string text, string option_1, string option_2, string text2, string text3, GJCommons.VoidDelegate on_hide, GJCommons.VoidDelegate delegate_1)
    // {
    //     if (!MainGame.paused && PlatformSpecific._cur_status != GameEvents.GameStatus.InMenu &&
    //         !GUIElements.me.buffs_panel.gameObject.activeSelf)
    //     {
    //         WriteLog("This shouldn't be happening!");
    //         BaseMenuGUI_SetControllsActive(__instance, true);
    //         on_hide.Invoke();
    //     }
    //     WriteLog("DialogGUI_Open");
    //     WriteLog($"DialogGUI_Open: Text: {text}");
    //     WriteLog($"DialogGUI_Open: Option 1: {option_1}");
    //     WriteLog($"DialogGUI_Open: Option 2: {option_2}");
    //     WriteLog($"DialogGUI_Open: Text 2: {text2}");
    //     WriteLog($"DialogGUI_Open: Text 3: {text3}");
    // }

    // [HarmonyPostfix]
    // [HarmonyPatch(typeof(MainGame), nameof(MainGame.SetPausedMode))]
    // public static void MainGame_SetPausedMode(bool is_paused)
    // {
    //     if (!is_paused)
    //     {
    //         WriteLog("MainGame_SetPausedMode");
    //     }
    // }

    // [HarmonyPostfix]
    // [HarmonyPatch(typeof(InGameMenuGUI), nameof(InGameMenuGUI.Hide))]
    // public static void InGameMenuGUI_Hide(ref InGameMenuGUI __instance)
    // {
    //     if (MainGame.paused) return;
    //     BaseMenuGUI_SetControllsActive(__instance, true);
    //     WriteLog("InGameMenuGUI_Hide: Setting controls active to true");
    // }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(InGameMenuGUI), nameof(InGameMenuGUI.OnPressedSaveAndExit))]
    public static bool InGameMenuGUI_OnPressedSaveAndExit(ref InGameMenuGUI __instance)
    {
        if (!ExitToDesktop.Value && !SaveOnExit.Value)
        {
            WriteLog("Exit to desktop and save on exit are both disabled. Letting original method run.");
            return true;
        }
    
        if (!Tools.TutorialDone()) return true;
        Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;
    
        __instance._stored_focus = null;
        BaseMenuGUI_SetControllsActive(__instance, false);
        __instance.OnClosePressed();
    
        var messageText = CreateMessageText();
    
        var dialog = GUIElements.me.dialog;
        var gui = __instance;
        dialog.OpenYesNo(messageText, delegate { SaveAndExit(gui); }, null, delegate
        {
            WriteLog("Save Cancelled");
            BaseMenuGUI_SetControllsActive(gui, true);
        });
    
        return false;
    
        string CreateMessageText()
        {
            var baseMessage = ExitToDesktop.Value ? strings.SaveAreYouSureDesktop : strings.SaveAreYouSureMenu;
            var progressMessage = !SaveOnExit.Value || CrossModFields.IsInDungeon
                ? strings.SaveProgressNotSaved
                : strings.SaveProgressSaved;
    
            return $"{baseMessage}?\n\n{progressMessage}.";
        }
    
        void SaveAndExit(InGameMenuGUI instance)
        {
            if (!SaveOnExit.Value || CrossModFields.IsInDungeon)
            {
                PerformExit(instance);
            }
            else
            {
                if (SaveLocation(true, MainGame.me.save_slot.filename_no_extension))
                {
                    PlatformSpecific.SaveGame(MainGame.me.save_slot, MainGame.me.save,
                        delegate { PerformExit(instance); });
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


    // if this isn't here, when you sleep, it teleport you back to where the mod saved you last
    [HarmonyPrefix]
    [HarmonyPatch(typeof(SleepGUI), nameof(SleepGUI.WakeUp))]
    public static void SleepGUI_WakeUp()
    {
        SaveLocation(false, MainGame.me.save_slot.filename_no_extension);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(InGameMenuGUI), nameof(InGameMenuGUI.Open))]
    public static void InGameMenuGUI_Open(ref InGameMenuGUI __instance)
    {
        if (__instance == null || !ExitToDesktop.Value) return;
        var contentTable = GameObject.Find("UI Root/Ingame menu/content table");
        var widgets = contentTable.GetComponent<SimpleUITable>();
        widgets._widgets[5].GetComponentInChildren<UILabel>().text = strings.ExitButtonText;

    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(MovementComponent), nameof(MovementComponent.UpdateMovement), null)]
    public static void MovementComponent_UpdateMovement(MovementComponent __instance)
    {
        CanSave = !__instance.player_controlled_by_script;
    }
}