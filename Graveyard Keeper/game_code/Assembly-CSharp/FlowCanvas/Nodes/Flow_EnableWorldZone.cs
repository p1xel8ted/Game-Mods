// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_EnableWorldZone
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Enable World Zone", 0)]
public class Flow_EnableWorldZone : MyFlowNode
{
  public ValueInput<bool> enable;

  public override void RegisterPorts()
  {
    ValueInput<string> zone_id = this.AddValueInput<string>("World Zone id");
    this.enable = this.AddValueInput<bool>("Enable World Zone");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldZone zoneById = WorldZone.GetZoneByID(zone_id.value);
      if ((Object) zoneById != (Object) null)
      {
        if (this.enable.value)
          zoneById.EnableWorldZone();
        else
          zoneById.DisableWorldZone();
      }
      flow_out.Call(f);
    }));
  }

  public override string name
  {
    get => !this.enable.value ? "Disable World Zone" : base.name;
    set => base.name = value;
  }
}
