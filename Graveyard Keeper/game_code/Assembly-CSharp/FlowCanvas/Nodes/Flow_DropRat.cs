// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_DropRat
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Drop Rat", 0)]
[Category("Game Actions")]
[Description("If WGO is null, then self")]
[Color("857fff")]
[ParadoxNotion.Design.Icon("ArrowDown", false, "")]
public class Flow_DropRat : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> par_wgo = this.AddValueInput<WorldGameObject>("WGO to drop");
    ValueInput<Direction> par_direction = this.AddValueInput<Direction>("Direction");
    ValueInput<string> par_rat_name = this.AddValueInput<string>("rat name");
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
        string id = par_rat_name.value;
        if (string.IsNullOrEmpty(id))
        {
          List<ItemDefinition> itemDefinitionList = new List<ItemDefinition>();
          foreach (ItemDefinition itemDefinition in GameBalance.me.items_data)
          {
            if (itemDefinition.type == ItemDefinition.ItemType.Rat)
              itemDefinitionList.Add(itemDefinition);
          }
          id = itemDefinitionList[UnityEngine.Random.Range(0, itemDefinitionList.Count)].id;
        }
        Item obj = new Item(id) { inventory_size = 999 };
        obj.AddItem(new Item("rat_status:normal"));
        worldGameObject.DropItem(obj, par_direction.value, force: par_direction.value == Direction.None ? 1f : 3f, check_walls: par_direction.value == Direction.None);
        flow_out.Call(f);
      }
    }));
  }
}
