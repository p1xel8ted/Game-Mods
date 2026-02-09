// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Refugees.Flow_SpawnNextRefugeeFromCraft
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DLCRefugees;
using ParadoxNotion.Design;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes.Refugees;

[Name("Spawn Next Refugee From Craft", 0)]
[Category("Game Actions/Refugees")]
public class Flow_SpawnNextRefugeeFromCraft : MyFlowNode
{
  public FlowInput @in;
  public FlowOutput @out;
  public ValueInput<GameObject> point_in;
  public ValueOutput<WorldGameObject> wgo_out;
  public WorldGameObject wgo_out_value;

  public override void RegisterPorts()
  {
    this.@in = this.AddFlowInput("In", new FlowHandler(this.SpawnRefugee));
    this.@out = this.AddFlowOutput("Out");
    this.point_in = this.AddValueInput<GameObject>("Point");
    this.wgo_out = this.AddValueOutput<WorldGameObject>("WGO", (ValueHandler<WorldGameObject>) (() => this.wgo_out_value));
  }

  public void SpawnRefugee(Flow flow)
  {
    if ((Object) this.point_in.value != (Object) null)
      this.wgo_out_value = RefugeesCampEngine.instance.SpawnNextRefugeeAtTransformFromCraft(this.point_in.value.transform);
    else
      Debug.LogError((object) "Point value is null");
    this.@out.Call(flow);
  }

  [CompilerGenerated]
  public WorldGameObject \u003CRegisterPorts\u003Eb__5_0() => this.wgo_out_value;
}
