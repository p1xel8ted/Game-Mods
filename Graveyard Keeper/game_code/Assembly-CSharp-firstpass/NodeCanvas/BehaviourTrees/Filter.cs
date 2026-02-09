// Decompiled with JetBrains decompiler
// Type: NodeCanvas.BehaviourTrees.Filter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections;
using UnityEngine;

#nullable disable
namespace NodeCanvas.BehaviourTrees;

[Category("Decorators")]
[Name("Filter", 0)]
[ParadoxNotion.Design.Icon("Filter", false, "")]
[Description("Filters the access of it's child node either a specific number of times, or every specific amount of time. By default the node is 'Treated as Inactive' to it's parent when child is Filtered. Unchecking this option will instead return Failure when Filtered.")]
public class Filter : BTDecorator
{
  public Filter.FilterMode filterMode = Filter.FilterMode.CoolDown;
  public BBParameter<int> maxCount = (BBParameter<int>) 1;
  public BBParameter<float> coolDownTime = (BBParameter<float>) 5f;
  public bool inactiveWhenLimited = true;
  public Filter.Policy policy;
  public int executedCount;
  public float currentTime;

  public override void OnGraphStarted()
  {
    this.executedCount = 0;
    this.currentTime = 0.0f;
  }

  public override NodeCanvas.Status OnExecute(Component agent, IBlackboard blackboard)
  {
    if (this.decoratedConnection == null)
      return NodeCanvas.Status.Resting;
    switch (this.filterMode)
    {
      case Filter.FilterMode.LimitNumberOfTimes:
        if (this.executedCount >= this.maxCount.value)
          return !this.inactiveWhenLimited ? NodeCanvas.Status.Failure : NodeCanvas.Status.Optional;
        this.status = this.decoratedConnection.Execute(agent, blackboard);
        if (this.status == NodeCanvas.Status.Success && this.policy == Filter.Policy.SuccessOnly || this.status == NodeCanvas.Status.Failure && this.policy == Filter.Policy.FailureOnly || (this.status == NodeCanvas.Status.Success || this.status == NodeCanvas.Status.Failure) && this.policy == Filter.Policy.SuccessOrFailure)
        {
          ++this.executedCount;
          break;
        }
        break;
      case Filter.FilterMode.CoolDown:
        if ((double) this.currentTime > 0.0)
          return !this.inactiveWhenLimited ? NodeCanvas.Status.Failure : NodeCanvas.Status.Optional;
        this.status = this.decoratedConnection.Execute(agent, blackboard);
        if (this.status == NodeCanvas.Status.Success || this.status == NodeCanvas.Status.Failure)
        {
          this.StartCoroutine(this.Cooldown());
          break;
        }
        break;
    }
    return this.status;
  }

  public IEnumerator Cooldown()
  {
    for (this.currentTime = this.coolDownTime.value; (double) this.currentTime > 0.0; this.currentTime -= Time.deltaTime)
      yield return (object) null;
  }

  public enum FilterMode
  {
    LimitNumberOfTimes,
    CoolDown,
  }

  public enum Policy
  {
    SuccessOrFailure,
    SuccessOnly,
    FailureOnly,
  }
}
