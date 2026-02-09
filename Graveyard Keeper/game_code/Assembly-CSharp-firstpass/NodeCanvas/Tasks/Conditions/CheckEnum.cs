// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.CheckEnum
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion.Design;
using System;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Category("✫ Blackboard")]
public class CheckEnum : ConditionTask
{
  [BlackboardOnly]
  public BBObjectParameter valueA = new BBObjectParameter(typeof (Enum));
  public BBObjectParameter valueB = new BBObjectParameter(typeof (Enum));

  public override string info => $"{this.valueA?.ToString()} == {this.valueB?.ToString()}";

  public override bool OnCheck() => object.Equals(this.valueA.value, this.valueB.value);
}
