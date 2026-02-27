// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMToggle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private MMButton _button;
  [SerializeField]
  private RectTransform _handle;
  [SerializeField]
  private RectTransform _onPosition;
  [SerializeField]
  private RectTransform _offPosition;
  [SerializeField]
  private CanvasGroup _onGraphic;
  [SerializeField]
  private CanvasGroup _canvasGroup;
  private bool _value = true;

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

  public MMButton Button => this._button;

  public CanvasGroup CanvasGroup => this._canvasGroup;

  private void Awake() => this._button.onClick.AddListener(new UnityAction(this.DoToggle));

  private void DoToggle()
  {
    this._value = !this._value;
    Action<bool> onValueChanged = this.OnValueChanged;
    if (onValueChanged != null)
      onValueChanged(this._value);
    this.UpdateState();
  }

  public void Toggle() => this.DoToggle();

  private void UpdateState(bool instant = false)
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
