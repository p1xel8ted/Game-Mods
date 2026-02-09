// Decompiled with JetBrains decompiler
// Type: Pathfinding.NodeLink3
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Pathfinding;

[AddComponentMenu("Pathfinding/Link3")]
[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_node_link3.php")]
public class NodeLink3 : GraphModifier
{
  public static Dictionary<GraphNode, NodeLink3> reference = new Dictionary<GraphNode, NodeLink3>();
  public Transform end;
  public float costFactor = 1f;
  public bool oneWay;
  public NodeLink3Node startNode;
  public NodeLink3Node endNode;
  public MeshNode connectedNode1;
  public MeshNode connectedNode2;
  public Vector3 clamped1;
  public Vector3 clamped2;
  public bool postScanCalled;
  public static Color GizmosColor = new Color(0.807843149f, 0.533333361f, 0.1882353f, 0.5f);
  public static Color GizmosColorSelected = new Color(0.921568632f, 0.482352942f, 0.1254902f, 1f);

  public static NodeLink3 GetNodeLink(GraphNode node)
  {
    NodeLink3 nodeLink;
    NodeLink3.reference.TryGetValue(node, out nodeLink);
    return nodeLink;
  }

  public Transform StartTransform => this.transform;

  public Transform EndTransform => this.end;

  public GraphNode StartNode => (GraphNode) this.startNode;

  public GraphNode EndNode => (GraphNode) this.endNode;

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

  public void InternalOnPostScan()
  {
    if (AstarPath.active.astarData.pointGraph == null)
      AstarPath.active.astarData.AddGraph((NavGraph) new PointGraph());
    this.startNode = AstarPath.active.astarData.pointGraph.AddNode<NodeLink3Node>(new NodeLink3Node(AstarPath.active), (Int3) this.StartTransform.position);
    this.startNode.link = this;
    this.endNode = AstarPath.active.astarData.pointGraph.AddNode<NodeLink3Node>(new NodeLink3Node(AstarPath.active), (Int3) this.EndTransform.position);
    this.endNode.link = this;
    this.connectedNode1 = (MeshNode) null;
    this.connectedNode2 = (MeshNode) null;
    if (this.startNode == null || this.endNode == null)
    {
      this.startNode = (NodeLink3Node) null;
      this.endNode = (NodeLink3Node) null;
    }
    else
    {
      this.postScanCalled = true;
      NodeLink3.reference[(GraphNode) this.startNode] = this;
      NodeLink3.reference[(GraphNode) this.endNode] = this;
      this.Apply(true);
    }
  }

  public override void OnGraphsPostUpdate()
  {
    if (AstarPath.active.isScanning)
      return;
    if (this.connectedNode1 != null && this.connectedNode1.Destroyed)
      this.connectedNode1 = (MeshNode) null;
    if (this.connectedNode2 != null && this.connectedNode2.Destroyed)
      this.connectedNode2 = (MeshNode) null;
    if (!this.postScanCalled)
      this.OnPostScan();
    else
      this.Apply(false);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    if (!Application.isPlaying || !((UnityEngine.Object) AstarPath.active != (UnityEngine.Object) null) || AstarPath.active.astarData == null || AstarPath.active.astarData.pointGraph == null)
      return;
    this.OnGraphsPostUpdate();
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this.postScanCalled = false;
    if (this.startNode != null)
      NodeLink3.reference.Remove((GraphNode) this.startNode);
    if (this.endNode != null)
      NodeLink3.reference.Remove((GraphNode) this.endNode);
    if (this.startNode == null || this.endNode == null)
      return;
    this.startNode.RemoveConnection((GraphNode) this.endNode);
    this.endNode.RemoveConnection((GraphNode) this.startNode);
    if (this.connectedNode1 == null || this.connectedNode2 == null)
      return;
    this.startNode.RemoveConnection((GraphNode) this.connectedNode1);
    this.connectedNode1.RemoveConnection((GraphNode) this.startNode);
    this.endNode.RemoveConnection((GraphNode) this.connectedNode2);
    this.connectedNode2.RemoveConnection((GraphNode) this.endNode);
  }

  public void RemoveConnections(GraphNode node) => node.ClearConnections(true);

  [ContextMenu("Recalculate neighbours")]
  public void ContextApplyForce()
  {
    if (!Application.isPlaying)
      return;
    this.Apply(true);
    if (!((UnityEngine.Object) AstarPath.active != (UnityEngine.Object) null))
      return;
    AstarPath.active.FloodFill();
  }

  public void Apply(bool forceNewCheck)
  {
    NNConstraint none = NNConstraint.None;
    none.distanceXZ = true;
    int graphIndex = (int) this.startNode.GraphIndex;
    none.graphMask = ~(1 << graphIndex);
    bool flag1 = true;
    NNInfo nearest1 = AstarPath.active.GetNearest(this.StartTransform.position, none);
    bool flag2 = ((flag1 ? 1 : 0) & (nearest1.node != this.connectedNode1 ? 0 : (nearest1.node != null ? 1 : 0))) != 0;
    this.connectedNode1 = nearest1.node as MeshNode;
    this.clamped1 = nearest1.clampedPosition;
    if (this.connectedNode1 != null)
      Debug.DrawRay((Vector3) this.connectedNode1.position, Vector3.up * 5f, Color.red);
    NNInfo nearest2 = AstarPath.active.GetNearest(this.EndTransform.position, none);
    bool flag3 = ((flag2 ? 1 : 0) & (nearest2.node != this.connectedNode2 ? 0 : (nearest2.node != null ? 1 : 0))) != 0;
    this.connectedNode2 = nearest2.node as MeshNode;
    this.clamped2 = nearest2.clampedPosition;
    if (this.connectedNode2 != null)
      Debug.DrawRay((Vector3) this.connectedNode2.position, Vector3.up * 5f, Color.cyan);
    if (this.connectedNode2 == null || this.connectedNode1 == null)
      return;
    this.startNode.SetPosition((Int3) this.StartTransform.position);
    this.endNode.SetPosition((Int3) this.EndTransform.position);
    if (flag3 && !forceNewCheck)
      return;
    this.RemoveConnections((GraphNode) this.startNode);
    this.RemoveConnections((GraphNode) this.endNode);
    uint cost1 = (uint) Mathf.RoundToInt((float) ((Int3) (this.StartTransform.position - this.EndTransform.position)).costMagnitude * this.costFactor);
    this.startNode.AddConnection((GraphNode) this.endNode, cost1);
    this.endNode.AddConnection((GraphNode) this.startNode, cost1);
    Int3 rhs = this.connectedNode2.position - this.connectedNode1.position;
    for (int i1 = 0; i1 < this.connectedNode1.GetVertexCount(); ++i1)
    {
      Int3 vertex1 = this.connectedNode1.GetVertex(i1);
      Int3 vertex2 = this.connectedNode1.GetVertex((i1 + 1) % this.connectedNode1.GetVertexCount());
      if (Int3.DotLong((vertex2 - vertex1).Normal2D(), rhs) <= 0L)
      {
        for (int i2 = 0; i2 < this.connectedNode2.GetVertexCount(); ++i2)
        {
          Int3 vertex3 = this.connectedNode2.GetVertex(i2);
          Int3 vertex4 = this.connectedNode2.GetVertex((i2 + 1) % this.connectedNode2.GetVertexCount());
          if (Int3.DotLong((vertex4 - vertex3).Normal2D(), rhs) >= 0L && (double) Int3.Angle(vertex4 - vertex3, vertex2 - vertex1) > 2.9670598109563189)
          {
            float val1 = 0.0f;
            float num = Math.Min(1f, VectorMath.ClosestPointOnLineFactor(vertex1, vertex2, vertex3));
            val1 = Math.Max(val1, VectorMath.ClosestPointOnLineFactor(vertex1, vertex2, vertex4));
            if ((double) num < (double) val1)
            {
              Debug.LogError((object) $"Wait wut!? {val1.ToString()} {num.ToString()} {(string) vertex1} {(string) vertex2} {(string) vertex3} {(string) vertex4}\nTODO, fix this error");
            }
            else
            {
              Vector3 vector3_1 = (Vector3) (vertex2 - vertex1) * val1 + (Vector3) vertex1;
              Vector3 vector3_2 = (Vector3) (vertex2 - vertex1) * num + (Vector3) vertex1;
              this.startNode.portalA = vector3_1;
              this.startNode.portalB = vector3_2;
              this.endNode.portalA = vector3_2;
              this.endNode.portalB = vector3_1;
              MeshNode connectedNode1_1 = this.connectedNode1;
              NodeLink3Node startNode1 = this.startNode;
              Int3 int3 = (Int3) (this.clamped1 - this.StartTransform.position);
              int cost2 = Mathf.RoundToInt((float) int3.costMagnitude * this.costFactor);
              connectedNode1_1.AddConnection((GraphNode) startNode1, (uint) cost2);
              MeshNode connectedNode2_1 = this.connectedNode2;
              NodeLink3Node endNode1 = this.endNode;
              int3 = (Int3) (this.clamped2 - this.EndTransform.position);
              int cost3 = Mathf.RoundToInt((float) int3.costMagnitude * this.costFactor);
              connectedNode2_1.AddConnection((GraphNode) endNode1, (uint) cost3);
              NodeLink3Node startNode2 = this.startNode;
              MeshNode connectedNode1_2 = this.connectedNode1;
              int3 = (Int3) (this.clamped1 - this.StartTransform.position);
              int cost4 = Mathf.RoundToInt((float) int3.costMagnitude * this.costFactor);
              startNode2.AddConnection((GraphNode) connectedNode1_2, (uint) cost4);
              NodeLink3Node endNode2 = this.endNode;
              MeshNode connectedNode2_2 = this.connectedNode2;
              int3 = (Int3) (this.clamped2 - this.EndTransform.position);
              int cost5 = Mathf.RoundToInt((float) int3.costMagnitude * this.costFactor);
              endNode2.AddConnection((GraphNode) connectedNode2_2, (uint) cost5);
              return;
            }
          }
        }
      }
    }
  }

  public void DrawCircle(Vector3 o, float r, int detail, Color col)
  {
    Vector3 from = new Vector3(Mathf.Cos(0.0f) * r, 0.0f, Mathf.Sin(0.0f) * r) + o;
    Gizmos.color = col;
    for (int index = 0; index <= detail; ++index)
    {
      float f = (float) ((double) index * 3.1415927410125732 * 2.0) / (float) detail;
      Vector3 to = new Vector3(Mathf.Cos(f) * r, 0.0f, Mathf.Sin(f) * r) + o;
      Gizmos.DrawLine(from, to);
      from = to;
    }
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

  public virtual void OnDrawGizmosSelected() => this.OnDrawGizmos(true);

  public void OnDrawGizmos() => this.OnDrawGizmos(false);

  public void OnDrawGizmos(bool selected)
  {
    Color col = selected ? NodeLink3.GizmosColorSelected : NodeLink3.GizmosColor;
    if ((UnityEngine.Object) this.StartTransform != (UnityEngine.Object) null)
      this.DrawCircle(this.StartTransform.position, 0.4f, 10, col);
    if ((UnityEngine.Object) this.EndTransform != (UnityEngine.Object) null)
      this.DrawCircle(this.EndTransform.position, 0.4f, 10, col);
    if (!((UnityEngine.Object) this.StartTransform != (UnityEngine.Object) null) || !((UnityEngine.Object) this.EndTransform != (UnityEngine.Object) null))
      return;
    Gizmos.color = col;
    this.DrawGizmoBezier(this.StartTransform.position, this.EndTransform.position);
    if (!selected)
      return;
    Vector3 normalized = Vector3.Cross(Vector3.up, this.EndTransform.position - this.StartTransform.position).normalized;
    this.DrawGizmoBezier(this.StartTransform.position + normalized * 0.1f, this.EndTransform.position + normalized * 0.1f);
    this.DrawGizmoBezier(this.StartTransform.position - normalized * 0.1f, this.EndTransform.position - normalized * 0.1f);
  }

  [CompilerGenerated]
  public bool \u003COnPostScan\u003Eb__20_0(bool force)
  {
    this.InternalOnPostScan();
    return true;
  }
}
