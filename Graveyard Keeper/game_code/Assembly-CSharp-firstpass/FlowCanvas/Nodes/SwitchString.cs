// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.SwitchString
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System;
using System.Collections.Generic;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Flow Controllers/Switchers")]
[Description("Branch the Flow based on a string value. The Default output is called if there is no other matching output same as the input value")]
[FlowNode.ContextDefinedInputs(new Type[] {typeof (string)})]
public class SwitchString : FlowControlNode
{
  public List<string> comparisonOutputs = new List<string>();

  public override void RegisterPorts()
  {
    ValueInput<string> name = this.AddValueInput<string>("Value");
    List<FlowOutput> outs = new List<FlowOutput>();
    for (int index = 0; index < this.comparisonOutputs.Count; ++index)
      outs.Add(this.AddFlowOutput($"\"{this.comparisonOutputs[index]}\"", index.ToString()));
    FlowOutput def = this.AddFlowOutput("Default");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      string str = name.value;
      if (str == null)
      {
        def.Call(f);
      }
      else
      {
        bool flag = false;
        for (int index = 0; index < this.comparisonOutputs.Count; ++index)
        {
          if (string.IsNullOrEmpty(str) && string.IsNullOrEmpty(this.comparisonOutputs[index]))
          {
            outs[index].Call(f);
            flag = true;
          }
          else if (this.comparisonOutputs[index].Trim().ToLower() == str.Trim().ToLower())
          {
            outs[index].Call(f);
            flag = true;
          }
        }
        if (flag)
          return;
        def.Call(f);
      }
    }));
  }
}
