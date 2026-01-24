// Decompiled with JetBrains decompiler
// Type: Lamb.UI.BranchConnectionListener
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class BranchConnectionListener : ConnectionListener
{
  [SerializeReference]
  public List<ConnectionListener> Connections = new List<ConnectionListener>();

  public override void Configure(MMUILineRenderer.Branch rootBranch)
  {
    base.Configure(rootBranch);
    UnityEngine.Debug.Log((object) "Configure Branch Connection Listener".Colour(Color.yellow));
    foreach (ConnectionListener connection in this.Connections)
    {
      if (connection.HighestNodeState > this._highestNodeState)
        this._highestNodeState = connection.HighestNodeState;
    }
    this._targetNodeState = this._highestNodeState;
    this.UpdateState(this.Branch.FillStyle == MMUILineRenderer.FillStyle.Reverse ? 0.0f : 1f);
  }

  public void OnConnectionStateDidChange(ConnectionListener connectionListener)
  {
    if (this.Branch.FillStyle == MMUILineRenderer.FillStyle.Reverse)
    {
      if (connectionListener.TargetNodeState > this._highestNodeState)
      {
        this._targetNodeState = connectionListener.TargetNodeState;
        this._isDirty = true;
      }
    }
    else if (connectionListener.HighestNodeState > this._highestNodeState)
    {
      this._highestNodeState = connectionListener.HighestNodeState;
      this._isDirty = true;
    }
    if (!this._isDirty)
      return;
    Action<ConnectionListener> onStateChanged = this.OnStateChanged;
    if (onStateChanged != null)
      onStateChanged((ConnectionListener) this);
    this.UpdateState(this.Branch.FillStyle == MMUILineRenderer.FillStyle.Reverse ? 1f : 0.0f);
  }

  public void OnEnable()
  {
    foreach (ConnectionListener connection in this.Connections)
      connection.OnStateChanged += new Action<ConnectionListener>(this.OnConnectionStateDidChange);
  }

  public void OnDisable()
  {
    foreach (ConnectionListener connection in this.Connections)
      connection.OnStateChanged -= new Action<ConnectionListener>(this.OnConnectionStateDidChange);
  }

  public override IEnumerator DoFillAnimation()
  {
    BranchConnectionListener connectionListener = this;
    yield return (object) connectionListener.\u003C\u003En__0();
    foreach (ConnectionListener connection in connectionListener.Connections)
    {
      if (connection.IsDirty)
        connection.PerformFillAnimation();
    }
    connectionListener._isDirty = false;
  }

  [CompilerGenerated]
  [DebuggerHidden]
  public IEnumerator \u003C\u003En__0() => base.DoFillAnimation();
}
