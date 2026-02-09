// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_FindWGO
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[FlowNode.ContextDefinedOutputs(new System.Type[] {typeof (WorldGameObject)})]
[Name("Find WGO", 0)]
[Category("Game Functions")]
[Color("eed9a7")]
[ParadoxNotion.Design.Icon("Cube", false, "")]
public class Flow_FindWGO : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> par_obj_id = this.AddValueInput<string>("Obj ID", "obj_id");
    ValueInput<string> par_ctag = this.AddValueInput<string>("Custom tag", "custom_tag");
    ValueInput<bool> par_ignore_error = this.AddValueInput<bool>("Ignore not found err", "ignore_error");
    this.AddValueOutput<WorldGameObject>("WGO", (ValueHandler<WorldGameObject>) (() =>
    {
      if (par_obj_id.HasValue<string>())
        return WorldMap.GetWorldGameObjectByObjId(par_obj_id.value, par_ignore_error.value);
      if (par_ctag.HasValue<string>())
        return WorldMap.GetWorldGameObjectByCustomTag(par_ctag.value, par_ignore_error.value);
      Debug.LogError((object) "Flow_FindWGO: no input parameters set");
      return (WorldGameObject) null;
    }));
  }

  public override string name
  {
    get
    {
      string name = base.name;
      if (!this.GetInputValuePort<string>("obj_id").isDefaultValue || this.GetInputValuePort<string>("obj_id").isConnected)
        return name + " by ObjID";
      return !this.GetInputValuePort<string>("custom_tag").isDefaultValue || this.GetInputValuePort<string>("custom_tag").isConnected ? name + " by tag" : name + " by ??????";
    }
    set => base.name = value;
  }

  public override void OnNodeInspectorGUI()
  {
    base.OnNodeInspectorGUI();
    this.MakeStringNullIfEmpty("obj_id");
    this.MakeStringNullIfEmpty("custom_tag");
  }
}
