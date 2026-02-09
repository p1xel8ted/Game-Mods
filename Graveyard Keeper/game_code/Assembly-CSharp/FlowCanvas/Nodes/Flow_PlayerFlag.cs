// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_PlayerFlag
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Player flag", 0)]
[Icon("Flag", false, "")]
[Category("Game Actions")]
public class Flow_PlayerFlag : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> par_param = this.AddValueInput<string>("param");
    FlowOutput flow_eq0 = this.AddFlowOutput("<color=red>✘</color>", "== 0");
    FlowOutput flow_moreeq1 = this.AddFlowOutput("<color=green>✔</color>", ">= 1");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      int paramInt = MainGame.me.player.GetParamInt(par_param.value);
      if (paramInt == 0)
        flow_eq0.Call(f);
      if (paramInt < 1)
        return;
      flow_moreeq1.Call(f);
    }));
  }
}
