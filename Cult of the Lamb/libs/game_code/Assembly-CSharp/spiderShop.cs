// Decompiled with JetBrains decompiler
// Type: spiderShop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class spiderShop : MonoBehaviour
{
  [SerializeField]
  public GameObject returnCustomer0;
  [SerializeField]
  public GameObject returnCustomer1;
  [SerializeField]
  public GameObject badFollower;
  [SerializeField]
  public GameObject postGameConvo;
  [SerializeField]
  public GameObject rotFollowerConvo;
  [SerializeField]
  public GameObject pedigreeParticles;
  [SerializeField]
  public Interaction_FollowerInSpiderWeb shopKeeper;
  public List<FollowersToBuy> followersToBuy = new List<FollowersToBuy>();
  [SerializeField]
  public Transform SpiderSeller;
  public int TotalCount;
  public bool gotOne;
  public FollowersToBuy pickedFollower;

  public GameObject PostGameConvo => this.postGameConvo;

  public GameObject RotFollowerConvo => this.rotFollowerConvo;

  public Interaction_FollowerInSpiderWeb ShopKeeper => this.shopKeeper;

  public bool canOnboardRotFollower
  {
    get
    {
      return PlayerFarming.Location == FollowerLocation.Base && DataManager.Instance.RecruitedRotFollower && !DataManager.Instance.OnboardedRotHelobFollowers;
    }
  }

  public void ChooseFollower()
  {
    while (!this.gotOne)
    {
      foreach (FollowersToBuy followersToBuy in this.followersToBuy)
      {
        if (UnityEngine.Random.Range(0, 100) < followersToBuy.chanceToSpawn)
        {
          this.pickedFollower = followersToBuy;
          this.gotOne = true;
        }
      }
    }
    this.shopKeeper.followerToBuy = this.pickedFollower;
    switch (this.pickedFollower.followerTypes)
    {
      case FollowersToBuy.FollowerBuyTypes.Ill:
        this.shopKeeper.SetIll();
        break;
      case FollowersToBuy.FollowerBuyTypes.Old:
        this.shopKeeper.SetOld();
        break;
      case FollowersToBuy.FollowerBuyTypes.Faithful:
        this.shopKeeper.SetFaithful();
        break;
    }
    if (this.canOnboardRotFollower)
    {
      this.shopKeeper.free = true;
      this.shopKeeper.SetMutated();
      this.rotFollowerConvo.gameObject.SetActive(true);
      UnityEngine.Object.Destroy((UnityEngine.Object) this.postGameConvo);
    }
    else if (SeasonsManager.Active && !DataManager.Instance.HasProducedChosenOne && ((double) UnityEngine.Random.value < 0.20000000298023224 || DataManager.Instance.BeatenExecutioner && !DataManager.Instance.HasGivenPedigreeFollower) && !GameManager.IsDungeon(PlayerFarming.Location))
    {
      if (this.shopKeeper._followerInfo.Traits.Count > 0)
        this.shopKeeper._followerInfo.Traits.RemoveAt(0);
      this.shopKeeper._followerInfo.Traits.Add(FollowerTrait.TraitType.PureBlood_1);
      this.shopKeeper._followerInfo.TraitsSet = true;
      this.pedigreeParticles.gameObject.SetActive(true);
    }
    else
    {
      if (!DataManager.Instance.RecruitedRotFollower || TimeManager.CurrentDay % 2 != 0 || !DataManager.Instance.OnboardedRotHelobFollowers || PlayerFarming.Location != FollowerLocation.Dungeon1_5 && PlayerFarming.Location != FollowerLocation.Dungeon1_6 && GameManager.IsDungeon(PlayerFarming.Location))
        return;
      this.shopKeeper.SetMutated();
    }
  }

  public void Start()
  {
    this.ChooseFollower();
    Debug.Log((object) ("s_ " + PlayerFarming.Location.ToString()));
    Debug.Log((object) ("s_ Beaten dungeon 1? " + DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_1).ToString()));
    if (PlayerFarming.Location == FollowerLocation.Base && !DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_1))
      this.gameObject.SetActive(false);
    this.TotalCount = DataManager.Instance.FollowerShopUses;
    if (DataManager.Instance.DeathCatBeaten || !((UnityEngine.Object) this.postGameConvo != (UnityEngine.Object) null))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.postGameConvo);
  }

  public void BoughtFollower()
  {
    if (this.shopKeeper.IsDungeon)
    {
      ++DataManager.Instance.FollowerShopUses;
      this.TotalCount = DataManager.Instance.FollowerShopUses;
    }
    if (this.shopKeeper._followerInfo.Traits.Contains(FollowerTrait.TraitType.PureBlood_1))
      DataManager.Instance.HasGivenPedigreeFollower = true;
    if (!(bool) (UnityEngine.Object) this.pedigreeParticles)
      return;
    this.pedigreeParticles.gameObject.SetActive(false);
  }

  public void GiveTarot() => this.StartCoroutine((IEnumerator) this.GiveTarotRoutine());

  public IEnumerator GiveTarotRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    spiderShop spiderShop = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      Debug.Log((object) "Spawn Tarot");
      GameManager.GetInstance().OnConversationNew();
      PlayerFarming.Instance._state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      GameObject gameObject = TarotCustomTarget.Create(spiderShop.SpiderSeller.transform.position, PlayerFarming.Instance.transform.position, 1.5f, TarotCards.Card.Arrows, new System.Action(spiderShop.pickedUp)).gameObject;
      GameManager.GetInstance().OnConversationNext(gameObject);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    ++DataManager.Instance.FollowerShopUses;
    spiderShop.shopKeeper.Interactable = false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForEndOfFrame();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void pickedUp()
  {
    GameManager.GetInstance().OnConversationEnd();
    PlayerFarming.SetStateForAllPlayers();
    this.shopKeeper.Interactable = true;
  }

  public void CheckCount()
  {
    if (!this.shopKeeper.IsDungeon)
      return;
    int followerShopUses = DataManager.Instance.FollowerShopUses;
    SimpleBark componentInChildren = this.shopKeeper.normalBark.gameObject.GetComponentInChildren<SimpleBark>();
    if (followerShopUses > 3 && !DataManager.Instance.PlayerFoundTrinkets.Contains(TarotCards.Card.Arrows))
    {
      componentInChildren.Close();
      this.returnCustomer1.SetActive(true);
    }
    else
    {
      switch (followerShopUses)
      {
        case 1:
          Debug.Log((object) "Free Follower");
          this.shopKeeper.free = true;
          componentInChildren.Close();
          this.returnCustomer0.SetActive(true);
          break;
        case 3:
          componentInChildren.Close();
          this.returnCustomer1.SetActive(true);
          break;
        case 5:
          this.shopKeeper.SetOld();
          this.shopKeeper.free = true;
          this.badFollower.SetActive(true);
          break;
        case 8:
          componentInChildren.Close();
          this.shopKeeper.free = true;
          this.returnCustomer0.SetActive(true);
          break;
        case 10:
          this.shopKeeper.SetIll();
          this.shopKeeper.free = true;
          this.badFollower.SetActive(true);
          break;
      }
    }
  }

  public void OnEnable()
  {
    this.shopKeeper.FollowerCreated += new System.Action(this.CheckCount);
    this.shopKeeper.BoughtFollowerCallback += new System.Action(this.BoughtFollower);
    LocationManager.OnPlayerLocationSet += new System.Action(this.CheckIfShouldShow);
  }

  public void OnDisable()
  {
    this.shopKeeper.FollowerCreated -= new System.Action(this.CheckCount);
    this.shopKeeper.BoughtFollowerCallback -= new System.Action(this.BoughtFollower);
    LocationManager.OnPlayerLocationSet -= new System.Action(this.CheckIfShouldShow);
  }

  public void CheckIfShouldShow()
  {
    Debug.Log((object) "s_ ========= Check if I should show");
    Debug.Log((object) ("s_PlayerFarming.Location: " + PlayerFarming.Location.ToString()));
    Debug.Log((object) ("s_DataManager.Instance.UnlockedDungeonDoor.Contains(FollowerLocation.Dungeon1_2): " + DataManager.Instance.UnlockedDungeonDoor.Contains(FollowerLocation.Dungeon1_2).ToString()));
    if (PlayerFarming.Location != FollowerLocation.Base || DataManager.Instance.UnlockedDungeonDoor.Contains(FollowerLocation.Dungeon1_2) || DataManager.Instance.DeathCatBeaten)
      return;
    Debug.Log((object) "s_Turn him off!!");
    this.gameObject.SetActive(false);
  }
}
