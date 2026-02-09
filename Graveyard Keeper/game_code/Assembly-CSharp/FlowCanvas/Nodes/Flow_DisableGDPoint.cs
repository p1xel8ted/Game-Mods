// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_DisableGDPoint
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Disable GDPoint", 0)]
[Category("Game Actions")]
[ParadoxNotion.Design.Icon("Cube", false, "")]
[Description("If Character is null, then Player")]
public class Flow_DisableGDPoint : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<GDPoint> in_gd_point = this.AddValueInput<GDPoint>("GDPoint");
    ValueInput<bool> in_enable = this.AddValueInput<bool>("enable");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if ((Object) in_gd_point.value != (Object) null)
      {
        in_gd_point.value.gameObject.SetActive(in_enable.value);
        if (in_enable.value)
        {
          RoundAndSortComponent[] componentsInChildren = in_gd_point.value.GetComponentsInChildren<RoundAndSortComponent>(true);
          if (componentsInChildren != null && componentsInChildren.Length != 0)
          {
            foreach (RoundAndSortComponent andSortComponent in componentsInChildren)
              andSortComponent.DoUpdateStuff(true);
          }
        }
      }
      else
        Debug.LogError((object) "GDPoint is null!");
      flow_out.Call(f);
    }));
  }

  public override string name
  {
    get
    {
      return this.GetInputValuePort<bool>("enable").value ? "<color=#FFFF50>Enable GDPoint</color>" : "<color=#30FF30>Disable GDPoint</color>";
    }
    set => base.name = value;
  }
}
