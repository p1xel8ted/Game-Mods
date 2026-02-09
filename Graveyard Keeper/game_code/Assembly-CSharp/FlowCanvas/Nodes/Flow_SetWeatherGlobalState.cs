// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_SetWeatherGlobalState
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Set Weather Global State", 0)]
[Category("Game Actions")]
public class Flow_SetWeatherGlobalState : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<EnvironmentEngine.State> state = this.AddValueInput<EnvironmentEngine.State>("state");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      EnvironmentEngine.me.SetEngineGlobalState(state.value);
      flow_out.Call(f);
    }));
  }
}
