// Decompiled with JetBrains decompiler
// Type: SaveAndLoad
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Data.ReadWrite.Conversion;
using src;
using System;
using System.IO;
using UnityEngine;

#nullable disable
public class SaveAndLoad : Singleton<SaveAndLoad>
{
  public static int SAVE_SLOT = 5;
  public static bool Loaded = false;
  public const string kSaveFileName = "slot_{0}.json";
  public const string kMetaFileName = "meta_{0}.json";
  public static System.Action OnSaveCompleted;
  public static Action<MMReadWriteError> OnSaveError;
  public static System.Action OnLoadComplete;
  public static Action<MMReadWriteError> OnLoadError;
  public static Action<int> OnSaveSlotDeleted;
  public COTLDataReadWriter<DataManager> _saveFileReadWriter = new COTLDataReadWriter<DataManager>();
  public COTLDataReadWriter<MetaData> _metaReadWriter = new COTLDataReadWriter<MetaData>();
  public const int BASE_GAME_BACK_UP_SAVE_OFFSET = 10;

  public SaveAndLoad()
  {
    COTLDataReadWriter<DataManager> saveFileReadWriter1 = this._saveFileReadWriter;
    saveFileReadWriter1.OnReadCompleted = saveFileReadWriter1.OnReadCompleted + (Action<DataManager>) (saveFile =>
    {
      DataManager.Instance = saveFile;
      if (DataManager.Instance?.Followers != null && DataManager.Instance.Followers.Count > 1)
        DataManager.Instance.Followers.Sort((Comparison<FollowerInfo>) ((a, b) => a.ID.CompareTo(b.ID)));
      DataManager.Instance.Objectives?.RemoveAll((Predicate<ObjectivesData>) (x => x == null));
      DataManager.Instance.CompletedObjectives?.RemoveAll((Predicate<ObjectivesData>) (x => x == null));
      DataManager.Instance.FailedObjectives?.RemoveAll((Predicate<ObjectivesData>) (x => x == null));
      DataManager.Instance.CompletedObjectivesHistory?.RemoveAll((Predicate<ObjectivesDataFinalized>) (x => x == null));
      DataManager.Instance.FailedObjectivesHistory?.RemoveAll((Predicate<ObjectivesDataFinalized>) (x => x == null));
      COTLDataConversion.ConvertObjectiveIDs(DataManager.Instance);
      COTLDataConversion.UpgradeTierMismatchFix(DataManager.Instance);
      SaveAndLoad.LoadMetaData();
      System.Action onLoadComplete = SaveAndLoad.OnLoadComplete;
      if (onLoadComplete != null)
        onLoadComplete();
      SaveAndLoad.Loaded = true;
    });
    COTLDataReadWriter<DataManager> saveFileReadWriter2 = this._saveFileReadWriter;
    saveFileReadWriter2.OnCreateDefault = saveFileReadWriter2.OnCreateDefault + (System.Action) (() =>
    {
      DataManager.ResetData();
      SaveAndLoad.LoadMetaData();
      System.Action onLoadComplete = SaveAndLoad.OnLoadComplete;
      if (onLoadComplete != null)
        onLoadComplete();
      SaveAndLoad.Loaded = true;
      System.Action onSaveCompleted = SaveAndLoad.OnSaveCompleted;
      if (onSaveCompleted == null)
        return;
      onSaveCompleted();
    });
    COTLDataReadWriter<DataManager> saveFileReadWriter3 = this._saveFileReadWriter;
    saveFileReadWriter3.OnWriteCompleted = saveFileReadWriter3.OnWriteCompleted + (System.Action) (() =>
    {
      System.Action onSaveCompleted = SaveAndLoad.OnSaveCompleted;
      if (onSaveCompleted == null)
        return;
      onSaveCompleted();
    });
    COTLDataReadWriter<DataManager> saveFileReadWriter4 = this._saveFileReadWriter;
    saveFileReadWriter4.OnWriteError = saveFileReadWriter4.OnWriteError + (Action<MMReadWriteError>) (error =>
    {
      Action<MMReadWriteError> onSaveError = SaveAndLoad.OnSaveError;
      if (onSaveError == null)
        return;
      onSaveError(error);
    });
    COTLDataReadWriter<DataManager> saveFileReadWriter5 = this._saveFileReadWriter;
    saveFileReadWriter5.OnReadError = saveFileReadWriter5.OnReadError + (Action<MMReadWriteError>) (error =>
    {
      Action<MMReadWriteError> onLoadError = SaveAndLoad.OnLoadError;
      if (onLoadError == null)
        return;
      onLoadError(error);
    });
    COTLDataReadWriter<MetaData> metaReadWriter1 = this._metaReadWriter;
    metaReadWriter1.OnReadCompleted = metaReadWriter1.OnReadCompleted + (Action<MetaData>) (metaData =>
    {
      DataManager.Instance.MetaData = metaData;
      DifficultyManager.ForceDifficulty(DataManager.Instance.MetaData.Difficulty);
    });
    COTLDataReadWriter<MetaData> metaReadWriter2 = this._metaReadWriter;
    metaReadWriter2.OnCreateDefault = metaReadWriter2.OnCreateDefault + (System.Action) (() =>
    {
      DataManager.Instance.MetaData = MetaData.Default(DataManager.Instance);
      DifficultyManager.ForceDifficulty(DataManager.Instance.MetaData.Difficulty);
    });
    SaveAndLoad.OnSaveCompleted += (System.Action) (() =>
    {
      MetaData metaData = DataManager.Instance.MetaData with
      {
        CultName = DataManager.Instance.CultName,
        FollowerCount = DataManager.Instance.Followers.Count,
        StructureCount = StructureManager.GetTotalHomesCount(),
        DeathCount = DataManager.Instance.Followers_Dead.Count,
        Day = TimeManager.CurrentDay,
        PlayTime = DataManager.Instance.TimeInGame,
        GameBeaten = DataManager.Instance.DeathCatBeaten,
        SandboxBeaten = DataManager.Instance.CompletedSandbox,
        Dungeon1Completed = DataManager.Instance.DungeonCompleted(FollowerLocation.Dungeon1_1),
        Dungeon2Completed = DataManager.Instance.DungeonCompleted(FollowerLocation.Dungeon1_2),
        Dungeon3Completed = DataManager.Instance.DungeonCompleted(FollowerLocation.Dungeon1_3),
        Dungeon4Completed = DataManager.Instance.DungeonCompleted(FollowerLocation.Dungeon1_4),
        Dungeon1NGPCompleted = DataManager.Instance.DungeonCompleted(FollowerLocation.Dungeon1_1, true),
        Dungeon2NGPCompleted = DataManager.Instance.DungeonCompleted(FollowerLocation.Dungeon1_2, true),
        Dungeon3NGPCompleted = DataManager.Instance.DungeonCompleted(FollowerLocation.Dungeon1_3, true),
        Dungeon4NGPCompleted = DataManager.Instance.DungeonCompleted(FollowerLocation.Dungeon1_4, true),
        DeathCatRecruited = DataManager.Instance.HasDeathCatFollower(),
        PercentageCompleted = CompletionCalculator.Calculate(DataManager.Instance),
        DLCPercentageCompleted = CompletionCalculator.CalculateDLC(DataManager.Instance),
        Permadeath = DataManager.Instance.PermadeDeathActive,
        QuickStart = DataManager.Instance.QuickStartActive,
        Penitence = DataManager.Instance.SurvivalModeActive,
        Version = Application.version,
        ActivatedMajorDLC = DataManager.Instance.SeasonsActive,
        RottingFollowerCount = DataManager.Instance.GetRottingFollowers(),
        LambGhostsCount = DataManager.Instance.TotalShrineGhostJuice,
        WinterCount = DataManager.Instance.WintersOccured,
        ExecutionerBeaten = DataManager.Instance.BeatenExecutioner,
        WolfBeaten = DataManager.Instance.BeatenWolf,
        YngyaBeaten = DataManager.Instance.BeatenYngya
      };
      DataManager.Instance.MetaData = metaData;
      SaveAndLoad.SaveMetaData(metaData);
    });
  }

