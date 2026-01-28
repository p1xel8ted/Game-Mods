// Decompiled with JetBrains decompiler
// Type: ForceLTR
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (TextMeshProUGUI))]
public class ForceLTR : MonoBehaviour
{
  public void OnEnable() => this.GetComponent<TextMeshProUGUI>().isRightToLeftText = false;
}
