// Decompiled with JetBrains decompiler
// Type: FollowerTask_MissionaryComplete
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_MissionaryComplete : FollowerTask
{
  public Coroutine _dissentBubbleCoroutine;
  public Follower follower;

  public override FollowerTaskType Type => FollowerTaskType.MissionaryComplete;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool ShouldSaveDestination => true;

  public override bool DisablePickUpInteraction => true;

  public override bool BlockReactTasks => true;

  public override bool BlockSermon => true;

  public override bool BlockTaskChanges => true;

  public override bool BlockThoughts => true;

  public override float Priorty => 1000f;

  public override PriorityCategory GetPriorityCategory(
    FollowerRole FollowerRole,
    WorkerPriority WorkerPriority,
    FollowerBrain brain)
  {
    return PriorityCategory.ExtremelyUrgent;
  }

  public override int GetSubTaskCode() => 0;

  public override float SatiationChange(float deltaGameTime) => 0.0f;

  public override float RestChange(float deltaGameTime) => 0.0f;

  public override void TaskTick(float deltaGameTime)
  {
  }

  public override void OnGoingToBegin(Follower follower)
  {
    base.OnGoingToBegin(follower);
    if (!(bool) (Object) follower)
      return;
    follower.Seeker.CancelCurrentPathRequest();
    this.ClearDestination();
    this.SetState(FollowerTaskState.Doing);
    follower.transform.position = this.UpdateDestination(follower);
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    List<Structures_Missionary> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_Missionary>(this._brain.Location);
    foreach (Interaction_Missionaries missionary in Interaction_Missionaries.Missionaries)
    {
      if (missionary.StructureInfo.ID == structuresOfType[follower.Brain._directInfoAccess.MissionaryIndex].Data.ID && missionary.StructureInfo.MultipleFollowerIDs.Contains(follower.Brain.Info.ID))
        return missionary.MissionarySlots[missionary.StructureInfo.MultipleFollowerIDs.IndexOf(follower.Brain.Info.ID)].Free.transform.position;
    }
    return structuresOfType.Count <= 0 ? this._brain.LastPosition : structuresOfType[this._brain._directInfoAccess.MissionaryIndex].Data.Position;
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    if (DataManager.Instance.Followers_Imprisoned_IDs.Contains(this.Brain.Info.ID))
      DataManager.Instance.Followers_Imprisoned_IDs.Remove(this.Brain.Info.ID);
    if (follower.Brain._directInfoAccess.MissionaryRewards == null)
      return;
    double num = (double) follower.SetBodyAnimation("attention", true);
    follower.HideStats();
    follower.GetComponent<Interaction_MissionaryComplete>().enabled = true;
    follower.Interaction_FollowerInteraction.Interactable = false;
    this._dissentBubbleCoroutine = follower.StartCoroutine((IEnumerator) this.DissentBubbleRoutine(follower));
    follower.SetOutfit(FollowerOutfitType.Sherpa, false);
    if (follower.Brain._directInfoAccess.MissionaryRewards.Length == 0)
    {
      follower.Brain.Stats.Rest = 10f;
      follower.SetFaceAnimation("Emotions/emotion-tired", true);
    }
    this.follower = follower;
    this.ClearDestination();
    this.SetState(FollowerTaskState.GoingTo);
  }

  public override void OnAbort()
  {
    base.OnAbort();
    if (!(bool) (Object) this.follower)
      return;
    this.follower.SetOutfit(FollowerOutfitType.Follower, false);
    this.follower.Interaction_FollowerInteraction.Interactable = true;
    this.follower.GetComponent<Interaction_MissionaryComplete>().enabled = false;
    if (!this._brain._directInfoAccess.MissionaryFinished || this._brain._directInfoAccess.MissionaryRewards.Length != 0)
      return;
    this.SetState(FollowerTaskState.Done);
    this.follower.StartCoroutine((IEnumerator) this.FrameDelayDeath());
  }

  public IEnumerator FrameDelayDeath()
  {
    yield return (object) new WaitForEndOfFrame();
    this.follower.Die(NotificationCentre.NotificationType.DiedOnMissionary);
  }

  public override void OnEnd()
  {
    base.OnEnd();
    if (!(bool) (Object) this.follower)
      return;
    if (this._dissentBubbleCoroutine != null)
    {
      this.follower.StopCoroutine(this._dissentBubbleCoroutine);
      this._dissentBubbleCoroutine = (Coroutine) null;
      this.follower.WorshipperBubble.Close();
    }
    this.follower.SetOutfit(FollowerOutfitType.Follower, false);
    this.follower.Interaction_FollowerInteraction.Interactable = true;
    this.follower.GetComponent<Interaction_MissionaryComplete>().enabled = false;
  }

  public override void OnComplete()
  {
    base.OnComplete();
    if (this._dissentBubbleCoroutine == null || !((Object) this.follower != (Object) null))
      return;
    this.follower.StopCoroutine(this._dissentBubbleCoroutine);
    this._dissentBubbleCoroutine = (Coroutine) null;
    this.follower.WorshipperBubble.Close();
  }

  public IEnumerator DissentBubbleRoutine(Follower follower)
  {
    float bubbleTimer = 0.3f;
    while (true)
    {
      if ((double) (bubbleTimer -= Time.deltaTime) < 0.0)
      {
        follower.WorshipperBubble.Play(WorshipperBubble.SPEECH_TYPE.HELP);
        bubbleTimer = (float) (4 + Random.Range(0, 2));
      }
      yield return (object) null;
    }
  }
}
