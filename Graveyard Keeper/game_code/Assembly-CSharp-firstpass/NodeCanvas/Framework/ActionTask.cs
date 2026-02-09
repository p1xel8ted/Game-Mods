// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Framework.ActionTask
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Services;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Framework;

public abstract class ActionTask : Task
{
  [NonSerialized]
  public NodeCanvas.Status status = NodeCanvas.Status.Resting;
  [NonSerialized]
  public float startedTime;
  [NonSerialized]
  public float pausedTime;
  [NonSerialized]
  public bool latch;
  [NonSerialized]
  public bool _isPaused;

  public float elapsedTime
  {
    get
    {
      if (this.isPaused)
        return this.pausedTime - this.startedTime;
      return this.isRunning ? Time.time - this.startedTime : 0.0f;
    }
  }

  public bool isRunning => this.status == NodeCanvas.Status.Running;

  public bool isPaused
  {
    get => this._isPaused;
    set => this._isPaused = value;
  }

  public void ExecuteAction(Component agent, Action<bool> callback)
  {
    this.ExecuteAction(agent, (IBlackboard) null, callback);
  }

  public void ExecuteAction(Component agent, IBlackboard blackboard, Action<bool> callback)
  {
    if (this.isRunning)
      return;
    MonoManager.current.StartCoroutine(this.ActionUpdater(agent, blackboard, callback));
  }

  public IEnumerator ActionUpdater(Component agent, IBlackboard blackboard, Action<bool> callback)
  {
    while (this.ExecuteAction(agent, blackboard) == NodeCanvas.Status.Running)
      yield return (object) null;
    if (callback != null)
      callback(this.status == NodeCanvas.Status.Success);
  }

  public NodeCanvas.Status ExecuteAction(Component agent, IBlackboard blackboard)
  {
    if (!this.isActive)
      return NodeCanvas.Status.Failure;
    if (this.isPaused)
    {
      this.startedTime += Time.time - this.pausedTime;
      this.isPaused = false;
    }
    if (this.status == NodeCanvas.Status.Running)
    {
      this.OnUpdate();
      this.latch = false;
      return this.status;
    }
    if (this.latch)
    {
      this.latch = false;
      return this.status;
    }
    if (!this.Set(agent, blackboard))
      return NodeCanvas.Status.Failure;
    this.startedTime = Time.time;
    this.status = NodeCanvas.Status.Running;
    this.OnExecute();
    if (this.status == NodeCanvas.Status.Running)
      this.OnUpdate();
    this.latch = false;
    return this.status;
  }

  public void EndAction() => this.EndAction(true);

  public void EndAction(bool success) => this.EndAction(new bool?(success));

  public void EndAction(bool? success)
  {
    this.latch = success.HasValue;
    if (this.status != NodeCanvas.Status.Running)
      return;
    this.isPaused = false;
    bool? nullable = success;
    bool flag = true;
    this.status = nullable.GetValueOrDefault() == flag & nullable.HasValue ? NodeCanvas.Status.Success : NodeCanvas.Status.Failure;
    this.OnStop();
  }

  public void PauseAction()
  {
    if (this.status != NodeCanvas.Status.Running)
      return;
    this.pausedTime = Time.time;
    this.isPaused = true;
    this.OnPause();
  }

  public virtual void OnExecute()
  {
  }

  public virtual void OnUpdate()
  {
  }

  public virtual void OnStop()
  {
  }

  public virtual void OnPause()
  {
  }
}
