// Decompiled with JetBrains decompiler
// Type: Pathfinding.GraphUpdateUtilities
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding.Util;
using System.Collections.Generic;

#nullable disable
namespace Pathfinding;

public static class GraphUpdateUtilities
{
  public static bool UpdateGraphsNoBlock(
    GraphUpdateObject guo,
    GraphNode node1,
    GraphNode node2,
    bool alwaysRevert = false)
  {
    List<GraphNode> graphNodeList = ListPool<GraphNode>.Claim();
    graphNodeList.Add(node1);
    graphNodeList.Add(node2);
    int num = GraphUpdateUtilities.UpdateGraphsNoBlock(guo, graphNodeList, alwaysRevert) ? 1 : 0;
    ListPool<GraphNode>.Release(graphNodeList);
    return num != 0;
  }

  public static bool UpdateGraphsNoBlock(
    GraphUpdateObject guo,
    List<GraphNode> nodes,
    bool alwaysRevert = false)
  {
    for (int index = 0; index < nodes.Count; ++index)
    {
      if (!nodes[index].Walkable)
        return false;
    }
    guo.trackChangedNodes = true;
    bool worked = true;
    AstarPath.RegisterSafeUpdate((System.Action) (() =>
    {
      AstarPath.active.UpdateGraphs(guo);
      AstarPath.active.FlushGraphUpdates();
      worked = worked && PathUtilities.IsPathPossible(nodes);
      if (!(!worked | alwaysRevert))
        return;
      guo.RevertFromBackup();
      AstarPath.active.FloodFill();
    }));
    AstarPath.active.FlushThreadSafeCallbacks();
    guo.trackChangedNodes = false;
    return worked;
  }
}
