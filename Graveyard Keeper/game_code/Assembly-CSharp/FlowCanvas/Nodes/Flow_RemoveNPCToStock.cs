// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_RemoveNPCToStock
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Remove NPC To Stock", 0)]
public class Flow_RemoveNPCToStock : MyFlowNode
{
  public bool is_with_puff_fx = true;

  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<string> in_gd_point_tag = this.AddValueInput<string>("GD Point Tag");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(in_wgo);
      if ((Object) worldGameObject == (Object) null)
      {
        Debug.LogError((object) "WGO is null!");
      }
      else
      {
        if (this.is_with_puff_fx)
          worldGameObject.DrawPuffFX();
        GDPoint gdPointByGdTag = WorldMap.GetGDPointByGDTag(!string.IsNullOrEmpty(in_gd_point_tag.value) ? in_gd_point_tag.value : "default_destroy_point");
        if ((Object) gdPointByGdTag == (Object) null)
        {
          Debug.LogError((object) ("Can't find GD point: " + in_gd_point_tag.value));
        }
        else
        {
          Debug.Log((object) $"Teleporting {worldGameObject.obj_id} to GD point: {gdPointByGdTag.name}", (Object) gdPointByGdTag.gameObject);
          worldGameObject.transform.position = gdPointByGdTag.transform.position;
          worldGameObject.RefreshPositionCache();
          flow_out.Call(f);
          worldGameObject.OnCameToGDPoint(gdPointByGdTag);
        }
      }
    }));
  }
}
