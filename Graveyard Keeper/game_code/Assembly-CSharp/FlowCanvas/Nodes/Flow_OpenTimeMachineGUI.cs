// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_OpenTimeMachineGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Open Time Machine GUI", 0)]
public class Flow_OpenTimeMachineGUI : MyFlowNode
{
  public override void RegisterPorts()
  {
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (!MainGame.me.player_char.control_enabled)
        GS.SetPlayerEnable(true, false);
      GUIElements.me.time_machine_gui.Open();
      flow_out.Call(f);
    }));
  }
}
