// Decompiled with JetBrains decompiler
// Type: src.UI.Items.RelicItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using Lamb.UI.Alerts;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace src.UI.Items;

public class RelicItem : MonoBehaviour
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
  public RelicAlert _alert;
  [SerializeField]
  public Image _flashIcon;
  [SerializeField]
  public Animator _animator;
  [SerializeField]
  public RectTransform _container;
  [CompilerGenerated]
  public RelicData \u003CData\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CLocked\u003Ek__BackingField;

  public RelicData Data
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

  public RelicAlert Alert => this._alert;

  public void Awake() => this._selectable.OnSelected += new System.Action(this.OnSelected);

  public void Configure(RelicData data)
  {
    this.Data = data;
    this._alert.Configure(data.RelicType);
    if (!((UnityEngine.Object) this.Data != (UnityEngine.Object) null))
      return;
    this._icon.sprite = this.Data.Sprite;
    if (DataManager.Instance.PlayerFoundRelics.Contains(data.RelicType))
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
    this.Configure(this.Data);
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
