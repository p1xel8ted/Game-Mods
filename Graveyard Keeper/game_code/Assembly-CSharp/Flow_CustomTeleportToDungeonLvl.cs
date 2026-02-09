// Decompiled with JetBrains decompiler
// Type: Flow_CustomTeleportToDungeonLvl
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using FlowCanvas;
using FlowCanvas.Nodes;
using ParadoxNotion.Design;

#nullable disable
[Category("Game Actions")]
[Name("Custom Teleport To Dungeon LVL", 0)]
public class Flow_CustomTeleportToDungeonLvl : MyFlowNode
{
  public FlowInput @in;
  public FlowOutput @out;
  public ValueInput<int> level_in;

  public override void RegisterPorts()
  {
    this.@in = this.AddFlowInput("In", new FlowHandler(this.TeleportToLvl));
    this.@out = this.AddFlowOutput("Out");
    this.level_in = this.AddValueInput<int>("lvl_number");
  }

  public void TeleportToLvl(Flow flow)
  {
    GJTimer.AddTimer(0.01f, (GJTimer.VoidDelegate) (() =>
    {
      MainGame.me.TeleportToDungeonLevelCustom(this.level_in.value);
      this.@out.Call(flow);
    }));
  }
}
