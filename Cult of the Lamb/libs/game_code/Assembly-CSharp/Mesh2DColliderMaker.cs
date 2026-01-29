// Decompiled with JetBrains decompiler
// Type: Mesh2DColliderMaker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (MeshFilter))]
[RequireComponent(typeof (PolygonCollider2D))]
[ExecuteInEditMode]
public class Mesh2DColliderMaker : MonoBehaviour
{
  public MeshFilter filter;
  public PolygonCollider2D polyCollider;

  public void Start()
  {
  }

  public void Create(MeshFilter filterVar, PolygonCollider2D polyColliderVar)
  {
    this.filter = filterVar;
    this.polyCollider = polyColliderVar;
    this.CreatePolygon2DColliderPoints();
  }

  public void CreatePolygon2DColliderPoints()
  {
    this.ApplyPathsToPolygonCollider(Mesh2DColliderMaker.BuildColliderPaths(this.BuildEdgesFromMesh()));
  }

  public void ApplyPathsToPolygonCollider(List<Vector2[]> paths)
  {
    if (paths == null)
      return;
    this.polyCollider.pathCount = paths.Count;
    for (int index = 0; index < paths.Count; ++index)
    {
      Vector2[] path = paths[index];
      this.polyCollider.SetPath(index, path);
    }
  }

  public Dictionary<Mesh2DColliderMaker.Edge2D, int> BuildEdgesFromMesh()
  {
    Mesh sharedMesh = this.filter.sharedMesh;
    if ((Object) sharedMesh == (Object) null)
      return (Dictionary<Mesh2DColliderMaker.Edge2D, int>) null;
    Vector3[] vertices = sharedMesh.vertices;
    int[] triangles = sharedMesh.triangles;
    Dictionary<Mesh2DColliderMaker.Edge2D, int> dictionary1 = new Dictionary<Mesh2DColliderMaker.Edge2D, int>();
    for (int index = 0; index < triangles.Length - 2; index += 3)
    {
      Vector3 vector3_1 = vertices[triangles[index]];
      Vector3 vector3_2 = vertices[triangles[index + 1]];
      Vector3 vector3_3 = vertices[triangles[index + 2]];
      Mesh2DColliderMaker.Edge2D[] edge2DArray = new Mesh2DColliderMaker.Edge2D[3];
      Mesh2DColliderMaker.Edge2D key1 = new Mesh2DColliderMaker.Edge2D();
      key1.a = (Vector2) vector3_1;
      key1.b = (Vector2) vector3_2;
      edge2DArray[0] = key1;
      key1 = new Mesh2DColliderMaker.Edge2D();
      key1.a = (Vector2) vector3_2;
      key1.b = (Vector2) vector3_3;
      edge2DArray[1] = key1;
      key1 = new Mesh2DColliderMaker.Edge2D();
      key1.a = (Vector2) vector3_3;
      key1.b = (Vector2) vector3_1;
      edge2DArray[2] = key1;
      foreach (Mesh2DColliderMaker.Edge2D key2 in edge2DArray)
      {
        if (dictionary1.ContainsKey(key2))
        {
          Dictionary<Mesh2DColliderMaker.Edge2D, int> dictionary2 = dictionary1;
          key1 = key2;
          dictionary2[key1]++;
        }
        else
          dictionary1[key2] = 1;
      }
    }
    return dictionary1;
  }

  public static List<Mesh2DColliderMaker.Edge2D> GetOuterEdges(
    Dictionary<Mesh2DColliderMaker.Edge2D, int> allEdges)
  {
    List<Mesh2DColliderMaker.Edge2D> outerEdges = new List<Mesh2DColliderMaker.Edge2D>();
    foreach (Mesh2DColliderMaker.Edge2D key in allEdges.Keys)
    {
      if (allEdges[key] == 1)
        outerEdges.Add(key);
    }
    return outerEdges;
  }

