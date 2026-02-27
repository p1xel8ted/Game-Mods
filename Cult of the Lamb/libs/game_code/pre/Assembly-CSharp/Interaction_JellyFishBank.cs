// Decompiled with JetBrains decompiler
// Type: Interaction_JellyFishBank
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using MMTools;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_JellyFishBank : Interaction
{
  public Interaction_JellyFishBank.States State;
  private int InvestmentAmount = 350;
  private int DonationAmount = 10;
  private bool Activated;
  private GameObject g;
  public Interaction_SimpleConversation IntroConversation;
  public Interaction_SimpleConversation GiveMoneyConversation;
  public Interaction_SimpleConversation RefuseConversation;
  public SkeletonAnimation ReceiveDonationSpine;
  [SerializeField]
  private bool overrideReward;
  [SerializeField]
  private float rewardNumber = -1f;
  private GameObject Target;
  private bool ShowingIndicator;
  private int cacheDay;

  private void Start()
  {
    if (!DataManager.Instance.MidasBankIntro)
      this.State = Interaction_JellyFishBank.States.Intro;
    else if (DataManager.Instance.midasDonation == null)
    {
      if (!DataManager.Instance.MidasBankUnlocked)
        this.State = Interaction_JellyFishBank.States.AwaitDonation;
      else
        this.State = Interaction_JellyFishBank.States.AwaitInvestment;
    }
    else if (TimeManager.CurrentDay - DataManager.Instance.midasDonation.Day > 2)
      this.State = Interaction_JellyFishBank.States.DonationProcessed;
    else
      this.State = Interaction_JellyFishBank.States.DonationProcessing;
  }

  private void LeaveMenu() => this.StartCoroutine((IEnumerator) this.LeaveMenuRoutine());

  private IEnumerator LeaveMenuRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Interaction_JellyFishBank interactionJellyFishBank = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      UnityEngine.Object.Destroy((UnityEngine.Object) interactionJellyFishBank.g);
      interactionJellyFishBank.Activated = false;
      interactionJellyFishBank.Interactable = true;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
    Debug.Log((object) "Leave Menu called");
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(1f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private string GetAffordColor()
  {
    return Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD) < this.InvestmentAmount ? "<color=red>" : "<color=#f4ecd3>";
  }

  private string GetAffordDonationColor()
  {
    return Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.GOLD_REFINED) < this.DonationAmount ? "<color=red>" : "<color=#f4ecd3>";
  }

  public override void GetLabel()
  {
    switch (this.State)
    {
      case Interaction_JellyFishBank.States.Intro:
        this.Label = ScriptLocalization.Interactions.Look;
        break;
      case Interaction_JellyFishBank.States.AwaitDonation:
        this.Label = string.Format(ScriptLocalization.UI_ItemSelector_Context.Give, (object) CostFormatter.FormatCost(InventoryItem.ITEM_TYPE.GOLD_REFINED, this.DonationAmount));
        break;
      case Interaction_JellyFishBank.States.AwaitInvestment:
        this.Label = string.Format(ScriptLocalization.UI_ItemSelector_Context.Give, (object) CostFormatter.FormatCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, this.InvestmentAmount));
        break;
      case Interaction_JellyFishBank.States.Active:
        this.Label = ScriptLocalization.Interactions.Buy;
        break;
      case Interaction_JellyFishBank.States.DonationProcessing:
        this.Interactable = false;
        this.Label = (!this.CheckSingleDayLeft() ? ScriptLocalization.Interactions_Bank.Processing_plural : ScriptLocalization.Interactions_Bank.Processing).Replace("{0}", this.GetDaysRemaining());
        break;
      case Interaction_JellyFishBank.States.DonationProcessed:
        this.Interactable = true;
        this.Label = ScriptLocalization.Interactions_Bank.Withdrawl;
        break;
    }
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    switch (this.State)
    {
      case Interaction_JellyFishBank.States.Intro:
        this.IntroConversation.gameObject.SetActive(true);
        this.IntroConversation.Callback.AddListener((UnityAction) (() =>
        {
          DataManager.Instance.MidasBankIntro = true;
          this.enabled = true;
          this.Start();
        }));
        this.enabled = false;
        break;
      case Interaction_JellyFishBank.States.AwaitDonation:
        if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.GOLD_REFINED) < this.DonationAmount)
        {
          MonoSingleton<Indicator>.Instance.PlayShake();
          break;
        }
        this.StartCoroutine((IEnumerator) this.UnlockBank());
        break;
      case Interaction_JellyFishBank.States.AwaitInvestment:
        if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD) < this.InvestmentAmount)
        {
          MonoSingleton<Indicator>.Instance.PlayShake();
          break;
        }
        this.StartCoroutine((IEnumerator) this.DepositRoutine());
        break;
      case Interaction_JellyFishBank.States.Active:
        state.CURRENT_STATE = StateMachine.State.InActive;
        break;
      case Interaction_JellyFishBank.States.DonationProcessing:
        this.Interactable = false;
        break;
      case Interaction_JellyFishBank.States.DonationProcessed:
        this.Interactable = true;
        this.GetReward();
        break;
    }
  }

  private string GetDaysRemaining()
  {
    return (DataManager.Instance.midasDonation.Day - TimeManager.CurrentDay + 3).ToString();
  }

  private bool CheckSingleDayLeft()
  {
    return DataManager.Instance.midasDonation.Day - TimeManager.CurrentDay + 3 == 1;
  }

  private void CheckRewardProcessed()
  {
    if (TimeManager.CurrentDay - DataManager.Instance.midasDonation.Day > 2)
      this.State = Interaction_JellyFishBank.States.DonationProcessed;
    else
      this.State = Interaction_JellyFishBank.States.DonationProcessing;
  }

  public void GetReward() => this.StartCoroutine((IEnumerator) this.GetRewardRoutine());

  private IEnumerator GetRewardRoutine()
  {
    Interaction_JellyFishBank interactionJellyFishBank1 = this;
    interactionJellyFishBank1.ReceiveDonationSpine.gameObject.SetActive(false);
    interactionJellyFishBank1.State = Interaction_JellyFishBank.States.Active;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionJellyFishBank1.gameObject, 6f);
    interactionJellyFishBank1.gameObject.transform.DOShakePosition(1f, new Vector3(0.1f, 0.0f, 0.0f));
    BiomeConstants.Instance.ShakeCamera();
    Vector3 position = interactionJellyFishBank1.transform.position;
    AudioManager.Instance.PlayOneShot("event:/locations/light_house/fireplace_shake", position);
    yield return (object) new WaitForSeconds(2f);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(position);
    if (!interactionJellyFishBank1.overrideReward)
      interactionJellyFishBank1.rewardNumber = (float) UnityEngine.Random.Range(1, 10);
    if ((double) interactionJellyFishBank1.rewardNumber <= 2.0)
    {
      // ISSUE: reference to a compiler-generated method
      MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
      {
        new ConversationEntry(interactionJellyFishBank1.gameObject, string.Format(ScriptLocalization.Interactions_Bank.BadReward), "")
        {
          Speaker = interactionJellyFishBank1.OutlineTarget,
          CharacterName = $"<color=yellow>{ScriptLocalization.Interactions_Bank.Title}</color>"
        }
      }, (List<MMTools.Response>) null, new System.Action(interactionJellyFishBank1.\u003CGetRewardRoutine\u003Eb__23_0)));
    }
    else if ((double) interactionJellyFishBank1.rewardNumber > 2.0 && (double) interactionJellyFishBank1.rewardNumber <= 5.0)
    {
      Interaction_JellyFishBank interactionJellyFishBank = interactionJellyFishBank1;
      ConversationEntry conversationEntry1 = new ConversationEntry(interactionJellyFishBank1.gameObject, string.Format(ScriptLocalization.Interactions_Bank.OkayReward), "");
      conversationEntry1.Speaker = interactionJellyFishBank1.OutlineTarget;
      conversationEntry1.CharacterName = $"<color=yellow>{ScriptLocalization.Interactions_Bank.Title}</color>";
      int reward = UnityEngine.Random.Range(350, 500);
      ConversationEntry conversationEntry2 = new ConversationEntry(interactionJellyFishBank1.gameObject, string.Format($"{(object) reward}<sprite name=\"icon_blackgold\">{InventoryItem.LocalizedName(InventoryItem.ITEM_TYPE.BLACK_GOLD)}"), "");
      MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
      {
        conversationEntry1,
        conversationEntry2
      }, (List<MMTools.Response>) null, (System.Action) (() =>
      {
        interactionJellyFishBank.GiveCoins(reward, InventoryItem.ITEM_TYPE.BLACK_GOLD);
        GameManager.GetInstance().OnConversationEnd();
      })));
    }
    else if ((double) interactionJellyFishBank1.rewardNumber > 5.0)
    {
      Interaction_JellyFishBank interactionJellyFishBank = interactionJellyFishBank1;
      ConversationEntry conversationEntry3 = new ConversationEntry(interactionJellyFishBank1.gameObject, string.Format(ScriptLocalization.Interactions_Bank.GoodReward), "");
      conversationEntry3.Speaker = interactionJellyFishBank1.OutlineTarget;
      conversationEntry3.CharacterName = $"<color=yellow>{ScriptLocalization.Interactions_Bank.Title}</color>";
      int reward = UnityEngine.Random.Range(500, 700);
      ConversationEntry conversationEntry4 = new ConversationEntry(interactionJellyFishBank1.gameObject, string.Format($"{(object) reward}<sprite name=\"icon_blackgold\">{InventoryItem.LocalizedName(InventoryItem.ITEM_TYPE.BLACK_GOLD)}"), "");
      MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
      {
        conversationEntry3,
        conversationEntry4
      }, (List<MMTools.Response>) null, (System.Action) (() =>
      {
        interactionJellyFishBank.GiveCoins(reward, InventoryItem.ITEM_TYPE.BLACK_GOLD);
        GameManager.GetInstance().OnConversationEnd();
      })));
    }
    else if ((double) interactionJellyFishBank1.rewardNumber > 9.0)
    {
      Interaction_JellyFishBank interactionJellyFishBank = interactionJellyFishBank1;
      ConversationEntry conversationEntry5 = new ConversationEntry(interactionJellyFishBank1.gameObject, string.Format(ScriptLocalization.Interactions_Bank.GreatReward), "");
      conversationEntry5.Speaker = interactionJellyFishBank1.OutlineTarget;
      conversationEntry5.CharacterName = $"<color=yellow>{ScriptLocalization.Interactions_Bank.Title}</color>";
      int reward = UnityEngine.Random.Range(700, 1000);
      ConversationEntry conversationEntry6 = new ConversationEntry(interactionJellyFishBank1.gameObject, string.Format($"{(object) reward}<sprite name=\"icon_blackgold\">{InventoryItem.LocalizedName(InventoryItem.ITEM_TYPE.BLACK_GOLD)}"), "");
      MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
      {
        conversationEntry5,
        conversationEntry6
      }, (List<MMTools.Response>) null, (System.Action) (() =>
      {
        interactionJellyFishBank.GiveCoins(reward, InventoryItem.ITEM_TYPE.BLACK_GOLD);
        GameManager.GetInstance().OnConversationEnd();
      })));
    }
    DataManager.Instance.midasDonation = (MidasDonation) null;
    interactionJellyFishBank1.Start();
    Debug.Log((object) "Gave Reward!");
  }

  private void GiveCoins(int quantity, InventoryItem.ITEM_TYPE item)
  {
    this.StartCoroutine((IEnumerator) this.GiveCoinsRoutine(quantity, item));
  }

  private IEnumerator GiveCoinsRoutine(int quantity, InventoryItem.ITEM_TYPE item)
  {
    Interaction_JellyFishBank interactionJellyFishBank = this;
    float increment = 1f / 20f;
    for (int i = 0; i < 20; ++i)
    {
      AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", interactionJellyFishBank.transform.position);
      ResourceCustomTarget.Create(PlayerFarming.Instance.CameraBone.gameObject, interactionJellyFishBank.transform.position, item, (System.Action) null);
      yield return (object) new WaitForSeconds(increment);
    }
    yield return (object) new WaitForSeconds(0.5f);
    Inventory.ChangeItemQuantity((int) item, quantity);
  }

  private IEnumerator DepositRoutine()
  {
    Interaction_JellyFishBank interactionJellyFishBank = this;
    interactionJellyFishBank.State = Interaction_JellyFishBank.States.Active;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionJellyFishBank.state.gameObject, 6f);
    interactionJellyFishBank.Target = interactionJellyFishBank.gameObject;
    yield return (object) new WaitForSeconds(1f);
    float increment = 1f / 20f;
    for (int i = 0; i < 20; ++i)
    {
      AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", interactionJellyFishBank.transform.position);
      ResourceCustomTarget.Create(interactionJellyFishBank.Target, PlayerFarming.Instance.CameraBone.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) null);
      yield return (object) new WaitForSeconds(increment);
    }
    yield return (object) new WaitForSeconds(0.5f);
    Inventory.ChangeItemQuantity(20, -interactionJellyFishBank.InvestmentAmount);
    GameManager.GetInstance().OnConversationEnd();
    DataManager.Instance.midasDonation = new MidasDonation()
    {
      Day = TimeManager.CurrentDay,
      InvestmentAmount = 350
    };
    interactionJellyFishBank.HasChanged = true;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
    interactionJellyFishBank.Start();
    Debug.Log((object) "Gave Money!");
    yield return (object) new WaitForSeconds(0.5f);
    CameraManager.instance.ShakeCameraForDuration(1f, 1.2f, 0.3f);
    AudioManager.Instance.PlayOneShot("event:/rituals/blood_sacrifice", PlayerFarming.Instance.transform.position);
  }

  private IEnumerator UnlockBank()
  {
    Interaction_JellyFishBank interactionJellyFishBank = this;
    interactionJellyFishBank.state.CURRENT_STATE = StateMachine.State.InActive;
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.PayMidas);
    yield return (object) new WaitForSeconds(1f);
    interactionJellyFishBank.State = Interaction_JellyFishBank.States.Active;
    interactionJellyFishBank.ShowingIndicator = false;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionJellyFishBank.state.gameObject, 6f);
    if (!DataManager.Instance.MidasBankUnlocked)
    {
      interactionJellyFishBank.ReceiveDonationSpine.gameObject.SetActive(true);
      interactionJellyFishBank.ReceiveDonationSpine.AnimationState.SetAnimation(0, "enter", false);
      interactionJellyFishBank.ReceiveDonationSpine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
      interactionJellyFishBank.Target = interactionJellyFishBank.ReceiveDonationSpine.gameObject;
    }
    yield return (object) new WaitForSeconds(1f);
    float increment = 1f / 20f;
    for (int i = 0; i < 10; ++i)
    {
      AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", interactionJellyFishBank.transform.position);
      ResourceCustomTarget.Create(interactionJellyFishBank.Target, PlayerFarming.Instance.CameraBone.transform.position, InventoryItem.ITEM_TYPE.GOLD_REFINED, (System.Action) null);
      yield return (object) new WaitForSeconds(increment);
    }
    yield return (object) new WaitForSeconds(0.5f);
    Inventory.ChangeItemQuantity(86, -interactionJellyFishBank.DonationAmount);
    GameManager.GetInstance().OnConversationEnd();
    interactionJellyFishBank.GiveMoneyConversation.gameObject.SetActive(true);
    // ISSUE: reference to a compiler-generated method
    interactionJellyFishBank.GiveMoneyConversation.Callback.AddListener(new UnityAction(interactionJellyFishBank.\u003CUnlockBank\u003Eb__28_0));
    interactionJellyFishBank.enabled = false;
    Debug.Log((object) "Gave Money!");
  }

  protected override void Update()
  {
    base.Update();
    if (this.State != Interaction_JellyFishBank.States.DonationProcessing || this.cacheDay == TimeManager.CurrentDay)
      return;
    this.HasChanged = true;
    this.Start();
    this.cacheDay = TimeManager.CurrentDay;
  }

  public enum States
  {
    Intro,
    AwaitDonation,
    AwaitInvestment,
    Active,
    DonationProcessing,
    DonationProcessed,
  }
}
