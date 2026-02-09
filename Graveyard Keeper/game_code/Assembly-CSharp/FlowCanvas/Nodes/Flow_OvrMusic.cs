// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_OvrMusic
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Ovr Music", 0)]
public class Flow_OvrMusic : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> in_music = this.AddValueInput<string>("music id");
    ValueInput<bool> in_play = this.AddValueInput<bool>("play?");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (in_play.value)
        SmartAudioEngine.me.PlayOvrMusic(in_music.value);
      else
        SmartAudioEngine.me.StopOvrMusic(in_music.value);
      flow_out.Call(f);
    }));
  }

  public override string name
  {
    get
    {
      return string.IsNullOrEmpty(this.GetInputValuePort<string>("music id").value) ? base.name + " [stop any]" : $"{base.name} [{(this.GetInputValuePort<bool>("play?").value ? "play" : "stop")}]";
    }
    set => base.name = value;
  }
}
