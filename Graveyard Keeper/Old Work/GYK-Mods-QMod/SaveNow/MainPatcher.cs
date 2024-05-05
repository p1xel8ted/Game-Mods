using HarmonyLib;
using Helper;
using SaveNow.lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using UnityEngine;

namespace SaveNow;

[HarmonyPatch]
public static partial class MainPatcher
{
    private static Vector3 _pos;
    private static string _dataPath;
    private static string _savePath;
    private static readonly List<SaveSlotData> AllSaveGames = new();
    private static List<SaveSlotData> _sortedTrimmedSaveGames = new();
    private static bool _canSave;
    private static string _currentSave;
    private static readonly Dictionary<string, Vector3> SaveLocationsDictionary = new();

    private static Config.Options _cfg;

    public static void Patch()
    {
        try
        {
            _cfg = Config.GetOptions();
            _dataPath = "./QMods/SaveNow/dont-remove.dat";
            _savePath = "./QMods/SaveNow/SaveBackup/";

            var harmony = new Harmony("p1xel8ted.GraveyardKeeper.SaveNow");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            LoadSaveLocations();
        }
        catch (Exception ex)
        {
            Log($"{ex.Message}, {ex.Source}, {ex.StackTrace}", true);
        }
    }

    private static void WriteSavesToFile()
    {
        using var file = new StreamWriter(_dataPath, false);
        foreach (var entry in SaveLocationsDictionary)
        {
            var result = entry.Value.ToString().Substring(1, entry.Value.ToString().Length - 2);
            result = result.Replace(" ", "");
            file.WriteLine("{0}={1}", entry.Key, result);
        }

        if (_cfg.backupSavesOnSave)
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
                if (!File.Exists(Path.Combine(_savePath, Path.GetFileName(file))))
                {
                    File.Copy(file, Path.Combine(_savePath, Path.GetFileName(file)));
                }
            }
        }
        catch (Exception e)
        {
            Log(e.Message, true);
        }

        yield return true;
    }

    private static void LoadSaveLocations()
    {
        if (!File.Exists(_dataPath)) return;

        var lines = File.ReadAllLines(_dataPath, Encoding.Default);
        foreach (var line in lines)
        {
            if (!line.Contains('=')) continue;
            var splitLine = line.Split('=');
            var saveName = splitLine[0];
            var tempVector = splitLine[1].Split(',');
            var vectorToAdd = new Vector3(float.Parse(tempVector[0].Trim(), CultureInfo.InvariantCulture),
                float.Parse(tempVector[1].Trim(), CultureInfo.InvariantCulture), float.Parse(tempVector[2].Trim(), CultureInfo.InvariantCulture));

            var found = SaveLocationsDictionary.TryGetValue(saveName, out _);

            if (!File.Exists(Path.Combine(PlatformSpecific.GetSaveFolder(), saveName + ".dat"))) continue;
            if (!found) SaveLocationsDictionary.Add(saveName, vectorToAdd);
        }
    }

    //reads co-ords from player, and saves to file
    private static bool SaveLocation(bool menuExit, string saveFile)
    {
        if (!Tools.TutorialDone()) return true;
        Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;

        _pos = MainGame.me.player.pos3;
        _currentSave = MainGame.me.save_slot.filename_no_extension;

        var overwrite = SaveLocationsDictionary.TryGetValue(_currentSave, out _);
        if (overwrite)
        {
            SaveLocationsDictionary.Remove(_currentSave);
            SaveLocationsDictionary.Add(_currentSave, _pos);
        }
        else
        {
            SaveLocationsDictionary.Add(_currentSave, _pos);
        }

        WriteSavesToFile();

        if (menuExit) return true;
        if (!_cfg.turnOffSaveGameNotificationText)
        {
            if (!saveFile.Equals(string.Empty))
            {
                if (_cfg.newFileOnAutoSave)
                    Tools.ShowMessage(strings.AutoSave + ": " + saveFile, Vector3.zero);
                else
                    Tools.ShowMessage(strings.AutoSave + "!", Vector3.zero);
            }
            else
            {
                Tools.ShowMessage(strings.SaveMessage, Vector3.zero);
            }
        }

        return true;
    }

    private static void Resize<T>(List<T> list, int size)
    {
        var count = list.Count;
        if (size < count) list.RemoveRange(size, count - size);
    }

    //reads co-ords from file and teleports player there
    private static void RestoreLocation()
    {
        Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;

        var homeVector = new Vector3(2841, -6396, -1332);
        var foundLocation =
            SaveLocationsDictionary.TryGetValue(MainGame.me.save_slot.filename_no_extension, out var posVector3);
        var pos = foundLocation ? posVector3 : homeVector;
        MainGame.me.player.PlaceAtPos(pos);
        if (!_cfg.turnOffTravelMessages) Tools.ShowMessage(strings.Rush, Vector3.zero);

        StartTimer();
    }

    private static void StartTimer()
    {
        if (_cfg.autoSave)
        {
            GJTimer.AddTimer(_cfg.saveInterval, AutoSave);
        }
    }

    private static void AutoSave()
    {
        if (!Tools.TutorialDone()) return;
        if (EnvironmentEngine.me.IsTimeStopped()) return;
        if (!Application.isFocused) return;
        if (!_canSave) return;
        if (!_cfg.newFileOnAutoSave)
        {
            PlatformSpecific.SaveGame(MainGame.me.save_slot, MainGame.me.save,
                delegate { SaveLocation(false, MainGame.me.save_slot.filename_no_extension); });
        }
        else
        {
            GUIElements.me.ShowSavingStatus(true);
            var date = DateTime.Now.ToString("ddmmyyhhmmss");
            var newSlot = $"autosave.{date}".Trim();

            MainGame.me.save_slot.filename_no_extension = newSlot;
            PlatformSpecific.SaveGame(MainGame.me.save_slot, MainGame.me.save,
                delegate
                {
                    SaveLocation(false, newSlot);
                    GUIElements.me.ShowSavingStatus(false);
                });
        }

        StartTimer();
    }
}