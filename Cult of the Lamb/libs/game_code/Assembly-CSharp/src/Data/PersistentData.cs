// Decompiled with JetBrains decompiler
// Type: src.Data.PersistentData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using src.Alerts;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace src.Data;

[Serializable]
public class PersistentData
{
  public bool PostGameRevealed;
  public List<PersistentData.GameCompletionSnapshot> GameCompletionSnapshots = new List<PersistentData.GameCompletionSnapshot>();
  public bool UnlockedSurvivalMode;
  public bool UnlockedWinterMode;
  public bool PlayedSurvivalMode;
  public bool PlayedWinterMode;
  public int PhotoModePictureIndex;
  public List<string> PhotosTakenPaths = new List<string>();
  public Vector2Int ComicPageIndex = Vector2Int.zero;
  public int ComicPanelIndex;
  public bool UnlockedBonusComicPages;
  public List<Vector2Int> ComicExploredPages = new List<Vector2Int>();
  public Vector2Int ComicExploredHistory = Vector2Int.zero;
  public bool RevealedComic;
  public bool RevealedBonusComic;
  public bool RevealedJalalasBag;
  public bool OpenedComic;
  public bool OpenedBonusComic;
  public AchievementAlerts AchievementAlerts = new AchievementAlerts();
  public int PlayedTrailerIndex;

  [Serializable]
  public struct GameCompletionSnapshot
  {
    public DifficultyManager.Difficulty Difficulty;
    public bool Permadeath;
  }
}
