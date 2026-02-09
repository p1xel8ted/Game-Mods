// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_SetEnvironmentPreset
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Set Environment Preset By Its Name")]
[Category("Game Actions")]
[Name("Set Environment Preset", 0)]
public class Flow_SetEnvironmentPreset : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> in_preset = this.AddValueInput<string>("Preset name");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      string preset_name = in_preset.value;
      MainGame.me.save.SetEnvironmentPreset(preset_name);
      flow_out.Call(f);
    }));
  }
}
