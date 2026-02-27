// Decompiled with JetBrains decompiler
// Type: spiderShop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class spiderShop : MonoBehaviour
{
  [SerializeField]
  private GameObject returnCustomer0;
  [SerializeField]
  private GameObject returnCustomer1;
  [SerializeField]
  private GameObject badFollower;
  [SerializeField]
  private Interaction_FollowerInSpiderWeb shopKeeper;
  public List<FollowersToBuy> followersToBuy = new List<FollowersToBuy>();
  [SerializeField]
  private Transform SpiderSeller;
  public int TotalCount;
  private bool gotOne;
  private FollowersToBuy pickedFollower;

  private void ChooseFollower()
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
  }

  private void Start()
  {
    this.ChooseFollower();
    if (PlayerFarming.Location == FollowerLocation.Base && !DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_1))
      this.gameObject.SetActive(false);
    this.TotalCount = DataManager.Instance.FollowerShopUses;
  }

  public void BoughtFollower()
  {
    Debug.Log((object) "Check Count");
    if (!this.shopKeeper.IsDungeon)
      return;
    ++DataManager.Instance.FollowerShopUses;
    this.TotalCount = DataManager.Instance.FollowerShopUses;
  }

  public void GiveTarot() => this.StartCoroutine((IEnumerator) this.GiveTarotRoutine());

  private IEnumerator GiveTarotRoutine()
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

  private void pickedUp()
  {
    GameManager.GetInstance().OnConversationEnd();
    PlayerFarming.Instance._state.CURRENT_STATE = StateMachine.State.Idle;
    this.shopKeeper.Interactable = true;
  }

  public void CheckCount()
  {
    if (!this.shopKeeper.IsDungeon)
      return;
    int followerShopUses = DataManager.Instance.FollowerShopUses;
    SimpleBark component = this.shopKeeper.normalBark.gameObject.GetComponent<SimpleBark>();
    if (followerShopUses > 3 && !DataManager.Instance.PlayerFoundTrinkets.Contains(TarotCards.Card.Arrows))
    {
      component.Close();
      this.returnCustomer1.SetActive(true);
    }
    else
    {
      switch (followerShopUses)
      {
        case 1:
          Debug.Log((object) "Free Follower");
          this.shopKeeper.free = true;
          component.Close();
          this.returnCustomer0.SetActive(true);
          break;
        case 3:
          component.Close();
          this.returnCustomer1.SetActive(true);
          break;
        case 5:
          this.shopKeeper.SetOld();
          this.shopKeeper.free = true;
          this.badFollower.SetActive(true);
          break;
        case 8:
          component.Close();
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

  private void OnEnable()
  {
    this.shopKeeper.FollowerCreated += new System.Action(this.CheckCount);
    this.shopKeeper.BoughtFollowerCallback += new System.Action(this.BoughtFollower);
    LocationManager.OnPlayerLocationSet += new System.Action(this.CheckIfShouldShow);
  }

  private void OnDisable()
  {
    this.shopKeeper.FollowerCreated -= new System.Action(this.CheckCount);
    this.shopKeeper.BoughtFollowerCallback -= new System.Action(this.BoughtFollower);
    LocationManager.OnPlayerLocationSet -= new System.Action(this.CheckIfShouldShow);
  }

  private void CheckIfShouldShow()
  {
    if (PlayerFarming.Location != FollowerLocation.Base || DataManager.Instance.UnlockedDungeonDoor.Contains(FollowerLocation.Dungeon1_2))
      return;
    this.gameObject.SetActive(false);
  }
}
