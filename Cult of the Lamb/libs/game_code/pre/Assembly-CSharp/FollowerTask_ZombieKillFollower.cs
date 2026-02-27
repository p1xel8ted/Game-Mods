// Decompiled with JetBrains decompiler
// Type: FollowerTask_ZombieKillFollower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_ZombieKillFollower : FollowerTask
{
  private FollowerBrain targetFollower;
  private Follower f;

  public override FollowerTaskType Type => FollowerTaskType.ZombieKillFollower;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockReactTasks => true;

  public override bool BlockTaskChanges => true;

  public FollowerTask_ZombieKillFollower(FollowerBrain targetFollower)
  {
    this.targetFollower = targetFollower;
  }

  protected override int GetSubTaskCode() => 0;

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    this.OnFollowerTaskStateChanged = this.OnFollowerTaskStateChanged + new FollowerTask.FollowerTaskDelegate(this.StateChange);
    this.f = follower;
  }

  private void StateChange(FollowerTaskState oldState, FollowerTaskState newState)
  {
    if (oldState != FollowerTaskState.GoingTo || newState != FollowerTaskState.Doing)
      return;
    Follower target = (Follower) null;
    Follower zombie = (Follower) null;
    foreach (Follower follower in FollowerManager.FollowersAtLocation(FollowerLocation.Base))
    {
      if (follower.Brain.Info.ID == this.targetFollower.Info.ID)
        target = follower;
      else if (follower.Brain.Info.ID == this._brain.Info.ID)
        zombie = follower;
    }
    if ((bool) (UnityEngine.Object) target && (bool) (UnityEngine.Object) zombie)
      zombie.StartCoroutine((IEnumerator) this.KillFollowerIE(target, zombie));
    else
      this.KillFollower();
  }

  protected override void TaskTick(float deltaGameTime)
  {
    if (!((UnityEngine.Object) this.f == (UnityEngine.Object) null))
      return;
    this.KillFollower();
  }

  public override void OnIdleBegin(Follower follower)
  {
    base.OnIdleBegin(follower);
    this.SetState(FollowerTaskState.GoingTo);
    if (this.targetFollower == null)
      return;
    this.targetFollower.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
  }

  private IEnumerator KillFollowerIE(Follower target, Follower zombie)
  {
    FollowerTask_ZombieKillFollower zombieKillFollower = this;
    target.Die(NotificationCentre.NotificationType.ZombieKilledFollower);
    yield return (object) new WaitForSeconds(2.5f);
    zombie.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num = (double) zombie.SetBodyAnimation("Insane/eat-poop", false);
    zombie.AddBodyAnimation("Insane/idle-insane", false, 0.0f);
    target.State.facingAngle = Utils.GetAngle(target.transform.position, zombie.transform.position);
    zombie.State.facingAngle = Utils.GetAngle(zombie.transform.position, target.transform.position);
    yield return (object) new WaitForSeconds(1f);
    Interaction_HarvestMeat interactionHarvestMeat1 = (Interaction_HarvestMeat) null;
    foreach (Interaction_HarvestMeat interactionHarvestMeat2 in Interaction_HarvestMeat.Interaction_HarvestMeats)
    {
      if ((UnityEngine.Object) interactionHarvestMeat1 == (UnityEngine.Object) null || (double) Vector3.Distance(interactionHarvestMeat2.transform.position, target.transform.position) < (double) Vector3.Distance(interactionHarvestMeat1.transform.position, target.transform.position))
        interactionHarvestMeat1 = interactionHarvestMeat2;
    }
    if ((bool) (UnityEngine.Object) interactionHarvestMeat1)
    {
      StructureManager.RemoveStructure(interactionHarvestMeat1.DeadWorshipper.Structure.Brain);
      UnityEngine.Object.Destroy((UnityEngine.Object) interactionHarvestMeat1.gameObject);
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", interactionHarvestMeat1.transform.position);
      for (int index = 0; index < 5; ++index)
        ResourceCustomTarget.Create(zombie.gameObject, interactionHarvestMeat1.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle, InventoryItem.ITEM_TYPE.FOLLOWER_MEAT, (System.Action) null);
    }
    zombie.State.CURRENT_STATE = StateMachine.State.Idle;
    yield return (object) new WaitForSeconds(1f);
    zombieKillFollower._brain.Stats.Satiation = 100f;
    zombieKillFollower._brain.HardSwapToTask((FollowerTask) new FollowerTask_Zombie());
    zombieKillFollower._brain.AddThought(Thought.ZombieAteMeal);
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.Info.CursedState != Thought.Zombie)
        allBrain.AddThought(Thought.ZombieKilledFollower);
    }
  }

  private void KillFollower()
  {
    int id = this.targetFollower.Info.ID;
    this.targetFollower.Die(NotificationCentre.NotificationType.ZombieKilledFollower);
    foreach (FollowerInfo followerInfo in DataManager.Instance.Followers_Dead)
    {
      if (followerInfo.ID == id)
      {
        using (List<StructureBrain>.Enumerator enumerator = StructureManager.GetAllStructuresOfType(FollowerLocation.Base, StructureBrain.TYPES.DEAD_WORSHIPPER).GetEnumerator())
        {
          if (enumerator.MoveNext())
          {
            StructureManager.RemoveStructure(StructureBrain.GetOrCreateBrain(enumerator.Current.Data));
            break;
          }
          break;
        }
      }
    }
    this._brain.Stats.Satiation = 100f;
    this._brain.HardSwapToTask((FollowerTask) new FollowerTask_Zombie());
    this._brain.AddThought(Thought.ZombieAteMeal);
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.Info.CursedState != Thought.Zombie)
        allBrain.AddThought(Thought.ZombieKilledFollower);
    }
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    if (this.targetFollower != null && FollowerBrain.AllBrains.Contains(this.targetFollower))
      this.targetFollower.CompleteCurrentTask();
    this.OnFollowerTaskStateChanged = this.OnFollowerTaskStateChanged - new FollowerTask.FollowerTaskDelegate(this.StateChange);
  }

  protected override Vector3 UpdateDestination(Follower follower)
  {
    return this.targetFollower.LastPosition + Vector3.right * ((double) this.targetFollower.LastPosition.x < (double) follower.transform.position.x ? 0.5f : -0.5f);
  }
}
