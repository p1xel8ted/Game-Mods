// Decompiled with JetBrains decompiler
// Type: NodeCanvas.BehaviourTrees.Sequencer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas.BehaviourTrees;

[Color("bf7fff")]
[ParadoxNotion.Design.Icon("Sequencer", false, "")]
[Description("Execute the child nodes in order or randonly and return Success if all children return Success, else return Failure\nIf is Dynamic, higher priority child status is revaluated. If a child returns Failure the Sequencer will bail out immediately in Failure too.")]
[Category("Composites")]
public class Sequencer : BTComposite
{
  public bool dynamic;
  public bool random;
  public int lastRunningNodeIndex;

  public override string name => base.name.ToUpper();

  public override NodeCanvas.Status OnExecute(Component agent, IBlackboard blackboard)
  {
    for (int runningNodeIndex = this.dynamic ? 0 : this.lastRunningNodeIndex; runningNodeIndex < this.outConnections.Count; ++runningNodeIndex)
    {
      this.status = this.outConnections[runningNodeIndex].Execute(agent, blackboard);
      switch (this.status)
      {
        case NodeCanvas.Status.Failure:
          if (this.dynamic && runningNodeIndex < this.lastRunningNodeIndex)
          {
            for (int index = runningNodeIndex + 1; index <= this.lastRunningNodeIndex; ++index)
              this.outConnections[index].Reset();
          }
          return NodeCanvas.Status.Failure;
        case NodeCanvas.Status.Running:
          if (this.dynamic && runningNodeIndex < this.lastRunningNodeIndex)
            this.outConnections[this.lastRunningNodeIndex].Reset();
          this.lastRunningNodeIndex = runningNodeIndex;
          return NodeCanvas.Status.Running;
        default:
          continue;
      }
    }
    return NodeCanvas.Status.Success;
  }

  public override void OnReset()
  {
    this.lastRunningNodeIndex = 0;
    if (!this.random)
      return;
    this.outConnections = this.Shuffle(this.outConnections);
  }

  public override void OnChildDisconnected(int index)
  {
    if (index == 0 || index != this.lastRunningNodeIndex)
      return;
    --this.lastRunningNodeIndex;
  }

  public override void OnGraphStarted() => this.OnReset();

  public List<Connection> Shuffle(List<Connection> list)
  {
    for (int index1 = list.Count - 1; index1 > 0; --index1)
    {
      int index2 = (int) Mathf.Floor(Random.value * (float) (index1 + 1));
      Connection connection = list[index1];
      list[index1] = list[index2];
      list[index2] = connection;
    }
    return list;
  }
}
