// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.InputRow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;

#nullable disable
namespace Rewired.UI.ControlMapper;

[AddComponentMenu("")]
public class InputRow : MonoBehaviour
{
  public TMP_Text label;
  private int rowIndex;
  private Action<int, ButtonInfo> inputFieldActivatedCallback;

  public ButtonInfo[] buttons { get; private set; }

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
