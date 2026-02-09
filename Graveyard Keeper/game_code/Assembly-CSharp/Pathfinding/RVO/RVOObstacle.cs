// Decompiled with JetBrains decompiler
// Type: Pathfinding.RVO.RVOObstacle
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Pathfinding.RVO;

public abstract class RVOObstacle : MonoBehaviour
{
  public RVOObstacle.ObstacleVertexWinding obstacleMode;
  public RVOLayer layer = RVOLayer.DefaultObstacle;
  public Simulator sim;
  public List<ObstacleVertex> addedObstacles;
  public List<Vector3[]> sourceObstacles;
  public bool gizmoDrawing;
  public List<Vector3[]> gizmoVerts;
  public RVOObstacle.ObstacleVertexWinding _obstacleMode;
  public Matrix4x4 prevUpdateMatrix;

  public abstract void CreateObstacles();

  public abstract bool ExecuteInEditor { get; }

  public abstract bool LocalCoordinates { get; }

  public abstract bool StaticObstacle { get; }

  public abstract float Height { get; }

  public abstract bool AreGizmosDirty();

  public void OnDrawGizmos() => this.OnDrawGizmos(false);

  public void OnDrawGizmosSelected() => this.OnDrawGizmos(true);

  public void OnDrawGizmos(bool selected)
  {
    this.gizmoDrawing = true;
    Gizmos.color = new Color(0.615f, 1f, 0.06f, selected ? 1f : 0.7f);
    if (this.gizmoVerts == null || this.AreGizmosDirty() || this._obstacleMode != this.obstacleMode)
    {
      this._obstacleMode = this.obstacleMode;
      if (this.gizmoVerts == null)
        this.gizmoVerts = new List<Vector3[]>();
      else
        this.gizmoVerts.Clear();
      this.CreateObstacles();
    }
    Matrix4x4 matrix = this.GetMatrix();
    for (int index1 = 0; index1 < this.gizmoVerts.Count; ++index1)
    {
      Vector3[] gizmoVert = this.gizmoVerts[index1];
      int index2 = 0;
      int index3 = gizmoVert.Length - 1;
      for (; index2 < gizmoVert.Length; index3 = index2++)
        Gizmos.DrawLine(matrix.MultiplyPoint3x4(gizmoVert[index2]), matrix.MultiplyPoint3x4(gizmoVert[index3]));
      if (selected)
      {
        int index4 = 0;
        int index5 = gizmoVert.Length - 1;
        for (; index4 < gizmoVert.Length; index5 = index4++)
        {
          Gizmos.DrawLine(matrix.MultiplyPoint3x4(gizmoVert[index4]) + Vector3.up * this.Height, matrix.MultiplyPoint3x4(gizmoVert[index5]) + Vector3.up * this.Height);
          Gizmos.DrawLine(matrix.MultiplyPoint3x4(gizmoVert[index4]), matrix.MultiplyPoint3x4(gizmoVert[index4]) + Vector3.up * this.Height);
        }
        int index6 = 0;
        int index7 = gizmoVert.Length - 1;
        for (; index6 < gizmoVert.Length; index7 = index6++)
        {
          Vector3 vector3_1 = matrix.MultiplyPoint3x4(gizmoVert[index7]);
          Vector3 vector3_2 = matrix.MultiplyPoint3x4(gizmoVert[index6]);
          Vector3 from = (vector3_1 + vector3_2) * 0.5f;
          Vector3 normalized = (vector3_2 - vector3_1).normalized;
          if (!(normalized == Vector3.zero))
          {
            Vector3 vector3_3 = Vector3.Cross(Vector3.up, normalized);
            Gizmos.DrawLine(from, from + vector3_3);
            Gizmos.DrawLine(from + vector3_3, from + vector3_3 * 0.5f + normalized * 0.5f);
            Gizmos.DrawLine(from + vector3_3, from + vector3_3 * 0.5f - normalized * 0.5f);
          }
        }
      }
    }
    this.gizmoDrawing = false;
  }

  public virtual Matrix4x4 GetMatrix()
  {
    return !this.LocalCoordinates ? Matrix4x4.identity : this.transform.localToWorldMatrix;
  }

