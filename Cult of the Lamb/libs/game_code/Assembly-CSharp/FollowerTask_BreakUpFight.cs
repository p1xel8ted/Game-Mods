// Decompiled with JetBrains decompiler
// Type: FollowerTask_BreakUpFight
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using UnityEngine;

#nullable disable
public class FollowerTask_BreakUpFight : FollowerTask
{
  public int fightingFollowerID0 = -1;
  public int fightingFollowerID1 = -1;

  public override FollowerTaskType Type => FollowerTaskType.BreakUpFight;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockTaskChanges => true;

  public override bool BlockReactTasks => true;

  public override bool BlockSocial => true;

  public override bool BlockThoughts => true;

  public FollowerTask_BreakUpFight(int fightingFollowerID0, int fightingFollowerID1)
  {
    this.fightingFollowerID0 = fightingFollowerID0;
    this.fightingFollowerID1 = fightingFollowerID1;
  }

  public override int GetSubTaskCode() => 0;

  public override void OnStart()
  {
    FollowerBrain brainById1 = FollowerBrain.FindBrainByID(this.fightingFollowerID0);
    FollowerBrain brainById2 = FollowerBrain.FindBrainByID(this.fightingFollowerID1);
    if (brainById1 == null || brainById1.CurrentTaskType != FollowerTaskType.FightFollower || brainById2 == null || brainById2.CurrentTaskType != FollowerTaskType.FightFollower)
      return;
    FollowerTask_FightFollower currentTask = (FollowerTask_FightFollower) brainById1.CurrentTask;
    if (currentTask != null)
    {
      if (currentTask.OtherChatTask != null && currentTask.OtherChatTask.Brain == brainById2)
        this.SetState(FollowerTaskState.GoingTo);
      else
        this.End();
    }
    else
      this.End();
  }

  public override void OnArrive()
  {
    if (LocationManager.GetLocationState(this.Location) == LocationState.Active)
    {
      base.OnArrive();
    }
    else
    {
      FollowerBrain brainById1 = FollowerBrain.FindBrainByID(this.fightingFollowerID0);
      FollowerBrain brainById2 = FollowerBrain.FindBrainByID(this.fightingFollowerID1);
      if (brainById1.CurrentTask is FollowerTask_FightFollower)
      {
        ((FollowerTask_FightFollower) brainById1.CurrentTask).Interrupt();
        brainById1.CurrentTask.Abort();
      }
      if (brainById2.CurrentTask is FollowerTask_FightFollower)
      {
        ((FollowerTask_FightFollower) brainById2.CurrentTask).Interrupt();
        brainById2.CurrentTask.Abort();
      }
      CultFaithManager.AddThought(Thought.Cult_BrokeUpFight);
      if ((UnityEngine.Object) HUD_Manager.Instance != (UnityEngine.Object) null)
        HUD_Manager.Instance.ClearFightingTarget();
      this.Brain.AddThought(Thought.Pacifist);
      this.End();
    }
  }

