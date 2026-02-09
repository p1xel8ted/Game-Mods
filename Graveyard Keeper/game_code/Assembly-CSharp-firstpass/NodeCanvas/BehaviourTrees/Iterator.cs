// Decompiled with JetBrains decompiler
// Type: NodeCanvas.BehaviourTrees.Iterator
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion.Design;
using System.Collections;
using UnityEngine;

#nullable disable
namespace NodeCanvas.BehaviourTrees;

[Description("Iterate any type of list and execute the child node for each element in the list. Keeps iterating until the Termination Condition is met or the whole list is iterated and return the child node status")]
[Category("Decorators")]
[Name("Iterate", 0)]
[ParadoxNotion.Design.Icon("List", false, "")]
public class Iterator : BTDecorator
{
  [BlackboardOnly]
  [RequiredField]
  public BBParameter<IList> targetList;
  [BlackboardOnly]
  public BBObjectParameter current;
  [BlackboardOnly]
  public BBParameter<int> storeIndex;
  public BBParameter<int> maxIteration = (BBParameter<int>) -1;
  public Iterator.TerminationConditions terminationCondition;
  public bool resetIndex = true;
  public int currentIndex;

  public IList list => this.targetList == null ? (IList) null : this.targetList.value;

  public override NodeCanvas.Status OnExecute(Component agent, IBlackboard blackboard)
  {
    if (this.decoratedConnection == null)
      return NodeCanvas.Status.Resting;
    if (this.list == null || this.list.Count == 0)
      return NodeCanvas.Status.Failure;
    for (int currentIndex = this.currentIndex; currentIndex < this.list.Count; ++currentIndex)
    {
      this.current.value = this.list[currentIndex];
      this.storeIndex.value = currentIndex;
      this.status = this.decoratedConnection.Execute(agent, blackboard);
      if (this.status == NodeCanvas.Status.Success && this.terminationCondition == Iterator.TerminationConditions.FirstSuccess)
        return NodeCanvas.Status.Success;
      if (this.status == NodeCanvas.Status.Failure && this.terminationCondition == Iterator.TerminationConditions.FirstFailure)
        return NodeCanvas.Status.Failure;
      if (this.status == NodeCanvas.Status.Running)
      {
        this.currentIndex = currentIndex;
        return NodeCanvas.Status.Running;
      }
      if (this.currentIndex == this.list.Count - 1 || this.currentIndex == this.maxIteration.value - 1)
      {
        if (this.resetIndex)
          this.currentIndex = 0;
        return this.status;
      }
      this.decoratedConnection.Reset();
      ++this.currentIndex;
    }
    return NodeCanvas.Status.Running;
  }

  public override void OnReset()
  {
    if (!this.resetIndex)
      return;
    this.currentIndex = 0;
  }

  public enum TerminationConditions
  {
    None,
    FirstSuccess,
    FirstFailure,
  }
}
