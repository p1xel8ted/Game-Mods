// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.CheckSpeed
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Description("Checks the current speed of the agent against a value based on it's Rigidbody velocity")]
[Category("GameObject")]
public class CheckSpeed : ConditionTask<Rigidbody>
{
  public CompareMethod checkType;
  public BBParameter<float> value;
  [SliderField(0.0f, 0.1f)]
  public float differenceThreshold = 0.05f;

  public override string info
  {
    get => $"Speed{OperationTools.GetCompareString(this.checkType)}{this.value?.ToString()}";
  }

  public override bool OnCheck()
  {
    return OperationTools.Compare(this.agent.velocity.magnitude, this.value.value, this.checkType, this.differenceThreshold);
  }
}
