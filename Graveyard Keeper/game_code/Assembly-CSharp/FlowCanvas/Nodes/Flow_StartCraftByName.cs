// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_StartCraftByName
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Start Craft WGO")]
[Name("Start Craft By Name", 0)]
[Category("Game Actions")]
public class Flow_StartCraftByName : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<string> in_craft_name = this.AddValueInput<string>("craft name");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(in_wgo);
      if ((Object) worldGameObject == (Object) null)
        Debug.LogError((object) "Can not ctart craft: WGO is null!");
      else if (string.IsNullOrEmpty(in_craft_name.value))
      {
        Debug.LogError((object) "Can not ctart craft: craft name is empty");
      }
      else
      {
        worldGameObject.TryStartCraft(in_craft_name.value);
        flow_out.Call(f);
      }
    }));
  }
}
