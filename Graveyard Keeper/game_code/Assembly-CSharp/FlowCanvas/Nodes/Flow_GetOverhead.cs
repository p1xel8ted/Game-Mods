// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_GetOverhead
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Get Overhead Item", 0)]
[Description("If Character is null, then Player")]
[Icon("Cube", false, "")]
public class Flow_GetOverhead : MyFlowNode
{
  public Item item;
  public string item_id = string.Empty;

  public override void RegisterPorts()
  {
    this.AddValueOutput<Item>("Item", (ValueHandler<Item>) (() => this.item));
    this.AddValueOutput<string>("Item ID", (ValueHandler<string>) (() => this.item_id));
    FlowOutput flow_no_overhead = this.AddFlowOutput("No overhead");
    FlowOutput flow_has_overhead = this.AddFlowOutput("Has overhead");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      this.item = MainGame.me.player.components.character.GetOverheadItem();
      if (this.item != null)
        this.item_id = this.item.id;
      if (this.item == null)
        flow_no_overhead.Call(f);
      else
        flow_has_overhead.Call(f);
      flow_out.Call(f);
    }));
  }
}
