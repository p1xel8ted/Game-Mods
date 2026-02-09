// Decompiled with JetBrains decompiler
// Type: UI.Keyboards.SteamBigPictureKeyboard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using Steamworks;
using System;

#nullable disable
namespace UI.Keyboards;

public class SteamBigPictureKeyboard : KeyboardBase, IKeyboardDelegate
{
  public Callback<GamepadTextInputDismissed_t> _textInputDismissedCallback;

  public SteamBigPictureKeyboard(IKeyboardDelegate keyboardDelegate, MMInputField inputField)
  {
    this._keyboardDelegate = keyboardDelegate;
    SteamUtils.ShowGamepadTextInput(EGamepadTextInputMode.k_EGamepadTextInputModeNormal, EGamepadTextInputLineMode.k_EGamepadTextInputLineModeSingleLine, string.Empty, Convert.ToUInt32(inputField.characterLimit), inputField.text);
    this._textInputDismissedCallback = Callback<GamepadTextInputDismissed_t>.Create(new Callback<GamepadTextInputDismissed_t>.DispatchDelegate(this.OnGamepadTextInputDismissed));
  }

  public void KeyboardDismissed(string result) => this._keyboardDelegate.KeyboardDismissed(result);

  public void OnGamepadTextInputDismissed(GamepadTextInputDismissed_t callback)
  {
    if (!callback.m_bSubmitted)
      return;
    string pchText;
    SteamUtils.GetEnteredGamepadTextInput(out pchText, callback.m_unSubmittedText + 1U);
    this.KeyboardDismissed(pchText);
    if (this._textInputDismissedCallback == null)
      return;
    this._textInputDismissedCallback.Dispose();
    this._textInputDismissedCallback = (Callback<GamepadTextInputDismissed_t>) null;
  }
}
