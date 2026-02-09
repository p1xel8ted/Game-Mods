// Decompiled with JetBrains decompiler
// Type: Flow_SetInventorySize
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using FlowCanvas;
using FlowCanvas.Nodes;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
[Category("Game Actions")]
[Name("Set Inventory Size", 0)]
public class Flow_SetInventorySize : MyFlowNode
{
  public FlowInput @in;
  public FlowOutput @out;
  public ValueInput<WorldGameObject> wgo_in;
  public ValueInput<int> inventory_size_in;

  public override void RegisterPorts()
  {
    this.@in = this.AddFlowInput("In", new FlowHandler(this.SetInventorySize));
    this.@out = this.AddFlowOutput("Out");
    this.wgo_in = this.AddValueInput<WorldGameObject>("WGO");
    this.inventory_size_in = this.AddValueInput<int>("Size");
  }

  public void SetInventorySize(Flow flow)
  {
    WorldGameObject worldGameObject = this.WGOParamOrSelf(this.wgo_in);
    if ((Object) worldGameObject != (Object) null)
    {
      worldGameObject.data.SetInventorySize(this.inventory_size_in.value);
      Debug.Log((object) $"{worldGameObject?.ToString()} changed inventory size to {this.inventory_size_in.value.ToString()}");
    }
    else
      Debug.LogError((object) "Flow_SetInventorySize error: WGO is null");
    this.@out.Call(flow);
  }
}
