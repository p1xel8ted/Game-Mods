// Decompiled with JetBrains decompiler
// Type: Ritual
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Ritual : BaseMonoBehaviour
{
  public static Action<bool> OnEnd;
  public static List<FollowerBrain> FollowerToAttendSermon;

  protected virtual UpgradeSystem.Type RitualType { get; }

  public static int FollowersAvailableToAttendSermon()
  {
    return Ritual.GetFollowersAvailableToAttendSermon().Count;
  }

  public static List<FollowerBrain> GetFollowersAvailableToAttendSermon(bool ignoreDissenters = false)
  {
    List<FollowerBrain> availableToAttendSermon = new List<FollowerBrain>();
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if ((!ignoreDissenters || allBrain.Info.CursedState != Thought.Dissenter) && (allBrain.CurrentTask == null || !allBrain.CurrentTask.BlockSermon) && !FollowerManager.FollowerLocked(allBrain.Info.ID, true))
        availableToAttendSermon.Add(allBrain);
    }
    return availableToAttendSermon;
  }

  public virtual void Play()
  {
    foreach (FollowerBrain followerBrain in Ritual.GetFollowersAvailableToAttendSermon())
    {
      if (followerBrain.CurrentTask != null && (followerBrain.CurrentTask is FollowerTask_AttendRitual || followerBrain.CurrentTask is FollowerTask_AttendTeaching))
        followerBrain.CurrentTask.Abort();
    }
    Interaction_TempleAltar.Instance.FrontWall.SetActive(false);
  }

  public void CompleteRitual(bool cancelled = false, int targetFollowerID_1 = -1, int targetFollowerID_2 = -1)
  {
    if (!cancelled)
      FaithBarFake.Play(UpgradeSystem.GetRitualFaithChange(this.RitualType));
    Interaction_TempleAltar.Instance.FrontWall.SetActive(true);
    Action<bool> onEnd = Ritual.OnEnd;
    if (onEnd != null)
      onEnd(cancelled);
    if (!cancelled)
    {
      ObjectiveManager.CompleteRitualObjective(this.RitualType, targetFollowerID_1, targetFollowerID_2);
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.PerformAnyRitual);
      DataManager.Instance.HasPerformedRitual = true;
    }
    if (DataManager.Instance.WokeUpEveryoneDay == TimeManager.CurrentDay && TimeManager.CurrentPhase == DayPhase.Night && !FollowerBrainStats.IsWorkThroughTheNight)
      CultFaithManager.AddThought(Thought.Cult_WokeUpEveryone);
    GameManager.GetInstance().StartCoroutine((IEnumerator) Interaction_TempleAltar.Instance.FollowersEnterForSermonRoutine(true));
    Interaction_TempleAltar.Instance.OnInteract(PlayerFarming.Instance.state);
    ChurchFollowerManager.Instance.DisableAllOverlays();
    if (this.RitualType != UpgradeSystem.Type.Ritual_Feast)
      return;
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      allBrain.Stats.Starvation = 0.0f;
      allBrain.Stats.Satiation = 100f;
      allBrain.AddThought(Thought.FeastTable);
    }
  }

  public IEnumerator CentrePlayer()
  {
    Interaction_TempleAltar.Instance.state.facingAngle = 270f;
    float Progress = 0.0f;
    Vector3 StartPosition = Interaction_TempleAltar.Instance.state.transform.position;
    while ((double) (Progress += Time.deltaTime) < 0.5)
    {
      Interaction_TempleAltar.Instance.state.transform.position = Vector3.Lerp(StartPosition, ChurchFollowerManager.Instance.AltarPosition.position, Mathf.SmoothStep(0.0f, 1f, Progress / 0.5f));
      yield return (object) null;
    }
  }

  public IEnumerator CentreAndAnimatePlayer()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Ritual ritual = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      PlayerFarming.Instance.simpleSpineAnimator.Animate("build", 0, true);
      ritual.StartCoroutine((IEnumerator) ritual.CentrePlayer());
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    Interaction_TempleAltar.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForEndOfFrame();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator WaitFollowersFormCircle(bool ignoreDissenters = false)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Ritual ritual = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      GameManager.GetInstance().OnConversationNext(Interaction_TempleAltar.Instance.PortalEffect.gameObject, 8f);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    GameManager.GetInstance().OnConversationNext(Interaction_TempleAltar.Instance.RitualCameraPosition, 12f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) ritual.StartCoroutine((IEnumerator) ritual.FollowersEnterForRitualRoutine(ignoreDissenters));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator FollowersEnterForRitualRoutine(bool ignoreDissenters = false)
  {
    bool getFollowers = Ritual.FollowerToAttendSermon == null || Ritual.FollowerToAttendSermon.Count <= 0;
    if (getFollowers)
      Ritual.FollowerToAttendSermon = new List<FollowerBrain>();
    if (TimeManager.CurrentPhase == DayPhase.Night)
      DataManager.Instance.WokeUpEveryoneDay = TimeManager.CurrentDay;
    foreach (FollowerBrain followerBrain in Ritual.GetFollowersAvailableToAttendSermon(ignoreDissenters))
    {
      Follower followerById = FollowerManager.FindFollowerByID(followerBrain.Info.ID);
      if ((bool) (UnityEngine.Object) followerById)
      {
        followerById.Brain.ShouldReconsiderTask = false;
        followerById.HideAllFollowerIcons();
        followerById.Spine.UseDeltaTime = false;
        followerById.UseUnscaledTime = true;
      }
      if (getFollowers)
        Ritual.FollowerToAttendSermon.Add(followerBrain);
      if (followerBrain.CurrentTask != null)
        followerBrain.CurrentTask.Abort();
      followerBrain.HardSwapToTask((FollowerTask) new FollowerTask_AttendRitual());
      followerBrain.ShouldReconsiderTask = false;
      if (followerBrain.CurrentTaskType == FollowerTaskType.ChangeLocation)
        followerBrain.CurrentTask.Arrive();
      FollowerManager.FindFollowerByID(followerBrain.Info.ID)?.HideAllFollowerIcons();
      yield return (object) new WaitForSeconds(UnityEngine.Random.Range(0.05f, 0.15f));
    }
    yield return (object) null;
    Debug.Log((object) ("RECALCULATE! " + (object) Ritual.FollowerToAttendSermon.Count));
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
      followerBrain.CurrentTask?.RecalculateDestination();
    float timer = 0.0f;
    while (!this.FollowersInPosition() && (double) (timer += Time.deltaTime) < 10.0)
      yield return (object) null;
    SimulationManager.Pause();
  }

  public void PlaySacrificePortalEffect()
  {
    Interaction_TempleAltar.Instance.PortalEffect.gameObject.SetActive(true);
    Interaction_TempleAltar.Instance.PortalEffect.AnimationState.SetAnimation(0, "start", false);
    Interaction_TempleAltar.Instance.PortalEffect.AnimationState.AddAnimation(0, "animation", true, 0.0f);
  }

  public void StopSacrificePortalEffect()
  {
    Interaction_TempleAltar.Instance.PortalEffect.AnimationState.SetAnimation(0, "stop", false);
    Interaction_TempleAltar.Instance.PortalEffect.AnimationState.Complete += new Spine.AnimationState.TrackEntryDelegate(this.PortalEffectComplete);
  }

  private void PortalEffectComplete(TrackEntry trackEntry)
  {
    Interaction_TempleAltar.Instance.PortalEffect.AnimationState.Complete -= new Spine.AnimationState.TrackEntryDelegate(this.PortalEffectComplete);
    Interaction_TempleAltar.Instance.PortalEffect.gameObject.SetActive(false);
  }

  private bool FollowersInPosition()
  {
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain.CurrentTaskType == FollowerTaskType.ChangeLocation || followerBrain.Location != PlayerFarming.Location || followerBrain.CurrentTaskType == FollowerTaskType.AttendTeaching && followerBrain.CurrentTask.State != FollowerTaskState.Doing)
      {
        if (followerBrain.Location != PlayerFarming.Location)
        {
          followerBrain.HardSwapToTask((FollowerTask) new FollowerTask_AttendRitual());
          followerBrain.ShouldReconsiderTask = false;
          if (followerBrain.CurrentTaskType == FollowerTaskType.ChangeLocation)
            followerBrain.CurrentTask.Arrive();
        }
        return false;
      }
    }
    return true;
  }

  public IEnumerator DelayFollowerReaction(FollowerBrain brain, float Delay)
  {
    Follower f = FollowerManager.FindFollowerByID(brain.Info.ID);
    yield return (object) new WaitForSecondsRealtime(Delay);
    if ((UnityEngine.Object) f != (UnityEngine.Object) null)
    {
      f.HideAllFollowerIcons();
      f.Spine.UseDeltaTime = false;
      f.UseUnscaledTime = true;
      f.HoodOff(onComplete: (System.Action) (() => f.TimedAnimation("Reactions/react-enlightened1", 1.5f, (System.Action) (() => f.Brain.HardSwapToTask((FollowerTask) new FollowerTask_AttendTeaching())), false, false)));
    }
  }

  public IEnumerator DelayFollowerReaction(FollowerBrain brain, string anim, float Delay)
  {
    Follower f = FollowerManager.FindFollowerByID(brain.Info.ID);
    yield return (object) new WaitForSecondsRealtime(Delay);
    if ((UnityEngine.Object) f != (UnityEngine.Object) null)
    {
      f.HideAllFollowerIcons();
      f.Spine.UseDeltaTime = false;
      f.UseUnscaledTime = true;
      f.HoodOff(onComplete: (System.Action) (() => f.TimedAnimation("Reactions/react-enlightened1", 1.5f, (System.Action) (() => f.Brain.HardSwapToTask((FollowerTask) new FollowerTask_AttendTeaching())), false, false)));
    }
  }

  public void CancelFollowers()
  {
    foreach (FollowerBrain followerBrain in Ritual.GetFollowersAvailableToAttendSermon())
      followerBrain.CompleteCurrentTask();
  }
}
