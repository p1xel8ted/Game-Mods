// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Refugees.Flow_AddPlaceItemsToTent
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
[Name("Add Place Items To Tents", 0)]
public class Flow_AddPlaceItemsToTent : MyFlowNode
{
  public FlowInput @in;
  public FlowOutput @out;
  public ValueInput<WorldGameObject> tent_object_in;

  public override void RegisterPorts()
  {
    this.@in = this.AddFlowInput("In", (FlowHandler) (flow =>
    {
      if ((Object) this.tent_object_in.value != (Object) null)
        RefugeesCampEngine.instance.AddItemsOnTentAppear(this.tent_object_in.value);
      else
        Debug.LogError((object) "Tent WGO is null");
      this.@out.Call(flow);
    }));
    this.@out = this.AddFlowOutput("Out");
    this.tent_object_in = this.AddValueInput<WorldGameObject>("Tent WGO");
  }

  [CompilerGenerated]
  public void \u003CRegisterPorts\u003Eb__3_0(Flow flow)
  {
    if ((Object) this.tent_object_in.value != (Object) null)
      RefugeesCampEngine.instance.AddItemsOnTentAppear(this.tent_object_in.value);
    else
      Debug.LogError((object) "Tent WGO is null");
    this.@out.Call(flow);
  }
}
