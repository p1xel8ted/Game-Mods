// Decompiled with JetBrains decompiler
// Type: RelicChargingSlot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
