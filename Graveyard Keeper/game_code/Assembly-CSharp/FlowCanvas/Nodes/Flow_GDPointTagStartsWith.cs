// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_GDPointTagStartsWith
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("GDPoint tag starts with", 0)]
[ParadoxNotion.Design.Icon("Cube", false, "")]
[Description("If WGO is null, then self")]
public class Flow_GDPointTagStartsWith : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<GDPoint> in_gd_point = this.AddValueInput<GDPoint>("GDPoint");
    ValueInput<string> in_custom_tag = this.AddValueInput<string>("custom_tag");
    FlowOutput flow_no = this.AddFlowOutput("No");
    FlowOutput flow_yes = this.AddFlowOutput("Yes");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if ((Object) in_gd_point.value == (Object) null)
      {
        Debug.LogError((object) "Flow_GDPointTagStartsWith error: WGO is null");
        flow_no.Call(f);
      }
      else if (in_gd_point.value.gd_tag.StartsWith(in_custom_tag.value))
        flow_yes.Call(f);
      else
        flow_no.Call(f);
    }));
  }
}
