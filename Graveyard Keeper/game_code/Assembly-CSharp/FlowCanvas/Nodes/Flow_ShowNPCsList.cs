// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_ShowNPCsList
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Show NPCs List Window", 0)]
[Category("Game Actions")]
public class Flow_ShowNPCsList : MyFlowNode
{
  public override void RegisterPorts()
  {
    FlowOutput flow_out = this.AddFlowOutput("out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      GUIElements.me.game_gui.OpenAtTab(GameGUI.TabType.NPCs);
      flow_out.Call(f);
    }));
  }
}
