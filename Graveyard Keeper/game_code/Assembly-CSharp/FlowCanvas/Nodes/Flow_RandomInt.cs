// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_RandomInt
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Random Integer", 0)]
[Category("Game Actions")]
[Description("Random Integer")]
public class Flow_RandomInt : MyFlowNode
{
  public override void RegisterPorts()
  {
    int random_number = 0;
    ValueInput<int> in_min = this.AddValueInput<int>("min (inclusive)");
    ValueInput<int> in_max = this.AddValueInput<int>("max (exclusive)");
    this.AddValueOutput<int>("random", (ValueHandler<int>) (() => random_number));
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (in_min.value >= in_max.value)
      {
        Debug.LogError((object) $"Wrong random bounds: min = {in_min.value.ToString()}, max = {in_max.value.ToString()}");
      }
      else
      {
        random_number = UnityEngine.Random.Range(in_min.value, in_max.value);
        flow_out.Call(f);
      }
    }));
  }
}
