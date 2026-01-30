// Decompiled with JetBrains decompiler
// Type: FollowerTask_Drunk
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_Drunk : FollowerTask
{
  public static Vector2 RANDOM_DURATION = new Vector2(7f, 9f);
  public float _gameTimeToNextStateUpdate;
  public float _gameTimeToNextScenario;
  public float _gameTimeToWakeUpFollowers;
  public Follower follower;
  public Structure trippingOverTargetStructure;
  public bool isSinging;
  public bool isTripping;
  public static List<Follower> wokenFollowers = new List<Follower>();
  public const float WAKE_FOLLOWER_RADIUS = 10f;

  public override FollowerTaskType Type => FollowerTaskType.Drunk;

  public override FollowerLocation Location => FollowerLocation.Base;

  public FollowerTask_Drunk()
  {
    this._gameTimeToNextStateUpdate = UnityEngine.Random.Range(4f, 6f);
    this._gameTimeToNextScenario = UnityEngine.Random.Range(FollowerTask_Drunk.RANDOM_DURATION.x, FollowerTask_Drunk.RANDOM_DURATION.y);
  }

  public override int GetSubTaskCode() => 0;

  public override void OnArrive()
  {
    if (!((UnityEngine.Object) this.trippingOverTargetStructure == (UnityEngine.Object) null))
      return;
    this.SetState(FollowerTaskState.Idle);
  }

  public override void TaskTick(float deltaGameTime)
  {
    if ((double) this._brain.Stats.Drunk <= 0.0)
    {
      this.End();
    }
    else
    {
      if ((UnityEngine.Object) this.trippingOverTargetStructure != (UnityEngine.Object) null && !this.isTripping && (double) Vector3.Distance(this.Brain.LastPosition, this.trippingOverTargetStructure.transform.position) < 0.5)
        this.EndTripOverStructure();
      this._gameTimeToNextScenario -= deltaGameTime;
      if (this._state == FollowerTaskState.Idle && (UnityEngine.Object) this.trippingOverTargetStructure == (UnityEngine.Object) null)
      {
        this._gameTimeToNextStateUpdate -= deltaGameTime;
        if ((double) this._gameTimeToNextStateUpdate <= 0.0)
        {
          this.ClearDestination();
          this.SetState(FollowerTaskState.GoingTo);
          this._gameTimeToNextStateUpdate = UnityEngine.Random.Range(4f, 6f);
        }
      }
      if ((double) this._gameTimeToNextScenario <= 0.0 && (UnityEngine.Object) this.trippingOverTargetStructure == (UnityEngine.Object) null)
      {
        int num = UnityEngine.Random.Range(0, 100);
        if (this.Brain.HasTrait(FollowerTrait.TraitType.Lightweight))
        {
          if (num <= 30)
            this.FightFollower();
          else if (num <= 60)
            this.StartTripOverStructure();
          else if (num <= 80 /*0x50*/)
            this.Vomit();
          else if (num <= 85)
            this.TalkToFollower();
          else if (num <= 90)
            this.Woozy();
          else if (num <= 95)
            this.Laugh();
          else if (num <= 100)
            this.Sing();
        }
        else if (num <= 10)
          this.TalkToFollower();
        else if (num <= 15)
          this.FightFollower();
        else if (num <= 20)
          this.StartTripOverStructure();
        else if (num <= 30)
          this.Vomit();
        else if (num <= 55)
          this.Woozy();
        else if (num <= 75)
          this.Laugh();
        else if (num <= 100)
          this.Sing();
        this._gameTimeToNextScenario = UnityEngine.Random.Range(FollowerTask_Drunk.RANDOM_DURATION.x, FollowerTask_Drunk.RANDOM_DURATION.y);
      }
      this._gameTimeToWakeUpFollowers -= deltaGameTime;
      if (!this.isSinging || (double) this._gameTimeToWakeUpFollowers > 0.0)
        return;
      this._gameTimeToWakeUpFollowers = UnityEngine.Random.Range(4f, 8f);
      this.WakeFollowers();
    }
  }

  public void TalkToFollower()
  {
    List<FollowerBrain> followerBrainList = new List<FollowerBrain>((IEnumerable<FollowerBrain>) this.GetPossibleFollowers());
    if (followerBrainList.Count <= 0)
      return;
    FollowerBrain otherBrain = followerBrainList[UnityEngine.Random.Range(0, followerBrainList.Count)];
    otherBrain._directInfoAccess.Social = 0.0f;
    this.Brain._directInfoAccess.Social = 0.0f;
    this.Brain.CheckForInteraction(otherBrain, 0.0f);
  }

  public void FightFollower()
  {
    List<FollowerBrain> followerBrainList = new List<FollowerBrain>((IEnumerable<FollowerBrain>) this.GetPossibleFollowers());
    if (followerBrainList.Count <= 0)
      return;
    this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_FightFollower(followerBrainList[UnityEngine.Random.Range(0, followerBrainList.Count)].Info.ID, true));
  }

  public void Vomit() => this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_Vomit());

  public void Woozy()
  {
    if (!((UnityEngine.Object) this.follower != (UnityEngine.Object) null))
      return;
    FollowerState_Drunk followerStateDrunk = (FollowerState_Drunk) null;
    if (this.follower.Brain.CurrentState != null && this.follower.Brain.CurrentState is FollowerState_Drunk currentState)
      followerStateDrunk = currentState;
    string animation = "Drunk/drunk-woozy-happy";
    if (followerStateDrunk != null)
    {
      if (followerStateDrunk.AngryDrunk)
        animation = "Drunk/drunk-woozy-angry";
      else if (followerStateDrunk.SadDrunk)
        animation = "Drunk/drunk-woozy-sad";
    }
    this.follower.TimedAnimation(animation, 1.9f);
  }

  public void Laugh()
  {
    if (!((UnityEngine.Object) this.follower != (UnityEngine.Object) null))
      return;
    this.follower.TimedAnimation("Drunk/drunk-react-laugh", 3f);
  }

  public void Sing()
  {
    if (!((UnityEngine.Object) this.follower != (UnityEngine.Object) null))
      return;
    this._gameTimeToWakeUpFollowers = UnityEngine.Random.Range(2f, 4f);
    this.isSinging = true;
    FollowerState_Drunk followerStateDrunk = (FollowerState_Drunk) null;
    if (this.follower.Brain.CurrentState != null && this.follower.Brain.CurrentState is FollowerState_Drunk currentState)
      followerStateDrunk = currentState;
    string animation = "Drunk/drunk-singing-happy";
    if (followerStateDrunk != null)
    {
      if (followerStateDrunk.AngryDrunk)
        animation = "Drunk/drunk-singing-angry";
      else if (followerStateDrunk.SadDrunk)
        animation = "Drunk/drunk-singing-sad";
    }
    this.follower.TimedAnimation(animation, 5.1f, (System.Action) (() =>
    {
      FollowerTask_Drunk.wokenFollowers.Clear();
      this.isSinging = false;
    }));
  }

  public void WakeFollowers()
  {
    List<Follower> followerList = new List<Follower>();
    float num1 = 10f;
    foreach (Follower follower in Follower.Followers)
    {
      if ((double) Vector3.Distance(follower.transform.position, this.Brain.LastPosition) <= (double) num1 && follower.Brain.CurrentTaskType == FollowerTaskType.Sleep && follower.Brain.Info.CursedState == Thought.None && !FollowerTask_Drunk.wokenFollowers.Contains(follower))
        followerList.Add(follower);
    }
    int num2 = Mathf.Min(followerList.Count, UnityEngine.Random.Range(1, 4));
    for (int index = 0; index < num2; ++index)
    {
      Follower follower = followerList[UnityEngine.Random.Range(0, followerList.Count)];
      FollowerTask_Drunk.wokenFollowers.Add(follower);
      followerList.Remove(follower);
      follower.StartCoroutine((IEnumerator) this.WakeUpFollowerAnnoyed(follower));
    }
  }

  public IEnumerator WakeUpFollowerAnnoyed(Follower follower)
  {
    FollowerTask_Drunk followerTaskDrunk = this;
    string previousAnim = follower.Spine.AnimationState.GetCurrent(1).Animation.Name;
    follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    follower.FacePosition(followerTaskDrunk.Brain.LastPosition);
    follower.TimedAnimation("tantrum", 3.2f);
    yield return (object) new WaitForSeconds(0.5f);
    follower.WorshipperBubble.gameObject.SetActive(true);
    follower.WorshipperBubble.Play(WorshipperBubble.SPEECH_TYPE.ENEMIES);
    yield return (object) new WaitForSeconds(2.7f);
    double num = (double) follower.SetBodyAnimation("sleepy", false);
    follower.AddBodyAnimation(previousAnim, true, 0.0f);
    LayerMask layerMask = (LayerMask) ((int) new LayerMask() | 1 << LayerMask.NameToLayer("Island"));
    Vector3 vector3;
    do
    {
      Vector2 direction = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(0.0f, -1f));
      vector3 = (Vector3) (Physics2D.Raycast((Vector2) Vector3.zero, direction, 1000f, (int) layerMask).point + -direction * UnityEngine.Random.Range(2f, 4f));
    }
    while ((double) Vector3.Distance(vector3, followerTaskDrunk._brain.LastPosition) < 10.0);
    follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_Sleep(vector3));
  }

  public void StartTripOverStructure()
  {
    List<Structure> source = new List<Structure>();
    foreach (Structure structure in Structure.Structures)
    {
      if ((UnityEngine.Object) structure != (UnityEngine.Object) null && structure.Brain != null && structure.Brain.Data != null && !structure.Brain.Data.IsCollapsed && !structure.Brain.Data.IsSnowedUnder && structure.Brain.Data.Bounds.x <= 1 && StructureManager.IsCollapsible(structure.Brain.Data.Type) && (double) Vector3.Distance(this.Brain.LastPosition, structure.Brain.Data.Position) < 5.0)
        source.Add(structure);
    }
    if (source.Count <= 0)
      return;
    this.ClearDestination();
    this.SetState(FollowerTaskState.Wait);
    source.OrderBy<Structure, float>((Func<Structure, float>) (x => Vector3.Distance(x.transform.position, this.Brain.LastPosition)));
    this.trippingOverTargetStructure = source[UnityEngine.Random.Range(0, source.Count)];
    this.SetState(FollowerTaskState.GoingTo);
  }

  public void EndTripOverStructure()
  {
    this.isTripping = true;
    double num = (double) this.follower.SetBodyAnimation("Drunk/drunk-trip", false);
    GameManager.GetInstance().WaitForSeconds(0.5f, (System.Action) (() =>
    {
      this.trippingOverTargetStructure.Brain.Collapse(false);
      NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("Notifications/StructureCollapsedFromDrunkFollower", this.Brain.Info.Name, this.trippingOverTargetStructure.Brain.Data.GetLocalizedName());
    }));
    GameManager.GetInstance().WaitForSeconds(3f, (System.Action) (() =>
    {
      this.trippingOverTargetStructure = (Structure) null;
      this.isTripping = false;
    }));
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    if (this.Brain.Info.IsDrunk)
      return;
    follower.SimpleAnimator.ResetAnimationsToDefaults();
  }

  public List<FollowerBrain> GetPossibleFollowers()
  {
    List<FollowerBrain> possibleFollowers = new List<FollowerBrain>();
    List<Follower> followerList = FollowerManager.FollowersAtLocation(this.Brain.Location);
    for (int index = 0; index < followerList.Count; ++index)
    {
      if ((UnityEngine.Object) followerList[index] != (UnityEngine.Object) null && followerList[index].Brain != this.Brain && (UnityEngine.Object) followerList[index].transform != (UnityEngine.Object) null && !FollowerManager.FollowerLocked(followerList[index].Brain.Info.ID) && followerList[index].Brain.Info.IsDrunk && followerList[index].Brain.CurrentTaskType != FollowerTaskType.FightFollower && followerList[index].Brain.CurrentTaskType != FollowerTaskType.Chat && (followerList[index].Brain.CurrentTask != null && !followerList[index].Brain.CurrentTask.BlockSocial || followerList[index].Brain.CurrentTask == null) && (double) Vector3.Distance(followerList[index].transform.position, this.Brain.LastPosition) < 5.0)
        possibleFollowers.Add(followerList[index].Brain);
    }
    return possibleFollowers;
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    this.follower = follower;
    this.SetAnims();
  }

  public void SetAnims()
  {
    this.SetState(FollowerTaskState.Idle);
    this.follower.SimpleAnimator.ResetAnimationsToDefaults();
    string NewAnimation1 = "Drinking/idle-drunk-happy";
    if (this.follower.Brain != null && (this.follower.Brain.HasTrait(FollowerTrait.TraitType.Argumentative) || this.follower.Brain.HasTrait(FollowerTrait.TraitType.Bastard) || this.follower.Brain.HasTrait(FollowerTrait.TraitType.CriminalHardened)))
      NewAnimation1 = "Drinking/idle-drunk-angry";
    else if (this.follower.Brain != null && (this.follower.Brain.HasTrait(FollowerTrait.TraitType.Scared) || this.follower.Brain.HasTrait(FollowerTrait.TraitType.Terrified) || this.follower.Brain.Info.HasTrait(FollowerTrait.TraitType.CriminalScarred) || this.follower.Brain.Info.HasTrait(FollowerTrait.TraitType.MissionaryTerrified)))
      NewAnimation1 = "Drinking/idle-drunk-sad";
    string NewAnimation2 = "Drinking/walk-drunk-happy";
    if (this.follower.Brain != null && (this.follower.Brain.HasTrait(FollowerTrait.TraitType.Argumentative) || this.follower.Brain.HasTrait(FollowerTrait.TraitType.Bastard) || this.follower.Brain.HasTrait(FollowerTrait.TraitType.CriminalHardened)))
      NewAnimation2 = "Drinking/walk-drunk-angry";
    else if (this.follower.Brain != null && (this.follower.Brain.HasTrait(FollowerTrait.TraitType.Scared) || this.follower.Brain.HasTrait(FollowerTrait.TraitType.Terrified) || this.follower.Brain.Info.HasTrait(FollowerTrait.TraitType.CriminalScarred) || this.follower.Brain.Info.HasTrait(FollowerTrait.TraitType.MissionaryTerrified)))
      NewAnimation2 = "Drinking/walk-drunk-sad";
    this.follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, NewAnimation1);
    this.follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, NewAnimation2);
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    if ((UnityEngine.Object) this.trippingOverTargetStructure != (UnityEngine.Object) null)
      return this.trippingOverTargetStructure.transform.position;
    Structures_Pub pub = this.GetPub();
    return pub != null ? pub.Data.Position + (Vector3) UnityEngine.Random.insideUnitCircle * 5f : TownCentre.RandomPositionInCachedTownCentre();
  }

  public Structures_Pub GetPub()
  {
    List<Structures_Pub> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_Pub>();
    return structuresOfType.Count > 0 ? structuresOfType[0] : (Structures_Pub) null;
  }

  [CompilerGenerated]
  public void \u003CSing\u003Eb__22_0()
  {
    FollowerTask_Drunk.wokenFollowers.Clear();
    this.isSinging = false;
  }

  [CompilerGenerated]
  public float \u003CStartTripOverStructure\u003Eb__26_0(Structure x)
  {
    return Vector3.Distance(x.transform.position, this.Brain.LastPosition);
  }

  [CompilerGenerated]
  public void \u003CEndTripOverStructure\u003Eb__27_0()
  {
    this.trippingOverTargetStructure.Brain.Collapse(false);
    NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("Notifications/StructureCollapsedFromDrunkFollower", this.Brain.Info.Name, this.trippingOverTargetStructure.Brain.Data.GetLocalizedName());
  }

  [CompilerGenerated]
  public void \u003CEndTripOverStructure\u003Eb__27_1()
  {
    this.trippingOverTargetStructure = (Structure) null;
    this.isTripping = false;
  }
}
