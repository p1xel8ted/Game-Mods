// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.MultiToggle
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Whenever In is called the 'current' output is called as well. Calling '+' or '-' changes the current output respectively up or down.")]
[Category("Flow Controllers/Togglers")]
[FlowNode.ContextDefinedOutputs(new System.Type[] {typeof (int)})]
[Name("Toggle (Multi)", 0)]
public class MultiToggle : FlowControlNode, IMultiPortNode
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

  public override string name => $"{base.name} {$"[{this.current.ToString()}]"}";

  public override void OnGraphStarted()
  {
    this.current = Mathf.Clamp(this.current, 0, this.portCount - 1);
    this.original = this.current;
  }

  public override void OnGraphStoped() => this.current = this.original;

  public override void RegisterPorts()
  {
    List<FlowOutput> outs = new List<FlowOutput>();
    for (int index = 0; index < this.portCount; ++index)
      outs.Add(this.AddFlowOutput(index.ToString()));
    this.AddFlowInput("In", (FlowHandler) (f => outs[this.current].Call(f)));
    this.AddFlowInput("-", (FlowHandler) (f => this.current = (int) Mathf.Repeat((float) (this.current - 1), (float) this.portCount)));
    this.AddFlowInput("+", (FlowHandler) (f => this.current = (int) Mathf.Repeat((float) (this.current + 1), (float) this.portCount)));
    this.AddValueOutput<int>("Current", (ValueHandler<int>) (() => this.current));
  }
}
