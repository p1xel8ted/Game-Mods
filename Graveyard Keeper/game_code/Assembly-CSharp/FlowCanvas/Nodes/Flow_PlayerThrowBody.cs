// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_PlayerThrowBody
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Player throw body", 0)]
public class Flow_PlayerThrowBody : MyFlowNode
{
  public bool thrown_is_not_worker = true;

  public override void RegisterPorts()
  {
    this.AddValueOutput<bool>("thrown NOT worker", (ValueHandler<bool>) (() => this.thrown_is_not_worker));
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      bool thrown_worker;
      MainGame.me.player_component.ThrowBodyInRiver(out thrown_worker);
      this.thrown_is_not_worker = !thrown_worker;
      flow_out.Call(f);
    }));
  }
}
