// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeGamePieceHolder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Flockade;

public abstract class FlockadeGamePieceHolder : FlockadeSelectableBase
{
  [Header("Game Piece Holder Specifics")]
  [SerializeField]
  public FlockadeBlessing _blessing;
  [SerializeField]
  public FlockadeGamePiece _gamePiece;
  [SerializeField]
  public GameObject _identifier;
  [SerializeField]
  public Image[] _inactiveOverlays;
  public Localize _identifierText;
  public FlockadeGamePieceInformation _information;
  public float[] _originInactiveOverlayOpacities;
  [CompilerGenerated]
  public bool \u003CActive\u003Ek__BackingField;
  [CompilerGenerated]
  public FlockadeGamePiece \u003CGamePiece\u003Ek__BackingField;
  [CompilerGenerated]
  public float \u003CHeight\u003Ek__BackingField;

  public bool Active
  {
    get => this.\u003CActive\u003Ek__BackingField;
    set => this.\u003CActive\u003Ek__BackingField = value;
  }

  public FlockadeBlessing Blessing => this._blessing;

  public virtual bool Disabled => !this.Active;

  public virtual FlockadeGamePiece GamePiece
  {
    get => this.\u003CGamePiece\u003Ek__BackingField;
    set => this.\u003CGamePiece\u003Ek__BackingField = value;
  }

  public abstract RectTransform GamePieceContainer { get; }

  public float Height
  {
    get => this.\u003CHeight\u003Ek__BackingField;
    set => this.\u003CHeight\u003Ek__BackingField = value;
  }

  public bool Identified
  {
    set => this._identifier.SetActive(value);
  }

  public string Identifier
  {
    get
    {
      string term = this._identifierText.Term;
      return term[term.Length - 1].ToString();
    }
  }

  public string LocalizedIdentifier
  {
    get => LocalizationManager.GetTranslation(this._identifierText.Term);
  }

  public void Configure(FlockadeGamePieceInformation information)
  {
    this.Configure();
    this._information = information;
    this._gamePiece.Configure(this);
    this.GamePiece = this._gamePiece;
    this._blessing.Bind(this._gamePiece);
  }

  public override void OnLateConfigure()
  {
    this.Height = this.GamePieceContainer.rect.height;
    this.Exit().Complete(true);
  }

  public override void Awake()
  {
    base.Awake();
    this._identifierText = this._identifier.GetComponentInChildren<Localize>(true);
    this.Identified = false;
    this._originInactiveOverlayOpacities = ((IEnumerable<Image>) this._inactiveOverlays).Select<Image, float>((Func<Image, float>) (overlay => overlay.color.a)).ToArray<float>();
  }

  public abstract DG.Tweening.Sequence Enter(bool killOtherAnimations = true);

  public abstract DG.Tweening.Sequence Exit(bool killOtherAnimations = true);

  public override void OnSelect() => this._information.Set(this.GamePiece);

  public override void OnDeselect() => this._information.Set((FlockadeGamePiece) null);

  public DG.Tweening.Sequence SetActive(bool active)
  {
    this.Active = active;
    return this.UpdateInactiveOverlays();
  }

  public DG.Tweening.Sequence UpdateInactiveOverlays()
  {
    DG.Tweening.Sequence s = DOTween.Sequence().AppendInterval(0.0166666675f);
    for (int index = 0; index < this._inactiveOverlays.Length; ++index)
      s.Join((Tween) DOTweenModuleUI.DOFade(this._inactiveOverlays[index], this.Disabled ? this._originInactiveOverlayOpacities[index] : 0.0f, 0.25f).SetEase<TweenerCore<Color, Color, ColorOptions>>(this.Disabled ? Ease.OutCubic : Ease.InCubic));
    return s;
  }
}
