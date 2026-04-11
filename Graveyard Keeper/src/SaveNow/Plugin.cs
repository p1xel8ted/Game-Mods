namespace SaveNow;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public partial class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.savenow";
    private const string PluginName = "Save Now!";
    private const string PluginVer = "2.5.8";

    private static ConfigEntry<bool> Debug { get; set; }
    private static ConfigEntry<int> SaveInterval { get; set; }
    private static ConfigEntry<bool> AutoSaveConfig { get; set; }
    private static ConfigEntry<bool> NewFileOnAutoSave { get; set; }
    private static ConfigEntry<bool> NewFileOnManualSave { get; set; }
    private static ConfigEntry<bool> NewFileOnNewDaySave { get; set; }
    private static ConfigEntry<bool> BackupSavesOnSave { get; set; }
    private static ConfigEntry<bool> SaveGameNotificationText { get; set; }
    private static ConfigEntry<bool> ExitToDesktop { get; set; }
    private static ConfigEntry<bool> SaveOnExit { get; set; }

    private static ConfigEntry<bool> SaveOnNewDay { get; set; }
    private static ConfigEntry<int> MaximumSavesVisible { get; set; }
    private static ConfigEntry<bool> SortByLastModified { get; set; }
    private static ConfigEntry<bool> AscendingSort { get; set; }
    private static ConfigEntry<bool> EnableManualSaveControllerButton { get; set; }
    private static ConfigEntry<KeyboardShortcut> ManualSaveKeyBind { get; set; }
    private static ConfigEntry<string> ManualSaveControllerButton { get; set; }
    private static ManualLogSource Log { get; set; }

    private void Awake()
    {
        Log = Logger;
        InitConfiguration();
        UpdateSaveData();
        Lang.Init(Assembly.GetExecutingAssembly(), Log);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
    }

    private void InitConfiguration()
    {
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

        Debug = Config.Bind("00. Advanced", "Debug Logging", false,
            new ConfigDescription("Enable or disable debug logging.", null,
                new ConfigurationManagerAttributes {IsAdvanced = true, Order = 3}));
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

    private static void SaveCallback(SaveSlotData slot)
    {
        Log.LogInfo($"[SaveNow] SaveCallback fired: slot='{slot.filename_no_extension}'");
        SaveLocation(false, slot.filename_no_extension);
        GUIElements.me.ShowSavingStatus(false);
    }

    private static void PerformNewDaySave()
    {
        Log.LogInfo("[SaveNow] New day save starting");
        MainGame.me.StartCoroutine(PerformNewDaySaveIE());
    }

    private static IEnumerator PerformNewDaySaveIE()
    {
        if (!CanSave)
        {
            Log.LogInfo("[SaveNow] New day save skipped: player controlled by script");
            yield break;
        }

        if (IsInDungeon)
        {
            Log.LogInfo("[SaveNow] New day save skipped: in dungeon");
            Lang.Reload();
            SpawnGerry(Lang.Get("CantSaveHere"));
            yield break;
        }

        if (NewFileOnNewDaySave.Value)
        {
            var date = DateTime.Now.ToString("ddmmyyhhmmss");
            var newSlot = $"newdaysave.{date}".Trim();
            Log.LogInfo($"[SaveNow] New day saving to new slot '{newSlot}'");
            MainGame.me.save_slot.filename_no_extension = newSlot;
        }
        else
        {
            Log.LogInfo($"[SaveNow] New day saving to existing slot '{MainGame.me.save_slot.filename_no_extension}'");
        }

        GUIElements.me.ShowSavingStatus(true);
        PlatformSpecific.SaveGame(MainGame.me.save_slot, MainGame.me.save, SaveCallback);
    }

    private static IEnumerator PerformManualSave()
    {
        if (EnvironmentEngine.me.IsTimeStopped())
        {
            Log.LogInfo("[SaveNow] Manual save skipped: time is stopped");
            yield break;
        }
        if (!Application.isFocused)
        {
            Log.LogInfo("[SaveNow] Manual save skipped: application not focused");
            yield break;
        }
        if (!CanSave)
        {
            Log.LogInfo("[SaveNow] Manual save skipped: player controlled by script");
            yield break;
        }

        if (IsInDungeon)
        {
            Log.LogInfo("[SaveNow] Manual save skipped: in dungeon");
            Lang.Reload();
            SpawnGerry(Lang.Get("CantSaveHere"));
            yield break;
        }

        if (NewFileOnManualSave.Value)
        {
            var date = DateTime.Now.ToString("ddmmyyhhmmss");
            var newSlot = $"manualsave.{date}".Trim();
            Log.LogInfo($"[SaveNow] Manual saving to new slot '{newSlot}'");
            MainGame.me.save_slot.filename_no_extension = newSlot;
        }
        else
        {
            Log.LogInfo($"[SaveNow] Manual saving to existing slot '{MainGame.me.save_slot.filename_no_extension}'");
        }

        GUIElements.me.ShowSavingStatus(true);
        PlatformSpecific.SaveGame(MainGame.me.save_slot, MainGame.me.save, SaveCallback);
    }
}
