// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_TrySetAnchorToChar
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Try to set anchor to character")]
[Name("Try to set anchor to character", 0)]
[Category("Game Actions")]
[ParadoxNotion.Design.Icon("CubePlus", false, "")]
public class Flow_TrySetAnchorToChar : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<GameObject> in_anchor = this.AddValueInput<GameObject>("Anchor");
    FlowOutput flow_correct = this.AddFlowOutput("Correctly Added");
    FlowOutput flow_wrong = this.AddFlowOutput("Mistake Happen");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if ((Object) in_anchor.value == (Object) null)
      {
        Debug.Log((object) "Anchor is null");
        flow_wrong.Call(f);
      }
      else if ((Object) in_wgo.value != (Object) null)
      {
        WorldGameObject worldGameObject = in_wgo.value;
        if ((Object) worldGameObject != (Object) null && worldGameObject.components != null && worldGameObject.components.character.enabled)
        {
          worldGameObject.components.character.SetAnchor(in_anchor.value);
          flow_correct.Call(f);
        }
        else
        {
          Debug.Log((object) "WGO Character is null");
          flow_wrong.Call(f);
        }
      }
      else
      {
        Debug.Log((object) "WGO is null");
        flow_wrong.Call(f);
      }
    }));
  }
}
