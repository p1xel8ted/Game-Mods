// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_IsKnownWorldZone
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Functions")]
[Name("Is Known World Zone", 0)]
public class Flow_IsKnownWorldZone : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> zone_id = this.AddValueInput<string>("zone_id");
    FlowOutput flow_yes = this.AddFlowOutput("Is known");
    FlowOutput flow_no = this.AddFlowOutput("Is NOT known");
    bool is_known = false;
    this.AddValueOutput<bool>("is_known (bool)", (ValueHandler<bool>) (() => is_known));
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (string.IsNullOrEmpty(zone_id.value))
      {
        flow_no.Call(f);
      }
      else
      {
        if (MainGame.me?.save?.known_world_zones == null)
          return;
        is_known = MainGame.me.save.known_world_zones.Contains(zone_id.value);
        if (is_known)
          flow_yes.Call(f);
        else
          flow_no.Call(f);
      }
    }));
  }
}
