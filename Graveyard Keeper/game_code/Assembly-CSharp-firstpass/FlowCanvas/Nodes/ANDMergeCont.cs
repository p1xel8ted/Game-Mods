// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ANDMergeCont
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("AND Cont", 0)]
[Description("Calls Out when all inputs are called (no matter at what frame)")]
[Category("Flow Controllers/Flow Merge")]
public class ANDMergeCont : FlowControlNode, IMultiPortNode
{
  [SerializeField]
  public int _portCount = 2;
  public FlowOutput fOut;
  public bool[] calls;
  public int lastFrameCall;

  public int portCount
  {
    get => this._portCount;
    set => this._portCount = value;
  }

  public override void RegisterPorts()
  {
    this.calls = new bool[this.portCount];
    this.fOut = this.AddFlowOutput("Out");
    for (int index = 0; index < this.portCount; ++index)
    {
      int i = index;
      this.AddFlowInput(i.ToString(), (FlowHandler) (f => this.Check(i, f)));
    }
  }

  public void Check(int index, Flow f)
  {
    this.calls[index] = true;
    bool flag = true;
    for (int index1 = 0; index1 < this.calls.Length; ++index1)
      flag &= this.calls[index1];
    if (!flag || Time.frameCount == this.lastFrameCall)
      return;
    this.lastFrameCall = Time.frameCount;
    this.fOut.Call(f);
  }
}
