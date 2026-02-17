// Decompiled with JetBrains decompiler
// Type: src.Managers.PersistenceManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using src.Data;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace src.Managers;

public class PersistenceManager : Singleton<PersistenceManager>
{
  public const string kPersistenceFilename = "persistence.json";
  public PersistentData _persistentData;
  public COTLDataReadWriter<PersistentData> _readWriter = new COTLDataReadWriter<PersistentData>();

  public static PersistentData PersistentData
  {
    get => Singleton<PersistenceManager>.Instance._persistentData;
  }

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
  public static void LoadUIManager() => Singleton<PersistenceManager>.Instance.Load();

  public PersistenceManager()
  {
    COTLDataReadWriter<PersistentData> readWriter1 = this._readWriter;
    readWriter1.OnReadCompleted = readWriter1.OnReadCompleted + (Action<PersistentData>) (data => this._persistentData = data);
    COTLDataReadWriter<PersistentData> readWriter2 = this._readWriter;
    readWriter2.OnCreateDefault = readWriter2.OnCreateDefault + new System.Action(this.OnCreateDefault);
  }

  public void Load() => this._readWriter.Read("persistence.json");

  public static void Save()
  {
    if (Singleton<PersistenceManager>.Instance._persistentData == null)
      Debug.LogError((object) "PersistenceManager: Attempted to save persistent data before it was loaded!");
    Singleton<PersistenceManager>.Instance._readWriter.Write(Singleton<PersistenceManager>.Instance._persistentData, "persistence.json", true, true);
  }

  public void OnCreateDefault()
  {
    this._persistentData = new PersistentData();
    this._readWriter.Write(this._persistentData, "persistence.json", true, true);
  }

  public static bool ShowNewGameOptionsMenu()
  {
    return PersistenceManager.PersistentData.GameCompletionSnapshots.Count > 0 || PersistenceManager.PersistentData.PostGameRevealed || PersistenceManager.PersistentData.UnlockedSurvivalMode;
  }

  public static bool HasFinishedGameOnPermadeath()
  {
    foreach (PersistentData.GameCompletionSnapshot completionSnapshot in PersistenceManager.PersistentData.GameCompletionSnapshots)
    {
      if (completionSnapshot.Permadeath)
        return true;
    }
    return false;
  }

  public static DifficultyManager.Difficulty HighestDifficultyCompleted()
  {
    DifficultyManager.Difficulty difficulty = DifficultyManager.Difficulty.Easy;
    foreach (PersistentData.GameCompletionSnapshot completionSnapshot in PersistenceManager.PersistentData.GameCompletionSnapshots)
    {
      if (completionSnapshot.Difficulty > difficulty)
        difficulty = completionSnapshot.Difficulty;
    }
    return difficulty;
  }

  [CompilerGenerated]
  public void \u003C\u002Ector\u003Eb__6_0(PersistentData data) => this._persistentData = data;
}
