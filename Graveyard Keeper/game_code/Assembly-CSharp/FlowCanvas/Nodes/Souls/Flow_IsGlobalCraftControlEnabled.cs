// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Souls.Flow_IsGlobalCraftControlEnabled
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes.Souls;

[Category("Game Actions/Souls")]
[Name("Is Global Craft Control Enable", 0)]
public class Flow_IsGlobalCraftControlEnabled : MyFlowNode
{
  public FlowInput @in;
  public FlowOutput @true;
  public FlowOutput @false;

  public override void RegisterPorts()
  {
    this.@in = this.AddFlowInput("In", new FlowHandler(this.CalculatePoints));
    this.@true = this.AddFlowOutput("True");
    this.@false = this.AddFlowOutput("False");
  }

  public void CalculatePoints(Flow flow)
  {
    if (GlobalCraftControlGUI.is_global_control_active)
      this.@true.Call(flow);
    else
      this.@false.Call(flow);
  }
}
