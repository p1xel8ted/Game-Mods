// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_TavernEngineAddGDPoint
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Tavern Engine Add GDPoint", 0)]
public class Flow_TavernEngineAddGDPoint : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<GDPoint> in_gd_point = this.AddValueInput<GDPoint>("GDPoint");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if ((Object) in_gd_point.value == (Object) null)
        Debug.LogError((object) "Flow_TavernEngineAddGDPoint error: GDPoint is null!");
      else
        MainGame.me.save.players_tavern_engine.AddGDPoint(in_gd_point.value);
      flow_out.Call(f);
    }));
  }
}
