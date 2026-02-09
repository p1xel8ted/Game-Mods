// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_PerspectiveTextGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Perspective Text", 0)]
public class Flow_PerspectiveTextGUI : MyFlowNode
{
  public override void RegisterPorts()
  {
    FlowOutput flow_out = this.AddFlowOutput("Out");
    FlowOutput flow_on_finished = this.AddFlowOutput("On finished");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      PerspectiveTextGUI componentInChildren = MainGame.me.ui_root.GetComponentInChildren<PerspectiveTextGUI>(true);
      if ((Object) componentInChildren != (Object) null)
      {
        componentInChildren.Init();
        componentInChildren.OpenSlidingText((GJCommons.VoidDelegate) (() => flow_on_finished.Call(f)));
      }
      else
        flow_on_finished.Call(f);
      flow_out.Call(f);
    }));
  }
}
