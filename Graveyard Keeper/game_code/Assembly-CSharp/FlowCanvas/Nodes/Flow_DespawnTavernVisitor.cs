// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_DespawnTavernVisitor
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Despawn Tavern visitor", 0)]
[Category("Game Actions")]
[ParadoxNotion.Design.Icon("CubePlus", false, "")]
public class Flow_DespawnTavernVisitor : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_visitor = this.AddValueInput<WorldGameObject>("Visitor WGO");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject visitor_wgo = this.WGOParamOrSelf(in_visitor);
      if ((Object) visitor_wgo == (Object) null)
      {
        Debug.LogError((object) "Flow_DespawnTavernVisitor error: WGO is null!");
        flow_out.Call(f);
      }
      else
      {
        MainGame.me.save.players_tavern_engine.RemoveVisitor(visitor_wgo);
        visitor_wgo.DestroyMe();
        flow_out.Call(f);
      }
    }));
  }
}
