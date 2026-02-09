// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ForLoop
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System;

#nullable disable
namespace FlowCanvas.Nodes;

[FlowNode.ContextDefinedOutputs(new Type[] {typeof (int)})]
[Description("Perform a for loop")]
[Category("Flow Controllers/Iterators")]
[FlowNode.ContextDefinedInputs(new Type[] {typeof (int)})]
public class ForLoop : FlowControlNode
{
  public int current;
  public bool broken;

  public override void RegisterPorts()
  {
    ValueInput<int> n = this.AddValueInput<int>("Loops");
    this.AddValueOutput<int>("Index", (ValueHandler<int>) (() => this.current));
    FlowOutput fCurrent = this.AddFlowOutput("Do");
    FlowOutput fFinish = this.AddFlowOutput("Done");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      this.current = 0;
      this.broken = false;
      f.Break = (FlowBreak) (() => this.broken = true);
      for (int index = 0; index < n.value && !this.broken; ++index)
      {
        this.current = index;
        fCurrent.Call(f);
      }
      f.Break = (FlowBreak) null;
      fFinish.Call(f);
    }));
    this.AddFlowInput("Break", (FlowHandler) (f => this.broken = true));
  }
}
