// Decompiled with JetBrains decompiler
// Type: Structures_Tree
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

#nullable disable
public class Structures_Tree : StructureBrain, ITaskProvider
{
  public System.Action OnRegrowTree;
  public Action<int> OnTreeProgressChanged;
  public System.Action OnRegrowTreeProgressChanged;
  public Action<bool> OnTreeComplete;
  public bool RegrowTree = true;

  public float TreeHP => this.Data.ProgressTarget - this.Data.Progress;

  public bool TreeChopped => (double) this.Data.Progress >= (double) this.Data.ProgressTarget;

  public void TreeHit(float treeDamage = 1f, bool dropLoot = true, int followerID = -1)
  {
    if (this.TreeChopped)
      return;
    this.Data.Progress += treeDamage;
    Action<int> treeProgressChanged = this.OnTreeProgressChanged;
    if (treeProgressChanged != null)
      treeProgressChanged(followerID);
    if (!this.TreeChopped)
      return;
    Action<bool> onTreeComplete = this.OnTreeComplete;
    if (onTreeComplete == null)
      return;
    onTreeComplete(dropLoot);
  }

  public override void OnAdded()
  {
    base.OnAdded();
    TimeManager.OnNewDayStarted += new System.Action(this.OnNewDayStarted);
  }

  public override void OnRemoved()
  {
    base.OnRemoved();
    TimeManager.OnNewDayStarted -= new System.Action(this.OnNewDayStarted);
  }

  public void OnNewDayStarted()
  {
    if (this.Data.IsSapling && SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter)
    {
      ++this.Data.GrowthStage;
      Debug.Log((object) ("Growth: " + this.Data.GrowthStage.ToString()));
      System.Action treeProgressChanged = this.OnRegrowTreeProgressChanged;
      if (treeProgressChanged != null)
        treeProgressChanged();
    }
    if ((double) this.Data.GrowthStage < 6.0)
      return;
    this.Data.IsSapling = false;
    this.Data.Progress = 0.0f;
    this.Data.GrowthStage = 0.0f;
    System.Action onRegrowTree = this.OnRegrowTree;
    if (onRegrowTree == null)
      return;
    onRegrowTree();
  }

  public override void ToDebugString(StringBuilder sb)
  {
    base.ToDebugString(sb);
    sb.AppendLine($"HP: {this.TreeHP}/{this.Data.ProgressTarget}, Chopped: {this.TreeChopped}");
  }

  public FollowerTask GetOverrideTask(FollowerBrain brain) => (FollowerTask) null;

  public bool CheckOverrideComplete() => true;

  public void GetAvailableTasks(ScheduledActivity activity, SortedList<float, FollowerTask> tasks)
  {
    if (activity != ScheduledActivity.Work || this.ReservedForTask || this.TreeChopped || this.Data.IsSapling)
      return;
    FollowerTask_ChopTrees followerTaskChopTrees = new FollowerTask_ChopTrees(this.Data.ID);
    tasks.Add(followerTaskChopTrees.Priorty, (FollowerTask) followerTaskChopTrees);
  }
}