  public static void Save(string filename)
  {
    if (!DataManager.Instance.AllowSaving || DataManager.Instance.DisableSaving)
      return;
    int num = CheatConsole.IN_DEMO ? 1 : 0;
  }

  public static void Save()
  {
    if (!DataManager.Instance.AllowSaving || DataManager.Instance.DisableSaving || CheatConsole.IN_DEMO)
      return;
    HideSaveicon.instance.StartRoutineSave(new System.Action(SaveAndLoad.Saving));
  }

  public static void Saving()
  {
    bool deletePreviousSave = false;
    if (SaveAndLoad.SAVE_SLOT >= 10 && !SaveAndLoad.SaveExist(SaveAndLoad.SAVE_SLOT - 10))
    {
      SaveAndLoad.SAVE_SLOT -= 10;
      deletePreviousSave = true;
    }
    Singleton<SaveAndLoad>.Instance._saveFileReadWriter.Write(DataManager.Instance, SaveAndLoad.MakeSaveSlot(SaveAndLoad.SAVE_SLOT), true, true);
    COTLDataReadWriter<DataManager> saveFileReadWriter = Singleton<SaveAndLoad>.Instance._saveFileReadWriter;
    saveFileReadWriter.OnWriteCompleted = saveFileReadWriter.OnWriteCompleted + (System.Action) (() =>
    {
      if (!deletePreviousSave)
        return;
      SaveAndLoad.DeleteSaveSlot(SaveAndLoad.SAVE_SLOT + 10);
      SaveAndLoad.DeleteMetaData(SaveAndLoad.SAVE_SLOT + 10);
    });
  }

