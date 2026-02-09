// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_DisableUpdateTrails
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Disable Update Trails", 0)]
[Category("Game Actions")]
[Description("Works for main character only")]
public class Flow_DisableUpdateTrails : MyFlowNode
{
  public ValueInput<bool> enable_par;

  public override void RegisterPorts()
  {
    FlowOutput @out = this.AddFlowOutput("Out");
    this.enable_par = this.AddValueInput<bool>("enable?");
    this.AddFlowInput("In", (FlowHandler) (flow =>
    {
      MainGame.me.player_component.Trail.UpdateTrails = this.enable_par.value;
      @out.Call(flow);
    }));
  }

  public override string name => (this.enable_par.value ? "Enable" : "Disable") + " Update Trails";
}
