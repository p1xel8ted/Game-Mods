// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MouseAndKeyboard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using src.Extensions;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class MouseAndKeyboard : InputDisplay
{
  [SerializeField]
  private RectTransform _keyboardContainer;
  [SerializeField]
  private RectTransform _mouseContainer;
  private GameObject _keyboard;
  private GameObject _mouse;

  public override void Configure(InputType inputType)
  {
    if ((Object) this._keyboard == (Object) null)
    {
      this._keyboard = MonoSingleton<UIManager>.Instance.KeyboardTemplate.Instantiate<InputController>((Transform) this._keyboardContainer).gameObject;
      this._keyboard.transform.localPosition = Vector3.zero;
      this._keyboard.transform.localScale = Vector3.one;
    }
    if ((Object) this._mouse == (Object) null)
    {
      this._mouse = MonoSingleton<UIManager>.Instance.MouseTemplate.Instantiate<InputController>((Transform) this._mouseContainer).gameObject;
      this._mouse.transform.localPosition = Vector3.zero;
      this._mouse.transform.localScale = Vector3.one;
    }
    this._keyboard.SetActive(inputType == InputType.Keyboard);
    this._mouse.SetActive(inputType == InputType.Keyboard);
  }
}
