// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.PickListElement`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("✫ Blackboard/Lists")]
public class PickListElement<T> : ActionTask
{
  [BlackboardOnly]
  [RequiredField]
  public BBParameter<List<T>> targetList;
  public BBParameter<int> index;
  [BlackboardOnly]
  public BBParameter<T> saveAs;

  public override string info => $"{this.saveAs} = {this.targetList} [{this.index}]";

  public override void OnExecute()
  {
    if (this.index.value < 0 || this.index.value >= this.targetList.value.Count)
    {
      this.EndAction(false);
    }
    else
    {
      this.saveAs.value = this.targetList.value[this.index.value];
      this.EndAction(true);
    }
  }
}
