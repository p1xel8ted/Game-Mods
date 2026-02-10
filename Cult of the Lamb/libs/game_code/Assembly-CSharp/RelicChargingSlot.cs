// Decompiled with JetBrains decompiler
// Type: RelicChargingSlot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
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
