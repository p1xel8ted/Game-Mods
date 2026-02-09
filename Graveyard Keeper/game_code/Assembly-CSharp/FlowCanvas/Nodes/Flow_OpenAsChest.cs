// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_OpenAsChest
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Open As Chest", 0)]
[Category("Game Actions")]
public class Flow_OpenAsChest : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_wgo = this.AddValueInput<WorldGameObject>("WGO");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if ((Object) in_wgo.value != (Object) null)
        GUIElements.me.chest.Open(in_wgo.value);
      else
        Debug.LogError((object) "Can not open WGO as chest: WGO is null!");
      flow_out.Call(f);
    }));
  }
}
