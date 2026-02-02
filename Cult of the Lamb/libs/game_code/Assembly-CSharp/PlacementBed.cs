// Decompiled with JetBrains decompiler
// Type: PlacementBed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
