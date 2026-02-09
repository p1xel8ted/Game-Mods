// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_GetStockWGO
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Get Idle Points Stock WGO")]
[Category("Game Actions")]
[Name("Get Idle Points Stock WGO", 0)]
public class Flow_GetStockWGO : MyFlowNode
{
  public WorldGameObject stock_wgo;
  public string prefix_string = string.Empty;

  public override void RegisterPorts()
  {
    ValueInput<GDPoint.IdlePointPrefix> in_idle_point_prefix = this.AddValueInput<GDPoint.IdlePointPrefix>("Prefix");
    this.AddValueOutput<WorldGameObject>("Stock WGO", (ValueHandler<WorldGameObject>) (() => this.stock_wgo));
    this.AddValueOutput<string>("Prefix String", (ValueHandler<string>) (() => this.prefix_string));
    FlowOutput flow_yes = this.AddFlowOutput("Found");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (in_idle_point_prefix.value == GDPoint.IdlePointPrefix.None)
      {
        Debug.LogError((object) "Prefix is NULL!");
      }
      else
      {
        this.prefix_string = GDPoint.GetIdlePrefix(in_idle_point_prefix.value);
        string custom_tag = this.prefix_string + "stock";
        this.stock_wgo = WorldMap.GetWorldGameObjectByCustomTag(custom_tag);
        if ((Object) this.stock_wgo == (Object) null)
          Debug.LogError((object) $"Stock WGO with custom_tag=\"{custom_tag}\" not found!");
        else
          flow_yes.Call(f);
      }
    }));
  }
}
