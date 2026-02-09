// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_TavernEngineNeedToSpawn
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Tavern Engine Need to spawn visitor", 0)]
[Category("Game Actions")]
public class Flow_TavernEngineNeedToSpawn : MyFlowNode
{
  public override void RegisterPorts()
  {
    FlowOutput flow_need_spawn = this.AddFlowOutput("Need to spawn");
    FlowOutput flow_do_nothing = this.AddFlowOutput("Do nothing");
    FlowOutput flow_need_to_remove = this.AddFlowOutput("Need to remove");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      int count = MainGame.me.save.players_tavern_engine.visitors.Count;
      int num = Mathf.FloorToInt((float) MainGame.me.save.players_tavern_engine.locks.Count * 0.6f) - count;
      if (num > 0)
        flow_need_spawn.Call(f);
      else if (num < 0)
        flow_need_to_remove.Call(f);
      else
        flow_do_nothing.Call(f);
    }));
  }
}
