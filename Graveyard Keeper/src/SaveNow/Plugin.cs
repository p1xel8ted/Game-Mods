namespace SaveNow;

public enum SaveSortMode
{
    RealTime,
    GameTime
}

public enum SaveSortDirection
{
    Descending,
    Ascending
}

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private const string ModGerryTag = "mod_gerry";

    // Section names. New scheme: ── Foo ── (alphabetical sort in CM).
    // Legacy section names get rewritten to these by MigrateRenamedSections() on first launch
    // of the new version, so existing user customisations are preserved.
    private const string AdvancedSection      = "── 1. Advanced ──";
    private const string SavingSection        = "── 2. Saving ──";
    private const string UISection            = "── 3. UI ──";
    private const string ControlsSection      = "── 4. Controls ──";
    private const string NotificationsSection = "── 5. Notifications ──";
    private const string ExitingSection       = "── 6. Exiting ──";
    private const string UpdatesSection       = "── 7. Updates ──";

    // Migrates the 2.5.9 section names to the current "── N. Name ──" form so existing
    // user values survive the rename. Idempotent — once migrated there are no old
    // headers left for the next launch to match.
    private static readonly Dictionary<string, string> SectionRenames = new()
    {
        ["00. Advanced"]      = AdvancedSection,
        ["01. Saving"]        = SavingSection,
        ["02. Notifications"] = NotificationsSection,
        ["03. Exiting"]       = ExitingSection,
        ["04. UI"]            = UISection,
        ["05. Controls"]      = ControlsSection,
    };

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
    internal static ConfigEntry<SaveSortMode> SortMode { get; private set; }
    internal static ConfigEntry<SaveSortDirection> SortDirection { get; private set; }
    internal static ConfigEntry<bool> PinLastPlayedToTop { get; private set; }
    internal static ConfigEntry<string> LastPlayedSlot { get; private set; }
    internal static ConfigEntry<bool> EnableManualSaveControllerButton { get; private set; }
    internal static ConfigEntry<KeyboardShortcut> ManualSaveKeyBind { get; private set; }
    internal static ConfigEntry<string> ManualSaveControllerButton { get; private set; }
    internal static ConfigEntry<bool> CheckForUpdates { get; private set; }

    internal static ManualLogSource Log { get; private set; }

    internal static Vector3 Pos { get; set; }
    internal static string DataPath { get; set; }
    internal static string SavePath { get; set; }
    internal static bool CanSave { get; set; }
    internal static string CurrentSave { get; set; }
    internal static readonly Dictionary<string, Vector3> SaveLocationsDictionary = new();
    internal static bool IsInDungeon
    {
        get
        {
            if (!MainGame.me || !MainGame.me.dungeon_root) return false;
            if (!MainGame.me.dungeon_root.dungeon_is_loaded_now) return false;

            // A teleport stone moves the player out via Flow_TeleportToWGO, which never
            // calls DestroyTiles() — the one place dungeon_is_loaded_now is ever cleared.
            // So the flag can stay true after the player is back on the surface.
            // Surface zones live under world_root; dungeons have no WorldZone. If the
            // player has a current_zone, they're physically outside the dungeon.
            var pc = MainGame.me.player_component;
            if (pc != null && pc.current_zone != null) return false;

            return true;
        }
    }

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
        MigrateRenamedSections();
        InitConfiguration();
        UpdateSaveData();
        Lang.Init(Assembly.GetExecutingAssembly(), Log);
        UpdateChecker.Register(Info, CheckForUpdates);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);
    }

    // Rewrites old "[01. Saving]" style headers to "[── Saving ──]" in the .cfg file
    // so existing user values survive the section rename. Idempotent — re-running on an
    // already-migrated file is a no-op (no old headers left to find).
    private void MigrateRenamedSections()
    {
        var path = Config.ConfigFilePath;
        if (!File.Exists(path)) return;

        string content;
        try
        {
            content = File.ReadAllText(path);
        }
        catch (Exception ex)
        {
            Log.LogWarning($"[Migration] Could not read {path} for section rename: {ex.Message}");
            return;
        }

        var renamed = 0;
        foreach (var kv in SectionRenames)
        {
            var oldHeader = $"[{kv.Key}]";
            var newHeader = $"[{kv.Value}]";
            if (!content.Contains(oldHeader)) continue;
            content = content.Replace(oldHeader, newHeader);
            renamed++;
        }
        if (renamed == 0) return;

        try
        {
            File.WriteAllText(path, content);
        }
        catch (Exception ex)
        {
            Log.LogWarning($"[Migration] Could not write {path} after section rename: {ex.Message}");
            return;
        }

        Log.LogInfo($"[Migration] Renamed {renamed} legacy config section header(s) to the '── Name ──' style. Existing user values preserved.");
        Config.Reload();
    }

    private void InitConfiguration()
    {
        // ── 1. Advanced ──
        Debug = Config.Bind(AdvancedSection, "Debug Logging", false,
            new ConfigDescription("Write detailed diagnostic info to the BepInEx log. Turn on before reporting bugs.", null,
                new ConfigurationManagerAttributes {Order = 100}));
        DebugEnabled = Debug.Value;
        Debug.SettingChanged += (_, _) => DebugEnabled = Debug.Value;

        LastPlayedSlot = Config.Bind(AdvancedSection, "Last Played Slot", string.Empty,
            new ConfigDescription("Internal: tracks the last save you played so it can be pinned to the top of the list.", null,
                new ConfigurationManagerAttributes {Browsable = false, IsAdvanced = true, HideDefaultButton = true, ReadOnly = true}));

        // ── 2. Saving ──
        AutoSaveConfig = Config.Bind(SavingSection, "Auto Save", true,
            new ConfigDescription("Save your game automatically on a timer while you play.", null,
                new ConfigurationManagerAttributes {Order = 100}));
        AutoSaveConfig.SettingChanged += (_, _) =>
        {
            KillTimers();
            if (AutoSaveConfig.Value)
            {
                StartTimer();
            }
        };

        SaveInterval = Config.Bind(SavingSection, "Save Interval (Minutes)", 10,
            new ConfigDescription("Minutes between automatic saves.",
                new AcceptableValueRange<int>(1, 60),
                new ConfigurationManagerAttributes {Order = 99, ShowRangeAsPercent = false, DispName = "    └ Save Interval (Minutes)"}));

        NewFileOnAutoSave = Config.Bind(SavingSection, "New File On Auto Save", false,
            new ConfigDescription("On: every auto save creates a new file. Off: a single auto save file is reused each time.", null,
                new ConfigurationManagerAttributes {Order = 98, DispName = "    └ New File On Auto Save"}));

        SaveOnNewDay = Config.Bind(SavingSection, "Save On New Day", true,
            new ConfigDescription("Save at the start of every in-game day. Runs independently of the Auto Save timer.", null,
                new ConfigurationManagerAttributes {Order = 90}));

        NewFileOnNewDaySave = Config.Bind(SavingSection, "New File On New Day Save", true,
            new ConfigDescription("On: each new-day save gets its own file. Off: a single new-day save file is reused.", null,
                new ConfigurationManagerAttributes {Order = 89, DispName = "    └ New File On New Day Save"}));

        NewFileOnManualSave = Config.Bind(SavingSection, "New File On Manual Save", true,
            new ConfigDescription("On: each manual save creates a new file. Off: your manual save overwrites the currently loaded slot.", null,
                new ConfigurationManagerAttributes {Order = 80}));

        BackupSavesOnSave = Config.Bind(SavingSection, "Backup Saves On Save", true,
            new ConfigDescription("Copy your save files to a backup folder inside the mod's plugin directory every time the game saves.", null,
                new ConfigurationManagerAttributes {Order = 70}));

        // ── 3. UI ──
        MaximumSavesVisible = Config.Bind(UISection, "Maximum Saves Visible", 20,
            new ConfigDescription("How many saves to show in the Load Game list. 0 = unlimited.",
                new AcceptableValueRange<int>(0, 100),
                new ConfigurationManagerAttributes {Order = 100, ShowRangeAsPercent = false}));

        SortMode = Config.Bind(UISection, "Sort Mode", SaveSortMode.GameTime,
            new ConfigDescription("How the Load Game list is ordered. Game Time uses how many in-game days have passed. Real Time uses the clock time when you saved.", null,
                new ConfigurationManagerAttributes {Order = 90}));

        SortDirection = Config.Bind(UISection, "Sort Direction", SaveSortDirection.Descending,
            new ConfigDescription("Descending puts newest or most-progressed saves at the top. Ascending puts oldest at the top.", null,
                new ConfigurationManagerAttributes {Order = 89}));

        PinLastPlayedToTop = Config.Bind(UISection, "Pin Last Played To Top", false,
            new ConfigDescription("Float the save you most recently loaded or saved to the top of the list, regardless of sort.", null,
                new ConfigurationManagerAttributes {Order = 80}));

        // ── 4. Controls ──
        ManualSaveKeyBind = Config.Bind(ControlsSection, "Manual Save Key Bind", new KeyboardShortcut(KeyCode.K),
            new ConfigDescription("Keyboard shortcut to save your game instantly.", null,
                new ConfigurationManagerAttributes {Order = 100}));

        EnableManualSaveControllerButton = Config.Bind(ControlsSection, "Enable Manual Save Controller Button", false,
            new ConfigDescription("Allow saving instantly with a controller button.", null,
                new ConfigurationManagerAttributes {Order = 90}));

        ManualSaveControllerButton = Config.Bind(ControlsSection, "Manual Save Controller Button",
            Enum.GetName(typeof(GamePadButton), GamePadButton.LT),
            new ConfigDescription("Controller button used to trigger a manual save.",
                new AcceptableValueList<string>(Enum.GetNames(typeof(GamePadButton))),
                new ConfigurationManagerAttributes {Order = 89, DispName = "    └ Manual Save Controller Button"}));

        // ── 5. Notifications ──
        SaveGameNotificationText = Config.Bind(NotificationsSection, "Save Game Notification Text", false,
            new ConfigDescription("Show a small 'Saved' message above your character every time the game saves.", null,
                new ConfigurationManagerAttributes {Order = 100}));

        // ── 6. Exiting ──
        SaveOnExit = Config.Bind(ExitingSection, "Save On Exit", true,
            new ConfigDescription("Save your game when you use the Save and Exit button.", null,
                new ConfigurationManagerAttributes {Order = 100}));

        ExitToDesktop = Config.Bind(ExitingSection, "Exit To Desktop", false,
            new ConfigDescription("Make the Save and Exit button quit to desktop instead of returning to the main menu.", null,
                new ConfigurationManagerAttributes {Order = 90}));

        // ── 7. Updates ──
        CheckForUpdates = Config.Bind(UpdatesSection, "Check for Updates", true,
            new ConfigDescription(
                "Once per session at the main menu, check NexusMods for a newer version of this mod and show a quiet notification in the top-right corner if one is available. " +
                "The check fetches a single small JSON file from GitHub (no Nexus login, no API key, no user identifier) and takes under a second. " +
                "Uncheck to disable. Default: on.",
                null,
                new ConfigurationManagerAttributes {Order = 100}));
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
        LastPlayedSlot.Value = slot.filename_no_extension;
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
            WriteLog("[SaveNow] New day save skipped: player controlled by script");
            yield break;
        }

        if (IsInDungeon)
        {
            WriteLog("[SaveNow] New day save skipped: in dungeon");
            Lang.Reload();
            SpawnGerry(Lang.Get("CantSaveHere"));
            yield break;
        }

        if (NewFileOnNewDaySave.Value)
        {
            var date = DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);
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
            WriteLog("[SaveNow] Manual save skipped: time is stopped");
            yield break;
        }
        if (!Application.isFocused)
        {
            WriteLog("[SaveNow] Manual save skipped: application not focused");
            yield break;
        }
        if (!CanSave)
        {
            WriteLog("[SaveNow] Manual save skipped: player controlled by script");
            yield break;
        }

        if (IsInDungeon)
        {
            WriteLog("[SaveNow] Manual save skipped: in dungeon");
            Lang.Reload();
            SpawnGerry(Lang.Get("CantSaveHere"));
            yield break;
        }

        if (NewFileOnManualSave.Value)
        {
            var date = DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);
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
        GUIElements.me.dialog.OpenOK(MyPluginInfo.PLUGIN_NAME, null, Lang.Get("DebugWarning"), true, string.Empty);
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
        LastPlayedSlot.Value = slot;
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
            var intervalSeconds = SaveInterval.Value * 60;
            if (Plugin.DebugEnabled) WriteLog($"[SaveNow] Starting auto-save timer: interval={SaveInterval.Value}min ({intervalSeconds}s)");
            var timer = GJTimer.AddTimer(intervalSeconds, AutoSave);
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
            WriteLog("[SaveNow] Auto-save skipped: time is stopped");
            yield break;
        }
        if (!Application.isFocused)
        {
            WriteLog("[SaveNow] Auto-save skipped: application not focused");
            yield break;
        }
        if (!CanSave)
        {
            WriteLog("[SaveNow] Auto-save skipped: player controlled by script");
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
            var date = DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);
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
