// Decompiled with JetBrains decompiler
// Type: UILoadingOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI;
using TMPro;
using UnityEngine;

#nullable disable
public class UILoadingOverlayController : UIMenuBase
{
  [SerializeField]
  private TextMeshProUGUI _message;

  public string Message
  {
    get => this._message.text;
    set => this._message.text = value;
  }
}
