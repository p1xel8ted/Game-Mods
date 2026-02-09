// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_SetCarryingItem
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Set Carrying Item", 0)]
[ParadoxNotion.Design.Icon("Cube", false, "")]
[Category("Game Actions")]
[Description("If Character is null, then Player")]
public class Flow_SetCarryingItem : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<string> in_item_id = this.AddValueInput<string>("Item ID");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      BaseCharacterComponent character = in_wgo.value?.components?.character;
      if (character == null)
      {
        Debug.LogError((object) "Flow_SetCarryingItem error: character is null!");
        flow_out.Call(f);
      }
      else
      {
        character.SetCarryingItem(new Item(in_item_id.value));
        flow_out.Call(f);
      }
    }));
  }
}
