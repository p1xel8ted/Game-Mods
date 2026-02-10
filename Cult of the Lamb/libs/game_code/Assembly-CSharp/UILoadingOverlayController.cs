// Decompiled with JetBrains decompiler
// Type: UILoadingOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using TMPro;
using UnityEngine;

#nullable disable
public class UILoadingOverlayController : UIMenuBase
{
  [SerializeField]
  public TextMeshProUGUI _message;

  public string Message
  {
    get => this._message.text;
    set => this._message.text = value;
  }
}
