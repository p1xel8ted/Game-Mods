// Decompiled with JetBrains decompiler
// Type: NodeCanvas.BehaviourTrees.Parallel
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas.BehaviourTrees;

[Description("Execute all child nodes once but simultaneously and return Success or Failure depending on the selected ParallelPolicy.\nIf set to Dynamic, child nodes are repeated until the Policy set is met, or until all children have had a chance to complete at least once.")]
[Category("Composites")]
[Color("ff64cb")]
[ParadoxNotion.Design.Icon("Parallel", false, "")]
public class Parallel : BTComposite
{
  public Parallel.ParallelPolicy policy;
  public bool dynamic;
  public List<Connection> finishedConnections = new List<Connection>();

  public override string name => base.name.ToUpper();

  public override NodeCanvas.Status OnExecute(Component agent, IBlackboard blackboard)
  {
    NodeCanvas.Status status = NodeCanvas.Status.Resting;
    for (int index = 0; index < this.outConnections.Count; ++index)
    {
      if (this.dynamic || !this.finishedConnections.Contains(this.outConnections[index]))
      {
        if (this.outConnections[index].status != NodeCanvas.Status.Running && this.finishedConnections.Contains(this.outConnections[index]))
          this.outConnections[index].Reset();
        this.status = this.outConnections[index].Execute(agent, blackboard);
        if (status == NodeCanvas.Status.Resting)
        {
          if (this.status == NodeCanvas.Status.Failure && (this.policy == Parallel.ParallelPolicy.FirstFailure || this.policy == Parallel.ParallelPolicy.FirstSuccessOrFailure))
            status = NodeCanvas.Status.Failure;
          if (this.status == NodeCanvas.Status.Success && (this.policy == Parallel.ParallelPolicy.FirstSuccess || this.policy == Parallel.ParallelPolicy.FirstSuccessOrFailure))
            status = NodeCanvas.Status.Success;
        }
        if (this.status != NodeCanvas.Status.Running && !this.finishedConnections.Contains(this.outConnections[index]))
          this.finishedConnections.Add(this.outConnections[index]);
      }
    }
    if (status != NodeCanvas.Status.Resting)
    {
      this.ResetRunning();
      return status;
    }
    if (this.finishedConnections.Count == this.outConnections.Count)
    {
      this.ResetRunning();
      switch (this.policy)
      {
        case Parallel.ParallelPolicy.FirstFailure:
          return NodeCanvas.Status.Success;
        case Parallel.ParallelPolicy.FirstSuccess:
          return NodeCanvas.Status.Failure;
      }
    }
    return NodeCanvas.Status.Running;
  }

  public override void OnReset() => this.finishedConnections.Clear();

  public void ResetRunning()
  {
    for (int index = 0; index < this.outConnections.Count; ++index)
    {
      if (this.outConnections[index].status == NodeCanvas.Status.Running)
        this.outConnections[index].Reset();
    }
  }

  public enum ParallelPolicy
  {
    FirstFailure,
    FirstSuccess,
    FirstSuccessOrFailure,
  }
}
