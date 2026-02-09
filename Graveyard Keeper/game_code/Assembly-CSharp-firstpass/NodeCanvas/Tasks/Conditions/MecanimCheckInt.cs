// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.MecanimCheckInt
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
[Name("Check Parameter Int", 0)]
public class MecanimCheckInt : ConditionTask<Animator>
{
  [RequiredField]
  public BBParameter<string> parameter;
  public CompareMethod comparison;
  public BBParameter<int> value;

  public override string info
  {
    get
    {
      return $"Mec.Int {this.parameter.ToString()}{OperationTools.GetCompareString(this.comparison)}{this.value?.ToString()}";
    }
  }

  public override bool OnCheck()
  {
    return OperationTools.Compare(this.agent.GetInteger(this.parameter.value), this.value.value, this.comparison);
  }
}
