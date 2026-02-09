// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_AddMoney
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("If WGO is null, then self")]
[Name("Add Money", 0)]
[Category("Game Actions")]
public class Flow_AddMoney : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<float> in_value = this.AddValueInput<float>("Value");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      MainGame.me.player.AddToParams("money", in_value.value);
      DropCollectGUI.OnMoneyCollected(in_value.value);
      flow_out.Call(f);
    }));
  }
}
