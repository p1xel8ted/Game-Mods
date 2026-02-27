// Decompiled with JetBrains decompiler
// Type: Lamb.UI.BindingPrompt
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Rewired;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class BindingPrompt : MonoBehaviour
{
  [Header("UI")]
  [SerializeField]
  public MMButtonPrompt _buttonPrompt;
  [SerializeField]
  public TextMeshProUGUI _text;
  [Header("Binding")]
  [SerializeField]
  public Platform _platform;
  [SerializeField]
  [ActionIdProperty(typeof (GamepadTemplate))]
  public int _button;
  [SerializeField]
  public KeyboardKeyCode _keyboardKeyCode;
  [SerializeField]
  public bool _predefinedAction;
  [SerializeField]
  [TermsPopup("")]
  public string[] _terms;

  public Platform Platform
  {
    set
    {
      if (this._platform == value)
        return;
      this._platform = value;
      this._buttonPrompt.Platform = this._platform;
    }
  }

  public int Button => this._button;

  public KeyboardKeyCode KeyboardKeyCode => this._keyboardKeyCode;

  public void OnValidate()
  {
    if ((Object) this._buttonPrompt == (Object) null || (Object) this._text == (Object) null)
      return;
    if (this._buttonPrompt.Platform != this._platform)
      this._buttonPrompt.Platform = this._platform;
    if (this._buttonPrompt.KeyboardKeyCode != this._keyboardKeyCode)
      this._buttonPrompt.KeyboardKeyCode = this._keyboardKeyCode;
    if (this._buttonPrompt.Button == this._button)
      return;
    this._buttonPrompt.Button = this._button;
  }

  public void Clear() => this._text.text = string.Empty;

  public void TryAddAction(
    ControllerTemplateElementTarget elementTarget,
    ActionElementMap actionElementMap)
  {
    if (this._predefinedAction || elementTarget.element.id != this._button)
      return;
    this.AddAction(ControlMappings.LocForAction(actionElementMap.actionId));
  }

  public void AddAction(string loc)
  {
    if (string.IsNullOrWhiteSpace(loc))
      return;
    if (string.IsNullOrEmpty(this._text.text))
      this._text.text = loc;
    else
      this._text.text = $"{this._text.text} / {loc}";
  }

  public void FinalizeBinding()
  {
    if (this._predefinedAction)
    {
      foreach (string term in this._terms)
        this.AddAction(LocalizationManager.GetTranslation(term));
    }
    else
    {
      if (!string.IsNullOrEmpty(this._text.text))
        return;
      this._text.text = "--";
    }
  }
}
