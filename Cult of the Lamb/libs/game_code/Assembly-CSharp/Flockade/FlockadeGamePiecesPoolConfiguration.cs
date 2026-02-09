// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeGamePiecesPoolConfiguration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
namespace Flockade;

[CreateAssetMenu(fileName = "FlockadeGamePiecesPoolConfiguration", menuName = "COTL/Flockade/FlockadeGamePiecesPoolConfiguration")]
public class FlockadeGamePiecesPoolConfiguration : ScriptableObject
{
  public const string _MINIMUM_POOL_SIZE_WARNING = "The pool doesn't contain enought game pieces for a game to be played!";
  [SerializeField]
  public FlockadeGamePiecesPoolConfiguration.GamePieceQuantity[] _pool;
  public bool _isInvalid;

  public IEnumerable<FlockadeGamePieceConfiguration> GetGamePieces()
  {
    FlockadeGamePiecesPoolConfiguration.GamePieceQuantity[] gamePieceQuantityArray = this._pool;
    for (int index1 = 0; index1 < gamePieceQuantityArray.Length; ++index1)
    {
      FlockadeGamePiecesPoolConfiguration.GamePieceQuantity configuration = gamePieceQuantityArray[index1];
      for (int index2 = 0; (long) index2 < (long) configuration.Quantity; ++index2)
        yield return configuration.GamePiece;
      configuration = (FlockadeGamePiecesPoolConfiguration.GamePieceQuantity) null;
    }
    gamePieceQuantityArray = (FlockadeGamePiecesPoolConfiguration.GamePieceQuantity[]) null;
  }

  public IEnumerable<FlockadeGamePieceConfiguration> GetAllPieces()
  {
    return ((IEnumerable<FlockadeGamePiecesPoolConfiguration.GamePieceQuantity>) this._pool).Select<FlockadeGamePiecesPoolConfiguration.GamePieceQuantity, FlockadeGamePieceConfiguration>((Func<FlockadeGamePiecesPoolConfiguration.GamePieceQuantity, FlockadeGamePieceConfiguration>) (configuration => configuration.GamePiece));
  }

  public IEnumerable<FlockadeGamePieceConfiguration> GetPieces(FlockadePieceType kindType)
  {
    return this.GetAllPieces().Where<FlockadeGamePieceConfiguration>((Func<FlockadeGamePieceConfiguration, bool>) (piece => piece.Type.GetKindType() == kindType));
  }

  public IEnumerable<FlockadeGamePieceConfiguration> GetPieces(ICollection<FlockadePieceType> types)
  {
    return this.GetAllPieces().Where<FlockadeGamePieceConfiguration>((Func<FlockadeGamePieceConfiguration, bool>) (piece => types.Contains(piece.Type)));
  }

  public FlockadeGamePieceConfiguration GetPiece(FlockadePieceType type)
  {
    return this.GetAllPieces().FirstOrDefault<FlockadeGamePieceConfiguration>((Func<FlockadeGamePieceConfiguration, bool>) (piece => piece.Type == type));
  }

  [Serializable]
  public class GamePieceQuantity
  {
    public FlockadeGamePieceConfiguration GamePiece;
    public uint Quantity = 1;
  }
}
