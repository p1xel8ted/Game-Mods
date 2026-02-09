// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.StartDelayedEvent
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("On Start Delayed", 9)]
[Description("Called only once and the first time the Graph is enabled.\nThis is called 1 frame after all Awake events are called.")]
[Category("Events/Graph")]
public class StartDelayedEvent : EventNode
{
  public FlowOutput start;
  public bool called;

  public override void OnGraphStarted()
  {
    if (this.called)
      return;
    this.called = true;
    this.StartCoroutine(this.DelayCall());
  }

  public IEnumerator DelayCall()
  {
    yield return (object) null;
    this.start.Call(new Flow());
  }

  public override void RegisterPorts() => this.start = this.AddFlowOutput("Once");
}
