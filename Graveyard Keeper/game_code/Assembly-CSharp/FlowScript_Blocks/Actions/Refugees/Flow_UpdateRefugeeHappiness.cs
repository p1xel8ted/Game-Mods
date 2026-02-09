// Decompiled with JetBrains decompiler
// Type: FlowScript_Blocks.Actions.Refugees.Flow_UpdateRefugeeHappiness
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DLCRefugees;
using FlowCanvas;
using FlowCanvas.Nodes;
using ParadoxNotion.Design;

#nullable disable
namespace FlowScript_Blocks.Actions.Refugees;

[Category("Game Actions/Refugees")]
[Name("Update Refugee Happiness", 0)]
public class Flow_UpdateRefugeeHappiness : MyFlowNode
{
  public FlowInput @in;
  public FlowOutput @out;
  public ValueInput<RefugeesCampEngine.UpdateHappinessItemsMode> mode;
  public ValueInput<float> happiness_delta;

  public override void RegisterPorts()
  {
    this.@in = this.AddFlowInput("In", new FlowHandler(this.UpdateRefugeeHappiness));
    this.@out = this.AddFlowOutput("Out");
    this.mode = this.AddValueInput<RefugeesCampEngine.UpdateHappinessItemsMode>("update mode");
    this.happiness_delta = this.AddValueInput<float>("happiness delta");
  }

  public void UpdateRefugeeHappiness(Flow flow)
  {
    RefugeesCampEngine.instance.UpdateRefugeeCampValues(this.happiness_delta.value, this.mode.value);
    this.@out.Call(flow);
  }
}
