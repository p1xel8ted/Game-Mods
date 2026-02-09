// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_FindWGOsList
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Functions")]
[ParadoxNotion.Design.Icon("Cube", false, "")]
[FlowNode.ContextDefinedOutputs(new System.Type[] {typeof (List<WorldGameObject>)})]
[Color("eed9a7")]
[Name("Find WGOs List", 0)]
public class Flow_FindWGOsList : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> par_objid = this.AddValueInput<string>("WGO obj ID", "wgo_obj_id");
    ValueInput<string> par_ctag = this.AddValueInput<string>("Custom tag", "custom_tag");
    this.AddValueOutput<List<WorldGameObject>>("List<WGO>", (ValueHandler<List<WorldGameObject>>) (() =>
    {
      if (!par_objid.isDefaultValue)
        return WorldMap.GetWorldGameObjectsByObjId(par_objid.value);
      if (!par_ctag.isDefaultValue)
        return WorldMap.GetWorldGameObjectsByCustomTag(par_ctag.value);
      Debug.LogError((object) "Flow_FindWGOsList: no input parameters set");
      return (List<WorldGameObject>) null;
    }));
  }

  public override string name
  {
    get
    {
      string name = base.name;
      if (!this.GetInputValuePort<string>("wgo_obj_id").isDefaultValue)
        return name + " by ObjID";
      return !this.GetInputValuePort<string>("custom_tag").isDefaultValue ? name + " by tag" : name + " by ??????";
    }
    set => base.name = value;
  }

  public override void OnNodeInspectorGUI()
  {
    base.OnNodeInspectorGUI();
    this.MakeStringNullIfEmpty("wgo_obj_id");
    this.MakeStringNullIfEmpty("custom_tag");
  }
}
