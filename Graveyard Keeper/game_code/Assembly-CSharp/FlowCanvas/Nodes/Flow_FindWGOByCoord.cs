// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_FindWGOByCoord
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Find WGO By Coordinates", 0)]
[Category("Game Functions")]
[ParadoxNotion.Design.Icon("Cube", false, "")]
[Color("eed9a7")]
[FlowNode.ContextDefinedOutputs(new System.Type[] {typeof (WorldGameObject)})]
public class Flow_FindWGOByCoord : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> par_obj_id = this.AddValueInput<string>("Obj ID", "obj_id");
    ValueInput<string> par_ctag = this.AddValueInput<string>("Custom tag", "custom_tag");
    ValueInput<Vector2> par_coord = this.AddValueInput<Vector2>("Coords");
    this.AddValueOutput<WorldGameObject>("WGO", (ValueHandler<WorldGameObject>) (() =>
    {
      List<WorldGameObject> worldGameObjectList = new List<WorldGameObject>();
      List<WorldGameObject> wgOs = WorldMap.FindWGOs(new string[1]
      {
        par_obj_id.value
      }, par_coord.value, par_ctag.value);
      if (wgOs != null)
      {
        if (wgOs.Count != 0)
          return wgOs[0];
        Debug.LogError((object) "Flow_FindWGO: no input parameters set");
        return (WorldGameObject) null;
      }
      Debug.LogError((object) "Flow_FindWGO: wgos is null");
      return (WorldGameObject) null;
    }));
  }

  public override void OnNodeInspectorGUI()
  {
    base.OnNodeInspectorGUI();
    this.MakeStringNullIfEmpty("obj_id");
    this.MakeStringNullIfEmpty("custom_tag");
  }
}
