// Decompiled with JetBrains decompiler
// Type: Structures_Morgue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Structures_Morgue : StructureBrain, ITaskProvider
{
  public override bool IsFull => this.Data.MultipleFollowerIDs.Count >= this.DEAD_BODY_SLOTS;

  public static int GetCapacity(StructureBrain.TYPES Type)
  {
    if (Type == StructureBrain.TYPES.MORGUE_1)
      return 3;
    return Type == StructureBrain.TYPES.MORGUE_2 ? 6 : 6;
  }

  public int DEAD_BODY_SLOTS => Structures_Morgue.GetCapacity(this.Data.Type);

  public event Structures_Morgue.MorgueEvent OnBodyDeposited;

  public event Structures_Morgue.MorgueEvent OnBodyWithdrawn;

  public FollowerTask GetOverrideTask(FollowerBrain brain) => (FollowerTask) null;

  public bool CheckOverrideComplete() => true;

  public void GetAvailableTasks(ScheduledActivity activity, SortedList<float, FollowerTask> tasks)
  {
    if (activity != ScheduledActivity.Work || this.ReservedForTask || this.IsFull)
      return;
    List<StructureBrain> structureBrainList = new List<StructureBrain>();
    structureBrainList.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(in this.Data.Location, StructureBrain.TYPES.DEAD_WORSHIPPER));
    for (int index = structureBrainList.Count - 1; index >= 0; --index)
    {
      if (structureBrainList[index].ReservedByPlayer || structureBrainList[index].ReservedForTask || structureBrainList[index].Data.BeenInMorgueAlready || structureBrainList[index].Data.BodyWrapped)
      {
        structureBrainList.RemoveAt(index);
      }
      else
      {
        FollowerInfo followerInfoById = FollowerManager.GetDeadFollowerInfoByID(structureBrainList[index].Data.FollowerID);
        if (followerInfoById != null && (followerInfoById.DiedFromRot || followerInfoById.FrozeToDeath))
          structureBrainList.RemoveAt(index);
      }
    }
    if (this.ReservedForTask || structureBrainList.Count <= 0)
      return;
    FollowerTask_Undertaker followerTaskUndertaker = new FollowerTask_Undertaker(this.Data.ID);
    tasks.Add(followerTaskUndertaker.Priorty, (FollowerTask) followerTaskUndertaker);
  }

  public void DepositBody(int followerID)
  {
    if (!this.Data.MultipleFollowerIDs.Contains(followerID))
      this.Data.MultipleFollowerIDs.Add(followerID);
    if (this.Data.MultipleFollowerIDs.Count >= this.DEAD_BODY_SLOTS && (Object) NotificationCentre.Instance != (Object) null)
      NotificationCentre.Instance.PlayGenericNotification("Notifications/Morgue_Full/Notification/On");
    Structures_Morgue.MorgueEvent onBodyDeposited = this.OnBodyDeposited;
    if (onBodyDeposited == null)
      return;
    onBodyDeposited();
  }

  public void WithdrawBody(int followerID)
  {
    this.Data.MultipleFollowerIDs.Remove(followerID);
    Structures_Morgue.MorgueEvent onBodyWithdrawn = this.OnBodyWithdrawn;
    if (onBodyWithdrawn == null)
      return;
    onBodyWithdrawn();
  }

  public void WithdrawBodies(List<FollowerInfo> followers)
  {
    foreach (FollowerInfo follower in followers)
      this.WithdrawBody(follower.ID);
  }

  public delegate void MorgueEvent();
}
