// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Refugees.Flow_SpawnNextRefugee
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DLCRefugees;
using ParadoxNotion.Design;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes.Refugees;

[Name("Spawn Next Refugee", 0)]
[Category("Game Actions/Refugees")]
public class Flow_SpawnNextRefugee : MyFlowNode
{
  public FlowInput @in;
  public FlowOutput @out;
  public ValueInput<GameObject> point_in;
  public ValueInput<string> home_gd_point;
  public ValueOutput<WorldGameObject> wgo_out;
  public WorldGameObject wgo_out_value;

  public override void RegisterPorts()
  {
    this.@in = this.AddFlowInput("In", new FlowHandler(this.SpawnRefugee));
    this.@out = this.AddFlowOutput("Out");
    this.point_in = this.AddValueInput<GameObject>("Spawn Point");
    this.home_gd_point = this.AddValueInput<string>("Home GD Point Tag");
    this.wgo_out = this.AddValueOutput<WorldGameObject>("WGO", (ValueHandler<WorldGameObject>) (() => this.wgo_out_value));
  }

  public void SpawnRefugee(Flow flow)
  {
    if ((Object) this.point_in.value != (Object) null)
      this.wgo_out_value = RefugeesCampEngine.instance.SpawnNextRefugeeAtTransform(this.point_in.value.transform, this.home_gd_point.value);
    else
      Debug.LogError((object) "Point value is null");
    this.@out.Call(flow);
  }

  public override string name
  {
    get
    {
      return string.IsNullOrEmpty(this.home_gd_point.value) ? base.name + "\n<color=#FF2020>NULL OR EMPTY HOME GD TAG</color>" : base.name;
    }
  }

  [CompilerGenerated]
  public WorldGameObject \u003CRegisterPorts\u003Eb__6_0() => this.wgo_out_value;
}
