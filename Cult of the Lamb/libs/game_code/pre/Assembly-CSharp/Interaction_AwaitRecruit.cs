// Decompiled with JetBrains decompiler
// Type: Interaction_AwaitRecruit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_AwaitRecruit : Interaction
{
  private Worshipper w;
  public float SacrificeDeathShakeTime = 4.8f;
  public float SacrificeTotalTime = 8f;
  public float RecruitTotalTime = 6f;
  private string sPassJudgement;
  private string sSacrifice;
  private bool completed;
  private SimpleSpineAnimator playerAnimator;
  private int WaitingForPositions;

  private void Start()
  {
    this.w = this.GetComponent<Worshipper>();
    this.UpdateLocalisation();
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sSacrifice = ScriptLocalization.Interactions.Sacrifice;
  }

  public override void GetLabel() => this.Label = this.completed ? "" : this.sPassJudgement;

  public override void OnInteract(StateMachine state)
  {
  }

  private void Recruit()
  {
    this.w.interaction_AwaitRecruit.enabled = false;
    this.StartCoroutine((IEnumerator) this.RecruitFollower());
  }

  private IEnumerator RecruitFollower()
  {
    Interaction_AwaitRecruit interactionAwaitRecruit = this;
    GameObject TargetPosition = new GameObject();
    float angle = Utils.GetAngle(interactionAwaitRecruit.transform.position, Altar.Instance.CentrePoint.transform.position);
    TargetPosition.transform.position = interactionAwaitRecruit.transform.position + new Vector3(1.5f * Mathf.Cos(angle * ((float) Math.PI / 180f)), 1.5f * Mathf.Sin(angle * ((float) Math.PI / 180f)));
    PlayerFarming playerFarming = interactionAwaitRecruit.state.GetComponent<PlayerFarming>();
    playerFarming.GoToAndStop(TargetPosition, interactionAwaitRecruit.w.gameObject);
    while (playerFarming.GoToAndStopping)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew(false);
    GameManager.GetInstance().OnConversationNext(interactionAwaitRecruit.w.gameObject, 4f);
    GameManager.GetInstance().AddPlayerToCamera();
    SimpleSpineAnimator componentInChildren = interactionAwaitRecruit.state.GetComponentInChildren<SimpleSpineAnimator>();
    interactionAwaitRecruit.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    componentInChildren.Animate("recruit", 0, true);
    componentInChildren.AddAnimate("idle", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(interactionAwaitRecruit.RecruitTotalTime - 3f);
    interactionAwaitRecruit.w.state.CURRENT_STATE = StateMachine.State.Recruited;
    yield return (object) new WaitForSeconds(4f);
    interactionAwaitRecruit.w.state.CURRENT_STATE = StateMachine.State.Idle;
    interactionAwaitRecruit.state.CURRENT_STATE = StateMachine.State.Idle;
    GameManager.GetInstance().OnConversationEnd();
  }

  private void Sacrifice()
  {
    this.w.state.CURRENT_STATE = StateMachine.State.SacrificeRecruit;
    GameManager.GetInstance().OnConversationNew(false);
    GameManager.GetInstance().OnConversationNext(this.w.gameObject, 4f);
    GameManager.GetInstance().AddPlayerToCamera();
    this.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    this.StartCoroutine((IEnumerator) this.FollowerToAltar());
  }

  private IEnumerator FollowerToAltar()
  {
    Interaction_AwaitRecruit interactionAwaitRecruit = this;
    interactionAwaitRecruit.StartCoroutine((IEnumerator) interactionAwaitRecruit.WaitForPlayerToAltar());
    interactionAwaitRecruit.StartCoroutine((IEnumerator) interactionAwaitRecruit.WaitForFollowerToAltar());
    while (interactionAwaitRecruit.WaitingForPositions < 2)
      yield return (object) null;
    yield return (object) new WaitForSeconds(1f);
    interactionAwaitRecruit.StartCoroutine((IEnumerator) interactionAwaitRecruit.SacrificeFollower());
  }

  private IEnumerator WaitForPlayerToAltar()
  {
    Interaction_AwaitRecruit interactionAwaitRecruit = this;
    PlayerFarming playerFarming = interactionAwaitRecruit.state.GetComponent<PlayerFarming>();
    playerFarming.GoToAndStop(Altar.Instance.SacrificePositions[1].gameObject);
    while (playerFarming.GoToAndStopping)
      yield return (object) null;
    interactionAwaitRecruit.state.facingAngle = Utils.GetAngle(Altar.Instance.SacrificePositions[1].transform.position, Altar.Instance.SacrificePositions[0].transform.position);
    ++interactionAwaitRecruit.WaitingForPositions;
  }

  private IEnumerator WaitForFollowerToAltar()
  {
    Interaction_AwaitRecruit interactionAwaitRecruit = this;
    interactionAwaitRecruit.w.GoToAndStop(Altar.Instance.SacrificePositions[0].gameObject, new System.Action(interactionAwaitRecruit.WaitState), Altar.Instance.SacrificePositions[1].gameObject, true);
    while (interactionAwaitRecruit.w.GoToAndStopping)
      yield return (object) null;
    yield return (object) new WaitForSeconds(0.5f);
    ++interactionAwaitRecruit.WaitingForPositions;
  }

  private void WaitState() => this.w.state.CURRENT_STATE = StateMachine.State.AwaitRecruit;

  private IEnumerator SacrificeFollower()
  {
    Interaction_AwaitRecruit interactionAwaitRecruit = this;
    interactionAwaitRecruit.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    interactionAwaitRecruit.w.state.CURRENT_STATE = StateMachine.State.SacrificeRecruit;
    interactionAwaitRecruit.playerAnimator = interactionAwaitRecruit.state.GetComponentInChildren<SimpleSpineAnimator>();
    interactionAwaitRecruit.playerAnimator.Animate("sacrifice", 0, false);
    interactionAwaitRecruit.playerAnimator.AddAnimate("idle", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(interactionAwaitRecruit.SacrificeDeathShakeTime);
    CameraManager.shakeCamera(0.3f, (float) UnityEngine.Random.Range(0, 360));
    yield return (object) new WaitForSeconds(interactionAwaitRecruit.SacrificeTotalTime - interactionAwaitRecruit.SacrificeDeathShakeTime);
    interactionAwaitRecruit.state.CURRENT_STATE = StateMachine.State.Idle;
    GameManager.GetInstance().OnConversationEnd();
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionAwaitRecruit.gameObject);
  }
}
