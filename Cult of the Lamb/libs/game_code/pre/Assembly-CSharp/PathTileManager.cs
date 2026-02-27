// Decompiled with JetBrains decompiler
// Type: PathTileManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#nullable disable
public class PathTileManager : BaseMonoBehaviour
{
  public static PathTileManager Instance;
  [SerializeField]
  private PathTileManager.TileData[] tiles;
  private GridLayout gridLayout;
  private PlacementRegion placementRegion;

  private void Awake()
  {
    PathTileManager.Instance = this;
    this.gridLayout = this.GetComponent<GridLayout>();
    this.placementRegion = this.GetComponentInParent<PlacementRegion>();
  }

  private void Start()
  {
    if (!((UnityEngine.Object) this.placementRegion != (UnityEngine.Object) null))
      return;
    this.placementRegion.structure.OnBrainAssigned += (System.Action) (() =>
    {
      for (int index = this.placementRegion.structureBrain.Data.pathData.Count - 1; index >= 0; --index)
      {
        if (this.placementRegion.structureBrain.Data.pathData[index].PathID != -1)
        {
          if (!DataManager.Instance.DLC_Cultist_Pack && this.GetTileID(StructureBrain.TYPES.TILE_FLOWERS) == this.placementRegion.structureBrain.Data.pathData[index].PathID)
            this.placementRegion.structureBrain.Data.pathData.RemoveAt(index);
          else
            this.SetTile(this.placementRegion.structureBrain.Data.pathData[index].WorldPosition, this.placementRegion.structureBrain.Data.pathData[index].PathID);
        }
      }
    });
  }

  private void SetTile(Vector3 worldPosition, int tileID)
  {
    PlacementRegion.TileGridTile tileAtWorldPosition = this.placementRegion.GetClosestTileGridTileAtWorldPosition(worldPosition);
    if (tileAtWorldPosition == null)
      return;
    tileAtWorldPosition.PathID = tileID;
    Vector3Int cell = this.gridLayout.WorldToCell(worldPosition);
    this.GetTileMap(this.tiles[tileID].Type).SetTile(cell, (TileBase) this.tiles[tileID].Tile);
  }

  public void SetTile(StructureBrain.TYPES type, Vector3 worldPosition)
  {
    this.DeleteTile(worldPosition);
    PlacementRegion.TileGridTile tileAtWorldPosition = this.placementRegion.GetClosestTileGridTileAtWorldPosition(worldPosition);
    if (tileAtWorldPosition == null)
      return;
    int tileId = this.GetTileID(type);
    tileAtWorldPosition.PathID = tileId;
    this.placementRegion.structureBrain.Data.SetPathData(tileAtWorldPosition.Position, tileAtWorldPosition.WorldPosition, tileId);
    Vector3Int cell = this.gridLayout.WorldToCell(worldPosition);
    this.GetTileMap(this.tiles[tileId].Type).SetTile(cell, (TileBase) this.tiles[tileId].Tile);
  }

  public void ShowPathsBeingBuilt()
  {
    List<Structures_BuildSite> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_BuildSite>();
    if (structuresOfType.Count <= 0)
      return;
    foreach (Structures_BuildSite structuresBuildSite in structuresOfType)
    {
      if (StructureBrain.IsPath(structuresBuildSite.Data.ToBuildType))
        this.DisplayTile(structuresBuildSite.Data.ToBuildType, structuresBuildSite.Data.Position);
    }
  }

  public void HidePathsBeingBuilt()
  {
    List<Structures_BuildSite> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_BuildSite>();
    if (structuresOfType.Count <= 0)
      return;
    foreach (Structures_BuildSite structuresBuildSite in structuresOfType)
    {
      if (StructureBrain.IsPath(structuresBuildSite.Data.ToBuildType))
        this.HideTile(structuresBuildSite.Data.Position);
    }
  }

  public StructureBrain.TYPES GetTileTypeAtPosition(Vector3 worldPosition)
  {
    foreach (PathTileManager.TileData tile1 in this.tiles)
    {
      TileBase tile2 = tile1.TileMap.GetTile(this.gridLayout.WorldToCell(worldPosition));
      if ((UnityEngine.Object) tile2 != (UnityEngine.Object) null)
      {
        int tileId = this.GetTileID(tile2);
        if (tileId != -1)
          return this.tiles[tileId].Type;
      }
    }
    return StructureBrain.TYPES.NONE;
  }

  public GroundType GetTileSoundAtPosition(Vector3 worldPosition)
  {
    foreach (PathTileManager.TileData tile1 in this.tiles)
    {
      TileBase tile2 = tile1.TileMap.GetTile(this.gridLayout.WorldToCell(worldPosition));
      if ((UnityEngine.Object) tile2 != (UnityEngine.Object) null)
      {
        int tileId = this.GetTileID(tile2);
        if (tileId != -1)
          return this.tiles[tileId].sfxSound;
      }
    }
    return GroundType.None;
  }

  public void DeleteTile(Vector3 worldPosition)
  {
    StructureBrain.TYPES Type = StructureBrain.TYPES.NONE;
    PlacementRegion.TileGridTile tileAtWorldPosition = this.placementRegion.GetClosestTileGridTileAtWorldPosition(worldPosition);
    if (tileAtWorldPosition != null)
    {
      tileAtWorldPosition.PathID = -1;
      this.placementRegion.structureBrain.Data.SetPathData(tileAtWorldPosition.Position, tileAtWorldPosition.WorldPosition, -1);
    }
    worldPosition = tileAtWorldPosition.WorldPosition;
    Vector3Int cell = this.gridLayout.WorldToCell(worldPosition);
    foreach (PathTileManager.TileData tile in this.tiles)
      tile.TileMap.SetTile(cell, (TileBase) null);
    if (Type == StructureBrain.TYPES.NONE)
      return;
    for (int index = 0; index < StructuresData.GetCost(Type).Count; ++index)
      Inventory.AddItem((int) StructuresData.GetCost(Type)[index].CostItem, StructuresData.GetCost(Type)[index].CostValue);
  }

  public void DisplayTile(StructureBrain.TYPES type, Vector3 worldPosition)
  {
    int tileId = this.GetTileID(type);
    Vector3Int cell = this.gridLayout.WorldToCell(worldPosition);
    this.GetTileMap(type).SetTile(cell, (TileBase) this.tiles[tileId].Tile);
  }

  public void HideTile(Vector3 worldPosition)
  {
    foreach (PathTileManager.TileData tile in this.tiles)
    {
      Vector3Int cell = this.gridLayout.WorldToCell(worldPosition);
      this.GetTileMap(tile.Type).SetTile(cell, (TileBase) null);
    }
  }

  private Tilemap GetTileMap(StructureBrain.TYPES type)
  {
    foreach (PathTileManager.TileData tile in this.tiles)
    {
      if (tile.Type == type)
        return tile.TileMap;
    }
    return (Tilemap) null;
  }

  public int GetTileID(StructureBrain.TYPES type)
  {
    for (int tileId = 0; tileId < this.tiles.Length; ++tileId)
    {
      if (this.tiles[tileId].Type == type)
        return tileId;
    }
    return -1;
  }

  public int GetTileID(TileBase tile)
  {
    for (int tileId = 0; tileId < this.tiles.Length; ++tileId)
    {
      if ((UnityEngine.Object) this.tiles[tileId].Tile == (UnityEngine.Object) tile)
        return tileId;
    }
    return -1;
  }

  [Serializable]
  public struct TileData
  {
    public StructureBrain.TYPES Type;
    public RuleTile Tile;
    public Tilemap TileMap;
    public GroundType sfxSound;
  }
}
