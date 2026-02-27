// Decompiled with JetBrains decompiler
// Type: Shrines
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private GameObject Player;
  private bool Activating;
  private float value;
  public float subtractedValue;
  public float dividedValue;
  public float currentGameTime;
  public float RechargeTime;
  private float Timer;
  private PickUp p;
  private TarotCards.TarotCard DrawnCard;

  private void SetTypeOfReward()
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
        return DataManager.RedHeartShrineCosts[DataManager.Instance.RedHeartShrineLevel];
      case Shrines.ShrineType.BLACK_HEART:
        return 35;
      case Shrines.ShrineType.WEAPON:
        return 25;
      case Shrines.ShrineType.TAROT_CARD:
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
          default:
            return 15;
        }
      case Shrines.ShrineType.DAMAGE:
        return 25;
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
      this.Label = $"{ScriptLocalization.Interactions.Requires} {(object) num} <sprite name=\"icon_Followers\">";
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
      if (this.TypeOfShrine != Shrines.ShrineType.TAROT_CARD || TarotCards.DrawRandomCard() != null)
        return;
      this.Label = "";
      this.Interactable = false;
    }
  }

  private void UpdateVisuals()
  {
    if (DataManager.Instance.dungeonRun < 10 && this.TypeOfShrine == Shrines.ShrineType.TAROT_CARD)
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

  public override void OnEnableInteraction()
  {
    this.SetTypeOfReward();
    this.TimeUsedShrine = this.GetShrineUsedTime();
    this.RadialProgress.material.SetFloat("_Angle", 90f);
    this.UpdateVisuals();
    base.OnEnableInteraction();
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.GetCostQuantity() <= Inventory.GetItemQuantity(20))
    {
      this.StartCoroutine((IEnumerator) this.GiveGold());
    }
    else
    {
      AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback", this.transform.position);
      MonoSingleton<Indicator>.Instance.PlayShake();
    }
  }

  private new void Update()
  {
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

  private IEnumerator GiveGold()
  {
    Shrines shrines = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(shrines.state.gameObject, 5f);
    Vector3 TargetPosition = shrines.transform.position - new Vector3(0.0f, 1f, 0.0f);
    PlayerFarming.Instance.GoToAndStop(TargetPosition);
    while (PlayerFarming.Instance.GoToAndStopping)
      yield return (object) null;
    shrines.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.simpleSpineAnimator.Animate("specials/special-activate-long", 0, false);
    for (int i = 0; i < shrines.GetCostQuantity(); ++i)
    {
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", shrines.state.gameObject.transform.position);
      ResourceCustomTarget.Create(shrines.gameObject, shrines.state.gameObject.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) null);
      Inventory.ChangeItemQuantity(20, -1);
      yield return (object) new WaitForSeconds(2f / (float) shrines.GetCostQuantity());
    }
    shrines.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.simpleSpineAnimator.Animate("pray", 0, false);
    yield return (object) new WaitForSeconds(2f);
    CameraManager.instance.ShakeCameraForDuration(0.1f, 0.25f, 0.5f);
    shrines.GiveReward();
    shrines.SetShrineUsed();
    shrines.transform.DOShakeScale(0.5f);
  }

  private void GiveReward()
  {
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_Short", this.transform.position);
    Debug.Log((object) "GiveReward!");
    switch (this.TypeOfShrine)
    {
      case Shrines.ShrineType.BLUE_HEARTS:
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLUE_HEART, 1, this.transform.position + Vector3.down, 0.0f);
        break;
      case Shrines.ShrineType.RED_HEART:
        HealthPlayer component = PlayerFarming.Instance.GetComponent<HealthPlayer>();
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
        this.StartCoroutine((IEnumerator) this.DrawCardRoutine());
        break;
      case Shrines.ShrineType.DAMAGE:
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.TRINKET_CARD, 1, this.transform.position + Vector3.down, 0.0f).GetComponent<Interaction_TarotCard>().CardOverride = TarotCards.Card.Moon;
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.TRINKET_CARD, 1, this.transform.position + Vector3.down, 0.0f).GetComponent<Interaction_TarotCard>().CardOverride = TarotCards.Card.Sun;
        break;
    }
  }

  private TarotCards.TarotCard GetCard()
  {
    TarotCards.TarotCard card;
    if (!DataManager.Instance.FirstTarot)
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

  private IEnumerator DrawCardRoutine()
  {
    Shrines shrines = this;
    HUD_Manager.Instance.Hide(false, 0);
    GameManager.GetInstance().CameraSetTargetZoom(4f);
    LetterBox.Show(false);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.state.facingAngle = -90f;
    CameraFollowTarget.Instance.DisablePlayerLook = true;
    PlayerFarming instance = PlayerFarming.Instance;
    AudioManager.Instance.PlayOneShot("event:/tarot/tarot_card_pull", shrines.gameObject);
    instance.simpleSpineAnimator.Animate("cards/cards-start", 0, false);
    instance.simpleSpineAnimator.AddAnimate("cards/cards-loop", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(0.1f);
    GameManager.GetInstance().CameraSetTargetZoom(6f);
    TarotCards.TarotCard card1 = shrines.GetCard();
    TarotCards.TarotCard card2 = shrines.GetCard();
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

    TarotCards.TarotCard GetOther(TarotCards.TarotCard card) => card == card1 ? card2 : card1;
  }

  private void BackToIdle() => this.StartCoroutine((IEnumerator) this.BackToIdleRoutine());

  private IEnumerator BackToIdleRoutine(TarotCards.TarotCard card, float delay)
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
      PlayerFarming.Instance.simpleSpineAnimator.Animate("cards/cards-stop-seperate", 0, false);
      PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
      shrines.StopAllCoroutines();
      GameManager.GetInstance().StartCoroutine((IEnumerator) shrines.DelayEffectsRoutine(card, delay));
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    LetterBox.Hide();
    HUD_Manager.Instance.Show(0);
    AudioManager.Instance.PlayOneShot("event:/tarot/tarot_card_close", shrines.gameObject);
    GameManager.GetInstance().CameraResetTargetZoom();
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
    TrinketManager.AddTrinket(card);
  }

  private IEnumerator BackToIdleRoutine()
  {
    Shrines shrines = this;
    Time.timeScale = 1f;
    LetterBox.Hide();
    HUD_Manager.Instance.Show(0);
    AudioManager.Instance.PlayOneShot("event:/tarot/tarot_card_close", shrines.gameObject);
    GameManager.GetInstance().CameraResetTargetZoom();
    CameraFollowTarget.Instance.DisablePlayerLook = false;
    yield return (object) null;
    PlayerFarming.Instance.simpleSpineAnimator.Animate("cards/cards-stop-seperate", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    shrines.state.CURRENT_STATE = StateMachine.State.Idle;
    PlayerFarming.Instance.SpineUseDeltaTime(true);
    SimpleSetCamera.EnableAll();
    GameManager.GetInstance().StartCoroutine((IEnumerator) shrines.DelayEffectsRoutine());
  }

  private IEnumerator DelayEffectsRoutine()
  {
    yield return (object) new WaitForSeconds(0.2f);
    TrinketManager.AddTrinket(this.DrawnCard);
  }

  private void SetShrineUsedTime(float Set)
  {
    foreach (ShrineUsageInfo shrineUsageInfo in DataManager.Instance.ShrineTimerInfo)
    {
      if (shrineUsageInfo.TypeOfShrine == this.TypeOfShrine)
        shrineUsageInfo.useTime = Set;
    }
  }

  private float GetShrineUsedTime()
  {
    foreach (ShrineUsageInfo shrineUsageInfo in DataManager.Instance.ShrineTimerInfo)
    {
      if (shrineUsageInfo.TypeOfShrine == this.TypeOfShrine)
        return shrineUsageInfo.useTime;
    }
    return -1f;
  }

  private void SetShrineUsed()
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
  }
}
