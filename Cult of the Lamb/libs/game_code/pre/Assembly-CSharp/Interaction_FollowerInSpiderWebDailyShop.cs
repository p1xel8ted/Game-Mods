// Decompiled with JetBrains decompiler
// Type: Interaction_FollowerInSpiderWebDailyShop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_FollowerInSpiderWebDailyShop : Interaction
{
  private string sRequires;
  public bool ForceSpecificSkin;
  [SpineSkin("", "", true, false, false)]
  public string ForceSkin = "";
  private bool doneCantAffordAnimation;
  public GameObject ShopKeeper;
  public SkeletonAnimation ShopKeeperSpine;
  public SkeletonAnimation followerSpine;
  public int Cost;
  public List<int> _Cost = new List<int>()
  {
    0,
    5,
    10,
    20,
    30
  };
  private FollowerInfo _followerInfo;
  private FollowerOutfit _outfit;
  private FollowerInfoManager wim;
  private int CostMax;
  private bool Activated;
  public SimpleBark simpleBarkBeforeCard;
  private StateMachine CompanionState;
  public GameObject ConversionBone;

  private void Start()
  {
    if (DataManager.Instance.LastDayUsedFollowerShop != TimeManager.CurrentDay)
      this.CreateNewFollower();
    else
      this.SetExistingFollower();
  }

  public override void OnEnableInteraction()
  {
    if (DataManager.Instance.LastDayUsedFollowerShop != TimeManager.CurrentDay)
      this.CreateNewFollower();
    else
      this.SetExistingFollower();
    base.OnEnableInteraction();
  }

  private void SetExistingFollower()
  {
    this._followerInfo = DataManager.Instance.FollowerForSale;
    if (this._followerInfo != null)
    {
      this._outfit = new FollowerOutfit(this._followerInfo);
      this._outfit.SetOutfit(this.followerSpine, false);
      this.SetCost();
      this.UpdateLocalisation();
    }
    else
      this.gameObject.SetActive(false);
  }

  private void SetCost()
  {
    this.CostMax = Mathf.Clamp(DataManager.Instance.Followers.Count, 0, 5);
    this.Cost = this._Cost[Mathf.Min(this._Cost.Count, this.CostMax)];
  }

  private void CreateNewFollower()
  {
    DataManager.Instance.LastDayUsedFollowerShop = TimeManager.CurrentDay;
    this._followerInfo = FollowerInfo.NewCharacter(PlayerFarming.Location, this.ForceSpecificSkin ? this.ForceSkin : "");
    this._outfit = new FollowerOutfit(this._followerInfo);
    this._outfit.SetOutfit(this.followerSpine, false);
    DataManager.Instance.FollowerForSale = this._followerInfo;
    this.SetCost();
    this.UpdateLocalisation();
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sRequires = ScriptLocalization.Interactions.Requires;
  }

  public override void GetLabel()
  {
    string str;
    if (!this.Activated)
      str = $"{this.sRequires} {(object) this.Cost} <sprite name=\"icon_blackgold\">";
    else
      str = "";
    this.Label = str;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (Inventory.GetItemQuantity(20) >= this.Cost || CheatConsole.BuildingsFree)
      this.StartCoroutine((IEnumerator) this.Purchase());
    else
      this.SpiderAnimationCantAfford();
  }

  private IEnumerator Purchase()
  {
    Interaction_FollowerInSpiderWebDailyShop spiderWebDailyShop = this;
    spiderWebDailyShop.simpleBarkBeforeCard.Close();
    spiderWebDailyShop.simpleBarkBeforeCard.enabled = false;
    GameManager.GetInstance().OnConversationNew(false);
    GameManager.GetInstance().OnConversationNext(spiderWebDailyShop.ConversionBone, 4f);
    GameManager.GetInstance().AddPlayerToCamera();
    spiderWebDailyShop.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    for (int i = 0; i < spiderWebDailyShop.Cost; ++i)
    {
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", spiderWebDailyShop.transform.position);
      ResourceCustomTarget.Create(spiderWebDailyShop.ShopKeeper, spiderWebDailyShop.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) null);
      yield return (object) new WaitForSeconds(0.1f);
    }
    Inventory.ChangeItemQuantity(20, -spiderWebDailyShop.Cost);
    spiderWebDailyShop.StartCoroutine((IEnumerator) spiderWebDailyShop.BoughtFollower());
    spiderWebDailyShop.Activated = true;
  }

  private void SpiderAnimationBoughtItem()
  {
    this.ShopKeeperSpine.AnimationState.SetAnimation(0, "buy", true);
  }

  private void SpiderAnimationCantAfford()
  {
    this.ShopKeeperSpine.AnimationState.SetAnimation(0, "cant-afford", false);
    this.ShopKeeperSpine.AnimationState.AddAnimation(0, "animation", true, 1.7f);
  }

  private IEnumerator BoughtFollower()
  {
    Interaction_FollowerInSpiderWebDailyShop spiderWebDailyShop = this;
    spiderWebDailyShop.SpiderAnimationBoughtItem();
    spiderWebDailyShop.Interactable = false;
    HealthPlayer h = spiderWebDailyShop.state.GetComponent<HealthPlayer>();
    h.untouchable = true;
    spiderWebDailyShop.state.GetComponentInChildren<SimpleSpineAnimator>();
    yield return (object) new WaitForSeconds(spiderWebDailyShop.followerSpine.AnimationState.SetAnimation(0, "spider-out", false).Animation.Duration);
    float duration = spiderWebDailyShop.followerSpine.AnimationState.SetAnimation(0, "spider-pop2", false).Animation.Duration;
    CameraManager.shakeCamera(0.5f, (float) UnityEngine.Random.Range(0, 360));
    yield return (object) new WaitForSeconds(duration);
    FollowerInfo newRecruit = FollowerManager.CreateNewRecruit(FollowerLocation.Base, spiderWebDailyShop._followerInfo.SkinName, NotificationCentre.NotificationType.NewRecruit);
    ThoughtData data = FollowerThoughts.GetData((double) UnityEngine.Random.value >= 0.699999988079071 ? ((double) UnityEngine.Random.value > 0.30000001192092896 ? Thought.InstantBelieverRescued : Thought.ResentfulRescued) : Thought.GratefulRecued);
    data.Init();
    newRecruit.Thoughts.Add(data);
    DataManager.Instance.FollowerForSale = (FollowerInfo) null;
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForSeconds(0.75f);
    spiderWebDailyShop.simpleBarkBeforeCard.enabled = true;
    UnityEngine.Object.Destroy((UnityEngine.Object) spiderWebDailyShop.gameObject);
    h.untouchable = false;
  }
}
