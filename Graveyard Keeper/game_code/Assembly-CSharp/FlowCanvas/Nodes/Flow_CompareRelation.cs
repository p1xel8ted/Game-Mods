// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_CompareRelation
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Compare Relation", 0)]
public class Flow_CompareRelation : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> in_param_name = this.AddValueInput<string>("NPC id (\"\" if self)", "npc_id");
    ValueInput<float> in_value = this.AddValueInput<float>("Value");
    FlowOutput flow_equal = this.AddFlowOutput("rel == value");
    FlowOutput flow_more = this.AddFlowOutput("rel > value");
    FlowOutput flow_less = this.AddFlowOutput("rel < value");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      string objId = in_param_name.value;
      if (string.IsNullOrEmpty(objId))
        objId = this.wgo.obj_id;
      float f1 = MainGame.me.player.GetParam("_rel_" + objId) - in_value.value;
      if ((double) Mathf.Abs(f1) < 0.0099999997764825821)
        flow_equal.Call(f);
      else if ((double) f1 > 0.0)
        flow_more.Call(f);
      else
        flow_less.Call(f);
    }));
  }
}
