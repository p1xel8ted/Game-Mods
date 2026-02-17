// Decompiled with JetBrains decompiler
// Type: FollowerTask_CryptPorter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_CryptPorter(
  int logisticsStructure,
  int rootStructure,
  int targetStructure) : FollowerTask_LogisticsPorter(logisticsStructure, rootStructure, targetStructure)
{
  public int holdingFollowerID = -1;

  public override bool TransportItems => false;

  public override List<InventoryItem> CollectItemsFromStructure(StructureBrain structureBrain)
  {
    if (structureBrain.Data.MultipleFollowerIDs.Count == 0)
      return (List<InventoryItem>) null;
    this.holdingFollowerID = structureBrain.Data.MultipleFollowerIDs[0];
    structureBrain.Data.MultipleFollowerIDs.RemoveAt(0);
    foreach (Interaction_Morgue morgue in Interaction_Morgue.Morgues)
      morgue.UpdateGauge();
    return new List<InventoryItem>();
  }

  public override List<InventoryItem> DepositItems(StructureBrain structureBrain)
  {
    structureBrain.Data.MultipleFollowerIDs.Add(this.holdingFollowerID);
    this.holdingFollowerID = -1;
    foreach (Interaction_Crypt crypt in Interaction_Crypt.Crypts)
      crypt.UpdateGauge();
    return new List<InventoryItem>();
  }

  public override void Loop(List<InventoryItem> leftovers) => base.Loop(leftovers);

  public override void OnEnd()
  {
    base.OnEnd();
    if (this.holdingFollowerID == -1)
      return;
    this.DropBody();
  }

  public override void OnAbort()
  {
    base.OnAbort();
    if (this.holdingFollowerID == -1)
      return;
    this.DropBody();
  }

  public void DropBody()
  {
    StructuresData infoByType = StructuresData.GetInfoByType(StructureBrain.TYPES.DEAD_WORSHIPPER, 0);
    infoByType.Position = this._brain.LastPosition;
    infoByType.BodyWrapped = false;
    infoByType.FollowerID = this.holdingFollowerID;
    this.holdingFollowerID = -1;
    StructureManager.BuildStructure(FollowerLocation.Base, infoByType, this._brain.LastPosition, Vector2Int.one, false, (Action<GameObject>) (g =>
    {
      DeadWorshipper component = g.GetComponent<DeadWorshipper>();
      PlacementRegion.TileGridTile tileAtWorldPosition = PlacementRegion.Instance.GetFreeClosestTileGridTileAtWorldPosition(component.transform.position);
      if (tileAtWorldPosition == null)
        return;
      component.Structure.Brain.AddToGrid(tileAtWorldPosition.Position);
    }));
  }
}
