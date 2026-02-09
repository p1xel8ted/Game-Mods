// Decompiled with JetBrains decompiler
// Type: Pathfinding.NodeLink
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Pathfinding;

[AddComponentMenu("Pathfinding/Link")]
[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_node_link.php")]
public class NodeLink : GraphModifier
{
  public Transform end;
  public float costFactor = 1f;
  public bool oneWay;
  public bool deleteConnection;

  public Transform Start => this.transform;

  public Transform End => this.end;

  public override void OnPostScan()
  {
    if (AstarPath.active.isScanning)
      this.InternalOnPostScan();
    else
      AstarPath.active.AddWorkItem(new AstarPath.AstarWorkItem((Func<bool, bool>) (force =>
      {
        this.InternalOnPostScan();
        return true;
      })));
  }

  public void InternalOnPostScan() => this.Apply();

  public override void OnGraphsPostUpdate()
  {
    if (AstarPath.active.isScanning)
      return;
    AstarPath.active.AddWorkItem(new AstarPath.AstarWorkItem((Func<bool, bool>) (force =>
    {
      this.InternalOnPostScan();
      return true;
    })));
  }

  public virtual void Apply()
  {
    if ((UnityEngine.Object) this.Start == (UnityEngine.Object) null || (UnityEngine.Object) this.End == (UnityEngine.Object) null || (UnityEngine.Object) AstarPath.active == (UnityEngine.Object) null)
      return;
    GraphNode node1 = AstarPath.active.GetNearest(this.Start.position).node;
    GraphNode node2 = AstarPath.active.GetNearest(this.End.position).node;
    if (node1 == null || node2 == null)
      return;
    if (this.deleteConnection)
    {
      node1.RemoveConnection(node2);
      if (this.oneWay)
        return;
      node2.RemoveConnection(node1);
    }
    else
    {
      uint cost = (uint) Math.Round((double) (node1.position - node2.position).costMagnitude * (double) this.costFactor);
      node1.AddConnection(node2, cost);
      if (this.oneWay)
        return;
      node2.AddConnection(node1, cost);
    }
  }

  public void OnDrawGizmos()
  {
    if ((UnityEngine.Object) this.Start == (UnityEngine.Object) null || (UnityEngine.Object) this.End == (UnityEngine.Object) null)
      return;
    Vector3 position1 = this.Start.position;
    Vector3 position2 = this.End.position;
    Gizmos.color = this.deleteConnection ? Color.red : Color.green;
    this.DrawGizmoBezier(position1, position2);
  }

  public void DrawGizmoBezier(Vector3 p1, Vector3 p2)
  {
    Vector3 vector3_1 = p2 - p1;
    if (vector3_1 == Vector3.zero)
      return;
    Vector3 rhs = Vector3.Cross(Vector3.up, vector3_1);
    Vector3 vector3_2 = Vector3.Cross(vector3_1, rhs);
    vector3_2 = vector3_2.normalized;
    vector3_2 *= vector3_1.magnitude * 0.1f;
    Vector3 p1_1 = p1 + vector3_2;
    Vector3 p2_1 = p2 + vector3_2;
    Vector3 from = p1;
    for (int index = 1; index <= 20; ++index)
    {
      float t = (float) index / 20f;
      Vector3 to = AstarSplines.CubicBezier(p1, p1_1, p2_1, p2, t);
      Gizmos.DrawLine(from, to);
      from = to;
    }
  }

  [CompilerGenerated]
  public bool \u003COnPostScan\u003Eb__8_0(bool force)
  {
    this.InternalOnPostScan();
    return true;
  }

  [CompilerGenerated]
  public bool \u003COnGraphsPostUpdate\u003Eb__10_0(bool force)
  {
    this.InternalOnPostScan();
    return true;
  }
}
