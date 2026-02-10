// Decompiled with JetBrains decompiler
// Type: PolygonColliderToMesh
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class PolygonColliderToMesh : MonoBehaviour
{
  public PolygonCollider2D polygonCollider;
  public MeshFilter meshFilter;
  public MeshRenderer meshRenderer;
  [SerializeField]
  public float extrusionDepth = 1f;
  [SerializeField]
  public Material extrusionMaterial;
  [SerializeField]
  public bool createMeshOnStart;
  [SerializeField]
  public bool createMeshExtrudedOnStart;

  public void Start()
  {
    this.polygonCollider = this.GetComponent<PolygonCollider2D>();
    this.meshFilter = this.GetComponent<MeshFilter>();
    this.meshRenderer = this.GetComponent<MeshRenderer>();
    this.polygonCollider.isTrigger = true;
    if (this.createMeshOnStart)
      this.GenerateMesh();
    if (!this.createMeshExtrudedOnStart)
      return;
    this.GenerateExtrudedMesh();
  }

  public void GenerateMesh()
  {
    this.polygonCollider = this.GetComponent<PolygonCollider2D>();
    this.meshFilter = this.GetComponent<MeshFilter>();
    this.meshRenderer = this.GetComponent<MeshRenderer>();
    Vector2[] path = this.polygonCollider.GetPath(0);
    Vector3[] vector3Array = new Vector3[path.Length];
    for (int index = 0; index < path.Length; ++index)
      vector3Array[index] = new Vector3(path[index].x, path[index].y, 0.0f);
    int[] numArray = new int[(path.Length - 2) * 3];
    int index1 = 0;
    int num = 0;
    while (index1 < numArray.Length)
    {
      numArray[index1] = 0;
      numArray[index1 + 1] = num + 1;
      numArray[index1 + 2] = num + 2;
      index1 += 3;
      ++num;
    }
    Mesh mesh = new Mesh();
    mesh.vertices = vector3Array;
    mesh.triangles = numArray;
    mesh.RecalculateNormals();
    this.meshFilter.mesh = mesh;
  }

  public void GenerateExtrudedMesh()
  {
    this.polygonCollider = this.GetComponent<PolygonCollider2D>();
    Mesh fromPolygonCollider = this.GenerateMeshFromPolygonCollider(this.polygonCollider, this.extrusionDepth);
    if ((UnityEngine.Object) fromPolygonCollider == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "No mesh could be generated from the provided collider.");
    }
    else
    {
      GameObject target = new GameObject("ExtrudedMesh");
      target.transform.SetParent(this.transform);
      target.transform.localPosition = Vector3.zero;
      ColliderToMeshUtility.ApplyMeshToRenderer(target, fromPolygonCollider, this.extrusionMaterial);
    }
  }

  public Mesh GenerateMeshFromPolygonCollider(
    PolygonCollider2D polygonCollider,
    float extrusionDepth)
  {
    if ((UnityEngine.Object) polygonCollider == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "PolygonCollider2D is null.");
      return (Mesh) null;
    }
    Mesh fromPolygonCollider = new Mesh();
    int pathCount = polygonCollider.pathCount;
    CombineInstance[] combine = new CombineInstance[pathCount];
    for (int index = 0; index < pathCount; ++index)
    {
      Mesh meshFromPath = PolygonColliderToMesh.CreateMeshFromPath(polygonCollider.GetPath(index));
      if (!((UnityEngine.Object) meshFromPath == (UnityEngine.Object) null))
      {
        PolygonColliderToMesh.ExtrudeMesh(meshFromPath, extrusionDepth);
        combine[index].mesh = meshFromPath;
        combine[index].transform = Matrix4x4.identity;
      }
    }
    fromPolygonCollider.CombineMeshes(combine, true, false);
    return fromPolygonCollider;
  }

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
    CombineInstance[] combine = new CombineInstance[compositeCollider.pathCount];
    for (int index = 0; index < compositeCollider.pathCount; ++index)
    {
      Vector2[] vector2Array = new Vector2[compositeCollider.GetPathPointCount(index)];
      compositeCollider.GetPath(index, vector2Array);
      Mesh meshFromPath = PolygonColliderToMesh.CreateMeshFromPath(vector2Array);
      PolygonColliderToMesh.ExtrudeMesh(meshFromPath, extrusionDepth);
      combine[index].mesh = meshFromPath;
      combine[index].transform = Matrix4x4.identity;
    }
    compositeCollider1.CombineMeshes(combine);
    return compositeCollider1;
  }

  public static Mesh CreateMeshFromPath(Vector2[] pathPoints)
  {
    if (pathPoints.Length < 3)
    {
      Debug.LogError((object) "Path must contain at least three points to form a mesh.");
      return (Mesh) null;
    }
    Mesh meshFromPath = new Mesh();
    Vector3[] vector3Array = new Vector3[pathPoints.Length];
    for (int index = 0; index < pathPoints.Length; ++index)
      vector3Array[index] = new Vector3(pathPoints[index].x, pathPoints[index].y, 0.0f);
    int[] numArray = PolygonColliderToMesh.TriangulateShape(pathPoints);
    meshFromPath.vertices = vector3Array;
    meshFromPath.triangles = numArray;
    return meshFromPath;
  }

  public static int[] TriangulateShape(Vector2[] pathPoints)
  {
    return new int[3]{ 0, 1, 2 };
  }

  public static void ExtrudeMesh(Mesh mesh, float extrusionDepth)
  {
    Vector3[] vertices = mesh.vertices;
    int length = vertices.Length;
    Vector3[] destinationArray1 = new Vector3[length * 2];
    Array.Copy((Array) vertices, (Array) destinationArray1, length);
    for (int index = 0; index < length; ++index)
      destinationArray1[index + length] = vertices[index] + Vector3.forward * extrusionDepth;
    int[] destinationArray2 = new int[mesh.triangles.Length * 2 + length * 6];
    Array.Copy((Array) mesh.triangles, (Array) destinationArray2, mesh.triangles.Length);
    for (int index = 0; index < length; ++index)
    {
      int num = (index + 1) % length;
      Array.Copy((Array) new int[6]
      {
        index,
        num,
        index + length,
        num,
        num + length,
        index + length
      }, 0, (Array) destinationArray2, mesh.triangles.Length + index * 6, 6);
    }
    mesh.vertices = destinationArray1;
    mesh.triangles = destinationArray2;
    mesh.RecalculateNormals();
  }

  public static void ApplyMeshToRenderer(GameObject target, Mesh mesh, Material mat = null)
  {
    if ((UnityEngine.Object) target == (UnityEngine.Object) null || (UnityEngine.Object) mesh == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Target GameObject or Mesh is null.");
    }
    else
    {
      MeshFilter meshFilter = target.GetComponent<MeshFilter>();
      if ((UnityEngine.Object) meshFilter == (UnityEngine.Object) null)
        meshFilter = target.AddComponent<MeshFilter>();
      MeshRenderer meshRenderer = target.GetComponent<MeshRenderer>();
      if ((UnityEngine.Object) meshRenderer == (UnityEngine.Object) null)
        meshRenderer = target.AddComponent<MeshRenderer>();
      meshFilter.mesh = mesh;
      if ((UnityEngine.Object) mat == (UnityEngine.Object) null)
      {
        if (!((UnityEngine.Object) meshRenderer.material == (UnityEngine.Object) null))
          return;
        meshRenderer.material = new Material(Shader.Find("Standard"));
      }
      else
        meshRenderer.sharedMaterial = mat;
    }
  }
}