  public static List<Vector2[]> BuildColliderPaths(
    Dictionary<Mesh2DColliderMaker.Edge2D, int> allEdges)
  {
    if (allEdges == null)
      return (List<Vector2[]>) null;
    List<Mesh2DColliderMaker.Edge2D> outerEdges = Mesh2DColliderMaker.GetOuterEdges(allEdges);
    List<List<Mesh2DColliderMaker.Edge2D>> edge2DListList = new List<List<Mesh2DColliderMaker.Edge2D>>();
    List<Mesh2DColliderMaker.Edge2D> edge2DList1 = (List<Mesh2DColliderMaker.Edge2D>) null;
    while (outerEdges.Count > 0)
    {
      if (edge2DList1 == null)
      {
        edge2DList1 = new List<Mesh2DColliderMaker.Edge2D>();
        edge2DList1.Add(outerEdges[0]);
        edge2DListList.Add(edge2DList1);
        outerEdges.RemoveAt(0);
      }
      bool flag1 = false;
      int index = 0;
      while (index < outerEdges.Count)
      {
        Mesh2DColliderMaker.Edge2D edge2D = outerEdges[index];
        bool flag2 = false;
        if (edge2D.b == edge2DList1[0].a)
        {
          edge2DList1.Insert(0, edge2D);
          flag2 = true;
        }
        else if (edge2D.a == edge2DList1[edge2DList1.Count - 1].b)
        {
          edge2DList1.Add(edge2D);
          flag2 = true;
        }
        if (flag2)
        {
          flag1 = true;
          outerEdges.RemoveAt(index);
        }
        else
          ++index;
      }
      if (!flag1)
        edge2DList1 = (List<Mesh2DColliderMaker.Edge2D>) null;
    }
    List<Vector2[]> vector2ArrayList = new List<Vector2[]>();
    foreach (List<Mesh2DColliderMaker.Edge2D> edge2DList2 in edge2DListList)
    {
      List<Vector2> coordinates = new List<Vector2>();
      foreach (Mesh2DColliderMaker.Edge2D edge2D in edge2DList2)
        coordinates.Add(edge2D.a);
      vector2ArrayList.Add(Mesh2DColliderMaker.CoordinatesCleaned(coordinates));
    }
    return vector2ArrayList;
  }

  public static bool CoordinatesFormLine(Vector2 a, Vector2 b, Vector2 c)
  {
    return Mathf.Approximately((float) ((double) a.x * ((double) b.y - (double) c.y) + (double) b.x * ((double) c.y - (double) a.y) + (double) c.x * ((double) a.y - (double) b.y)), 0.0f);
  }

  public static Vector2[] CoordinatesCleaned(List<Vector2> coordinates)
  {
    List<Vector2> vector2List = new List<Vector2>();
    vector2List.Add(coordinates[0]);
    int index1 = 0;
    for (int index2 = 1; index2 < coordinates.Count; ++index2)
    {
      Vector2 coordinate1 = coordinates[index2];
      Vector2 coordinate2 = coordinates[index1];
      Vector2 vector2 = index2 + 1 >= coordinates.Count ? coordinates[0] : coordinates[index2 + 1];
      Vector2 b = coordinate1;
      Vector2 c = vector2;
      if (!Mesh2DColliderMaker.CoordinatesFormLine(coordinate2, b, c))
      {
        vector2List.Add(coordinate1);
        index1 = index2;
      }
    }
    return vector2List.ToArray();
  }

  public struct Edge2D
  {
    public Vector2 a;
    public Vector2 b;

    public override bool Equals(object obj)
    {
      if (!(obj is Mesh2DColliderMaker.Edge2D edge2D))
        return false;
      if (edge2D.a == this.a && edge2D.b == this.b)
        return true;
      return edge2D.b == this.a && edge2D.a == this.b;
    }

    public override int GetHashCode() => this.a.GetHashCode() ^ this.b.GetHashCode();

    public override string ToString()
    {
      return string.Format($"[{this.a.x.ToString()},{this.a.y.ToString()}->{this.b.x.ToString()},{this.b.y.ToString()}]");
    }
  }
}
