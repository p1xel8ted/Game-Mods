// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_ProcessMerchantTraiding
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Process Merchant Traiding", 0)]
public class Flow_ProcessMerchantTraiding : MyFlowNode
{
  public override void RegisterPorts()
  {
    float total_money = 0.0f;
    int sold_items_count = 0;
    List<string> sold_items = new List<string>();
    ValueInput<WorldGameObject> in_wgo = this.AddValueInput<WorldGameObject>("Pallet");
    this.AddValueOutput<float>("total money", (ValueHandler<float>) (() => total_money));
    this.AddValueOutput<int>("sold items count", (ValueHandler<int>) (() => sold_items_count));
    this.AddValueOutput<List<string>>("sold items", (ValueHandler<List<string>>) (() => sold_items));
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      total_money = 0.0f;
      sold_items_count = 0;
      sold_items = new List<string>();
      if ((Object) in_wgo.value == (Object) null)
      {
        Debug.LogError((object) "Can not Process Merchant Traiding: WGO is null!");
        flow_out.Call(f);
      }
      else if (in_wgo.value.data == null)
      {
        Debug.LogError((object) "Can not Process Merchant Traiding: WGO.data is null!");
        flow_out.Call(f);
      }
      else
      {
        List<Item> objList = new List<Item>();
        foreach (Item obj in in_wgo.value.data.inventory)
        {
          if (obj != null && obj.definition != null && (!(obj.id != "box_vegetables_silver") || !(obj.id != "box_vegetables_gold") || !(obj.id != "box_goods")))
          {
            total_money += obj.definition.base_price * (float) obj.value;
            sold_items_count += obj.value;
            if (obj.value <= 1)
            {
              sold_items.Add(obj.id);
            }
            else
            {
              for (int index = 0; index < obj.value; ++index)
                sold_items.Add(obj.id);
            }
            objList.Add(obj);
          }
        }
        if (sold_items.Count != sold_items_count)
          Debug.LogError((object) "Something wrong with merchant traiding! Call Bulat!");
        foreach (Item obj in objList)
          in_wgo.value.data.RemoveItem(obj.id, obj.value);
        flow_out.Call(f);
      }
    }));
  }
}
