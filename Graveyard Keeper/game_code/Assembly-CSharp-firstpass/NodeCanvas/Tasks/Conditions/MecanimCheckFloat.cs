// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.MecanimCheckFloat
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Category("Animator")]
[Name("Check Parameter Float", 0)]
public class MecanimCheckFloat : ConditionTask<Animator>
{
  [RequiredField]
  public BBParameter<string> parameter;
  public CompareMethod comparison;
  public BBParameter<float> value;

  public override string info
  {
    get
    {
      return $"Mec.Float {this.parameter.ToString()}{OperationTools.GetCompareString(this.comparison)}{this.value?.ToString()}";
    }
  }

  public override bool OnCheck()
  {
    return OperationTools.Compare(this.agent.GetFloat(this.parameter.value), this.value.value, this.comparison, 0.1f);
  }
}
