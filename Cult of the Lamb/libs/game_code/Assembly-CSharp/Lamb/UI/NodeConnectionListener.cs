// Decompiled with JetBrains decompiler
// Type: Lamb.UI.NodeConnectionListener
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class NodeConnectionListener : ConnectionListener
{
  public UpgradeTreeNode Node;

  public override void Configure(MMUILineRenderer.Branch rootBranch)
  {
    base.Configure(rootBranch);
    UnityEngine.Debug.Log((object) "Configure Node Connection Listener".Colour(Color.cyan));
    this._highestNodeState = this.Node.State;
    this._targetNodeState = this._highestNodeState;
    this.UpdateState(this.Branch.FillStyle == MMUILineRenderer.FillStyle.Reverse ? 0.0f : 1f);
  }

  public void OnEnable()
  {
    this.Node.OnStateDidChange += new Action<UpgradeTreeNode>(this.OnNodeStateDidChange);
  }

  public void OnDisable()
  {
    this.Node.OnStateDidChange -= new Action<UpgradeTreeNode>(this.OnNodeStateDidChange);
  }

  public void OnNodeStateDidChange(UpgradeTreeNode node)
  {
    if (this.Branch == null)
      return;
    if (this.Branch.FillStyle == MMUILineRenderer.FillStyle.Reverse)
    {
      if (this.Node.State > this._highestNodeState)
      {
        this._targetNodeState = this.Node.State;
        this._isDirty = true;
      }
    }
    else if (this.Node.State > this._highestNodeState)
    {
      this._highestNodeState = this.Node.State;
      this._isDirty = true;
    }
    if (!this._isDirty)
      return;
    Action<ConnectionListener> onStateChanged = this.OnStateChanged;
    if (onStateChanged != null)
      onStateChanged((ConnectionListener) this);
    this.UpdateState(this.Branch.FillStyle == MMUILineRenderer.FillStyle.Reverse ? 1f : 0.0f);
  }

  public override IEnumerator DoFillAnimation()
  {
    NodeConnectionListener connectionListener = this;
    yield return (object) connectionListener.\u003C\u003En__0();
    if (connectionListener.Branch.FillStyle != MMUILineRenderer.FillStyle.Reverse)
      yield return (object) connectionListener.Node.DoUpdateStateAnimation();
    connectionListener._isDirty = false;
  }

  [CompilerGenerated]
  [DebuggerHidden]
  public IEnumerator \u003C\u003En__0() => base.DoFillAnimation();
}
