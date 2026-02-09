// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_ActivateSpawners
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Activate Spawners", 0)]
[Description("Activate Spawners by custom tag")]
[Icon("CubePlus", false, "")]
public class Flow_ActivateSpawners : MyFlowNode
{
  public const float _ACTIVATION_PERIOD = 0.1f;

  public override void RegisterPorts()
  {
    ValueInput<string> par_custom_tag = this.AddValueInput<string>("Custom tag");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      List<MobSpawner> spawnersByCustomTag = WorldMap.GetMobSpawnersByCustomTag(par_custom_tag.value);
      float seconds = 0.0f;
      foreach (MobSpawner mobSpawner in spawnersByCustomTag)
      {
        MobSpawner spawner = mobSpawner;
        GJTimer.AddTimer(seconds, (GJTimer.VoidDelegate) (() =>
        {
          if (spawner.spawned_mobs != null && spawner.spawned_mobs.Count > 0)
          {
            foreach (WorldGameObject worldGameObject in new List<WorldGameObject>((IEnumerable<WorldGameObject>) spawner.spawned_mobs))
              worldGameObject.DestroyMe();
          }
          spawner.spawned_mobs = new List<WorldGameObject>();
          spawner.ActivateSpawner();
        }));
        seconds += 0.1f;
      }
      flow_out.Call(f);
    }));
  }
}
