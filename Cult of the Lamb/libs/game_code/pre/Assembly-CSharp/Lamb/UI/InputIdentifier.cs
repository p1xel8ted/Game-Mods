// Decompiled with JetBrains decompiler
// Type: Lamb.UI.InputIdentifier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private ControllerType _controllerType;
  [SerializeField]
  private Platform _platform;
  [SerializeField]
  [ActionIdProperty(typeof (GamepadTemplate))]
  private int _button;
  [SerializeField]
  private KeyboardKeyCode _keyboardKeyCode;
  [SerializeField]
  private MouseInputElement _mouseInputElement;
  [SerializeField]
  private Image _image;
  [SerializeField]
  private CanvasGroup _canvasGroup;

  public int Button => this._button;

  public KeyboardKeyCode KeyboardKeyCode => this._keyboardKeyCode;

  public ControllerType ControllerType => this._controllerType;

  public MouseInputElement MouseInputElement => this._mouseInputElement;

  private void OnValidate()
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
