// Decompiled with JetBrains decompiler
// Type: ForceLTR
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (TextMeshProUGUI))]
public class ForceLTR : MonoBehaviour
{
  public void OnEnable() => this.GetComponent<TextMeshProUGUI>().isRightToLeftText = false;
}
