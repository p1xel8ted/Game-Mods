// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.InsertElementToList`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("✫ Blackboard/Lists")]
public class InsertElementToList<T> : ActionTask
{
  [BlackboardOnly]
  [RequiredField]
  public BBParameter<List<T>> targetList;
  public BBParameter<T> targetElement;
  public BBParameter<int> targetIndex;

  public override string info
  {
    get => $"Insert {this.targetElement} in {this.targetList} at {this.targetIndex}";
  }

  public override void OnExecute()
  {
    int index = this.targetIndex.value;
    List<T> objList = this.targetList.value;
    if (index < 0 || index >= objList.Count)
    {
      this.EndAction(false);
    }
    else
    {
      objList.Insert(index, this.targetElement.value);
      this.EndAction(true);
    }
  }
}
