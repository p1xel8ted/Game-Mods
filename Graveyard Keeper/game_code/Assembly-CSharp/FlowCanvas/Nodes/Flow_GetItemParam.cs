// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_GetItemParam
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Get Item's Param", 0)]
[Category("Game Actions")]
public class Flow_GetItemParam : MyFlowNode
{
  public float out_param_value;

  public override void RegisterPorts()
  {
    ValueInput<Item> in_item = this.AddValueInput<Item>("Item");
    ValueInput<string> in_param = this.AddValueInput<string>("Param name");
    this.AddValueOutput<float>("Param Value", (ValueHandler<float>) (() => this.out_param_value));
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (in_item.value == null || in_item.value.IsEmpty())
      {
        Debug.LogError((object) "Item null or empty");
        flow_out.Call(f);
      }
      else
      {
        this.out_param_value = in_item.value.GetParam(in_param.value);
        flow_out.Call(f);
      }
    }));
  }
}
