// Decompiled with JetBrains decompiler
// Type: SaveAndLoad
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Data.ReadWrite.Conversion;
using System;
using System.IO;
using UnityEngine;

#nullable disable
public class SaveAndLoad : Singleton<SaveAndLoad>
{
  public static int SAVE_SLOT = 5;
  public static bool Loaded = false;
  private const string kSaveFileName = "slot_{0}.json";
  private const string kMetaFileName = "meta_{0}.json";
  public static System.Action OnSaveCompleted;
  public static Action<MMReadWriteError> OnSaveError;
  public static System.Action OnLoadComplete;
  public static Action<MMReadWriteError> OnLoadError;
  public static Action<int> OnSaveSlotDeleted;
  private COTLDataReadWriter<DataManager> _saveFileReadWriter = new COTLDataReadWriter<DataManager>();
  private COTLDataReadWriter<MetaData> _metaReadWriter = new COTLDataReadWriter<MetaData>();

  public SaveAndLoad()
  {
    COTLDataReadWriter<DataManager> saveFileReadWriter1 = this._saveFileReadWriter;
    saveFileReadWriter1.OnReadCompleted = saveFileReadWriter1.OnReadCompleted + (Action<DataManager>) (saveFile =>
    {
      DataManager.Instance = saveFile;
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
  }

  public static void Save()
  {
    if (!DataManager.Instance.AllowSaving || CheatConsole.IN_DEMO)
      return;
    HideSaveicon.instance.StartRoutineSave(new System.Action(SaveAndLoad.Saving));
  }

  private static void Saving()
  {
    Singleton<SaveAndLoad>.Instance._saveFileReadWriter.Write(DataManager.Instance, SaveAndLoad.MakeSaveSlot(SaveAndLoad.SAVE_SLOT), true, true);
    MetaData metaData = DataManager.Instance.MetaData with
    {
      CultName = DataManager.Instance.CultName,
      FollowerCount = DataManager.Instance.Followers.Count,
      StructureCount = StructureManager.GetTotalHomesCount(),
      DeathCount = DataManager.Instance.Followers_Dead.Count,
      Day = TimeManager.CurrentDay,
      PlayTime = DataManager.Instance.TimeInGame,
      GameBeaten = DataManager.Instance.DeathCatBeaten,
      Dungeon1Completed = DataManager.Instance.DungeonCompleted(FollowerLocation.Dungeon1_1),
      Dungeon2Completed = DataManager.Instance.DungeonCompleted(FollowerLocation.Dungeon1_2),
      Dungeon3Completed = DataManager.Instance.DungeonCompleted(FollowerLocation.Dungeon1_3),
      Dungeon4Completed = DataManager.Instance.DungeonCompleted(FollowerLocation.Dungeon1_4),
      PercentageCompleted = 0
    };
    DataManager.Instance.MetaData = metaData;
    SaveAndLoad.SaveMetaData(metaData);
  }

  public static void Load(int saveSlot)
  {
    if (CheatConsole.IN_DEMO)
      return;
    SaveAndLoad.SAVE_SLOT = saveSlot;
    Singleton<SaveAndLoad>.Instance._saveFileReadWriter.Read(SaveAndLoad.MakeSaveSlot(SaveAndLoad.SAVE_SLOT));
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

  private static void DeleteScreenshotInfo(int slot)
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

  private static void LoadMetaData()
  {
    Debug.Log((object) "Load MetaData".Colour(Color.yellow));
    Singleton<SaveAndLoad>.Instance._metaReadWriter.Read(SaveAndLoad.MakeMetaSlot(SaveAndLoad.SAVE_SLOT));
  }

  private static void DeleteMetaData(int saveSlot)
  {
    Debug.Log((object) "Delete MetaData".Colour(Color.yellow));
    Singleton<SaveAndLoad>.Instance._metaReadWriter.Delete(SaveAndLoad.MakeMetaSlot(saveSlot));
  }

  public static string MakeSaveSlot(int slot) => $"slot_{slot}.json";

  public static string MakeMetaSlot(int slot) => $"meta_{slot}.json";
}
