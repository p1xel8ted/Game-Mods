// Decompiled with JetBrains decompiler
// Type: NodeCanvas.BehaviourTrees.Repeater
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.BehaviourTrees;

[Category("Decorators")]
[Description("Repeat the child either x times or until it returns the specified status, or forever")]
[ParadoxNotion.Design.Icon("Repeat", false, "")]
[Name("Repeat", 0)]
public class Repeater : BTDecorator
{
  public Repeater.RepeaterMode repeaterMode;
  public Repeater.RepeatUntilStatus repeatUntilStatus = Repeater.RepeatUntilStatus.Success;
  public BBParameter<int> repeatTimes = (BBParameter<int>) 1;
  public int currentIteration = 1;

  public override NodeCanvas.Status OnExecute(Component agent, IBlackboard blackboard)
  {
    if (this.decoratedConnection == null)
      return NodeCanvas.Status.Resting;
    if (this.decoratedConnection.status == NodeCanvas.Status.Success || this.decoratedConnection.status == NodeCanvas.Status.Failure)
      this.decoratedConnection.Reset();
    this.status = this.decoratedConnection.Execute(agent, blackboard);
    switch (this.status)
    {
      case NodeCanvas.Status.Running:
        return NodeCanvas.Status.Running;
      case NodeCanvas.Status.Resting:
        return NodeCanvas.Status.Running;
      default:
        switch (this.repeaterMode)
        {
          case Repeater.RepeaterMode.RepeatTimes:
            if (this.currentIteration >= this.repeatTimes.value)
              return this.status;
            ++this.currentIteration;
            break;
          case Repeater.RepeaterMode.RepeatUntil:
            if (this.status == (NodeCanvas.Status) this.repeatUntilStatus)
              return this.status;
            break;
        }
        return NodeCanvas.Status.Running;
    }
  }

  public override void OnReset() => this.currentIteration = 1;

  public enum RepeaterMode
  {
    RepeatTimes,
    RepeatUntil,
    RepeatForever,
  }

  public enum RepeatUntilStatus
  {
    Failure,
    Success,
  }
}
