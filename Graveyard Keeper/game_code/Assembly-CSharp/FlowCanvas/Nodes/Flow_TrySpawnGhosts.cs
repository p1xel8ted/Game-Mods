// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_TrySpawnGhosts
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[ParadoxNotion.Design.Icon("CubeArrowCube", false, "")]
[Name("Try Spawn Ghosts at Graveyard", 0)]
[Category("Game Actions")]
[Description("Try Spawn Ghosts at Graveyard")]
public class Flow_TrySpawnGhosts : MyFlowNode
{
  public override void RegisterPorts()
  {
    FlowOutput flow_out_correct = this.AddFlowOutput("Correct");
    FlowOutput flow_out_wrong = this.AddFlowOutput("Wrong");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldZone zoneById = WorldZone.GetZoneByID("graveyard");
      if ((Object) zoneById == (Object) null)
      {
        flow_out_wrong.Call(f);
      }
      else
      {
        List<WorldGameObject> darkGraves = zoneById.GetDarkGraves();
        if (darkGraves == null)
        {
          flow_out_wrong.Call(f);
        }
        else
        {
          foreach (WorldGameObject worldGameObject in darkGraves)
          {
            Vector3 position = worldGameObject.transform.position;
            worldGameObject.custom_tag = $"{{{position.x.ToString()};{position.y.ToString()}}} + grave";
            WorldMap.SpawnWGO(worldGameObject.transform.parent, "ghost", new Vector3?(worldGameObject.transform.position)).components.character.SetAnchor(worldGameObject);
            Debug.Log((object) ("Spawned ghost with anchor " + worldGameObject.name), (Object) worldGameObject);
          }
          flow_out_correct.Call(f);
        }
      }
    }));
  }
}
