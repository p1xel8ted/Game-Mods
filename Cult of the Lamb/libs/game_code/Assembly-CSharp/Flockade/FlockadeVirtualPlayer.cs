// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeVirtualPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;

#nullable disable
namespace Flockade;

public class FlockadeVirtualPlayer : IFlockadePlayer
{
  [CompilerGenerated]
  public IFlockadeCounter \u003CPoints\u003Ek__BackingField;
  [CompilerGenerated]
  public FlockadeGameBoardSide \u003CSide\u003Ek__BackingField;
  [CompilerGenerated]
  public IFlockadeCounter \u003CVictories\u003Ek__BackingField;

  public FlockadeVirtualPlayer(
    FlockadeGameBoardSide side,
    IFlockadeCounter points,
    IFlockadeCounter victories)
  {
    this.\u003CSide\u003Ek__BackingField = side;
    this.\u003CPoints\u003Ek__BackingField = points;
    this.\u003CVictories\u003Ek__BackingField = victories;
  }

  public FlockadeVirtualPlayer Core => this;

  public IFlockadeCounter Points => this.\u003CPoints\u003Ek__BackingField;

  public FlockadeGameBoardSide Side => this.\u003CSide\u003Ek__BackingField;

  public IFlockadeCounter Victories => this.\u003CVictories\u003Ek__BackingField;

  public FlockadeVirtualPlayer Clone()
  {
    return new FlockadeVirtualPlayer(this.Side, (IFlockadeCounter) new FlockadeVirtualCounter(this.Points.Count), (IFlockadeCounter) new FlockadeVirtualCounter(this.Victories.Count));
  }
}
