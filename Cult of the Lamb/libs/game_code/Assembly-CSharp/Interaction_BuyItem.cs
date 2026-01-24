// Decompiled with JetBrains decompiler
// Type: Interaction_BuyItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using src.UINavigator;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_BuyItem : Interaction
{
  public bool AllowActivateDistanceOverride = true;
  [CompilerGenerated]
  public bool \u003CActivated\u003Ek__BackingField;
  public int devotionCost = 10;
  public BuyEntry itemForSale;
  public bool customItemForSale;
  public bool CreatedDiscount;
  public UnityEvent CallBack;
  [Range(0.0f, 100f)]
  public int chanceOfSale = 7;
  public bool SaleIsOn;
  public int SaleAmount;
  public float saleAmountFloat;
  public string BuyString;
  public string off;
  public bool UsingDecorations;
  public bool HideHudOnPurchase = true;
  public float AmountToDiscount;
  public shopKeeperManager shopKeeperManager;
  public bool DoublePrice;
  public int randomChanceOfSale;
  public GameObject DecorationItem;
  public string DecorationName = string.Empty;
  public string SaleText = string.Empty;
  public Vector3 BookTargetPosition;
  public float Timer;
  public GameObject PlayerPosition;
  public List<GameObject> clones;
  public GameObject spawnObject;

  public bool Activated
  {
    get => this.\u003CActivated\u003Ek__BackingField;
    set => this.\u003CActivated\u003Ek__BackingField = value;
  }

  public event System.Action OnItemBought;

  public void Start()
  {
    this.shopKeeperManager = this.GetComponentInParent<shopKeeperManager>();
    this.UpdateLocalisation();
    if (this.AllowActivateDistanceOverride)
      this.ActivateDistance = 2f;
    this.SaleIsOn = false;
    this.SaleAmount = 0;
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.randomChanceOfSale = UnityEngine.Random.Range(0, 100);
  }

  public int GetCost()
  {
    if (this.randomChanceOfSale <= this.chanceOfSale && !this.DoublePrice)
    {
      if (!this.CreatedDiscount)
      {
        this.SaleIsOn = true;
        this.SaleAmount = UnityEngine.Random.Range(1, 5);
        this.saleAmountFloat = (float) this.SaleAmount;
        this.saleAmountFloat *= 0.1f;
        this.CreatedDiscount = true;
      }
      this.AmountToDiscount = Mathf.Round((float) this.itemForSale.itemCost - (float) this.itemForSale.itemCost * this.saleAmountFloat);
      if ((double) this.saleAmountFloat < 1.0 && (double) this.AmountToDiscount < 1.0)
        this.AmountToDiscount = 1f;
      return this.devotionCost = (int) this.AmountToDiscount;
    }
    if (this.SaleIsOn)
    {
      this.AmountToDiscount = Mathf.Round((float) this.itemForSale.itemCost - (float) this.itemForSale.itemCost * this.saleAmountFloat);
      return this.devotionCost = (int) this.AmountToDiscount;
    }
    return this.DoublePrice ? this.itemForSale.itemCost * 2 : this.itemForSale.itemCost;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.BuyString = ScriptLocalization.Interactions.Buy;
    this.off = ScriptLocalization.UI_Generic.Off;
  }

  public void GetDecoration()
  {
    this.GetComponent<InventoryItemDisplay>().SetImage(TypeAndPlacementObjects.GetByType(this.itemForSale.decorationToBuy).IconImage, false);
    if (this.itemForSale.decorationToBuy == StructureBrain.TYPES.FARM_PLOT_SIGN)
    {
      this.transform.localScale = Vector3.one * 0.25f;
      for (int index = 0; index < this.transform.childCount; ++index)
        this.transform.GetChild(index).transform.localScale /= 2f;
    }
    else
      this.transform.localScale = Vector3.one * 0.12f;
  }

  public override void GetLabel()
  {
    if (this.itemForSale.Decoration && this.itemForSale.decorationToBuy == StructureBrain.TYPES.NONE && this.itemForSale.Decoration)
    {
      this.Interactable = false;
    }
    else
    {
      this.DecorationName = !this.itemForSale.Decoration ? (!this.itemForSale.TarotCard ? InventoryItem.LocalizedName(this.itemForSale.itemToBuy) : TarotCards.LocalisedName(this.itemForSale.Card)) : StructuresData.LocalizedName(this.itemForSale.decorationToBuy);
      if (this.DoublePrice)
        this.SaleText = "200%".Colour(Color.red);
      else if (this.SaleIsOn)
        this.SaleText = this.SaleAmount != 0 ? string.Format(this.off, (object) (float) (this.SaleAmount * 10)).Colour(Color.yellow) : string.Empty;
      if (!this.SaleIsOn)
        this.SaleText = string.Empty;
      string buy = ScriptLocalization.UI_ItemSelector_Context.Buy;
      string str;
      if (this.itemForSale.quantity > 1)
        str = string.Format(buy, (object) string.Join(" ", (object) this.itemForSale.quantity, (object) this.DecorationName), (object) CostFormatter.FormatCost(this.itemForSale.costType, this.GetCost()));
      else
        str = string.Format(buy, (object) this.DecorationName, (object) CostFormatter.FormatCost(this.itemForSale.costType, this.GetCost()));
      if (!string.IsNullOrEmpty(this.SaleText))
        str = string.Join(" ", str, this.SaleText);
      this.Label = str;
    }
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (Inventory.GetItemQuantity((int) this.itemForSale.costType) >= this.GetCost() && !this.Activated)
    {
      AudioManager.Instance.PlayOneShot("event:/shop/buy", this.gameObject);
      this.Activated = true;
      this.boughtItem(this.itemForSale);
      this.Interactable = false;
      if (this.HideHudOnPurchase)
        HUD_Manager.Instance.Hide(false);
      for (int index = 0; index < this.GetCost(); ++index)
      {
        if (index < 10)
        {
          Inventory.GetItemByType((int) this.itemForSale.costType);
          AudioManager.Instance.PlayOneShot("event:/followers/pop_in", this.playerFarming.transform.position);
          ResourceCustomTarget.Create(this.gameObject, this.playerFarming.gameObject.transform.position, this.itemForSale.costType, (System.Action) null);
        }
        Inventory.ChangeItemQuantity((int) this.itemForSale.costType, -1);
      }
      PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive);
      if (this.itemForSale.TarotCard || this.itemForSale.Decoration)
        this.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      this.StartCoroutine((IEnumerator) this.DelayOnActivate());
      CameraManager.shakeCamera(0.3f, (float) UnityEngine.Random.Range(0, 360));
    }
    else
      this.cantAfford();
  }

  public void AlreadyBought() => this.gameObject.SetActive(false);

  public void cantAfford()
  {
    AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback", this.gameObject);
    this.playerFarming.indicator.PlayShake();
    PlayerFarming.SetStateForAllPlayers();
    if (!((UnityEngine.Object) this.shopKeeperManager.CantAffordBark != (UnityEngine.Object) null))
      return;
    if ((UnityEngine.Object) this.shopKeeperManager.NormalBark != (UnityEngine.Object) null)
      this.shopKeeperManager.NormalBark.SetActive(false);
    this.shopKeeperManager.CantAffordBark.SetActive(true);
  }

  public void boughtItem(BuyEntry item)
  {
    if ((UnityEngine.Object) this.shopKeeperManager.BoughtItemBark != (UnityEngine.Object) null)
    {
      if ((UnityEngine.Object) this.shopKeeperManager.NormalBark != (UnityEngine.Object) null)
        this.shopKeeperManager.NormalBark.SetActive(false);
      this.shopKeeperManager.BoughtItemBark.SetActive(true);
    }
    switch (DataManager.GetDecorationType(item.decorationToBuy))
    {
      case DataManager.DecorationType.Rot:
      case DataManager.DecorationType.Wolf:
      case DataManager.DecorationType.Woolhaven:
        ++DataManager.Instance.WoolhavenDecorationCouunt;
        break;
    }
    this.shopKeeperManager.OnItemBought?.Invoke(item);
  }

  public IEnumerator DelayOnActivate()
  {
    yield return (object) new WaitForSeconds(0.25f);
    this.Activate();
  }

  public IEnumerator BoughtCustomItem()
  {
    yield return (object) new WaitForSeconds(0.2f);
    this.CallBack?.Invoke();
  }

  public override void Update()
  {
    base.Update();
    int num = (UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null ? 1 : 0;
  }

  public void updateQuantity()
  {
    this.clones.Clear();
    if (this.itemForSale.quantity <= 1)
      return;
    SpriteRenderer component = this.gameObject.GetComponent<SpriteRenderer>();
    for (int index = 0; index < this.itemForSale.quantity; ++index)
    {
      if ((UnityEngine.Object) this.spawnObject != (UnityEngine.Object) null)
      {
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.spawnObject, new Vector3(UnityEngine.Random.Range(-0.05f, 0.05f), UnityEngine.Random.Range(-0.025f, 0.025f), 0.0f) + this.transform.position, this.transform.rotation, this.transform.parent);
        gameObject.SetActive(true);
        gameObject.GetComponent<SpriteRenderer>().sprite = component.sprite;
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        this.clones.Add(gameObject);
      }
    }
  }

  public void Activate()
  {
    Debug.Log((object) "activate");
    System.Action onItemBought = this.OnItemBought;
    if (onItemBought != null)
      onItemBought();
    if (this.customItemForSale)
      return;
    if (this.itemForSale.Decoration)
    {
      BiomeConstants.Instance.EmitSmokeExplosionVFX(this.transform.position);
      RumbleManager.Instance.Rumble();
      StructuresData.CompleteResearch(this.itemForSale.decorationToBuy);
      StructuresData.SetRevealed(this.itemForSale.decorationToBuy);
      foreach (BuyEntry buyEntry in this.shopKeeperManager.shop.itemsForSale)
      {
        if (buyEntry.decorationToBuy == this.itemForSale.decorationToBuy)
        {
          buyEntry.Bought = true;
          break;
        }
      }
      DataManager.Instance.UpdateShop(this.shopKeeperManager.shop);
      MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = this.playerFarming;
      UINewItemOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowNewItemOverlay();
      overlayController.pickedBuilding = this.itemForSale.decorationToBuy;
      overlayController.Show(UINewItemOverlayController.TypeOfCard.Decoration, this.transform.position, false);
      overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() =>
      {
        PlayerFarming.SetStateForAllPlayers(PlayerNotToInclude: this.playerFarming);
        if (PlayerFarming.Location != FollowerLocation.DecorationShop_Inside)
          return;
        this.Interactable = true;
        this.shopKeeperManager.InitShopDecoration(this.shopKeeperManager.GetShopSlotIndex(this));
        this.gameObject.SetActive(true);
        BiomeConstants.Instance.EmitSmokeExplosionVFX(this.transform.position);
        AudioManager.Instance.PlayOneShot("event:/dlc/env/ghost/chazaq_shop_item_appear", this.transform.position);
        this.Activated = false;
      });
      this.gameObject.SetActive(false);
    }
    else if (this.itemForSale.TarotCard)
    {
      BiomeConstants.Instance.EmitSmokeExplosionVFX(this.transform.position);
      RumbleManager.Instance.Rumble();
      TarotCustomTarget.Create(this.transform.position, this.playerFarming.transform.position, 0.0f, this.itemForSale.Card, (System.Action) null);
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }
    else
    {
      PickUp pickUp = (PickUp) null;
      if (this.itemForSale.quantity > 1)
      {
        for (int index = 0; index < this.clones.Count; ++index)
        {
          Debug.Log((object) ("Spawn: " + this.itemForSale.itemToBuy.ToString()));
          InventoryItem.Spawn(this.itemForSale.itemToBuy, 1, this.transform.position);
          UnityEngine.Object.Destroy((UnityEngine.Object) this.clones[index]);
        }
      }
      else
      {
        pickUp = InventoryItem.Spawn(this.itemForSale.itemToBuy, 1, this.transform.position);
        pickUp.transform.parent = this.transform.parent;
      }
      this.itemForSale.Bought = true;
      if ((UnityEngine.Object) this.shopKeeperManager != (UnityEngine.Object) null)
      {
        foreach (BuyEntry buyEntry in this.shopKeeperManager.shop.itemsForSale)
        {
          if (buyEntry.itemToBuy == this.itemForSale.itemToBuy && !InventoryItem.AllAnimals.Contains(buyEntry.itemToBuy))
            buyEntry.Bought = true;
        }
        if ((UnityEngine.Object) pickUp != (UnityEngine.Object) null)
        {
          pickUp.transform.position = this.playerFarming.transform.position;
          pickUp.MagnetToPlayer = true;
          if ((bool) (UnityEngine.Object) pickUp.GetComponent<Interaction>())
            pickUp.GetComponent<Interaction>().AutomaticallyInteract = true;
          else
            PlayerFarming.SetStateForAllPlayers();
        }
        DataManager.Instance.UpdateShop(this.shopKeeperManager.shop);
      }
      PlayerFarming.SetStateForAllPlayers(PlayerNotToInclude: this.playerFarming);
      this.gameObject.SetActive(false);
    }
  }

  [CompilerGenerated]
  public void \u003CActivate\u003Eb__47_0()
  {
    PlayerFarming.SetStateForAllPlayers(PlayerNotToInclude: this.playerFarming);
    if (PlayerFarming.Location != FollowerLocation.DecorationShop_Inside)
      return;
    this.Interactable = true;
    this.shopKeeperManager.InitShopDecoration(this.shopKeeperManager.GetShopSlotIndex(this));
    this.gameObject.SetActive(true);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(this.transform.position);
    AudioManager.Instance.PlayOneShot("event:/dlc/env/ghost/chazaq_shop_item_appear", this.transform.position);
    this.Activated = false;
  }
}
