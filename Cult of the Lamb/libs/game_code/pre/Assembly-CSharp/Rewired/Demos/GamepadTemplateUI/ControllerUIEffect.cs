// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.GamepadTemplateUI.ControllerUIEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Rewired.Demos.GamepadTemplateUI;

[RequireComponent(typeof (Image))]
public class ControllerUIEffect : MonoBehaviour
{
  [SerializeField]
  private Color _highlightColor = Color.white;
  private Image _image;
  private Color _color;
  private Color _origColor;
  private bool _isActive;
  private float _highlightAmount;

  private void Awake()
  {
    this._image = this.GetComponent<Image>();
    this._origColor = this._image.color;
    this._color = this._origColor;
  }

  public void Activate(float amount)
  {
    amount = Mathf.Clamp01(amount);
    if (this._isActive && (double) amount == (double) this._highlightAmount)
      return;
    this._highlightAmount = amount;
    this._color = Color.Lerp(this._origColor, this._highlightColor, this._highlightAmount);
    this._isActive = true;
    this.RedrawImage();
  }

  public void Deactivate()
  {
    if (!this._isActive)
      return;
    this._color = this._origColor;
    this._highlightAmount = 0.0f;
    this._isActive = false;
    this.RedrawImage();
  }

  private void RedrawImage()
  {
    this._image.color = this._color;
    this._image.enabled = this._isActive;
  }
}
