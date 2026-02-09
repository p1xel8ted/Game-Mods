// Decompiled with JetBrains decompiler
// Type: RanchFence
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RanchFence : MonoBehaviour
{
  public static List<RanchFence> Fences = new List<RanchFence>();
  [SerializeField]
  public Structure structure;
  [SerializeField]
  public GameObject connectionPoint;
  [SerializeField]
  public AnimationCurve ropeCurve;
  [SerializeField]
  public RanchFence.DirectionalFence[] directionFences;
  [SerializeField]
  public GameObject[] fenceRenderers;
  public int configuredFrame = -1;
  public bool fenceConnected;

  public Structure Structure => this.structure;

  public bool Connected => this.fenceConnected;

  public void Awake()
  {
    RanchFence.Fences.Add(this);
    this.structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
  }

  public void OnDestroy()
  {
    RanchFence.Fences.Remove(this);
    if (!((UnityEngine.Object) this.structure != (UnityEngine.Object) null))
      return;
    this.structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
  }

  public void OnBrainAssigned()
  {
    this.structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    for (int index = 0; index < this.fenceRenderers.Length; ++index)
      this.fenceRenderers[index].gameObject.SetActive(index == this.structure.Brain.Data.VariantIndex - 1);
  }

  public void ConfigureFence()
  {
    if (this.configuredFrame == Time.frameCount)
      return;
    this.configuredFrame = Time.frameCount;
    List<PlacementRegion.TileGridTile> surroundingTiles = this.GetSurroundingTiles();
    PlacementRegion.TileGridTile tileAtWorldPosition = PlacementRegion.Instance.GetClosestTileGridTileAtWorldPosition(this.transform.position);
    int num = 0;
    if (tileAtWorldPosition == null || this.GetConnectionsCount(tileAtWorldPosition, surroundingTiles) < 2)
    {
      this.DisableFence();
    }
    else
    {
      List<Vector2Int> vector2IntList = new List<Vector2Int>(4)
      {
        new Vector2Int(1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0),
        new Vector2Int(0, 1)
      };
      for (int index = 0; index < surroundingTiles.Count; ++index)
      {
        PlacementRegion.TileGridTile tile = surroundingTiles[index];
        GameObject connectionPoint = (GameObject) null;
        bool active = tile != null && (tile.ObjectOnTile == StructureBrain.TYPES.RANCH || tile.ObjectOnTile == StructureBrain.TYPES.RANCH_2 || tile.ObjectOnTile == StructureBrain.TYPES.RANCH_FENCE);
        if (tile != null && tile.ObjectOnTile == StructureBrain.TYPES.RANCH_FENCE)
        {
          RanchFence fenceFromTile = this.GetFenceFromTile(tile);
          if ((UnityEngine.Object) fenceFromTile != (UnityEngine.Object) null)
          {
            connectionPoint = fenceFromTile.connectionPoint;
            ++num;
          }
        }
        else if (tile != null && (tile.ObjectOnTile == StructureBrain.TYPES.RANCH || tile.ObjectOnTile == StructureBrain.TYPES.RANCH_2))
        {
          Interaction_Ranch ranch = this.GetRanch(tile.ObjectID);
          if ((UnityEngine.Object) ranch != (UnityEngine.Object) null && tile.Position != ranch.Structure.Brain.Data.GridTilePosition)
          {
            connectionPoint = ranch.GetClosestConnectionPoint(tileAtWorldPosition.WorldPosition);
            ++num;
          }
        }
        if (tile == null || this.GetConnectionsCount(tile, surroundingTiles) < 2)
          active = false;
        this.SetFenceConnected(vector2IntList[index], active, connectionPoint);
      }
    }
  }

  public List<PlacementRegion.TileGridTile> GetSurroundingTiles()
  {
    List<PlacementRegion.TileGridTile> surroundingTiles = new List<PlacementRegion.TileGridTile>();
    for (int x = -1; x < 2; ++x)
    {
      for (int y = -1; y < 2; ++y)
      {
        if (Mathf.Abs(x) + Mathf.Abs(y) == 1)
        {
          Vector2Int vector2Int = new Vector2Int(x, y);
          PlacementRegion.TileGridTile tileGridTile = PlacementRegion.Instance.GetTileGridTile(this.Structure.Brain.Data.GridTilePosition + vector2Int);
          if (tileGridTile != null)
            surroundingTiles.Add(tileGridTile);
        }
      }
    }
    return surroundingTiles;
  }

  public int GetConnectionsCount(
    PlacementRegion.TileGridTile tile,
    List<PlacementRegion.TileGridTile> surroundingTiles)
  {
    int connectionsCount = 0;
    foreach (PlacementRegion.TileGridTile surroundingTile in surroundingTiles)
    {
      if (surroundingTile != null && surroundingTile.ObjectOnTile == StructureBrain.TYPES.RANCH_FENCE)
      {
        if ((UnityEngine.Object) this.GetFenceFromTile(surroundingTile) != (UnityEngine.Object) null)
          ++connectionsCount;
      }
      else if (surroundingTile != null && (surroundingTile.ObjectOnTile == StructureBrain.TYPES.RANCH || surroundingTile.ObjectOnTile == StructureBrain.TYPES.RANCH_2) && (UnityEngine.Object) this.GetRanch(surroundingTile.ObjectID) != (UnityEngine.Object) null)
        ++connectionsCount;
    }
    return connectionsCount;
  }

  public void SetFenceConnected(Vector2Int direction, bool active, GameObject connectionPoint)
  {
    RanchFence.DirectionalFence directionalFence = (RanchFence.DirectionalFence) null;
    foreach (RanchFence.DirectionalFence directionFence in this.directionFences)
    {
      if (directionFence.Direction == direction)
        directionalFence = directionFence;
    }
    if (directionalFence == null)
      return;
    if (active && (UnityEngine.Object) connectionPoint != (UnityEngine.Object) null)
    {
      Vector3[] ropePoints = this.GetRopePoints(this.connectionPoint.transform.position, connectionPoint.transform.position);
      directionalFence.renderer.positionCount = ropePoints.Length;
      directionalFence.renderer.SetPositions(ropePoints);
      this.fenceConnected = true;
    }
    else
      directionalFence.renderer.positionCount = 0;
  }

  public void DisableFence()
  {
    foreach (RanchFence.DirectionalFence directionFence in this.directionFences)
      directionFence.renderer.positionCount = 0;
    if (this.fenceConnected)
      BiomeConstants.Instance.EmitSmokeInteractionVFX(this.transform.position, Vector3.one);
    this.fenceConnected = false;
  }

  public RanchFence GetFenceFromTile(PlacementRegion.TileGridTile tile)
  {
    foreach (RanchFence fence in RanchFence.Fences)
    {
      if (fence.Structure.Brain != null && fence.Structure.Brain.Data.GridTilePosition == tile.Position)
        return fence;
    }
    return (RanchFence) null;
  }

  public Interaction_Ranch GetRanch(int ranchID)
  {
    foreach (Interaction_Ranch ranch in Interaction_Ranch.Ranches)
    {
      if (ranch.Brain != null && ranch.Brain.Data.ID == ranchID)
        return ranch;
    }
    return (Interaction_Ranch) null;
  }

  public Vector3[] GetRopePoints(Vector3 from, Vector3 to)
  {
    Vector3[] ropePoints = new Vector3[this.ropeCurve.keys.Length];
    for (int index = 0; index < ropePoints.Length; ++index)
    {
      float num = (float) index / (float) (ropePoints.Length - 1);
      Vector3 vector3 = Vector3.Lerp(from, to, num);
      vector3.z += 1f - this.ropeCurve.Evaluate(num);
      ropePoints[index] = vector3;
    }
    return ropePoints;
  }

  [Serializable]
  public class DirectionalFence
  {
    public Vector2Int Direction;
    public LineRenderer renderer;
  }
}
