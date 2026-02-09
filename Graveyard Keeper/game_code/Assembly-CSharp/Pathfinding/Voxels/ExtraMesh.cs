// Decompiled with JetBrains decompiler
// Type: Pathfinding.Voxels.ExtraMesh
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Pathfinding.Voxels;

public struct ExtraMesh
{
  public MeshFilter original;
  public int area;
  public Vector3[] vertices;
  public int[] triangles;
  public Bounds bounds;
  public Matrix4x4 matrix;

  public ExtraMesh(Vector3[] v, int[] t, Bounds b)
  {
    this.matrix = Matrix4x4.identity;
    this.vertices = v;
    this.triangles = t;
    this.bounds = b;
    this.original = (MeshFilter) null;
    this.area = 0;
  }

  public ExtraMesh(Vector3[] v, int[] t, Bounds b, Matrix4x4 matrix)
  {
    this.matrix = matrix;
    this.vertices = v;
    this.triangles = t;
    this.bounds = b;
    this.original = (MeshFilter) null;
    this.area = 0;
  }

  public void RecalculateBounds()
  {
    Bounds bounds = new Bounds(this.matrix.MultiplyPoint3x4(this.vertices[0]), Vector3.zero);
    for (int index = 1; index < this.vertices.Length; ++index)
      bounds.Encapsulate(this.matrix.MultiplyPoint3x4(this.vertices[index]));
    this.bounds = bounds;
  }
}
