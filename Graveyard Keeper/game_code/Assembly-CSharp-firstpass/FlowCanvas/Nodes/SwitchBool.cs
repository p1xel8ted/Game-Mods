// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.SwitchBool
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Flow Controllers/Switchers")]
[Name("Switch Condition", 0)]
[FlowNode.ContextDefinedInputs(new Type[] {typeof (bool)})]
[Description("Branch the Flow based on a conditional boolean value")]
public class SwitchBool : FlowControlNode
{
  public override void RegisterPorts()
  {
    ValueInput<bool> c = this.AddValueInput<bool>("Condition");
    FlowOutput tOut = this.AddFlowOutput("True");
    FlowOutput fOut = this.AddFlowOutput("False");
    this.AddFlowInput("In", (FlowHandler) (f => this.Call(c.value ? tOut : fOut, f)));
  }
}
