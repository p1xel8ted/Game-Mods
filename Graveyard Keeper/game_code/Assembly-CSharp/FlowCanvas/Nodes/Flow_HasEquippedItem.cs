// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_HasEquippedItem
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("True if wgo has equipped item of type")]
[Name("Has equiped item", 0)]
[Category("Game Actions")]
public class Flow_HasEquippedItem : MyFlowNode
{
  public Item _item;

  public override void RegisterPorts()
  {
    this.AddValueInput<ItemDefinition.EquipmentType>("Equip type");
    this.AddValueOutput<Item>("item", (ValueHandler<Item>) (() => this._item));
    FlowOutput flow_yes = this.AddFlowOutput("Yes");
    FlowOutput flow_no = this.AddFlowOutput("No");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      this._item = MainGame.me.player.GetEquippedItem(ItemDefinition.EquipmentType.FishingRod);
      if (this._item == null || this._item.IsEmpty())
        flow_no.Call(f);
      else
        flow_yes.Call(f);
    }));
  }
}
