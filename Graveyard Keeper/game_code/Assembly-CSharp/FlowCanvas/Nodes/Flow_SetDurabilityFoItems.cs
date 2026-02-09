// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_SetDurabilityFoItems
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Description("Set durability for items in grave WGO inventory (only GraveStone and GraveFence)")]
[Name("Set durability for items in Grave", 0)]
public class Flow_SetDurabilityFoItems : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_wgo = this.AddValueInput<WorldGameObject>("Grave WGO");
    ValueInput<float> in_dur = this.AddValueInput<float>("durability");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(in_wgo);
      if ((Object) worldGameObject == (Object) null)
      {
        Debug.LogError((object) "Can not set items durability: WGO is null!");
        flow_out.Call(f);
      }
      else if (worldGameObject.obj_def == null)
      {
        Debug.LogError((object) "Can not set items durability: WGO definition is null!");
        flow_out.Call(f);
      }
      else
      {
        foreach (Item obj in worldGameObject.data.inventory)
        {
          if (obj != null && (obj.definition.type == ItemDefinition.ItemType.GraveStone || obj.definition.type == ItemDefinition.ItemType.GraveFence))
            obj.durability = in_dur.value;
        }
        worldGameObject.Redraw();
        flow_out.Call(f);
      }
    }));
  }
}
