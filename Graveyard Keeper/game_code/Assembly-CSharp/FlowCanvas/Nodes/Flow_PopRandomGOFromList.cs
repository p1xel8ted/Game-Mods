// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_PopRandomGOFromList
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Pop Random GO from List", 0)]
[Category("Game Functions")]
[Color("eed9a7")]
[FlowNode.ContextDefinedOutputs(new System.Type[] {typeof (GameObject)})]
public class Flow_PopRandomGOFromList : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<List<GameObject>> par_list = this.AddValueInput<List<GameObject>>("List");
    this.AddValueOutput<GameObject>("GameObject", (ValueHandler<GameObject>) (() =>
    {
      if (par_list.value.Count == 0)
        return (GameObject) null;
      int index = NGUITools.RandomRange(0, par_list.value.Count);
      GameObject gameObject = par_list.value[index];
      par_list.value.RemoveAt(index);
      return gameObject;
    }));
  }
}
