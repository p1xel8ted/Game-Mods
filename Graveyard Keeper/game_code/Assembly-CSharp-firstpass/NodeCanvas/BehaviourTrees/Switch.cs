// Decompiled with JetBrains decompiler
// Type: NodeCanvas.BehaviourTrees.Switch
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion.Design;
using System;
using UnityEngine;

#nullable disable
namespace NodeCanvas.BehaviourTrees;

[ParadoxNotion.Design.Icon("IndexSwitcher", false, "")]
[Description("Executes ONE child based on the provided int or enum and return it's status. If set the Dynamic and 'case' change while a child is running, that child will be interrupted before the new child is executed.")]
[Category("Composites")]
[Color("b3ff7f")]
public class Switch : BTComposite
{
  public bool dynamic;
  [BlackboardOnly]
  public BBObjectParameter enumCase = new BBObjectParameter(typeof (Enum));
  public BBParameter<int> intCase;
  public Switch.CaseSelectionMode selectionMode;
  public Switch.OutOfRangeMode outOfRangeMode = Switch.OutOfRangeMode.LoopIndex;
  public int current;
  public int runningIndex;

  public override string name => base.name.ToUpper();

  public override NodeCanvas.Status OnExecute(Component agent, IBlackboard blackboard)
  {
    if (this.outConnections.Count == 0)
      return NodeCanvas.Status.Failure;
    if (this.status == NodeCanvas.Status.Resting || this.dynamic)
    {
      if (this.selectionMode == Switch.CaseSelectionMode.IndexBased)
      {
        this.current = this.intCase.value;
        if (this.outOfRangeMode == Switch.OutOfRangeMode.LoopIndex)
          this.current = Mathf.Abs(this.current) % this.outConnections.Count;
      }
      else
        this.current = (int) this.enumCase.value;
      if (this.runningIndex != this.current)
        this.outConnections[this.runningIndex].Reset();
      if (this.current < 0 || this.current >= this.outConnections.Count)
        return NodeCanvas.Status.Failure;
    }
    this.status = this.outConnections[this.current].Execute(agent, blackboard);
    if (this.status == NodeCanvas.Status.Running)
      this.runningIndex = this.current;
    return this.status;
  }

  public enum CaseSelectionMode
  {
    IndexBased,
    EnumBased,
  }

  public enum OutOfRangeMode
  {
    ReturnFailure,
    LoopIndex,
  }
}
