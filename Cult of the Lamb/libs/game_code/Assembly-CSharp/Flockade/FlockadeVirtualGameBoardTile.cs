// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeVirtualGameBoardTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Flockade;

public class FlockadeVirtualGameBoardTile : IFlockadeGameBoardTile
{
  public IFlockadeGameBoardTile _wrapper;
  [CompilerGenerated]
  public IFlockadeGameBoard \u003CGameBoard\u003Ek__BackingField;
  [CompilerGenerated]
  public IFlockadeGamePiece \u003CGamePiece\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CIndex\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CLocked\u003Ek__BackingField;
  [CompilerGenerated]
  public FlockadeGameBoardSide \u003CSide\u003Ek__BackingField;

  public FlockadeVirtualGameBoardTile(
    IFlockadeGameBoard gameBoard,
    FlockadeGameBoardSide side,
    int index,
    IFlockadeGamePiece gamePiece,
    IFlockadeGameBoardTile wrapper = null)
  {
    this._wrapper = wrapper;
    this.\u003CGameBoard\u003Ek__BackingField = gameBoard;
    this.\u003CSide\u003Ek__BackingField = side;
    this.\u003CIndex\u003Ek__BackingField = index;
    this.\u003CGamePiece\u003Ek__BackingField = gamePiece;
    gamePiece.Tile = this.This;
    this.Locked = (bool) (Object) gamePiece.Configuration;
  }

  public FlockadeVirtualGameBoardTile Core => this;

  public IFlockadeGameBoard GameBoard => this.\u003CGameBoard\u003Ek__BackingField;

  public IFlockadeGamePiece GamePiece => this.\u003CGamePiece\u003Ek__BackingField;

  public int Index => this.\u003CIndex\u003Ek__BackingField;

  public bool Locked
  {
    get => this.\u003CLocked\u003Ek__BackingField;
    set => this.\u003CLocked\u003Ek__BackingField = value;
  }

  public FlockadeGameBoardSide Side => this.\u003CSide\u003Ek__BackingField;

  public IFlockadeGameBoardTile This => this._wrapper ?? (IFlockadeGameBoardTile) this;

  public IFlockadeBlessing.OnPlacedResult Place()
  {
    this.Locked = true;
    return this.This.GamePiece.Blessing.OnPlaced(this.This);
  }

  public IFlockadeBlessing.OnRemovedResult Wipe()
  {
    IFlockadeBlessing.OnRemovedResult onRemovedResult = this.This.GamePiece.Blessing.OnRemoved(this.This.GamePiece, this.This.Side, this.This.GameBoard);
    this.This.GamePiece.Pop();
    this.Locked = false;
    return onRemovedResult;
  }

  public FlockadeVirtualGameBoardTile Clone(IFlockadeGameBoard gameBoard)
  {
    FlockadeVirtualGamePiece gamePiece = this.GamePiece.Core.Clone();
    return new FlockadeVirtualGameBoardTile(gameBoard, this.Side, this.Index, (IFlockadeGamePiece) gamePiece);
  }
}
