// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_Vendor
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Vendor", 0)]
public class Flow_Vendor : MyFlowNode
{
  public override void RegisterPorts()
  {
    FlowOutput flow_out = this.AddFlowOutput("Out");
    FlowOutput flow_closed = this.AddFlowOutput("On Hide");
    ValueInput<WorldGameObject> par_wgo = this.AddValueInput<WorldGameObject>("WGO");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject wgo = par_wgo.value;
      if ((Object) wgo == (Object) null)
        wgo = this.wgo;
      if (!MainGame.me.player_char.control_enabled)
        GS.SetPlayerEnable(true, false);
      GUIElements.me.vendor.Open(wgo, (GJCommons.VoidDelegate) (() => flow_closed.Call(f)));
      flow_out.Call(f);
    }));
  }

  public override string name
  {
    get => "Open vendor gui";
    set => base.name = value;
  }
}
