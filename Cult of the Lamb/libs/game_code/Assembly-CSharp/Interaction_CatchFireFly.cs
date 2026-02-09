// Decompiled with JetBrains decompiler
// Type: Interaction_CatchFireFly
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_CatchFireFly : Interaction
{
  public const int AmountForSkin = 15;
  public CritterBee CritterBee;
  public LayerMask collisionMask;
  public string sString;
  public bool Activating;
  public int maxRange = 4;

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.ActivateDistance = 1f;
    this.CritterBee.enabled = true;
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

  public void Start()
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

  public IEnumerator CatchCritterRoutine()
  {
    Interaction_CatchFireFly interactionCatchFireFly = this;
    ++DataManager.Instance.TotalFirefliesCaught;
    if (DataManager.Instance.TotalFirefliesCaught >= 15 && !DataManager.Instance.FollowerSkinsUnlocked.Contains("Butterfly"))
    {
      interactionCatchFireFly.StartCoroutine((IEnumerator) interactionCatchFireFly.UnlockSkinIE());
    }
    else
    {
      interactionCatchFireFly.Activating = true;
      interactionCatchFireFly.CritterBee.enabled = false;
      interactionCatchFireFly.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      interactionCatchFireFly.playerFarming.Spine.AnimationState.SetAnimation(0, "catch-critter", false);
      interactionCatchFireFly.playerFarming.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
      interactionCatchFireFly.state.facingAngle = Utils.GetAngle(interactionCatchFireFly.state.transform.position, interactionCatchFireFly.transform.position);
      interactionCatchFireFly.transform.DOMove(interactionCatchFireFly.playerFarming.CameraBone.transform.position, 0.5f);
      AudioManager.Instance.PlayOneShot("event:/player/weed_pick", interactionCatchFireFly.transform.position);
      yield return (object) new WaitForSeconds(0.2f);
      AudioManager.Instance.PlayOneShot("event:/player/catch_firefly", interactionCatchFireFly.transform.position);
      interactionCatchFireFly.gameObject.SetActive(false);
      if (DataManager.Instance.HasBuiltShrine1)
      {
        for (int i = 0; i < UnityEngine.Random.Range(1, interactionCatchFireFly.maxRange); ++i)
        {
          yield return (object) new WaitForSeconds(0.05f);
          if ((GameManager.HasUnlockAvailable() ? 1 : (DataManager.Instance.DeathCatBeaten ? 1 : 0)) != 0)
            SoulCustomTarget.Create(interactionCatchFireFly.state.gameObject, interactionCatchFireFly.CritterBee.spriteRenderer.transform.position, Color.white, new System.Action(interactionCatchFireFly.\u003CCatchCritterRoutine\u003Eb__11_0));
          else
            InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, interactionCatchFireFly.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(8f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
        }
      }
      yield return (object) new WaitForSeconds(0.3f);
      interactionCatchFireFly.state.CURRENT_STATE = StateMachine.State.Idle;
      interactionCatchFireFly.gameObject.Recycle();
    }
  }

  public IEnumerator UnlockSkinIE()
  {
    Interaction_CatchFireFly interactionCatchFireFly = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionCatchFireFly.gameObject, 4f);
    Vector3 TargetPosition;
    if ((double) interactionCatchFireFly.playerFarming.gameObject.transform.position.x < (double) interactionCatchFireFly.transform.position.x)
    {
      float distance = Vector3.Distance(interactionCatchFireFly.transform.position, interactionCatchFireFly.transform.position + Vector3.left);
      Vector3 normalized = (interactionCatchFireFly.transform.position + Vector3.left - interactionCatchFireFly.transform.position).normalized;
      TargetPosition = (UnityEngine.Object) Physics2D.Raycast((Vector2) interactionCatchFireFly.transform.position, (Vector2) normalized, distance, (int) interactionCatchFireFly.collisionMask).collider != (UnityEngine.Object) null ? interactionCatchFireFly.transform.position + Vector3.right : interactionCatchFireFly.transform.position + Vector3.left;
    }
    else
    {
      float distance = Vector3.Distance(interactionCatchFireFly.transform.position, interactionCatchFireFly.transform.position + Vector3.right);
      Vector3 normalized = (interactionCatchFireFly.transform.position + Vector3.right - interactionCatchFireFly.transform.position).normalized;
      TargetPosition = (UnityEngine.Object) Physics2D.Raycast((Vector2) interactionCatchFireFly.transform.position, (Vector2) normalized, distance, (int) interactionCatchFireFly.collisionMask).collider != (UnityEngine.Object) null ? interactionCatchFireFly.transform.position + Vector3.left : interactionCatchFireFly.transform.position + Vector3.right;
    }
    interactionCatchFireFly.playerFarming.playerController.speed = 0.0f;
    interactionCatchFireFly.playerFarming.GoToAndStop(TargetPosition, interactionCatchFireFly.gameObject);
    AudioManager.Instance.PlayOneShot("event:/Stings/upgrade_cult", interactionCatchFireFly.transform.position);
    interactionCatchFireFly.CritterBee.enabled = false;
    Vector3 pos = interactionCatchFireFly.transform.GetChild(0).transform.localPosition;
    float dur = 3f;
    float t = 0.0f;
    while ((double) t < (double) dur)
    {
      t += Time.deltaTime;
      interactionCatchFireFly.transform.GetChild(0).transform.localPosition = pos + UnityEngine.Random.insideUnitSphere * (t / dur) * 0.05f;
      yield return (object) null;
    }
    interactionCatchFireFly.gameObject.SetActive(false);
    BiomeConstants.Instance.EmitSmokeInteractionVFX(interactionCatchFireFly.transform.GetChild(0).transform.position + Vector3.down * 0.25f, Vector3.one);
    AudioManager.Instance.PlayOneShot("event:/player/catch_firefly", interactionCatchFireFly.transform.position);
    FollowerSkinCustomTarget.Create(interactionCatchFireFly.transform.GetChild(0).transform.position - Vector3.back * 0.5f, interactionCatchFireFly.playerFarming.transform.position, 1f, "Butterfly", new System.Action(interactionCatchFireFly.\u003CUnlockSkinIE\u003Eb__12_0));
  }

  [CompilerGenerated]
  public void \u003CCatchCritterRoutine\u003Eb__11_0() => this.playerFarming.GetSoul(1);

  [CompilerGenerated]
  public void \u003CUnlockSkinIE\u003Eb__12_0()
  {
    GameManager.GetInstance().OnConversationEnd();
    this.gameObject.Recycle();
  }
}
