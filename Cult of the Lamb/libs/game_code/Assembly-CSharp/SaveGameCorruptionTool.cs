// Decompiled with JetBrains decompiler
// Type: SaveGameCorruptionTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

#nullable disable
public class SaveGameCorruptionTool
{
  public static Dictionary<string, SaveGameCorruptionTool.SaveFileStats> s_ErrorStats = new Dictionary<string, SaveGameCorruptionTool.SaveFileStats>();
  public static string s_CurrentSaveFile;
  public static bool s_RunningTest = false;

  public static void ScanSaves() => SaveGameCorruptionTool.ScanSaveTask();

  public static async Task<bool> ScanSaveTask()
  {
    string saveDir = Path.Combine(Application.persistentDataPath, "saves");
    bool result = true;
    SaveGameCorruptionTool.s_RunningTest = true;
    SaveGameCorruptionTool.s_ErrorStats.Clear();
    StreamWriter writer = new StreamWriter(SaveGameCorruptionTool.GetUniqueFileName(Path.Combine(Application.persistentDataPath, "savedata_check_stats.csv")));
    await writer.WriteLineAsync(SaveGameCorruptionTool.SaveFileStats.CSVHeader);
    string[] strArray = Directory.GetFiles(saveDir, "*.json");
    int index;
    string file;
    for (index = 0; index < strArray.Length; ++index)
    {
      file = strArray[index];
      if (await SaveGameCorruptionTool.TestSaveTask(file))
        Debug.Log((object) $"SaveCorruptionCheckTool: {file} passed the load/save test");
      else
        result = false;
      SaveGameCorruptionTool.SaveFileStats saveFileStats;
      if (SaveGameCorruptionTool.s_ErrorStats.TryGetValue(file, out saveFileStats))
      {
        await writer.WriteLineAsync(saveFileStats.CSVRecord);
        await writer.FlushAsync();
      }
      file = (string) null;
    }
    strArray = (string[]) null;
    strArray = Directory.GetFiles(saveDir, "*.mp");
    for (index = 0; index < strArray.Length; ++index)
    {
      file = strArray[index];
      if (await SaveGameCorruptionTool.TestSaveTask(file))
        Debug.Log((object) $"SaveCorruptionCheckTool: {file} passed the load/save test");
      else
        result = false;
      SaveGameCorruptionTool.SaveFileStats saveFileStats;
      if (SaveGameCorruptionTool.s_ErrorStats.TryGetValue(file, out saveFileStats))
      {
        await writer.WriteLineAsync(saveFileStats.CSVRecord);
        await writer.FlushAsync();
      }
      file = (string) null;
    }
    strArray = (string[]) null;
    foreach (SaveGameCorruptionTool.SaveFileStats saveFileStats in SaveGameCorruptionTool.s_ErrorStats.Values)
      saveFileStats.LogStats();
    SaveGameCorruptionTool.s_RunningTest = false;
    bool flag = result;
    saveDir = (string) null;
    writer = (StreamWriter) null;
    return flag;
  }

