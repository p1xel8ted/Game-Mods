// Decompiled with JetBrains decompiler
// Type: Interaction_TarotCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_TarotCard : Interaction
{
  private string sSummonTrinket;
  private string sNoAvailableTrinkets;
  public bool Activated;
  public SpriteRenderer sprite;
  private PlayerFarming pFarming;
  private CameraFollowTarget c;
  private int devotionCost;
  public Transform SpawnPosition;
  private TarotCards.TarotCard DrawnCard;

  public TarotCards.Card CardOverride { get; set; } = TarotCards.Card.Count;

  public bool ForceAllow { get; set; }

  private void Start()
  {
    this.UpdateLocalisation();
    if ((DataManager.Instance.PlayerFleece != 4 || this.ForceAllow) && TarotCards.DrawRandomCard() != null)
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

  private IEnumerator CentrePlayer()
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

  private void DoRoutine() => this.StartCoroutine((IEnumerator) this.DoRoutineRoutine());

  private IEnumerator DoRoutineRoutine()
  {
    Interaction_TarotCard interactionTarotCard = this;
    Time.timeScale = 0.0f;
    HUD_Manager.Instance.Hide(false, 0);
    // ISSUE: reference to a compiler-generated method
    interactionTarotCard.transform.DOScale(0.0f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(interactionTarotCard.\u003CDoRoutineRoutine\u003Eb__23_0)).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    GameManager.GetInstance().CameraSetTargetZoom(4f);
    LetterBox.Show(false);
    interactionTarotCard.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    interactionTarotCard.state.facingAngle = -90f;
    interactionTarotCard.c = CameraFollowTarget.Instance;
    interactionTarotCard.c.DisablePlayerLook = true;
    interactionTarotCard.pFarming = interactionTarotCard.state.GetComponent<PlayerFarming>();
    interactionTarotCard.pFarming.SpineUseDeltaTime(false);
    AudioManager.Instance.PlayOneShot("event:/tarot/tarot_card_pull", interactionTarotCard.gameObject);
    interactionTarotCard.pFarming.simpleSpineAnimator.Animate("cards/cards-start", 0, false);
    interactionTarotCard.pFarming.simpleSpineAnimator.AddAnimate("cards/cards-loop", 0, true, 0.0f);
    if (interactionTarotCard.CardOverride == TarotCards.Card.Count)
    {
      interactionTarotCard.DrawnCard = TarotCards.DrawRandomCard();
    }
    else
    {
      DataManager.UnlockTrinket(interactionTarotCard.CardOverride);
      NotificationCentre.Instance.PlayGenericNotification("UI/TarotMenu/NewTarotCardUnlocked");
      interactionTarotCard.DrawnCard.CardType = interactionTarotCard.CardOverride;
    }
    float t = 0.0f;
    while ((double) t < 1.0)
    {
      t += Time.unscaledDeltaTime;
      Time.timeScale = Mathf.Lerp(1f, 0.0f, t / 1f);
      yield return (object) null;
    }
    GameManager.GetInstance().CameraSetTargetZoom(6f);
    UITrinketCards.Play(interactionTarotCard.DrawnCard, new System.Action(interactionTarotCard.BackToIdle), 0.0f);
    for (int index = 0; index < interactionTarotCard.transform.childCount; ++index)
      interactionTarotCard.transform.GetChild(index).gameObject.SetActive(false);
  }

  private void BackToIdle() => this.StartCoroutine((IEnumerator) this.BackToIdleRoutine());

  private IEnumerator BackToIdleRoutine()
  {
    Interaction_TarotCard interactionTarotCard = this;
    Time.timeScale = 1f;
    LetterBox.Hide();
    HUD_Manager.Instance.Show(0);
    AudioManager.Instance.PlayOneShot("event:/tarot/tarot_card_close", interactionTarotCard.gameObject);
    GameManager.GetInstance().CameraResetTargetZoom();
    CameraFollowTarget.Instance.DisablePlayerLook = false;
    yield return (object) null;
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "cards/cards-stop-seperate", false);
    PlayerFarming.Instance.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
    PlayerFarming.Instance.Spine.UseDeltaTime = true;
    GameManager.GetInstance().StartCoroutine((IEnumerator) interactionTarotCard.DelayEffectsRoutine());
  }

  private IEnumerator DelayEffectsRoutine()
  {
    Interaction_TarotCard interactionTarotCard = this;
    yield return (object) new WaitForSeconds(0.2f);
    TrinketManager.AddTrinket(interactionTarotCard.DrawnCard);
    if ((UnityEngine.Object) interactionTarotCard.gameObject != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) interactionTarotCard.gameObject);
  }
}
