// Decompiled with JetBrains decompiler
// Type: Structures_PlacementRegion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Structures_PlacementRegion : StructureBrain
{
  public List<PlacementRegion.ResourcesAndCount> ResourcesToPlace = new List<PlacementRegion.ResourcesAndCount>();
  public bool PlaceWeeds = true;
  public bool PlaceRubble = true;

  public PlacementRegion.TileGridTile GetTileGridTile(Vector2Int Position)
  {
    foreach (PlacementRegion.TileGridTile tileGridTile in this.Data.Grid)
    {
      if (tileGridTile.Position.x == Position.x && tileGridTile.Position.y == Position.y)
        return tileGridTile;
    }
    return (PlacementRegion.TileGridTile) null;
  }

  public StructuresData GetObstructionAtPosition(Vector2Int Position)
  {
    StructuresData obstructionAtPosition = (StructuresData) null;
    foreach (StructuresData structuresData in StructureManager.StructuresDataAtLocation(this.Data.Location))
    {
      if (Position.x >= structuresData.GridTilePosition.x && Position.x < structuresData.GridTilePosition.x + structuresData.Bounds.x && Position.y >= structuresData.GridTilePosition.y && Position.y < structuresData.GridTilePosition.y + structuresData.Bounds.y && structuresData.IsObstruction)
        obstructionAtPosition = structuresData;
    }
    return obstructionAtPosition;
  }

  public StructureBrain GetOccupationAtPosition(Vector2Int Position)
  {
    StructureBrain occupationAtPosition = (StructureBrain) null;
    foreach (StructureBrain allBrain in StructureBrain.AllBrains)
    {
      if (Position.x >= allBrain.Data.GridTilePosition.x && Position.x < allBrain.Data.GridTilePosition.x + allBrain.Data.Bounds.x && Position.y >= allBrain.Data.GridTilePosition.y && Position.y < allBrain.Data.GridTilePosition.y + allBrain.Data.Bounds.y && !allBrain.Data.IsObstruction)
        occupationAtPosition = allBrain;
    }
    return occupationAtPosition;
  }

  public Structures_Weeds GetWeedAtLocation(Vector2Int Position)
  {
    Structures_Weeds weedAtLocation = (Structures_Weeds) null;
    foreach (Structures_Weeds structuresWeeds in StructureManager.GetAllStructuresOfType<Structures_Weeds>())
    {
      Debug.Log((object) $"WEED  {(object) structuresWeeds.Data.GridTilePosition}   {(object) structuresWeeds.Data.Position}   {(object) Position}");
      if (Position.x >= structuresWeeds.Data.GridTilePosition.x && Position.x < structuresWeeds.Data.GridTilePosition.x + structuresWeeds.Data.Bounds.x && Position.y >= structuresWeeds.Data.GridTilePosition.y && Position.y < structuresWeeds.Data.GridTilePosition.y + structuresWeeds.Data.Bounds.y && structuresWeeds.Data.IsObstruction)
        weedAtLocation = structuresWeeds;
    }
    return weedAtLocation;
  }

  public void ClearStructureFromGrid(StructureBrain structure)
  {
    this.ClearStructureFromGrid(structure, structure.Data.GridTilePosition);
  }

  public void ClearStructureFromGrid(StructureBrain structure, Vector2Int gridTilePosition)
  {
    if (this.GetTileGridTile(gridTilePosition) == null)
      return;
    Vector2Int bounds = structure.Data.Bounds;
    int num1 = -1;
    while (++num1 < bounds.x)
    {
      int num2 = -1;
      while (++num2 < bounds.y)
      {
        PlacementRegion.TileGridTile tileGridTile = this.GetTileGridTile(new Vector2Int(gridTilePosition.x + num1, gridTilePosition.y + num2));
        if (tileGridTile != null)
        {
          if (structure.Data.IsObstruction)
            tileGridTile.Obstructed = false;
          else
            tileGridTile.Occupied = false;
          tileGridTile.ReservedForWaste = false;
          if (structure.Data.Type != StructureBrain.TYPES.BUILD_SITE && tileGridTile.ObjectOnTile != StructureBrain.TYPES.BUILD_SITE)
            tileGridTile.OldObjectID = tileGridTile.ObjectID != -1 ? tileGridTile.ObjectID : tileGridTile.OldObjectID;
          tileGridTile.ObjectID = -1;
          tileGridTile.ObjectOnTile = StructureBrain.TYPES.NONE;
        }
      }
    }
    int num3 = -2;
    while (++num3 < bounds.x + 1)
    {
      int num4 = -2;
      while (++num4 < bounds.y + 1)
      {
        PlacementRegion.TileGridTile tileGridTile = this.GetTileGridTile(new Vector2Int(gridTilePosition.x + num3, gridTilePosition.y + num4));
        if (tileGridTile != null)
          tileGridTile.BlockNeighbouringTiles = Mathf.Max(0, tileGridTile.BlockNeighbouringTiles - 1);
      }
    }
  }

  public void AddStructureToGrid(
    StructuresData structure,
    Vector2Int gridTilePosition,
    bool upgrade = false)
  {
    int num1 = -1;
    while (++num1 < structure.Bounds.x)
    {
      int num2 = -1;
      while (++num2 < structure.Bounds.y)
      {
        PlacementRegion.TileGridTile tileGridTile = this.GetTileGridTile(new Vector2Int(gridTilePosition.x + num1, gridTilePosition.y + num2));
        if (tileGridTile != null)
        {
          if (structure.IsObstruction)
            tileGridTile.Obstructed = true;
          else if (!structure.DoesNotOccupyGrid)
            tileGridTile.Occupied = true;
          if (structure.Type != StructureBrain.TYPES.BUILD_SITE && tileGridTile.ObjectOnTile != StructureBrain.TYPES.BUILD_SITE)
            tileGridTile.OldObjectID = tileGridTile.ObjectID != -1 ? tileGridTile.ObjectID : tileGridTile.OldObjectID;
          tileGridTile.ObjectOnTile = structure.Type;
          tileGridTile.ObjectID = structure.ID;
          tileGridTile.IsUpgrade = upgrade;
        }
      }
    }
    switch (structure.Type)
    {
      case StructureBrain.TYPES.TREE:
        break;
      case StructureBrain.TYPES.RUBBLE:
      case StructureBrain.TYPES.WEEDS:
        break;
      case StructureBrain.TYPES.BERRY_BUSH:
        break;
      case StructureBrain.TYPES.RUBBLE_BIG:
        break;
      default:
        int num3 = -2;
        while (++num3 < structure.Bounds.x + 1)
        {
          int num4 = -2;
          while (++num4 < structure.Bounds.y + 1)
          {
            PlacementRegion.TileGridTile tileGridTile = this.GetTileGridTile(new Vector2Int(gridTilePosition.x + num3, gridTilePosition.y + num4));
            if (tileGridTile != null)
              ++tileGridTile.BlockNeighbouringTiles;
          }
        }
        break;
    }
  }

  public void AddStructureToGrid(StructuresData structure, bool upgrade = false)
  {
    this.AddStructureToGrid(structure, structure.GridTilePosition, upgrade);
  }

  public int GetPreviousUpgradeID(StructuresData structure)
  {
    int num1 = -1;
    while (++num1 < structure.Bounds.x)
    {
      int num2 = -1;
      while (++num2 < structure.Bounds.y)
      {
        PlacementRegion.TileGridTile tileGridTile = this.GetTileGridTile(new Vector2Int(structure.GridTilePosition.x + num1, structure.GridTilePosition.y + num2));
        if (tileGridTile != null && tileGridTile.Occupied && tileGridTile.IsUpgrade)
          return tileGridTile.OldObjectID;
      }
    }
    return -1;
  }

  public void MarkObstructionsForClearing(Vector2Int GridPosition, Vector2Int Bounds)
  {
    int num1 = -1;
    while (++num1 < Bounds.x)
    {
      int num2 = -1;
      while (++num2 < Bounds.y)
      {
        Vector2Int Position = new Vector2Int(GridPosition.x + num1, GridPosition.y + num2);
        PlacementRegion.TileGridTile tileGridTile = this.GetTileGridTile(Position);
        if (tileGridTile != null && tileGridTile.Obstructed)
        {
          StructuresData obstructionAtPosition = this.GetObstructionAtPosition(Position);
          if (obstructionAtPosition != null)
            obstructionAtPosition.Prioritised = true;
          else
            tileGridTile.Obstructed = false;
        }
      }
    }
  }
}
