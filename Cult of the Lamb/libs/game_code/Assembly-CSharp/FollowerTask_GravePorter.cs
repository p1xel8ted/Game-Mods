// Decompiled with JetBrains decompiler
// Type: FollowerTask_GravePorter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
