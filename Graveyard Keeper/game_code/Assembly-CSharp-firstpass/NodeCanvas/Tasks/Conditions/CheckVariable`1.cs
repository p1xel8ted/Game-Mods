// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.CheckVariable`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Description("It's best to use the respective Condition for a type if existant since they support operations as well")]
[Category("✫ Blackboard")]
public class CheckVariable<T> : ConditionTask
{
  [BlackboardOnly]
  public BBParameter<T> valueA;
  public BBParameter<T> valueB;

  public override string info => $"{this.valueA?.ToString()} == {this.valueB?.ToString()}";

  public override bool OnCheck()
  {
    return object.Equals((object) this.valueA.value, (object) this.valueB.value);
  }
}
