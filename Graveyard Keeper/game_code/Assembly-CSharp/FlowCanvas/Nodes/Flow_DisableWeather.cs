// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_DisableWeather
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Disable weather. Use only when time is disabled. Don't forget to enable weather back.")]
[Name("Enable Weather", 0)]
[Color("000000")]
[Category("Game Actions")]
public class Flow_DisableWeather : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<bool> par_enable = this.AddValueInput<bool>("Enable weather");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      EnvironmentEngine.me.weather_is_forced = !par_enable.value;
      flow_out.Call(f);
    }));
  }

  public override string name
  {
    get => !this.GetInputValuePort<bool>("Enable weather").value ? "Disable Weather" : base.name;
    set => base.name = value;
  }
}
