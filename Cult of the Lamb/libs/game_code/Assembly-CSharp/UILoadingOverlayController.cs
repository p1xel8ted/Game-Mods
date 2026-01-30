// Decompiled with JetBrains decompiler
// Type: UILoadingOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
