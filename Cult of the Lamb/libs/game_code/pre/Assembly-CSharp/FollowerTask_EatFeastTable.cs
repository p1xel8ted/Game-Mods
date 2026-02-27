// Decompiled with JetBrains decompiler
// Type: FollowerTask_EatFeastTable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_EatFeastTable : FollowerTask
{
  private float _feastTabletEndPhase;

  public override FollowerTaskType Type => FollowerTaskType.EatFeastTable;

  public override FollowerLocation Location => FollowerLocation.Church;

  public override float Priorty => 1000f;

  public override bool BlockReactTasks => true;

  public override bool BlockTaskChanges => true;

  public override bool BlockThoughts => true;

  public FollowerTask_EatFeastTable(int feastTableID)
  {
  }

  protected override float SatiationChange(float deltaGameTime) => 0.0f;

  protected override float RestChange(float deltaGameTime) => 0.0f;

  protected override int GetSubTaskCode() => 0;

  protected override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  protected override void TaskTick(float deltaGameTime)
  {
  }

  protected override void OnEnd()
  {
    base.OnEnd();
    this._brain.AddThought(Thought.FeastTable, true);
    this._brain.Stats.Satiation = 100f;
  }

  protected override Vector3 UpdateDestination(Follower follower)
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
