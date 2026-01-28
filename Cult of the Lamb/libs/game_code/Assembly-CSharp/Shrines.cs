// Decompiled with JetBrains decompiler
// Type: Shrines
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using Lamb.UI;
using System.Collections;
using UnityEngine;

#nullable disable
public class Shrines : Interaction
{
  public Shrines.ShrineType TypeOfShrine;
  public SpriteRenderer Shrine;
  public Light lightOnShrine;
  public GameObject XPBar;
  public SpriteRenderer RadialProgress;
  public InventoryItemDisplay typeOfReward;
  public Sprite ShrineOn;
  public Sprite ShrineOff;
  public int Cost = -1;
  public bool Used;
  public float TimeUsedShrine = -1f;
  public Coroutine giveGoldRoutine;
  public GameObject Player;
  public bool Activating;
  public float value;
  public float subtractedValue;
  public float dividedValue;
  public float currentGameTime;
  public float RechargeTime;
  public float Timer;
  public PickUp p;
  public TarotCards.TarotCard DrawnCard;

  public void SetTypeOfReward()
  {
    switch (this.TypeOfShrine)
    {
      case Shrines.ShrineType.BLUE_HEARTS:
        this.typeOfReward.SetImage(InventoryItem.ITEM_TYPE.BLUE_HEART);
        break;
      case Shrines.ShrineType.RED_HEART:
        this.typeOfReward.SetImage(InventoryItem.ITEM_TYPE.RED_HEART);
        break;
      case Shrines.ShrineType.BLACK_HEART:
        this.typeOfReward.SetImage(InventoryItem.ITEM_TYPE.BLACK_HEART);
        break;
      case Shrines.ShrineType.TAROT_CARD:
        this.typeOfReward.SetImage(InventoryItem.ITEM_TYPE.TRINKET_CARD);
        break;
    }
  }

  public int GetCostQuantity()
  {
    if (this.Cost != -1)
      return this.Cost;
    switch (this.TypeOfShrine)
    {
      case Shrines.ShrineType.BLUE_HEARTS:
        return 25;
      case Shrines.ShrineType.RED_HEART:
        return DataManager.RedHeartShrineCosts[Mathf.Clamp(DataManager.Instance.RedHeartShrineLevel, 0, DataManager.RedHeartShrineCosts.Count - 1)];
      case Shrines.ShrineType.BLACK_HEART:
        return 35;
      case Shrines.ShrineType.WEAPON:
        return 25;
      case Shrines.ShrineType.TAROT_CARD:
        if (DataManager.Instance.DeathCatBeaten)
        {
          switch (PlayerFarming.Location)
          {
            case FollowerLocation.Dungeon1_1:
              return 50;
            case FollowerLocation.Dungeon1_2:
              return 55;
            case FollowerLocation.Dungeon1_3:
              return 65;
            case FollowerLocation.Dungeon1_4:
              return 70;
            case FollowerLocation.Dungeon1_5:
              return 60;
            case FollowerLocation.Dungeon1_6:
              return 70;
          }
        }
        else
        {
          switch (PlayerFarming.Location)
          {
            case FollowerLocation.Dungeon1_1:
              return !DataManager.Instance.DungeonCompleted(FollowerLocation.Dungeon1_1) ? 15 : 20;
            case FollowerLocation.Dungeon1_2:
              return !DataManager.Instance.DungeonCompleted(FollowerLocation.Dungeon1_2) ? 20 : 30;
            case FollowerLocation.Dungeon1_3:
              return !DataManager.Instance.DungeonCompleted(FollowerLocation.Dungeon1_3) ? 30 : 45;
            case FollowerLocation.Dungeon1_4:
              return !DataManager.Instance.DungeonCompleted(FollowerLocation.Dungeon1_4) ? 40 : 50;
            case FollowerLocation.Dungeon1_5:
              return !DataManager.Instance.DungeonCompleted(FollowerLocation.Dungeon1_5) ? 60 : 100;
            case FollowerLocation.Dungeon1_6:
              return !DataManager.Instance.DungeonCompleted(FollowerLocation.Dungeon1_6) ? 70 : 100;
          }
        }
        return 15;
      case Shrines.ShrineType.DAMAGE:
        return 25;
      case Shrines.ShrineType.RELIC:
        int count = DataManager.Instance.BossesCompleted.Count;
        if (count < 2)
          return 25;
        if (count < 3)
          return 50;
        return count < 4 ? 75 : 100;
      default:
        return 25;
    }
  }

  public int GetRechargeTime() => this.TypeOfShrine == Shrines.ShrineType.RED_HEART ? 0 : 1000;

