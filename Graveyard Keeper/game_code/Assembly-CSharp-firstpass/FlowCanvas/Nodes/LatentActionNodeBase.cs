// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.LatentActionNodeBase
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Services;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

public abstract class LatentActionNodeBase : SimplexNode
{
  public LatentActionNodeBase.InvocationMode invocationMode;
  public FlowOutput outFlow;
  public FlowOutput doing;
  public FlowOutput done;
  public Queue<IEnumerator> enumeratorQueue = new Queue<IEnumerator>();
  public Queue<Flow> flowQueue = new Queue<Flow>();
  public Coroutine currentCoroutine;
  public bool graphPaused;

  public override string name
  {
    get
    {
      return this.enumeratorQueue.Count <= 0 ? base.name : $"{base.name} [{this.enumeratorQueue.Count.ToString()}]";
    }
  }

  public sealed override void OnGraphStarted()
  {
    this.enumeratorQueue = new Queue<IEnumerator>();
    this.flowQueue = new Queue<Flow>();
    this.currentCoroutine = (Coroutine) null;
  }

  public sealed override void OnGraphStoped() => this.Break();

  public sealed override void OnGraphPaused() => this.graphPaused = true;

  public sealed override void OnGraphUnpaused() => this.graphPaused = false;

  public void Begin(IEnumerator enumerator, Flow f)
  {
    if (this.exposeRoutineControls && this.invocationMode == LatentActionNodeBase.InvocationMode.QueueCalls && !this.enumeratorQueue.Contains(enumerator))
    {
      this.enumeratorQueue.Enqueue(enumerator);
      this.flowQueue.Enqueue(f);
    }
    if (this.currentCoroutine != null)
      return;
    this.currentCoroutine = MonoManager.current.StartCoroutine(this.InternalCoroutine(enumerator, f));
  }

  public void Break()
  {
    if (this.currentCoroutine == null)
      return;
    MonoManager.current.StopCoroutine(this.currentCoroutine);
    this.enumeratorQueue = new Queue<IEnumerator>();
    this.flowQueue = new Queue<Flow>();
    this.currentCoroutine = (Coroutine) null;
    this.done.parent.SetStatus(NodeCanvas.Status.Resting);
    this.OnBreak();
    this.done.Call(new Flow());
  }

  public IEnumerator InternalCoroutine(IEnumerator enumerator, Flow f)
  {
    LatentActionNodeBase latentActionNodeBase = this;
    FlowNode parentNode = latentActionNodeBase.done.parent;
    parentNode.SetStatus(NodeCanvas.Status.Running);
    if (latentActionNodeBase.outFlow != null)
      latentActionNodeBase.outFlow.Call(f);
    f.Break = new FlowBreak(latentActionNodeBase.Break);
    while (enumerator.MoveNext())
    {
      while (latentActionNodeBase.graphPaused)
        yield return (object) null;
      if (latentActionNodeBase.doing != null)
        latentActionNodeBase.doing.Call(f);
      yield return enumerator.Current;
    }
    f.Break = (FlowBreak) null;
    parentNode.SetStatus(NodeCanvas.Status.Resting);
    latentActionNodeBase.done.Call(f);
    latentActionNodeBase.currentCoroutine = (Coroutine) null;
    if (latentActionNodeBase.enumeratorQueue.Count > 0)
    {
      latentActionNodeBase.enumeratorQueue.Dequeue();
      latentActionNodeBase.flowQueue.Dequeue();
      if (latentActionNodeBase.enumeratorQueue.Count > 0)
        latentActionNodeBase.Begin(latentActionNodeBase.enumeratorQueue.Peek(), latentActionNodeBase.flowQueue.Peek());
    }
  }

  public override void OnRegisterPorts(FlowNode node)
  {
    if (this.exposeRoutineControls)
    {
      this.outFlow = node.AddFlowOutput("Start", "Out");
      this.doing = node.AddFlowOutput("Update", "Doing");
    }
    this.done = node.AddFlowOutput("Finish", "Done");
    this.OnRegisterDerivedPorts(node);
    if (!this.exposeRoutineControls)
      return;
    node.AddFlowInput("Break", (FlowHandler) (f => this.Break()));
  }

  public abstract void OnRegisterDerivedPorts(FlowNode node);

  public virtual void OnBreak()
  {
  }

  public virtual bool exposeRoutineControls => true;

  [CompilerGenerated]
  public void \u003COnRegisterPorts\u003Eb__18_0(Flow f) => this.Break();

  public enum InvocationMode
  {
    QueueCalls,
    FilterCalls,
  }
}