  public static async Task<bool> TestSaveTask(string filename)
  {
    Debug.Log((object) $"SaveCorruptionCheckTool: {filename}: Beginning load check");
    MMDebugText.s_DebugLines.Clear();
    try
    {
      if (SceneManager.GetActiveScene().name != "Main Menu")
      {
        Debug.Log((object) $"SaveCorruptionCheckTool: {filename}: Transitioning to Main Menu, resetting game systems");
        if (CoopManager.CoopActive)
          CoopManager.RemovePlayerFromMenu();
        SimulationManager.Pause();
        DeviceLightingManager.Reset();
        FollowerManager.Reset();
        StructureManager.Reset();
        TwitchManager.Abort();
        MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade, "Main Menu", 1f, "", (System.Action) (() => { }));
        Debug.Log((object) $"SaveCorruptionCheckTool: {filename}: Waiting for main menu transition to complete...");
        await SaveGameCorruptionTool.WaitUntil((Func<bool>) (() => !MMTransition.IsPlaying));
        Debug.Log((object) $"SaveCorruptionCheckTool: {filename}: Main menu transition Complete...");
      }
      SaveGameCorruptionTool.BindEvents();
      SaveGameCorruptionTool.SaveFileStats stats = new SaveGameCorruptionTool.SaveFileStats()
      {
        Filename = filename
      };
      SaveGameCorruptionTool.s_ErrorStats.Add(filename, stats);
      SaveGameCorruptionTool.s_CurrentSaveFile = filename;
      SaveAndLoad.LoadByPath(filename);
      Debug.Log((object) $"SaveCorruptionCheckTool: {filename}: Waiting for data to load");
      await SaveGameCorruptionTool.WaitUntil((Func<bool>) (() => SaveAndLoad.Loaded));
      Debug.Log((object) $"SaveCorruptionCheckTool: {filename}: Data Loaded, transitioning to base biome 1");
      MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, "Base Biome 1", 3f, "", (System.Action) (() => { }));
      await SaveGameCorruptionTool.WaitUntil((Func<bool>) (() => !MMTransition.IsPlaying));
      Debug.Log((object) $"SaveCorruptionCheckTool: {filename}: Transition to base biome 1 complete, waiting for the base biome manager to be initialised");
      await SaveGameCorruptionTool.WaitUntil((Func<bool>) (() => (UnityEngine.Object) BiomeBaseManager.Instance != (UnityEngine.Object) null));
      Debug.Log((object) $"SaveCorruptionCheckTool: {filename}: Data Loaded, waiting for the base set up routine to start...");
      await SaveGameCorruptionTool.WaitUntil((Func<bool>) (() => MMTransition.CanResume));
      Debug.Log((object) $"SaveCorruptionCheckTool: {filename}: Base is (kind of) ready, waiting for the player to be ready...");
      await SaveGameCorruptionTool.WaitUntil((Func<bool>) (() => !LetterBox.IsPlaying));
      Debug.Log((object) $"SaveCorruptionCheckTool: {filename}: LocationManagers are all ready, waiting for Base Location to be Active");
      await SaveGameCorruptionTool.WaitUntil((Func<bool>) (() => LocationManager.GetLocationState(FollowerLocation.Base) == LocationState.Active));
      Debug.Log((object) $"SaveCorruptionCheckTool: {filename}: Base Location is active, waiting for followers to be spawned");
      LocationManager baseLocation = LocationManager.LocationManagers[FollowerLocation.Base];
      if ((UnityEngine.Object) baseLocation != (UnityEngine.Object) null)
      {
        await SaveGameCorruptionTool.WaitUntil((Func<bool>) (() => baseLocation.FollowersSpawned));
        Debug.Log((object) $"SaveCorruptionCheckTool: {filename}: Base Followers spawned, waiting for structures to be placed");
        SaveGameCorruptionTool.SaveFileStats saveFileStats = stats;
        List<Follower> followerList = FollowerManager.FollowersAtLocation(FollowerLocation.Base);
        // ISSUE: explicit non-virtual call
        int count = followerList != null ? __nonvirtual (followerList.Count) : 0;
        saveFileStats.FollowerCount = count;
        await SaveGameCorruptionTool.WaitUntil((Func<bool>) (() => baseLocation.StructuresPlaced));
        stats.StructureCount = baseLocation.StructuresData.Count;
        Debug.Log((object) $"SaveCorruptionCheckTool: {filename}: Structures are placed");
      }
      else
        Debug.LogWarning((object) $"SaveCorruptionCheckTool: {filename}: No Base Location?");
      Debug.Log((object) $"SaveCorruptionCheckTool: {filename}: Waiting for simulation to unpause");
      await SaveGameCorruptionTool.WaitUntil((Func<bool>) (() => !SimulationManager.IsPaused));
      Debug.Log((object) $"SaveCorruptionCheckTool: {filename}: Game is Ready, letting it run a few frames, followers brain ticks are staggered over a few frames so give each follower a chance to stuff up...");
      await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(0.5));
      Debug.Log((object) $"SaveCorruptionCheckTool: {filename}: Passed Load Check");
      Debug.Log((object) $"SaveCorruptionCheckTool: {filename}: Running a Test Save");
      string uniqueFileName = SaveGameCorruptionTool.GetUniqueFileName(Path.Combine(Path.GetDirectoryName(filename), "testsaves", Path.GetFileName(filename)));
      if (!Directory.Exists(Path.GetDirectoryName(uniqueFileName)))
        Directory.CreateDirectory(Path.GetDirectoryName(uniqueFileName));
      SaveAndLoad.Save(uniqueFileName);
      await SaveGameCorruptionTool.WaitUntil((Func<bool>) (() => !HideSaveicon.IsSaving()));
      Debug.Log((object) $"SaveCorruptionCheckTool: {filename}: Test Save Complete");
      stats = (SaveGameCorruptionTool.SaveFileStats) null;
    }
    catch (Exception ex)
    {
      Debug.LogException((Exception) ex);
    }
    SaveGameCorruptionTool.UnbindEvents();
    MMDebugText.s_DebugLines.Clear();
    return true;
  }

  public static void UnbindEvents()
  {
    SaveAndLoad.OnSaveError -= new Action<MMReadWriteError>(SaveGameCorruptionTool.OnSaveError);
    SaveAndLoad.OnLoadError -= new Action<MMReadWriteError>(SaveGameCorruptionTool.OnLoadError);
    Application.logMessageReceived -= new Application.LogCallback(SaveGameCorruptionTool.HandleLog);
  }

  public static void BindEvents()
  {
    Application.logMessageReceived += new Application.LogCallback(SaveGameCorruptionTool.HandleLog);
    SaveAndLoad.OnLoadError += new Action<MMReadWriteError>(SaveGameCorruptionTool.OnLoadError);
    SaveAndLoad.OnSaveError += new Action<MMReadWriteError>(SaveGameCorruptionTool.OnSaveError);
  }

  public static void OnLoadError(MMReadWriteError obj)
  {
    Debug.Log((object) $"SaveCorruptionCheckTool: {SaveGameCorruptionTool.s_CurrentSaveFile}: LoadError: {obj.Message}");
    MMDebugText.s_DebugLines.Add($"{SaveGameCorruptionTool.s_CurrentSaveFile}: LoadDataError: {obj.Message}");
    SaveGameCorruptionTool.SaveFileStats saveFileStats;
    if (!SaveGameCorruptionTool.s_ErrorStats.TryGetValue(SaveGameCorruptionTool.s_CurrentSaveFile, out saveFileStats))
      return;
    ++saveFileStats.LoadDataErrors;
  }

  public static void OnSaveError(MMReadWriteError obj)
  {
    Debug.Log((object) $"SaveCorruptionCheckTool: {SaveGameCorruptionTool.s_CurrentSaveFile}: SaveError: {obj.Message}");
    MMDebugText.s_DebugLines.Add($"{SaveGameCorruptionTool.s_CurrentSaveFile}: SaveDataError: {obj.Message}");
    SaveGameCorruptionTool.SaveFileStats saveFileStats;
    if (!SaveGameCorruptionTool.s_ErrorStats.TryGetValue(SaveGameCorruptionTool.s_CurrentSaveFile, out saveFileStats))
      return;
    ++saveFileStats.SaveDataErrors;
  }

  public static void HandleLog(string condition, string stackTrace, LogType type)
  {
    SaveGameCorruptionTool.SaveFileStats saveFileStats;
    if (type != LogType.Error && type != LogType.Exception || !SaveGameCorruptionTool.s_ErrorStats.TryGetValue(SaveGameCorruptionTool.s_CurrentSaveFile, out saveFileStats))
      return;
    if (type != LogType.Error)
    {
      if (type != LogType.Exception)
        return;
      MMDebugText.s_DebugLines.Add($"{SaveGameCorruptionTool.s_CurrentSaveFile}: Exception: {condition}");
      ++saveFileStats.ExceptionLogCount;
    }
    else
    {
      MMDebugText.s_DebugLines.Add($"{SaveGameCorruptionTool.s_CurrentSaveFile}: Error: {condition}");
      ++saveFileStats.ErrorLogCount;
    }
  }

  public static async System.Threading.Tasks.Task WaitUntil(Func<bool> predicate)
  {
    while (!predicate())
      await System.Threading.Tasks.Task.Yield();
  }

  public static string GetUniqueFileName(string filePath)
  {
    if (!File.Exists(filePath))
      return filePath;
    string directoryName = Path.GetDirectoryName(filePath);
    string withoutExtension = Path.GetFileNameWithoutExtension(filePath);
    string extension = Path.GetExtension(filePath);
    int num = 1;
    string path = filePath;
    while (File.Exists(path))
    {
      path = Path.Combine(directoryName, $"{withoutExtension}_{num}{extension}");
      Debug.Log((object) ("Now trying path: " + path));
      ++num;
    }
    return path;
  }

  public class SaveFileStats
  {
    public string Filename = "";
    public int ErrorLogCount;
    public List<string> ErrorMsgs = new List<string>();
    public int ExceptionLogCount;
    public List<string> ExceptionMsgs = new List<string>();
    public int LoadDataErrors;
    public int SaveDataErrors;
    public int FollowerCount;
    public int StructureCount;

    public void LogStats()
    {
      Debug.Log((object) $"SaveGameCorruptionCheckStats: {{Name:{this.Filename},ErrorLogMsgs:{this.ErrorLogCount},ExceptionLogMsgs:{this.ExceptionLogCount},LoadDataErros:{this.LoadDataErrors},SaveDataErrors:{this.SaveDataErrors},FollowerCount:{this.FollowerCount},StructureCount:{this.StructureCount}}}");
    }

    public static string CSVHeader
    {
      get
      {
        return "Filepath,error_msgs,exception_msgs,load_data_errors,save_data_errors,follower_count,structure_count";
      }
    }

    public string CSVRecord
    {
      get
      {
        return $"{this.Filename},{this.ErrorLogCount},{this.ExceptionLogCount},{this.LoadDataErrors},{this.SaveDataErrors},{this.FollowerCount},{this.StructureCount}";
      }
    }
  }
}
