// Decompiled with JetBrains decompiler
// Type: NodeCanvas.BehaviourTrees.RootSwitcher
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.BehaviourTrees;

[DoNotList]
[Category("Mutators (beta)")]
[Name("Root Switcher", 0)]
[Description("Switch the root node of the behaviour tree to a new one defined by tag\nBeta Feature!")]
public class RootSwitcher : BTNode
{
  public string targetNodeTag;
  public Node targetNode;

  public override void OnGraphStarted()
  {
    this.targetNode = this.graph.GetNodeWithTag<Node>(this.targetNodeTag);
  }

  public override NodeCanvas.Status OnExecute(Component agent, IBlackboard blackboard)
  {
    if (string.IsNullOrEmpty(this.targetNodeTag) || this.targetNode == null)
      return NodeCanvas.Status.Failure;
    if (this.graph.primeNode != this.targetNode)
      this.graph.primeNode = this.targetNode;
    return NodeCanvas.Status.Success;
  }
}
