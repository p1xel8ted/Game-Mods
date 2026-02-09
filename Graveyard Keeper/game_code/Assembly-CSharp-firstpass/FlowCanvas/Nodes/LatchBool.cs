// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.LatchBool
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using System;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Flow Controllers/Flow Convert")]
[Description("Convert a Flow signal to boolean value")]
[FlowNode.ContextDefinedOutputs(new Type[] {typeof (bool)})]
[DeserializeFrom(new string[] {"FlowCanvas.Nodes.Latch"})]
[Name("Latch Condition", 0)]
public class LatchBool : FlowControlNode
{
  public bool latched;

  public override void RegisterPorts()
  {
    FlowOutput o = this.AddFlowOutput("Out");
    this.AddFlowInput("True", (FlowHandler) (f =>
    {
      this.latched = true;
      o.Call(f);
    }));
    this.AddFlowInput("False", (FlowHandler) (f =>
    {
      this.latched = false;
      o.Call(f);
    }));
    this.AddValueOutput<bool>("Value", (ValueHandler<bool>) (() => this.latched));
  }
}
