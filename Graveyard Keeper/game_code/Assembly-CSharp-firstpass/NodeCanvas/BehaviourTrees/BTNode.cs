// Decompiled with JetBrains decompiler
// Type: NodeCanvas.BehaviourTrees.BTNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using NodeCanvas.Framework;
using ParadoxNotion;
using System;
using System.Collections.Generic;

#nullable disable
namespace NodeCanvas.BehaviourTrees;

public abstract class BTNode : NodeCanvas.Framework.Node
{
  public sealed override Type outConnectionType => typeof (BTConnection);

  public sealed override int maxInConnections => 1;

  public override int maxOutConnections => 0;

  public sealed override bool allowAsPrime => true;

  public override Alignment2x2 commentsAlignment => Alignment2x2.Bottom;

  public sealed override Alignment2x2 iconAlignment => Alignment2x2.Default;

  public T AddChild<T>(int childIndex) where T : BTNode
  {
    if (this.outConnections.Count >= this.maxOutConnections && this.maxOutConnections != -1)
      return default (T);
    T targetNode = this.graph.AddNode<T>();
    this.graph.ConnectNodes((NodeCanvas.Framework.Node) this, (NodeCanvas.Framework.Node) targetNode, childIndex);
    return targetNode;
  }

  public T AddChild<T>() where T : BTNode
  {
    if (this.outConnections.Count >= this.maxOutConnections && this.maxOutConnections != -1)
      return default (T);
    T targetNode = this.graph.AddNode<T>();
    this.graph.ConnectNodes((NodeCanvas.Framework.Node) this, (NodeCanvas.Framework.Node) targetNode);
    return targetNode;
  }

  public List<BTNode> GetAllChildNodesRecursively(bool includeThis)
  {
    List<BTNode> nodesRecursively = new List<BTNode>();
    if (includeThis)
      nodesRecursively.Add(this);
    foreach (BTNode btNode in this.outConnections.Select<Connection, NodeCanvas.Framework.Node>((Func<Connection, NodeCanvas.Framework.Node>) (c => c.targetNode)))
      nodesRecursively.AddRange((IEnumerable<BTNode>) btNode.GetAllChildNodesRecursively(true));
    return nodesRecursively;
  }

  public Dictionary<BTNode, int> GetAllChildNodesWithDepthRecursively(
    bool includeThis,
    int startIndex)
  {
    Dictionary<BTNode, int> depthRecursively = new Dictionary<BTNode, int>();
    if (includeThis)
      depthRecursively[this] = startIndex;
    foreach (BTNode btNode in this.outConnections.Select<Connection, NodeCanvas.Framework.Node>((Func<Connection, NodeCanvas.Framework.Node>) (c => c.targetNode)))
    {
      foreach (KeyValuePair<BTNode, int> keyValuePair in btNode.GetAllChildNodesWithDepthRecursively(true, startIndex + 1))
        depthRecursively[keyValuePair.Key] = keyValuePair.Value;
    }
    return depthRecursively;
  }
}
