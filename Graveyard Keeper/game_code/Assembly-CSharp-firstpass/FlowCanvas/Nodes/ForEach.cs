// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ForEach
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using ParadoxNotion.Design;
using System;
using System.Collections;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Flow Controllers/Iterators")]
[FlowNode.ContextDefinedInputs(new Type[] {typeof (IEnumerable)})]
[Description("Enumerate a value (usualy a list or array) for each of it's elements")]
[FlowNode.ContextDefinedOutputs(new Type[] {typeof (object)})]
public class ForEach : FlowControlNode
{
  public object current;
  public bool broken;
  public ValueInput<IEnumerable> enumerableInput;

  public override void RegisterPorts()
  {
    this.enumerableInput = this.AddValueInput<IEnumerable>("Value");
    this.AddValueOutput<object>("Current", (ValueHandler<object>) (() => this.current));
    FlowOutput fCurrent = this.AddFlowOutput("Do");
    FlowOutput fFinish = this.AddFlowOutput("Done");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      IEnumerable enumerable = this.enumerableInput.value;
      if (enumerable == null)
      {
        fFinish.Call(f);
      }
      else
      {
        this.broken = false;
        f.Break = (FlowBreak) (() => this.broken = true);
        foreach (object obj in enumerable)
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

  public override Type GetNodeWildDefinitionType() => typeof (IEnumerable);

  public override void OnPortConnected(Port port, Port otherPort)
  {
    if (port != this.enumerableInput)
      return;
    Type enumerableElementType = otherPort.type.GetEnumerableElementType();
    if (!Type.op_Inequality(enumerableElementType, (Type) null))
      return;
    this.ReplaceWith(typeof (ForEach<>).RTMakeGenericType(enumerableElementType));
  }
}
