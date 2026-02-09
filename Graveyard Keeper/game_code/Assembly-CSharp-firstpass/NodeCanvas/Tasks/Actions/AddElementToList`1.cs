// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.AddElementToList`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("✫ Blackboard/Lists")]
public class AddElementToList<T> : ActionTask
{
  [BlackboardOnly]
  [RequiredField]
  public BBParameter<List<T>> targetList;
  public BBParameter<T> targetElement;

  public override string info => $"Add {this.targetElement} In {this.targetList}";

  public override void OnExecute()
  {
    this.targetList.value.Add(this.targetElement.value);
    this.EndAction();
  }
}
