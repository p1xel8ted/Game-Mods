// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_AddRelation
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Add Relation", 0)]
[Color("f386ca")]
[Category("Game Actions")]
public class Flow_AddRelation : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> in_param_name = this.AddValueInput<string>("NPC id (\"\" if self)", "npc_id");
    ValueInput<float> in_value = this.AddValueInput<float>("Value");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      string objId = in_param_name.value;
      if (string.IsNullOrEmpty(objId))
        objId = this.wgo.obj_id;
      MainGame.me.player.AddToParams("_rel_" + objId, in_value.value);
      flow_out.Call(f);
    }));
  }
}
