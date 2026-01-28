// Decompiled with JetBrains decompiler
// Type: Interaction_Tarot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using src.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_Tarot : Interaction
{
  public string sSummonTrinket;
  public bool Activated;
  public SimpleBark simpleBarkBeforeCard;
  public SimpleBark simpleBarkAfterCard;
  public Interaction_SimpleConversation conversation;
  public Interaction_SimpleConversation winterConversation;
  public Interaction_SimpleConversation rotConversation;
  public GameObject tarotcards;
  public Interaction_SimpleConversation relicRiddle;
  public Interaction_SimpleConversation introGiveDeck;
  public CameraFollowTarget c;
  public int devotionCost;
  public bool _playedSFX;
  public UITarotChoiceOverlayController tarotChoiceOverlayInstance;
  public Coroutine giveTarotRoutine;
  public Sprite TarotCardSprite;
  public Transform SpawnPosition;

  public override void OnDisable()
  {
    base.OnDisable();
    if (this.giveTarotRoutine == null)
      return;
    this.tarotChoiceOverlayInstance.Hide(true);
    LetterBox.Hide();
    HUD_Manager.Instance.Show(0);
    GameManager.GetInstance().RemoveAllFromCamera();
    GameManager.GetInstance().CameraResetTargetZoom();
    GameManager.GetInstance().AddPlayerToCamera();
    this.c.DisablePlayerLook = false;
    this.Activated = false;
    PlayerFarming.SetStateForAllPlayers();
    this.tarotChoiceOverlayInstance = (UITarotChoiceOverlayController) null;
    this.giveTarotRoutine = (Coroutine) null;
  }

  public void Start()
  {
    this.UpdateLocalisation();
    this.simpleBarkBeforeCard.enabled = DataManager.Instance.HasEncounteredTarot || DungeonSandboxManager.Active;
    this.simpleBarkAfterCard.enabled = false;
    bool flag1 = (UnityEngine.Object) this.winterConversation != (UnityEngine.Object) null && DataManager.Instance.CurrentSeason == SeasonsManager.Season.Winter && PlayerFarming.Location == FollowerLocation.Dungeon1_5 && !DataManager.Instance.SpokenToClauneckWinter;
    bool flag2 = (UnityEngine.Object) this.rotConversation != (UnityEngine.Object) null && PlayerFarming.Location == FollowerLocation.Dungeon1_6 && !DataManager.Instance.SpokenToClauneckRot;
    this.enabled = !flag2 && !flag1 && DataManager.Instance.HasEncounteredTarot || DungeonSandboxManager.Active;
    if (DataManager.Instance.GivenRelicLighthouseRiddle)
      this.SetRelicRiddleManual();
    else if (DataManager.Instance.OnboardedRelics && !DungeonSandboxManager.Active)
      this.relicRiddle.OnInteraction += (Interaction.InteractionEvent) (state => ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/ClauneckRelic", Objectives.CustomQuestTypes.FindClauneckRelic), true));
    if (DataManager.Instance.BossesCompleted.Count < 2 && (UnityEngine.Object) this.relicRiddle != (UnityEngine.Object) null)
      this.relicRiddle.gameObject.SetActive(false);
    Debug.Log((object) "Setting up Clauneck Conversations!");
    if (flag1 | flag2)
    {
      Debug.Log((object) $"Configuring Clauneck for winter/rot intro! Winter: {flag1}, Rot: {flag2}");
      this.winterConversation.gameObject.SetActive(flag1);
      this.rotConversation.gameObject.SetActive(flag2);
      this.simpleBarkBeforeCard.enabled = false;
      this.conversation.enabled = false;
    }
    else
      this.conversation.enabled = !DataManager.Instance.HasEncounteredTarot && !DungeonSandboxManager.Active;
    this.introGiveDeck.enabled = false;
    this._playedSFX = false;
  }

  public void SetRelicRiddleManual()
  {
    this.relicRiddle.ActivateDistance = 1.25f;
    this.relicRiddle.AutomaticallyInteract = false;
    GameManager.GetInstance().WaitForSeconds(1f, (System.Action) (() =>
    {
      this.relicRiddle.Finished = false;
      this.relicRiddle.Spoken = false;
    }));
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sSummonTrinket = ScriptLocalization.Interactions.TakeTrinket;
  }

  public override void OnEnableInteraction()
  {
    if ((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null)
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

  public void FinishedWinterConversation()
  {
    this.enabled = true;
    this.simpleBarkBeforeCard.enabled = DataManager.Instance.HasEncounteredTarot || DungeonSandboxManager.Active;
  }

  public void TarotCardsGiven() => this.StartCoroutine((IEnumerator) this.GiveIntroTarotsDeck());

  public IEnumerator GiveIntroTarotsDeck()
  {
    Interaction_Tarot interactionTarot = this;
    DataManager.Instance.HasEncounteredTarot = true;
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().OnConversationNew();
    UITarotCardsMenuController menu = MonoSingleton<UIManager>.Instance.TarotCardsMenuTemplate.Instantiate<UITarotCardsMenuController>();
    List<TarotCards.Card> list = ((IEnumerable<TarotCards.Card>) TarotCards.DefaultCards).ToList<TarotCards.Card>();
    if (CoopManager.CoopActive)
      list.AddRange((IEnumerable<TarotCards.Card>) TarotCards.CoopCards);
    menu.Show(list.ToArray());
    yield return (object) menu.YieldUntilHidden();
    if (CoopManager.CoopActive)
      DataManager.Instance.UnlockedCoopTarots = true;
    LetterBox.Hide();
    HUD_Manager.Instance.Show(0);
    GameManager.GetInstance().CameraResetTargetZoom();
    GameManager.GetInstance().AddPlayerToCamera();
    interactionTarot.c.DisablePlayerLook = false;
    PlayerFarming.SetStateForAllPlayers();
    interactionTarot.playerFarming.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    interactionTarot.StopAllCoroutines();
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.Activated)
      return;
    base.OnInteract(state);
    this.simpleBarkBeforeCard.Close();
    this.simpleBarkBeforeCard.enabled = false;
    this.Activated = true;
    PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive);
    this.DoRoutine();
  }

  public IEnumerator CentrePlayer()
  {
    Interaction_Tarot interactionTarot = this;
    float Progress = 0.0f;
    Vector3 StartPosition = interactionTarot.playerFarming.state.transform.position;
    while ((double) (Progress += Time.deltaTime * 2f) < 0.5)
    {
      interactionTarot.playerFarming.state.transform.position = Vector3.Lerp(StartPosition, interactionTarot.transform.position + Vector3.down, Mathf.SmoothStep(0.0f, 1f, Progress));
      yield return (object) null;
    }
  }

  public void DoRoutine()
  {
    this.giveTarotRoutine = this.StartCoroutine((IEnumerator) this.DoRoutineRoutine());
  }

  public TarotCards.TarotCard GetCard(bool canBeCorrupted)
  {
    TarotCards.TarotCard card;
    if (!DataManager.Instance.FirstTarot && TarotCards.DrawRandomCard(this.playerFarming) != null && !DungeonSandboxManager.Active)
    {
      DataManager.Instance.FirstTarot = true;
      card = new TarotCards.TarotCard(TarotCards.Card.Lovers1, 0);
    }
    else
    {
      card = TarotCards.DrawRandomCard(this.playerFarming, canBeCorrupted);
      if (card != null && card.CardType == TarotCards.Card.Spider && EquipmentManager.IsPoisonWeapon(this.playerFarming.currentWeapon))
        card = TarotCards.DrawRandomCard(this.playerFarming, canBeCorrupted);
    }
    if (card != null)
      TrinketManager.AddEncounteredTrinket(card, this.playerFarming);
    return card;
  }

  public IEnumerator GiveIntroTarots()
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
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(interactionTarot.playerFarming.gameObject, 4f);
      interactionTarot.DoRoutine();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForEndOfFrame();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator DoRoutineRoutine()
  {
    Interaction_Tarot interactionTarot = this;
    HUD_Manager.Instance.Hide(false, 0);
    GameManager.GetInstance().CameraSetTargetZoom(4f);
    LetterBox.Show(false);
    interactionTarot.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    interactionTarot.playerFarming.state.facingAngle = -90f;
    interactionTarot.c = CameraFollowTarget.Instance;
    interactionTarot.c.DisablePlayerLook = true;
    interactionTarot.StartCoroutine((IEnumerator) interactionTarot.CentrePlayer());
    AudioManager.Instance.PlayOneShot("event:/tarot/tarot_card_pull", interactionTarot.gameObject);
    interactionTarot.playerFarming.simpleSpineAnimator.Animate("cards/cards-start", 0, false);
    interactionTarot.playerFarming.simpleSpineAnimator.AddAnimate("cards/cards-loop", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(0.1f);
    GameManager.GetInstance().CameraSetTargetZoom(6f);
    if ((UnityEngine.Object) interactionTarot.tarotcards != (UnityEngine.Object) null)
      interactionTarot.tarotcards.SetActive(false);
    TarotCards.TarotCard card1 = interactionTarot.GetCard(true);
    TarotCards.TarotCard card2 = interactionTarot.GetCard(card1 == null || !TarotCards.CorruptedCards.Contains<TarotCards.Card>(card1.CardType));
    if (card1 != null && card2 != null)
    {
      interactionTarot.tarotChoiceOverlayInstance = MonoSingleton<UIManager>.Instance.ShowTarotChoice(card1, card2);
      interactionTarot.tarotChoiceOverlayInstance.OnTarotCardSelected += (System.Action<TarotCards.TarotCard>) (card =>
      {
        TarotCards.TarotCard card3 = card1;
        if (card == card1)
          card3 = card2;
        if (CoopManager.CoopActive)
          GameManager.GetInstance().StartCoroutine((IEnumerator) this.DelayEffectsRoutine(card3, 0.0f, this.playerFarming.isLamb ? PlayerFarming.players[1] : PlayerFarming.players[0]));
        this.StartCoroutine((IEnumerator) this.BackToIdleRoutine(card, 0.0f));
      });
      UITarotChoiceOverlayController choiceOverlayInstance = interactionTarot.tarotChoiceOverlayInstance;
      choiceOverlayInstance.OnHidden = choiceOverlayInstance.OnHidden + (System.Action) (() => this.tarotChoiceOverlayInstance = (UITarotChoiceOverlayController) null);
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
    if (!DataManager.Instance.HasEncounteredTarot)
      interactionTarot.introGiveDeck.enabled = true;
  }

  public IEnumerator BackToIdleRoutine(TarotCards.TarotCard card, float delay)
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
      interactionTarot.playerFarming.simpleSpineAnimator.Animate("cards/cards-stop-seperate", 0, false);
      interactionTarot.playerFarming.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
      interactionTarot.simpleBarkAfterCard.enabled = true;
      interactionTarot.StopAllCoroutines();
      GameManager.GetInstance().StartCoroutine((IEnumerator) interactionTarot.DelayEffectsRoutine(card, delay, interactionTarot.playerFarming));
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    interactionTarot.giveTarotRoutine = (Coroutine) null;
    LetterBox.Hide();
    HUD_Manager.Instance.Show(0);
    AudioManager.Instance.PlayOneShot("event:/tarot/tarot_card_close", interactionTarot.gameObject);
    GameManager.GetInstance().RemoveAllFromCamera();
    GameManager.GetInstance().CameraResetTargetZoom();
    GameManager.GetInstance().AddPlayerToCamera();
    interactionTarot.c.DisablePlayerLook = false;
    PlayerFarming.SetStateForAllPlayers();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator DelayEffectsRoutine(
    TarotCards.TarotCard card,
    float delay,
    PlayerFarming playerFarming)
  {
    yield return (object) new WaitForSeconds(0.2f + delay);
    if (card != null)
      TrinketManager.AddTrinket(card, playerFarming);
  }

  [CompilerGenerated]
  public void \u003CSetRelicRiddleManual\u003Eb__17_0()
  {
    this.relicRiddle.Finished = false;
    this.relicRiddle.Spoken = false;
  }
}
