// Decompiled with JetBrains decompiler
// Type: UILoadingOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
