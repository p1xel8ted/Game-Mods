// Decompiled with JetBrains decompiler
// Type: Flockade.IFlockadeGameBoardTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
