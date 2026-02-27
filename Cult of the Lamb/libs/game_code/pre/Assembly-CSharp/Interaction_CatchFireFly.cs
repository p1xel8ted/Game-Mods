// Decompiled with JetBrains decompiler
// Type: Interaction_CatchFireFly
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_CatchFireFly : Interaction
{
  public CritterBee CritterBee;
  private LayerMask collisionMask;
  private string sString;
  private bool Activating;
  private int maxRange = 4;

  public void OnValidate() => this.CritterBee.IsFireFly = true;

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.ActivateDistance = 1f;
    if (UnityEngine.Random.Range(1, 20) == 15)
    {
      this.maxRange = 10;
      this.transform.localScale = Vector3.one * 1.5f;
    }
    else
    {
      this.maxRange = 4;
      this.transform.localScale = Vector3.one;
    }
  }

  private void Start()
  {
    this.UpdateLocalisation();
    this.collisionMask = (LayerMask) ((int) this.collisionMask | 1 << LayerMask.NameToLayer("Island"));
    this.collisionMask = (LayerMask) ((int) this.collisionMask | 1 << LayerMask.NameToLayer("Obstacles"));
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = ScriptLocalization.Interactions.CatchCritter;
  }

  public override void GetLabel() => this.Label = this.Activating ? "" : this.sString;

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.CatchCritterRoutine());
  }

  private IEnumerator CatchCritterRoutine()
  {
    Interaction_CatchFireFly interactionCatchFireFly = this;
    ++DataManager.Instance.TotalFirefliesCaught;
    interactionCatchFireFly.Activating = true;
    interactionCatchFireFly.CritterBee.enabled = false;
    interactionCatchFireFly.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "catch-critter", false);
    PlayerFarming.Instance.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    interactionCatchFireFly.state.facingAngle = Utils.GetAngle(interactionCatchFireFly.state.transform.position, interactionCatchFireFly.transform.position);
    interactionCatchFireFly.transform.DOMove(PlayerFarming.Instance.CameraBone.transform.position, 0.5f);
    AudioManager.Instance.PlayOneShot("event:/player/weed_pick", interactionCatchFireFly.transform.position);
    yield return (object) new WaitForSeconds(0.2f);
    AudioManager.Instance.PlayOneShot("event:/player/catch_firefly", interactionCatchFireFly.transform.position);
    interactionCatchFireFly.gameObject.SetActive(false);
    for (int i = 0; i < UnityEngine.Random.Range(1, interactionCatchFireFly.maxRange); ++i)
    {
      yield return (object) new WaitForSeconds(0.05f);
      if (GameManager.HasUnlockAvailable())
        SoulCustomTarget.Create(interactionCatchFireFly.state.gameObject, interactionCatchFireFly.CritterBee.spriteRenderer.transform.position, Color.white, (System.Action) (() => PlayerFarming.Instance.GetSoul(1)));
      else
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, interactionCatchFireFly.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(8f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
    }
    yield return (object) new WaitForSeconds(0.3f);
    interactionCatchFireFly.state.CURRENT_STATE = StateMachine.State.Idle;
    interactionCatchFireFly.gameObject.Recycle();
  }
}
