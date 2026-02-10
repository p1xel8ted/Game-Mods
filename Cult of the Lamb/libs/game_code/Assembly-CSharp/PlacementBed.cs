// Decompiled with JetBrains decompiler
// Type: PlacementBed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PlacementBed : MonoBehaviour
{
  public Interaction_Bed bed;

  public void Update()
  {
    if ((Object) this.bed == (Object) null)
      this.bed = this.GetComponentInChildren<Interaction_Bed>();
    else
      this.bed.UpdateFreezing();
  }
}
