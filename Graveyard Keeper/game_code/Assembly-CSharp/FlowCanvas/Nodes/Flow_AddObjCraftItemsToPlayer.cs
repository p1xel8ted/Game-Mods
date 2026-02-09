// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_AddObjCraftItemsToPlayer
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Add Obj Craft Items To Player", 0)]
[Category("Game Actions")]
public class Flow_AddObjCraftItemsToPlayer : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> in_craft_id = this.AddValueInput<string>("Obj Craft");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      ObjectCraftDefinition data = GameBalance.me.GetData<ObjectCraftDefinition>(in_craft_id.value);
      if (data == null)
      {
        Debug.Log((object) $"Trying add items to player for obj craft:[{in_craft_id.value}] but it is null");
        flow_out.Call(f);
      }
      else
      {
        MainGame.me.player.AddToInventory(data.needs);
        flow_out.Call(f);
      }
    }));
  }
}
