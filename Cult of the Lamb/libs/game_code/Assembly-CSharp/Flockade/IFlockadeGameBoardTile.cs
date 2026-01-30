// Decompiled with JetBrains decompiler
// Type: Flockade.IFlockadeGameBoardTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
namespace Flockade;

public interface IFlockadeGameBoardTile
{
  FlockadeVirtualGameBoardTile Core { get; }

  IFlockadeGameBoard GameBoard => this.Core.GameBoard;

  IFlockadeGamePiece GamePiece => this.Core.GamePiece;

  int Index => this.Core.Index;

  bool Locked => this.Core.Locked;

  FlockadeGameBoardSide Side => this.Core.Side;

  IFlockadeBlessing.OnPlacedResult Place() => this.Core.Place();

  IFlockadeBlessing.OnRemovedResult Wipe() => this.Core.Wipe();
}
