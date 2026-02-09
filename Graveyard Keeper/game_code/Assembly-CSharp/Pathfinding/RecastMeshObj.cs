// Decompiled with JetBrains decompiler
// Type: Pathfinding.RecastMeshObj
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Pathfinding;

[AddComponentMenu("Pathfinding/Navmesh/RecastMeshObj")]
[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_recast_mesh_obj.php")]
public class RecastMeshObj : MonoBehaviour
{
  public static RecastBBTree tree = new RecastBBTree();
  public static List<RecastMeshObj> dynamicMeshObjs = new List<RecastMeshObj>();
  [HideInInspector]
  public Bounds bounds;
  public bool dynamic = true;
  public int area;
  public bool _dynamic;
  public bool registered;

  public static void GetAllInBounds(List<RecastMeshObj> buffer, Bounds bounds)
  {
    if (!Application.isPlaying)
    {
      RecastMeshObj[] objectsOfType = UnityEngine.Object.FindObjectsOfType(typeof (RecastMeshObj)) as RecastMeshObj[];
      for (int index = 0; index < objectsOfType.Length; ++index)
      {
        objectsOfType[index].RecalculateBounds();
        if (objectsOfType[index].GetBounds().Intersects(bounds))
          buffer.Add(objectsOfType[index]);
      }
    }
    else
    {
      if ((double) Time.timeSinceLevelLoad == 0.0)
      {
        foreach (RecastMeshObj recastMeshObj in UnityEngine.Object.FindObjectsOfType(typeof (RecastMeshObj)) as RecastMeshObj[])
          recastMeshObj.Register();
      }
      for (int index = 0; index < RecastMeshObj.dynamicMeshObjs.Count; ++index)
      {
        if (RecastMeshObj.dynamicMeshObjs[index].GetBounds().Intersects(bounds))
          buffer.Add(RecastMeshObj.dynamicMeshObjs[index]);
      }
      Rect bounds1 = Rect.MinMaxRect(bounds.min.x, bounds.min.z, bounds.max.x, bounds.max.z);
      RecastMeshObj.tree.QueryInBounds(bounds1, buffer);
    }
  }

  public void OnEnable() => this.Register();

  public void Register()
  {
    if (this.registered)
      return;
    this.registered = true;
    this.area = Mathf.Clamp(this.area, -1, 33554432 /*0x02000000*/);
    Renderer component1 = this.GetComponent<Renderer>();
    Collider component2 = this.GetComponent<Collider>();
    if ((UnityEngine.Object) component1 == (UnityEngine.Object) null && (UnityEngine.Object) component2 == (UnityEngine.Object) null)
      throw new Exception("A renderer or a collider should be attached to the GameObject");
    MeshFilter component3 = this.GetComponent<MeshFilter>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && (UnityEngine.Object) component3 == (UnityEngine.Object) null)
      throw new Exception("A renderer was attached but no mesh filter");
    this.bounds = (UnityEngine.Object) component1 != (UnityEngine.Object) null ? component1.bounds : component2.bounds;
    this._dynamic = this.dynamic;
    if (this._dynamic)
      RecastMeshObj.dynamicMeshObjs.Add(this);
    else
      RecastMeshObj.tree.Insert(this);
  }

  public void RecalculateBounds()
  {
    Renderer component1 = this.GetComponent<Renderer>();
    Collider collider = this.GetCollider();
    if ((UnityEngine.Object) component1 == (UnityEngine.Object) null && (UnityEngine.Object) collider == (UnityEngine.Object) null)
      throw new Exception("A renderer or a collider should be attached to the GameObject");
    MeshFilter component2 = this.GetComponent<MeshFilter>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && (UnityEngine.Object) component2 == (UnityEngine.Object) null)
      throw new Exception("A renderer was attached but no mesh filter");
    this.bounds = (UnityEngine.Object) component1 != (UnityEngine.Object) null ? component1.bounds : collider.bounds;
  }

  public Bounds GetBounds()
  {
    if (this._dynamic)
      this.RecalculateBounds();
    return this.bounds;
  }

  public MeshFilter GetMeshFilter() => this.GetComponent<MeshFilter>();

  public Collider GetCollider() => this.GetComponent<Collider>();

  public void OnDisable()
  {
    this.registered = false;
    if (this._dynamic)
      RecastMeshObj.dynamicMeshObjs.Remove(this);
    else if (!RecastMeshObj.tree.Remove(this))
      throw new Exception("Could not remove RecastMeshObj from tree even though it should exist in it. Has the object moved without being marked as dynamic?");
    this._dynamic = this.dynamic;
  }
}
