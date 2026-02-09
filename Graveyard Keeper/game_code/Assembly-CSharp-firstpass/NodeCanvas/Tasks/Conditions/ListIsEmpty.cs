// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.ListIsEmpty
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Category("✫ Blackboard/Lists")]
public class ListIsEmpty : ConditionTask
{
  [RequiredField]
  [BlackboardOnly]
  public BBParameter<IList> targetList;

  public override string info => $"{this.targetList} Is Empty";

  public override bool OnCheck() => this.targetList.value.Count == 0;
}
