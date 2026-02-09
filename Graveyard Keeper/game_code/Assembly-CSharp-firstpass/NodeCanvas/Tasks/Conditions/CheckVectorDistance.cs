// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.CheckVectorDistance
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Category("✫ Blackboard")]
public class CheckVectorDistance : ConditionTask
{
  [BlackboardOnly]
  public BBParameter<Vector3> vectorA;
  [BlackboardOnly]
  public BBParameter<Vector3> vectorB;
  public CompareMethod comparison;
  public BBParameter<float> distance;

  public override string info
  {
    get
    {
      return $"Distance ({this.vectorA}, {this.vectorB}) {OperationTools.GetCompareString(this.comparison)} {this.distance}";
    }
  }

  public override bool OnCheck()
  {
    return OperationTools.Compare(Vector3.Distance(this.vectorA.value, this.vectorB.value), this.distance.value, this.comparison, 0.0f);
  }
}
