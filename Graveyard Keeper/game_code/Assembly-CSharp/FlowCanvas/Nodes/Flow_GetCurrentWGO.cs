// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_GetCurrentWGO
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Get Current WGO", 0)]
[ParadoxNotion.Design.Icon("Cube", false, "")]
public class Flow_GetCurrentWGO : MyFlowNode
{
  public WorldGameObject current_wgo;

  public override void RegisterPorts()
  {
    this.AddValueOutput<WorldGameObject>("Current WGO", (ValueHandler<WorldGameObject>) (() => this.current_wgo));
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      this.current_wgo = this.wgo;
      if ((Object) this.current_wgo == (Object) null)
        Debug.LogError((object) "FollowWGO error: WGO is null");
      else
        flow_out.Call(f);
    }));
  }
}
