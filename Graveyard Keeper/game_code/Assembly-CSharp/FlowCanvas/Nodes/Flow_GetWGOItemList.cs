// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_GetWGOItemList
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Color("eed9a7")]
[ParadoxNotion.Design.Icon("Cube", false, "")]
[Category("Game Functions")]
[Name("Get WGO List Item", 0)]
[FlowNode.ContextDefinedOutputs(new System.Type[] {typeof (List<WorldGameObject>)})]
public class Flow_GetWGOItemList : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> par_me_wgo = this.AddValueInput<WorldGameObject>("WGO");
    this.AddValueOutput<List<Item>>("List<Item>", (ValueHandler<List<Item>>) (() =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(par_me_wgo);
      if (!((UnityEngine.Object) worldGameObject == (UnityEngine.Object) null))
        return worldGameObject.data.inventory;
      Debug.LogError((object) "WGO is null!");
      return (List<Item>) null;
    }));
  }
}
