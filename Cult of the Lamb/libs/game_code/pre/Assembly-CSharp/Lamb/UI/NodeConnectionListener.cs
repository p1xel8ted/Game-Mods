// Decompiled with JetBrains decompiler
// Type: Lamb.UI.NodeConnectionListener
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class NodeConnectionListener : ConnectionListener
{
  public UpgradeTreeNode Node;

  public override void Configure(MMUILineRenderer.Branch rootBranch)
  {
    base.Configure(rootBranch);
    Debug.Log((object) "Configure Node Connection Listener".Colour(Color.cyan));
    this._highestNodeState = this.Node.State;
    this._targetNodeState = this._highestNodeState;
    this.UpdateState(this.Branch.FillStyle == MMUILineRenderer.FillStyle.Reverse ? 0.0f : 1f);
  }

  private void OnEnable()
  {
    this.Node.OnStateDidChange += new Action<UpgradeTreeNode>(this.OnNodeStateDidChange);
  }

  private void OnDisable()
  {
    this.Node.OnStateDidChange -= new Action<UpgradeTreeNode>(this.OnNodeStateDidChange);
  }

  private void OnNodeStateDidChange(UpgradeTreeNode node)
  {
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

  protected override IEnumerator DoFillAnimation()
  {
    NodeConnectionListener connectionListener = this;
    // ISSUE: reference to a compiler-generated method
    yield return (object) connectionListener.\u003C\u003En__0();
    if (connectionListener.Branch.FillStyle != MMUILineRenderer.FillStyle.Reverse)
      yield return (object) connectionListener.Node.DoUpdateStateAnimation();
    connectionListener._isDirty = false;
  }
}
