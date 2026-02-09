// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_ShowWGOQuality
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Enable Show WGO Quality", 0)]
[Category("Game Actions")]
[Description("Enable Show WGO Quality")]
public class Flow_ShowWGOQuality : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<bool> par_do_enable = this.AddValueInput<bool>("Enable");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      MainGame.me.player.data.SetParam("do_not_show_wgo_qualities", par_do_enable.value ? 0.0f : 1f);
      MainGame.me.player_component.CheckShowWGOQuality();
      flow_out.Call(f);
    }));
  }

  public override string name
  {
    get
    {
      return this.GetInputValuePort<bool>("Enable").value ? "<color=#FFFF50>Enable Show WGO Quality</color>" : "<color=#30FF30>Disable Show WGO Quality</color>";
    }
    set => base.name = value;
  }
}
