// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Sequence
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Flip Flop (Multi)", 0)]
[Category("Flow Controllers/Togglers")]
[Description("Each time input is signaled, the next output in order is called. After the last output, the order loops from the start.\nReset, resets the current index to zero.")]
[FlowNode.ContextDefinedOutputs(new System.Type[] {typeof (int)})]
public class Sequence : FlowControlNode, IMultiPortNode
{
  [SerializeField]
  public int _portCount = 4;
  public int current;
  public int original;

  public int portCount
  {
    get => this._portCount;
    set => this._portCount = value;
  }

  public override void OnGraphStarted()
  {
    this.current = Mathf.Clamp(this.current, 0, this.portCount - 1);
    this.original = this.current;
  }

  public override void OnGraphStoped() => this.current = this.original;

  public override string name => $"{base.name} {$"[{this.current.ToString()}]"}";

  public override void RegisterPorts()
  {
    List<FlowOutput> outs = new List<FlowOutput>();
    for (int index = 0; index < this.portCount; ++index)
      outs.Add(this.AddFlowOutput(index.ToString()));
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      outs[this.current].Call(f);
      this.current = (int) Mathf.Repeat((float) (this.current + 1), (float) this.portCount);
    }));
    this.AddFlowInput("Reset", (FlowHandler) (f => this.current = 0));
    this.AddValueOutput<int>("Current", (ValueHandler<int>) (() => this.current));
  }
}
