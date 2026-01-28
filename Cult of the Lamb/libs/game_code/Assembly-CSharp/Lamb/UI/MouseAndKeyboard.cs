// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MouseAndKeyboard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using src.Extensions;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class MouseAndKeyboard : InputDisplay
{
  [SerializeField]
  public RectTransform _keyboardContainer;
  [SerializeField]
  public RectTransform _mouseContainer;
  public GameObject _keyboard;
  public GameObject _mouse;

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
