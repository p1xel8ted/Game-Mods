namespace SaveNow;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.savenow";
    private const string PluginName = "Save Now!";
    private const string PluginVer = "2.5.9";

    private const string ModGerryTag = "mod_gerry";

    internal static ConfigEntry<bool> Debug { get; private set; }
    internal static bool DebugEnabled;
    internal static bool DebugDialogShown;
    internal static ConfigEntry<int> SaveInterval { get; private set; }
    internal static ConfigEntry<bool> AutoSaveConfig { get; private set; }
    internal static ConfigEntry<bool> NewFileOnAutoSave { get; private set; }
    internal static ConfigEntry<bool> NewFileOnManualSave { get; private set; }
    internal static ConfigEntry<bool> NewFileOnNewDaySave { get; private set; }
    internal static ConfigEntry<bool> BackupSavesOnSave { get; private set; }
    internal static ConfigEntry<bool> SaveGameNotificationText { get; private set; }
    internal static ConfigEntry<bool> ExitToDesktop { get; private set; }
    internal static ConfigEntry<bool> SaveOnExit { get; private set; }
    internal static ConfigEntry<bool> SaveOnNewDay { get; private set; }
    internal static ConfigEntry<int> MaximumSavesVisible { get; private set; }
    internal static ConfigEntry<bool> SortByLastModified { get; private set; }
    internal static ConfigEntry<bool> AscendingSort { get; private set; }
    internal static ConfigEntry<bool> EnableManualSaveControllerButton { get; private set; }
    internal static ConfigEntry<KeyboardShortcut> ManualSaveKeyBind { get; private set; }
    internal static ConfigEntry<string> ManualSaveControllerButton { get; private set; }

    internal static ManualLogSource Log { get; private set; }

    internal static Vector3 Pos { get; set; }
    internal static string DataPath { get; set; }
    internal static string SavePath { get; set; }
    internal static bool CanSave { get; set; }
    internal static string CurrentSave { get; set; }
    internal static readonly Dictionary<string, Vector3> SaveLocationsDictionary = new();
    internal static bool IsInDungeon => MainGame.me && MainGame.me.dungeon_root && MainGame.me.dungeon_root.dungeon_is_loaded_now;

    private static readonly string[] TutorialQuests =
    [
        "start",
        "place_body_on_table",
        "place_interrupted",
        "place_interrupted_2",
        "grave_digging",
        "go_to_graveyard",
        "go_to_tavern",
        "go_to_lighthouse",
        "start2",
        "bishop",
        "circular_saw",
        "inquisitor_1",
        "player_repairs_sword",
        "blacksmith"
    ];

    private static readonly List<GJTimer> Timers = [];

    private static WorldGameObject _gerry;
    private static bool _gerryRunning;
    private static Coroutine AutoSaveCoroutine;

    private void Awake()
    {
        Log = Logger;
        LogHelper.Log = Logger;
        InitConfiguration();
        UpdateSaveData();
        Lang.Init(Assembly.GetExecutingAssembly(), Log);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
    }

    private void InitConfiguration()
    {
        Debug = Config.Bind("00. Advanced", "Debug Logging", false,
            new ConfigDescription("Enable or disable debug logging.", null,
                new ConfigurationManagerAttributes {Order = 3}));
        DebugEnabled = Debug.Value;
        Debug.SettingChanged += (_, _) => DebugEnabled = Debug.Value;

        SaveInterval = Config.Bind("01. Saving", "Save Interval", 600,
            new ConfigDescription("Interval between automatic saves in seconds.", null,
                new ConfigurationManagerAttributes {Order = 19}));

        SaveOnNewDay = Config.Bind("01. Saving", "Save On New Day", true,
            new ConfigDescription("Save the game when a new day starts. This is independent of the Auto Save timer.", null,
                new ConfigurationManagerAttributes {Order = 18}));

        AutoSaveConfig = Config.Bind("01. Saving", "Auto Save", true,
            new ConfigDescription("Enable or disable the automatic save timer. Note: 'Save On New Day' is a separate feature.", null,
                new ConfigurationManagerAttributes {Order = 17}));
        AutoSaveConfig.SettingChanged += (_, _) =>
        {
            KillTimers();
            if (AutoSaveConfig.Value)
            {
                StartTimer();
            }
        };

        NewFileOnAutoSave = Config.Bind("01. Saving", "New File On Auto Save", true,
            new ConfigDescription("Create a new save file for each auto save.", null,
                new ConfigurationManagerAttributes {Order = 16}));

        NewFileOnManualSave = Config.Bind("01. Saving", "New File On Manual Save", true,
            new ConfigDescription("Create a new save file for each manual save.", null,
                new ConfigurationManagerAttributes {Order = 15}));

        NewFileOnNewDaySave = Config.Bind("01. Saving", "New File On New Day Save", true,
            new ConfigDescription("Create a new save file for each new day.", null,
                new ConfigurationManagerAttributes {Order = 14}));

        BackupSavesOnSave = Config.Bind("01. Saving", "Backup Saves On Save", true,
            new ConfigDescription("Backup saves when saving the game.", null,
                new ConfigurationManagerAttributes {Order = 13}));

        SaveGameNotificationText = Config.Bind("02. Notifications", "Save Game Notification Text", false,
            new ConfigDescription("Disable save game notification text.", null,
                new ConfigurationManagerAttributes {Order = 12}));

        ExitToDesktop = Config.Bind("03. Exiting", "Exit To Desktop", false,
            new ConfigDescription("Enable or disable exit to desktop.", null,
                new ConfigurationManagerAttributes {Order = 11}));

        SaveOnExit = Config.Bind("03. Exiting", "Save On Exit", true,
            new ConfigDescription("Save the game when exiting.", null,
                new ConfigurationManagerAttributes {Order = 10}));

        MaximumSavesVisible = Config.Bind("04. UI", "Maximum Saves Visible", 3,
            new ConfigDescription("Maximum number of save files visible in the UI.", null,
                new ConfigurationManagerAttributes {Order = 9}));

        SortByLastModified = Config.Bind("04. UI", "Sort By Real Time", false,
            new ConfigDescription("Sort save files by real time instead of in-game time.", null,
                new ConfigurationManagerAttributes {Order = 8}));

        AscendingSort = Config.Bind("04. UI", "Ascending Sort", false,
            new ConfigDescription("Sort save files in ascending order.", null,
                new ConfigurationManagerAttributes {Order = 7}));

        ManualSaveKeyBind = Config.Bind("05. Controls", "Manual Save Key Bind", new KeyboardShortcut(KeyCode.K),
            new ConfigDescription("Key bind for manually saving the game.", null,
                new ConfigurationManagerAttributes {Order = 6}));

        EnableManualSaveControllerButton = Config.Bind("05. Controls", "Enable Manual Save Controller Button", false,
            new ConfigDescription("Enable or disable the manual save controller button.", null,
                new ConfigurationManagerAttributes {Order = 5}));
        ManualSaveControllerButton = Config.Bind("05. Controls", "Manual Save Controller Button",
            Enum.GetName(typeof(GamePadButton), GamePadButton.LT),
            new ConfigDescription("Controller button for manually saving the game.",
                new AcceptableValueList<string>(Enum.GetNames(typeof(GamePadButton))),
                new ConfigurationManagerAttributes {Order = 4}));
    }

    private static void UpdateSaveData()
    {
        SavePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "SaveBackup");

        if (!Directory.Exists(SavePath))
        {
            Directory.CreateDirectory(SavePath);
        }

        DataPath = Path.Combine(PlatformSpecific.GetSaveFolder(), "save-locations-savenow.dat");
        LoadSaveLocations();
    }

    internal static void SaveCallback(SaveSlotData slot)
    {
        if (Plugin.DebugEnabled) WriteLog($"[SaveNow] SaveCallback fired: slot='{slot.filename_no_extension}'");
        SaveLocation(false, slot.filename_no_extension);
        GUIElements.me.ShowSavingStatus(false);
    }

    internal static void PerformNewDaySave()
    {
        if (Plugin.DebugEnabled) WriteLog("[SaveNow] New day save starting");
        MainGame.me.StartCoroutine(PerformNewDaySaveIE());
    }

    private static IEnumerator PerformNewDaySaveIE()
    {
        if (!CanSave)
        {
            if (Plugin.DebugEnabled) WriteLog("[SaveNow] New day save skipped: player controlled by script");
            yield break;
        }

        if (IsInDungeon)
        {
            if (Plugin.DebugEnabled) WriteLog("[SaveNow] New day save skipped: in dungeon");
            Lang.Reload();
            SpawnGerry(Lang.Get("CantSaveHere"));
            yield break;
        }

        if (NewFileOnNewDaySave.Value)
        {
            var date = DateTime.Now.ToString("ddmmyyhhmmss");
            var newSlot = $"newdaysave.{date}".Trim();
            if (Plugin.DebugEnabled) WriteLog($"[SaveNow] New day saving to new slot '{newSlot}'");
            MainGame.me.save_slot.filename_no_extension = newSlot;
        }
        else
        {
            if (Plugin.DebugEnabled) WriteLog($"[SaveNow] New day saving to existing slot '{MainGame.me.save_slot.filename_no_extension}'");
        }

        GUIElements.me.ShowSavingStatus(true);
        PlatformSpecific.SaveGame(MainGame.me.save_slot, MainGame.me.save, SaveCallback);
    }

    internal static IEnumerator PerformManualSave()
    {
        if (EnvironmentEngine.me.IsTimeStopped())
        {
            if (Plugin.DebugEnabled) WriteLog("[SaveNow] Manual save skipped: time is stopped");
            yield break;
        }
        if (!Application.isFocused)
        {
            if (Plugin.DebugEnabled) WriteLog("[SaveNow] Manual save skipped: application not focused");
            yield break;
        }
        if (!CanSave)
        {
            if (Plugin.DebugEnabled) WriteLog("[SaveNow] Manual save skipped: player controlled by script");
            yield break;
        }

        if (IsInDungeon)
        {
            if (Plugin.DebugEnabled) WriteLog("[SaveNow] Manual save skipped: in dungeon");
            Lang.Reload();
            SpawnGerry(Lang.Get("CantSaveHere"));
            yield break;
        }

        if (NewFileOnManualSave.Value)
        {
            var date = DateTime.Now.ToString("ddmmyyhhmmss");
            var newSlot = $"manualsave.{date}".Trim();
            if (Plugin.DebugEnabled) WriteLog($"[SaveNow] Manual saving to new slot '{newSlot}'");
            MainGame.me.save_slot.filename_no_extension = newSlot;
        }
        else
        {
            if (Plugin.DebugEnabled) WriteLog($"[SaveNow] Manual saving to existing slot '{MainGame.me.save_slot.filename_no_extension}'");
        }

        GUIElements.me.ShowSavingStatus(true);
        PlatformSpecific.SaveGame(MainGame.me.save_slot, MainGame.me.save, SaveCallback);
    }

    internal static bool TutorialDone()
    {
        if (!MainGame.game_started) return false;

        foreach (var quest in TutorialQuests)
        {
            if (!MainGame.me.save.quests.IsQuestSucced(quest))
                return false;
        }

        return !MainGame.me.save.IsInTutorial();
    }

    private static void ShowMessage(string msg)
    {
        if (GJL.IsEastern())
        {
            MainGame.me.player.Say(msg, null, false, SpeechBubbleGUI.SpeechBubbleType.Think,
                SmartSpeechEngine.VoiceID.None, true);
        }
        else
        {
            var pos = MainGame.me.player.pos3;
            pos.y += 125f;
            EffectBubblesManager.ShowImmediately(pos, msg,
                EffectBubblesManager.BubbleColor.Relation, true, 3f);
        }
    }

    private static void SpawnGerry(string message)
    {
        if (_gerryRunning) return;

        var location = MainGame.me.player_pos;
        location.x -= 75f;

        if (_gerry == null)
        {
            _gerry = WorldMap.SpawnWGO(MainGame.me.world_root.transform, "talking_skull", location);
            GS.AddCameraTarget(_gerry.transform);
            GS.SetPlayerEnable(false, true);
            _gerry.tag = ModGerryTag;
            _gerry.custom_tag = ModGerryTag;
            _gerry.ReplaceWithObject("talking_skull", true);
            _gerry.tag = ModGerryTag;
            _gerry.custom_tag = ModGerryTag;
            _gerryRunning = true;
        }

        GJTimer.AddTimer(0.5f, delegate
        {
            if (_gerry == null)
            {
                GS.AddCameraTarget(MainGame.me.player.transform);
                GS.SetPlayerEnable(true, true);
                return;
            }

            _gerry.Say(message, delegate
            {
                GJTimer.AddTimer(0.25f, delegate
                {
                    if (_gerry == null)
                    {
                        GS.AddCameraTarget(MainGame.me.player.transform);
                        GS.SetPlayerEnable(true, true);
                        return;
                    }

                    _gerry.ReplaceWithObject("talking_skull", true);
                    _gerry.tag = ModGerryTag;
                    _gerry.custom_tag = ModGerryTag;
                    GS.AddCameraTarget(MainGame.me.player.transform);
                    GS.SetPlayerEnable(true, true);
                    _gerry.DestroyMe();
                    _gerry = null;
                    _gerryRunning = false;
                });
            }, null, SpeechBubbleGUI.SpeechBubbleType.Talk, SmartSpeechEngine.VoiceID.Skull);
        });
    }

    internal static void WriteLog(string message, bool error = false)
    {
        if (error)
        {
            LogHelper.Error(message);
        }
        else
        {
            LogHelper.Info(message);
        }
    }

    internal static void ShowDebugWarningOnce()
    {
        if (!DebugEnabled || DebugDialogShown) return;
        DebugDialogShown = true;
        Lang.Reload();
        GUIElements.me.dialog.OpenOK(PluginName, null, Lang.Get("DebugWarning"), true, string.Empty);
    }

    private static void WriteSavesToFile()
    {
        using var file = new StreamWriter(DataPath, false);
        foreach (var entry in SaveLocationsDictionary)
        {
            var result = entry.Value.ToString().Substring(1, entry.Value.ToString().Length - 2);
            result = result.Replace(" ", "");
            file.WriteLine("{0}={1}", entry.Key, result);
        }

        if (BackupSavesOnSave.Value)
        {
            MainGame.me.StartCoroutine(BackUpSaveDirectory());
        }
    }

    private static IEnumerator BackUpSaveDirectory()
    {
        try
        {
            foreach (var file in Directory.GetFiles(PlatformSpecific.GetSaveFolder()))
            {
                if (!File.Exists(Path.Combine(SavePath, Path.GetFileName(file))))
                {
                    File.Copy(file, Path.Combine(SavePath, Path.GetFileName(file)));
                }
            }
        }
        catch (Exception e)
        {
            WriteLog(e.Message, true);
        }

        yield return true;
    }

    private static void LoadSaveLocations()
    {
        if (!File.Exists(DataPath)) return;

        var lines = File.ReadAllLines(DataPath, Encoding.Default);
        foreach (var line in lines)
        {
            if (!line.Contains('=')) continue;
            var splitLine = line.Split('=');
            var saveName = splitLine[0];
            var tempVector = splitLine[1].Split(',');
            var vectorToAdd = new Vector3(float.Parse(tempVector[0].Trim(), CultureInfo.InvariantCulture),
                float.Parse(tempVector[1].Trim(), CultureInfo.InvariantCulture), float.Parse(tempVector[2].Trim(), CultureInfo.InvariantCulture));

            if (!File.Exists(Path.Combine(PlatformSpecific.GetSaveFolder(), saveName + ".dat"))) continue;
            if (!SaveLocationsDictionary.ContainsKey(saveName))
            {
                SaveLocationsDictionary.Add(saveName, vectorToAdd);
            }
        }
    }

    internal static bool SaveLocation(bool menuExit, string saveSlot)
    {
        Lang.Reload();

        Pos = MainGame.me.player.pos3;
        CurrentSave = MainGame.me.save_slot.filename_no_extension;

        SaveLocationsDictionary[CurrentSave] = Pos;

        WriteSavesToFile();

        if (menuExit) return true;

        if (SaveGameNotificationText.Value)
        {
            if (NewFileOnAutoSave.Value || NewFileOnNewDaySave.Value || NewFileOnManualSave.Value)
            {
                ShowMessage(Lang.Get("SaveMessage") + ": " + saveSlot);
            }
            else
            {
                ShowMessage(Lang.Get("SaveMessage"));
            }
        }

        return true;
    }

    internal static void Resize<T>(List<T> list, int size)
    {
        var count = list.Count;
        if (size < count) list.RemoveRange(size, count - size);
    }

    internal static void RestoreLocation()
    {
        LoadSaveLocations();

        var slot = MainGame.me.save_slot.filename_no_extension;
        var homeVector = new Vector3(2841, -6396, -1332);
        var foundLocation = SaveLocationsDictionary.TryGetValue(slot, out var posVector3);
        var pos = foundLocation ? posVector3 : homeVector;
        if (Plugin.DebugEnabled) WriteLog($"[SaveNow] RestoreLocation: slot='{slot}', found={foundLocation}, pos={pos}");
        MainGame.me.player.PlaceAtPos(pos);

        StartTimer();
    }

    internal static void KillTimers()
    {
        Timers.RemoveAll(a => !a);
        foreach (var timer in Timers)
        {
            if (Plugin.DebugEnabled) WriteLog($"Timer '{timer.name}' killed");
            timer.Stop();
            timer.DestroyComponent();
        }
        Timers.Clear();
        if (AutoSaveCoroutine != null)
        {
            MainGame.me.StopCoroutine(AutoSaveCoroutine);
            AutoSaveCoroutine = null;
        }
    }

    internal static void StartTimer()
    {
        KillTimers();
        if (AutoSaveConfig.Value)
        {
            if (Plugin.DebugEnabled) WriteLog($"[SaveNow] Starting auto-save timer: interval={SaveInterval.Value}s");
            var timer = GJTimer.AddTimer(SaveInterval.Value, AutoSave);
            timer.name = "AutoSaveTimer";
            Timers.Add(timer);
        }
        else
        {
            if (Plugin.DebugEnabled) WriteLog("[SaveNow] Auto-save is disabled, no timer started");
        }
    }

    private static void AutoSave()
    {
        if (Plugin.DebugEnabled) WriteLog("[SaveNow] Auto-save timer fired");
        if (AutoSaveCoroutine != null)
        {
            MainGame.me.StopCoroutine(AutoSaveCoroutine);
            AutoSaveCoroutine = null;
        }
        AutoSaveCoroutine = MainGame.me.StartCoroutine(AutoSaveIE());
        StartTimer();
    }

    private static IEnumerator AutoSaveIE()
    {
        if (EnvironmentEngine.me.IsTimeStopped())
        {
            if (Plugin.DebugEnabled) WriteLog("[SaveNow] Auto-save skipped: time is stopped");
            yield break;
        }
        if (!Application.isFocused)
        {
            if (Plugin.DebugEnabled) WriteLog("[SaveNow] Auto-save skipped: application not focused");
            yield break;
        }
        if (!CanSave)
        {
            if (Plugin.DebugEnabled) WriteLog("[SaveNow] Auto-save skipped: player controlled by script");
            yield break;
        }
        if (!NewFileOnAutoSave.Value)
        {
            var slot = MainGame.me.save_slot.filename_no_extension;
            if (Plugin.DebugEnabled) WriteLog($"[SaveNow] Auto-saving to existing slot '{slot}'");
            PlatformSpecific.SaveGame(MainGame.me.save_slot, MainGame.me.save,
                delegate
                {
                    if (Plugin.DebugEnabled) WriteLog($"[SaveNow] Auto-save complete: '{slot}'");
                    SaveLocation(false, slot);
                });
        }
        else
        {
            GUIElements.me.ShowSavingStatus(true);
            var date = DateTime.Now.ToString("ddmmyyhhmmss");
            var newSlot = $"autosave.{date}".Trim();
            if (Plugin.DebugEnabled) WriteLog($"[SaveNow] Auto-saving to new slot '{newSlot}'");

            MainGame.me.save_slot.filename_no_extension = newSlot;
            PlatformSpecific.SaveGame(MainGame.me.save_slot, MainGame.me.save,
                delegate
                {
                    if (Plugin.DebugEnabled) WriteLog($"[SaveNow] Auto-save complete: '{newSlot}'");
                    SaveLocation(false, newSlot);
                    GUIElements.me.ShowSavingStatus(false);
                });
        }
    }
}
