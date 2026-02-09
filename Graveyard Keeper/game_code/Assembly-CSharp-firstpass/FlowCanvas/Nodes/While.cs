// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.While
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Once called, will continuously call 'Do' while the input boolean condition is true. Once condition becomes or is false, 'Done' is called")]
[Category("Flow Controllers/Repeaters")]
[Name("While True", 0)]
[FlowNode.ContextDefinedInputs(new System.Type[] {typeof (bool)})]
public class While : FlowControlNode
{
  public Coroutine coroutine;

  public override void OnGraphStarted() => this.coroutine = (Coroutine) null;

  public override void OnGraphStoped()
  {
    if (this.coroutine == null)
      return;
    this.StopCoroutine(this.coroutine);
    this.coroutine = (Coroutine) null;
  }

  public override void RegisterPorts()
  {
    ValueInput<bool> c = this.AddValueInput<bool>("Condition");
    FlowOutput fCurrent = this.AddFlowOutput("Do");
    FlowOutput fFinish = this.AddFlowOutput("Done");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (this.coroutine != null)
        return;
      this.coroutine = this.StartCoroutine(this.DoWhile(fCurrent, fFinish, f, c));
    }));
  }

  public IEnumerator DoWhile(
    FlowOutput fCurrent,
    FlowOutput fFinish,
    Flow f,
    ValueInput<bool> condition)
  {
    While @while = this;
    f.Break = new FlowBreak(@while.\u003CDoWhile\u003Eb__4_0);
    while (@while.coroutine != null && condition.value)
    {
      while (@while.graph.isPaused)
        yield return (object) null;
      fCurrent.Call(f);
      yield return (object) null;
    }
    @while.coroutine = (Coroutine) null;
    f.Break = (FlowBreak) null;
    fFinish.Call(f);
  }

  [CompilerGenerated]
  public void \u003CDoWhile\u003Eb__4_0() => this.coroutine = (Coroutine) null;
}
