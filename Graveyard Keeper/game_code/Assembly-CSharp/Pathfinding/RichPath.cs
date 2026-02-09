// Decompiled with JetBrains decompiler
// Type: Pathfinding.RichPath
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Pathfinding;

public class RichPath
{
  public int currentPart;
  public List<RichPathPart> parts = new List<RichPathPart>();
  public Seeker seeker;

  public void Initialize(
    Seeker s,
    Path p,
    bool mergePartEndpoints,
    RichFunnel.FunnelSimplification simplificationMode)
  {
    List<GraphNode> nodes = !p.error ? p.path : throw new ArgumentException("Path has an error");
    if (nodes.Count == 0)
      throw new ArgumentException("Path traverses no nodes");
    this.seeker = s;
    for (int index = 0; index < this.parts.Count; ++index)
    {
      if (this.parts[index] is RichFunnel)
        ObjectPool<RichFunnel>.Release(this.parts[index] as RichFunnel);
      else if (this.parts[index] is RichSpecial)
        ObjectPool<RichSpecial>.Release(this.parts[index] as RichSpecial);
    }
    this.parts.Clear();
    this.currentPart = 0;
    for (int index1 = 0; index1 < nodes.Count; ++index1)
    {
      if (nodes[index1] is TriangleMeshNode)
      {
        NavGraph graph = AstarData.GetGraph(nodes[index1]);
        RichFunnel richFunnel = ObjectPool<RichFunnel>.Claim().Initialize(this, graph);
        richFunnel.funnelSimplificationMode = simplificationMode;
        int num = index1;
        uint graphIndex = nodes[num].GraphIndex;
        while (index1 < nodes.Count && ((int) nodes[index1].GraphIndex == (int) graphIndex || nodes[index1] is NodeLink3Node))
          ++index1;
        --index1;
        richFunnel.exactStart = num != 0 ? (Vector3) nodes[mergePartEndpoints ? num - 1 : num].position : p.vectorPath[0];
        richFunnel.exactEnd = index1 != nodes.Count - 1 ? (Vector3) nodes[mergePartEndpoints ? index1 + 1 : index1].position : p.vectorPath[p.vectorPath.Count - 1];
        richFunnel.BuildFunnelCorridor(nodes, num, index1);
        this.parts.Add((RichPathPart) richFunnel);
      }
      else if ((UnityEngine.Object) NodeLink2.GetNodeLink(nodes[index1]) != (UnityEngine.Object) null)
      {
        NodeLink2 nodeLink = NodeLink2.GetNodeLink(nodes[index1]);
        int index2 = index1;
        uint graphIndex = nodes[index2].GraphIndex;
        int index3 = index1 + 1;
        while (index3 < nodes.Count && (int) nodes[index3].GraphIndex == (int) graphIndex)
          ++index3;
        index1 = index3 - 1;
        if (index1 - index2 > 1)
          throw new Exception("NodeLink2 path length greater than two (2) nodes. " + (index1 - index2).ToString());
        if (index1 - index2 != 0)
          this.parts.Add((RichPathPart) ObjectPool<RichSpecial>.Claim().Initialize(nodeLink, nodes[index2]));
      }
    }
  }

  public bool PartsLeft() => this.currentPart < this.parts.Count;

  public void NextPart()
  {
    ++this.currentPart;
    if (this.currentPart < this.parts.Count)
      return;
    this.currentPart = this.parts.Count;
  }

  public RichPathPart GetCurrentPart()
  {
    return this.currentPart >= this.parts.Count ? (RichPathPart) null : this.parts[this.currentPart];
  }
}
