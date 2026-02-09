// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_FadeSoundToVolume
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Fade Sound To Volume", 0)]
[Category("Game Actions")]
[Description("Fades sound to the volume(0..1)")]
public class Flow_FadeSoundToVolume : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> in_sound = this.AddValueInput<string>("sound");
    ValueInput<float> in_sound_volume = this.AddValueInput<float>("volume to fade");
    ValueInput<float> in_fade_value = this.AddValueInput<float>("fade time");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      DarkTonic.MasterAudio.MasterAudio.FadeSoundGroupToVolume(in_sound.value, in_sound_volume.value, in_fade_value.value);
      flow_out.Call(f);
    }));
  }
}
