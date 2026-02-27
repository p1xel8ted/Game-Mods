// Decompiled with JetBrains decompiler
// Type: Lamb.UI.InputIdentifier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Rewired;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

[RequireComponent(typeof (Image))]
[RequireComponent(typeof (CanvasGroup))]
public class InputIdentifier : BaseMonoBehaviour
{
  [SerializeField]
  public ControllerType _controllerType;
  [SerializeField]
  public Platform _platform;
  [SerializeField]
  [ActionIdProperty(typeof (GamepadTemplate))]
  public int _button;
  [SerializeField]
  public KeyboardKeyCode _keyboardKeyCode;
  [SerializeField]
  public MouseInputElement _mouseInputElement;
  [SerializeField]
  public Image _image;
  [SerializeField]
  public CanvasGroup _canvasGroup;

  public int Button => this._button;

  public KeyboardKeyCode KeyboardKeyCode => this._keyboardKeyCode;

  public ControllerType ControllerType => this._controllerType;

  public MouseInputElement MouseInputElement => this._mouseInputElement;

  public void OnValidate()
  {
    if ((Object) this._image == (Object) null)
      this._image = this.GetComponent<Image>();
    if (!((Object) this._canvasGroup == (Object) null))
      return;
    this._canvasGroup = this.GetComponent<CanvasGroup>();
  }

  public void Show(bool instant = false)
  {
    this._canvasGroup.DOKill();
    if (instant)
      this._canvasGroup.alpha = 1f;
    else
      this._canvasGroup.DOFade(1f, 0.1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
  }

  public void Hide(bool instant = false)
  {
    this._canvasGroup.DOKill();
    if (instant)
      this._canvasGroup.alpha = 0.0f;
    else
      this._canvasGroup.DOFade(0.0f, 0.1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
  }
}
