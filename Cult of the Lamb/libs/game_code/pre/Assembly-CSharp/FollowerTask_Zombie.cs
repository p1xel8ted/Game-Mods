// Decompiled with JetBrains decompiler
// Type: FollowerTask_Zombie
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_Zombie : FollowerTask
{
  private const float IDLE_DURATION_GAME_MINUTES_MIN = 10f;
  private const float IDLE_DURATION_GAME_MINUTES_MAX = 20f;
  private float _gameTimeToNextStateUpdate;
  private float _speechDurationRemaining;
  private Follower follower;
  private float delay = 5f;
  private float deadBodyCheck = 5f;
  private StructureBrain deadBody;

  public override FollowerTaskType Type => FollowerTaskType.Zombie;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockReactTasks => true;

  protected override int GetSubTaskCode() => 0;

  protected override void TaskTick(float deltaGameTime)
  {
    if (this._state == FollowerTaskState.Idle)
    {
      this._gameTimeToNextStateUpdate -= deltaGameTime;
      if ((double) this._gameTimeToNextStateUpdate <= 0.0)
      {
        this.ClearDestination();
        this.SetState(FollowerTaskState.GoingTo);
        this._gameTimeToNextStateUpdate = UnityEngine.Random.Range(10f, 20f);
      }
    }
    this.delay -= Time.deltaTime;
    this.deadBodyCheck -= Time.deltaTime;
    if ((double) this.deadBodyCheck <= 0.0 && this.deadBody == null)
    {
      List<StructureBrain> structuresOfType = StructureManager.GetAllStructuresOfType(FollowerLocation.Base, StructureBrain.TYPES.DEAD_WORSHIPPER);
      if (structuresOfType.Count > 0)
      {
        this.deadBody = structuresOfType[0];
        this.SetState(FollowerTaskState.GoingTo);
      }
      this.deadBodyCheck = 5f;
    }
    if (this.deadBody != null && this._state == FollowerTaskState.Idle)
      this.SetState(FollowerTaskState.GoingTo);
    if ((double) this._brain.Stats.Satiation != 0.0 || (double) this.delay >= 0.0 || this.deadBody != null)
      return;
    FollowerBrain targetFollower = (FollowerBrain) null;
    float num1 = float.MaxValue;
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      float num2 = Vector3.Distance(PlayerFarming.Instance.transform.position, allBrain.LastPosition);
      if ((double) num2 < (double) num1 && allBrain.Info.CursedState != Thought.Zombie && !DataManager.Instance.Followers_OnMissionary_IDs.Contains(allBrain._directInfoAccess.ID) && !DataManager.Instance.Followers_Imprisoned_IDs.Contains(allBrain._directInfoAccess.ID))
      {
        num1 = num2;
        targetFollower = allBrain;
      }
    }
    this._brain.HardSwapToTask((FollowerTask) new FollowerTask_ZombieKillFollower(targetFollower));
  }

  public override void OnIdleBegin(Follower follower)
  {
    base.OnIdleBegin(follower);
    if (!((UnityEngine.Object) follower != (UnityEngine.Object) null) || (double) this._brain.Stats.Starvation < 37.5 || follower.WorshipperBubble.Active)
      return;
    WorshipperBubble.SPEECH_TYPE Type = WorshipperBubble.SPEECH_TYPE.FOLLOWERMEAT;
    follower.WorshipperBubble.Play(Type);
  }

  protected override void OnArrive()
  {
    if (this.deadBody != null)
    {
      if (StructureManager.GetStructureByID<StructureBrain>(this.deadBody.Data.ID) != null)
      {
        if ((double) Vector3.Distance(this.deadBody.Data.Position, this._brain.LastPosition) < 2.0)
        {
          if ((UnityEngine.Object) this.follower != (UnityEngine.Object) null)
            this.follower.StartCoroutine((IEnumerator) this.EatDeadBodyIE());
          else
            this.EatDeadBody();
        }
        else
          this.SetState(FollowerTaskState.Idle);
      }
      else
      {
        this.deadBody = (StructureBrain) null;
        this.SetState(FollowerTaskState.Idle);
      }
    }
    else
      this.SetState(FollowerTaskState.Idle);
  }

  private IEnumerator EatDeadBodyIE()
  {
    FollowerTask_Zombie followerTaskZombie = this;
    foreach (Interaction_HarvestMeat interactionHarvestMeat in Interaction_HarvestMeat.Interaction_HarvestMeats)
    {
      if (interactionHarvestMeat.structure.Structure_Info.ID == followerTaskZombie.deadBody.Data.ID)
      {
        interactionHarvestMeat.enabled = false;
        break;
      }
    }
    followerTaskZombie.follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num = (double) followerTaskZombie.follower.SetBodyAnimation("Insane/eat-poop", false);
    followerTaskZombie.follower.AddBodyAnimation("Insane/idle-insane", false, 0.0f);
    yield return (object) new WaitForSeconds(2f);
    followerTaskZombie.deadBody.Remove();
    AudioManager.Instance.PlayOneShot("event:/followers/pop_in", followerTaskZombie.follower.gameObject);
    for (int index = 0; index < 5; ++index)
      ResourceCustomTarget.Create(followerTaskZombie.follower.gameObject, followerTaskZombie.deadBody.Data.Position + (Vector3) UnityEngine.Random.insideUnitCircle, InventoryItem.ITEM_TYPE.FOLLOWER_MEAT, (System.Action) null);
    yield return (object) new WaitForSeconds(1f);
    followerTaskZombie._brain.Stats.Satiation = 100f;
    followerTaskZombie.deadBody = (StructureBrain) null;
    followerTaskZombie.follower.State.CURRENT_STATE = StateMachine.State.Idle;
    followerTaskZombie.SetState(FollowerTaskState.Idle);
    followerTaskZombie._brain.AddThought(Thought.ZombieAteMeal);
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.Info.CursedState != Thought.Zombie)
        allBrain.AddThought(Thought.ZombieAteDeadFollower);
    }
  }

  private void EatDeadBody()
  {
    this._brain.Stats.Satiation = 100f;
    this.deadBody.Remove();
    this.deadBody = (StructureBrain) null;
    this.SetState(FollowerTaskState.Idle);
    this._brain.AddThought(Thought.ZombieAteMeal);
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.Info.CursedState != Thought.Zombie)
        allBrain.AddThought(Thought.ZombieAteDeadFollower);
    }
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Insane/idle-insane");
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Insane/run-insane");
    follower.SetOutfit(FollowerOutfitType.Rags, false);
    this.follower = follower;
  }

  protected override Vector3 UpdateDestination(Follower follower)
  {
    return this.deadBody != null ? this.deadBody.Data.Position + (Vector3) UnityEngine.Random.insideUnitCircle * 0.5f : TownCentre.RandomCircleFromTownCentre(8f);
  }

  protected override float RestChange(float deltaGameTime) => 0.0f;

  protected override float SocialChange(float deltaGameTime) => 0.0f;

  protected override float VomitChange(float deltaGameTime) => 0.0f;
}
