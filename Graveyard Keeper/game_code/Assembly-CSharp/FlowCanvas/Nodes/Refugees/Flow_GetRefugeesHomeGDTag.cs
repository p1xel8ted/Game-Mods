// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Refugees.Flow_GetRefugeesHomeGDTag
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DLCRefugees;
using ParadoxNotion.Design;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes.Refugees;

[Category("Game Actions/Refugees")]
[Name("Get Refugee's Home GD Tag", 0)]
public class Flow_GetRefugeesHomeGDTag : MyFlowNode
{
  public FlowInput @in;
  public FlowOutput @out;
  public ValueInput<WorldGameObject> refuge_wgo_in;
  public ValueOutput<string> home_gd_tag_out;
  public string home_gd_tag_out_value;

  public override void RegisterPorts()
  {
    this.@in = this.AddFlowInput("In", (FlowHandler) (flow =>
    {
      if ((Object) this.refuge_wgo_in.value != (Object) null)
        this.home_gd_tag_out_value = RefugeesCampEngine.instance.GetRefugeesHomeGDTag(this.refuge_wgo_in.value);
      else
        Debug.LogError((object) "Refugee WGO is null");
      this.@out.Call(flow);
    }));
    this.@out = this.AddFlowOutput("Out");
    this.refuge_wgo_in = this.AddValueInput<WorldGameObject>("Refugee WGO");
    this.home_gd_tag_out = this.AddValueOutput<string>("Home GD Tag", (ValueHandler<string>) (() => this.home_gd_tag_out_value));
  }

  [CompilerGenerated]
  public void \u003CRegisterPorts\u003Eb__5_0(Flow flow)
  {
    if ((Object) this.refuge_wgo_in.value != (Object) null)
      this.home_gd_tag_out_value = RefugeesCampEngine.instance.GetRefugeesHomeGDTag(this.refuge_wgo_in.value);
    else
      Debug.LogError((object) "Refugee WGO is null");
    this.@out.Call(flow);
  }

  [CompilerGenerated]
  public string \u003CRegisterPorts\u003Eb__5_1() => this.home_gd_tag_out_value;
}
