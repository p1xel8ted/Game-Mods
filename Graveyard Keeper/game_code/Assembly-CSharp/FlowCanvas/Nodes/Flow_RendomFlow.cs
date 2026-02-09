// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_RendomFlow
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Description("Random Flow")]
[Name("Random Flow", 0)]
public class Flow_RendomFlow : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<float> in_chance = this.AddValueInput<float>("chance");
    FlowOutput flow_rolled = this.AddFlowOutput("true");
    FlowOutput flow_not_rolled = this.AddFlowOutput("false");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if ((double) UnityEngine.Random.Range(0.0f, 1f) < (double) in_chance.value)
        flow_rolled.Call(f);
      else
        flow_not_rolled.Call(f);
    }));
  }
}
