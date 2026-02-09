// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.PickRandomListElement`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("✫ Blackboard/Lists")]
public class PickRandomListElement<T> : ActionTask
{
  [RequiredField]
  public BBParameter<List<T>> targetList;
  public BBParameter<T> saveAs;

  public override string info => $"{this.saveAs} = Random From {this.targetList}";

  public override void OnExecute()
  {
    if (this.targetList.value.Count <= 0)
    {
      this.EndAction(false);
    }
    else
    {
      this.saveAs.value = this.targetList.value[Random.Range(0, this.targetList.value.Count)];
      this.EndAction(true);
    }
  }
}
