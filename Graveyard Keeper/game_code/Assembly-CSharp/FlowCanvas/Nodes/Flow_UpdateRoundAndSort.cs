// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_UpdateRoundAndSort
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Update RoundAndSort", 0)]
[Category("Game Actions")]
[Description("Update RoundAndSort component for wgo. If wgo is null then self")]
public class Flow_UpdateRoundAndSort : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> par_wgo = this.AddValueInput<WorldGameObject>("WGO");
    WorldGameObject _wgo = (WorldGameObject) null;
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddValueOutput<WorldGameObject>("WGO", (ValueHandler<WorldGameObject>) (() => _wgo));
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(par_wgo);
      _wgo = worldGameObject;
      if ((Object) worldGameObject == (Object) null)
      {
        Debug.LogError((object) "Flow_UpdateRoundAndSort: WGO is null");
        flow_out.Call(f);
      }
      else
      {
        RoundAndSortComponent component = worldGameObject.GetComponent<RoundAndSortComponent>();
        if ((Object) component != (Object) null)
          component.DoUpdateStuff();
        flow_out.Call(f);
      }
    }));
  }
}
