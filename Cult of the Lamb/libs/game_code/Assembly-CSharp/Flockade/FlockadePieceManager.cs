// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadePieceManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unify;
using UnityEngine;
using UnityEngine.Pool;

#nullable disable
namespace Flockade;

public static class FlockadePieceManager
{
  public const int ALL_FLOCKADE_PIECES_REQUIRED = 36;
  public const float MINIMUM_PIECES_IN_POOL = 12f;
  public const string _POOL_PATH = "Data/Flockade Data/FlockadeGamePiecesPoolConfiguration";
  public static FlockadeGamePiecesPoolConfiguration _piecesPool;

  public static event FlockadePieceManager.FlockadePieceUpdated OnFlockadePieceUnlocked;

  public static FlockadeGamePiecesPoolConfiguration GetPiecesPool()
  {
    if (!(bool) (UnityEngine.Object) FlockadePieceManager._piecesPool)
      FlockadePieceManager._piecesPool = Resources.Load<FlockadeGamePiecesPoolConfiguration>("Data/Flockade Data/FlockadeGamePiecesPoolConfiguration");
    return FlockadePieceManager._piecesPool;
  }

  public static void UnloadPieces()
  {
    Resources.UnloadAsset((UnityEngine.Object) FlockadePieceManager._piecesPool);
    FlockadePieceManager._piecesPool = (FlockadeGamePiecesPoolConfiguration) null;
  }

  public static List<FlockadeGamePieceConfiguration> GetLockedPieceConfigurations()
  {
    return FlockadePieceManager.GetPiecesPool().GetAllPieces().Where<FlockadeGamePieceConfiguration>((Func<FlockadeGamePieceConfiguration, bool>) (piece => !FlockadePieceManager.IsPieceUnlocked(piece.Type))).ToList<FlockadeGamePieceConfiguration>();
  }

  public static int GetUnlockedPiecesCount() => DataManager.Instance.PlayerFoundPieces.Count;

  public static bool IsPieceUnlocked(FlockadePieceType piece)
  {
    return DataManager.Instance.PlayerFoundPieces.Contains(piece);
  }

  public static bool IsAnyPieceOfSameKindUnlocked(FlockadePieceType piece)
  {
    FlockadePieceType kind = piece.GetKindType();
    return DataManager.Instance.PlayerFoundPieces.Any<FlockadePieceType>((Func<FlockadePieceType, bool>) (unlocked => unlocked.GetKindType() == kind));
  }

  public static void UnlockPiece(FlockadePieceType piece)
  {
    if (DataManager.Instance.PlayerFoundPieces.Contains(piece))
      return;
    DataManager.Instance.PlayerFoundPieces.Add(piece);
    int num = 36;
    int unlockedPiecesCount = FlockadePieceManager.GetUnlockedPiecesCount();
    Debug.Log((object) $"Unlocking Flockade Piece {piece}, {unlockedPiecesCount}/{num}");
    FlockadePieceManager.FlockadeAchievementCheck();
    FlockadePieceManager.FlockadePieceUpdated flockadePieceUnlocked = FlockadePieceManager.OnFlockadePieceUnlocked;
    if (flockadePieceUnlocked == null)
      return;
    flockadePieceUnlocked(piece);
  }

  public static void FlockadeAchievementCheck()
  {
    int num = 36;
    if (FlockadePieceManager.GetUnlockedPiecesCount() < num)
      return;
    Debug.Log((object) "Achievement: Defeated Flockade Pieces Collected!");
    AchievementsWrapper.UnlockAchievement(Achievements.Instance.Lookup(AchievementsWrapper.Tags.ALL_FLOCKADE));
  }

  public static Sprite GetFlockadeGamePieceConfiguration(FlockadePieceType piece)
  {
    return FlockadePieceManager.GetPiecesPool().GetPiece(piece).Image;
  }

  public static IEnumerator AwardPieces(IReadOnlyList<FlockadePieceType> piecesIn)
  {
    List<FlockadePieceType> pieces;
    PooledObject<List<FlockadePieceType>> pooledObject = CollectionPool<List<FlockadePieceType>, FlockadePieceType>.Get(out pieces);
    try
    {
      foreach (FlockadePieceType piece in (IEnumerable<FlockadePieceType>) piecesIn)
      {
        if (!FlockadePieceManager.IsPieceUnlocked(piece))
          pieces.Add(piece);
      }
      if (pieces.Count == 0)
        yield break;
      System.Threading.Tasks.Task task = MonoSingleton<UIManager>.Instance.LoadFlockadePiecesAssets();
      yield return (object) new WaitUntil((Func<bool>) (() => task.IsCompleted));
      UIFlockadePiecesMenuController piecesMenuController1 = MonoSingleton<UIManager>.Instance.FlockadePiecesMenuTemplate.Instantiate<UIFlockadePiecesMenuController>();
      piecesMenuController1.Show(pieces, PlayerFarming.Instance);
      foreach (FlockadePieceType flockadePieceType in (IEnumerable<FlockadePieceType>) piecesIn)
        DataManager.Instance.PlayerFoundPieces.Add(flockadePieceType);
      bool waiting = true;
      UIFlockadePiecesMenuController piecesMenuController2 = piecesMenuController1;
      piecesMenuController2.OnHide = piecesMenuController2.OnHide + (System.Action) (() => waiting = false);
      while (waiting)
        yield return (object) null;
    }
    finally
    {
      pooledObject.Dispose();
    }
    pieces = (List<FlockadePieceType>) null;
    pooledObject = new PooledObject<List<FlockadePieceType>>();
    FlockadePieceManager.FlockadeAchievementCheck();
  }

  public delegate void FlockadePieceUpdated(FlockadePieceType piece);
}
