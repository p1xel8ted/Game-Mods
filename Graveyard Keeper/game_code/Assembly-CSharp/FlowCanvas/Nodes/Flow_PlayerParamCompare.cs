// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_PlayerParamCompare
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Player Param compare", 0)]
[Category("Game Actions")]
public class Flow_PlayerParamCompare : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> par_param = this.AddValueInput<string>("param");
    ValueInput<float> par_value = this.AddValueInput<float>("value");
    FlowOutput flow_eq = this.AddFlowOutput("==");
    FlowOutput flow_neq = this.AddFlowOutput("!=");
    FlowOutput flow_more = this.AddFlowOutput(">");
    FlowOutput flow_less = this.AddFlowOutput("<");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject player = MainGame.me.player;
      string str = par_param.value;
      if (str == "_rel")
        str = "_rel_" + this.wgo.obj_id;
      string param_name = str;
      float a = player.GetParam(param_name);
      if (a.EqualsTo(par_value.value))
      {
        flow_eq.Call(f);
      }
      else
      {
        flow_neq.Call(f);
        if ((double) a > (double) par_value.value)
          flow_more.Call(f);
        if ((double) a >= (double) par_value.value)
          return;
        flow_less.Call(f);
      }
    }));
  }
}
