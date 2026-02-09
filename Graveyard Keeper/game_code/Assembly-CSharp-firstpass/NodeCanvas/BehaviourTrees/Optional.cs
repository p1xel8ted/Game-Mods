// Decompiled with JetBrains decompiler
// Type: NodeCanvas.BehaviourTrees.Optional
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.BehaviourTrees;

[Description("Executes the decorated node without taking into account it's return status, thus making it optional to the parent node for whether it returns Success or Failure.\nThis has the same effect as disabling the node, but instead it executes normaly")]
[ParadoxNotion.Design.Icon("UpwardsArrow", false, "")]
[Name("Optional", 0)]
[Category("Decorators")]
public class Optional : BTDecorator
{
  public override NodeCanvas.Status OnExecute(Component agent, IBlackboard blackboard)
  {
    if (this.decoratedConnection == null)
      return NodeCanvas.Status.Optional;
    if (this.status == NodeCanvas.Status.Resting)
      this.decoratedConnection.Reset();
    this.status = this.decoratedConnection.Execute(agent, blackboard);
    return this.status != NodeCanvas.Status.Running ? NodeCanvas.Status.Optional : NodeCanvas.Status.Running;
  }
}
