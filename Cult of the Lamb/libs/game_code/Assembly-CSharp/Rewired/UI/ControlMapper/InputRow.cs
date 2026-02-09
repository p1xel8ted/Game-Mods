// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.InputRow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

#nullable disable
namespace Rewired.UI.ControlMapper;

[AddComponentMenu("")]
public class InputRow : MonoBehaviour
{
  public TMP_Text label;
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
