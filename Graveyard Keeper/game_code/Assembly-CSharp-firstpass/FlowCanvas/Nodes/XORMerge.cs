// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.XORMerge
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Obsolete]
[Category("Flow Controllers/Flow Merge")]
[Name("XOR", 0)]
[Description("Calls Out when either single Input is called, but no other is in the same frame.")]
public class XORMerge : FlowControlNode, IMultiPortNode
{
  [SerializeField]
  public int _portCount = 2;
  public FlowOutput fOut;
  public int[] calls;
  public int lastFrameCall;

  public int portCount
  {
    get => this._portCount;
    set => this._portCount = value;
  }

  public override void RegisterPorts()
  {
    this.calls = new int[this.portCount];
    this.fOut = this.AddFlowOutput("Out");
    for (int index = 0; index < this.portCount; ++index)
    {
      int i = index;
      this.AddFlowInput(i.ToString(), (FlowHandler) (f => this.Check(i, f)));
    }
  }

  public void Check(int index, Flow f)
  {
    this.calls[index] = Time.frameCount;
    for (int index1 = 0; index1 < this.calls.Length; ++index1)
    {
      if (index1 != index && this.calls[index1] == this.lastFrameCall)
        return;
    }
    this.fOut.Call(f);
    this.lastFrameCall = Time.frameCount;
  }
}
