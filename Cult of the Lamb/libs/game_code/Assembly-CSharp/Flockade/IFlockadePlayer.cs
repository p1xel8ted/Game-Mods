// Decompiled with JetBrains decompiler
// Type: Flockade.IFlockadePlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
