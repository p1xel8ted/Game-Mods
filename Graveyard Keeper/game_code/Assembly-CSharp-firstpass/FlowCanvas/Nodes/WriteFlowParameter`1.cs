// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.WriteFlowParameter`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System;

#nullable disable
namespace FlowCanvas.Nodes;

[FlowNode.ContextDefinedInputs(new Type[] {typeof (Wild)})]
[Description("Writes (or creates) a named parameter to the incomming Flow, which you can later read with a ReadFlowParameter node.\nFlow parameters are temporary variables that exist only in the context of the same Flow.")]
[Category("Flow Controllers/Flow Convert")]
public class WriteFlowParameter<T> : FlowControlNode
{
  public override void RegisterPorts()
  {
    FlowOutput o = this.AddFlowOutput("Out");
    ValueInput<string> pName = this.AddValueInput<string>("Name");
    ValueInput<T> pValue = this.AddValueInput<T>("Value");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      f.WriteParameter<T>(pName.value, pValue.value);
      o.Call(f);
    }));
  }
}
