// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.MessageEventNode`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[FlowNode.ContextDefinedOutputs(new System.Type[] {typeof (Wild)})]
public abstract class MessageEventNode<T> : EventNode where T : Component
{
  public MessageEventNode<T>.TargetMode targetMode;
  [ShowIf("targetMode", 0)]
  public BBParameter<T> target;
  [ShowIf("targetMode", 1)]
  public BBParameter<List<T>> targets;

  public override string name
  {
    get
    {
      string empty = string.Empty;
      string str = this.targetMode != MessageEventNode<T>.TargetMode.SingleTarget ? this.targets.ToString() : (!this.target.isNull || this.target.useBlackboard ? this.target.ToString() : "Self");
      return $"{base.name.ToUpper()} ({str})";
    }
  }

  public abstract string[] GetTargetMessageEvents();

  public sealed override void OnGraphStarted()
  {
    if (this.targetMode == MessageEventNode<T>.TargetMode.SingleTarget)
    {
      if (this.target.isNull && !this.target.useBlackboard)
        this.target.value = this.graphAgent.GetComponent<T>();
      if (this.target.isNull)
      {
        int num = (int) this.Fail($"Target is missing component of type '{typeof (T).Name}'");
        return;
      }
      string[] targetMessageEvents = this.GetTargetMessageEvents();
      if (targetMessageEvents != null && targetMessageEvents.Length != 0)
        this.RegisterEvents((Component) this.target.value, targetMessageEvents);
    }
    if (this.targetMode != MessageEventNode<T>.TargetMode.MultipleTargets)
      return;
    if (this.targets.isNull || this.targets.value.Count == 0)
    {
      int num1 = (int) this.Fail("No Targets specified");
    }
    else
    {
      string[] targetMessageEvents = this.GetTargetMessageEvents();
      if (targetMessageEvents == null || targetMessageEvents.Length == 0)
        return;
      foreach (T targetAgent in this.targets.value)
        this.RegisterEvents((Component) targetAgent, targetMessageEvents);
    }
  }

  public sealed override void OnGraphStoped()
  {
    if (this.targetMode == MessageEventNode<T>.TargetMode.SingleTarget)
      this.UnRegisterEvents((Component) this.target.value, this.GetTargetMessageEvents());
    if (this.targetMode != MessageEventNode<T>.TargetMode.MultipleTargets)
      return;
    string[] targetMessageEvents = this.GetTargetMessageEvents();
    foreach (T targetAgent in this.targets.value)
      this.UnRegisterEvents((Component) targetAgent, targetMessageEvents);
  }

  public T ResolveReceiver(GameObject receiver)
  {
    return this.targetMode == MessageEventNode<T>.TargetMode.SingleTarget ? this.target.value : this.targets.value.Find((Predicate<T>) (t => (UnityEngine.Object) t.gameObject == (UnityEngine.Object) receiver));
  }

  public enum TargetMode
  {
    SingleTarget,
    MultipleTargets,
  }
}
