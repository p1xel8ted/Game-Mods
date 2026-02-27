// Decompiled with JetBrains decompiler
// Type: MMButtonPrompt
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Rewired;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class MMButtonPrompt : MonoBehaviour
{
  [Header("Components")]
  [SerializeField]
  private TextMeshProUGUI _text;
  [SerializeField]
  private TextMeshProUGUI _iconText;
  [SerializeField]
  private Image _icon;
  [Header("Settings")]
  [SerializeField]
  private Platform _platform;
  [SerializeField]
  [ActionIdProperty(typeof (GamepadTemplate))]
  private int _button;
  [SerializeField]
  private KeyboardKeyCode _keyboardKeyCode;
  [SerializeField]
  private ControlMappings _controlMappings;

  public Platform Platform
  {
    get => this._platform;
    set
    {
      if (this._platform == value)
        return;
      this._platform = value;
      this.UpdatePrompt();
    }
  }

  public int Button
  {
    get => this._button;
    set
    {
      if (this._button == value)
        return;
      this._button = value;
      this.UpdatePrompt();
    }
  }

  public KeyboardKeyCode KeyboardKeyCode
  {
    get => this._keyboardKeyCode;
    set
    {
      if (this._keyboardKeyCode == value)
        return;
      this._keyboardKeyCode = value;
      this.UpdatePrompt();
    }
  }

  private void UpdatePrompt()
  {
    if ((Object) this._iconText == (Object) null || (Object) this._icon == (Object) null || (Object) this._text == (Object) null)
      return;
    if (this._platform != Platform.Undefined)
    {
      this._iconText.font = this._controlMappings.GetFontForPlatform(this._platform);
      if (this._platform != Platform.PC)
      {
        this._iconText.fontSize = 42f;
        this._iconText.enableAutoSizing = false;
        this._iconText.fontStyle = FontStyles.Normal;
        this._icon.gameObject.SetActive(false);
        this._text.gameObject.SetActive(false);
        this._iconText.gameObject.SetActive(true);
        this._iconText.verticalAlignment = VerticalAlignmentOptions.Geometry;
        this._iconText.text = ControlMappings.GetControllerCodeFromID(this._button);
      }
      else
      {
        bool isSpecialCharacter;
        string keyboardCode = ControlMappings.GetKeyboardCode(this._keyboardKeyCode, out isSpecialCharacter);
        if (!isSpecialCharacter)
        {
          this._text.text = keyboardCode;
          this._text.gameObject.SetActive(true);
          this._text.fontSize = 30f;
          this._text.fontSizeMin = 10f;
          this._text.fontSizeMax = 30f;
          this._text.enableAutoSizing = true;
          this._iconText.gameObject.SetActive(false);
          this._iconText.fontStyle = FontStyles.Normal;
          this._icon.gameObject.SetActive(true);
        }
        else
        {
          this._iconText.font = this._controlMappings.GetFontForPlatform(Platform.PC);
          this._iconText.text = keyboardCode;
          this._iconText.fontSize = 70f;
          this._iconText.fontStyle = FontStyles.Bold;
          this._iconText.enableAutoSizing = false;
          this._iconText.gameObject.SetActive(true);
          this._iconText.verticalAlignment = VerticalAlignmentOptions.Geometry;
          this._text.gameObject.SetActive(false);
          this._icon.gameObject.SetActive(false);
        }
      }
    }
    else
    {
      this._iconText.text = "";
      this._text.text = "";
      this._icon.gameObject.SetActive(false);
    }
  }
}
