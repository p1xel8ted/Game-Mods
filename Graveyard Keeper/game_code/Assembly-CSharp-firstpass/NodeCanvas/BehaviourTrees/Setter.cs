// Decompiled with JetBrains decompiler
// Type: NodeCanvas.BehaviourTrees.Setter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.BehaviourTrees;

[Name("Override Agent", 0)]
[Category("Decorators")]
[Description("Set another Agent for the rest of the Tree dynamicaly from this point and on. All nodes under this will be executed for the new agent")]
[ParadoxNotion.Design.Icon("Set", false, "")]
public class Setter : BTDecorator
{
  public BBParameter<GameObject> newAgent;

  public override NodeCanvas.Status OnExecute(Component agent, IBlackboard blackboard)
  {
    if (this.decoratedConnection == null)
      return NodeCanvas.Status.Resting;
    if ((Object) this.newAgent.value != (Object) null)
      agent = (Component) this.newAgent.value.transform;
    return this.decoratedConnection.Execute(agent, blackboard);
  }
}
