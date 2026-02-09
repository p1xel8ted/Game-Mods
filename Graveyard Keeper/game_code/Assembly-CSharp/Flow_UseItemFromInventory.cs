// Decompiled with JetBrains decompiler
// Type: Flow_UseItemFromInventory
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using FlowCanvas;
using FlowCanvas.Nodes;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
[Description("Trying to use item from character's inventory. Happens nothing, if item not found")]
[Name("Use Item From Inventory", 0)]
[Category("Game Actions")]
public class Flow_UseItemFromInventory : MyFlowNode
{
  public FlowInput @in;
  public FlowOutput @out;
  public ValueInput<string> item_id;

  public override void RegisterPorts()
  {
    this.@in = this.AddFlowInput("In", new FlowHandler(this.UseItemFromInventory));
    this.@out = this.AddFlowOutput("Out");
    this.item_id = this.AddValueInput<string>("Item ID");
  }

  public void UseItemFromInventory(Flow flow)
  {
    Item itemWithId = MainGame.me.player.data.GetItemWithID(this.item_id.value);
    if (itemWithId != null)
      MainGame.me.player.UseItemFromInventory(itemWithId);
    else
      Debug.LogError((object) ("Found not found by ID: " + this.item_id.value));
    this.@out.Call(flow);
  }
}
