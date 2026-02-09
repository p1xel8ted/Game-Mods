// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_RevealCrafth
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Reveal Craft", 0)]
[Category("Game Actions")]
public class Flow_RevealCrafth : MyFlowNode
{
  public override void RegisterPorts()
  {
    FlowOutput flow_out = this.AddFlowOutput("Out");
    ValueInput<string> craft_id = this.AddValueInput<string>("craft_id");
    ValueInput<bool> silent = this.AddValueInput<bool>("silent?");
    ValueInput<bool> not_add_to_one_time = this.AddValueInput<bool>("not_add_to_one_time?");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (string.IsNullOrEmpty(craft_id.value))
      {
        Debug.LogError((object) "Can not reveal craft: craft_id is null!");
        flow_out.Call(f);
      }
      else
      {
        CraftDefinition craftDefinition = GameBalance.me.GetData<CraftDefinition>(craft_id.value);
        ObjectCraftDefinition data = GameBalance.me.GetData<ObjectCraftDefinition>(craft_id.value);
        if (craftDefinition == null)
          craftDefinition = (CraftDefinition) data;
        if (craftDefinition == null)
        {
          Debug.LogError((object) $"Can not reveal craft: craft with id \"{craft_id.value}\" not found!");
          flow_out.Call(f);
        }
        else if (silent.value)
        {
          if (!not_add_to_one_time.value)
            MainGame.me.save.completed_one_time_crafts.Add(craft_id.value);
          flow_out.Call(f);
        }
        else
        {
          if (!not_add_to_one_time.value)
            MainGame.me.save.completed_one_time_crafts.Add(craft_id.value);
          string str = string.Empty;
          if (craftDefinition.output == null || craftDefinition.output.Count == 0)
          {
            str = craftDefinition.id;
          }
          else
          {
            foreach (Item obj in craftDefinition.output)
            {
              if (!(obj.id == "r") && !(obj.id == "g") && !(obj.id == "b"))
                str = obj.id;
            }
            if (string.IsNullOrEmpty(str))
              str = craftDefinition.id;
          }
          GUIElements.me.tech_dialog.Open(new TechDefinition()
          {
            id = str,
            crafts = {
              craft_id.value
            },
            price = new GameRes()
          }, (GJCommons.VoidDelegate) (() => flow_out.Call(f)), true, pseudotech: true);
        }
      }
    }));
  }
}
