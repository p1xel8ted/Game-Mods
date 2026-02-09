// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_StartCraftWGO
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Description("Start Craft WGO")]
[Name("Start Craft WGO", 0)]
public class Flow_StartCraftWGO : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<Item> in_item = this.AddValueInput<Item>("Item");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if ((Object) in_wgo.value == (Object) null)
      {
        Debug.LogError((object) "WGO is null!");
      }
      else
      {
        in_wgo.value.components.craft.Craft(GameBalance.me.GetData<CraftDefinition>("set_" + in_item.value.id));
        flow_out.Call(f);
      }
    }));
  }
}
