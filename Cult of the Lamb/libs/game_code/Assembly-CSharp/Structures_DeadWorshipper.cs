// Decompiled with JetBrains decompiler
// Type: Structures_DeadWorshipper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Structures_DeadWorshipper : StructureBrain
{
  public List<Structures_PlacementRegion> cachedPlacementRegions = new List<Structures_PlacementRegion>();

  public override void AddToGrid(Vector2Int gridTilePosition)
  {
    StructureManager.TryGetAllStructuresOfType<Structures_PlacementRegion>(ref this.cachedPlacementRegions, this.Data.Location);
    foreach (Structures_PlacementRegion cachedPlacementRegion in this.cachedPlacementRegions)
    {
      PlacementRegion.TileGridTile tileGridTile = cachedPlacementRegion.GetTileGridTile(gridTilePosition);
      Vector3 vector3 = tileGridTile != null ? tileGridTile.WorldPosition : new Vector3((float) gridTilePosition.x, (float) gridTilePosition.y, 0.0f);
      if (PlayerFarming.Location == FollowerLocation.Base)
      {
        PolygonCollider2D polygonCollider2D = (PolygonCollider2D) null;
        if (Follower.Points.Length == 0 && (UnityEngine.Object) BiomeBaseManager.Instance != (UnityEngine.Object) null)
        {
          polygonCollider2D = BiomeBaseManager.Instance.Room.Pieces[0].Collider;
          Follower.Points = polygonCollider2D.GetPath(0);
        }
        if ((UnityEngine.Object) polygonCollider2D != (UnityEngine.Object) null && !Utils.PointWithinPolygon(vector3, Follower.Points) && !polygonCollider2D.OverlapPoint((Vector2) vector3))
        {
          Debug.Log((object) "Body was out of bounds, placing at zero");
          gridTilePosition = Vector2Int.zero;
          foreach (Structure structure in Structure.Structures)
          {
            if (structure.Brain == this)
              structure.transform.position = new Vector3((float) gridTilePosition.x, (float) gridTilePosition.y, 0.0f);
          }
        }
      }
      base.AddToGrid(gridTilePosition);
    }
    this.cachedPlacementRegions.Clear();
  }

  public override void OnAdded()
  {
    base.OnAdded();
    TimeManager.OnNewDayStarted += new System.Action(this.OnNewDayStarted);
  }

  public override void OnRemoved()
  {
    base.OnRemoved();
    TimeManager.OnNewDayStarted -= new System.Action(this.OnNewDayStarted);
  }

  public void OnNewDayStarted()
  {
    ++this.Data.Age;
    if (this.Data.Age < 2 || !this.Data.CanBecomeRotten)
      return;
    this.Data.Rotten = true;
  }
}
