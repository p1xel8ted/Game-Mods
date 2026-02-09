// Decompiled with JetBrains decompiler
// Type: Pathfinding.GraphUpdateScene
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Pathfinding;

[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_graph_update_scene.php")]
[AddComponentMenu("Pathfinding/GraphUpdateScene")]
public class GraphUpdateScene : GraphModifier
{
  public Vector3[] points;
  public Vector3[] convexPoints;
  [HideInInspector]
  public bool convex = true;
  [HideInInspector]
  public float minBoundsHeight = 1f;
  [HideInInspector]
  public int penaltyDelta;
  [HideInInspector]
  public bool modifyWalkability;
  [HideInInspector]
  public bool setWalkability;
  [HideInInspector]
  public bool applyOnStart = true;
  [HideInInspector]
  public bool applyOnScan = true;
  [HideInInspector]
  public bool useWorldSpace;
  [HideInInspector]
  public bool updatePhysics;
  [HideInInspector]
  public bool resetPenaltyOnPhysics = true;
  [HideInInspector]
  public bool updateErosion = true;
  [HideInInspector]
  public bool lockToY;
  [HideInInspector]
  public float lockToYValue;
  [HideInInspector]
  public bool modifyTag;
  [HideInInspector]
  public int setTag;
  public int setTagInvert;
  public bool firstApplied;

  public void Start()
  {
    if (this.firstApplied || !this.applyOnStart)
      return;
    this.Apply();
  }

  public override void OnPostScan()
  {
    if (!this.applyOnScan)
      return;
    this.Apply();
  }

  public virtual void InvertSettings()
  {
    this.setWalkability = !this.setWalkability;
    this.penaltyDelta = -this.penaltyDelta;
    if (this.setTagInvert == 0)
    {
      this.setTagInvert = this.setTag;
      this.setTag = 0;
    }
    else
    {
      this.setTag = this.setTagInvert;
      this.setTagInvert = 0;
    }
  }

  public void RecalcConvex()
  {
    this.convexPoints = this.convex ? Polygon.ConvexHullXZ(this.points) : (Vector3[]) null;
  }

  public void ToggleUseWorldSpace()
  {
    this.useWorldSpace = !this.useWorldSpace;
    if (this.points == null)
      return;
    this.convexPoints = (Vector3[]) null;
    Matrix4x4 matrix4x4 = this.useWorldSpace ? this.transform.localToWorldMatrix : this.transform.worldToLocalMatrix;
    for (int index = 0; index < this.points.Length; ++index)
      this.points[index] = matrix4x4.MultiplyPoint3x4(this.points[index]);
  }

  public void LockToY()
  {
    if (this.points == null)
      return;
    for (int index = 0; index < this.points.Length; ++index)
      this.points[index].y = this.lockToYValue;
  }

  public void Apply(AstarPath active)
  {
    if (!this.applyOnScan)
      return;
    this.Apply();
  }

  public Bounds GetBounds()
  {
    Bounds bounds;
    if (this.points == null || this.points.Length == 0)
    {
      Collider component1 = this.GetComponent<Collider>();
      Renderer component2 = this.GetComponent<Renderer>();
      if ((Object) component1 != (Object) null)
      {
        bounds = component1.bounds;
      }
      else
      {
        if (!((Object) component2 != (Object) null))
          return new Bounds(Vector3.zero, Vector3.zero);
        bounds = component2.bounds;
      }
    }
    else
    {
      Matrix4x4 matrix4x4 = Matrix4x4.identity;
      if (!this.useWorldSpace)
        matrix4x4 = this.transform.localToWorldMatrix;
      Vector3 lhs1 = matrix4x4.MultiplyPoint3x4(this.points[0]);
      Vector3 lhs2 = matrix4x4.MultiplyPoint3x4(this.points[0]);
      for (int index = 0; index < this.points.Length; ++index)
      {
        Vector3 rhs = matrix4x4.MultiplyPoint3x4(this.points[index]);
        lhs1 = Vector3.Min(lhs1, rhs);
        lhs2 = Vector3.Max(lhs2, rhs);
      }
      bounds = new Bounds((lhs1 + lhs2) * 0.5f, lhs2 - lhs1);
    }
    if ((double) bounds.size.y < (double) this.minBoundsHeight)
      bounds.size = new Vector3(bounds.size.x, this.minBoundsHeight, bounds.size.z);
    return bounds;
  }

  public void Apply()
  {
    if ((Object) AstarPath.active == (Object) null)
    {
      Debug.LogError((object) "There is no AstarPath object in the scene");
    }
    else
    {
      GraphUpdateObject ob;
      if (this.points == null || this.points.Length == 0)
      {
        Collider component1 = this.GetComponent<Collider>();
        Renderer component2 = this.GetComponent<Renderer>();
        Bounds bounds;
        if ((Object) component1 != (Object) null)
          bounds = component1.bounds;
        else if ((Object) component2 != (Object) null)
        {
          bounds = component2.bounds;
        }
        else
        {
          Debug.LogWarning((object) "Cannot apply GraphUpdateScene, no points defined and no renderer or collider attached");
          return;
        }
        if ((double) bounds.size.y < (double) this.minBoundsHeight)
          bounds.size = new Vector3(bounds.size.x, this.minBoundsHeight, bounds.size.z);
        ob = new GraphUpdateObject(bounds);
      }
      else
      {
        GraphUpdateShape graphUpdateShape = new GraphUpdateShape();
        graphUpdateShape.convex = this.convex;
        Vector3[] vector3Array = this.points;
        if (!this.useWorldSpace)
        {
          vector3Array = new Vector3[this.points.Length];
          Matrix4x4 localToWorldMatrix = this.transform.localToWorldMatrix;
          for (int index = 0; index < vector3Array.Length; ++index)
            vector3Array[index] = localToWorldMatrix.MultiplyPoint3x4(this.points[index]);
        }
        graphUpdateShape.points = vector3Array;
        Bounds bounds = graphUpdateShape.GetBounds();
        if ((double) bounds.size.y < (double) this.minBoundsHeight)
          bounds.size = new Vector3(bounds.size.x, this.minBoundsHeight, bounds.size.z);
        ob = new GraphUpdateObject(bounds);
        ob.shape = graphUpdateShape;
      }
      this.firstApplied = true;
      ob.modifyWalkability = this.modifyWalkability;
      ob.setWalkability = this.setWalkability;
      ob.addPenalty = this.penaltyDelta;
      ob.updatePhysics = this.updatePhysics;
      ob.updateErosion = this.updateErosion;
      ob.resetPenaltyOnPhysics = this.resetPenaltyOnPhysics;
      ob.modifyTag = this.modifyTag;
      ob.setTag = this.setTag;
      AstarPath.active.UpdateGraphs(ob);
    }
  }

  public void OnDrawGizmos() => this.OnDrawGizmos(false);

  public void OnDrawGizmosSelected() => this.OnDrawGizmos(true);

  public void OnDrawGizmos(bool selected)
  {
    Color a = selected ? new Color(0.8901961f, 0.239215687f, 0.08627451f, 1f) : new Color(0.8901961f, 0.239215687f, 0.08627451f, 0.9f);
    if (selected)
    {
      Gizmos.color = Color.Lerp(a, new Color(1f, 1f, 1f, 0.2f), 0.9f);
      Bounds bounds = this.GetBounds();
      Gizmos.DrawCube(bounds.center, bounds.size);
      Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
    if (this.points == null)
      return;
    if (this.convex)
      a.a *= 0.5f;
    Gizmos.color = a;
    Matrix4x4 matrix4x4 = this.useWorldSpace ? Matrix4x4.identity : this.transform.localToWorldMatrix;
    if (this.convex)
    {
      a.r -= 0.1f;
      a.g -= 0.2f;
      a.b -= 0.1f;
      Gizmos.color = a;
    }
    if (selected || !this.convex)
    {
      for (int index = 0; index < this.points.Length; ++index)
        Gizmos.DrawLine(matrix4x4.MultiplyPoint3x4(this.points[index]), matrix4x4.MultiplyPoint3x4(this.points[(index + 1) % this.points.Length]));
    }
    if (!this.convex)
      return;
    if (this.convexPoints == null)
      this.RecalcConvex();
    Gizmos.color = selected ? new Color(0.8901961f, 0.239215687f, 0.08627451f, 1f) : new Color(0.8901961f, 0.239215687f, 0.08627451f, 0.9f);
    for (int index = 0; index < this.convexPoints.Length; ++index)
      Gizmos.DrawLine(matrix4x4.MultiplyPoint3x4(this.convexPoints[index]), matrix4x4.MultiplyPoint3x4(this.convexPoints[(index + 1) % this.convexPoints.Length]));
  }
}
