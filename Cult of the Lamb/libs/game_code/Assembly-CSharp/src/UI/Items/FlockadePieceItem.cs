// Decompiled with JetBrains decompiler
// Type: src.UI.Items.FlockadePieceItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Flockade;
using Lamb.UI;
using Lamb.UI.Alerts;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace src.UI.Items;

public class FlockadePieceItem : MonoBehaviour
{
  public const string kUnlockedLayer = "Unlocked";
  public const string kLockedLayer = "Locked";
  [SerializeField]
  public RectTransform _rectTransform;
  [SerializeField]
  public Image _icon;
  [SerializeField]
  public GameObject _lockedContainer;
  [SerializeField]
  public MMSelectable _selectable;
  [SerializeField]
  public FlockadePieceAlert _alert;
  [SerializeField]
  public Image _flashIcon;
  [SerializeField]
  public Animator _animator;
  [SerializeField]
  public RectTransform _container;
  [SerializeField]
  public Image _outline;
  [CompilerGenerated]
  public FlockadeGamePieceConfiguration \u003CData\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CLocked\u003Ek__BackingField;
  public Color? _highlightColorOverride;
  public Color _originHighlightColor;

  public FlockadeGamePieceConfiguration Data
  {
    set => this.\u003CData\u003Ek__BackingField = value;
    get => this.\u003CData\u003Ek__BackingField;
  }

  public bool Locked
  {
    set => this.\u003CLocked\u003Ek__BackingField = value;
    get => this.\u003CLocked\u003Ek__BackingField;
  }

  public RectTransform RectTransform => this._rectTransform;

  public MMSelectable Selectable => this._selectable;

  public FlockadePieceAlert Alert => this._alert;

  public void Awake()
  {
    this._originHighlightColor = this._outline.color;
    this._selectable.OnSelected += new Action(this.OnSelected);
  }

  public void Configure(FlockadeGamePieceConfiguration data, Color? highlightColorOverride = null)
  {
    this.Data = data;
    this._highlightColorOverride = highlightColorOverride;
    if (!(bool) (UnityEngine.Object) this.Data)
      return;
    this._outline.color = highlightColorOverride ?? this._originHighlightColor;
    this._alert.Configure(data.Type);
    this._icon.sprite = this.Data.Image;
    if (FlockadePieceManager.IsPieceUnlocked(data.Type))
      return;
    this.Locked = true;
    this.SetAsLocked();
  }

  public void SetAsLocked()
  {
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Unlocked"), 0.0f);
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Locked"), 1f);
  }

  public void ForceIncognitoState()
  {
    this._alert.gameObject.SetActive(false);
    this._icon.color = new Color(0.0f, 1f, 1f, 1f);
    this._flashIcon.gameObject.SetActive(true);
    this._flashIcon.color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
  }

  public void ForceLockedState()
  {
    this.SetAsLocked();
    this._alert.gameObject.SetActive(false);
  }

  public IEnumerator DoUnlock()
  {
    yield return (object) this.Flash();
    yield return (object) this.ShowAlert();
  }

  public IEnumerator Flash()
  {
    this._container.DOScale(Vector3.one * 0.75f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuad).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.25f);
    this.Configure(this.Data, this._highlightColorOverride);
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Unlocked"), 1f);
    this._animator.SetLayerWeight(this._animator.GetLayerIndex("Locked"), 0.0f);
    this._icon.color = new Color(1f, 1f, 1f, 1f);
    this._flashIcon.gameObject.SetActive(true);
    this._alert.gameObject.SetActive(false);
    UIManager.PlayAudio("event:/unlock_building/unlock");
    this._container.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.25f);
    DOTweenModuleUI.DOColor(this._flashIcon, new Color(1f, 1f, 1f, 0.0f), 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.25f);
    yield return (object) new WaitForSecondsRealtime(0.1f);
    this.Locked = false;
  }

  public IEnumerator ShowAlert()
  {
    Vector3 one = Vector3.one;
    this._alert.transform.localScale = Vector3.zero;
    this._alert.transform.DOScale(one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBounce).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this._alert.gameObject.SetActive(true);
    yield return (object) new WaitForSecondsRealtime(0.5f);
  }

  public void OnSelected() => this._alert.TryRemoveAlert();
}
