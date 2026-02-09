// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_RemoveInteractionEvent
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("If WGO is null, then self")]
[Icon("MinusDialogue", false, "")]
[Name("Remove Interaction Event", 0)]
[Category("Game Actions")]
public class Flow_RemoveInteractionEvent : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> par_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<string> par_txt = this.AddValueInput<string>("Event");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(par_wgo);
      string str = par_txt.value;
      if (worldGameObject.custom_interaction_events.Contains(str))
        worldGameObject.custom_interaction_events.Remove(str);
      worldGameObject.RedrawBubble();
      flow_out.Call(f);
    }));
  }
}