  public override void GetLabel()
  {
    int num;
    switch (this.TypeOfShrine)
    {
      case Shrines.ShrineType.CURSE:
        num = 8;
        break;
      case Shrines.ShrineType.WEAPON:
        num = 5;
        break;
      default:
        num = 0;
        break;
    }
    if (DataManager.Instance.Followers.Count < num)
    {
      this.Label = $"{ScriptLocalization.Interactions.Requires} {LocalizeIntegration.ReverseText(num.ToString())} <sprite name=\"icon_Followers\">";
      this.Interactable = false;
    }
    else
    {
      if ((double) this.GetShrineUsedTime() == -1.0)
      {
        this.Label = string.Join(": ", ScriptLocalization.Interactions.MakeOffering, CostFormatter.FormatCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, this.GetCostQuantity()));
        this.Interactable = true;
      }
      else
      {
        this.Label = ScriptLocalization.Interactions.Recharging;
        this.Interactable = false;
      }
      if (this.TypeOfShrine != Shrines.ShrineType.TAROT_CARD || TarotCards.DrawRandomCard(this.playerFarming) != null)
        return;
      this.Label = "";
      this.Interactable = false;
    }
  }

  public void UpdateVisuals()
  {
    if (DataManager.Instance.dungeonRun < 10 && this.TypeOfShrine == Shrines.ShrineType.TAROT_CARD)
      this.gameObject.SetActive(false);
    if (this.TypeOfShrine == Shrines.ShrineType.RELIC && !DataManager.Instance.OnboardedRelics)
      this.gameObject.SetActive(false);
    if ((double) this.GetShrineUsedTime() != -1.0)
    {
      this.Shrine.sprite = this.ShrineOff;
      this.Shrine.color = new Color(0.5f, 0.5f, 0.5f);
      this.lightOnShrine.enabled = false;
      this.XPBar.SetActive(true);
    }
    else
    {
      this.Shrine.sprite = this.ShrineOn;
      this.XPBar.SetActive(false);
      this.Shrine.color = Color.white;
      this.lightOnShrine.enabled = true;
    }
  }

  public override void OnDisable()
  {
    base.OnDisable();
    if (this.giveGoldRoutine == null)
      return;
    GameManager.GetInstance().OnConversationEnd();
    this.giveGoldRoutine = (Coroutine) null;
  }

  public override void OnEnableInteraction()
  {
    this.SetTypeOfReward();
    this.TimeUsedShrine = this.GetShrineUsedTime();
    this.RadialProgress.material.SetFloat("_Angle", 90f);
    this.ActivateDistance = 2f;
    this.UpdateVisuals();
    base.OnEnableInteraction();
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.GetCostQuantity() <= Inventory.GetItemQuantity(20))
    {
      this.giveGoldRoutine = this.StartCoroutine((IEnumerator) this.GiveGold(this.playerFarming));
    }
    else
    {
      AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback", this.transform.position);
      this.playerFarming.indicator.PlayShake();
    }
  }

  public override void Update()
  {
    base.Update();
    this.currentGameTime = TimeManager.TotalElapsedGameTime;
    this.RechargeTime = (float) this.GetRechargeTime();
    if ((UnityEngine.Object) (this.Player = GameObject.FindWithTag("Player")) == (UnityEngine.Object) null || (double) this.GetShrineUsedTime() == -1.0)
      return;
    this.Used = true;
    if (!this.XPBar.activeSelf)
      this.XPBar.SetActive(true);
    this.value = TimeManager.TotalElapsedGameTime - this.GetShrineUsedTime();
    this.subtractedValue = this.value;
    this.value /= (float) this.GetRechargeTime();
    this.dividedValue = this.value;
    this.RadialProgress.material.SetFloat("_Frac", this.value);
    if ((double) this.value < 1.0)
      return;
    this.Used = false;
    this.TimeUsedShrine = -1f;
    this.SetShrineUsedTime(this.TimeUsedShrine);
    this.UpdateVisuals();
    this.value = 0.0f;
  }

  public IEnumerator GiveGold(PlayerFarming playerFarming)
  {
    Shrines shrines = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(shrines.state.gameObject, 5f);
    playerFarming.GoToAndStop(shrines.transform.position - new Vector3(0.0f, 1f, 0.0f));
    while (playerFarming.GoToAndStopping)
      yield return (object) null;
    shrines.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    playerFarming.simpleSpineAnimator.Animate("specials/special-activate-long", 0, false);
    int cost = shrines.GetCostQuantity();
    for (int i = 0; i < Mathf.Min(cost, 15); ++i)
    {
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", shrines.state.gameObject.transform.position);
      ResourceCustomTarget.Create(shrines.gameObject, shrines.state.gameObject.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) null);
      yield return (object) new WaitForSeconds(2f / (float) shrines.GetCostQuantity());
    }
    shrines.giveGoldRoutine = (Coroutine) null;
    Inventory.ChangeItemQuantity(20, -cost);
    shrines.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    playerFarming.simpleSpineAnimator.Animate("pray", 0, false);
    yield return (object) new WaitForSeconds(2f);
    CameraManager.instance.ShakeCameraForDuration(0.1f, 0.25f, 0.5f);
    shrines.GiveReward(playerFarming);
    shrines.SetShrineUsed();
    shrines.transform.DOShakeScale(0.5f);
  }

  public void GiveReward(PlayerFarming playerFarming)
  {
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_Short", this.transform.position);
    Debug.Log((object) "GiveReward!");
    switch (this.TypeOfShrine)
    {
      case Shrines.ShrineType.BLUE_HEARTS:
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLUE_HEART, 1, this.transform.position + Vector3.down, 0.0f);
        break;
      case Shrines.ShrineType.RED_HEART:
        HealthPlayer component = playerFarming.GetComponent<HealthPlayer>();
        ++component.totalHP;
        component.HP = component.totalHP;
        ++DataManager.Instance.RedHeartShrineLevel;
        break;
      case Shrines.ShrineType.BLACK_HEART:
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_HEART, 1, this.transform.position + Vector3.down, 0.0f);
        break;
      case Shrines.ShrineType.CURSE:
        this.p = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOUND_ITEM_CURSE, 1, this.transform.position + Vector3.back, 0.0f);
        this.p.SetInitialSpeedAndDiraction(3f, 270f);
        this.p.MagnetDistance = 2f;
        this.p.CanStopFollowingPlayer = false;
        break;
      case Shrines.ShrineType.WEAPON:
        this.p = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOUND_ITEM_WEAPON, 1, this.transform.position + Vector3.back, 0.0f);
        this.p.SetInitialSpeedAndDiraction(3f, 270f);
        this.p.MagnetDistance = 2f;
        this.p.CanStopFollowingPlayer = false;
        break;
      case Shrines.ShrineType.TAROT_CARD:
        Debug.Log((object) "TAROT CARD!");
        this.StartCoroutine((IEnumerator) this.DrawCardRoutine(playerFarming));
        break;
      case Shrines.ShrineType.DAMAGE:
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.TRINKET_CARD, 1, this.transform.position + Vector3.down, 0.0f).GetComponent<Interaction_TarotCard>().CardOverride = TarotCards.Card.Moon;
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.TRINKET_CARD, 1, this.transform.position + Vector3.down, 0.0f).GetComponent<Interaction_TarotCard>().CardOverride = TarotCards.Card.Sun;
        break;
      case Shrines.ShrineType.RELIC:
        this.p = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.RELIC, 1, this.transform.position, 0.0f);
        this.p.SetInitialSpeedAndDiraction(6f, 270f);
        this.p.MagnetDistance = 2f;
        this.p.CanStopFollowingPlayer = false;
        GameManager.GetInstance().CameraResetTargetZoom();
        GameManager.GetInstance().OnConversationEnd();
        break;
    }
  }

  public TarotCards.TarotCard GetCard(PlayerFarming playerFarming, bool canBeCorrupted)
  {
    TarotCards.TarotCard card;
    if (!DataManager.Instance.FirstTarot)
    {
      DataManager.Instance.FirstTarot = true;
      card = new TarotCards.TarotCard(TarotCards.Card.Lovers1, 0);
    }
    else
      card = TarotCards.DrawRandomCard(playerFarming, canBeCorrupted);
    if (card != null)
      TrinketManager.AddEncounteredTrinket(card, playerFarming);
    return card;
  }

  public IEnumerator DrawCardRoutine(PlayerFarming playerFarming)
  {
    Shrines shrines = this;
    HUD_Manager.Instance.Hide(false, 0);
    GameManager.GetInstance().CameraSetTargetZoom(4f);
    LetterBox.Show(false);
    playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    playerFarming.state.facingAngle = -90f;
    CameraFollowTarget.Instance.DisablePlayerLook = true;
    AudioManager.Instance.PlayOneShot("event:/tarot/tarot_card_pull", shrines.gameObject);
    playerFarming.simpleSpineAnimator.Animate("cards/cards-start", 0, false);
    playerFarming.simpleSpineAnimator.AddAnimate("cards/cards-loop", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(0.1f);
    GameManager.GetInstance().CameraSetTargetZoom(6f);
    TarotCards.TarotCard card1 = shrines.GetCard(playerFarming, true);
    TarotCards.TarotCard card2 = shrines.GetCard(playerFarming, card1 == null || !TarotCards.CorruptedCards.Contains<TarotCards.Card>(card1.CardType));
    if (card1 != null && card2 != null)
    {
      UITarotChoiceOverlayController tarotChoiceOverlayInstance = MonoSingleton<UIManager>.Instance.ShowTarotChoice(card1, card2);
      tarotChoiceOverlayInstance.OnTarotCardSelected += (System.Action<TarotCards.TarotCard>) (card =>
      {
        TarotCards.TarotCard card3 = card1;
        if (card == card1)
          card3 = card2;
        if (CoopManager.CoopActive)
          GameManager.GetInstance().StartCoroutine((IEnumerator) this.DelayEffectsRoutine(card3, 0.0f, playerFarming.isLamb ? PlayerFarming.players[1] : PlayerFarming.players[0]));
        this.StartCoroutine((IEnumerator) this.BackToIdleRoutine(card, 0.0f));
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
  }

  public void BackToIdle(PlayerFarming playerFarming)
  {
    this.StartCoroutine((IEnumerator) this.BackToIdleRoutine(playerFarming));
  }

  public IEnumerator BackToIdleRoutine(TarotCards.TarotCard card, float delay)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Shrines shrines = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      shrines.playerFarming.simpleSpineAnimator.Animate("cards/cards-stop-seperate", 0, false);
      shrines.playerFarming.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
      shrines.StopAllCoroutines();
      GameManager.GetInstance().StartCoroutine((IEnumerator) shrines.DelayEffectsRoutine(card, delay, shrines.playerFarming));
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    LetterBox.Hide();
    HUD_Manager.Instance.Show(0);
    AudioManager.Instance.PlayOneShot("event:/tarot/tarot_card_close", shrines.gameObject);
    GameManager.GetInstance().CameraResetTargetZoom();
    CameraFollowTarget.Instance.DisablePlayerLook = true;
    GameManager.GetInstance().RemoveAllFromCamera();
    GameManager.GetInstance().AddPlayersToCamera();
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
    TrinketManager.AddTrinket(card, playerFarming);
  }

  public IEnumerator BackToIdleRoutine(PlayerFarming playerFarming)
  {
    Shrines shrines = this;
    Time.timeScale = 1f;
    LetterBox.Hide();
    HUD_Manager.Instance.Show(0);
    AudioManager.Instance.PlayOneShot("event:/tarot/tarot_card_close", shrines.gameObject);
    GameManager.GetInstance().CameraResetTargetZoom();
    CameraFollowTarget.Instance.DisablePlayerLook = false;
    yield return (object) null;
    playerFarming.simpleSpineAnimator.Animate("cards/cards-stop-seperate", 0, false);
    playerFarming.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    shrines.state.CURRENT_STATE = StateMachine.State.Idle;
    playerFarming.SpineUseDeltaTime(true);
    SimpleSetCamera.EnableAll();
    GameManager.GetInstance().StartCoroutine((IEnumerator) shrines.DelayEffectsRoutine(playerFarming));
  }

  public IEnumerator DelayEffectsRoutine(PlayerFarming playerFarming)
  {
    yield return (object) new WaitForSeconds(0.2f);
    TrinketManager.AddTrinket(this.DrawnCard, playerFarming);
  }

  public void SetShrineUsedTime(float Set)
  {
    foreach (ShrineUsageInfo shrineUsageInfo in DataManager.Instance.ShrineTimerInfo)
    {
      if (shrineUsageInfo.TypeOfShrine == this.TypeOfShrine)
        shrineUsageInfo.useTime = Set;
    }
  }

  public float GetShrineUsedTime()
  {
    foreach (ShrineUsageInfo shrineUsageInfo in DataManager.Instance.ShrineTimerInfo)
    {
      if (shrineUsageInfo.TypeOfShrine == this.TypeOfShrine)
        return shrineUsageInfo.useTime;
    }
    return -1f;
  }

  public void SetShrineUsed()
  {
    this.Used = true;
    bool flag = false;
    this.TimeUsedShrine = TimeManager.TotalElapsedGameTime;
    Debug.Log((object) TimeManager.TotalElapsedGameTime);
    this.UpdateVisuals();
    foreach (ShrineUsageInfo shrineUsageInfo in DataManager.Instance.ShrineTimerInfo)
    {
      if (shrineUsageInfo.TypeOfShrine == this.TypeOfShrine)
      {
        flag = true;
        shrineUsageInfo.useTime = this.TimeUsedShrine;
      }
    }
    if (flag)
      return;
    DataManager.Instance.ShrineTimerInfo.Add(new ShrineUsageInfo()
    {
      TypeOfShrine = this.TypeOfShrine,
      useTime = this.TimeUsedShrine
    });
  }

  public enum ShrineType
  {
    NONE,
    BLUE_HEARTS,
    RED_HEART,
    BLACK_HEART,
    CURSE,
    WEAPON,
    BLACK_SOULS,
    TAROT_CARD,
    DAMAGE,
    RELIC,
  }
}
