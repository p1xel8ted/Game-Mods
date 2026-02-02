// Decompiled with JetBrains decompiler
// Type: RelicChargingSlot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class RelicChargingSlot : MonoBehaviour
{
  [SerializeField]
  public Image activeIcon;

  public void SetActive(bool active) => this.activeIcon.gameObject.SetActive(active);
}
