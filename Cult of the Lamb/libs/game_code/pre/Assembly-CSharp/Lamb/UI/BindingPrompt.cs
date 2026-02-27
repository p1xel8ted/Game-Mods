// Decompiled with JetBrains decompiler
// Type: Lamb.UI.BindingPrompt
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private MMButtonPrompt _buttonPrompt;
  [SerializeField]
  private TextMeshProUGUI _text;
  [Header("Binding")]
  [SerializeField]
  private Platform _platform;
  [SerializeField]
  [ActionIdProperty(typeof (GamepadTemplate))]
  private int _button;
  [SerializeField]
  private KeyboardKeyCode _keyboardKeyCode;
  [SerializeField]
  private bool _predefinedAction;
  [SerializeField]
  [TermsPopup("")]
  private string[] _terms;

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

  private void OnValidate()
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

  private void AddAction(string loc)
  {
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
