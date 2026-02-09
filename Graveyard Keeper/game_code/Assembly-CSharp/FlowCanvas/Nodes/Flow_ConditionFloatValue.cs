// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_ConditionFloatValue
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Condition: Float value", 0)]
public class Flow_ConditionFloatValue : MyFlowNode
{
  public override void RegisterPorts()
  {
    float _v = 0.0f;
    ValueInput<bool> par_condition = this.AddValueInput<bool>("condition");
    ValueInput<float> par_value_true = this.AddValueInput<float>("if true");
    ValueInput<float> par_value_false = this.AddValueInput<float>("if false");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddValueOutput<float>("result", (ValueHandler<float>) (() => _v));
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      _v = par_condition.value ? par_value_true.value : par_value_false.value;
      flow_out.Call(f);
    }));
  }
}
