// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_PostRefreshZone
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Post-refresh zone", 0)]
[Category("Game Actions")]
public class Flow_PostRefreshZone : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> par_zone = this.AddValueInput<string>("zone id");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldZone zoneById = WorldZone.GetZoneByID(par_zone.value);
      if ((Object) zoneById != (Object) null)
      {
        zoneById.PostRefreshZone();
        GUIElements.ChangeBubblesVisibility(MainGame.me.player_char.control_enabled);
      }
      flow_out.Call(f);
    }));
  }
}
