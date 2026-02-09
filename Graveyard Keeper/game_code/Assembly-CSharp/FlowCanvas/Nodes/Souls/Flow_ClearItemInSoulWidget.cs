// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Souls.Flow_ClearItemInSoulWidget
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes.Souls;

[Name("Clear Item In Soul Widget", 0)]
[Category("Game Actions/Souls")]
public class Flow_ClearItemInSoulWidget : MyFlowNode
{
  public FlowInput @in;
  public FlowOutput @out;
  public float _out_gp_value;

  public override void RegisterPorts()
  {
    this.@in = this.AddFlowInput("In", new FlowHandler(this.ClearItem));
    this.@out = this.AddFlowOutput("Out");
  }

  public void ClearItem(Flow flow)
  {
    GUIElements.me.soul_healer_gui.soul_healing_widget.ClearSoulItemInGUI();
    this.@out.Call(flow);
  }
}
