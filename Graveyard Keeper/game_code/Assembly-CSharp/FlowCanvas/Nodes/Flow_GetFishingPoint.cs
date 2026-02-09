// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_GetFishingPoint
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Get Fishing Point", 0)]
[Category("Game Actions")]
[Description("Get fishing point of fishing spot")]
public class Flow_GetFishingPoint : MyFlowNode
{
  public override void RegisterPorts()
  {
    GDPoint fishing_point = (GDPoint) null;
    string fishing_point_tag = string.Empty;
    this.AddValueOutput<GDPoint>("GDPoint", (ValueHandler<GDPoint>) (() => fishing_point));
    this.AddValueOutput<string>("GDPoint tag", (ValueHandler<string>) (() => fishing_point_tag));
    ValueInput<WorldGameObject> in_wgo = this.AddValueInput<WorldGameObject>("WGO");
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
        fishing_point_tag = worldGameObject.obj_id + "_point";
        fishing_point = WorldMap.GetGDPointByGDTag(fishing_point_tag);
        flow_out.Call(f);
      }
    }));
  }
}
