// Decompiled with JetBrains decompiler
// Type: shopKeeperManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class shopKeeperManager : BaseMonoBehaviour
{
  public bool DailyShop;
  public FollowerLocation Location;
  private bool gotOne;
  public GameObject[] itemSlots;
  public List<BuyEntry> ItemsForSale;
  private bool containsInt;
  private int randomInt;
  public bool DecorationsForSale;
  public bool TarotCardShop;
  public bool AddCoolDownToNewItems;
  public GameObject SoldOutSign;
  private GameObject SoldOutSignObject;
  public GameObject BoughtItemBark;
  public GameObject NormalBark;
  public GameObject CantAffordBark;
  private List<StructureBrain.TYPES> availableUnlocks = new List<StructureBrain.TYPES>();
  public List<int> PickedItems = new List<int>();
  private bool FoundOne;
  private int LoopCount;
  public List<BuyEntry> LocalList = new List<BuyEntry>();
  public ShopLocationTracker shop;
  [SerializeField]
  private Vector3 SoldOutOffset = Vector3.zero;

  private void Start()
  {
    if (this.TarotCardShop)
      this.InitTarotShop();
    else if (!this.DecorationsForSale && !this.DailyShop)
      this.InitShop();
    else if (this.DecorationsForSale)
      this.GetAvailableDecorations();
    else
      this.InitDailyShop();
    if ((bool) (Object) this.BoughtItemBark)
      this.BoughtItemBark.SetActive(false);
    if (!(bool) (Object) this.CantAffordBark)
      return;
    this.CantAffordBark.SetActive(false);
  }

  private void GetAvailableDecorations()
  {
    Debug.Log((object) "Init Decoration Shop");
    this.availableUnlocks.Clear();
    this.availableUnlocks = new List<StructureBrain.TYPES>();
    Debug.Log((object) ("Player Farming Location: " + (object) this.Location));
    foreach (StructureBrain.TYPES types in DataManager.Instance.GetDecorationListFromLocation(this.Location))
      this.availableUnlocks.Add(types);
    if (DataManager.Instance.GetDecorationListFromCategory(DataManager.DecorationType.All).Count < 3)
    {
      foreach (StructureBrain.TYPES types in DataManager.Instance.GetDecorationListFromCategory(DataManager.DecorationType.All))
        this.availableUnlocks.Add(types);
    }
    this.InitShopDecoration();
  }

  private bool ItemAlreadyForSale(StructureBrain.TYPES type)
  {
    foreach (GameObject itemSlot in this.itemSlots)
    {
      if (itemSlot.GetComponent<Interaction_BuyItem>().itemForSale.decorationToBuy == type)
        return true;
    }
    return false;
  }

  private void InitDailyShop()
  {
    Debug.Log((object) "Init Daily Shop");
    if (!DataManager.Instance.CheckShopExists(this.Location, this.gameObject.name))
    {
      this.shop = new ShopLocationTracker(this.Location);
      DataManager.Instance.Shops.Add(this.shop);
      Debug.Log((object) "Create Shop");
      this.shop.lastDayUsed = -1;
      this.shop.shopKeeperName = this.gameObject.name;
    }
    else
      this.shop = DataManager.Instance.GetShop(this.Location, this.gameObject.name);
    if (this.shop.lastDayUsed != TimeManager.CurrentDay)
    {
      Debug.Log((object) "Create Shop items");
      this.shop.itemsForSale.Clear();
      for (int index = 0; index < this.itemSlots.Length; ++index)
      {
        this.PickedItems.Clear();
        Interaction_BuyItem component1 = this.itemSlots[index].GetComponent<Interaction_BuyItem>();
        InventoryItemDisplay component2 = this.itemSlots[index].GetComponent<InventoryItemDisplay>();
        this.gotOne = false;
        int num = 0;
        while (!this.gotOne)
        {
          ++num;
          if (num > 30)
          {
            Debug.Log((object) "Cant Find Item for Sale that isnt picked");
            if (!((Object) this.SoldOutSign != (Object) null))
              return;
            this.SoldOutSignObject = Object.Instantiate<GameObject>(this.SoldOutSign, component1.transform.position + this.SoldOutOffset, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f), this.gameObject.transform);
            Object.Destroy((Object) component1.gameObject);
            return;
          }
          this.randomInt = Random.Range(0, this.ItemsForSale.Count);
          if (!this.ItemsForSale[this.randomInt].pickedForSale)
          {
            Debug.Log((object) "Found False");
            component1.itemForSale = this.ItemsForSale[this.randomInt];
            component1.GetCost();
            this.ItemsForSale[this.randomInt].pickedForSale = true;
            if (this.ItemsForSale[this.randomInt].quantity == 0)
              this.ItemsForSale[this.randomInt].quantity = 1;
            if ((Object) component2 != (Object) null && this.ItemsForSale[index].itemToBuy != InventoryItem.ITEM_TYPE.NONE)
              component2.SetImage(this.ItemsForSale[this.randomInt].itemToBuy);
            component1.itemForSale.quantity = this.ItemsForSale[this.randomInt].quantity;
            if (component1.itemForSale.quantity > 1)
              component1.updateQuantity();
            this.shop.itemsForSale.Add(component1.itemForSale);
            component1.shopKeeperManager = this;
            this.gotOne = true;
          }
        }
      }
      this.shop.lastDayUsed = TimeManager.CurrentDay;
      DataManager.Instance.UpdateShop(this.shop);
    }
    else
    {
      Debug.Log((object) ("Shop Exists, amount of items: " + (object) this.shop.itemsForSale.Count));
      for (int index = 0; index < this.itemSlots.Length; ++index)
      {
        if (!this.shop.itemsForSale[index].Bought)
        {
          InventoryItemDisplay component3 = this.itemSlots[index].GetComponent<InventoryItemDisplay>();
          Interaction_BuyItem component4 = this.itemSlots[index].GetComponent<Interaction_BuyItem>();
          component4.itemForSale = new BuyEntry(this.shop.itemsForSale[index].itemToBuy, this.shop.itemsForSale[index].costType, this.shop.itemsForSale[index].itemCost, this.shop.itemsForSale[index].quantity);
          Debug.Log((object) ("BUY ITEM: " + (object) component4.itemForSale.decorationToBuy));
          component4.GetCost();
          component4.shopKeeperManager = this;
          int itemToBuy = (int) this.shop.itemsForSale[index].itemToBuy;
          component3.SetImage((InventoryItem.ITEM_TYPE) itemToBuy);
          component4.updateQuantity();
        }
        else
        {
          this.itemSlots[index].SetActive(false);
          if ((Object) this.SoldOutSign != (Object) null)
            this.SoldOutSignObject = Object.Instantiate<GameObject>(this.SoldOutSign, this.itemSlots[index].transform.position + this.SoldOutOffset, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f), this.gameObject.transform);
        }
      }
    }
  }

  private void CreateSoldOutSign(GameObject buyItem)
  {
    if ((Object) this.SoldOutSign == (Object) null)
      return;
    this.SoldOutSignObject = Object.Instantiate<GameObject>(this.SoldOutSign, buyItem.transform.position + this.SoldOutOffset, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f), this.gameObject.transform);
    Object.Destroy((Object) buyItem.gameObject);
  }

  private void InitShopDecoration()
  {
    if (!DataManager.Instance.CheckShopExists(this.Location, this.gameObject.name))
    {
      this.shop = new ShopLocationTracker(this.Location);
      DataManager.Instance.Shops.Add(this.shop);
      Debug.Log((object) "Create Shop");
      this.shop.lastDayUsed = -1;
      this.shop.shopKeeperName = this.gameObject.name;
    }
    else
      this.shop = DataManager.Instance.GetShop(this.Location, this.gameObject.name);
    if (this.shop.lastDayUsed != TimeManager.CurrentDay)
    {
      Debug.Log((object) "Init Dec Shop: Havent picked items for the day");
      this.shop.itemsForSale.Clear();
      this.PickedItems.Clear();
      this.LoopCount = this.itemSlots.Length;
      int count = this.availableUnlocks.Count;
      for (int index = 0; index < this.LoopCount; ++index)
      {
        Interaction_BuyItem component = this.itemSlots[index].GetComponent<Interaction_BuyItem>();
        Debug.Log((object) ("Unlocks Count: " + (object) this.availableUnlocks.Count));
        Debug.Log((object) ("Unlocks Count: " + (object) this.LoopCount));
        if (count > 0)
        {
          this.FoundOne = false;
          int num = 0;
          while (!this.FoundOne)
          {
            ++num;
            if (num > 60)
            {
              this.CreateSoldOutSign(component.gameObject);
              Debug.Log((object) "Cant Find Item for Sale that isnt picked");
              break;
            }
            this.randomInt = Random.Range(0, this.availableUnlocks.Count);
            Debug.Log((object) ("Random Int = " + (object) this.randomInt));
            Debug.Log((object) ("Available Unlocks Count = " + (object) this.availableUnlocks.Count));
            this.FoundOne = !DataManager.Instance.UnlockedStructures.Contains(this.availableUnlocks[this.randomInt]);
            if (this.PickedItems.Count > 0 && this.PickedItems.Contains(this.randomInt))
              this.FoundOne = false;
          }
          if (this.FoundOne)
          {
            this.PickedItems.Add(this.randomInt);
            component.itemForSale.Decoration = true;
            DataManager.DecorationType decorationType = DataManager.GetDecorationType(this.availableUnlocks[this.randomInt]);
            component.itemForSale = new BuyEntry(this.availableUnlocks[this.randomInt], DataManager.GetDecorationTypeCost(decorationType).costType, DataManager.GetDecorationTypeCost(decorationType).costAmount);
            Debug.Log((object) ("BUY ITEM: " + (object) component.itemForSale.decorationToBuy));
            component.GetCost();
            component.GetDecoration();
            if (component.itemForSale.quantity == 0)
              component.itemForSale.quantity = 1;
            this.shop.itemsForSale.Add(component.itemForSale);
            this.shop.lastDayUsed = TimeManager.CurrentDay;
            --count;
          }
        }
        else
          this.CreateSoldOutSign(component.gameObject);
      }
    }
    else
    {
      Debug.Log((object) "Init Dec Shop: Picked items for the day");
      for (int index = 0; index < this.itemSlots.Length; ++index)
      {
        if (index < this.shop.itemsForSale.Count && !this.shop.itemsForSale[index].Bought)
        {
          Interaction_BuyItem component = this.itemSlots[index].GetComponent<Interaction_BuyItem>();
          component.itemForSale.Decoration = true;
          component.itemForSale = new BuyEntry(this.shop.itemsForSale[index].decorationToBuy, this.shop.itemsForSale[index].costType, this.shop.itemsForSale[index].itemCost);
          Debug.Log((object) ("BUY ITEM: " + (object) component.itemForSale.decorationToBuy));
          component.GetCost();
          component.GetDecoration();
        }
        else
        {
          this.itemSlots[index].SetActive(false);
          if ((Object) this.SoldOutSign != (Object) null)
          {
            this.itemSlots[index].gameObject.SetActive(false);
            this.SoldOutSignObject = Object.Instantiate<GameObject>(this.SoldOutSign, this.itemSlots[index].transform.position + this.SoldOutOffset, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f), this.gameObject.transform);
          }
        }
      }
    }
  }

  private void InitTarotShop()
  {
    Debug.Log((object) "Init Tarot Shop");
    for (int index = 0; index < this.itemSlots.Length; ++index)
    {
      Interaction_BuyItem component1 = this.itemSlots[index].GetComponent<Interaction_BuyItem>();
      InventoryItemDisplay component2 = this.itemSlots[index].GetComponent<InventoryItemDisplay>();
      if (DataManager.TrinketUnlocked(this.ItemsForSale[index].Card))
      {
        component1.itemForSale.Card = this.ItemsForSale[index].Card;
        component1.itemForSale = this.ItemsForSale[index];
        component1.GetCost();
        this.ItemsForSale[index].pickedForSale = true;
        if (this.ItemsForSale[this.randomInt].quantity == 0)
          this.ItemsForSale[this.randomInt].quantity = 1;
        if ((Object) component2 != (Object) null && this.ItemsForSale[index].itemToBuy != InventoryItem.ITEM_TYPE.NONE)
          component2.SetImage(this.ItemsForSale[index].itemToBuy);
        component1.itemForSale.quantity = this.ItemsForSale[index].quantity;
        if (component1.itemForSale.quantity > 1)
          component1.updateQuantity();
      }
      else
      {
        component1.AlreadyBought();
        if ((Object) this.SoldOutSign != (Object) null)
        {
          component1.gameObject.SetActive(false);
          this.SoldOutSignObject = Object.Instantiate<GameObject>(this.SoldOutSign, component1.transform.position + this.SoldOutOffset, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f), this.gameObject.transform);
        }
      }
    }
  }

  public void DoublePrices()
  {
    foreach (GameObject itemSlot in this.itemSlots)
      itemSlot.GetComponent<Interaction_BuyItem>().DoublePrice = true;
  }

  public void InitShop()
  {
    Debug.Log((object) "Init Normal Shop");
    for (int index = 0; index < this.itemSlots.Length; ++index)
    {
      Interaction_BuyItem component1 = this.itemSlots[index].GetComponent<Interaction_BuyItem>();
      InventoryItemDisplay component2 = this.itemSlots[index].GetComponent<InventoryItemDisplay>();
      this.gotOne = false;
      while (!this.gotOne)
      {
        if (0 + 1 > 30)
        {
          Debug.Log((object) "Cant Find Item for Sale that isnt picked");
          break;
        }
        this.randomInt = Random.Range(0, this.ItemsForSale.Count);
        if (!this.ItemsForSale[this.randomInt].pickedForSale)
        {
          Debug.Log((object) "Found False");
          component1.itemForSale = this.ItemsForSale[this.randomInt];
          component1.GetCost();
          this.ItemsForSale[this.randomInt].pickedForSale = true;
          if (this.ItemsForSale[this.randomInt].quantity == 0)
            this.ItemsForSale[this.randomInt].quantity = 1;
          if ((Object) component2 != (Object) null && this.ItemsForSale[index].itemToBuy != InventoryItem.ITEM_TYPE.NONE)
            component2.SetImage(this.ItemsForSale[this.randomInt].itemToBuy);
          component1.itemForSale.quantity = this.ItemsForSale[this.randomInt].quantity;
          if (component1.itemForSale.quantity > 1)
            component1.updateQuantity();
          this.gotOne = true;
        }
      }
    }
  }
}
