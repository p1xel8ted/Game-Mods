// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_RemoveGhosts
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Remove Ghosts from Graveyard and decay graves")]
[ParadoxNotion.Design.Icon("CubeArrowCube", false, "")]
[Name("Remove Ghosts from Graveyard", 0)]
[Category("Game Actions")]
public class Flow_RemoveGhosts : MyFlowNode
{
  public override void RegisterPorts()
  {
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      List<WorldGameObject> gameObjectsByObjId = WorldMap.GetWorldGameObjectsByObjId("ghost");
      if (gameObjectsByObjId == null)
      {
        Debug.LogError((object) "Something wrong!");
      }
      else
      {
        int count = gameObjectsByObjId.Count;
        if (count > 0)
        {
          WorldZone zoneById = WorldZone.GetZoneByID("graveyard");
          if ((Object) zoneById == (Object) null)
            Debug.LogError((object) "Zone graveyard not found");
          else if (zoneById.GetZoneWGOs() == null)
          {
            Debug.LogError((object) "WGOs list of zone graveyard is null!", (Object) zoneById);
          }
          else
          {
            foreach (WorldGameObject zoneWgO in zoneById.GetZoneWGOs())
            {
              if (!((Object) zoneWgO == (Object) null) && !(zoneWgO.obj_id != "grave_ground"))
                zoneWgO.data.AddToParams("decay", (float) count);
            }
            while (gameObjectsByObjId.Count > 0)
            {
              gameObjectsByObjId[0].DestroyMe();
              gameObjectsByObjId.RemoveAt(0);
            }
          }
        }
      }
      flow_out.Call(f);
    }));
  }
}
