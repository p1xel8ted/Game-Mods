// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.CoroutineState
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Flow Controllers/Repeaters")]
[Name("Coroutine", 0)]
[Description("Start a Coroutine that will repeat until Break is signaled")]
public class CoroutineState : FlowControlNode
{
  public bool active;
  public Coroutine coroutine;

  public override void OnGraphStoped()
  {
    if (this.coroutine == null)
      return;
    this.StopCoroutine(this.coroutine);
    this.active = false;
  }

  public override void RegisterPorts()
  {
    FlowOutput fStarted = this.AddFlowOutput("Start");
    FlowOutput fUpdate = this.AddFlowOutput("Update");
    FlowOutput fFinish = this.AddFlowOutput("Finish");
    this.AddFlowInput("Start", (FlowHandler) (f =>
    {
      if (this.active)
        return;
      this.active = true;
      this.coroutine = this.StartCoroutine(this.DoRepeat(fStarted, fUpdate, fFinish, f));
    }));
    this.AddFlowInput("Break", (FlowHandler) (f => this.active = false));
  }

  public IEnumerator DoRepeat(FlowOutput fStarted, FlowOutput fUpdate, FlowOutput fFinish, Flow f)
  {
    CoroutineState coroutineState = this;
    f.Break = new FlowBreak(coroutineState.\u003CDoRepeat\u003Eb__4_0);
    fStarted.Call(f);
    while (coroutineState.active)
    {
      while (coroutineState.graph.isPaused)
        yield return (object) null;
      fUpdate.Call(f);
      yield return (object) null;
    }
    f.Break = (FlowBreak) null;
    fFinish.Call(f);
  }

  [CompilerGenerated]
  public void \u003CDoRepeat\u003Eb__4_0() => this.active = false;
}
