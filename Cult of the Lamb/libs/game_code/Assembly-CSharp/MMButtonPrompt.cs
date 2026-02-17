// Decompiled with JetBrains decompiler
// Type: MMButtonPrompt
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Rewired;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class MMButtonPrompt : MonoBehaviour
{
  [Header("Components")]
  [SerializeField]
  public TextMeshProUGUI _text;
  [SerializeField]
  public TextMeshProUGUI _iconText;
  [SerializeField]
  public Image _icon;
  [Header("Settings")]
  [SerializeField]
  public bool initOnLoad;
  [SerializeField]
  public Platform _platform;
  [SerializeField]
  [ActionIdProperty(typeof (GamepadTemplate))]
  public int _button;
  [SerializeField]
  public MMButtonPrompt.KeyboardOrMouse _keyboardOrMouse;
  [SerializeField]
  public MouseInputElement _mouseInputElement;
  [SerializeField]
  public KeyboardKeyCode _keyboardKeyCode;
  [SerializeField]
  public ControlMappings _controlMappings;

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

  public void Awake()
  {
    if (!this.initOnLoad)
      return;
    this.UpdatePrompt();
  }

  public void UpdatePrompt()
  {
    if ((UnityEngine.Object) this._iconText == (UnityEngine.Object) null || (UnityEngine.Object) this._icon == (UnityEngine.Object) null || (UnityEngine.Object) this._text == (UnityEngine.Object) null)
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
        bool isSpecialCharacter = false;
        string str;
        if (this._keyboardOrMouse == MMButtonPrompt.KeyboardOrMouse.Keyboard)
        {
          str = ControlMappings.GetKeyboardCode(this._keyboardKeyCode, out isSpecialCharacter);
        }
        else
        {
          str = ControlMappings.GetMouseCode(this._mouseInputElement, Pole.Positive);
          isSpecialCharacter = true;
        }
        if (!isSpecialCharacter)
        {
          this._text.text = str;
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
          this._iconText.text = str;
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

  [Serializable]
  public enum KeyboardOrMouse
  {
    Keyboard,
    Mouse,
  }
}
