// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.SwitchComparison
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Flow Controllers/Switchers")]
[Description("Branch the Flow based on a comparison between two comparable objects")]
[FlowNode.ContextDefinedInputs(new Type[] {typeof (IComparable)})]
public class SwitchComparison : FlowControlNode
{
  public override void RegisterPorts()
  {
    FlowOutput equal = this.AddFlowOutput("A = B", "==");
    FlowOutput notEqual = this.AddFlowOutput("A ≠ B", "!=");
    FlowOutput greater = this.AddFlowOutput("A > B", ">");
    FlowOutput less = this.AddFlowOutput("A < B", "<");
    ValueInput<IComparable> a = this.AddValueInput<IComparable>("A");
    ValueInput<IComparable> b = this.AddValueInput<IComparable>("B");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      IComparable comparable1 = a.value;
      IComparable comparable2 = b.value;
      if (comparable1 == null || comparable2 == null)
      {
        if (comparable1 == comparable2)
          equal.Call(f);
        if (comparable1 == comparable2)
          return;
        notEqual.Call(f);
      }
      else
      {
        int num1 = TypeConverter.QuickConvert<int>((object) comparable1);
        int num2 = TypeConverter.QuickConvert<int>((object) comparable2);
        if (num1 == num2)
          equal.Call(f);
        else
          notEqual.Call(f);
        if (num1 > num2)
          greater.Call(f);
        if (num1 >= num2)
          return;
        less.Call(f);
      }
    }));
  }
}
