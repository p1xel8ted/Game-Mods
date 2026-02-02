// Decompiled with JetBrains decompiler
// Type: ForceLTR
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (TextMeshProUGUI))]
public class ForceLTR : MonoBehaviour
{
  public void OnEnable() => this.GetComponent<TextMeshProUGUI>().isRightToLeftText = false;
}
