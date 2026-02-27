// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeNpcAI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;

#nullable disable
namespace Flockade;

public class FlockadeNpcAI
{
  public FlockadeStrategy[] _strategies;
  public float _randomness;
  public FlockadeStrategy _currentStrategy;

  public FlockadeNpcAI(FlockadeGameBoard gameBoard, FlockadeGameBoardSide side, float randomness)
  {
    this._randomness = randomness;
    this._strategies = new FlockadeStrategy[2]
    {
      (FlockadeStrategy) new FlockadePlaySmartlyStrategy(gameBoard, side),
      (FlockadeStrategy) new FlockadePlayRandomlyStrategy(gameBoard, side)
    };
  }

  public FlockadeGamePieceChoice Select(FlockadeGamePieceChoice[] choices)
  {
    this._currentStrategy = (double) UnityEngine.Random.value > (double) this._randomness ? Array.Find<FlockadeStrategy>(this._strategies, (Predicate<FlockadeStrategy>) (strategy => strategy.IsApplicable(choices))) : Array.FindLast<FlockadeStrategy>(this._strategies, (Predicate<FlockadeStrategy>) (strategy => strategy.IsApplicable(choices)));
    return this._currentStrategy.Select(choices);
  }

  public FlockadeGameBoardTile Place(FlockadeGameBoardTile[] availableTiles)
  {
    return this._currentStrategy.Place(availableTiles);
  }
}
