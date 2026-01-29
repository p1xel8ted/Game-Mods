// Decompiled with JetBrains decompiler
// Type: Flockade.IFlockadePlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
namespace Flockade;

public interface IFlockadePlayer
{
  FlockadeVirtualPlayer Core { get; }

  IFlockadeCounter Points => this.Core.Points;

  FlockadeGameBoardSide Side => this.Core.Side;

  IFlockadeCounter Victories => this.Core.Victories;
}
