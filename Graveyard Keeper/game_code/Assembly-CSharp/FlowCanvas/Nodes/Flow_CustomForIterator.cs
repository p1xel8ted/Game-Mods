// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_CustomForIterator
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Custom For Loop")]
[Category("Flow Controllers/Iterators")]
[FlowNode.ContextDefinedInputs(new Type[] {typeof (int)})]
[FlowNode.ContextDefinedOutputs(new Type[] {typeof (int)})]
public class Flow_CustomForIterator : FlowControlNode
{
  public int current;

  public override void RegisterPorts()
  {
    ValueInput<int> n = this.AddValueInput<int>("Loops");
    this.AddValueOutput<int>("Index", (ValueHandler<int>) (() => this.current));
    FlowOutput fCurrent = this.AddFlowOutput("Do");
    FlowOutput fFinish = this.AddFlowOutput("Done");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      this.current = 0;
      fCurrent.Call(f);
    }));
    this.AddFlowInput("Iterate", (FlowHandler) (f =>
    {
      ++this.current;
      if (this.current < n.value)
        fCurrent.Call(f);
      else
        fFinish.Call(f);
    }));
  }
}
