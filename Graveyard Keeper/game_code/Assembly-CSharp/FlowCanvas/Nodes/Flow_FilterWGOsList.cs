// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_FilterWGOsList
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Filter WGOs List", 0)]
[Category("Game Actions")]
[Description("Filter WGOs List")]
public class Flow_FilterWGOsList : MyFlowNode
{
  public List<WorldGameObject> filtered_wgos;

  public override void RegisterPorts()
  {
    ValueInput<List<WorldGameObject>> par_wgo = this.AddValueInput<List<WorldGameObject>>("WGOs List");
    ValueInput<string> par_obj_id = this.AddValueInput<string>("Obj ID", "wgo_name");
    ValueInput<string> par_custom_tag = this.AddValueInput<string>("Custom Tag", "custom_tag");
    this.AddValueOutput<List<WorldGameObject>>("WGOs List", (ValueHandler<List<WorldGameObject>>) (() => this.filtered_wgos));
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      this.filtered_wgos = new List<WorldGameObject>();
      bool flag1 = false;
      bool flag2 = false;
      List<WorldGameObject> worldGameObjectList = par_wgo.value;
      if (worldGameObjectList == null || worldGameObjectList.Count == 0)
      {
        flow_out.Call(f);
      }
      else
      {
        if (!string.IsNullOrEmpty(par_obj_id.value))
          flag1 = true;
        if (!string.IsNullOrEmpty(par_custom_tag.value))
          flag2 = true;
        if (flag1 | flag2)
        {
          foreach (WorldGameObject worldGameObject in worldGameObjectList)
          {
            if (!((Object) worldGameObject == (Object) null) && (!flag1 || !(worldGameObject.obj_id != par_obj_id.value)) && (!flag2 || !(worldGameObject.custom_tag != par_custom_tag.value)))
              this.filtered_wgos.Add(worldGameObject);
          }
        }
        flow_out.Call(f);
      }
    }));
  }

  public override string name
  {
    get
    {
      string name = base.name;
      if (!this.GetInputValuePort<string>("wgo_name").isDefaultValue)
        return name + " by name";
      return !this.GetInputValuePort<string>("custom_tag").isDefaultValue ? name + " by tag" : name + " by ??????";
    }
    set => base.name = value;
  }

  public override void OnNodeInspectorGUI()
  {
    base.OnNodeInspectorGUI();
    this.MakeStringNullIfEmpty("wgo_name");
    this.MakeStringNullIfEmpty("custom_tag");
  }
}
