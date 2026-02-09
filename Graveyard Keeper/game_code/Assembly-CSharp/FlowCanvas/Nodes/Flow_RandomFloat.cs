// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_RandomFloat
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Random Float")]
[Category("Game Actions")]
[Name("Random Float", 0)]
public class Flow_RandomFloat : MyFlowNode
{
  public override void RegisterPorts()
  {
    float random_number = 0.0f;
    ValueInput<float> in_min = this.AddValueInput<float>("min");
    ValueInput<float> in_max = this.AddValueInput<float>("max");
    this.AddValueOutput<float>("random", (ValueHandler<float>) (() => random_number));
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if ((double) in_min.value > (double) in_max.value)
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
