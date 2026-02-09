// Decompiled with JetBrains decompiler
// Type: NodeCanvas.BehaviourTrees.ProbabilitySelector
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas.BehaviourTrees;

[ParadoxNotion.Design.Icon("ProbabilitySelector", false, "")]
[Category("Composites")]
[Description("Select a child to execute based on it's chance to be selected and return Success if it returns Success, otherwise pick another.\nReturns Failure if no child returns Success or a direct 'Failure Chance' is introduced")]
[Color("b3ff7f")]
public class ProbabilitySelector : BTComposite
{
  public List<BBParameter<float>> childWeights = new List<BBParameter<float>>();
  public BBParameter<float> failChance = new BBParameter<float>();
  public float probability;
  public float currentProbability;
  public List<int> failedIndeces = new List<int>();

  public override string name => base.name.ToUpper();

  public override void OnChildConnected(int index)
  {
    List<BBParameter<float>> childWeights = this.childWeights;
    int index1 = index;
    BBParameter<float> bbParameter = new BBParameter<float>();
    bbParameter.value = 1f;
    bbParameter.bb = this.graphBlackboard;
    childWeights.Insert(index1, bbParameter);
  }

  public override void OnChildDisconnected(int index) => this.childWeights.RemoveAt(index);

  public override void OnGraphStarted() => this.OnReset();

  public override NodeCanvas.Status OnExecute(Component agent, IBlackboard blackboard)
  {
    this.currentProbability = this.probability;
    for (int index1 = 0; index1 < this.outConnections.Count; ++index1)
    {
      if (!this.failedIndeces.Contains(index1))
      {
        if ((double) this.currentProbability > (double) this.childWeights[index1].value)
        {
          this.currentProbability -= this.childWeights[index1].value;
        }
        else
        {
          this.status = this.outConnections[index1].Execute(agent, blackboard);
          if (this.status == NodeCanvas.Status.Success || this.status == NodeCanvas.Status.Running)
            return this.status;
          if (this.status == NodeCanvas.Status.Failure)
          {
            this.failedIndeces.Add(index1);
            float total = this.GetTotal();
            for (int index2 = 0; index2 < this.failedIndeces.Count; ++index2)
              total -= this.childWeights[index2].value;
            this.probability = Random.Range(0.0f, total);
            return NodeCanvas.Status.Running;
          }
        }
      }
    }
    return NodeCanvas.Status.Failure;
  }

  public override void OnReset()
  {
    this.failedIndeces.Clear();
    this.probability = Random.Range(0.0f, this.GetTotal());
  }

  public float GetTotal()
  {
    float total = this.failChance.value;
    foreach (BBParameter<float> childWeight in this.childWeights)
      total += childWeight.value;
    return total;
  }
}
