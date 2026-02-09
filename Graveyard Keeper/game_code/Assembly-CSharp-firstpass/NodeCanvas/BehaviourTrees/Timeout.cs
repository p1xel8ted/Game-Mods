// Decompiled with JetBrains decompiler
// Type: NodeCanvas.BehaviourTrees.Timeout
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.BehaviourTrees;

[Category("Decorators")]
[Description("Interupts decorated child node and returns Failure if the child node is still Running after the timeout period")]
[ParadoxNotion.Design.Icon("Timeout", false, "")]
public class Timeout : BTDecorator
{
  public BBParameter<float> timeout = (BBParameter<float>) 1f;
  public float timer;

  public override NodeCanvas.Status OnExecute(Component agent, IBlackboard blackboard)
  {
    if (this.decoratedConnection == null)
      return NodeCanvas.Status.Resting;
    this.status = this.decoratedConnection.Execute(agent, blackboard);
    if (this.status == NodeCanvas.Status.Running)
      this.timer += Time.deltaTime;
    if ((double) this.timer < (double) this.timeout.value)
      return this.status;
    this.timer = 0.0f;
    this.decoratedConnection.Reset();
    return NodeCanvas.Status.Failure;
  }

  public override void OnReset() => this.timer = 0.0f;
}
