// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMToggle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Lamb.UI;

public class MMToggle : MonoBehaviour
{
  public Action<bool> OnValueChanged;
  [SerializeField]
  public MMButton _button;
  [SerializeField]
  public RectTransform _handle;
  [SerializeField]
  public RectTransform _onPosition;
  [SerializeField]
  public RectTransform _offPosition;
  [SerializeField]
  public CanvasGroup _onGraphic;
  [SerializeField]
  public CanvasGroup _canvasGroup;
  public bool _value = true;

  public bool Value
  {
    get => this._value;
    set
    {
      if (value == this._value)
        return;
      this._value = value;
      this.UpdateState();
    }
  }

  public bool Interactable
  {
    get => this._button.Interactable;
    set
    {
      if (this._button.Interactable == value)
        return;
      this._button.Interactable = value;
      this._canvasGroup.alpha = value ? 1f : 0.5f;
    }
  }

  public MMButton Button => this._button;

  public CanvasGroup CanvasGroup => this._canvasGroup;

  public void Awake() => this._button.onClick.AddListener(new UnityAction(this.DoToggle));

  public void DoToggle()
  {
    this._value = !this._value;
    Action<bool> onValueChanged = this.OnValueChanged;
    if (onValueChanged != null)
      onValueChanged(this._value);
    this.UpdateState();
  }

  public void Toggle() => this.DoToggle();

  public void UpdateState(bool instant = false)
  {
    this._handle.DOKill();
    this._onGraphic.DOKill();
    if (!instant)
    {
      this._handle.DOAnchorPos(this._value ? this._onPosition.anchoredPosition : this._offPosition.anchoredPosition, 0.15f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutSine).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
      this._onGraphic.DOFade((float) this._value.ToInt(), 0.15f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutSine).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    }
    else
    {
      this._handle.anchoredPosition = this._value ? this._onPosition.anchoredPosition : this._offPosition.anchoredPosition;
      this._onGraphic.alpha = (float) this._value.ToInt();
    }
  }
}
