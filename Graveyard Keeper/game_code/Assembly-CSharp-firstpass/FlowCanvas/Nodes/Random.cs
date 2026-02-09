// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Random
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[FlowNode.ContextDefinedOutputs(new System.Type[] {typeof (Flow), typeof (int)})]
[Description("Calls one random output each time In is called")]
public class Random : FlowControlNode, IMultiPortNode
{
  [SerializeField]
  public int _portCount = 4;
  public int current;

  public int portCount
  {
    get => this._portCount;
    set => this._portCount = value;
  }

  public override void RegisterPorts()
  {
    List<FlowOutput> outs = new List<FlowOutput>();
    for (int index = 0; index < this.portCount; ++index)
      outs.Add(this.AddFlowOutput(index.ToString()));
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      this.current = UnityEngine.Random.Range(0, this.portCount);
      outs[this.current].Call(f);
    }));
    this.AddValueOutput<int>("Current", (ValueHandler<int>) (() => this.current));
  }
}
