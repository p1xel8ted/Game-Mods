// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_ParamCompare
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Param compare", 0)]
[Description("If WGO is null, then self")]
public class Flow_ParamCompare : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> par_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<string> par_param = this.AddValueInput<string>("param");
    ValueInput<float> par_value = this.AddValueInput<float>("value");
    WorldGameObject _wgo = (WorldGameObject) null;
    FlowOutput flow_eq = this.AddFlowOutput("==");
    FlowOutput flow_neq = this.AddFlowOutput("!=");
    FlowOutput flow_more = this.AddFlowOutput(">");
    FlowOutput flow_less = this.AddFlowOutput("<");
    this.AddValueOutput<WorldGameObject>("WGO", (ValueHandler<WorldGameObject>) (() => _wgo));
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      _wgo = this.WGOParamOrSelf(par_wgo);
      if ((Object) _wgo == (Object) null)
        return;
      double a = (double) _wgo.GetParam(par_param.value);
      if (((float) a).EqualsTo(par_value.value))
        flow_eq.Call(f);
      else
        flow_neq.Call(f);
      if (a > (double) par_value.value)
        flow_more.Call(f);
      if (a >= (double) par_value.value)
        return;
      flow_less.Call(f);
    }));
  }

  public override string name
  {
    get
    {
      if (!this.GetOutputPort("==").isConnected)
        return base.name + " <color=#FF2020>[!!!]</color>";
      return !this.GetOutputPort("!=").isConnected && (!this.GetOutputPort(">").isConnected || !this.GetOutputPort("<").isConnected) ? base.name + " <color=#FF2020>[!!!]</color>" : base.name;
    }
    set => base.name = value;
  }
}
