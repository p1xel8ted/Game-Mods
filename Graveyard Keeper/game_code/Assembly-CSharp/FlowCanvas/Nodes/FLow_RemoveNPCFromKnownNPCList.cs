// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.FLow_RemoveNPCFromKnownNPCList
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Remove NPC From Known NPC List", 0)]
public class FLow_RemoveNPCFromKnownNPCList : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> in_wgo = this.AddValueInput<string>("NPC ID");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      MainGame.me.save.known_npcs.RemoveNPC(in_wgo.value);
      flow_out.Call(f);
    }));
  }
}
