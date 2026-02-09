// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.LatchInt
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Convert a Flow signal to an integer value")]
[FlowNode.ContextDefinedOutputs(new System.Type[] {typeof (int)})]
[Category("Flow Controllers/Flow Convert")]
[Name("Latch Integer", 0)]
public class LatchInt : FlowControlNode, IMultiPortNode
{
  [SerializeField]
  public int _portCount = 4;
  public int latched;

  public int portCount
  {
    get => this._portCount;
    set => this._portCount = value;
  }

  public override void RegisterPorts()
  {
    FlowOutput o = this.AddFlowOutput("Out");
    for (int index = 0; index < this.portCount; ++index)
    {
      int i = index;
      this.AddFlowInput(i.ToString(), (FlowHandler) (f =>
      {
        this.latched = i;
        o.Call(f);
      }));
    }
    this.AddValueOutput<int>("Value", (ValueHandler<int>) (() => this.latched));
  }
}