  public void OnDisable()
  {
    if (this.addedObstacles == null)
      return;
    if (this.sim == null)
      throw new Exception("This should not happen! Make sure you are not overriding the OnEnable function");
    for (int index = 0; index < this.addedObstacles.Count; ++index)
      this.sim.RemoveObstacle(this.addedObstacles[index]);
  }

  public void OnEnable()
  {
    if (this.addedObstacles == null)
      return;
    if (this.sim == null)
      throw new Exception("This should not happen! Make sure you are not overriding the OnDisable function");
    for (int index = 0; index < this.addedObstacles.Count; ++index)
    {
      ObstacleVertex obstacleVertex1 = this.addedObstacles[index];
      ObstacleVertex obstacleVertex2 = obstacleVertex1;
      do
      {
        obstacleVertex1.layer = this.layer;
        obstacleVertex1 = obstacleVertex1.next;
      }
      while (obstacleVertex1 != obstacleVertex2);
      this.sim.AddObstacle(this.addedObstacles[index]);
    }
  }

  public void Start()
  {
    this.addedObstacles = new List<ObstacleVertex>();
    this.sourceObstacles = new List<Vector3[]>();
    this.prevUpdateMatrix = this.GetMatrix();
    this.CreateObstacles();
  }

  public void Update()
  {
    Matrix4x4 matrix = this.GetMatrix();
    if (!(matrix != this.prevUpdateMatrix))
      return;
    for (int index = 0; index < this.addedObstacles.Count; ++index)
      this.sim.UpdateObstacle(this.addedObstacles[index], this.sourceObstacles[index], matrix);
    this.prevUpdateMatrix = matrix;
  }

  public void FindSimulator()
  {
    RVOSimulator objectOfType = UnityEngine.Object.FindObjectOfType(typeof (RVOSimulator)) as RVOSimulator;
    this.sim = !((UnityEngine.Object) objectOfType == (UnityEngine.Object) null) ? objectOfType.GetSimulator() : throw new InvalidOperationException("No RVOSimulator could be found in the scene. Please add one to any GameObject");
  }

  public void AddObstacle(Vector3[] vertices, float height)
  {
    if (vertices == null)
      throw new ArgumentNullException("Vertices Must Not Be Null");
    if ((double) height < 0.0)
      throw new ArgumentOutOfRangeException("Height must be non-negative");
    if (vertices.Length < 2)
      throw new ArgumentException("An obstacle must have at least two vertices");
    if (this.gizmoDrawing)
    {
      Vector3[] destinationArray = new Vector3[vertices.Length];
      this.WindCorrectly(vertices);
      Array.Copy((Array) vertices, (Array) destinationArray, vertices.Length);
      this.gizmoVerts.Add(destinationArray);
    }
    else
    {
      if (this.sim == null)
        this.FindSimulator();
      if (vertices.Length == 2)
      {
        this.AddObstacleInternal(vertices, height);
      }
      else
      {
        this.WindCorrectly(vertices);
        this.AddObstacleInternal(vertices, height);
      }
    }
  }

  public void AddObstacleInternal(Vector3[] vertices, float height)
  {
    this.addedObstacles.Add(this.sim.AddObstacle(vertices, height, this.GetMatrix(), this.layer));
    this.sourceObstacles.Add(vertices);
  }

  public void WindCorrectly(Vector3[] vertices)
  {
    int index1 = 0;
    float num = float.PositiveInfinity;
    for (int index2 = 0; index2 < vertices.Length; ++index2)
    {
      if ((double) vertices[index2].x < (double) num)
      {
        index1 = index2;
        num = vertices[index2].x;
      }
    }
    if (VectorMath.IsClockwiseXZ(vertices[(index1 - 1 + vertices.Length) % vertices.Length], vertices[index1], vertices[(index1 + 1) % vertices.Length]))
    {
      if (this.obstacleMode != RVOObstacle.ObstacleVertexWinding.KeepOut)
        return;
      Array.Reverse((Array) vertices);
    }
    else
    {
      if (this.obstacleMode != RVOObstacle.ObstacleVertexWinding.KeepIn)
        return;
      Array.Reverse((Array) vertices);
    }
  }

  public enum ObstacleVertexWinding
  {
    KeepOut,
    KeepIn,
  }
}
