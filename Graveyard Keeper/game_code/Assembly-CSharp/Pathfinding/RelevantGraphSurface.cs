// Decompiled with JetBrains decompiler
// Type: Pathfinding.RelevantGraphSurface
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Pathfinding;

[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_relevant_graph_surface.php")]
[AddComponentMenu("Pathfinding/Navmesh/RelevantGraphSurface")]
public class RelevantGraphSurface : MonoBehaviour
{
  public static RelevantGraphSurface root;
  public float maxRange = 1f;
  public RelevantGraphSurface prev;
  public RelevantGraphSurface next;
  public Vector3 position;

  public Vector3 Position => this.position;

  public RelevantGraphSurface Next => this.next;

  public RelevantGraphSurface Prev => this.prev;

  public static RelevantGraphSurface Root => RelevantGraphSurface.root;

  public void UpdatePosition() => this.position = this.transform.position;

  public void OnEnable()
  {
    this.UpdatePosition();
    if ((Object) RelevantGraphSurface.root == (Object) null)
    {
      RelevantGraphSurface.root = this;
    }
    else
    {
      this.next = RelevantGraphSurface.root;
      RelevantGraphSurface.root.prev = this;
      RelevantGraphSurface.root = this;
    }
  }

  public void OnDisable()
  {
    if ((Object) RelevantGraphSurface.root == (Object) this)
    {
      RelevantGraphSurface.root = this.next;
      if ((Object) RelevantGraphSurface.root != (Object) null)
        RelevantGraphSurface.root.prev = (RelevantGraphSurface) null;
    }
    else
    {
      if ((Object) this.prev != (Object) null)
        this.prev.next = this.next;
      if ((Object) this.next != (Object) null)
        this.next.prev = this.prev;
    }
    this.prev = (RelevantGraphSurface) null;
    this.next = (RelevantGraphSurface) null;
  }

  public static void UpdateAllPositions()
  {
    for (RelevantGraphSurface relevantGraphSurface = RelevantGraphSurface.root; (Object) relevantGraphSurface != (Object) null; relevantGraphSurface = relevantGraphSurface.Next)
      relevantGraphSurface.UpdatePosition();
  }

  public static void FindAllGraphSurfaces()
  {
    RelevantGraphSurface[] objectsOfType = Object.FindObjectsOfType(typeof (RelevantGraphSurface)) as RelevantGraphSurface[];
    for (int index = 0; index < objectsOfType.Length; ++index)
    {
      objectsOfType[index].OnDisable();
      objectsOfType[index].OnEnable();
    }
  }

  public void OnDrawGizmos()
  {
    Gizmos.color = new Color(0.223529413f, 0.827451f, 0.180392161f, 0.4f);
    Gizmos.DrawLine(this.transform.position - Vector3.up * this.maxRange, this.transform.position + Vector3.up * this.maxRange);
  }

  public void OnDrawGizmosSelected()
  {
    Gizmos.color = new Color(0.223529413f, 0.827451f, 0.180392161f);
    Gizmos.DrawLine(this.transform.position - Vector3.up * this.maxRange, this.transform.position + Vector3.up * this.maxRange);
  }
}
