// Decompiled with JetBrains decompiler
// Type: Pathfinding.NavmeshCut
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding.ClipperLib;
using Pathfinding.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Pathfinding;

[AddComponentMenu("Pathfinding/Navmesh/Navmesh Cut")]
[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_navmesh_cut.php")]
public class NavmeshCut : MonoBehaviour
{
  public static List<NavmeshCut> allCuts = new List<NavmeshCut>();
  [Tooltip("Shape of the cut")]
  public NavmeshCut.MeshType type;
  [Tooltip("The contour(s) of the mesh will be extracted. This mesh should only be a 2D surface, not a volume (see documentation).")]
  public Mesh mesh;
  public Vector2 rectangleSize = new Vector2(1f, 1f);
  public float circleRadius = 1f;
  public int circleResolution = 6;
  public float height = 1f;
  [Tooltip("Scale of the custom mesh")]
  public float meshScale = 1f;
  public Vector3 center;
  [Tooltip("Distance between positions to require an update of the navmesh\nA smaller distance gives better accuracy, but requires more updates when moving the object over time, so it is often slower.")]
  public float updateDistance = 0.4f;
  [Tooltip("Only makes a split in the navmesh, but does not remove the geometry to make a hole")]
  public bool isDual;
  public bool cutsAddedGeom = true;
  [Tooltip("How many degrees rotation that is required for an update to the navmesh. Should be between 0 and 180.")]
  public float updateRotationDistance = 10f;
  [Tooltip("Includes rotation in calculations. This is slower since a lot more matrix multiplications are needed but gives more flexibility.")]
  public bool useRotation;
  public Vector3[][] contours;
  public Transform tr;
  public Mesh lastMesh;
  public Vector3 lastPosition;
  public Quaternion lastRotation;
  public bool wasEnabled;
  public Bounds lastBounds;
  public static Dictionary<Int2, int> edges = new Dictionary<Int2, int>();
  public static Dictionary<int, int> pointers = new Dictionary<int, int>();
  public static Color GizmoColor = new Color(0.145098045f, 0.721568644f, 0.9372549f);

  public static event Action<NavmeshCut> OnDestroyCallback;

  public static void AddCut(NavmeshCut obj) => NavmeshCut.allCuts.Add(obj);

  public static void RemoveCut(NavmeshCut obj) => NavmeshCut.allCuts.Remove(obj);

  public static List<NavmeshCut> GetAllInRange(Bounds b)
  {
    List<NavmeshCut> allInRange = ListPool<NavmeshCut>.Claim();
    for (int index = 0; index < NavmeshCut.allCuts.Count; ++index)
    {
      if (NavmeshCut.allCuts[index].enabled && NavmeshCut.Intersects(b, NavmeshCut.allCuts[index].GetBounds()))
        allInRange.Add(NavmeshCut.allCuts[index]);
    }
    return allInRange;
  }

  public static bool Intersects(Bounds b1, Bounds b2)
  {
    Vector3 min1 = b1.min;
    Vector3 max1 = b1.max;
    Vector3 min2 = b2.min;
    Vector3 max2 = b2.max;
    return (double) min1.x <= (double) max2.x && (double) max1.x >= (double) min2.x && (double) min1.y <= (double) max2.y && (double) max1.y >= (double) min2.y && (double) min1.z <= (double) max2.z && (double) max1.z >= (double) min2.z;
  }

  public static List<NavmeshCut> GetAll() => NavmeshCut.allCuts;

  public Bounds LastBounds => this.lastBounds;

  public void Awake()
  {
    this.tr = this.transform;
    NavmeshCut.AddCut(this);
  }

  public void OnEnable()
  {
    this.lastPosition = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
    this.lastRotation = this.tr.rotation;
  }

  public void OnDestroy()
  {
    if (NavmeshCut.OnDestroyCallback != null)
      NavmeshCut.OnDestroyCallback(this);
    NavmeshCut.RemoveCut(this);
  }

  public void ForceUpdate()
  {
    this.lastPosition = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
  }

  public bool RequiresUpdate()
  {
    if (this.wasEnabled != this.enabled)
      return true;
    if (!this.wasEnabled)
      return false;
    if ((double) (this.tr.position - this.lastPosition).sqrMagnitude > (double) this.updateDistance * (double) this.updateDistance)
      return true;
    return this.useRotation && (double) Quaternion.Angle(this.lastRotation, this.tr.rotation) > (double) this.updateRotationDistance;
  }

  public virtual void UsedForCut()
  {
  }

  public void NotifyUpdated()
  {
    this.wasEnabled = this.enabled;
    if (!this.wasEnabled)
      return;
    this.lastPosition = this.tr.position;
    this.lastBounds = this.GetBounds();
    if (!this.useRotation)
      return;
    this.lastRotation = this.tr.rotation;
  }

  public void CalculateMeshContour()
  {
    if ((UnityEngine.Object) this.mesh == (UnityEngine.Object) null)
      return;
    NavmeshCut.edges.Clear();
    NavmeshCut.pointers.Clear();
    Vector3[] vertices = this.mesh.vertices;
    int[] triangles = this.mesh.triangles;
    for (int index = 0; index < triangles.Length; index += 3)
    {
      if (VectorMath.IsClockwiseXZ(vertices[triangles[index]], vertices[triangles[index + 1]], vertices[triangles[index + 2]]))
      {
        int num = triangles[index];
        triangles[index] = triangles[index + 2];
        triangles[index + 2] = num;
      }
      NavmeshCut.edges[new Int2(triangles[index], triangles[index + 1])] = index;
      NavmeshCut.edges[new Int2(triangles[index + 1], triangles[index + 2])] = index;
      NavmeshCut.edges[new Int2(triangles[index + 2], triangles[index])] = index;
    }
    for (int index1 = 0; index1 < triangles.Length; index1 += 3)
    {
      for (int index2 = 0; index2 < 3; ++index2)
      {
        if (!NavmeshCut.edges.ContainsKey(new Int2(triangles[index1 + (index2 + 1) % 3], triangles[index1 + index2 % 3])))
          NavmeshCut.pointers[triangles[index1 + index2 % 3]] = triangles[index1 + (index2 + 1) % 3];
      }
    }
    List<Vector3[]> vector3ArrayList = new List<Vector3[]>();
    List<Vector3> list = ListPool<Vector3>.Claim();
    for (int key1 = 0; key1 < vertices.Length; ++key1)
    {
      if (NavmeshCut.pointers.ContainsKey(key1))
      {
        list.Clear();
        int key2 = key1;
        do
        {
          int pointer = NavmeshCut.pointers[key2];
          if (pointer != -1)
          {
            NavmeshCut.pointers[key2] = -1;
            list.Add(vertices[key2]);
            key2 = pointer;
            if (key2 == -1)
            {
              Debug.LogError((object) $"Invalid Mesh '{this.mesh.name} in {this.gameObject.name}");
              break;
            }
          }
          else
            break;
        }
        while (key2 != key1);
        if (list.Count > 0)
          vector3ArrayList.Add(list.ToArray());
      }
    }
    ListPool<Vector3>.Release(list);
    this.contours = vector3ArrayList.ToArray();
  }

  public Bounds GetBounds()
  {
    Bounds bounds1;
    switch (this.type)
    {
      case NavmeshCut.MeshType.Rectangle:
        if (this.useRotation)
        {
          Matrix4x4 localToWorldMatrix = this.tr.localToWorldMatrix;
          bounds1 = new Bounds(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(-this.rectangleSize.x, -this.height, -this.rectangleSize.y) * 0.5f), Vector3.zero);
          bounds1.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(this.rectangleSize.x, -this.height, -this.rectangleSize.y) * 0.5f));
          bounds1.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(this.rectangleSize.x, -this.height, this.rectangleSize.y) * 0.5f));
          bounds1.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(-this.rectangleSize.x, -this.height, this.rectangleSize.y) * 0.5f));
          bounds1.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(-this.rectangleSize.x, this.height, -this.rectangleSize.y) * 0.5f));
          bounds1.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(this.rectangleSize.x, this.height, -this.rectangleSize.y) * 0.5f));
          bounds1.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(this.rectangleSize.x, this.height, this.rectangleSize.y) * 0.5f));
          bounds1.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(-this.rectangleSize.x, this.height, this.rectangleSize.y) * 0.5f));
          break;
        }
        bounds1 = new Bounds(this.tr.position + this.center, new Vector3(this.rectangleSize.x, this.height, this.rectangleSize.y));
        break;
      case NavmeshCut.MeshType.Circle:
        bounds1 = !this.useRotation ? new Bounds(this.transform.position + this.center, new Vector3(this.circleRadius * 2f, this.height, this.circleRadius * 2f)) : new Bounds(this.tr.localToWorldMatrix.MultiplyPoint3x4(this.center), new Vector3(this.circleRadius * 2f, this.height, this.circleRadius * 2f));
        break;
      case NavmeshCut.MeshType.CustomMesh:
        if ((UnityEngine.Object) this.mesh == (UnityEngine.Object) null)
        {
          bounds1 = new Bounds();
          break;
        }
        Bounds bounds2 = this.mesh.bounds;
        if (this.useRotation)
        {
          Matrix4x4 localToWorldMatrix = this.tr.localToWorldMatrix;
          bounds2.center *= this.meshScale;
          bounds2.size *= this.meshScale;
          bounds1 = new Bounds(localToWorldMatrix.MultiplyPoint3x4(this.center + bounds2.center), Vector3.zero);
          Vector3 max = bounds2.max;
          Vector3 min = bounds2.min;
          bounds1.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(max.x, max.y, max.z)));
          bounds1.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(min.x, max.y, max.z)));
          bounds1.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(min.x, max.y, min.z)));
          bounds1.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(max.x, max.y, min.z)));
          bounds1.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(max.x, min.y, max.z)));
          bounds1.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(min.x, min.y, max.z)));
          bounds1.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(min.x, min.y, min.z)));
          bounds1.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(max.x, min.y, min.z)));
          Vector3 size = bounds1.size;
          size.y = Mathf.Max(size.y, this.height * this.tr.lossyScale.y);
          bounds1.size = size;
          break;
        }
        Vector3 size1 = bounds2.size * this.meshScale;
        size1.y = Mathf.Max(size1.y, this.height);
        bounds1 = new Bounds(this.transform.position + this.center + bounds2.center * this.meshScale, size1);
        break;
      default:
        throw new Exception("Invalid mesh type");
    }
    return bounds1;
  }

  public void GetContour(List<List<IntPoint>> buffer)
  {
    if (this.circleResolution < 3)
      this.circleResolution = 3;
    Vector3 position = this.tr.position;
    Matrix4x4 matrix = Matrix4x4.identity;
    bool flag1 = false;
    if (this.useRotation)
    {
      matrix = this.tr.localToWorldMatrix;
      flag1 = VectorMath.ReversesFaceOrientationsXZ(matrix);
    }
    switch (this.type)
    {
      case NavmeshCut.MeshType.Rectangle:
        List<IntPoint> intPointList1 = ListPool<IntPoint>.Claim();
        bool flag2 = flag1 ^ (double) this.rectangleSize.x < 0.0 ^ (double) this.rectangleSize.y < 0.0;
        if (this.useRotation)
        {
          intPointList1.Add(NavmeshCut.V3ToIntPoint(matrix.MultiplyPoint3x4(this.center + new Vector3(-this.rectangleSize.x, 0.0f, -this.rectangleSize.y) * 0.5f)));
          intPointList1.Add(NavmeshCut.V3ToIntPoint(matrix.MultiplyPoint3x4(this.center + new Vector3(this.rectangleSize.x, 0.0f, -this.rectangleSize.y) * 0.5f)));
          intPointList1.Add(NavmeshCut.V3ToIntPoint(matrix.MultiplyPoint3x4(this.center + new Vector3(this.rectangleSize.x, 0.0f, this.rectangleSize.y) * 0.5f)));
          intPointList1.Add(NavmeshCut.V3ToIntPoint(matrix.MultiplyPoint3x4(this.center + new Vector3(-this.rectangleSize.x, 0.0f, this.rectangleSize.y) * 0.5f)));
        }
        else
        {
          Vector3 vector3 = position + this.center;
          intPointList1.Add(NavmeshCut.V3ToIntPoint(vector3 + new Vector3(-this.rectangleSize.x, 0.0f, -this.rectangleSize.y) * 0.5f));
          intPointList1.Add(NavmeshCut.V3ToIntPoint(vector3 + new Vector3(this.rectangleSize.x, 0.0f, -this.rectangleSize.y) * 0.5f));
          intPointList1.Add(NavmeshCut.V3ToIntPoint(vector3 + new Vector3(this.rectangleSize.x, 0.0f, this.rectangleSize.y) * 0.5f));
          intPointList1.Add(NavmeshCut.V3ToIntPoint(vector3 + new Vector3(-this.rectangleSize.x, 0.0f, this.rectangleSize.y) * 0.5f));
        }
        if (flag2)
          intPointList1.Reverse();
        buffer.Add(intPointList1);
        break;
      case NavmeshCut.MeshType.Circle:
        List<IntPoint> intPointList2 = ListPool<IntPoint>.Claim(this.circleResolution);
        bool flag3 = flag1 ^ (double) this.circleRadius < 0.0;
        if (this.useRotation)
        {
          for (int index = 0; index < this.circleResolution; ++index)
            intPointList2.Add(NavmeshCut.V3ToIntPoint(matrix.MultiplyPoint3x4(this.center + new Vector3(Mathf.Cos((float) (index * 2) * 3.14159274f / (float) this.circleResolution), 0.0f, Mathf.Sin((float) (index * 2) * 3.14159274f / (float) this.circleResolution)) * this.circleRadius)));
        }
        else
        {
          Vector3 vector3 = position + this.center;
          for (int index = 0; index < this.circleResolution; ++index)
            intPointList2.Add(NavmeshCut.V3ToIntPoint(vector3 + new Vector3(Mathf.Cos((float) (index * 2) * 3.14159274f / (float) this.circleResolution), 0.0f, Mathf.Sin((float) (index * 2) * 3.14159274f / (float) this.circleResolution)) * this.circleRadius));
        }
        if (flag3)
          intPointList2.Reverse();
        buffer.Add(intPointList2);
        break;
      case NavmeshCut.MeshType.CustomMesh:
        if ((UnityEngine.Object) this.mesh != (UnityEngine.Object) this.lastMesh || this.contours == null)
        {
          this.CalculateMeshContour();
          this.lastMesh = this.mesh;
        }
        if (this.contours == null)
          break;
        Vector3 vector3_1 = position + this.center;
        bool flag4 = flag1 ^ (double) this.meshScale < 0.0;
        for (int index1 = 0; index1 < this.contours.Length; ++index1)
        {
          Vector3[] contour = this.contours[index1];
          List<IntPoint> intPointList3 = ListPool<IntPoint>.Claim(contour.Length);
          if (this.useRotation)
          {
            for (int index2 = 0; index2 < contour.Length; ++index2)
              intPointList3.Add(NavmeshCut.V3ToIntPoint(matrix.MultiplyPoint3x4(this.center + contour[index2] * this.meshScale)));
          }
          else
          {
            for (int index3 = 0; index3 < contour.Length; ++index3)
              intPointList3.Add(NavmeshCut.V3ToIntPoint(vector3_1 + contour[index3] * this.meshScale));
          }
          if (flag4)
            intPointList3.Reverse();
          buffer.Add(intPointList3);
        }
        break;
    }
  }

  public static IntPoint V3ToIntPoint(Vector3 p)
  {
    Int3 int3 = (Int3) p;
    return new IntPoint((long) int3.x, (long) int3.z);
  }

  public static Vector3 IntPointToV3(IntPoint p) => (Vector3) new Int3((int) p.X, 0, (int) p.Y);

  public void OnDrawGizmos()
  {
    if ((UnityEngine.Object) this.tr == (UnityEngine.Object) null)
      this.tr = this.transform;
    List<List<IntPoint>> intPointListList = ListPool<List<IntPoint>>.Claim();
    this.GetContour(intPointListList);
    Gizmos.color = NavmeshCut.GizmoColor;
    Bounds bounds = this.GetBounds();
    float y = bounds.min.y;
    Vector3 vector3 = Vector3.up * (bounds.max.y - y);
    for (int index1 = 0; index1 < intPointListList.Count; ++index1)
    {
      List<IntPoint> intPointList = intPointListList[index1];
      for (int index2 = 0; index2 < intPointList.Count; ++index2)
      {
        Vector3 v3_1 = NavmeshCut.IntPointToV3(intPointList[index2]) with
        {
          y = y
        };
        Vector3 v3_2 = NavmeshCut.IntPointToV3(intPointList[(index2 + 1) % intPointList.Count]) with
        {
          y = y
        };
        Gizmos.DrawLine(v3_1, v3_2);
        Gizmos.DrawLine(v3_1 + vector3, v3_2 + vector3);
        Gizmos.DrawLine(v3_1, v3_1 + vector3);
        Gizmos.DrawLine(v3_2, v3_2 + vector3);
      }
    }
    ListPool<List<IntPoint>>.Release(intPointListList);
  }

  public void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.Lerp(NavmeshCut.GizmoColor, new Color(1f, 1f, 1f, 0.2f), 0.9f);
    Bounds bounds = this.GetBounds();
    Gizmos.DrawCube(bounds.center, bounds.size);
    Gizmos.DrawWireCube(bounds.center, bounds.size);
  }

  public enum MeshType
  {
    Rectangle,
    Circle,
    CustomMesh,
  }
}
