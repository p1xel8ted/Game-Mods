// Decompiled with JetBrains decompiler
// Type: NodeCanvas.BehaviourTrees.Remapper
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.BehaviourTrees;

[Name("Remap", 0)]
[Category("Decorators")]
[Description("Remap the child node's status to another status. Used to either invert the child's return status or to always return a specific status.")]
[ParadoxNotion.Design.Icon("Remap", false, "")]
public class Remapper : BTDecorator
{
  public Remapper.RemapStatus successRemap = Remapper.RemapStatus.Success;
  public Remapper.RemapStatus failureRemap;

  public override NodeCanvas.Status OnExecute(Component agent, IBlackboard blackboard)
  {
    if (this.decoratedConnection == null)
      return NodeCanvas.Status.Resting;
    this.status = this.decoratedConnection.Execute(agent, blackboard);
    switch (this.status)
    {
      case NodeCanvas.Status.Failure:
        return (NodeCanvas.Status) this.failureRemap;
      case NodeCanvas.Status.Success:
        return (NodeCanvas.Status) this.successRemap;
      default:
        return this.status;
    }
  }

  public enum RemapStatus
  {
    Failure,
    Success,
  }
}
