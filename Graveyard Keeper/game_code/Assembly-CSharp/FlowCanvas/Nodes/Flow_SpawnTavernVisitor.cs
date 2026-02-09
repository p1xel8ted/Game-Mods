// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_SpawnTavernVisitor
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Spawn Tavern visitor", 0)]
[Category("Game Actions")]
[ParadoxNotion.Design.Icon("CubePlus", false, "")]
public class Flow_SpawnTavernVisitor : MyFlowNode
{
  public WorldGameObject o_wgo;

  public override void RegisterPorts()
  {
    ValueInput<GameObject> par_go = this.AddValueInput<GameObject>("Point");
    ValueInput<string> par_obj_id = this.AddValueInput<string>("Object id");
    ValueInput<string> par_custom_tag = this.AddValueInput<string>("Custom tag");
    this.AddValueOutput<WorldGameObject>("WGO", (ValueHandler<WorldGameObject>) (() => this.o_wgo));
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      this.o_wgo = GS.Spawn(par_obj_id.value, par_go.value.transform, par_custom_tag.value);
      if ((Object) this.o_wgo == (Object) null)
        Debug.LogError((object) $"Couldn't spawn: {par_obj_id.value} at {par_go.value?.ToString()}");
      else
        MainGame.me.save.players_tavern_engine.AddNewVisitor(this.o_wgo);
      flow_out.Call(f);
    }));
  }
}
