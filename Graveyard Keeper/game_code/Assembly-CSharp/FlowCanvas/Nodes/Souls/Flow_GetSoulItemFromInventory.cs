// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Souls.Flow_GetSoulItemFromInventory
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes.Souls;

[Category("Game Actions/Souls")]
[Name("Get Soul Item From Inventory", 0)]
public class Flow_GetSoulItemFromInventory : MyFlowNode
{
  public FlowInput @in;
  public FlowOutput @out;
  public ValueInput<WorldGameObject> wgo;
  public ValueOutput<Item> soul_item;
  public WorldGameObject _wgo;
  public Item _out_soul_item;

  public override void RegisterPorts()
  {
    this.@in = this.AddFlowInput("In", new FlowHandler(this.GetSoulItem));
    this.@out = this.AddFlowOutput("Out");
    this.wgo = this.AddValueInput<WorldGameObject>("WGO");
    this.soul_item = this.AddValueOutput<Item>("soul_item", (ValueHandler<Item>) (() => this._out_soul_item));
  }

  public void GetSoulItem(Flow flow)
  {
    this._wgo = this.WGOParamOrSelf(this.wgo);
    if ((Object) this._wgo != (Object) null)
      this._out_soul_item = this._wgo.GetItemOfType(ItemDefinition.ItemType.Soul);
    this.@out.Call(flow);
  }

  [CompilerGenerated]
  public Item \u003CRegisterPorts\u003Eb__6_0() => this._out_soul_item;
}
