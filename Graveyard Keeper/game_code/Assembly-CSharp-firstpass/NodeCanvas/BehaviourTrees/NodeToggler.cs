// Decompiled with JetBrains decompiler
// Type: NodeCanvas.BehaviourTrees.NodeToggler
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas.BehaviourTrees;

[Description("Enable, Disable or Toggle one or more nodes with provided tag. In practise their incomming connections are disabled\nBeta Feature!")]
[Category("Mutators (beta)")]
[Name("Node Toggler", 0)]
[DoNotList]
public class NodeToggler : BTNode
{
  public NodeToggler.ToggleMode toggleMode = NodeToggler.ToggleMode.Toggle;
  public string targetNodeTag;
  public List<NodeCanvas.Framework.Node> targetNodes;

  public override void OnGraphStarted()
  {
    this.targetNodes = this.graph.GetNodesWithTag<NodeCanvas.Framework.Node>(this.targetNodeTag);
  }

  public override NodeCanvas.Status OnExecute(Component agent, IBlackboard blackboard)
  {
    if (string.IsNullOrEmpty(this.targetNodeTag) || this.targetNodes.Count == 0)
      return NodeCanvas.Status.Failure;
    if (this.toggleMode == NodeToggler.ToggleMode.Enable)
    {
      foreach (NodeCanvas.Framework.Node targetNode in this.targetNodes)
        targetNode.inConnections[0].isActive = true;
    }
    if (this.toggleMode == NodeToggler.ToggleMode.Disable)
    {
      foreach (NodeCanvas.Framework.Node targetNode in this.targetNodes)
        targetNode.inConnections[0].isActive = false;
    }
    if (this.toggleMode == NodeToggler.ToggleMode.Toggle)
    {
      foreach (NodeCanvas.Framework.Node targetNode in this.targetNodes)
        targetNode.inConnections[0].isActive = !targetNode.inConnections[0].isActive;
    }
    return NodeCanvas.Status.Success;
  }

  public enum ToggleMode
  {
    Enable,
    Disable,
    Toggle,
  }
}
