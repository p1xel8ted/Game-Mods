// Decompiled with JetBrains decompiler
// Type: Pathfinding.NavmeshAdd
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding.Util;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Pathfinding;

[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_navmesh_add.php")]
public class NavmeshAdd : MonoBehaviour
{
  public static List<NavmeshAdd> allCuts = new List<NavmeshAdd>();
  public NavmeshAdd.MeshType type;
  public Mesh mesh;
  public Vector3[] verts;
  public int[] tris;
  public Vector2 rectangleSize = new Vector2(1f, 1f);
  public float meshScale = 1f;
  public Vector3 center;
  public Bounds bounds;
  public bool useRotation;
  public Transform tr;
  public static Color GizmoColor = new Color(0.368627459f, 0.9372549f, 0.145098045f);

  public static void Add(NavmeshAdd obj) => NavmeshAdd.allCuts.Add(obj);

  public static void Remove(NavmeshAdd obj) => NavmeshAdd.allCuts.Remove(obj);

  public static List<NavmeshAdd> GetAllInRange(Bounds b)
  {
    List<NavmeshAdd> allInRange = ListPool<NavmeshAdd>.Claim();
    for (int index = 0; index < NavmeshAdd.allCuts.Count; ++index)
    {
      if (NavmeshAdd.allCuts[index].enabled && NavmeshAdd.Intersects(b, NavmeshAdd.allCuts[index].GetBounds()))
        allInRange.Add(NavmeshAdd.allCuts[index]);
    }
    return allInRange;
  }

  public static bool Intersects(Bounds b1, Bounds b2)
  {
    Vector3 min1 = b1.min;
    Vector3 max1 = b1.max;
    Vector3 min2 = b2.min;
    Vector3 max2 = b2.max;
    return (double) min1.x <= (double) max2.x && (double) max1.x >= (double) min2.x && (double) min1.z <= (double) max2.z && (double) max1.z >= (double) min2.z;
  }

  public static List<NavmeshAdd> GetAll() => NavmeshAdd.allCuts;

  public void Awake() => NavmeshAdd.Add(this);

  public void OnEnable() => this.tr = this.transform;

  public void OnDestroy() => NavmeshAdd.Remove(this);

  public Vector3 Center
  {
    get
    {
      return this.tr.position + (this.useRotation ? this.tr.TransformPoint(this.center) : this.center);
    }
  }

  [ContextMenu("Rebuild Mesh")]
  public void RebuildMesh()
  {
    if (this.type == NavmeshAdd.MeshType.CustomMesh)
    {
      if ((Object) this.mesh == (Object) null)
      {
        this.verts = (Vector3[]) null;
        this.tris = (int[]) null;
      }
      else
      {
        this.verts = this.mesh.vertices;
        this.tris = this.mesh.triangles;
      }
    }
    else
    {
      if (this.verts == null || this.verts.Length != 4 || this.tris == null || this.tris.Length != 6)
      {
        this.verts = new Vector3[4];
        this.tris = new int[6];
      }
      this.tris[0] = 0;
      this.tris[1] = 1;
      this.tris[2] = 2;
      this.tris[3] = 0;
      this.tris[4] = 2;
      this.tris[5] = 3;
      this.verts[0] = new Vector3((float) (-(double) this.rectangleSize.x * 0.5), 0.0f, (float) (-(double) this.rectangleSize.y * 0.5));
      this.verts[1] = new Vector3(this.rectangleSize.x * 0.5f, 0.0f, (float) (-(double) this.rectangleSize.y * 0.5));
      this.verts[2] = new Vector3(this.rectangleSize.x * 0.5f, 0.0f, this.rectangleSize.y * 0.5f);
      this.verts[3] = new Vector3((float) (-(double) this.rectangleSize.x * 0.5), 0.0f, this.rectangleSize.y * 0.5f);
    }
  }

  public Bounds GetBounds()
  {
    switch (this.type)
    {
      case NavmeshAdd.MeshType.Rectangle:
        if (this.useRotation)
        {
          Matrix4x4 matrix4x4 = Matrix4x4.TRS(this.tr.position, this.tr.rotation, Vector3.one);
          this.bounds = new Bounds(matrix4x4.MultiplyPoint3x4(this.center + new Vector3(-this.rectangleSize.x, 0.0f, -this.rectangleSize.y) * 0.5f), Vector3.zero);
          this.bounds.Encapsulate(matrix4x4.MultiplyPoint3x4(this.center + new Vector3(this.rectangleSize.x, 0.0f, -this.rectangleSize.y) * 0.5f));
          this.bounds.Encapsulate(matrix4x4.MultiplyPoint3x4(this.center + new Vector3(this.rectangleSize.x, 0.0f, this.rectangleSize.y) * 0.5f));
          this.bounds.Encapsulate(matrix4x4.MultiplyPoint3x4(this.center + new Vector3(-this.rectangleSize.x, 0.0f, this.rectangleSize.y) * 0.5f));
          break;
        }
        this.bounds = new Bounds(this.tr.position + this.center, new Vector3(this.rectangleSize.x, 0.0f, this.rectangleSize.y));
        break;
      case NavmeshAdd.MeshType.CustomMesh:
        if (!((Object) this.mesh == (Object) null))
        {
          Bounds bounds = this.mesh.bounds;
          if (this.useRotation)
          {
            Matrix4x4 matrix4x4 = Matrix4x4.TRS(this.tr.position, this.tr.rotation, Vector3.one * this.meshScale);
            this.bounds = new Bounds(matrix4x4.MultiplyPoint3x4(this.center + bounds.center), Vector3.zero);
            Vector3 max = bounds.max;
            Vector3 min = bounds.min;
            this.bounds.Encapsulate(matrix4x4.MultiplyPoint3x4(this.center + new Vector3(max.x, min.y, max.z)));
            this.bounds.Encapsulate(matrix4x4.MultiplyPoint3x4(this.center + new Vector3(min.x, min.y, max.z)));
            this.bounds.Encapsulate(matrix4x4.MultiplyPoint3x4(this.center + new Vector3(min.x, max.y, min.z)));
            this.bounds.Encapsulate(matrix4x4.MultiplyPoint3x4(this.center + new Vector3(max.x, max.y, min.z)));
            break;
          }
          Vector3 size = bounds.size * this.meshScale;
          this.bounds = new Bounds(this.transform.position + this.center + bounds.center * this.meshScale, size);
          break;
        }
        break;
    }
    return this.bounds;
  }

  public void GetMesh(Int3 offset, ref Int3[] vbuffer, out int[] tbuffer)
  {
    if (this.verts == null)
      this.RebuildMesh();
    if (this.verts == null)
    {
      tbuffer = new int[0];
    }
    else
    {
      if (vbuffer == null || vbuffer.Length < this.verts.Length)
        vbuffer = new Int3[this.verts.Length];
      tbuffer = this.tris;
      if (this.useRotation)
      {
        Matrix4x4 matrix4x4 = Matrix4x4.TRS(this.tr.position + this.center, this.tr.rotation, this.tr.localScale * this.meshScale);
        for (int index = 0; index < this.verts.Length; ++index)
          vbuffer[index] = offset + (Int3) matrix4x4.MultiplyPoint3x4(this.verts[index]);
      }
      else
      {
        Vector3 vector3 = this.tr.position + this.center;
        for (int index = 0; index < this.verts.Length; ++index)
          vbuffer[index] = offset + (Int3) (vector3 + this.verts[index] * this.meshScale);
      }
    }
  }

  public enum MeshType
  {
    Rectangle,
    CustomMesh,
  }
}
