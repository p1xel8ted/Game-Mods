// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.InputRow
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Rewired.UI.ControlMapper;

[AddComponentMenu("")]
public class InputRow : MonoBehaviour
{
  public Text label;
  [CompilerGenerated]
  public ButtonInfo[] \u003Cbuttons\u003Ek__BackingField;
  public int rowIndex;
  public Action<int, ButtonInfo> inputFieldActivatedCallback;

  public ButtonInfo[] buttons
  {
    get => this.\u003Cbuttons\u003Ek__BackingField;
    set => this.\u003Cbuttons\u003Ek__BackingField = value;
  }

  public void Initialize(
    int rowIndex,
    string label,
    Action<int, ButtonInfo> inputFieldActivatedCallback)
  {
    this.rowIndex = rowIndex;
    this.label.text = label;
    this.inputFieldActivatedCallback = inputFieldActivatedCallback;
    this.buttons = this.transform.GetComponentsInChildren<ButtonInfo>(true);
  }

  public void OnButtonActivated(ButtonInfo buttonInfo)
  {
    if (this.inputFieldActivatedCallback == null)
      return;
    this.inputFieldActivatedCallback(this.rowIndex, buttonInfo);
  }
}
