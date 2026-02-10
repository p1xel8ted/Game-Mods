// Decompiled with JetBrains decompiler
// Type: Flockade.IFlockadePlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
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