  public static void Load(int saveSlot)
  {
    if (CheatConsole.IN_DEMO)
      return;
    int length = EquipmentManager.RelicData.Length;
    SaveAndLoad.SAVE_SLOT = saveSlot;
    Singleton<SaveAndLoad>.Instance._saveFileReadWriter.Read(SaveAndLoad.MakeSaveSlot(SaveAndLoad.SAVE_SLOT));
  }

  public static void LoadByPath(string savePath)
  {
    if (CheatConsole.IN_DEMO)
      return;
    Singleton<SaveAndLoad>.Instance._saveFileReadWriter.Read(savePath);
  }

  public static bool SaveExist(int saveSlot)
  {
    return Singleton<SaveAndLoad>.Instance._saveFileReadWriter.FileExists(SaveAndLoad.MakeSaveSlot(saveSlot));
  }

  public static void DeleteSaveSlot(int saveSlot)
  {
    Singleton<SaveAndLoad>.Instance._saveFileReadWriter.Delete(SaveAndLoad.MakeSaveSlot(saveSlot));
    SaveAndLoad.DeleteMetaData(saveSlot);
    Action<int> onSaveSlotDeleted = SaveAndLoad.OnSaveSlotDeleted;
    if (onSaveSlotDeleted == null)
      return;
    onSaveSlotDeleted(saveSlot);
  }

  public static void DeleteScreenshotInfo(int slot)
  {
    COTLDataReadWriter<MetaData> cotlDataReadWriter = new COTLDataReadWriter<MetaData>();
    cotlDataReadWriter.Read(SaveAndLoad.MakeMetaSlot(slot));
    cotlDataReadWriter.OnReadCompleted = cotlDataReadWriter.OnReadCompleted + (Action<MetaData>) (meta =>
    {
      for (int index = 0; index <= meta.Day; ++index)
      {
        string path = Path.Combine(Application.persistentDataPath, "Screenshots", $"day_{index}_{slot}.png");
        if (File.Exists(path))
          File.Delete(path);
      }
    });
  }

  public static void ResetSave(int saveSlot, bool newGame)
  {
    SaveAndLoad.SAVE_SLOT = saveSlot;
    DataManager.ResetData();
    if (!newGame)
      SaveAndLoad.Save();
    SaveAndLoad.Loaded = true;
  }

  public static void SaveMetaData(MetaData metaData)
  {
    Debug.Log((object) "Save MetaData".Colour(Color.yellow));
    Singleton<SaveAndLoad>.Instance._metaReadWriter.Write(metaData, SaveAndLoad.MakeMetaSlot(SaveAndLoad.SAVE_SLOT), true, true);
  }

  public static void LoadMetaData()
  {
    Debug.Log((object) "Load MetaData".Colour(Color.yellow));
    Singleton<SaveAndLoad>.Instance._metaReadWriter.Read(SaveAndLoad.MakeMetaSlot(SaveAndLoad.SAVE_SLOT));
  }

  public static void DeleteMetaData(int saveSlot)
  {
    Debug.Log((object) "Delete MetaData".Colour(Color.yellow));
    Singleton<SaveAndLoad>.Instance._metaReadWriter.Delete(SaveAndLoad.MakeMetaSlot(saveSlot));
  }

  public static string MakeSaveSlot(int slot) => $"slot_{slot}.json";

  public static string MakeMetaSlot(int slot) => $"meta_{slot}.json";

  public static void MakeBaseGameBackUpSave(System.Action callback)
  {
    if (SaveAndLoad.SAVE_SLOT >= 10 && SaveAndLoad.SaveExist(SaveAndLoad.SAVE_SLOT - 10))
      return;
    HideSaveicon.instance.StartRoutineSave((System.Action) (() =>
    {
      int saveSlot = SaveAndLoad.SAVE_SLOT;
      if (SaveAndLoad.SAVE_SLOT >= 10 && !SaveAndLoad.SaveExist(SaveAndLoad.SAVE_SLOT - 10))
      {
        SaveAndLoad.SAVE_SLOT -= 10;
        saveSlot = SaveAndLoad.SAVE_SLOT;
      }
      else
        SaveAndLoad.SAVE_SLOT += 10;
      SaveAndLoad.SaveMetaData(DataManager.Instance.MetaData);
      SaveAndLoad.Saving();
      SaveAndLoad.SAVE_SLOT = saveSlot;
      System.Action action = callback;
      if (action == null)
        return;
      action();
    }));
  }
}
