// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.CheckUnityObject
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Category("✫ Blackboard")]
public class CheckUnityObject : ConditionTask
{
  [BlackboardOnly]
  public BBParameter<Object> valueA;
  public BBParameter<Object> valueB;

  public override string info => $"{this.valueA?.ToString()} == {this.valueB?.ToString()}";

  public override bool OnCheck() => this.valueA.value == this.valueB.value;
}
