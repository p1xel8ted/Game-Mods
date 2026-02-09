// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_IsGDPointEnable
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Is GDPoint Enable", 0)]
[Category("Game Actions")]
[ParadoxNotion.Design.Icon("Cube", false, "")]
public class Flow_IsGDPointEnable : MyFlowNode
{
  public bool _enable_flag;

  public override void RegisterPorts()
  {
    ValueInput<GDPoint> in_gd_point = this.AddValueInput<GDPoint>("GDPoint");
    this.AddValueOutput<bool>("is_enable", (ValueHandler<bool>) (() => this._enable_flag));
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if ((Object) in_gd_point.value != (Object) null)
        this._enable_flag = in_gd_point.value.gameObject.activeSelf;
      else
        Debug.LogError((object) "GDPoint is null!");
      flow_out.Call(f);
    }));
  }
}
