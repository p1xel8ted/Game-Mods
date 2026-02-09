// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ReadFlowParameter`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Reads a named parameter from the incomming Flow and returns it's value.\nFlow parameters can be set with a WriteFlowParameter node.\nFlow parameters are temporary variables that exist only in the context of the same Flow.")]
[Category("Flow Controllers/Flow Convert")]
[FlowNode.ContextDefinedOutputs(new Type[] {typeof (Wild)})]
public class ReadFlowParameter<T> : FlowControlNode
{
  public T flowValue;

  public override void RegisterPorts()
  {
    FlowOutput o = this.AddFlowOutput("Out");
    ValueInput<string> pName = this.AddValueInput<string>("Name");
    this.AddValueOutput<T>("Value", (ValueHandler<T>) (() => this.flowValue));
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      this.flowValue = f.ReadParameter<T>(pName.value);
      o.Call(f);
    }));
  }
}
