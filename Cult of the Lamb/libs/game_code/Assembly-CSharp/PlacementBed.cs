// Decompiled with JetBrains decompiler
// Type: PlacementBed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
