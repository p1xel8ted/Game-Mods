// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.CheckBoolean
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Category("✫ Blackboard")]
public class CheckBoolean : ConditionTask
{
  [BlackboardOnly]
  public BBParameter<bool> valueA;
  public BBParameter<bool> valueB = (BBParameter<bool>) true;

  public override string info => $"{this.valueA?.ToString()} == {this.valueB?.ToString()}";

  public override bool OnCheck() => this.valueA.value == this.valueB.value;
}
