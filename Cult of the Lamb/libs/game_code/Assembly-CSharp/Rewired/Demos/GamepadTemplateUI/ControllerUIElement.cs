// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.GamepadTemplateUI.ControllerUIElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Rewired.Demos.GamepadTemplateUI;

[RequireComponent(typeof (Image))]
public class ControllerUIElement : MonoBehaviour
{
  [SerializeField]
  public Color _highlightColor = Color.white;
  [SerializeField]
  public ControllerUIEffect _positiveUIEffect;
  [SerializeField]
  public ControllerUIEffect _negativeUIEffect;
  [SerializeField]
  public Text _label;
  [SerializeField]
  public Text _positiveLabel;
  [SerializeField]
  public Text _negativeLabel;
  [SerializeField]
  public ControllerUIElement[] _childElements = new ControllerUIElement[0];
  public Image _image;
  public Color _color;
  public Color _origColor;
  public bool _isActive;
  public float _highlightAmount;

  public bool hasEffects
  {
    get
    {
      return (Object) this._positiveUIEffect != (Object) null || (Object) this._negativeUIEffect != (Object) null;
    }
  }

  public void Awake()
  {
    this._image = this.GetComponent<Image>();
    this._origColor = this._image.color;
    this._color = this._origColor;
    this.ClearLabels();
  }

  public void Activate(float amount)
  {
    amount = Mathf.Clamp(amount, -1f, 1f);
    if (this.hasEffects)
    {
      if ((double) amount < 0.0 && (Object) this._negativeUIEffect != (Object) null)
        this._negativeUIEffect.Activate(Mathf.Abs(amount));
      if ((double) amount > 0.0 && (Object) this._positiveUIEffect != (Object) null)
        this._positiveUIEffect.Activate(Mathf.Abs(amount));
    }
    else
    {
      if (this._isActive && (double) amount == (double) this._highlightAmount)
        return;
      this._highlightAmount = amount;
      this._color = Color.Lerp(this._origColor, this._highlightColor, this._highlightAmount);
    }
    this._isActive = true;
    this.RedrawImage();
    if (this._childElements.Length == 0)
      return;
    for (int index = 0; index < this._childElements.Length; ++index)
    {
      if (!((Object) this._childElements[index] == (Object) null))
        this._childElements[index].Activate(amount);
    }
  }

  public void Deactivate()
  {
    if (!this._isActive)
      return;
    this._color = this._origColor;
    this._highlightAmount = 0.0f;
    if ((Object) this._positiveUIEffect != (Object) null)
      this._positiveUIEffect.Deactivate();
    if ((Object) this._negativeUIEffect != (Object) null)
      this._negativeUIEffect.Deactivate();
    this._isActive = false;
    this.RedrawImage();
    if (this._childElements.Length == 0)
      return;
    for (int index = 0; index < this._childElements.Length; ++index)
    {
      if (!((Object) this._childElements[index] == (Object) null))
        this._childElements[index].Deactivate();
    }
  }

  public void SetLabel(string text, AxisRange labelType)
  {
    Text text1;
    switch (labelType)
    {
      case AxisRange.Full:
        text1 = this._label;
        break;
      case AxisRange.Positive:
        text1 = this._positiveLabel;
        break;
      case AxisRange.Negative:
        text1 = this._negativeLabel;
        break;
      default:
        text1 = (Text) null;
        break;
    }
    if ((Object) text1 != (Object) null)
      text1.text = text;
    if (this._childElements.Length == 0)
      return;
    for (int index = 0; index < this._childElements.Length; ++index)
    {
      if (!((Object) this._childElements[index] == (Object) null))
        this._childElements[index].SetLabel(text, labelType);
    }
  }

  public void ClearLabels()
  {
    if ((Object) this._label != (Object) null)
      this._label.text = string.Empty;
    if ((Object) this._positiveLabel != (Object) null)
      this._positiveLabel.text = string.Empty;
    if ((Object) this._negativeLabel != (Object) null)
      this._negativeLabel.text = string.Empty;
    if (this._childElements.Length == 0)
      return;
    for (int index = 0; index < this._childElements.Length; ++index)
    {
      if (!((Object) this._childElements[index] == (Object) null))
        this._childElements[index].ClearLabels();
    }
  }

  public void RedrawImage() => this._image.color = this._color;
}
