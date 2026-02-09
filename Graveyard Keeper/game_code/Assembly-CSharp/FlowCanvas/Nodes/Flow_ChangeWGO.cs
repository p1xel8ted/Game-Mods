// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_ChangeWGO
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Change WGO", 0)]
[Category("Game Actions")]
[ParadoxNotion.Design.Icon("CubeArrowCube", false, "")]
[Description("If WGO is null, then self")]
public class Flow_ChangeWGO : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> par_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<string> par_obj_id = this.AddValueInput<string>("obj_id");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(par_wgo);
      if ((Object) worldGameObject == (Object) null)
        return;
      worldGameObject.ReplaceWithObject(par_obj_id.value);
      worldGameObject.Redraw();
      flow_out.Call(f);
    }));
  }

  public override string name
  {
    get
    {
      string name = base.name;
      return this.IsEmptyStringInputPort("obj_id") ? name + "\n<color=red>obj_id is empty</color>" : name;
    }
    set => base.name = value;
  }
}
