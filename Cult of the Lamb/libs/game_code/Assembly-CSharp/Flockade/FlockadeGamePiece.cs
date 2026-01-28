// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeGamePiece
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Flockade;

public class FlockadeGamePiece : MonoBehaviour, IFlockadeGamePiece
{
  [SerializeField]
  public Image _image;
  public RectTransform _rectTransform;
  public FlockadeGamePiece.Stance _stance;
  [CompilerGenerated]
  public FlockadeVirtualGamePiece \u003CCore\u003Ek__BackingField;
  [CompilerGenerated]
  public FlockadeGamePieceHolder \u003CHolder\u003Ek__BackingField;

  public event Action<IFlockadeGamePiece.State?> Changed;

  public FlockadeVirtualGamePiece Core
  {
    get => this.\u003CCore\u003Ek__BackingField;
    set => this.\u003CCore\u003Ek__BackingField = value;
  }

  public FlockadeGamePieceHolder Holder
  {
    get => this.\u003CHolder\u003Ek__BackingField;
    set => this.\u003CHolder\u003Ek__BackingField = value;
  }

  public Image Image => this._image;

  public RectTransform RectTransform
  {
    get
    {
      if (!(bool) (UnityEngine.Object) this._rectTransform)
        this._rectTransform = this.GetComponent<RectTransform>();
      return this._rectTransform;
    }
  }

  public void Configure(FlockadeGamePieceHolder holder = null)
  {
    this.Core = new FlockadeVirtualGamePiece(wrapper: (IFlockadeGamePiece) this);
    this.Holder = holder;
  }

  FlockadeVirtualBlessingActivator IFlockadeGamePiece.CreateBlessingActivator(bool nullified)
  {
    return (FlockadeVirtualBlessingActivator) new FlockadeBlessingActivator(this, nullified);
  }

  IFlockadeGamePiece.State IFlockadeGamePiece.Pop()
  {
    IFlockadeGamePiece.State gamePiece;
    this.Pop(out gamePiece);
    return gamePiece;
  }

  public Sequence Pop(out IFlockadeGamePiece.State gamePiece, bool killOtherAnimations = true)
  {
    FlockadeGameBoardTile from = this.Tile;
    gamePiece = this.Core.Pop();
    this._stance = FlockadeGamePiece.Stance.Normal;
    if ((bool) (UnityEngine.Object) this.Holder)
      return this.Holder.Exit(killOtherAnimations).PrependCallback((TweenCallback) (() =>
      {
        Action<IFlockadeGamePiece.State?> changed = this.Changed;
        if (changed != null)
          changed(new IFlockadeGamePiece.State?());
        if (!(bool) (UnityEngine.Object) from)
          return;
        FlockadeGameBoard.Evaluate(from);
      })).AppendCallback((TweenCallback) (() => this.UpdateVisuals()));
    Action<IFlockadeGamePiece.State?> changed1 = this.Changed;
    if (changed1 != null)
      changed1(new IFlockadeGamePiece.State?());
    if ((bool) (UnityEngine.Object) from)
      FlockadeGameBoard.Evaluate(from);
    this.UpdateVisuals();
    return (Sequence) null;
  }

  void IFlockadeGamePiece.Set(IFlockadeGamePiece.State gamePiece) => this.Set(gamePiece, true);

  public Sequence Set(FlockadeGamePieceConfiguration gamePiece, bool killOtherAnimations)
  {
    return this.Set(new IFlockadeGamePiece.State(gamePiece, (FlockadeVirtualBlessingActivator) this.CreateBlessingActivator(false)), killOtherAnimations);
  }

  public Sequence Set(IFlockadeGamePiece.State gamePiece, bool killOtherAnimations)
  {
    FlockadeGameBoardTile from = this.Tile;
    this.Core.Set(gamePiece);
    if ((bool) (UnityEngine.Object) this.Holder)
      return this.Holder.Enter(killOtherAnimations).PrependCallback((TweenCallback) (() =>
      {
        Action<IFlockadeGamePiece.State?> changed = this.Changed;
        if (changed != null)
          changed(new IFlockadeGamePiece.State?(gamePiece));
        if ((bool) (UnityEngine.Object) from)
          FlockadeGameBoard.Evaluate(from);
        this.UpdateVisuals(gamePiece);
      }));
    Action<IFlockadeGamePiece.State?> changed1 = this.Changed;
    if (changed1 != null)
      changed1(new IFlockadeGamePiece.State?(gamePiece));
    if ((bool) (UnityEngine.Object) from)
      FlockadeGameBoard.Evaluate(from);
    this.UpdateVisuals(gamePiece);
    return (Sequence) null;
  }

  public void SetStance(FlockadeGamePiece.Stance stance)
  {
    if (this._stance == stance)
      return;
    this._stance = stance;
    this.UpdateVisuals(this.Get());
  }

  public void UpdateVisuals(IFlockadeGamePiece.State gamePiece = default (IFlockadeGamePiece.State))
  {
    if (!(bool) (UnityEngine.Object) gamePiece.Configuration)
    {
      this._image.sprite = (Sprite) null;
      this._image.color = Color.clear;
    }
    else
    {
      Image image = this._image;
      Sprite sprite;
      switch (this._stance)
      {
        case FlockadeGamePiece.Stance.Attacking:
          sprite = gamePiece.Blessing.Nullified ? gamePiece.Configuration.BaseConfiguration.AttackingImage : gamePiece.Configuration.AttackingImage;
          break;
        case FlockadeGamePiece.Stance.Flinching:
          sprite = gamePiece.Blessing.Nullified ? gamePiece.Configuration.BaseConfiguration.FlinchingImage : gamePiece.Configuration.FlinchingImage;
          break;
        default:
          sprite = gamePiece.Blessing.Nullified ? gamePiece.Configuration.BaseConfiguration.Image : gamePiece.Configuration.Image;
          break;
      }
      image.sprite = sprite;
      this._image.color = Color.white;
    }
  }

  public FlockadeBlessingActivator Blessing => this.Core.Blessing as FlockadeBlessingActivator;

  public FlockadeGamePieceConfiguration Configuration => this.Core.Configuration;

  public FlockadeGameBoardTile Tile => this.Core.Tile as FlockadeGameBoardTile;

  public IFlockadeGamePiece.State Copy() => this.Core.Copy();

  public FlockadeBlessingActivator CreateBlessingActivator(bool nullified = false)
  {
    return ((IFlockadeGamePiece) this).CreateBlessingActivator(nullified) as FlockadeBlessingActivator;
  }

  public FlockadeFight.Result Fight(IFlockadeGamePiece other) => this.Core.Fight(other);

  public IFlockadeGamePiece.State Get() => this.Core.Get();

  public IFlockadeGamePiece.State Pop() => ((IFlockadeGamePiece) this).Pop();

  public void Set(FlockadeGamePieceConfiguration gamePiece) => this.Core.Set(gamePiece);

  public void Set(IFlockadeGamePiece.State gamePiece) => ((IFlockadeGamePiece) this).Set(gamePiece);

  public enum Kind
  {
    Shield,
    Scribe,
    Sword,
    Shepherd,
  }

  public enum Stance
  {
    Normal,
    Attacking,
    Flinching,
  }
}
