// Decompiled with JetBrains decompiler
// Type: Interaction_FollowerInSpiderWeb
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using I2.Loc;
using Spine.Unity;
using Spine.Unity.Examples;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_FollowerInSpiderWeb : Interaction
{
  public string sBuy;
  public string sRequires;
  public bool ForceSpecificSkin;
  [SpineSkin("", "", true, false, false)]
  public string ForceSkin = "";
  public bool doneCantAffordAnimation;
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
  public FollowerOutfit _outfit;
  [SerializeField]
  public FollowerInfoManager wim;
  public FollowersToBuy followerToBuy;
  public EventInstance receiveLoop;
  public System.Action BoughtFollowerCallback;
  public System.Action FollowerCreated;
  public string skin;
  public Transform playerMovePos;
  public bool IsDungeon;
  public bool free;
  public bool saleOn;
  public float SaleAmount;
  public string saleText;
  public string off;
  public bool Activated;
  public GameObject Player;
  public StateMachine CompanionState;
  public GameObject ConversionBone;
  [SerializeField]
  public GameObject normalBark;
  [SerializeField]
  public GameObject buyBark;
  [SerializeField]
  public GameObject cantAffordBark;

  public void Start()
  {
    this._followerInfo = FollowerInfo.NewCharacter(FollowerLocation.Base, this.skin);
    this.wim.SetV_I(this._followerInfo);
    if (this._followerInfo.SkinName == "Giraffe")
      this._followerInfo.Name = LocalizationManager.GetTranslation("FollowerNames/Sparkles");
    if (this._followerInfo.SkinName == "Poppy")
      this._followerInfo.Name = LocalizationManager.GetTranslation("FollowerNames/Poppy");
    if (this._followerInfo.SkinName == "Pudding")
      this._followerInfo.Name = LocalizationManager.GetTranslation("FollowerNames/Pudding");
    if (this.IsDungeon)
      this.GetComponentInParent<spiderShop>().CheckCount();
    this.UpdateLocalisation();
    this.StartCoroutine((IEnumerator) this.CheckSaleRoutine());
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.portalSpine.gameObject.SetActive(false);
    Debug.Log((object) $"DataManager.Instance.LastFollowerPurchasedFromSpider: {DataManager.Instance.LastFollowerPurchasedFromSpider.ToString()}  TimeManager.CurrentDay: {TimeManager.CurrentDay.ToString()}");
    if (DataManager.Instance.LastFollowerPurchasedFromSpider != TimeManager.CurrentDay || this.IsDungeon)
      return;
    this.normalBark.SetActive(false);
    this.gameObject.SetActive(false);
  }

  public IEnumerator CheckSaleRoutine()
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
    else if (this._followerInfo.StartingCursedState == Thought.Freezing)
      this.followerSpine.AnimationState.SetAnimation(1, "Emotions/emotion-freezing", true);
    this.wim.SetOutfit();
  }

  public int GetCost()
  {
    int cost = this.followerToBuy.followerCost;
    if (this.saleOn)
      cost = (int) Mathf.Round((float) this.followerToBuy.followerCost - (float) this.followerToBuy.followerCost * (this.SaleAmount / 100f));
    if (this.free)
      cost = 0;
    if (DataManager.Instance.DeathCatBeaten)
      cost *= 2;
    return cost;
  }

  public IEnumerator WaitForFollowerInfoSet(System.Action callback)
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
      if (!this._followerInfo.Traits.Contains(FollowerTrait.TraitType.Mutated))
        this._followerInfo.StartingCursedState = Thought.OldAge;
      this.UpdateFollower();
    })));
  }

  public void SetIll()
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.WaitForFollowerInfoSet((System.Action) (() =>
    {
      if (!this._followerInfo.Traits.Contains(FollowerTrait.TraitType.Mutated))
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
          FollowerTrait.TraitType faithfulTrait = FollowerTrait.FaithfulTraits[UnityEngine.Random.Range(0, FollowerTrait.FaithfulTraits.Count)];
          if (!this._followerInfo.Traits.Contains(faithfulTrait))
          {
            this._followerInfo.Traits.Add(faithfulTrait);
            break;
          }
        }
      }
      this._followerInfo.TraitsSet = true;
    })));
  }

  public void SetMutated()
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.WaitForFollowerInfoSet((System.Action) (() =>
    {
      this._followerInfo.StartingCursedState = Thought.None;
      if (!this._followerInfo.Traits.Contains(FollowerTrait.TraitType.Mutated))
      {
        if (this._followerInfo.Traits.Count >= 3)
          this._followerInfo.Traits.RemoveAt(0);
        this._followerInfo.Traits.Add(FollowerTrait.TraitType.Mutated);
      }
      this.UpdateFollower();
      FollowerSpineEventListener componentInChildren = this.ShopKeeper.GetComponentInChildren<FollowerSpineEventListener>(true);
      if (!(bool) (UnityEngine.Object) componentInChildren)
        return;
      componentInChildren.RefreshRotValue();
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
    if (!string.IsNullOrEmpty(str1) && LocalizationManager.CurrentLanguageCode == "it")
      str2 = string.Join(" ", str2, str1);
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

  public string GetAffordColor()
  {
    return this.free || Inventory.GetItemQuantity(20) >= this.GetCost() ? "<color=#f4ecd3>" : "<color=red>";
  }

  public IEnumerator Purchase()
  {
    Interaction_FollowerInSpiderWeb followerInSpiderWeb = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(followerInSpiderWeb.ConversionBone, 4f);
    followerInSpiderWeb.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    followerInSpiderWeb.playerFarming.GoToAndStop(followerInSpiderWeb.playerMovePos.position, followerInSpiderWeb.transform.gameObject);
    while (followerInSpiderWeb.playerFarming.GoToAndStopping)
      yield return (object) null;
    AudioManager.Instance.PlayAtmos("event:/dlc/env/woolhaven/helob_purchase");
    AudioManager.Instance.PlayOneShot("event:/shop/buy", followerInSpiderWeb.playerFarming.transform.position);
    followerInSpiderWeb.ShopKeeperSpine.AnimationState.SetAnimation(0, "buy", false);
    followerInSpiderWeb.ShopKeeperSpine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    if (!followerInSpiderWeb.free && followerInSpiderWeb.GetCost() != 0)
    {
      for (int i = 0; i < followerInSpiderWeb.Cost; ++i)
      {
        ResourceCustomTarget.Create(followerInSpiderWeb.ShopKeeper, followerInSpiderWeb.playerFarming.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) null, followerInSpiderWeb.transform);
        yield return (object) new WaitForSeconds(0.1f);
      }
      Inventory.ChangeItemQuantity(20, -followerInSpiderWeb.GetCost());
    }
    followerInSpiderWeb.BoughtFollowerCallback();
    followerInSpiderWeb.StartCoroutine((IEnumerator) followerInSpiderWeb.BoughtFollower());
    followerInSpiderWeb.Activated = true;
  }

  public override void Update()
  {
    base.Update();
    if ((UnityEngine.Object) (this.Player = GameObject.FindWithTag("Player")) == (UnityEngine.Object) null)
      return;
    this.GetCost();
  }

  public void SpiderAnimationBoughtItem() => this.buyBark.SetActive(true);

  public void SpiderAnimationCantAfford()
  {
    AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback", this.playerFarming.transform.position);
    this.playerFarming.indicator.PlayShake();
    this.cantAffordBark.SetActive(true);
  }

  public IEnumerator BoughtFollower()
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
    AudioManager.Instance.PlayOneShot("event:/player/receive_animation_start", followerInSpiderWeb.playerFarming.gameObject);
    followerInSpiderWeb.receiveLoop = AudioManager.Instance.CreateLoop("event:/player/receive_animation_loop", followerInSpiderWeb.playerFarming.gameObject, true);
    followerInSpiderWeb.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    float duration2 = followerInSpiderWeb.playerFarming.simpleSpineAnimator.Animate("specials/special-activate-long", 0, true).Animation.Duration;
    CameraManager.shakeCamera(0.5f, (float) UnityEngine.Random.Range(0, 360));
    yield return (object) new WaitForSeconds(duration2 - 1f);
    AudioManager.Instance.PlayOneShot("event:/player/receive_animation_end", followerInSpiderWeb.playerFarming.gameObject);
    int num = (int) followerInSpiderWeb.receiveLoop.stop(STOP_MODE.ALLOWFADEOUT);
    if ((UnityEngine.Object) BiomeBaseManager.Instance != (UnityEngine.Object) null && BiomeBaseManager.Instance.SpawnExistingRecruits && DataManager.Instance.Followers_Recruit.Count <= 0)
      BiomeBaseManager.Instance.SpawnExistingRecruits = false;
    if (!followerInSpiderWeb.IsDungeon)
    {
      Debug.Log((object) "SET DATE OF LAST PURCHASE".Colour(Color.red));
      DataManager.Instance.LastFollowerPurchasedFromSpider = TimeManager.CurrentDay;
    }
    followerInSpiderWeb._followerInfo.SkinCharacter = WorshipperData.Instance.GetSkinIndexFromName(followerInSpiderWeb._followerInfo.SkinName.StripNumbers());
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

  [CompilerGenerated]
  public void \u003CSetOld\u003Eb__31_0()
  {
    if (!this._followerInfo.Traits.Contains(FollowerTrait.TraitType.Mutated))
      this._followerInfo.StartingCursedState = Thought.OldAge;
    this.UpdateFollower();
  }

  [CompilerGenerated]
  public void \u003CSetIll\u003Eb__32_0()
  {
    if (!this._followerInfo.Traits.Contains(FollowerTrait.TraitType.Mutated))
      this._followerInfo.StartingCursedState = Thought.Ill;
    this.UpdateFollower();
  }

  [CompilerGenerated]
  public void \u003CSetFaithful\u003Eb__33_0()
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
        FollowerTrait.TraitType faithfulTrait = FollowerTrait.FaithfulTraits[UnityEngine.Random.Range(0, FollowerTrait.FaithfulTraits.Count)];
        if (!this._followerInfo.Traits.Contains(faithfulTrait))
        {
          this._followerInfo.Traits.Add(faithfulTrait);
          break;
        }
      }
    }
    this._followerInfo.TraitsSet = true;
  }

  [CompilerGenerated]
  public void \u003CSetMutated\u003Eb__34_0()
  {
    this._followerInfo.StartingCursedState = Thought.None;
    if (!this._followerInfo.Traits.Contains(FollowerTrait.TraitType.Mutated))
    {
      if (this._followerInfo.Traits.Count >= 3)
        this._followerInfo.Traits.RemoveAt(0);
      this._followerInfo.Traits.Add(FollowerTrait.TraitType.Mutated);
    }
    this.UpdateFollower();
    FollowerSpineEventListener componentInChildren = this.ShopKeeper.GetComponentInChildren<FollowerSpineEventListener>(true);
    if (!(bool) (UnityEngine.Object) componentInChildren)
      return;
    componentInChildren.RefreshRotValue();
  }
}
