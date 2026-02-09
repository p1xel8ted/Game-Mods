// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ANDMerge
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Flow Controllers/Flow Merge")]
[Description("Calls Out when all inputs are called together in the same frame")]
[Name("AND", 0)]
public class ANDMerge : FlowControlNode, IMultiPortNode
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
      if (this.calls[index1] != this.calls[index])
        return;
    }
    if (Time.frameCount == this.lastFrameCall)
      return;
    this.lastFrameCall = Time.frameCount;
    this.fOut.Call(f);
  }
}
