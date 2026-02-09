// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_CampMusic
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DLCRefugees;
using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Set Camp Music", 0)]
[Category("Game Actions")]
public class Flow_CampMusic : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<RefugeeCampMusic> music_type = this.AddValueInput<RefugeeCampMusic>("Music Type");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      RefugeesCampEngine.instance.SetCampMusic(music_type.value);
      flow_out.Call(f);
    }));
  }
}
