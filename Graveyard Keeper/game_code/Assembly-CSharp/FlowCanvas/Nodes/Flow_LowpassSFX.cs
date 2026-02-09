// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_LowpassSFX
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Lowpass SFX to low", 0)]
public class Flow_LowpassSFX : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<bool> normal_frequency = this.AddValueInput<bool>("To Normal Frequency?");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      SmartAudioEngine.me.mixer.SetFloat("sfx_cutoff", normal_frequency.value ? 22000f : 5000f);
      flow_out.Call(f);
    }));
  }

  public override string name
  {
    get
    {
      return this.GetInputValuePort<bool>("To Normal Frequency?").value ? "Lowpass SFX to normal" : base.name;
    }
    set => base.name = value;
  }
}
