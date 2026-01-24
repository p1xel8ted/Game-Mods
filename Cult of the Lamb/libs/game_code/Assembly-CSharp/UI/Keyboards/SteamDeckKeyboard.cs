// Decompiled with JetBrains decompiler
// Type: UI.Keyboards.SteamDeckKeyboard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using Steamworks;
using UnityEngine;

#nullable disable
namespace UI.Keyboards;

public class SteamDeckKeyboard : KeyboardBase, IKeyboardDelegate
{
  public MMInputField _inputField;
  public Callback<FloatingGamepadTextInputDismissed_t> _floatingTextInputDismissedCallback;

  public SteamDeckKeyboard(IKeyboardDelegate keyboardDelegate, MMInputField inputField)
  {
    this._inputField = inputField;
    this._keyboardDelegate = keyboardDelegate;
    this._inputField.ActivateInputField();
    SteamUtils.ShowFloatingGamepadTextInput(EFloatingGamepadTextInputMode.k_EFloatingGamepadTextInputModeModeSingleLine, 0, 0, Screen.width, Screen.height / 2);
    this._floatingTextInputDismissedCallback = Callback<FloatingGamepadTextInputDismissed_t>.Create(new Callback<FloatingGamepadTextInputDismissed_t>.DispatchDelegate(this.OnFloatingTextInputDismissed));
  }

  public void KeyboardDismissed(string result) => this._keyboardDelegate.KeyboardDismissed(result);

  public void OnFloatingTextInputDismissed(FloatingGamepadTextInputDismissed_t callback)
  {
    this._inputField.DeactivateInputField();
    if (this._floatingTextInputDismissedCallback == null)
      return;
    this._floatingTextInputDismissedCallback.Dispose();
    this._floatingTextInputDismissedCallback = (Callback<FloatingGamepadTextInputDismissed_t>) null;
  }
}
