// Decompiled with JetBrains decompiler
// Type: Structures_Furnace
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Structures_Furnace : StructureBrain
{
  public override void OnAdded()
  {
    base.OnAdded();
    DataManager.Instance.HasFurnace = true;
  }

  public override void OnRemoved()
  {
    base.OnRemoved();
    DataManager.Instance.HasFurnace = false;
  }

  public override void OnSeasonChanged(SeasonsManager.Season season)
  {
    base.OnSeasonChanged(season);
    if (season != SeasonsManager.Season.Winter || !UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Upgrade_Furnace_Full))
      return;
    this.Data.Fuel = this.Data.MaxFuel;
    this.UpdateFuel(0);
  }

  public void SpawnWaste()
  {
    int num = 10;
    List<PlacementRegion.TileGridTile> tileGridTileList = new List<PlacementRegion.TileGridTile>();
    for (int x = -num; x < num; ++x)
    {
      for (int y = -num; y < num; ++y)
      {
        PlacementRegion.TileGridTile tileGridTile = PlacementRegion.Instance.GetTileGridTile(this.Data.GridTilePosition + new Vector2Int(x, y));
        if (tileGridTile != null && tileGridTile.CanPlaceObstruction)
          tileGridTileList.Add(tileGridTile);
      }
    }
    PlacementRegion.TileGridTile tileGridTile1 = (PlacementRegion.TileGridTile) null;
    for (int index = 0; index < tileGridTileList.Count; ++index)
    {
      if (tileGridTile1 == null || (double) Vector3.Distance(this.Data.Position, tileGridTileList[index].WorldPosition) < (double) Vector3.Distance(this.Data.Position, tileGridTile1.WorldPosition))
        tileGridTile1 = tileGridTileList[index];
    }
    if (tileGridTile1 == null)
      return;
    StructuresData infoByType = StructuresData.GetInfoByType(StructureBrain.TYPES.TOXIC_WASTE, 0);
    infoByType.GridTilePosition = tileGridTile1.Position;
    PlacementRegion.Instance.structureBrain.AddStructureToGrid(infoByType, tileGridTile1.Position);
    StructureManager.BuildStructure(FollowerLocation.Base, infoByType, tileGridTile1.WorldPosition, Vector2Int.one);
  }
}
