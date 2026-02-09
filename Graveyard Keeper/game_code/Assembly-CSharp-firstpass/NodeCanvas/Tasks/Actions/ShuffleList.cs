// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.ShuffleList
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("✫ Blackboard/Lists")]
public class ShuffleList : ActionTask
{
  [BlackboardOnly]
  [RequiredField]
  public BBParameter<IList> targetList;

  public override void OnExecute()
  {
    IList list = this.targetList.value;
    for (int index1 = list.Count - 1; index1 > 0; --index1)
    {
      int index2 = (int) Mathf.Floor(Random.value * (float) (index1 + 1));
      object obj = list[index1];
      list[index1] = list[index2];
      list[index2] = obj;
    }
    this.EndAction();
  }
}
