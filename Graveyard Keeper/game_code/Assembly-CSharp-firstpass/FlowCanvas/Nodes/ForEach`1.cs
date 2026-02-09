// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ForEach`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System;
using System.Collections.Generic;

#nullable disable
namespace FlowCanvas.Nodes;

[ExposeAsDefinition]
[FlowNode.ContextDefinedOutputs(new Type[] {typeof (Wild)})]
[Description("Enumerate a value (usualy a list or array) for each of it's elements")]
[Category("Flow Controllers/Iterators")]
[FlowNode.ContextDefinedInputs(new Type[] {typeof (IEnumerable<>)})]
public class ForEach<T> : FlowControlNode
{
  public T current;
  public bool broken;

  public override void RegisterPorts()
  {
    ValueInput<IEnumerable<T>> list = this.AddValueInput<IEnumerable<T>>("Value");
    this.AddValueOutput<T>("Current", (ValueHandler<T>) (() => this.current));
    FlowOutput fCurrent = this.AddFlowOutput("Do");
    FlowOutput fFinish = this.AddFlowOutput("Done");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      IEnumerable<T> objs = list.value;
      if (objs == null)
      {
        fFinish.Call(f);
      }
      else
      {
        this.broken = false;
        f.Break = (FlowBreak) (() => this.broken = true);
        foreach (T obj in objs)
        {
          if (!this.broken)
          {
            this.current = obj;
            fCurrent.Call(f);
          }
          else
            break;
        }
        f.Break = (FlowBreak) null;
        fFinish.Call(f);
      }
    }));
    this.AddFlowInput("Break", (FlowHandler) (f => this.broken = true));
  }
}
