// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.CheckDistanceToGameObject
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Category("GameObject")]
[Name("Target Within Distance", 0)]
public class CheckDistanceToGameObject : ConditionTask<Transform>
{
  [RequiredField]
  public BBParameter<GameObject> checkTarget;
  public CompareMethod checkType = CompareMethod.LessThan;
  public BBParameter<float> distance = (BBParameter<float>) 10f;
  [SliderField(0.0f, 0.1f)]
  public float floatingPoint = 0.05f;

  public override string info
  {
    get
    {
      return $"Distance{OperationTools.GetCompareString(this.checkType)}{this.distance?.ToString()} to {this.checkTarget?.ToString()}";
    }
  }

  public override bool OnCheck()
  {
    return OperationTools.Compare(Vector3.Distance(this.agent.position, this.checkTarget.value.transform.position), this.distance.value, this.checkType, this.floatingPoint);
  }

  public override void OnDrawGizmosSelected()
  {
    if (!((Object) this.agent != (Object) null))
      return;
    Gizmos.DrawWireSphere(this.agent.position, this.distance.value);
  }
}
