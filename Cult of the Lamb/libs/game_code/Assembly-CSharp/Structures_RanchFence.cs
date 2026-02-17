// Decompiled with JetBrains decompiler
// Type: Structures_RanchFence
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Structures_RanchFence : StructureBrain
{
  public List<int> ConnectedRanchIDs = new List<int>();

  public override void OnAdded()
  {
    base.OnAdded();
    if (this.Data.VariantIndex != 0)
      return;
    this.Data.VariantIndex = Random.Range(1, 4);
    if ((double) Random.value >= 0.20000000298023224)
      return;
    this.Data.VariantIndex += 3;
  }
}
