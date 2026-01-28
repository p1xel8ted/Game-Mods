// Decompiled with JetBrains decompiler
// Type: FollowerTask_GravePorter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class FollowerTask_GravePorter(
  int logisticsStructure,
  int rootStructure,
  int targetStructure) : FollowerTask_CryptPorter(logisticsStructure, rootStructure, targetStructure)
{
  public override bool TransportItems => false;

  public override List<InventoryItem> DepositItems(StructureBrain structureBrain)
  {
    structureBrain.Data.FollowerID = this.holdingFollowerID;
    this.holdingFollowerID = -1;
    foreach (Grave grave in Grave.Graves)
      grave.SetGameObjects();
    return new List<InventoryItem>();
  }
}
