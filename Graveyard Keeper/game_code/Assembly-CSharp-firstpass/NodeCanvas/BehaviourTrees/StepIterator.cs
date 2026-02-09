// Decompiled with JetBrains decompiler
// Type: NodeCanvas.BehaviourTrees.StepIterator
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.BehaviourTrees;

[ParadoxNotion.Design.Icon("StepIterator", false, "")]
[Description("Executes AND immediately returns children node status ONE-BY-ONE. Step Sequencer always moves forward by one and loops it's index")]
[Name("Step Sequencer", 0)]
[Color("bf7fff")]
[Category("Composites")]
public class StepIterator : BTComposite
{
  public int current;

  public override string name => base.name.ToUpper();

  public override void OnGraphStarted() => this.current = 0;

  public override NodeCanvas.Status OnExecute(Component agent, IBlackboard blackboard)
  {
    this.current %= this.outConnections.Count;
    return this.outConnections[this.current].Execute(agent, blackboard);
  }

  public override void OnReset() => ++this.current;
}
