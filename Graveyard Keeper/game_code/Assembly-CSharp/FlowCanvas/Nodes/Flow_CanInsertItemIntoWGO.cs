// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_CanInsertItemIntoWGO
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Description("Open craft GUI")]
[Name("Can Insert Item Into WGO", 0)]
public class Flow_CanInsertItemIntoWGO : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<Item> in_item = this.AddValueInput<Item>("Item");
    FlowOutput flow_yes = this.AddFlowOutput("Yes");
    FlowOutput flow_no = this.AddFlowOutput("No");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (this.wgo.CanInsertItem(in_item.value))
        flow_yes.Call(f);
      else
        flow_no.Call(f);
    }));
  }
}
