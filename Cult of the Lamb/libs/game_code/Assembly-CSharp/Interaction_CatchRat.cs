// Decompiled with JetBrains decompiler
// Type: Interaction_CatchRat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_CatchRat : Interaction
{
  [SerializeField]
  public CritterRat CritterRat;
  public bool Activating;
  public string sString;

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.ActivateDistance = 1f;
  }

  public void Start() => this.UpdateLocalisation();

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = ScriptLocalization.Interactions.CatchCritter;
  }

  public override void GetLabel() => this.Label = this.Activating ? "" : this.sString;

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if ((UnityEngine.Object) this.CritterRat == (UnityEngine.Object) null || this.CritterRat.state.CURRENT_STATE == StateMachine.State.CustomAnimation)
      this.enabled = false;
    else
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.CatchCritterRoutine());
  }

  public override void OnBecomeCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeCurrent(playerFarming);
    this.CritterRat.IsCurrent = true;
  }

  public override void OnBecomeNotCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeNotCurrent(playerFarming);
    this.CritterRat.IsCurrent = false;
  }

  public IEnumerator CatchCritterRoutine()
  {
    Interaction_CatchRat interactionCatchRat = this;
    AudioManager.Instance.PlayOneShot("event:/player/weed_pick", interactionCatchRat.transform.position);
    interactionCatchRat.Activating = true;
    interactionCatchRat.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    interactionCatchRat.playerFarming.Spine.AnimationState.SetAnimation(0, "catch-critter", false);
    interactionCatchRat.playerFarming.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    interactionCatchRat.state.facingAngle = Utils.GetAngle(interactionCatchRat.state.transform.position, interactionCatchRat.transform.position);
    interactionCatchRat.transform.DOMove(interactionCatchRat.playerFarming.transform.position, 1f);
    yield return (object) new WaitForSeconds(0.2f);
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/spider_small/gethit", interactionCatchRat.transform.position);
    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.MEAT_MORSEL, 1, interactionCatchRat.transform.position);
    interactionCatchRat.gameObject.SetActive(false);
    yield return (object) new WaitForSeconds(0.4f);
    interactionCatchRat.state.CURRENT_STATE = StateMachine.State.Idle;
    interactionCatchRat.gameObject.Recycle();
    interactionCatchRat.Activating = false;
  }

  public IEnumerator Delay(float delay, System.Action callback)
  {
    yield return (object) new WaitForSeconds(delay);
    System.Action action = callback;
    if (action != null)
      action();
  }
}
