// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_StopSound
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Stop Sound", 0)]
[Description("Stop sound by name")]
[Category("Game Actions")]
public class Flow_StopSound : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> in_sound = this.AddValueInput<string>("sound");
    ValueInput<bool> in_stop_with_fade = this.AddValueInput<bool>("stop with fade?");
    ValueInput<float> in_fade_value = this.AddValueInput<float>("fade out time");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      int num = in_stop_with_fade.value ? 1 : 0;
      float fadeTime = in_fade_value.value;
      if (num == 0)
        DarkTonic.MasterAudio.MasterAudio.StopAllOfSound(in_sound.value);
      else
        DarkTonic.MasterAudio.MasterAudio.FadeOutAllOfSound(in_sound.value, fadeTime);
      flow_out.Call(f);
    }));
  }
}
