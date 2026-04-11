namespace SaveNow;

public partial class Plugin
{
    private const string ModGerryTag = "mod_gerry";

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

    private static bool TutorialDone()
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

    private static WorldGameObject _gerry;
    private static bool _gerryRunning;

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

    private static void WriteLog(string message, bool error = false)
    {
        if (error)
        {
            Log.LogError($"{message}");
        }
        else
        {
            if (Debug.Value)
            {
                Log.LogWarning($"{message}");
            }
        }
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

    private static bool SaveLocation(bool menuExit, string saveSlot)
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

    private static void Resize<T>(List<T> list, int size)
    {
        var count = list.Count;
        if (size < count) list.RemoveRange(size, count - size);
    }

    private static void RestoreLocation()
    {
        LoadSaveLocations();

        var slot = MainGame.me.save_slot.filename_no_extension;
        var homeVector = new Vector3(2841, -6396, -1332);
        var foundLocation = SaveLocationsDictionary.TryGetValue(slot, out var posVector3);
        var pos = foundLocation ? posVector3 : homeVector;
        Log.LogInfo($"[SaveNow] RestoreLocation: slot='{slot}', found={foundLocation}, pos={pos}");
        MainGame.me.player.PlaceAtPos(pos);

        StartTimer();
    }

    private static readonly List<GJTimer> Timers = [];

    private static void KillTimers()
    {
        Timers.RemoveAll(a => !a);
        foreach (var timer in Timers)
        {
            WriteLog($"Timer '{timer.name}' killed");
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

    private static void StartTimer()
    {
        KillTimers();
        if (AutoSaveConfig.Value)
        {
            Log.LogInfo($"[SaveNow] Starting auto-save timer: interval={SaveInterval.Value}s");
            var timer = GJTimer.AddTimer(SaveInterval.Value, AutoSave);
            timer.name = "AutoSaveTimer";
            Timers.Add(timer);
        }
        else
        {
            Log.LogInfo("[SaveNow] Auto-save is disabled, no timer started");
        }
    }

    private static Coroutine AutoSaveCoroutine;

    private static void AutoSave()
    {
        Log.LogInfo("[SaveNow] Auto-save timer fired");
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
            Log.LogInfo("[SaveNow] Auto-save skipped: time is stopped");
            yield break;
        }
        if (!Application.isFocused)
        {
            Log.LogInfo("[SaveNow] Auto-save skipped: application not focused");
            yield break;
        }
        if (!CanSave)
        {
            Log.LogInfo("[SaveNow] Auto-save skipped: player controlled by script");
            yield break;
        }
        if (!NewFileOnAutoSave.Value)
        {
            var slot = MainGame.me.save_slot.filename_no_extension;
            Log.LogInfo($"[SaveNow] Auto-saving to existing slot '{slot}'");
            PlatformSpecific.SaveGame(MainGame.me.save_slot, MainGame.me.save,
                delegate
                {
                    Log.LogInfo($"[SaveNow] Auto-save complete: '{slot}'");
                    SaveLocation(false, slot);
                });
        }
        else
        {
            GUIElements.me.ShowSavingStatus(true);
            var date = DateTime.Now.ToString("ddmmyyhhmmss");
            var newSlot = $"autosave.{date}".Trim();
            Log.LogInfo($"[SaveNow] Auto-saving to new slot '{newSlot}'");

            MainGame.me.save_slot.filename_no_extension = newSlot;
            PlatformSpecific.SaveGame(MainGame.me.save_slot, MainGame.me.save,
                delegate
                {
                    Log.LogInfo($"[SaveNow] Auto-save complete: '{newSlot}'");
                    SaveLocation(false, newSlot);
                    GUIElements.me.ShowSavingStatus(false);
                });
        }
    }
}
