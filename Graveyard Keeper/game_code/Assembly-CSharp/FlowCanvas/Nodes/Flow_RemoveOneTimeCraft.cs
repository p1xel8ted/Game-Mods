// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_RemoveOneTimeCraft
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Removes one time craft if it exists")]
[Name("Remove One Time Craft", 0)]
[Category("Game Actions")]
public class Flow_RemoveOneTimeCraft : MyFlowNode
{
  public override void RegisterPorts()
  {
    FlowOutput flow_out = this.AddFlowOutput("Out");
    ValueInput<string> craft = this.AddValueInput<string>("Craft id");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      MainGame.me.save.completed_one_time_crafts.Remove(craft.value);
      flow_out.Call(f);
    }));
  }
}
