// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_PlaySound
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Play sound by name")]
[Category("Game Actions")]
[Name("Play Sound", 0)]
public class Flow_PlaySound : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> in_sound = this.AddValueInput<string>("sound");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      DarkTonic.MasterAudio.MasterAudio.PlaySound(in_sound.value);
      flow_out.Call(f);
    }));
  }
}
