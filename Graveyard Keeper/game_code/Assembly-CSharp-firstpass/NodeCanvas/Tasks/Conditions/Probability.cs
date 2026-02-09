// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.Probability
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Description("Return true or false based on the probability settings. Outcome is calculated EACH time this is checked")]
[Category("✫ Utility")]
public class Probability : ConditionTask
{
  public BBParameter<float> probability = (BBParameter<float>) 0.5f;
  public BBParameter<float> maxValue = (BBParameter<float>) 1f;

  public override string info
  {
    get
    {
      return ((float) ((double) this.probability.value / (double) this.maxValue.value * 100.0)).ToString() + "%";
    }
  }

  public override bool OnCheck()
  {
    return (double) Random.Range(0.0f, this.maxValue.value) <= (double) this.probability.value;
  }
}
