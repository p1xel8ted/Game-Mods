// Decompiled with JetBrains decompiler
// Type: RitualFollowerWedding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class RitualFollowerWedding : Ritual
{
  public static Follower contestant1;
  public static Follower contestant2;
  public FollowerTask_ManualControl Task1;
  public FollowerTask_ManualControl Task2;
  public bool isDivorce;
  public bool Waiting = true;

  public override UpgradeSystem.Type RitualType
  {
    get
    {
      return this.isDivorce ? UpgradeSystem.Type.Ritual_Divorce : UpgradeSystem.Type.Ritual_FollowerWedding;
    }
  }

  public override void Play()
  {
    base.Play();
    this.StartCoroutine(this.RitualRoutine());
  }

  public IEnumerator RitualRoutine()
  {
    RitualFollowerWedding ritualFollowerWedding = this;
    AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual");
    bool canceled = false;
    ritualFollowerWedding.isDivorce = false;
    ritualFollowerWedding.Waiting = true;
    PlayerFarming.Instance.GoToAndStop(ChurchFollowerManager.Instance.RitualCenterPosition.position + Vector3.up * 0.5f, DisableCollider: true, GoToCallback: (System.Action) (() =>
    {
      this.Waiting = false;
      Interaction_TempleAltar.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      PlayerFarming.Instance.simpleSpineAnimator.Animate("idle", 0, true);
    }), maxDuration: 5f, forcePositionOnTimeout: true);
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    yield return (object) ritualFollowerWedding.StartCoroutine(ritualFollowerWedding.WaitFollowersFormCircle());
    yield return (object) new WaitForSeconds(1f);
    List<FollowerSelectEntry> followerSelectEntries = Ritual.GetFollowerSelectEntriesForSermon();
    for (int index = followerSelectEntries.Count - 1; index >= 0; --index)
    {
      if (followerSelectEntries[index].FollowerInfo.SpouseFollowerID != -1)
      {
        if (DataManager.Instance.MAJOR_DLC)
        {
          bool flag = false;
          foreach (FollowerSelectEntry followerSelectEntry in followerSelectEntries)
          {
            if (followerSelectEntry.FollowerInfo.ID == followerSelectEntries[index].FollowerInfo.SpouseFollowerID)
            {
              flag = true;
              break;
            }
          }
          if (!flag)
            followerSelectEntries[index].AvailabilityStatus = FollowerSelectEntry.Status.UnavailableAlreadyMarriedToFollower;
        }
        else
          followerSelectEntries[index].AvailabilityStatus = FollowerSelectEntry.Status.UnavailableAlreadyMarriedToFollower;
      }
    }
    UIFollowerSelectMenuController followerSelectInstance1 = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    followerSelectInstance1.VotingType = TwitchVoting.VotingType.RITUAL_FOLLOWERWEDDING;
    followerSelectInstance1.Show(followerSelectEntries, followerSelectionType: ritualFollowerWedding.RitualType);
    UIFollowerSelectMenuController selectMenuController1 = followerSelectInstance1;
    selectMenuController1.OnFollowerHighlighted = selectMenuController1.OnFollowerHighlighted + (Action<FollowerInfo>) (followerInfo =>
    {
      if (followerInfo.SpouseFollowerID != -1 && DataManager.Instance.MAJOR_DLC)
        followerSelectInstance1.ShowCustomAcceptTerm("UI/Divorce");
      else
        followerSelectInstance1.HideCustomAcceptTerm();
    });
    UIFollowerSelectMenuController selectMenuController2 = followerSelectInstance1;
    selectMenuController2.OnFollowerSelected = selectMenuController2.OnFollowerSelected + (Action<FollowerInfo>) (followerInfo =>
    {
      RitualFollowerWedding.contestant1 = FollowerManager.FindFollowerByID(followerInfo.ID);
      AudioManager.Instance.PlayOneShot("event:/rituals/wedding_select_follower", PlayerFarming.Instance.gameObject);
      this.loopedSound = AudioManager.Instance.CreateLoop("event:/sermon/preach_loop", PlayerFarming.Instance.gameObject, true, false);
      this.Task1 = new FollowerTask_ManualControl();
      RitualFollowerWedding.contestant1.Brain.HardSwapToTask((FollowerTask) this.Task1);
      if (followerInfo.SpouseFollowerID != -1)
      {
        this.isDivorce = true;
        this.Waiting = false;
        RitualFollowerWedding.contestant2 = FollowerManager.FindFollowerByID(followerInfo.SpouseFollowerID);
        this.Task2 = new FollowerTask_ManualControl();
        RitualFollowerWedding.contestant2.Brain.HardSwapToTask((FollowerTask) this.Task2);
      }
      else
        this.StartCoroutine(this.SetUpCombatant1Routine());
    });
    UIFollowerSelectMenuController selectMenuController3 = followerSelectInstance1;
    selectMenuController3.OnCancel = selectMenuController3.OnCancel + (System.Action) (() =>
    {
      AudioManager.Instance.StopLoop(this.loopedSound);
      GameManager.GetInstance().StartCoroutine(this.RitualFinished(true, -1, -1));
      this.CancelFollowers();
      canceled = true;
    });
    UIFollowerSelectMenuController selectMenuController4 = followerSelectInstance1;
    selectMenuController4.OnHidden = selectMenuController4.OnHidden + (System.Action) (() => followerSelectInstance1 = (UIFollowerSelectMenuController) null);
    while ((UnityEngine.Object) followerSelectInstance1 != (UnityEngine.Object) null || ritualFollowerWedding.Waiting)
      yield return (object) null;
    if (!canceled)
    {
      if (ritualFollowerWedding.isDivorce)
      {
        yield return (object) ritualFollowerWedding.StartCoroutine(ritualFollowerWedding.ContinueRitualDivorce());
      }
      else
      {
        ritualFollowerWedding.Waiting = true;
        for (int index = followerSelectEntries.Count - 1; index >= 0; --index)
        {
          if (followerSelectEntries[index].FollowerInfo == RitualFollowerWedding.contestant1.Brain._directInfoAccess)
            followerSelectEntries.RemoveAt(index);
          else if (FollowerManager.AreRelated(RitualFollowerWedding.contestant1.Brain._directInfoAccess.ID, followerSelectEntries[index].FollowerInfo.ID))
            followerSelectEntries[index].AvailabilityStatus = FollowerSelectEntry.Status.Unavailable;
        }
        UIFollowerSelectMenuController followerSelectInstance2 = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
        followerSelectInstance2.VotingType = TwitchVoting.VotingType.RITUAL_FOLLOWERWEDDING;
        followerSelectInstance2.Show(followerSelectEntries, followerSelectionType: ritualFollowerWedding.RitualType);
        UIFollowerSelectMenuController selectMenuController5 = followerSelectInstance2;
        selectMenuController5.OnShownCompleted = selectMenuController5.OnShownCompleted + (System.Action) (() => this.SetFollowerRelationships(followerSelectInstance2, RitualFollowerWedding.contestant1.Brain));
        UIFollowerSelectMenuController selectMenuController6 = followerSelectInstance2;
        selectMenuController6.OnFollowerSelected = selectMenuController6.OnFollowerSelected + (Action<FollowerInfo>) (followerInfo =>
        {
          RitualFollowerWedding.contestant2 = FollowerManager.FindFollowerByID(followerInfo.ID);
          AudioManager.Instance.PlayOneShot("event:/rituals/wedding_select_follower", PlayerFarming.Instance.gameObject);
          this.loopedSound = AudioManager.Instance.CreateLoop("event:/sermon/preach_loop", PlayerFarming.Instance.gameObject, true, false);
          this.Task2 = new FollowerTask_ManualControl();
          RitualFollowerWedding.contestant2.Brain.HardSwapToTask((FollowerTask) this.Task2);
          this.StartCoroutine(this.SetUpCombatant2Routine());
        });
        UIFollowerSelectMenuController selectMenuController7 = followerSelectInstance2;
        selectMenuController7.OnCancel = selectMenuController7.OnCancel + (System.Action) (() =>
        {
          GameManager.GetInstance().StartCoroutine(this.RitualFinished(true, -1, -1));
          this.CancelFollowers();
          canceled = true;
        });
        UIFollowerSelectMenuController selectMenuController8 = followerSelectInstance2;
        selectMenuController8.OnHidden = selectMenuController8.OnHidden + (System.Action) (() => followerSelectInstance2 = (UIFollowerSelectMenuController) null);
        while ((UnityEngine.Object) followerSelectInstance2 != (UnityEngine.Object) null || ritualFollowerWedding.Waiting)
          yield return (object) null;
        if (!canceled)
        {
          Interaction_TempleAltar.Instance.CloseUpCamera.Play();
          yield return (object) new WaitForSeconds(1f);
          PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "rituals/sin-onboarding-start", false);
          PlayerFarming.Instance.Spine.AnimationState.AddAnimation(0, "rituals/sin-onboarding-loop", true, 0.0f);
          if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
          {
            if (!PlayerFarming.Instance.isLamb || PlayerFarming.Instance.IsGoat)
              AudioManager.Instance.PlayOneShot("event:/player/goat_player/goat_speak_to_follower_noBookPage", ritualFollowerWedding.gameObject);
            else
              AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower_noBookPage", ritualFollowerWedding.gameObject);
          }
          double num1 = (double) RitualFollowerWedding.contestant1.SetBodyAnimation("Reactions/react-bow", false);
          double num2 = (double) RitualFollowerWedding.contestant2.SetBodyAnimation("Reactions/react-bow", false);
          yield return (object) new WaitForSeconds(1.93333328f);
          int WaitCount = 0;
          ritualFollowerWedding.Task1.GoToAndStop(RitualFollowerWedding.contestant1, Interaction_TempleAltar.Instance.PortalEffect.transform.position + Vector3.right * 0.6f, (System.Action) (() => ++WaitCount));
          ritualFollowerWedding.Task2.GoToAndStop(RitualFollowerWedding.contestant2, Interaction_TempleAltar.Instance.PortalEffect.transform.position + Vector3.left * 0.6f, (System.Action) (() => ++WaitCount));
          while (WaitCount < 2)
            yield return (object) null;
          double num3 = (double) RitualFollowerWedding.contestant1.SetBodyAnimation("kiss", true);
          double num4 = (double) RitualFollowerWedding.contestant2.SetBodyAnimation("kiss", true);
          RitualFollowerWedding.contestant1.Brain._directInfoAccess.SpouseFollowerID = RitualFollowerWedding.contestant2.Brain.Info.ID;
          RitualFollowerWedding.contestant2.Brain._directInfoAccess.SpouseFollowerID = RitualFollowerWedding.contestant1.Brain.Info.ID;
          yield return (object) new WaitForSeconds(0.8333333f);
          BiomeConstants.Instance.EmitHeartPickUpVFX(ChurchFollowerManager.Instance.RitualCenterPosition.position, 0.0f, "red", "burst_big");
          BiomeConstants.Instance.EmitConfettiVFX(ChurchFollowerManager.Instance.RitualCenterPosition.position);
          CameraManager.instance.ShakeCameraForDuration(0.1f, 1f, 0.5f);
          MMVibrate.Haptic(MMVibrate.HapticTypes.SoftImpact);
          AudioManager.Instance.PlayOneShot("event:/building/building_bell_ring", PlayerFarming.Instance.transform.position);
          Interaction_TempleAltar.Instance.PulseDisplacementObject(Interaction_TempleAltar.Instance.PortalEffect.transform.position - Vector3.back);
          foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
          {
            if (followerBrain.CurrentTaskType == FollowerTaskType.AttendTeaching)
              (followerBrain.CurrentTask as FollowerTask_AttendRitual).Cheer();
          }
          yield return (object) new WaitForSeconds(2f);
          IDAndRelationship relationship = RitualFollowerWedding.contestant1.Brain.Info.GetOrCreateRelationship(RitualFollowerWedding.contestant2.Brain.Info.ID);
          IDAndRelationship relationship1 = RitualFollowerWedding.contestant2.Brain.Info.GetOrCreateRelationship(RitualFollowerWedding.contestant1.Brain.Info.ID);
          if (relationship.CurrentRelationshipState != IDAndRelationship.RelationshipState.Enemies)
          {
            relationship.Relationship = 11;
            relationship1.Relationship = 11;
            relationship.CurrentRelationshipState = IDAndRelationship.RelationshipState.Lovers;
            relationship1.CurrentRelationshipState = IDAndRelationship.RelationshipState.Lovers;
          }
          PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "reactions/react-happy", false);
          PlayerFarming.Instance.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
          if (RitualFollowerWedding.contestant1.Brain.HasThought(Thought.Dissenter) || relationship.CurrentRelationshipState == IDAndRelationship.RelationshipState.Enemies || RitualFollowerWedding.contestant1.Brain.HasTrait(FollowerTrait.TraitType.Spy))
          {
            double num5 = (double) RitualFollowerWedding.contestant1.SetBodyAnimation("tantrum", false);
            RitualFollowerWedding.contestant1.AddBodyAnimation("idle", true, 0.0f);
          }
          else
          {
            double num6 = (double) RitualFollowerWedding.contestant1.SetBodyAnimation("Egg/mating-pose", false);
            RitualFollowerWedding.contestant1.AddBodyAnimation("idle", true, 0.0f);
          }
          if (RitualFollowerWedding.contestant2.Brain.HasThought(Thought.Dissenter) || relationship.CurrentRelationshipState == IDAndRelationship.RelationshipState.Enemies || RitualFollowerWedding.contestant2.Brain.HasTrait(FollowerTrait.TraitType.Spy))
          {
            double num7 = (double) RitualFollowerWedding.contestant2.SetBodyAnimation("tantrum", false);
            RitualFollowerWedding.contestant2.AddBodyAnimation("idle", true, 0.0f);
          }
          else
          {
            double num8 = (double) RitualFollowerWedding.contestant2.SetBodyAnimation("Egg/mating-pose3", false);
            RitualFollowerWedding.contestant2.AddBodyAnimation("idle", true, 0.0f);
          }
          yield return (object) new WaitForSeconds(3f);
          if (RitualFollowerWedding.contestant1.Brain.HasThought(Thought.Dissenter) || RitualFollowerWedding.contestant1.Brain.HasTrait(FollowerTrait.TraitType.Spy))
          {
            RitualFollowerWedding.contestant1.Brain.AddThought(Thought.FollowerWedding_3);
            RitualFollowerWedding.contestant1.Brain.AddTrait(FollowerTrait.TraitType.MarriedUnhappily);
            ritualFollowerWedding.AddDistressedThought(RitualFollowerWedding.contestant1.Brain);
          }
          else if (RitualFollowerWedding.contestant2.Brain.HasThought(Thought.Dissenter) || RitualFollowerWedding.contestant2.Brain.HasTrait(FollowerTrait.TraitType.Spy))
          {
            RitualFollowerWedding.contestant2.Brain.AddThought(Thought.FollowerWedding_3);
            RitualFollowerWedding.contestant2.Brain.AddTrait(FollowerTrait.TraitType.MarriedUnhappily);
            ritualFollowerWedding.AddDistressedThought(RitualFollowerWedding.contestant2.Brain);
          }
          else if (RitualFollowerWedding.contestant1.Brain.HasTrait(FollowerTrait.TraitType.MarriedJealous) || RitualFollowerWedding.contestant1.Brain.HasTrait(FollowerTrait.TraitType.MarriedMurderouslyJealous) && RitualFollowerWedding.contestant2.Brain.Info.MarriedToLeader)
          {
            RitualFollowerWedding.contestant1.Brain.AddThought(Thought.FollowerWedding_3);
            RitualFollowerWedding.contestant1.Brain.AddTrait(FollowerTrait.TraitType.MarriedUnhappily);
            ritualFollowerWedding.AddDistressedThought(RitualFollowerWedding.contestant1.Brain);
          }
          else if (RitualFollowerWedding.contestant2.Brain.HasTrait(FollowerTrait.TraitType.MarriedJealous) || RitualFollowerWedding.contestant2.Brain.HasTrait(FollowerTrait.TraitType.MarriedMurderouslyJealous) && RitualFollowerWedding.contestant1.Brain.Info.MarriedToLeader)
          {
            RitualFollowerWedding.contestant2.Brain.AddThought(Thought.FollowerWedding_3);
            ritualFollowerWedding.AddDistressedThought(RitualFollowerWedding.contestant2.Brain);
            RitualFollowerWedding.contestant2.Brain.AddTrait(FollowerTrait.TraitType.MarriedUnhappily);
          }
          else if (RitualFollowerWedding.contestant1.Brain.HasTrait(FollowerTrait.TraitType.Polyamory) && RitualFollowerWedding.contestant1.Brain.Info.MarriedToLeader || RitualFollowerWedding.contestant2.Brain.Info.MarriedToLeader)
          {
            ritualFollowerWedding.AddInspiredThought(RitualFollowerWedding.contestant1.Brain);
            RitualFollowerWedding.contestant1.Brain.AddThought(Thought.FollowerWedding_1);
            RitualFollowerWedding.contestant1.Brain.AddTrait(FollowerTrait.TraitType.MarriedHappily);
          }
          else if (RitualFollowerWedding.contestant2.Brain.HasTrait(FollowerTrait.TraitType.Polyamory) && RitualFollowerWedding.contestant2.Brain.Info.MarriedToLeader || RitualFollowerWedding.contestant1.Brain.Info.MarriedToLeader)
          {
            ritualFollowerWedding.AddInspiredThought(RitualFollowerWedding.contestant2.Brain);
            RitualFollowerWedding.contestant2.Brain.AddThought(Thought.FollowerWedding_1);
            RitualFollowerWedding.contestant2.Brain.AddTrait(FollowerTrait.TraitType.MarriedHappily);
          }
          RitualFollowerWedding.contestant1.Brain.RemoveTrait(FollowerTrait.TraitType.GrievingWidow);
          RitualFollowerWedding.contestant2.Brain.RemoveTrait(FollowerTrait.TraitType.GrievingWidow);
          RitualFollowerWedding.contestant1.Brain.RemoveTrait(FollowerTrait.TraitType.HappilyWidowed);
          RitualFollowerWedding.contestant2.Brain.RemoveTrait(FollowerTrait.TraitType.HappilyWidowed);
          foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
          {
            if (followerBrain == RitualFollowerWedding.contestant1.Brain || followerBrain == RitualFollowerWedding.contestant2.Brain)
            {
              if ((followerBrain.HasTrait(FollowerTrait.TraitType.MarriedHappily) ? 1 : (followerBrain.HasTrait(FollowerTrait.TraitType.MarriedUnhappily) ? 1 : 0)) == 0)
              {
                if (followerBrain.HasTrait(FollowerTrait.TraitType.MarriedJealous) || followerBrain.HasTrait(FollowerTrait.TraitType.MarriedMurderouslyJealous) && followerBrain.Info.MarriedToLeader)
                {
                  followerBrain.AddThought(Thought.FollowerWedding_3);
                  followerBrain.AddTrait(FollowerTrait.TraitType.MarriedUnhappily, true);
                  ritualFollowerWedding.AddDistressedThought(followerBrain);
                }
                else if (followerBrain.HasTrait(FollowerTrait.TraitType.Polyamory) && followerBrain.Info.MarriedToLeader)
                {
                  followerBrain.AddThought(Thought.FollowerWedding_1);
                  followerBrain.AddTrait(FollowerTrait.TraitType.MarriedHappily);
                  ritualFollowerWedding.AddInspiredThought(followerBrain);
                }
                else if (relationship.CurrentRelationshipState == IDAndRelationship.RelationshipState.Lovers)
                {
                  followerBrain.AddThought(Thought.FollowerWedding_1);
                  followerBrain.AddTrait(FollowerTrait.TraitType.MarriedHappily);
                }
                else if (relationship.CurrentRelationshipState == IDAndRelationship.RelationshipState.Friends)
                {
                  followerBrain.AddThought(Thought.FollowerWedding_2);
                  followerBrain.AddTrait(FollowerTrait.TraitType.MarriedHappily);
                }
                else if (relationship.CurrentRelationshipState == IDAndRelationship.RelationshipState.Enemies)
                {
                  followerBrain.AddThought(Thought.FollowerWedding_3);
                  followerBrain.AddTrait(FollowerTrait.TraitType.MarriedUnhappily);
                }
              }
            }
            else
              followerBrain.AddThought(Thought.FollowerWeddingRitual);
            if (followerBrain.CurrentTask is FollowerTask_AttendRitual)
              (followerBrain.CurrentTask as FollowerTask_AttendRitual).Cheer();
          }
          yield return (object) new WaitForSeconds(0.5f);
          ChurchFollowerManager.Instance.AddBrainToAudience(RitualFollowerWedding.contestant1.Brain);
          ChurchFollowerManager.Instance.AddBrainToAudience(RitualFollowerWedding.contestant2.Brain);
          GameManager.GetInstance().StartCoroutine(ritualFollowerWedding.EndRitual(RitualFollowerWedding.contestant1.Brain.Info.ID, RitualFollowerWedding.contestant2.Brain.Info.ID));
        }
      }
    }
  }

  public void AddInspiredThought(FollowerBrain followerBrain)
  {
    Thought randomThoughtFromSet = FollowerThoughts.GetRandomThoughtFromSet(Thought.Inspired_0, Thought.Inspired_1, Thought.Inspired_2, Thought.Inspired_3, Thought.Inspired_4);
    if (followerBrain.HasThought(randomThoughtFromSet))
      return;
    followerBrain.AddThought(randomThoughtFromSet);
  }

  public void AddDistressedThought(FollowerBrain followerBrain)
  {
    Thought randomThoughtFromSet = FollowerThoughts.GetRandomThoughtFromSet(Thought.Distressed_0, Thought.Distressed_1, Thought.Distressed_2, Thought.Distressed_3);
    if (followerBrain.HasThought(randomThoughtFromSet))
      return;
    followerBrain.AddThought(randomThoughtFromSet);
  }

  public void SetFollowerRelationships(
    UIFollowerSelectMenuController followerSelectInstance,
    FollowerBrain follower)
  {
    foreach (FollowerInformationBox followerInfoBox in followerSelectInstance.FollowerInfoBoxes)
    {
      IDAndRelationship relationship = RitualFollowerWedding.contestant1.Brain.Info.GetOrCreateRelationship(followerInfoBox.FollowerInfo.ID);
      string str = ScriptLocalization.UI.Strangers;
      if (relationship.CurrentRelationshipState == IDAndRelationship.RelationshipState.Friends)
        str = ScriptLocalization.UI.Friends;
      else if (relationship.CurrentRelationshipState == IDAndRelationship.RelationshipState.Enemies)
        str = ScriptLocalization.UI.Enemies;
      else if (relationship.CurrentRelationshipState == IDAndRelationship.RelationshipState.Lovers)
        str = ScriptLocalization.UI.Lovers;
      followerInfoBox.AppendFollowerRole($"{str} {string.Format(ScriptLocalization.UI.RelationshipDescription, (object) follower.Info.Name)}");
    }
  }

  public IEnumerator SetUpCombatant1Routine()
  {
    RitualFollowerWedding ritualFollowerWedding = this;
    yield return (object) null;
    ChurchFollowerManager.Instance.RemoveBrainFromAudience(RitualFollowerWedding.contestant1.Brain);
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.CurrentTaskType == FollowerTaskType.AttendTeaching)
      {
        allBrain.CurrentTask.RecalculateDestination();
        allBrain.CurrentTask.Setup(FollowerManager.FindFollowerByID(allBrain.Info.ID));
      }
    }
    ritualFollowerWedding.Task1.GoToAndStop(RitualFollowerWedding.contestant1, Interaction_TempleAltar.Instance.PortalEffect.transform.position + Vector3.right * 1f, new System.Action(ritualFollowerWedding.\u003CSetUpCombatant1Routine\u003Eb__13_0));
  }

  public IEnumerator SetUpCombatant2Routine()
  {
    RitualFollowerWedding ritualFollowerWedding = this;
    yield return (object) null;
    ChurchFollowerManager.Instance.RemoveBrainFromAudience(RitualFollowerWedding.contestant2.Brain);
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.CurrentTaskType == FollowerTaskType.AttendTeaching)
      {
        allBrain.CurrentTask.RecalculateDestination();
        allBrain.CurrentTask.Setup(FollowerManager.FindFollowerByID(allBrain.Info.ID));
      }
    }
    ritualFollowerWedding.Task2.GoToAndStop(RitualFollowerWedding.contestant2, Interaction_TempleAltar.Instance.PortalEffect.transform.position + Vector3.left * 1f, new System.Action(ritualFollowerWedding.\u003CSetUpCombatant2Routine\u003Eb__14_0));
  }

  public IEnumerator ContinueRitualDivorce()
  {
    RitualFollowerWedding ritualFollowerWedding = this;
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    Interaction_TempleAltar.Instance.CloseUpCamera.Play();
    ritualFollowerWedding.Waiting = true;
    yield return (object) ritualFollowerWedding.StartCoroutine(ritualFollowerWedding.SetUpCombatant1Routine());
    yield return (object) ritualFollowerWedding.StartCoroutine(ritualFollowerWedding.SetUpCombatant2Routine());
    while (ritualFollowerWedding.Waiting)
      yield return (object) null;
    PlayerFarming.Instance.simpleSpineAnimator.Animate("bleat", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("reactions/react-happy", 0, false, 0.0f);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
    {
      if (!PlayerFarming.Instance.isLamb || PlayerFarming.Instance.IsGoat)
        AudioManager.Instance.PlayOneShot("event:/player/goat_player/goat_speak_to_follower_noBookPage", ritualFollowerWedding.gameObject);
      else
        AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower_noBookPage", ritualFollowerWedding.gameObject);
    }
    GameObject ring = ChurchFollowerManager.Instance.Ring.gameObject;
    ring.SetActive(true);
    yield return (object) new WaitForSeconds(2.36666656f);
    ring.SetActive(false);
    Vector3 position = Interaction_TempleAltar.Instance.PortalEffect.transform.position;
    CameraManager.instance.ShakeCameraForDuration(0.1f, 1f, 0.5f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.SoftImpact);
    AudioManager.Instance.PlayOneShot("event:/building/building_bell_ring", PlayerFarming.Instance.transform.position);
    Interaction_TempleAltar.Instance.PulseDisplacementObject(Interaction_TempleAltar.Instance.PortalEffect.transform.position - Vector3.back);
    ritualFollowerWedding.SetDivocrceBehavior(RitualFollowerWedding.contestant1);
    ritualFollowerWedding.SetDivocrceBehavior(RitualFollowerWedding.contestant2);
    yield return (object) new WaitForSeconds(4f);
    ChurchFollowerManager.Instance.AddBrainToAudience(RitualFollowerWedding.contestant1.Brain);
    ChurchFollowerManager.Instance.AddBrainToAudience(RitualFollowerWedding.contestant2.Brain);
    RitualFollowerWedding.contestant1.Brain._directInfoAccess.SpouseFollowerID = -1;
    RitualFollowerWedding.contestant2.Brain._directInfoAccess.SpouseFollowerID = -1;
    RitualFollowerWedding.contestant1.Brain.ClearMarriageTraits(true);
    RitualFollowerWedding.contestant2.Brain.ClearMarriageTraits(true);
    AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.Temple);
    float num = 0.0f;
    foreach (FollowerBrain brain in Ritual.FollowerToAttendSermon)
    {
      float Delay = UnityEngine.Random.Range(0.1f, 0.5f);
      num += Delay;
      ritualFollowerWedding.StartCoroutine(ritualFollowerWedding.DelayFollowerReaction(brain, Delay));
    }
    AudioManager.Instance.StopLoop(ritualFollowerWedding.loopedSound);
    Interaction_TempleAltar.Instance.CloseUpCamera.Reset();
    JudgementMeter.ShowModify(1);
    yield return (object) new WaitForSeconds(0.5f);
    ritualFollowerWedding.CompleteRitual(targetFollowerID_1: RitualFollowerWedding.contestant1.Brain.Info.ID, targetFollowerID_2: RitualFollowerWedding.contestant2.Brain.Info.ID);
    yield return (object) new WaitForSeconds(1f);
    CultFaithManager.AddThought(Thought.Cult_DivorceRitual, RitualFollowerWedding.contestant1.Brain.Info.ID);
    CultFaithManager.AddThought(Thought.Cult_DivorceRitual, RitualFollowerWedding.contestant2.Brain.Info.ID);
    ritualFollowerWedding.Waiting = false;
  }

  public void SetDivocrceBehavior(Follower contestant)
  {
    if (contestant.Brain.HasTrait(FollowerTrait.TraitType.MarriedUnhappily))
    {
      double num1 = (double) contestant.SetBodyAnimation("Reactions/react-happy1", false);
    }
    else if (contestant.Brain.HasTrait(FollowerTrait.TraitType.MarriedHappily) || contestant.Brain.HasTrait(FollowerTrait.TraitType.MarriedDevoted))
    {
      double num2 = (double) contestant.SetBodyAnimation("Reactions/react-cry", false);
    }
    else if (contestant.Brain.HasTrait(FollowerTrait.TraitType.MarriedJealous) || contestant.Brain.HasTrait(FollowerTrait.TraitType.MarriedMurderouslyJealous))
    {
      double num3 = (double) contestant.SetBodyAnimation("Reactions/react-non-believers", false);
    }
    else
    {
      double num4 = (double) contestant.SetBodyAnimation("Conversations/react-mean" + UnityEngine.Random.Range(1, 4).ToString(), false);
    }
    if ((double) UnityEngine.Random.value < 0.2)
    {
      contestant.Brain.AddTrait(FollowerTrait.TraitType.JiltedLover);
      contestant.Brain.MakeDissenter();
    }
    contestant.AddBodyAnimation("idle", true, 0.0f);
  }

  public IEnumerator EndRitual(int follower1, int follower2)
  {
    RitualFollowerWedding ritualFollowerWedding = this;
    JudgementMeter.ShowModify(-1);
    AudioManager.Instance.StopLoop(ritualFollowerWedding.loopedSound);
    float EndingDelay = 0.0f;
    yield return (object) null;
    foreach (FollowerBrain brain in Ritual.GetFollowersAvailableToAttendSermon())
    {
      float Delay = UnityEngine.Random.Range(0.1f, 0.5f);
      EndingDelay += Delay;
      ritualFollowerWedding.StartCoroutine(ritualFollowerWedding.DelayFollowerReaction(brain, Delay));
    }
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().StartCoroutine(ritualFollowerWedding.RitualFinished(false, follower1, follower2));
  }

  public IEnumerator RitualFinished(bool cancelled, int follower1, int follower2)
  {
    RitualFollowerWedding ritualFollowerWedding = this;
    AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.Temple);
    AudioManager.Instance.StopLoop(ritualFollowerWedding.loopedSound);
    ritualFollowerWedding.CompleteRitual(cancelled, follower1, follower2);
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    yield return (object) new WaitForSeconds(1f);
    if (!cancelled && !ritualFollowerWedding.isDivorce)
      CultFaithManager.AddThought(Thought.Cult_FollowerWeddingRitual);
  }

  [CompilerGenerated]
  public void \u003CSetUpCombatant1Routine\u003Eb__13_0()
  {
    RitualFollowerWedding.contestant1.FacePosition(Interaction_TempleAltar.Instance.PortalEffect.transform.position);
    double num = (double) RitualFollowerWedding.contestant1.SetBodyAnimation("idle", true);
    this.Waiting = false;
  }

  [CompilerGenerated]
  public void \u003CSetUpCombatant2Routine\u003Eb__14_0()
  {
    RitualFollowerWedding.contestant2.FacePosition(Interaction_TempleAltar.Instance.PortalEffect.transform.position);
    double num = (double) RitualFollowerWedding.contestant2.SetBodyAnimation("idle", true);
    this.Waiting = false;
  }
}
