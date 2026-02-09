// Decompiled with JetBrains decompiler
// Type: NodeCanvas.BehaviourTrees.Inverter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.BehaviourTrees;

[Category("Decorators")]
[ParadoxNotion.Design.Icon("Remap", false, "")]
[Description("Inverts Success to Failure and Failure to Success")]
[Name("Invert", 0)]
public class Inverter : BTDecorator
{
  public override NodeCanvas.Status OnExecute(Component agent, IBlackboard blackboard)
  {
    if (this.decoratedConnection == null)
      return NodeCanvas.Status.Resting;
    this.status = this.decoratedConnection.Execute(agent, blackboard);
    switch (this.status)
    {
      case NodeCanvas.Status.Failure:
        return NodeCanvas.Status.Success;
      case NodeCanvas.Status.Success:
        return NodeCanvas.Status.Failure;
      default:
        return this.status;
    }
  }
}
