// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ORMerge
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Calls Out when either input is called")]
[Category("Flow Controllers/Flow Merge")]
[Name("OR", 0)]
public class ORMerge : FlowControlNode, IMultiPortNode
{
  public FlowOutput fOut;
  public int lastFrameCall;
  [SerializeField]
  public int _portCount = 2;

  public int portCount
  {
    get => this._portCount;
    set => this._portCount = value;
  }

  public override void RegisterPorts()
  {
    this.fOut = this.AddFlowOutput("Out");
    for (int index = 0; index < this.portCount; ++index)
    {
      int i = index;
      this.AddFlowInput(i.ToString(), (FlowHandler) (f => this.Check(i, f)));
    }
  }

  public void Check(int index, Flow f)
  {
    if (Time.frameCount == this.lastFrameCall)
      return;
    this.lastFrameCall = Time.frameCount;
    this.fOut.Call(f);
  }
}
