// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_TavernEngineGetIdlePoint
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Tavern Engine get idle point", 0)]
[Category("Game Actions")]
[ParadoxNotion.Design.Icon("CubePlus", false, "")]
public class Flow_TavernEngineGetIdlePoint : MyFlowNode
{
  public GDPoint out_point;

  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_visitor = this.AddValueInput<WorldGameObject>("visitor");
    ValueInput<bool> in_change_lock = this.AddValueInput<bool>("change lock");
    this.AddValueOutput<GDPoint>("GDPoint", (ValueHandler<GDPoint>) (() => this.out_point));
    FlowOutput flow_out_ok = this.AddFlowOutput("OK");
    FlowOutput flow_out_nope = this.AddFlowOutput("Error");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (in_change_lock.value && (Object) in_visitor.value == (Object) null)
      {
        Debug.LogError((object) "Flow_TavernEngineGetIdlePoint error: in_change_lock == true but visitor WGO is null!");
        flow_out_nope.Call(f);
      }
      else
      {
        long lock_by = in_change_lock.value ? in_visitor.value.unique_id : -1L;
        if (MainGame.me.save.players_tavern_engine.TryGetAvailablePoint(out this.out_point, lock_by))
        {
          flow_out_ok.Call(f);
        }
        else
        {
          Debug.LogError((object) "Flow_TavernEngineGetIdlePoint error: not found available GDPoint!");
          flow_out_nope.Call(f);
        }
      }
    }));
  }
}
