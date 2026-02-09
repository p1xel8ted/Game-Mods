// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_PlayAmbient
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Play Ambient by name")]
[Name("Play Ambient", 0)]
[Category("Game Actions")]
public class Flow_PlayAmbient : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> in_sound = this.AddValueInput<string>("sound");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (string.IsNullOrEmpty(in_sound.value))
        DarkTonic.MasterAudio.MasterAudio.StopPlaylist("ambient");
      else
        DarkTonic.MasterAudio.MasterAudio.TriggerPlaylistClip("ambient", in_sound.value);
      flow_out.Call(f);
    }));
  }

  public override string name
  {
    get
    {
      return !this.GetInputValuePort("sound").isConnected && this.GetInputValuePort("sound").isDefaultValue ? "Stop Ambient" : base.name;
    }
    set => base.name = value;
  }
}
