// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_PausePlaylist
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Enable(Disable) playlist by its name. If clip name set then it will be triggered in specified playlist")]
[Category("Game Actions")]
[Name("Pause Playlist", 0)]
public class Flow_PausePlaylist : MyFlowNode
{
  public override void RegisterPorts()
  {
    FlowOutput flow_out = this.AddFlowOutput("Out");
    ValueInput<bool> in_pause = this.AddValueInput<bool>("Pause?");
    ValueInput<string> in_playlist_name = this.AddValueInput<string>("Playlist name");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      string playlistControllerName = in_playlist_name.value;
      bool flag = in_pause.value;
      if (!string.IsNullOrEmpty(playlistControllerName))
      {
        if (flag)
          DarkTonic.MasterAudio.MasterAudio.PausePlaylist(playlistControllerName);
        else
          DarkTonic.MasterAudio.MasterAudio.UnpausePlaylist(playlistControllerName);
      }
      flow_out.Call(f);
    }));
  }

  public override string name
  {
    get => !this.GetInputValuePort<bool>("Pause?").value ? "Unpause Playlist" : base.name;
    set => base.name = value;
  }
}
