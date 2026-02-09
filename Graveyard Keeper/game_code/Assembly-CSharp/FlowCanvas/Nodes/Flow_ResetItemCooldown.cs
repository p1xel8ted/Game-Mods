// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_ResetItemCooldown
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Reset item cooldown", 0)]
public class Flow_ResetItemCooldown : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> in_item_id = this.AddValueInput<string>("item id");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      MainGame.me.player.SetParam("_cooldown_" + in_item_id.value, 0.0f);
      flow_out.Call(f);
    }));
  }
}
