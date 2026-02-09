// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_PopRandomWGOFromList
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Functions")]
[Name("Pop Random WGO from List", 0)]
[Color("eed9a7")]
[FlowNode.ContextDefinedOutputs(new System.Type[] {typeof (WorldGameObject)})]
public class Flow_PopRandomWGOFromList : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<List<WorldGameObject>> par_list = this.AddValueInput<List<WorldGameObject>>("List");
    this.AddValueOutput<WorldGameObject>("WGO", (ValueHandler<WorldGameObject>) (() =>
    {
      if (par_list.value.Count == 0)
        return (WorldGameObject) null;
      int index = UnityEngine.Random.Range(0, par_list.value.Count);
      WorldGameObject worldGameObject = par_list.value[index];
      par_list.value.RemoveAt(index);
      return worldGameObject;
    }));
  }
}
