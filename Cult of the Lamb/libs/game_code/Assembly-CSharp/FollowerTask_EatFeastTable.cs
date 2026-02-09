// Decompiled with JetBrains decompiler
// Type: FollowerTask_EatFeastTable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_EatFeastTable : FollowerTask
{
  public float _feastTabletEndPhase;

  public override FollowerTaskType Type => FollowerTaskType.EatFeastTable;

  public override FollowerLocation Location => FollowerLocation.Church;

  public override float Priorty => 1000f;

  public override bool BlockReactTasks => true;

  public override bool BlockTaskChanges => true;

  public override bool BlockThoughts => true;

  public FollowerTask_EatFeastTable(int feastTableID)
  {
  }

  public override float SatiationChange(float deltaGameTime) => 0.0f;

  public override float RestChange(float deltaGameTime) => 0.0f;

  public override int GetSubTaskCode() => 0;

  public override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  public override void TaskTick(float deltaGameTime)
  {
  }

  public override void OnEnd()
  {
    base.OnEnd();
    this._brain.AddThought(Thought.FeastTable, true);
    this._brain.Stats.Satiation = 100f;
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    return Interaction_FeastTable.FeastTables[0].GetEatPosition(follower);
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    if (this.State == FollowerTaskState.Doing)
      follower.State.facingAngle = Utils.GetAngle(follower.transform.position, Interaction_FeastTable.FeastTables[0].transform.position);
    follower.HideStats();
    follower.Interaction_FollowerInteraction.Interactable = false;
  }

  public override void OnDoingBegin(Follower follower)
  {
    follower.State.facingAngle = Utils.GetAngle(follower.transform.position, Interaction_FeastTable.FeastTables[0].transform.position);
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.Interaction_FollowerInteraction.Interactable = true;
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "idle");
  }
}
