// Decompiled with JetBrains decompiler
// Type: UI.Keyboards.MMOnScreenKeyboard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI;
using Steamworks;
using UnityEngine;

#nullable disable
namespace UI.Keyboards;

public class MMOnScreenKeyboard : MonoBehaviour
{
  private KeyboardBase _keyboard;

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
    return SteamUtils.IsSteamRunningOnSteamDeck() || SteamUtils.IsSteamInBigPictureMode();
  }

  public void Update()
  {
    if (this._keyboard == null)
      return;
    this._keyboard.Update();
  }

  private void OnDestroy() => this._keyboard = (KeyboardBase) null;
}
