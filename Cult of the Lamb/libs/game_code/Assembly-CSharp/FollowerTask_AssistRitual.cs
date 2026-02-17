// Decompiled with JetBrains decompiler
// Type: FollowerTask_AssistRitual
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_AssistRitual : FollowerTask_AssistPlayerBase
{
  public WeightPlate WeightPlate;
  public bool OnPlate;

  public override FollowerTaskType Type => FollowerTaskType.AssistRitual;

  public override FollowerLocation Location => this._brain.Location;

  public FollowerTask_AssistRitual(WeightPlate WeightPlate)
  {
    this._helpingPlayer = true;
    this.WeightPlate = WeightPlate;
  }

  public override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "worship");
  }

  public override void AssistPlayerTick(float deltaGameTime)
  {
    if (!this.EndIfPlayerIsDistant())
      return;
    this.WeightPlate.Reserved = false;
  }

  public override bool EndIfPlayerIsDistant()
  {
    PlayerFarming instance = PlayerFarming.Instance;
    WeightPlateManager weightPlateManager = this.WeightPlate.WeightPlateManager;
    if (!((Object) weightPlateManager != (Object) null) || !((Object) instance != (Object) null) || (double) Vector3.Distance(instance.transform.position, weightPlateManager.transform.position) <= (double) this.WeightPlate.WeightPlateManager.DeactivateRange)
      return false;
    this.End();
    return true;
  }

  public override void OnArrive()
  {
    base.OnArrive();
    this.OnPlate = true;
    this.WeightPlate.OnTriggerEnter2D((Collider2D) null);
  }

  public override void OnEnd()
  {
    if (this.OnPlate)
      this.WeightPlate.OnTriggerExit2D((Collider2D) null);
    base.OnEnd();
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    return !((Object) this.WeightPlate == (Object) null) ? this.WeightPlate.transform.position : follower.transform.position;
  }

  public override void OnDoingBegin(Follower follower)
  {
    follower.FacePosition(this.WeightPlate.WeightPlateManager.transform.position);
  }
}
