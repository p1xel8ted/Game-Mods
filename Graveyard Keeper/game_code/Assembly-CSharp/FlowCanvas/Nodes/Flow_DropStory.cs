// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_DropStory
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[ParadoxNotion.Design.Icon("ArrowDown", false, "")]
[Category("Game Actions")]
[Name("Drop Story", 0)]
[Color("857fff")]
[Description("If WGO is null, then self")]
public class Flow_DropStory : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> par_wgo = this.AddValueInput<WorldGameObject>("WGO to drop");
    ValueInput<float> par_story_br = this.AddValueInput<float>("Story brnz");
    ValueInput<float> par_story_silv = this.AddValueInput<float>("Story silv");
    ValueInput<float> par_story_gold = this.AddValueInput<float>("Story gold");
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
        worldGameObject.DropStory(par_story_br.value, par_story_silv.value, par_story_gold.value);
        flow_out.Call(f);
      }
    }));
  }
}
