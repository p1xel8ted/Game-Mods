// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeBlessingActivator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Flockade;

public class FlockadeBlessingActivator(FlockadeGamePiece gamePiece, bool nullified = false) : 
  FlockadeVirtualBlessingActivator((IFlockadeGamePiece) gamePiece, nullified)
{
  public const string _BLESSING_ACTIVATION_SOUND = "event:/dlc/ui/flockade_minigame/piece_blessing_activate";
  public const string _BLESSING_DEACTIVATION_SOUND = "event:/dlc/ui/flockade_minigame/piece_blessing_deactivate";
  [CompilerGenerated]
  public bool \u003CConsumed\u003Ek__BackingField;

  public FlockadeGamePiece GamePiece => (FlockadeGamePiece) base.GamePiece;

  public bool Consumed
  {
    get => this.\u003CConsumed\u003Ek__BackingField;
    set => this.\u003CConsumed\u003Ek__BackingField = value;
  }

  public override Sequence Nullify()
  {
    if (!(bool) (FlockadeVirtualBlessingActivator) this || this.Nullified)
      return (Sequence) null;
    Sequence s = DOTween.Sequence().AppendCallback((TweenCallback) (() => AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/piece_blessing_deactivate"))).AppendInterval(0.0166666675f);
    Sequence t = base.Nullify();
    if (t != null)
      s.Join((Tween) t);
    FlockadeGameBoardTile from = this.GamePiece.Tile;
    IFlockadeGamePiece.State gamePiece = this.GamePiece.Get();
    if ((bool) (Object) this.GamePiece.Holder)
    {
      s.Join((Tween) this.GamePiece.Holder.Exit().PrependCallback((TweenCallback) (() =>
      {
        if (!(bool) (Object) from)
          return;
        FlockadeGameBoard.Evaluate(from);
      })).AppendCallback((TweenCallback) (() =>
      {
        if ((bool) (Object) from)
          FlockadeGameBoard.Evaluate(from);
        this.GamePiece.UpdateVisuals(gamePiece);
      })).Append((Tween) this.GamePiece.Holder.Enter(false))).Join((Tween) this.GamePiece.Holder.Blessing.Nullify());
    }
    else
    {
      if ((bool) (Object) from)
        FlockadeGameBoard.Evaluate(from);
      this.GamePiece.UpdateVisuals(gamePiece);
    }
    return s;
  }

  public Sequence Activate() => this.Activate(false);

  public Sequence Activate(bool consume)
  {
    if (consume)
      this.Consumed = true;
    FlockadeBlessingActivator.State blessing = this.Get();
    List<Sequence> sequenceList = new List<Sequence>();
    if ((bool) (Object) this.GamePiece.Tile)
    {
      foreach (FlockadePlayerBase player in this.GamePiece.Tile.GameBoard.GetPlayers())
      {
        FlockadeBlessing boundBlessing = player.GetBoundBlessing(this);
        if ((bool) (Object) boundBlessing)
          sequenceList.Add(boundBlessing.Activate(blessing));
      }
    }
    if ((bool) (Object) this.GamePiece.Holder)
      sequenceList.Add(this.GamePiece.Holder.Blessing.Activate(blessing));
    return FlockadeUtils.Combine((IEnumerable<Tween>) sequenceList).PrependCallback((TweenCallback) (() => AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/piece_blessing_activate")));
  }

  public FlockadeBlessingActivator.State Get()
  {
    return new FlockadeBlessingActivator.State(this.GamePiece.Configuration.BlessingConfiguration, this);
  }

  public struct State(
    FlockadeBlessingConfiguration configuration,
    FlockadeBlessingActivator activator)
  {
    [CompilerGenerated]
    public FlockadeBlessingConfiguration \u003CConfiguration\u003Ek__BackingField = configuration;
    [CompilerGenerated]
    public FlockadeBlessingActivator \u003CActivator\u003Ek__BackingField = activator;

    public readonly FlockadeBlessingConfiguration Configuration
    {
      get => this.\u003CConfiguration\u003Ek__BackingField;
    }

    public readonly FlockadeBlessingActivator Activator
    {
      get => this.\u003CActivator\u003Ek__BackingField;
    }
  }
}
