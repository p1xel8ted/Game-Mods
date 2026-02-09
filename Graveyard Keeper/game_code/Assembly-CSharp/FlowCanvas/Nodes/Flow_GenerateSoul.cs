// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_GenerateSoul
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Color("857fff")]
[Name("Extract Soul", 0)]
[Description("If WGO is null, then self")]
[Category("Game Actions")]
[ParadoxNotion.Design.Icon("ArrowDown", false, "")]
public class Flow_GenerateSoul : MyFlowNode
{
  public Item out_soul_item_value;

  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> par_wgo = this.AddValueInput<WorldGameObject>("WGO to drop");
    this.AddValueInput<Direction>("Direction");
    ValueInput<float> par_dec_durability = this.AddValueInput<float>("dec durability [0..100]");
    ValueInput<float> min_damage_value = this.AddValueInput<float>("Min damage value [0..1]");
    ValueInput<float> max_damage_value = this.AddValueInput<float>("Max damage value [0..1]");
    this.AddValueOutput<Item>("Soul Item", (ValueHandler<Item>) (() => this.out_soul_item_value));
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(par_wgo);
      if ((Object) worldGameObject == (Object) null)
      {
        Debug.LogError((object) "WGO is null!");
      }
      else
      {
        Item bodyFromInventory = worldGameObject.GetBodyFromInventory();
        if (bodyFromInventory == null)
          return;
        Item itemOfType = bodyFromInventory.GetItemOfType(ItemDefinition.ItemType.SoulBodyPart);
        if (itemOfType != null)
        {
          Flow_GenerateSoul.RemoveBodyPartFromBody(bodyFromInventory, itemOfType);
          if ((double) par_dec_durability.value > 0.1)
            itemOfType.durability = (float) (1.0 - (double) par_dec_durability.value / 100.0);
          float num = UnityEngine.Random.Range(min_damage_value.value, max_damage_value.value);
          itemOfType.durability -= num;
          CraftDefinition dataOrNull = GameBalance.me.GetDataOrNull<CraftDefinition>($"{worldGameObject.obj_id}:{itemOfType.id}");
          itemOfType.SetItemID(dataOrNull.output[0].id);
          this.out_soul_item_value = itemOfType;
        }
        else
        {
          Item obj1 = new Item("sin_shard", 1);
          Item obj2 = new Item("sin_shard_body_part", 1);
          Flow_GenerateSoul.RemoveBodyPartFromBody(bodyFromInventory, obj2);
          this.out_soul_item_value = obj1;
        }
        flow_out.Call(f);
      }
    }));
  }

  public static void RemoveBodyPartFromBody(Item body, Item item)
  {
    foreach (Item obj1 in body.inventory)
    {
      if (obj1.id == item.id)
      {
        body.RemoveItem(item, 1);
        break;
      }
      foreach (Item obj2 in obj1.inventory)
      {
        if (obj2.id == item.id)
        {
          obj1.RemoveItem(item, 1);
          return;
        }
      }
    }
  }
}
