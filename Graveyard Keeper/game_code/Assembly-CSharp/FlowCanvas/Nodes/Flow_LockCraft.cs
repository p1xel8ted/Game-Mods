// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_LockCraft
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Lock Craft", 0)]
public class Flow_LockCraft : MyFlowNode
{
  public override void RegisterPorts()
  {
    FlowOutput flow_out = this.AddFlowOutput("Out");
    ValueInput<string> craft = this.AddValueInput<string>("Craft id");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      MainGame.me.save.LockCraft(craft.value);
      flow_out.Call(f);
    }));
  }
}
