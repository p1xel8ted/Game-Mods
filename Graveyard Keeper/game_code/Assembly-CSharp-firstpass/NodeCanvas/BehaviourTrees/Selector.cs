// Decompiled with JetBrains decompiler
// Type: NodeCanvas.BehaviourTrees.Selector
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas.BehaviourTrees;

[Color("b3ff7f")]
[ParadoxNotion.Design.Icon("Selector", false, "")]
[Category("Composites")]
[Description("Execute the child nodes in order or randonly until the first that returns Success and return Success as well. If none returns Success, then returns Failure.\nIf is Dynamic, then higher priority children Status are revaluated and if one returns Success the Selector will select that one and bail out immediately in Success too")]
public class Selector : BTComposite
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
        case NodeCanvas.Status.Success:
          if (this.dynamic && runningNodeIndex < this.lastRunningNodeIndex)
          {
            for (int index = runningNodeIndex + 1; index <= this.lastRunningNodeIndex; ++index)
              this.outConnections[index].Reset();
          }
          return NodeCanvas.Status.Success;
        case NodeCanvas.Status.Running:
          if (this.dynamic && runningNodeIndex < this.lastRunningNodeIndex)
            this.outConnections[this.lastRunningNodeIndex].Reset();
          this.lastRunningNodeIndex = runningNodeIndex;
          return NodeCanvas.Status.Running;
        default:
          continue;
      }
    }
    return NodeCanvas.Status.Failure;
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
