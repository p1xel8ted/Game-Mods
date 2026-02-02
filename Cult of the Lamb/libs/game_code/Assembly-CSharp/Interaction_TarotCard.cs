// Decompiled with JetBrains decompiler
// Type: Interaction_TarotCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_TarotCard : Interaction
{
  public string sSummonTrinket;
  public string sNoAvailableTrinkets;
  public bool Activated;
  public SpriteRenderer sprite;
  public CameraFollowTarget c;
  public int devotionCost;
  [CompilerGenerated]
  public TarotCards.Card \u003CCardOverride\u003Ek__BackingField = TarotCards.Card.Count;
  [CompilerGenerated]
  public bool \u003CForceAllow\u003Ek__BackingField;
  public Transform SpawnPosition;
  public TarotCards.TarotCard DrawnCard;

  public TarotCards.Card CardOverride
  {
    get => this.\u003CCardOverride\u003Ek__BackingField;
    set => this.\u003CCardOverride\u003Ek__BackingField = value;
  }

  public bool ForceAllow
  {
    get => this.\u003CForceAllow\u003Ek__BackingField;
    set => this.\u003CForceAllow\u003Ek__BackingField = value;
  }

  public void Start()
  {
    this.UpdateLocalisation();
    if ((!PlayerFleeceManager.FleecePreventTarotCards() || this.ForceAllow) && TarotCards.DrawRandomCard(this.playerFarming, false) != null)
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    if ((!PlayerFleeceManager.FleecePreventTarotCards() || this.ForceAllow) && TarotCards.DrawRandomCard(this.playerFarming, false) != null)
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sSummonTrinket = ScriptLocalization.Interactions.PickUp;
  }

  public override void GetLabel()
  {
    this.Interactable = true;
    this.Label = this.Activated ? "" : this.sSummonTrinket;
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.Activated)
      return;
    base.OnInteract(state);
    this.Activated = true;
    state.CURRENT_STATE = StateMachine.State.InActive;
    this.DoRoutine();
  }

  public IEnumerator CentrePlayer()
  {
    Interaction_TarotCard interactionTarotCard = this;
    float Progress = 0.0f;
    Vector3 StartPosition = interactionTarotCard.state.transform.position;
    while ((double) (Progress += Time.deltaTime * 2f) < 0.5)
    {
      interactionTarotCard.state.transform.position = Vector3.Lerp(StartPosition, interactionTarotCard.transform.position + Vector3.down, Mathf.SmoothStep(0.0f, 1f, Progress));
      yield return (object) null;
    }
  }

  public void DoRoutine() => this.StartCoroutine((IEnumerator) this.DoRoutineRoutine());

  public IEnumerator DoRoutineRoutine()
  {
    Interaction_TarotCard interactionTarotCard = this;
    Time.timeScale = 0.0f;
    foreach (Health health in Health.team2)
    {
      if ((UnityEngine.Object) health != (UnityEngine.Object) null)
        health.AddFreezeTime();
    }
    Health.isGlobalTimeFreeze = true;
    HUD_Manager.Instance.Hide(false, 0);
    interactionTarotCard.transform.DOScale(0.0f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(interactionTarotCard.\u003CDoRoutineRoutine\u003Eb__23_0)).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    GameManager.GetInstance().CameraSetTargetZoom(4f);
    LetterBox.Show(false);
    interactionTarotCard.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    interactionTarotCard.state.facingAngle = -90f;
    interactionTarotCard.c = CameraFollowTarget.Instance;
    interactionTarotCard.c.DisablePlayerLook = true;
    interactionTarotCard.playerFarming.SpineUseDeltaTime(false);
    AudioManager.Instance.PlayOneShot("event:/tarot/tarot_card_pull", interactionTarotCard.gameObject);
    interactionTarotCard.playerFarming.simpleSpineAnimator.Animate("cards/cards-start", 0, false);
    interactionTarotCard.playerFarming.simpleSpineAnimator.AddAnimate("cards/cards-loop", 0, true, 0.0f);
    if (interactionTarotCard.CardOverride == TarotCards.Card.Count)
    {
      interactionTarotCard.DrawnCard = TarotCards.DrawRandomCard(interactionTarotCard.playerFarming, false);
    }
    else
    {
      DataManager.UnlockTrinket(interactionTarotCard.CardOverride);
      NotificationCentre.Instance.PlayGenericNotification("UI/TarotMenu/NewTarotCardUnlocked");
      interactionTarotCard.DrawnCard.CardType = interactionTarotCard.CardOverride;
    }
    TrinketManager.AddEncounteredTrinket(interactionTarotCard.DrawnCard, interactionTarotCard.playerFarming);
    float t = 0.0f;
    while ((double) t < 0.15000000596046448)
    {
      t += Time.unscaledDeltaTime;
      Time.timeScale = Mathf.Lerp(0.15f, 0.0f, t / 0.15f);
      yield return (object) null;
    }
    GameManager.GetInstance().CameraSetTargetZoom(6f);
    UITarotPickUpOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowTarotPickUp(interactionTarotCard.DrawnCard);
    overlayController.OnHide = overlayController.OnHide + new System.Action(interactionTarotCard.BackToIdle);
    for (int index = 0; index < interactionTarotCard.transform.childCount; ++index)
      interactionTarotCard.transform.GetChild(index).gameObject.SetActive(false);
  }

  public void BackToIdle()
  {
    this.StartCoroutine((IEnumerator) this.BackToIdleRoutine(this.playerFarming));
  }

  public IEnumerator BackToIdleRoutine(PlayerFarming playerFarming)
  {
    Interaction_TarotCard interactionTarotCard = this;
    Time.timeScale = 1f;
    LetterBox.Hide();
    HUD_Manager.Instance.Show(0);
    AudioManager.Instance.PlayOneShot("event:/tarot/tarot_card_close", interactionTarotCard.gameObject);
    GameManager.GetInstance().CameraResetTargetZoom();
    CameraFollowTarget.Instance.DisablePlayerLook = false;
    yield return (object) null;
    playerFarming.Spine.AnimationState.SetAnimation(0, "cards/cards-stop-seperate", false);
    playerFarming.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    yield return (object) new WaitForSeconds(0.25f);
    foreach (Health health in Health.team2)
    {
      if ((UnityEngine.Object) health != (UnityEngine.Object) null)
        health.ClearFreezeTime();
    }
    Health.isGlobalTimeFreeze = false;
    PlayerFarming.SetStateForAllPlayers();
    playerFarming.Spine.UseDeltaTime = true;
    GameManager.GetInstance().StartCoroutine((IEnumerator) interactionTarotCard.DelayEffectsRoutine(playerFarming));
  }

  public IEnumerator DelayEffectsRoutine(PlayerFarming playerFarming)
  {
    Interaction_TarotCard interactionTarotCard = this;
    yield return (object) new WaitForSeconds(0.2f);
    TrinketManager.AddTrinket(interactionTarotCard.DrawnCard, playerFarming);
    if ((UnityEngine.Object) interactionTarotCard.gameObject != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) interactionTarotCard.gameObject);
  }

  [CompilerGenerated]
  public void \u003CDoRoutineRoutine\u003Eb__23_0()
  {
    for (int index = 0; index < this.transform.childCount; ++index)
      this.transform.GetChild(index).gameObject.SetActive(false);
  }
}
