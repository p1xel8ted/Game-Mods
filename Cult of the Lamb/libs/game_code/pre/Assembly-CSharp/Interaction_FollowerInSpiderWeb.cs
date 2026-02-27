// Decompiled with JetBrains decompiler
// Type: Interaction_FollowerInSpiderWeb
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using I2.Loc;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_FollowerInSpiderWeb : Interaction
{
  private string sBuy;
  private string sRequires;
  public bool ForceSpecificSkin;
  [SpineSkin("", "", true, false, false)]
  public string ForceSkin = "";
  private bool doneCantAffordAnimation;
  public GameObject ShopKeeper;
  public SkeletonAnimation ShopKeeperSpine;
  public SkeletonAnimation followerSpine;
  public SkeletonAnimation portalSpine;
  public ParticleSystem recruitParticles;
  public int Cost;
  public List<int> _Cost = new List<int>()
  {
    10,
    20,
    40,
    80 /*0x50*/
  };
  public FollowerInfo _followerInfo;
  private FollowerOutfit _outfit;
  [SerializeField]
  private FollowerInfoManager wim;
  public FollowersToBuy followerToBuy;
  private EventInstance receiveLoop;
  public System.Action BoughtFollowerCallback;
  public System.Action FollowerCreated;
  private string skin;
  public Transform playerMovePos;
  public bool IsDungeon;
  public bool free;
  public bool saleOn;
  private float SaleAmount;
  private string saleText;
  private string off;
  public bool Activated;
  private GameObject Player;
  private StateMachine CompanionState;
  public GameObject ConversionBone;
  [SerializeField]
  public GameObject normalBark;
  [SerializeField]
  private GameObject buyBark;
  [SerializeField]
  private GameObject cantAffordBark;

  private void Start()
  {
    this._followerInfo = FollowerInfo.NewCharacter(FollowerLocation.Base, this.skin);
    this.wim.SetV_I(this._followerInfo);
    if (this._followerInfo.SkinName == "Giraffe")
      this._followerInfo.Name = LocalizationManager.GetTranslation("FollowerNames/Sparkles");
    if (this.IsDungeon)
      this.GetComponentInParent<spiderShop>().CheckCount();
    this.UpdateLocalisation();
    this.StartCoroutine((IEnumerator) this.CheckSaleRoutine());
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.portalSpine.gameObject.SetActive(false);
    Debug.Log((object) $"DataManager.Instance.LastFollowerPurchasedFromSpider: {(object) DataManager.Instance.LastFollowerPurchasedFromSpider}  TimeManager.CurrentDay: {(object) TimeManager.CurrentDay}");
    if (DataManager.Instance.LastFollowerPurchasedFromSpider != TimeManager.CurrentDay || this.IsDungeon)
      return;
    this.normalBark.SetActive(false);
    this.gameObject.SetActive(false);
  }

  private IEnumerator CheckSaleRoutine()
  {
    this.SaleAmount = 0.0f;
    this.saleOn = false;
    yield return (object) new WaitForEndOfFrame();
    if ((UnityEngine.Object) BiomeBaseManager.Instance == (UnityEngine.Object) null)
    {
      this.saleOn = true;
      this.SaleAmount = (float) UnityEngine.Random.Range(50, 100);
    }
  }

  public void UpdateFollower()
  {
    if (this._followerInfo.StartingCursedState == Thought.OldAge)
    {
      this.wim.outfit = FollowerOutfitType.Old;
      this.wim.v_i.Outfit = FollowerOutfitType.Old;
      this._followerInfo.Outfit = FollowerOutfitType.Old;
    }
    else if (this._followerInfo.StartingCursedState == Thought.Ill)
      this.followerSpine.AnimationState.SetAnimation(1, "Emotions/emotion-sick", true);
    else if (this._followerInfo.StartingCursedState == Thought.BecomeStarving)
      this.followerSpine.AnimationState.SetAnimation(1, "Emotions/emotion-unhappy", true);
    else if (this._followerInfo.StartingCursedState == Thought.Dissenter)
      this.followerSpine.AnimationState.SetAnimation(1, "Emotions/emotion-dissenter", true);
    this.wim.SetOutfit();
  }

  private int GetCost()
  {
    if (this.free)
      return 0;
    return this.saleOn ? (int) Mathf.Round((float) this.followerToBuy.followerCost - (float) this.followerToBuy.followerCost * (this.SaleAmount / 100f)) : this.followerToBuy.followerCost;
  }

  private IEnumerator WaitForFollowerInfoSet(System.Action callback)
  {
    while (this._followerInfo == null)
      yield return (object) null;
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void SetOld()
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.WaitForFollowerInfoSet((System.Action) (() =>
    {
      this._followerInfo.StartingCursedState = Thought.OldAge;
      this.UpdateFollower();
    })));
  }

  public void SetIll()
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.WaitForFollowerInfoSet((System.Action) (() =>
    {
      this._followerInfo.StartingCursedState = Thought.Ill;
      this.UpdateFollower();
    })));
  }

  public void SetFaithful()
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.WaitForFollowerInfoSet((System.Action) (() =>
    {
      this._followerInfo.Traits.Clear();
      this._followerInfo.Traits.Add(FollowerTrait.TraitType.Faithful);
      float num1 = UnityEngine.Random.value;
      int num2 = 1;
      if ((double) num1 < 0.10000000149011612)
        num2 = 3;
      else if ((double) num1 < 0.30000001192092896)
        num2 = 2;
      for (int index1 = 0; index1 < num2; ++index1)
      {
        for (int index2 = 0; index2 < 50; ++index2)
        {
          FollowerTrait.TraitType goodTrait = FollowerTrait.GoodTraits[UnityEngine.Random.Range(0, FollowerTrait.GoodTraits.Count)];
          if (!this._followerInfo.Traits.Contains(goodTrait))
          {
            this._followerInfo.Traits.Add(goodTrait);
            break;
          }
        }
      }
      this._followerInfo.TraitsSet = true;
    })));
  }

  public void SetLevel1() => this._followerInfo.XPLevel = 1;

  public void SetLevel2() => this._followerInfo.XPLevel = 2;

  public void SetLevel3() => this._followerInfo.XPLevel = 3;

  public void SetLevel4() => this._followerInfo.XPLevel = 4;

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sBuy = ScriptLocalization.Interactions.Buy;
    this.sRequires = ScriptLocalization.Interactions.Requires;
    this.off = ScriptLocalization.UI_Generic.Off;
  }

  public override void GetLabel()
  {
    string str1 = string.Empty;
    if (this.followerToBuy != null)
    {
      switch (this._followerInfo.StartingCursedState)
      {
        case Thought.Ill:
          str1 = ScriptLocalization.Interactions_FollowerShop.Ill;
          break;
        case Thought.OldAge:
          str1 = ScriptLocalization.Interactions_FollowerShop.Old;
          break;
      }
    }
    if (this._followerInfo.Traits.Contains(FollowerTrait.TraitType.Faithful))
      str1 = ScriptLocalization.Interactions_FollowerShop.Faithful;
    string str2 = this._followerInfo.Name;
    if (!string.IsNullOrEmpty(str1))
      str2 = string.Join(" ", str1, str2);
    if (this.saleOn)
      this.saleText = (double) this.SaleAmount != 0.0 ? string.Format(this.off, (object) this.SaleAmount) : string.Empty;
    if (this.GetCost() == 0)
      this.saleText = string.Format(this.off, (object) 100);
    string str3 = string.Empty;
    if (!this.Activated)
    {
      str3 = string.Format(ScriptLocalization.UI_ItemSelector_Context.Buy, (object) str2, (object) CostFormatter.FormatCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, this.GetCost()));
      if (!string.IsNullOrEmpty(this.saleText))
        str3 = string.Join(" ", str3, this.saleText.Colour(Color.yellow));
    }
    this.Label = str3;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (Inventory.GetItemQuantity(20) >= this.GetCost() || CheatConsole.BuildingsFree)
      this.StartCoroutine((IEnumerator) this.Purchase());
    else
      this.SpiderAnimationCantAfford();
  }

  private string GetAffordColor()
  {
    return this.free || Inventory.GetItemQuantity(20) >= this.GetCost() ? "<color=#f4ecd3>" : "<color=red>";
  }

  private IEnumerator Purchase()
  {
    Interaction_FollowerInSpiderWeb followerInSpiderWeb = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(followerInSpiderWeb.ConversionBone, 4f);
    followerInSpiderWeb.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.GoToAndStop(followerInSpiderWeb.playerMovePos.position, followerInSpiderWeb.transform.gameObject);
    while (PlayerFarming.Instance.GoToAndStopping)
      yield return (object) null;
    AudioManager.Instance.PlayOneShot("event:/shop/buy", PlayerFarming.Instance.transform.position);
    followerInSpiderWeb.ShopKeeperSpine.AnimationState.SetAnimation(0, "buy", false);
    followerInSpiderWeb.ShopKeeperSpine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    if (!followerInSpiderWeb.free)
    {
      for (int i = 0; i < followerInSpiderWeb.Cost; ++i)
      {
        ResourceCustomTarget.Create(followerInSpiderWeb.ShopKeeper, PlayerFarming.Instance.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) null, followerInSpiderWeb.transform);
        yield return (object) new WaitForSeconds(0.1f);
      }
      Inventory.ChangeItemQuantity(20, -followerInSpiderWeb.GetCost());
    }
    followerInSpiderWeb.BoughtFollowerCallback();
    followerInSpiderWeb.StartCoroutine((IEnumerator) followerInSpiderWeb.BoughtFollower());
    followerInSpiderWeb.Activated = true;
  }

  private new void Update()
  {
    if ((UnityEngine.Object) (this.Player = GameObject.FindWithTag("Player")) == (UnityEngine.Object) null)
      return;
    this.GetCost();
  }

  private void SpiderAnimationBoughtItem() => this.buyBark.SetActive(true);

  private void SpiderAnimationCantAfford()
  {
    AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback", PlayerFarming.Instance.transform.position);
    MonoSingleton<Indicator>.Instance.PlayShake();
    this.cantAffordBark.SetActive(true);
  }

  private IEnumerator BoughtFollower()
  {
    Interaction_FollowerInSpiderWeb followerInSpiderWeb = this;
    followerInSpiderWeb.SpiderAnimationBoughtItem();
    followerInSpiderWeb.Interactable = false;
    HealthPlayer h = followerInSpiderWeb.state.GetComponent<HealthPlayer>();
    h.untouchable = true;
    followerInSpiderWeb.state.GetComponentInChildren<SimpleSpineAnimator>();
    GameManager.GetInstance().OnConversationNext(followerInSpiderWeb.followerSpine.gameObject, 4f);
    yield return (object) new WaitForSeconds(followerInSpiderWeb.followerSpine.AnimationState.SetAnimation(0, "spider-out", false).Animation.Duration);
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/spider_small/stuck", AudioManager.Instance.Listener.transform.position);
    float duration1 = followerInSpiderWeb.followerSpine.AnimationState.SetAnimation(0, "spider-pop2", false).Animation.Duration;
    CameraManager.shakeCamera(0.5f, (float) UnityEngine.Random.Range(0, 360));
    yield return (object) new WaitForSeconds(duration1);
    AudioManager.Instance.PlayOneShot("event:/followers/rescue", AudioManager.Instance.Listener.transform.position);
    followerInSpiderWeb.followerSpine.AnimationState.SetAnimation(0, "convert-short", false);
    followerInSpiderWeb.portalSpine.gameObject.SetActive(true);
    followerInSpiderWeb.portalSpine.AnimationState.SetAnimation(0, "convert-short", false);
    followerInSpiderWeb.recruitParticles.Play();
    AudioManager.Instance.PlayOneShot("event:/player/receive_animation_start", PlayerFarming.Instance.gameObject);
    followerInSpiderWeb.receiveLoop = AudioManager.Instance.CreateLoop("event:/player/receive_animation_loop", PlayerFarming.Instance.gameObject, true);
    followerInSpiderWeb.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    float duration2 = PlayerFarming.Instance.simpleSpineAnimator.Animate("specials/special-activate-long", 0, true).Animation.Duration;
    CameraManager.shakeCamera(0.5f, (float) UnityEngine.Random.Range(0, 360));
    yield return (object) new WaitForSeconds(duration2 - 1f);
    AudioManager.Instance.PlayOneShot("event:/player/receive_animation_end", PlayerFarming.Instance.gameObject);
    int num = (int) followerInSpiderWeb.receiveLoop.stop(STOP_MODE.ALLOWFADEOUT);
    if ((UnityEngine.Object) BiomeBaseManager.Instance != (UnityEngine.Object) null && BiomeBaseManager.Instance.SpawnExistingRecruits && DataManager.Instance.Followers_Recruit.Count <= 0)
      BiomeBaseManager.Instance.SpawnExistingRecruits = false;
    if (!followerInSpiderWeb.IsDungeon)
    {
      Debug.Log((object) "SET DATE OF LAST PURCHASE".Colour(Color.red));
      DataManager.Instance.LastFollowerPurchasedFromSpider = TimeManager.CurrentDay;
    }
    FollowerManager.CreateNewRecruit(followerInSpiderWeb._followerInfo, NotificationCentre.NotificationType.NewRecruit);
    DataManager.SetFollowerSkinUnlocked(followerInSpiderWeb._followerInfo.SkinName);
    ThoughtData data = FollowerThoughts.GetData((double) UnityEngine.Random.value >= 0.699999988079071 ? ((double) UnityEngine.Random.value > 0.30000001192092896 ? Thought.InstantBelieverRescued : Thought.ResentfulRescued) : Thought.GratefulRecued);
    data.Init();
    followerInSpiderWeb._followerInfo.Thoughts.Add(data);
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForSeconds(0.75f);
    UnityEngine.Object.Destroy((UnityEngine.Object) followerInSpiderWeb.gameObject);
    h.untouchable = false;
  }
}
