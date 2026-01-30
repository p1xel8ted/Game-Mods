// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeVirtualGamePiece
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Flockade;

public class FlockadeVirtualGamePiece : IFlockadeGamePiece
{
  public IFlockadeGamePiece _wrapper;
  [CompilerGenerated]
  public FlockadeVirtualBlessingActivator \u003CBlessing\u003Ek__BackingField;
  [CompilerGenerated]
  public FlockadeGamePieceConfiguration \u003CConfiguration\u003Ek__BackingField;
  [CompilerGenerated]
  public IFlockadeGameBoardTile \u003CTile\u003Ek__BackingField;

  public FlockadeVirtualGamePiece(
    FlockadeGamePieceConfiguration configuration = null,
    FlockadeVirtualBlessingActivator blessing = null,
    IFlockadeGamePiece wrapper = null)
  {
    this._wrapper = wrapper;
    this.Configuration = configuration;
    this.Blessing = blessing ?? this.This.CreateBlessingActivator();
  }

  public FlockadeVirtualBlessingActivator Blessing
  {
    get => this.\u003CBlessing\u003Ek__BackingField;
    set => this.\u003CBlessing\u003Ek__BackingField = value;
  }

  public FlockadeGamePieceConfiguration Configuration
  {
    get => this.\u003CConfiguration\u003Ek__BackingField;
    set => this.\u003CConfiguration\u003Ek__BackingField = value;
  }

  public FlockadeVirtualGamePiece Core => this;

  public IFlockadeGamePiece This => this._wrapper ?? (IFlockadeGamePiece) this;

  public IFlockadeGameBoardTile Tile
  {
    get => this.\u003CTile\u003Ek__BackingField;
    set => this.\u003CTile\u003Ek__BackingField = value;
  }

  public IFlockadeGamePiece.State Copy()
  {
    return new IFlockadeGamePiece.State(this.This.Configuration, this.This.CreateBlessingActivator(this.This.Blessing.Nullified));
  }

  public FlockadeVirtualBlessingActivator CreateBlessingActivator(bool nullified = false)
  {
    return new FlockadeVirtualBlessingActivator(this.This, nullified);
  }

  public FlockadeFight.Result Fight(IFlockadeGamePiece other)
  {
    if (!(bool) (Object) this.This.Configuration || !(bool) (Object) other.Configuration)
      return FlockadeFight.Result.Unknown;
    if (this.This.Configuration.BaseConfiguration.Kind == other.Configuration.BaseConfiguration.Kind)
      return FlockadeFight.Result.Tie;
    return this.This.Configuration.BaseConfiguration.Kind == FlockadeGamePiece.Kind.Shepherd || other.Configuration.BaseConfiguration.Kind != FlockadeGamePiece.Kind.Shepherd && this.This.Configuration.BaseConfiguration.Kind != (FlockadeGamePiece.Kind) ((int) (other.Configuration.BaseConfiguration.Kind + 1) % 3) ? FlockadeFight.Result.Defeat : FlockadeFight.Result.Win;
  }

  public IFlockadeGamePiece.State Get()
  {
    return new IFlockadeGamePiece.State(this.This.Configuration, this.This.Blessing);
  }

  public IFlockadeGamePiece.State Pop()
  {
    IFlockadeGamePiece.State state = this.This.Get();
    state.Blessing.GamePiece = (IFlockadeGamePiece) null;
    this.Configuration = (FlockadeGamePieceConfiguration) null;
    this.Blessing = this.This.CreateBlessingActivator();
    return state;
  }

  public void Set(FlockadeGamePieceConfiguration gamePiece)
  {
    this.This.Set(new IFlockadeGamePiece.State(gamePiece, this.This.CreateBlessingActivator()));
  }

  public void Set(IFlockadeGamePiece.State gamePiece)
  {
    this.Configuration = gamePiece.Configuration;
    gamePiece.Blessing.GamePiece = this.This;
    this.Blessing = gamePiece.Blessing;
  }

  public FlockadeVirtualGamePiece Clone()
  {
    FlockadeVirtualGamePiece gamePiece = new FlockadeVirtualGamePiece(this.Configuration);
    gamePiece.Blessing = new FlockadeVirtualBlessingActivator((IFlockadeGamePiece) gamePiece, this.Blessing.Nullified);
    return gamePiece;
  }
}
