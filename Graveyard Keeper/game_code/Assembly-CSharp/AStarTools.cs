// Decompiled with JetBrains decompiler
// Type: AStarTools
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding;
using UnityEngine;

#nullable disable
public static class AStarTools
{
  public const int SMALL_ASTAR_NODE_SIZE = 8;
  public const float SMALL_ASTAR_PLAYER_DIAMETER = 3.4f;
  public const int PLAYER_GRAPH_N = 2;

  public static void Rescan() => AstarPath.active.Scan();

  public static void RescanTile(Vector2 pos, int size = 1)
  {
    AStarTools.UpdateAstarBounds(new Bounds((Vector3) pos, (Vector3) (Vector2.one * (float) (size * 96 /*0x60*/))));
  }

  public static Vector2 ToVector2(this Int3 value)
  {
    return new Vector2((float) value.x / 1000f, (float) value.y / 1000f);
  }

  public static void RefreshPlayerGraph(Vector2 from, Vector2 to)
  {
    AStarTools.RefreshGraph(2, from, to);
  }

  public static void RefreshGraph(int graph_n, Vector2 from, Vector2 to)
  {
    Vector2 coords = (from + to) / 2f;
    Vector2 size = new Vector2(Mathf.Abs(from.x - to.x), Mathf.Abs(from.y - to.y));
    DebugDraw.DrawBox((Vector3) coords, size, Color.magenta);
    if (AstarPath.active.graphs.Length <= graph_n)
    {
      Debug.LogWarning((object) $"Can't refresh graph #{graph_n.ToString()} because current graphs length = {AstarPath.active.graphs.Length.ToString()}");
    }
    else
    {
      GridGraph graph = AstarPath.active.graphs[graph_n] as GridGraph;
      graph.nodeSize = 8f;
      Vector2 vector2 = size + Vector2.one * 25f * graph.nodeSize;
      coords.x = (float) ((double) Mathf.Round(coords.x / (graph.nodeSize * 2f)) * (double) graph.nodeSize * 2.0);
      coords.y = (float) ((double) Mathf.Round(coords.y / (graph.nodeSize * 2f)) * (double) graph.nodeSize * 2.0);
      graph.center = (Vector3) coords;
      graph.width = Mathf.CeilToInt((float) ((double) vector2.x / (double) graph.nodeSize / 2.0)) * 2;
      graph.depth = Mathf.CeilToInt((float) ((double) vector2.y / (double) graph.nodeSize / 2.0)) * 2;
      graph.collision.diameter = 3.4f;
      graph.UpdateSizeFromWidthDepth();
      AstarPath.active.Scan(graph_n);
    }
  }

  public static void UpdateAstarBounds(Vector2 coord1, Vector2 coord2)
  {
    float num1 = (double) coord1.x > (double) coord2.x ? coord1.x - coord2.x : coord2.x - coord1.x;
    float num2 = (double) coord1.y > (double) coord2.y ? coord1.y - coord2.y : coord2.y - coord1.y;
    double x1 = (double) ((double) coord1.x < (double) coord2.x ? coord1 : coord2).x + (double) num1 / 2.0;
    float num3 = ((double) coord1.y < (double) coord2.y ? coord1 : coord2).y + num2 / 2f;
    float x2 = num1 + 192f;
    float y1 = num2 + 192f;
    double y2 = (double) num3;
    AStarTools.UpdateAstarBounds(new Bounds((Vector3) new Vector2((float) x1, (float) y2), (Vector3) new Vector2(x2, y1)));
  }

  public static void UpdateAstarBounds(Bounds bounds)
  {
    GraphUpdateObject ob1 = new GraphUpdateObject(bounds)
    {
      only_specific_graph = 0
    };
    AstarPath.active.UpdateGraphs(ob1);
    GraphUpdateObject ob2 = new GraphUpdateObject(bounds)
    {
      only_specific_graph = 2
    };
    AstarPath.active.UpdateGraphs(ob2);
  }

  public static void InitialAstarScan()
  {
    if (!(AstarPath.active.graphs[0] is GridGraph graph))
    {
      Debug.LogError((object) ("GridGraph not found, total graphs = " + AstarPath.active.graphs.Length.ToString()));
    }
    else
    {
      int width = graph.Width;
      int depth = graph.Depth;
      graph.collision.collisionCheck = false;
      AstarPath.active.graphs[0] = (NavGraph) graph;
      for (int only_specific_graph = 0; only_specific_graph < AstarPath.active.graphs.Length; ++only_specific_graph)
        AstarPath.active.Scan(only_specific_graph);
      graph.collision.collisionCheck = true;
      AstarPath.active.graphs[0] = (NavGraph) graph;
    }
  }
}
