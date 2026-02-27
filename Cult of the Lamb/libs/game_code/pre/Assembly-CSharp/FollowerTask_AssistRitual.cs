// Decompiled with JetBrains decompiler
// Type: FollowerTask_AssistRitual
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_AssistRitual : FollowerTask_AssistPlayerBase
{
  public WeightPlate WeightPlate;
  private bool OnPlate;

  public override FollowerTaskType Type => FollowerTaskType.AssistRitual;

  public override FollowerLocation Location => this._brain.Location;

  public FollowerTask_AssistRitual(WeightPlate WeightPlate)
  {
    this._helpingPlayer = true;
    this.WeightPlate = WeightPlate;
  }

  protected override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "worship");
  }

  protected override void AssistPlayerTick(float deltaGameTime)
  {
    if (!this.EndIfPlayerIsDistant())
      return;
    this.WeightPlate.Reserved = false;
  }

  protected override bool EndIfPlayerIsDistant()
  {
    PlayerFarming instance = PlayerFarming.Instance;
    WeightPlateManager weightPlateManager = this.WeightPlate.WeightPlateManager;
    if (!((Object) weightPlateManager != (Object) null) || !((Object) instance != (Object) null) || (double) Vector3.Distance(instance.transform.position, weightPlateManager.transform.position) <= (double) this.WeightPlate.WeightPlateManager.DeactivateRange)
      return false;
    this.End();
    return true;
  }

  protected override void OnArrive()
  {
    base.OnArrive();
    this.OnPlate = true;
    this.WeightPlate.OnTriggerEnter2D((Collider2D) null);
  }

  protected override void OnEnd()
  {
    if (this.OnPlate)
      this.WeightPlate.OnTriggerExit2D((Collider2D) null);
    base.OnEnd();
  }

  protected override Vector3 UpdateDestination(Follower follower)
  {
    return !((Object) this.WeightPlate == (Object) null) ? this.WeightPlate.transform.position : follower.transform.position;
  }

  public override void OnDoingBegin(Follower follower)
  {
    follower.FacePosition(this.WeightPlate.WeightPlateManager.transform.position);
  }
}
