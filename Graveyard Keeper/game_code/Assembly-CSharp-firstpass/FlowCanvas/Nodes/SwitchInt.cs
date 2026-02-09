// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.SwitchInt
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Branch the Flow based on an integer value. The Default output is called when the Index value is out of range.")]
[FlowNode.ContextDefinedInputs(new System.Type[] {typeof (int)})]
[Category("Flow Controllers/Switchers")]
[Name("Switch Integer", 0)]
public class SwitchInt : FlowControlNode, IMultiPortNode
{
  [SerializeField]
  public int _portCount = 4;

  public int portCount
  {
    get => this._portCount;
    set => this._portCount = value;
  }

  public override void RegisterPorts()
  {
    ValueInput<int> index = this.AddValueInput<int>("Index");
    List<FlowOutput> outs = new List<FlowOutput>();
    for (int index1 = 0; index1 < this.portCount; ++index1)
      outs.Add(this.AddFlowOutput(index1.ToString()));
    FlowOutput def = this.AddFlowOutput("Default");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      int index2 = index.value;
      if (index2 >= 0 && index2 < outs.Count)
        outs[index2].Call(f);
      else
        def.Call(f);
    }));
  }
}
