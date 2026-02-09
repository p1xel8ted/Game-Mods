// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_SpawnNPCFromStock
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Spawn NPC From Stock", 0)]
public class Flow_SpawnNPCFromStock : MyFlowNode
{
  public WorldGameObject out_o;

  public override void RegisterPorts()
  {
    ValueInput<string> in_wgo_name = this.AddValueInput<string>("WGO ID", "obj_id");
    ValueInput<string> in_wgo_tag = this.AddValueInput<string>("WGO tag", "custom_tag");
    ValueInput<string> in_gd_point_tag = this.AddValueInput<string>("GD Point Tag", "gd_point");
    this.AddValueOutput<WorldGameObject>("WGO", (ValueHandler<WorldGameObject>) (() => this.out_o));
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = (WorldGameObject) null;
      if (!string.IsNullOrEmpty(in_wgo_tag.value))
        worldGameObject = WorldMap.GetWorldGameObjectByCustomTag(in_wgo_tag.value);
      else if (!string.IsNullOrEmpty(in_wgo_name.value))
      {
        List<WorldGameObject> gameObjectsByObjId = WorldMap.GetWorldGameObjectsByObjId(in_wgo_name.value);
        if (gameObjectsByObjId != null && gameObjectsByObjId.Count > 0)
          worldGameObject = gameObjectsByObjId[0];
      }
      else
      {
        Debug.LogError((object) "Tag and name are null!");
        return;
      }
      if ((Object) worldGameObject == (Object) null)
      {
        Debug.LogError((object) "WGO is null!");
      }
      else
      {
        GDPoint gdPointByGdTag = WorldMap.GetGDPointByGDTag(in_gd_point_tag.value);
        if ((Object) gdPointByGdTag == (Object) null)
        {
          Debug.LogError((object) ("Can't find GD point: " + in_gd_point_tag.value));
        }
        else
        {
          worldGameObject.transform.position = gdPointByGdTag.transform.position;
          worldGameObject.RefreshPositionCache();
          worldGameObject.gameObject.SetActive(true);
          worldGameObject.OnCameToGDPoint(gdPointByGdTag);
          worldGameObject.DrawPuffFX();
          this.out_o = worldGameObject;
          Debug.Log((object) $"Teleporting, output name = {worldGameObject.name}, obj_id = {worldGameObject.obj_id}, instance_id = {worldGameObject.gameObject.GetInstanceID().ToString()}");
          flow_out.Call(f);
        }
      }
    }));
  }

  public override void OnNodeInspectorGUI()
  {
    base.OnNodeInspectorGUI();
    this.MakeStringNullIfEmpty("obj_id");
    this.MakeStringNullIfEmpty("custom_tag");
    this.MakeStringNullIfEmpty("gd_point");
  }
}
