// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.FlipFlop
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System;

#nullable disable
namespace FlowCanvas.Nodes;

[FlowNode.ContextDefinedOutputs(new Type[] {typeof (bool)})]
[Description("Flip Flops between the 2 outputs each time In is called")]
[Category("Flow Controllers/Togglers")]
public class FlipFlop : FlowControlNode
{
  public bool isFlip = true;
  public bool original;

  public override string name => $"{base.name} {(this.isFlip ? "[FLIP]" : "[FLOP]")}";

  public override void OnGraphStarted() => this.original = this.isFlip;

  public override void OnGraphStoped() => this.isFlip = this.original;

  public override void RegisterPorts()
  {
    FlowOutput flipF = this.AddFlowOutput("Flip");
    FlowOutput flopF = this.AddFlowOutput("Flop");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      this.Call(this.isFlip ? flipF : flopF, f);
      this.isFlip = !this.isFlip;
    }));
    this.AddFlowInput("Reset", (FlowHandler) (f => this.isFlip = false));
    this.AddValueOutput<bool>("Is Flip", (ValueHandler<bool>) (() => this.isFlip));
  }
}
