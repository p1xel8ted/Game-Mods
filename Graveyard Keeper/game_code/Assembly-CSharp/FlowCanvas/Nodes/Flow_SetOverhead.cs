// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_SetOverhead
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("If Character is null, then Player")]
[Icon("Cube", false, "")]
[Category("Game Actions")]
[Name("Set Overhead Item", 0)]
public class Flow_SetOverhead : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<Item> in_item = this.AddValueInput<Item>("Item");
    ValueInput<string> in_item_id = this.AddValueInput<string>("Item ID");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      BaseCharacterComponent playerChar = MainGame.me.player_char;
      if ((in_item.value == null || in_item.value.IsEmpty()) && string.IsNullOrEmpty(in_item_id.value))
      {
        playerChar.SetOverheadItem((Item) null);
      }
      else
      {
        Item obj = in_item.value == null || in_item.value.IsEmpty() ? new Item(in_item_id.value, 1) : in_item.value;
        playerChar.SetOverheadItem(obj);
      }
      flow_out.Call(f);
    }));
  }
}
