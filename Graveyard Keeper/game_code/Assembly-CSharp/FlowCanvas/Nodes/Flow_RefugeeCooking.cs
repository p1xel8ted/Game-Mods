// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_RefugeeCooking
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DLCRefugees;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Refugee Cooking", 0)]
[Category("Game Actions")]
public class Flow_RefugeeCooking : MyFlowNode
{
  public override void RegisterPorts()
  {
    FlowOutput flow_out = this.AddFlowOutput("Out");
    bool has_craft = false;
    float craft_time = 0.0f;
    ValueInput<WorldGameObject> wgo = this.AddValueInput<WorldGameObject>("WGO");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject wgo1 = this.WGOParamOrSelf(wgo);
      if ((Object) wgo1 != (Object) null)
      {
        has_craft = wgo1.components.craft.is_crafting;
        if (!has_craft)
          craft_time = RefugeesCampEngine.instance.StartCooking(wgo1);
      }
      else
        Debug.LogError((object) "Null WGO for Flow_RefugeeCooking");
      flow_out.Call(f);
    }));
    this.AddValueOutput<bool>("Has craft", (ValueHandler<bool>) (() => has_craft));
    this.AddValueOutput<float>("Cooking Time", (ValueHandler<float>) (() => craft_time));
  }
}
