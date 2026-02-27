// Decompiled with JetBrains decompiler
// Type: Lamb.UI.ConnectionListener
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI.Assets;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public abstract class ConnectionListener : BaseMonoBehaviour
{
  protected float kFillPerSecond = 2500f;
  public Action<ConnectionListener> OnStateChanged;
  protected bool _isDirty;
  protected UpgradeTreeNode.NodeState _highestNodeState;
  protected UpgradeTreeNode.NodeState _targetNodeState;
  [NonSerialized]
  public MMUILineRenderer.Branch Branch;
  public int BranchHash;
  public UpgradeTreeNode.TreeTier TreeTier;
  public int Depth;
  public UpgradeTreeConfiguration Configuration;

  public bool IsDirty => this._isDirty;

  public UpgradeTreeNode.NodeState HighestNodeState => this._highestNodeState;

  public UpgradeTreeNode.NodeState TargetNodeState => this._targetNodeState;

  public virtual void Configure(MMUILineRenderer.Branch rootBranch)
  {
    if (this.Branch != null)
      return;
    this.Branch = rootBranch.FindBranchByHash(this.BranchHash);
  }

  protected void UpdateState(float fill)
  {
    this.UpdateColor(this._highestNodeState);
    this.Branch.Fill = fill;
  }

  private void UpdateColor(UpgradeTreeNode.NodeState nodeState)
  {
    switch (nodeState)
    {
      case UpgradeTreeNode.NodeState.Locked:
        this.Branch.Color = StaticColors.DarkGreyColor;
        break;
      case UpgradeTreeNode.NodeState.Unavailable:
        this.Branch.Color = StaticColors.GreyColor;
        break;
      case UpgradeTreeNode.NodeState.Available:
        this.Branch.Color = StaticColors.RedColor;
        break;
      case UpgradeTreeNode.NodeState.Unlocked:
        this.Branch.Color = StaticColors.GreenColor;
        break;
    }
  }

  public void PerformFillAnimation() => this.StartCoroutine((IEnumerator) this.DoFillAnimation());

  protected virtual IEnumerator DoFillAnimation()
  {
    float rate = this.kFillPerSecond / this.Branch.TotalLength;
    if (this.Branch.FillStyle == MMUILineRenderer.FillStyle.Standard)
    {
      while ((double) this.Branch.Fill < 1.0)
      {
        this.Branch.Fill += rate * Time.unscaledDeltaTime;
        yield return (object) null;
      }
    }
    else if (this.Branch.FillStyle == MMUILineRenderer.FillStyle.Reverse)
    {
      while ((double) this.Branch.Fill > 0.0)
      {
        this.Branch.Fill -= rate * Time.unscaledDeltaTime;
        yield return (object) null;
      }
      this._highestNodeState = this._targetNodeState;
      this.UpdateColor(this._highestNodeState);
    }
  }
}
