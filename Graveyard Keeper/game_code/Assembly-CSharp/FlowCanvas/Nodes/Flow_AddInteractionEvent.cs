// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_AddInteractionEvent
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[ParadoxNotion.Design.Icon("PlusDialogue", false, "")]
[Name("Add Interaction Event", 0)]
[Description("If WGO is null, then self")]
[Category("Game Actions")]
public class Flow_AddInteractionEvent : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> par_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<string> par_txt = this.AddValueInput<string>("Event");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(par_wgo);
      if ((Object) worldGameObject == (Object) null)
      {
        Debug.LogError((object) "Flow_AddInteractionEvent: WGO is null");
      }
      else
      {
        worldGameObject.AddInteractionEvent(par_txt.value);
        flow_out.Call(f);
      }
    }));
  }
}
