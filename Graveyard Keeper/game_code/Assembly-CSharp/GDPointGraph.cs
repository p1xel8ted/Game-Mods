// Decompiled with JetBrains decompiler
// Type: GDPointGraph
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GDPointGraph : PointGraph, IUpdatableGraph
{
  public override void ScanInternal(OnScanStatus status_callback)
  {
    this.nodes = new PointNode[0];
    this.nodeCount = 0;
    if (!Application.isPlaying)
    {
      MainGame.me = Object.FindObjectOfType<MainGame>();
      MainGame.me.world = Object.FindObjectOfType<World>();
    }
    if ((Object) MainGame.me == (Object) null || (Object) MainGame.me.world == (Object) null)
    {
      Debug.Log((object) "GDPointGraph: ScanInternal skipping, has no world");
    }
    else
    {
      GDPoint[] componentsInChildren = MainGame.me.world.GetComponentsInChildren<GDPoint>(true);
      foreach (GDPoint gdPoint in componentsInChildren)
      {
        gdPoint.is_graph_waypoint = false;
        gdPoint.DeInitNode();
      }
      foreach (GDPoint context in componentsInChildren)
      {
        if (context.next_gd_points.Count > 0)
          context.is_graph_waypoint = true;
        foreach (GDPoint nextGdPoint in context.next_gd_points)
        {
          if ((Object) nextGdPoint == (Object) null)
            Debug.LogError((object) $"GDPoint {context.name} has a null next_gd_point!", (Object) context);
          else
            nextGdPoint.is_graph_waypoint = true;
        }
      }
      foreach (GDPoint gdPoint in componentsInChildren)
      {
        if (gdPoint.is_graph_waypoint)
        {
          Int3 position = (Int3) gdPoint.gameObject.transform.position;
          if (position.z < 1000)
            position.z = 0;
          gdPoint.InitNode();
          this.AddNode<GDPointNode>(gdPoint.node, position);
        }
      }
      string[] strArray = new string[7]
      {
        "GDPointGraph: ScanInternal finished correctly! Nodes count: ",
        componentsInChildren.Length.ToString(),
        " (",
        null,
        null,
        null,
        null
      };
      int num = this.nodeCount;
      strArray[3] = num.ToString();
      strArray[4] = "/";
      num = this.nodes.Length;
      strArray[5] = num.ToString();
      strArray[6] = ")";
      Debug.Log((object) string.Concat(strArray));
    }
  }

  public new void UpdateArea(GraphUpdateObject o)
  {
  }

  public new void UpdateAreaInit(GraphUpdateObject o)
  {
  }

  public void UpdateGraph(List<GDPoint> gd_points_input)
  {
    this.nodes = new PointNode[0];
    this.nodeCount = 0;
    foreach (GDPoint gdPoint in gd_points_input)
    {
      gdPoint.is_graph_waypoint = false;
      gdPoint.DeInitNode();
    }
    foreach (GDPoint context in gd_points_input)
    {
      if (context.next_gd_points.Count > 0)
        context.is_graph_waypoint = true;
      foreach (GDPoint nextGdPoint in context.next_gd_points)
      {
        if ((Object) nextGdPoint == (Object) null)
          Debug.LogError((object) $"GDPoint {context.name} has a null next_gd_point!", (Object) context);
        else
          nextGdPoint.is_graph_waypoint = true;
      }
    }
    foreach (GDPoint gdPoint in gd_points_input)
    {
      if (gdPoint.is_graph_waypoint)
      {
        Int3 position = (Int3) gdPoint.gameObject.transform.position;
        if (position.z < 1000)
          position.z = 0;
        gdPoint.InitNode();
        this.AddNode<GDPointNode>(gdPoint.node, position);
      }
    }
    string[] strArray = new string[7]
    {
      "GDPointGraph: UpdateGraph finished correctly! Nodes count: ",
      gd_points_input.Count.ToString(),
      " (",
      null,
      null,
      null,
      null
    };
    int num = this.nodeCount;
    strArray[3] = num.ToString();
    strArray[4] = "/";
    num = this.nodes.Length;
    strArray[5] = num.ToString();
    strArray[6] = ")";
    Debug.Log((object) string.Concat(strArray));
  }
}
