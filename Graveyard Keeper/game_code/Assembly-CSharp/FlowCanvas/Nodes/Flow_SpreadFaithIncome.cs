// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_SpreadFaithIncome
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Pray: Spread faith income", 0)]
public class Flow_SpreadFaithIncome : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<List<WorldGameObject>> par_prayers = this.AddValueInput<List<WorldGameObject>>("prayers");
    ValueInput<int> par_faith = this.AddValueInput<int>("faith");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      PrayLogics.SpreadFaithIncome(par_prayers.value, par_faith.value);
      flow_out.Call(f);
    }));
  }
}
