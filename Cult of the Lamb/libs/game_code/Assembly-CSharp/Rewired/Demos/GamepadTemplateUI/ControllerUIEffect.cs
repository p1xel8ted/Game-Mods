// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.GamepadTemplateUI.ControllerUIEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Rewired.Demos.GamepadTemplateUI;

[RequireComponent(typeof (Image))]
public class ControllerUIEffect : MonoBehaviour
{
  [SerializeField]
  public Color _highlightColor = Color.white;
  public Image _image;
  public Color _color;
  public Color _origColor;
  public bool _isActive;
  public float _highlightAmount;

  public void Awake()
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

  public void RedrawImage()
  {
    this._image.color = this._color;
    this._image.enabled = this._isActive;
  }
}
