// Decompiled with JetBrains decompiler
// Type: UI.Keyboards.MMOnScreenKeyboard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using Steamworks;
using UnityEngine;

#nullable disable
namespace UI.Keyboards;

public class MMOnScreenKeyboard : MonoBehaviour
{
  public KeyboardBase _keyboard;

  public void Show(IKeyboardDelegate keyboardDelegate, MMInputField inputField)
  {
    if (SteamUtils.IsSteamRunningOnSteamDeck())
    {
      this._keyboard = (KeyboardBase) new SteamBigPictureKeyboard(keyboardDelegate, inputField);
    }
    else
    {
      if (!SteamUtils.IsSteamInBigPictureMode())
        return;
      this._keyboard = (KeyboardBase) new SteamBigPictureKeyboard(keyboardDelegate, inputField);
    }
  }

  public static bool RequiresOnScreenKeyboard()
  {
    if (!SteamAPI.Init())
      return false;
    return SteamUtils.IsSteamRunningOnSteamDeck() || SteamUtils.IsSteamInBigPictureMode();
  }

  public void Update()
  {
    if (this._keyboard == null)
      return;
    this._keyboard.Update();
  }

  public void OnDestroy() => this._keyboard = (KeyboardBase) null;
}
