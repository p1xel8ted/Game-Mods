// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_OpenDungeonWindow
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Open Dungeon Window", 0)]
[Category("Game Actions")]
public class Flow_OpenDungeonWindow : MyFlowNode
{
  public override void RegisterPorts()
  {
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      GUIElements.me.dungeon_window.Open();
      flow_out.Call(f);
    }));
  }
}
