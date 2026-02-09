// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_PrayForBuff
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Pray for buff", 0)]
public class Flow_PrayForBuff : MyFlowNode
{
  public override void RegisterPorts()
  {
    FlowOutput flow_finished = this.AddFlowOutput("Finished");
    FlowOutput flow_middle = this.AddFlowOutput("Middle");
    this.AddFlowInput("In", (FlowHandler) (f => GUIElements.me.pray_craft.DoPrayForBuff(PrayLogics.last_pray_result.success, (System.Action) (() => flow_finished.Call(f)), (System.Action) (() => flow_middle.Call(f)))));
  }
}
