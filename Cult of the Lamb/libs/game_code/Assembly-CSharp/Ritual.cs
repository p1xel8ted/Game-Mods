// Decompiled with JetBrains decompiler
// Type: Ritual
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using Lamb.UI.FollowerSelect;
using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Ritual : BaseMonoBehaviour
{
  public static Action<bool> OnEnd;
  public static Action<bool> OnBegin;
  public static List<FollowerBrain> FollowerToAttendSermon = new List<FollowerBrain>();
  [CompilerGenerated]
  public UpgradeSystem.Type \u003CRitualType\u003Ek__BackingField;
  public EventInstance loopedSound;
  public static List<UpgradeSystem.Type> MAJOR_DLC_RITUALS = new List<UpgradeSystem.Type>()
  {
    UpgradeSystem.Type.Ritual_FollowerWedding,
    UpgradeSystem.Type.Ritual_ConvertToRot,
    UpgradeSystem.Type.Ritual_Snowman,
    UpgradeSystem.Type.Ritual_RanchHarvest,
    UpgradeSystem.Type.Ritual_RanchMeat
  };

  public virtual UpgradeSystem.Type RitualType => this.\u003CRitualType\u003Ek__BackingField;

  public void OnDisable() => AudioManager.Instance.StopLoop(this.loopedSound);

  public static int FollowersAvailableToAttendSermon()
  {
    return Ritual.GetFollowersAvailableToAttendSermon().Count;
  }

  public static List<FollowerBrain> GetFollowersAvailableToAttendSermon(bool ignoreDissenters = false)
  {
    List<FollowerBrain> source = new List<FollowerBrain>();
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if ((!ignoreDissenters || allBrain.Info.CursedState != Thought.Dissenter) && (allBrain.CurrentTask == null || !allBrain.CurrentTask.BlockSermon) && !FollowerManager.FollowerLocked(allBrain.Info.ID, true, excludeFreezing: true) && (double) allBrain.Info.Pleasure < 65.0)
        source.Add(allBrain);
    }
    return source.OrderByDescending<FollowerBrain, bool>((Func<FollowerBrain, bool>) (x => x.Info.IsDisciple)).ToList<FollowerBrain>();
  }

  public static List<FollowerSelectEntry> GetFollowerSelectEntriesForSermon(
    int minFollowerLevel = 0,
    bool excludeDisciples = false)
  {
    List<FollowerSelectEntry> entriesForSermon = new List<FollowerSelectEntry>();
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain.Info.XPLevel >= minFollowerLevel)
        entriesForSermon.Add(new FollowerSelectEntry(followerBrain, FollowerManager.GetFollowerAvailabilityStatus(followerBrain, minFollowerLevel, true, excludeDisciples)));
    }
    return entriesForSermon;
  }

  public virtual void Play()
  {
    foreach (FollowerBrain followerBrain in Ritual.GetFollowersAvailableToAttendSermon())
    {
      if (followerBrain.CurrentTask != null && (followerBrain.CurrentTask is FollowerTask_AttendRitual || followerBrain.CurrentTask is FollowerTask_AttendTeaching))
        followerBrain.CurrentTask.Abort();
    }
    Action<bool> onBegin = Ritual.OnBegin;
    if (onBegin != null)
      onBegin(false);
    Interaction_TempleAltar.Instance.FrontWall.SetActive(false);
  }

  public void CompleteRitual(
    bool cancelled = false,
    int targetFollowerID_1 = -1,
    int targetFollowerID_2 = -1,
    bool PlayFakeBar = true,
    bool ignoreNightPenalty = false)
  {
    if (!cancelled & PlayFakeBar)
    {
      if (!FollowerBrainStats.BrainWashed)
        FaithBarFake.Play(UpgradeSystem.GetRitualFaithChange(this.RitualType));
      if ((double) UpgradeSystem.GetRitualWarmthChange(this.RitualType) != 0.0 && SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter)
        WarmthBarFake.Play(UpgradeSystem.GetRitualWarmthChange(this.RitualType), FollowerBrainStats.LockedWarmth);
    }
    Interaction_TempleAltar.Instance.FrontWall.SetActive(true);
    Action<bool> onEnd = Ritual.OnEnd;
    if (onEnd != null)
      onEnd(cancelled);
    if (!cancelled)
    {
      ObjectiveManager.CompleteRitualObjective(this.RitualType, targetFollowerID_1, targetFollowerID_2);
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.PerformAnyRitual);
      DataManager.Instance.HasPerformedRitual = true;
      DataManager.Instance.NextRitualFree = false;
    }
    if (DataManager.Instance.WokeUpEveryoneDay == TimeManager.CurrentDay && TimeManager.CurrentPhase == DayPhase.Night && !ignoreNightPenalty && (!DataManager.Instance.LongNightActive || this.RitualType != UpgradeSystem.Type.Ritual_Midwinter) && FollowerBrainStats.ShouldSleep)
      CultFaithManager.AddThought(Thought.Cult_WokeUpEveryone);
    Interaction_TempleAltar.Instance.OnInteract(PlayerFarming.Instance.state);
    ChurchFollowerManager.Instance.DisableAllOverlays();
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

  public IEnumerator WaitFollowersFormCircle(bool ignoreDissenters = false, bool putOnHoods = true, float zoom = 12f)
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
    GameManager.GetInstance().OnConversationNext(Interaction_TempleAltar.Instance.RitualCameraPosition, zoom);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) ritual.StartCoroutine((IEnumerator) ritual.FollowersEnterForRitualRoutine(ignoreDissenters, putOnHoods));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator FollowersEnterForRitualRoutine(bool ignoreDissenters = false, bool putOnHoods = true)
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
        followerById.Spine.UseDeltaTime = true;
        followerById.UseUnscaledTime = false;
        if (followerById.State.CURRENT_STATE == StateMachine.State.TimedAction)
          followerById.State.CURRENT_STATE = StateMachine.State.Idle;
      }
      if (getFollowers)
        Ritual.FollowerToAttendSermon.Add(followerBrain);
      if (followerBrain.CurrentTask != null)
        followerBrain.CurrentTask.Abort();
      followerBrain.HardSwapToTask((FollowerTask) new FollowerTask_AttendRitual(putOnHoods));
      followerBrain.ShouldReconsiderTask = false;
      if (followerBrain.CurrentTaskType == FollowerTaskType.ChangeLocation)
        followerBrain.CurrentTask.Arrive();
      FollowerManager.FindFollowerByID(followerBrain.Info.ID)?.HideAllFollowerIcons();
      yield return (object) new WaitForSeconds(UnityEngine.Random.Range(0.05f, 0.15f));
    }
    yield return (object) null;
    Debug.Log((object) ("RECALCULATE! " + Ritual.FollowerToAttendSermon.Count.ToString()));
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

  public void PortalEffectComplete(TrackEntry trackEntry)
  {
    Interaction_TempleAltar.Instance.PortalEffect.AnimationState.Complete -= new Spine.AnimationState.TrackEntryDelegate(this.PortalEffectComplete);
    Interaction_TempleAltar.Instance.PortalEffect.gameObject.SetActive(false);
  }

  public bool FollowersInPosition()
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

  public IEnumerator DelayFollowerReaction(FollowerBrain brain, float Delay, bool hoodOff = true)
  {
    yield return (object) this.DelayFollowerReaction(brain, "Reactions/react-enlightened1", Delay, hoodOff);
  }

  public IEnumerator DelayFollowerReaction(
    FollowerBrain brain,
    string anim,
    float Delay,
    bool hoodOff = true,
    float animTime = 1.5f)
  {
    Follower f = FollowerManager.FindFollowerByID(brain.Info.ID);
    yield return (object) new WaitForSecondsRealtime(Delay);
    if ((UnityEngine.Object) f != (UnityEngine.Object) null)
    {
      f.HideAllFollowerIcons();
      f.HoodOff(snap: !hoodOff, onComplete: (System.Action) (() =>
      {
        if ((double) f.Brain.Info.Pleasure >= 65.0)
          return;
        f.UseUnscaledTime = true;
        f.TimedAnimation(anim, animTime, (System.Action) (() => f.Brain.HardSwapToTask((FollowerTask) new FollowerTask_AttendTeaching())), false, false);
      }));
    }
  }

  public void CancelFollowers()
  {
    foreach (FollowerBrain followerBrain in Ritual.GetFollowersAvailableToAttendSermon())
      followerBrain.CompleteCurrentTask();
  }
}