  public override void OnDoingBegin(Follower follower)
  {
    Follower followerById1 = FollowerManager.FindFollowerByID(this.fightingFollowerID0);
    Follower followerById2 = FollowerManager.FindFollowerByID(this.fightingFollowerID1);
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.BreakUpFightIE(follower, followerById1, followerById2));
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    FollowerBrain brainById1 = FollowerBrain.FindBrainByID(this.fightingFollowerID0);
    FollowerBrain brainById2 = FollowerBrain.FindBrainByID(this.fightingFollowerID1);
    return brainById1 != null && brainById1.CurrentTaskType == FollowerTaskType.FightFollower && brainById2 != null && brainById2.CurrentTaskType == FollowerTaskType.FightFollower ? (brainById1.LastPosition + brainById2.LastPosition) / 2f : this._brain.LastPosition;
  }

  public override void TaskTick(float deltaGameTime)
  {
  }

  public IEnumerator BreakUpFightIE(
    Follower follower,
    Follower fightFollower0,
    Follower fightFollower1)
  {
    FollowerTask_BreakUpFight taskBreakUpFight = this;
    if (fightFollower0.Brain.CurrentTask is FollowerTask_FightFollower currentTask1)
    {
      if (currentTask1.Interrupted || currentTask1.FinishedMurderCallback)
      {
        taskBreakUpFight.End();
        yield break;
      }
      currentTask1.Interrupt();
    }
    if (fightFollower1.Brain.CurrentTask is FollowerTask_FightFollower currentTask2)
    {
      if (currentTask2.Interrupted || currentTask2.FinishedMurderCallback)
      {
        taskBreakUpFight.End();
        yield break;
      }
      currentTask2.Interrupt();
    }
    Interaction_BreakUpFight breakUpFight0 = fightFollower0.GetComponent<Interaction_BreakUpFight>();
    Interaction_BreakUpFight breakUpFight1 = fightFollower1.GetComponent<Interaction_BreakUpFight>();
    if ((UnityEngine.Object) breakUpFight0 != (UnityEngine.Object) null)
      breakUpFight0.Interactable = false;
    if ((UnityEngine.Object) breakUpFight1 != (UnityEngine.Object) null)
      breakUpFight1.Interactable = false;
    yield return (object) new WaitForSeconds(0.75f);
    follower.TimedAnimationWithDuration("Conversations/react-mean3", Loop: false);
    yield return (object) new WaitForSeconds(0.25f);
    fightFollower0.transform.DOMove(fightFollower0.transform.position + (fightFollower0.transform.position - fightFollower1.transform.position).normalized / 2f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    fightFollower1.transform.DOMove(fightFollower1.transform.position + (fightFollower1.transform.position - fightFollower0.transform.position).normalized / 2f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    if (!(fightFollower0.Brain.CurrentTask is FollowerTask_ManualControl))
    {
      fightFollower0.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
      float time = 2f;
      fightFollower0.TimedAnimation(taskBreakUpFight.GetReactionAnim(fightFollower0.Brain, fightFollower1.Brain, out time), time, (System.Action) (() =>
      {
        fightFollower0.Brain.CurrentTask?.Abort();
        UnityEngine.Object.Destroy((UnityEngine.Object) breakUpFight0);
      }));
      fightFollower0.AddBodyAnimation("idle", true, 0.0f);
    }
    if (!(fightFollower1.Brain.CurrentTask is FollowerTask_ManualControl))
    {
      fightFollower1.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
      float time = 2f;
      fightFollower1.TimedAnimation(taskBreakUpFight.GetReactionAnim(fightFollower1.Brain, fightFollower0.Brain, out time), time, (System.Action) (() =>
      {
        fightFollower1.Brain.CurrentTask?.Abort();
        UnityEngine.Object.Destroy((UnityEngine.Object) breakUpFight1);
      }));
      fightFollower1.AddBodyAnimation("idle", true, 0.0f);
    }
    yield return (object) new WaitForSeconds(2f);
    CultFaithManager.AddThought(Thought.Cult_BrokeUpFight);
    if ((UnityEngine.Object) HUD_Manager.Instance != (UnityEngine.Object) null)
      HUD_Manager.Instance.ClearFightingTarget();
    taskBreakUpFight.End();
  }

  public string GetReactionAnim(FollowerBrain brain, FollowerBrain otherBrain, out float time)
  {
    if (brain.Info.IsDisciple && !otherBrain.Info.IsDisciple || brain.HasTrait(FollowerTrait.TraitType.Bastard) || brain.HasTrait(FollowerTrait.TraitType.Argumentative) || brain.HasTrait(FollowerTrait.TraitType.CriminalHardened))
    {
      time = 3.33333325f;
      return "Reactions/react-laugh";
    }
    if (brain.Info.HasTrait(FollowerTrait.TraitType.Scared) || brain.Info.HasTrait(FollowerTrait.TraitType.CriminalScarred))
    {
      time = 2.9333334f;
      return "Reactions/react-feared";
    }
    if ((double) UnityEngine.Random.value < 0.25)
    {
      time = 9f;
      return "Reactions/react-cry";
    }
    if ((double) UnityEngine.Random.value < 0.25)
    {
      time = 3f;
      return "Reactions/react-embarrassed";
    }
    time = 2f;
    return "Conversations/react-mean" + UnityEngine.Random.Range(1, 4).ToString();
  }
}
