// Decompiled with JetBrains decompiler
// Type: ColliderToMeshUtility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public static class ColliderToMeshUtility
{
  public const float INSET_AMOUNT = -0.15f;

  public static Mesh GenerateMeshFromCompositeCollider(
    CompositeCollider2D compositeCollider,
    float extrusionDepth)
  {
    if ((UnityEngine.Object) compositeCollider == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "CompositeCollider2D is null.");
      return (Mesh) null;
    }
    Mesh compositeCollider1 = new Mesh();
    int pathCount = compositeCollider.pathCount;
    CombineInstance[] combine = new CombineInstance[pathCount];
    for (int index = 0; index < pathCount; ++index)
    {
      Vector2[] vector2Array = new Vector2[compositeCollider.GetPathPointCount(index)];
      compositeCollider.GetPath(index, vector2Array);
      Mesh insetSurfaceMesh = ColliderToMeshUtility.CreateInsetSurfaceMesh(vector2Array);
      if (!((UnityEngine.Object) insetSurfaceMesh == (UnityEngine.Object) null))
      {
        ColliderToMeshUtility.ExtrudeMeshWallsOnly(insetSurfaceMesh, extrusionDepth, Vector3.forward);
        combine[index].mesh = insetSurfaceMesh;
        combine[index].transform = Matrix4x4.identity;
      }
    }
    compositeCollider1.CombineMeshes(combine, true, false);
    compositeCollider1.RecalculateNormals();
    compositeCollider1.RecalculateBounds();
    return compositeCollider1;
  }

  public static Mesh CreateInsetSurfaceMesh(Vector2[] pathPoints)
  {
    int length = pathPoints.Length;
    if (length < 3)
      return (Mesh) null;
    Vector3[] vector3Array = new Vector3[length];
    Vector2[] vector2Array = new Vector2[length];
    for (int index = 0; index < length; ++index)
    {
      Vector2 pathPoint1 = pathPoints[(index - 1 + length) % length];
      Vector2 pathPoint2 = pathPoints[index];
      Vector2 pathPoint3 = pathPoints[(index + 1) % length];
      Vector2 vector2_1 = pathPoint2 - pathPoint1;
      Vector2 normalized1 = vector2_1.normalized;
      Vector2 vector2_2 = pathPoint2;
      vector2_1 = pathPoint3 - vector2_2;
      Vector2 normalized2 = vector2_1.normalized;
      Vector2 vector2_3 = new Vector2(-normalized1.y, normalized1.x);
      Vector2 vector2_4 = new Vector2(-normalized2.y, normalized2.x);
      Vector2 vector2_5 = vector2_3 + vector2_4;
      if ((double) vector2_5.sqrMagnitude < 9.9999999747524271E-07)
        vector2_5 = vector2_3;
      vector2_5.Normalize();
      Vector2 vector2_6 = pathPoint2 - vector2_5 * -0.15f;
      vector3Array[index] = new Vector3(vector2_6.x, vector2_6.y, 0.0f);
      vector2Array[index] = vector2_6;
    }
    Mesh insetSurfaceMesh = new Mesh();
    insetSurfaceMesh.name = "InsetSurface";
    insetSurfaceMesh.vertices = vector3Array;
    insetSurfaceMesh.uv = vector2Array;
    insetSurfaceMesh.triangles = ColliderToMeshUtility.TriangulateConvexPolygon(length);
    insetSurfaceMesh.RecalculateNormals();
    insetSurfaceMesh.RecalculateBounds();
    return insetSurfaceMesh;
  }

  public static int[] TriangulateConvexPolygon(int n)
  {
    int[] numArray1 = new int[(n - 2) * 3];
    int num1 = 0;
    for (int index1 = 1; index1 < n - 1; ++index1)
    {
      int[] numArray2 = numArray1;
      int index2 = num1;
      int num2 = index2 + 1;
      numArray2[index2] = 0;
      int[] numArray3 = numArray1;
      int index3 = num2;
      int num3 = index3 + 1;
      int num4 = index1;
      numArray3[index3] = num4;
      int[] numArray4 = numArray1;
      int index4 = num3;
      num1 = index4 + 1;
      int num5 = index1 + 1;
      numArray4[index4] = num5;
    }
    return numArray1;
  }

  public static void ExtrudeMeshWallsOnly(Mesh mesh, float depth, Vector3 dir)
  {
    Vector3[] vertices = mesh.vertices;
    int length = vertices.Length;
    Vector3[] destinationArray = new Vector3[length * 2];
    Array.Copy((Array) vertices, (Array) destinationArray, length);
    for (int index = 0; index < length; ++index)
      destinationArray[index + length] = vertices[index] + dir * depth;
    int[] numArray1 = new int[length * 6];
    int num1 = 0;
    for (int index1 = 0; index1 < length; ++index1)
    {
      int num2 = index1;
      int num3 = (index1 + 1) % length;
      int num4 = num2 + length;
      int num5 = num3 + length;
      int[] numArray2 = numArray1;
      int index2 = num1;
      int num6 = index2 + 1;
      int num7 = num2;
      numArray2[index2] = num7;
      int[] numArray3 = numArray1;
      int index3 = num6;
      int num8 = index3 + 1;
      int num9 = num3;
      numArray3[index3] = num9;
      int[] numArray4 = numArray1;
      int index4 = num8;
      int num10 = index4 + 1;
      int num11 = num4;
      numArray4[index4] = num11;
      int[] numArray5 = numArray1;
      int index5 = num10;
      int num12 = index5 + 1;
      int num13 = num3;
      numArray5[index5] = num13;
      int[] numArray6 = numArray1;
      int index6 = num12;
      int num14 = index6 + 1;
      int num15 = num5;
      numArray6[index6] = num15;
      int[] numArray7 = numArray1;
      int index7 = num14;
      num1 = index7 + 1;
      int num16 = num4;
      numArray7[index7] = num16;
    }
    mesh.vertices = destinationArray;
    mesh.triangles = numArray1;
    Vector2[] vector2Array = new Vector2[destinationArray.Length];
    for (int index = 0; index < length; ++index)
      vector2Array[index] = new Vector2(vertices[index].x, vertices[index].y);
    for (int index = length; index < length * 2; ++index)
      vector2Array[index] = new Vector2(vertices[index - length].x, vertices[index - length].y);
    mesh.uv = vector2Array;
    mesh.RecalculateNormals();
    mesh.RecalculateBounds();
  }

  public static void ApplyMeshToRenderer(GameObject target, Mesh mesh, Material mat = null)
  {
    if ((UnityEngine.Object) target == (UnityEngine.Object) null || (UnityEngine.Object) mesh == (UnityEngine.Object) null)
      return;
    MeshFilter meshFilter = target.GetComponent<MeshFilter>();
    if ((UnityEngine.Object) meshFilter == (UnityEngine.Object) null)
      meshFilter = target.AddComponent<MeshFilter>();
    MeshRenderer meshRenderer = target.GetComponent<MeshRenderer>();
    if ((UnityEngine.Object) meshRenderer == (UnityEngine.Object) null)
      meshRenderer = target.AddComponent<MeshRenderer>();
    meshFilter.mesh = mesh;
    if ((UnityEngine.Object) mat != (UnityEngine.Object) null)
    {
      meshRenderer.sharedMaterial = mat;
    }
    else
    {
      if (!((UnityEngine.Object) meshRenderer.material == (UnityEngine.Object) null))
        return;
      meshRenderer.material = new Material(Shader.Find("Standard"));
    }
  }
}
