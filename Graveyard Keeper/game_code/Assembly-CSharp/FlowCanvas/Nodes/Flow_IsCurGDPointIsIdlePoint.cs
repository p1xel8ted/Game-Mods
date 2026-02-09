// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_IsCurGDPointIsIdlePoint
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Does CurGDPoint Is Idle Point")]
[Category("Game Actions")]
[Name("Does CurGDPoint Is Idle Point", 0)]
public class Flow_IsCurGDPointIsIdlePoint : MyFlowNode
{
  public int idle_point_num;
  public GDPoint.IdlePointPrefix idle_point_prefix_enum = GDPoint.IdlePointPrefix.None;
  public string idle_point_prefix_string = string.Empty;

  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_npc_wgo = this.AddValueInput<WorldGameObject>("NPC");
    this.AddValueOutput<int>("Point Num", (ValueHandler<int>) (() => this.idle_point_num));
    this.AddValueOutput<GDPoint.IdlePointPrefix>("Prefix Enum", (ValueHandler<GDPoint.IdlePointPrefix>) (() => this.idle_point_prefix_enum));
    this.AddValueOutput<string>("Prefix String", (ValueHandler<string>) (() => this.idle_point_prefix_string));
    FlowOutput flow_yes = this.AddFlowOutput("Is Idle Point");
    FlowOutput flow_no = this.AddFlowOutput("Not Idle Point");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(in_npc_wgo);
      if ((Object) worldGameObject == (Object) null)
      {
        Debug.LogError((object) "NPC is null!");
      }
      else
      {
        string curGdPoint = worldGameObject.cur_gd_point;
        if (string.IsNullOrEmpty(curGdPoint))
        {
          Debug.Log((object) "not Is Idle point");
          flow_no.Call(f);
        }
        else if (GDPoint.TryParseIdlePointTag(curGdPoint, out this.idle_point_prefix_enum, out this.idle_point_num) || curGdPoint.StartsWith("personal_"))
        {
          if (curGdPoint.StartsWith("personal_"))
            this.idle_point_prefix_enum = GDPoint.IdlePointPrefix.Camp;
          this.idle_point_prefix_string = GDPoint.GetIdlePrefix(this.idle_point_prefix_enum);
          flow_yes.Call(f);
          Debug.Log((object) "Is Idle point");
        }
        else
        {
          Debug.Log((object) "not Is Idle point");
          flow_no.Call(f);
        }
      }
    }));
  }
}
