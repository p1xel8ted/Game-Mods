// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_OpenTavernCashbox
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Open Tavern Cashbox", 0)]
[Category("Game Actions")]
public class Flow_OpenTavernCashbox : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_cashbox_wgo = this.AddValueInput<WorldGameObject>("cashbox WGO");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      GUIElements.me.pray_report.OpenTavernCashbox(this.WGOParamOrSelf(in_cashbox_wgo));
      flow_out.Call(f);
    }));
  }
}
