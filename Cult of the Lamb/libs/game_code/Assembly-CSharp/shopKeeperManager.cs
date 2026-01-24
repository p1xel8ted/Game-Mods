// Decompiled with JetBrains decompiler
// Type: shopKeeperManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class shopKeeperManager : BaseMonoBehaviour
{
  public bool DailyShop;
  public FollowerLocation Location;
  public bool InitialiseDailyShop = true;
  public bool gotOne;
  public GameObject[] itemSlots;
  public List<BuyEntry> ItemsForSale;
  public bool containsInt;
  public int randomInt;
  public bool DecorationsForSale;
  public bool TarotCardShop;
  public bool AddCoolDownToNewItems;
  public GameObject SoldOutSign;
  public GameObject SoldOutSignObject;
  public GameObject BoughtItemBark;
  public GameObject NormalBark;
  public GameObject CantAffordBark;
  public Dictionary<int, GameObject> SoldOutSigns = new Dictionary<int, GameObject>();
  public UnityEvent<BuyEntry> OnItemBought;
  public List<StructureBrain.TYPES> availableUnlocks = new List<StructureBrain.TYPES>();
  public List<int> PickedItems = new List<int>();
  public bool FoundOne;
  public int LoopCount;
  [CompilerGenerated]
  public ShopLocationTracker \u003Cshop\u003Ek__BackingField;
  [SerializeField]
  public Vector3 SoldOutOffset = Vector3.zero;
  public bool GetCostFromComponent;

  public void Start()
  {
    if (this.TarotCardShop)
      this.InitTarotShop();
    else if (!this.DecorationsForSale && !this.DailyShop)
      this.InitShop();
    else if (this.DecorationsForSale)
      this.GetAvailableDecorations();
    else if (this.InitialiseDailyShop)
      this.InitDailyShop();
    if ((bool) (Object) this.BoughtItemBark)
      this.BoughtItemBark.SetActive(false);
    if (!(bool) (Object) this.CantAffordBark)
      return;
    this.CantAffordBark.SetActive(false);
  }

  public void GetAvailableDecorations()
  {
    Debug.Log((object) "Init Decoration Shop");
    this.availableUnlocks.Clear();
    this.availableUnlocks = new List<StructureBrain.TYPES>();
    Debug.Log((object) ("Player Farming Location: " + this.Location.ToString()));
    foreach (StructureBrain.TYPES types in DataManager.Instance.GetDecorationListFromLocation(this.Location))
      this.availableUnlocks.Add(types);
    if (this.Location != FollowerLocation.DecorationShop_Inside && DataManager.Instance.GetDecorationListFromCategory(DataManager.DecorationType.All).Count < 3)
    {
      foreach (StructureBrain.TYPES types in DataManager.Instance.GetDecorationListFromCategory(DataManager.DecorationType.All))
        this.availableUnlocks.Add(types);
    }
    this.InitShopDecoration();
  }

  public bool ItemAlreadyForSale(StructureBrain.TYPES type)
  {
    foreach (GameObject itemSlot in this.itemSlots)
    {
      if (itemSlot.GetComponent<Interaction_BuyItem>().itemForSale.decorationToBuy == type)
        return true;
    }
    return false;
  }

  public ShopLocationTracker shop
  {
    get => this.\u003Cshop\u003Ek__BackingField;
    set => this.\u003Cshop\u003Ek__BackingField = value;
  }

  public void InitDailyShop()
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
            this.SoldOutSigns.Add(index, this.SoldOutSignObject);
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
      Debug.Log((object) ("Shop Exists, amount of items: " + this.shop.itemsForSale.Count.ToString()));
      for (int index = 0; index < this.itemSlots.Length; ++index)
      {
        if (!this.shop.itemsForSale[index].Bought)
        {
          InventoryItemDisplay component3 = this.itemSlots[index].GetComponent<InventoryItemDisplay>();
          Interaction_BuyItem component4 = this.itemSlots[index].GetComponent<Interaction_BuyItem>();
          component4.itemForSale = new BuyEntry(this.shop.itemsForSale[index].itemToBuy, this.shop.itemsForSale[index].costType, this.shop.itemsForSale[index].itemCost, this.shop.itemsForSale[index].quantity);
          Debug.Log((object) ("BUY ITEM: " + component4.itemForSale.decorationToBuy.ToString()));
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
          {
            this.SoldOutSignObject = Object.Instantiate<GameObject>(this.SoldOutSign, this.itemSlots[index].transform.position + this.SoldOutOffset, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f), this.gameObject.transform);
            this.SoldOutSigns.Add(index, this.SoldOutSignObject);
          }
        }
      }
    }
  }

  public void CreateSoldOutSign(GameObject buyItem)
  {
    if ((Object) this.SoldOutSign == (Object) null)
      return;
    this.SoldOutSignObject = Object.Instantiate<GameObject>(this.SoldOutSign, buyItem.transform.position + this.SoldOutOffset, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f), this.gameObject.transform);
    Object.Destroy((Object) buyItem.gameObject);
  }

  public void InitShopDecoration(int forcedIndex = -1)
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
    if (this.shop.lastDayUsed != TimeManager.CurrentDay || forcedIndex != -1)
    {
      Debug.Log((object) "Init Dec Shop: Havent picked items for the day");
      if (forcedIndex == -1)
      {
        this.shop.itemsForSale.Clear();
        this.PickedItems.Clear();
      }
      int count = this.availableUnlocks.Count;
      this.LoopCount = this.itemSlots.Length;
      for (int index = 0; index < this.LoopCount; ++index)
      {
        if (forcedIndex == -1 || index == forcedIndex)
        {
          Interaction_BuyItem component = this.itemSlots[index].GetComponent<Interaction_BuyItem>();
          Debug.Log((object) ("Unlocks Count: " + this.availableUnlocks.Count.ToString()));
          Debug.Log((object) ("Unlocks Count: " + this.LoopCount.ToString()));
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
              Debug.Log((object) ("Random Int = " + this.randomInt.ToString()));
              Debug.Log((object) ("Available Unlocks Count = " + this.availableUnlocks.Count.ToString()));
              if (index == 0 && this.Location == FollowerLocation.DLC_ShrineRoom && this.availableUnlocks.Contains(StructureBrain.TYPES.DECORATION_DLC_WOLF_DIORAMA))
                this.randomInt = this.availableUnlocks.IndexOf(StructureBrain.TYPES.DECORATION_DLC_WOLF_DIORAMA);
              this.FoundOne = !DataManager.Instance.UnlockedStructures.Contains(this.availableUnlocks[this.randomInt]);
              if (this.PickedItems.Count > 0 && this.PickedItems.Contains(this.randomInt))
                this.FoundOne = false;
            }
            if (this.FoundOne)
            {
              this.PickedItems.Add(this.randomInt);
              component.itemForSale.Decoration = true;
              DataManager.DecorationType decorationType = DataManager.GetDecorationType(this.availableUnlocks[this.randomInt]);
              component.itemForSale = new BuyEntry(this.availableUnlocks[this.randomInt], DataManager.GetDecorationTypeCost(decorationType).costType, this.GetCostFromComponent ? component.itemForSale.itemCost : DataManager.GetDecorationTypeCost(decorationType).costAmount);
              switch (decorationType)
              {
                case DataManager.DecorationType.Rot:
                case DataManager.DecorationType.Wolf:
                case DataManager.DecorationType.Woolhaven:
                  switch (DataManager.Instance.WoolhavenDecorationCouunt)
                  {
                    case 2:
                    case 3:
                    case 4:
                      component.itemForSale.itemCost = 15;
                      break;
                    case 5:
                    case 6:
                    case 7:
                      component.itemForSale.itemCost = 20;
                      break;
                    case 8:
                    case 9:
                    case 10:
                      component.itemForSale.itemCost = 25;
                      break;
                    case 11:
                    case 12:
                    case 13:
                      component.itemForSale.itemCost = 30;
                      break;
                    case 14:
                    case 15:
                    case 16 /*0x10*/:
                    case 17:
                    case 18:
                    case 19:
                      component.itemForSale.itemCost = 35;
                      break;
                    default:
                      component.itemForSale.itemCost = 10;
                      break;
                  }
                  break;
              }
              component.GetCost();
              component.GetDecoration();
              component.itemForSale.Bought = false;
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
          Debug.Log((object) ("BUY ITEM: " + component.itemForSale.decorationToBuy.ToString()));
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
            this.SoldOutSigns.Add(index, this.SoldOutSignObject);
          }
        }
      }
    }
  }

  public int GetShopSlotIndex(Interaction_BuyItem buyItem)
  {
    for (int shopSlotIndex = 0; shopSlotIndex < this.itemSlots.Length; ++shopSlotIndex)
    {
      if ((Object) this.itemSlots[shopSlotIndex] != (Object) null && (Object) this.itemSlots[shopSlotIndex].GetComponent<Interaction_BuyItem>() == (Object) buyItem)
        return shopSlotIndex;
    }
    return 0;
  }

  public void InitTarotShop()
  {
    Debug.Log((object) "Init Tarot Shop");
    for (int index = 0; index < this.itemSlots.Length; ++index)
    {
      if (!((Object) this.itemSlots[index] == (Object) null))
      {
        Interaction_BuyItem component1 = this.itemSlots[index].GetComponent<Interaction_BuyItem>();
        InventoryItemDisplay component2 = this.itemSlots[index].GetComponent<InventoryItemDisplay>();
        if (!DataManager.TrinketUnlocked(this.ItemsForSale[index].Card))
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
            this.SoldOutSigns.Add(index, this.SoldOutSignObject);
          }
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
      int num = 0;
      while (!this.gotOne)
      {
        ++num;
        if (num > 30)
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
