// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Souls.Flow_CalculateGratitudePoints
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes.Souls;

[Name("Calculate Points For Released Soul", 0)]
[Category("Game Actions/Souls")]
public class Flow_CalculateGratitudePoints : MyFlowNode
{
  public FlowInput @in;
  public FlowOutput @out;
  public ValueInput<Item> _in_item;
  public ValueOutput<float> _out_gp;
  public float _out_gp_value;

  public override void RegisterPorts()
  {
    this.@in = this.AddFlowInput("In", new FlowHandler(this.CalculatePoints));
    this.@out = this.AddFlowOutput("Out");
    this._in_item = this.AddValueInput<Item>("Healed Soul");
    this._out_gp = this.AddValueOutput<float>("GP", (ValueHandler<float>) (() => this._out_gp_value));
  }

  public void CalculatePoints(Flow flow)
  {
    if (this._in_item.value == null || this._in_item.value.IsEmpty())
    {
      Debug.LogError((object) "Item is null or empty");
      this.@out.Call(flow);
    }
    else
    {
      this._out_gp_value = SoulsHelper.CalculatePointsAfterSoulRelease(this._in_item.value);
      this.@out.Call(flow);
    }
  }

  [CompilerGenerated]
  public float \u003CRegisterPorts\u003Eb__5_0() => this._out_gp_value;
}
