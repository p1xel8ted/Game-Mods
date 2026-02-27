// Decompiled with JetBrains decompiler
// Type: Interaction_Tarot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_Tarot : Interaction
{
  private string sSummonTrinket;
  public bool Activated;
  public SimpleBark simpleBarkBeforeCard;
  public SimpleBark simpleBarkAfterCard;
  public Interaction_SimpleConversation conversation;
  public GameObject tarotcards;
  private PlayerFarming pFarming;
  private CameraFollowTarget c;
  private int devotionCost;
  private bool _playedSFX;
  public Sprite TarotCardSprite;
  public Transform SpawnPosition;

  private void Start()
  {
    this.UpdateLocalisation();
    this.simpleBarkBeforeCard.enabled = DataManager.Instance.HasEncounteredTarot;
    this.simpleBarkAfterCard.enabled = false;
    this.enabled = DataManager.Instance.HasEncounteredTarot;
    this.conversation.enabled = !DataManager.Instance.HasEncounteredTarot;
    this._playedSFX = false;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sSummonTrinket = ScriptLocalization.Interactions.TakeTrinket;
  }

  public override void OnEnableInteraction()
  {
    if ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      return;
    base.OnEnableInteraction();
    if (this._playedSFX)
      return;
    AudioManager.Instance.PlayOneShot("event:/Stings/tarot_room", this.transform.position);
    this._playedSFX = true;
  }

  public override void GetLabel()
  {
    this.Interactable = true;
    this.Label = this.Activated ? "" : this.sSummonTrinket;
  }

  public void FinishedConversation() => this.StartCoroutine((IEnumerator) this.GiveIntroTarots());

  public override void OnInteract(StateMachine state)
  {
    if (this.Activated)
      return;
    this.simpleBarkBeforeCard.Close();
    this.simpleBarkBeforeCard.enabled = false;
    base.OnInteract(state);
    this.Activated = true;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.InActive;
    this.DoRoutine();
  }

  private IEnumerator CentrePlayer()
  {
    Interaction_Tarot interactionTarot = this;
    float Progress = 0.0f;
    Vector3 StartPosition = PlayerFarming.Instance.state.transform.position;
    while ((double) (Progress += Time.deltaTime * 2f) < 0.5)
    {
      PlayerFarming.Instance.state.transform.position = Vector3.Lerp(StartPosition, interactionTarot.transform.position + Vector3.down, Mathf.SmoothStep(0.0f, 1f, Progress));
      yield return (object) null;
    }
  }

  private void DoRoutine() => this.StartCoroutine((IEnumerator) this.DoRoutineRoutine());

  private TarotCards.TarotCard GetCard()
  {
    TarotCards.TarotCard card;
    if (!DataManager.Instance.FirstTarot && TarotCards.DrawRandomCard() != null)
    {
      DataManager.Instance.FirstTarot = true;
      card = new TarotCards.TarotCard(TarotCards.Card.Lovers1, 0);
    }
    else
      card = TarotCards.DrawRandomCard();
    if (card != null)
      DataManager.Instance.PlayerRunTrinkets.Add(card);
    return card;
  }

  private IEnumerator GiveIntroTarots()
  {
    DataManager.Instance.HasEncounteredTarot = true;
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 4f);
    this.DoRoutine();
  }

  private IEnumerator DoRoutineRoutine()
  {
    Interaction_Tarot interactionTarot = this;
    HUD_Manager.Instance.Hide(false, 0);
    GameManager.GetInstance().CameraSetTargetZoom(4f);
    LetterBox.Show(false);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.state.facingAngle = -90f;
    interactionTarot.c = CameraFollowTarget.Instance;
    interactionTarot.c.DisablePlayerLook = true;
    interactionTarot.pFarming = PlayerFarming.Instance;
    interactionTarot.StartCoroutine((IEnumerator) interactionTarot.CentrePlayer());
    AudioManager.Instance.PlayOneShot("event:/tarot/tarot_card_pull", interactionTarot.gameObject);
    interactionTarot.pFarming.simpleSpineAnimator.Animate("cards/cards-start", 0, false);
    interactionTarot.pFarming.simpleSpineAnimator.AddAnimate("cards/cards-loop", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(0.1f);
    GameManager.GetInstance().CameraSetTargetZoom(6f);
    if ((UnityEngine.Object) interactionTarot.tarotcards != (UnityEngine.Object) null)
      interactionTarot.tarotcards.SetActive(false);
    TarotCards.TarotCard card1 = interactionTarot.GetCard();
    TarotCards.TarotCard card2 = interactionTarot.GetCard();
    if (card1 != null && card2 != null)
    {
      UITarotChoiceOverlayController tarotChoiceOverlayInstance = MonoSingleton<UIManager>.Instance.ShowTarotChoice(card1, card2);
      tarotChoiceOverlayInstance.OnTarotCardSelected += (System.Action<TarotCards.TarotCard>) (card =>
      {
        this.StartCoroutine((IEnumerator) this.BackToIdleRoutine(card, 0.0f));
        DataManager.Instance.PlayerRunTrinkets.Remove(GetOther(card));
      });
      UITarotChoiceOverlayController overlayController = tarotChoiceOverlayInstance;
      overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() => tarotChoiceOverlayInstance = (UITarotChoiceOverlayController) null);
    }
    else if (card1 != null || card2 != null)
    {
      if (card1 != null)
        UITrinketCards.Play(card1, (System.Action) (() => this.StartCoroutine((IEnumerator) this.BackToIdleRoutine(card1, 0.0f))));
      else if (card2 != null)
        UITrinketCards.Play(card2, (System.Action) (() => this.StartCoroutine((IEnumerator) this.BackToIdleRoutine(card2, 0.0f))));
    }
    else
    {
      int i = -1;
      while (++i <= 25)
      {
        AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", interactionTarot.gameObject);
        CameraManager.shakeCamera(UnityEngine.Random.Range(0.4f, 0.6f));
        PickUp pickUp = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, interactionTarot.transform.position + Vector3.back, 0.0f);
        pickUp.SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
        pickUp.MagnetDistance = 3f;
        pickUp.CanStopFollowingPlayer = false;
        yield return (object) new WaitForSeconds(0.01f);
      }
      yield return (object) new WaitForSeconds(1f);
      interactionTarot.StartCoroutine((IEnumerator) interactionTarot.BackToIdleRoutine((TarotCards.TarotCard) null, 0.0f));
    }

    TarotCards.TarotCard GetOther(TarotCards.TarotCard card) => card == card1 ? card2 : card1;
  }

  private IEnumerator BackToIdleRoutine(TarotCards.TarotCard card, float delay)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Interaction_Tarot interactionTarot = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      interactionTarot.pFarming.simpleSpineAnimator.Animate("cards/cards-stop-seperate", 0, false);
      interactionTarot.pFarming.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
      interactionTarot.simpleBarkAfterCard.enabled = true;
      interactionTarot.StopAllCoroutines();
      GameManager.GetInstance().StartCoroutine((IEnumerator) interactionTarot.DelayEffectsRoutine(card, delay));
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    LetterBox.Hide();
    HUD_Manager.Instance.Show(0);
    AudioManager.Instance.PlayOneShot("event:/tarot/tarot_card_close", interactionTarot.gameObject);
    GameManager.GetInstance().CameraResetTargetZoom();
    interactionTarot.c.DisablePlayerLook = false;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private IEnumerator DelayEffectsRoutine(TarotCards.TarotCard card, float delay)
  {
    yield return (object) new WaitForSeconds(0.2f + delay);
    if (card != null)
      TrinketManager.AddTrinket(card);
  }
}
