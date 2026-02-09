// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.ListContainsElement`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Category("✫ Blackboard/Lists")]
[Description("Check if an element is contained in the target list")]
public class ListContainsElement<T> : ConditionTask
{
  [RequiredField]
  [BlackboardOnly]
  public BBParameter<List<T>> targetList;
  public BBParameter<T> checkElement;

  public override string info
  {
    get => $"{this.targetList?.ToString()} contains {this.checkElement?.ToString()}";
  }

  public override bool OnCheck() => this.targetList.value.Contains(this.checkElement.value);
}
