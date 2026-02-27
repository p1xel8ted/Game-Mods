// Decompiled with JetBrains decompiler
// Type: Interaction_BuyItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_BuyItem : Interaction
{
  private bool Activated;
  private int devotionCost = 10;
  public BuyEntry itemForSale;
  public bool customItemForSale;
  public UnityEvent CallBack;
  [Range(0.0f, 100f)]
  public int chanceOfSale = 7;
  public bool SaleIsOn;
  public int SaleAmount;
  public float saleAmountFloat;
  private string BuyString;
  private string off;
  public bool UsingDecorations;
  private float AmountToDiscount;
  public shopKeeperManager shopKeeperManager;
  public bool DoublePrice;
  public System.Action OnItemBought;
  private bool createdDiscount;
  private int randomChanceOfSale;
  public GameObject DecorationItem;
  private string DecorationName = string.Empty;
  private string SaleText = string.Empty;
  private Vector3 BookTargetPosition;
  private float Timer;
  public GameObject PlayerPosition;
  public List<GameObject> clones;
  public GameObject spawnObject;

  private void Start()
  {
    this.shopKeeperManager = this.GetComponentInParent<shopKeeperManager>();
    this.UpdateLocalisation();
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
      if (!this.createdDiscount)
      {
        Debug.Log((object) $"Random chance of sale = {(object) this.randomChanceOfSale}Chance: {(object) this.chanceOfSale}");
        this.devotionCost = this.itemForSale.itemCost;
        this.SaleIsOn = true;
        this.SaleAmount = UnityEngine.Random.Range(1, 5);
        this.saleAmountFloat = (float) this.SaleAmount;
        this.saleAmountFloat *= 0.1f;
        Debug.Log((object) $"Amount to discount = {(object) this.devotionCost} - {(object) this.saleAmountFloat}");
        this.AmountToDiscount = Mathf.Round((float) this.devotionCost - (float) this.devotionCost * this.saleAmountFloat);
        if ((double) this.AmountToDiscount < 1.0)
          this.AmountToDiscount = 1f;
        this.createdDiscount = true;
      }
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
      this.boughtItem();
      this.Interactable = false;
      HUD_Manager.Instance.Hide(false);
      for (int index = 0; index < this.GetCost(); ++index)
      {
        if (index < 10)
        {
          Inventory.GetItemByType((int) this.itemForSale.costType);
          AudioManager.Instance.PlayOneShot("event:/followers/pop_in", PlayerFarming.Instance.transform.position);
          ResourceCustomTarget.Create(this.gameObject, PlayerFarming.Instance.gameObject.transform.position, this.itemForSale.costType, (System.Action) null);
        }
        Inventory.ChangeItemQuantity((int) this.itemForSale.costType, -1);
      }
      if (this.itemForSale.TarotCard || this.itemForSale.Decoration)
        PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      this.StartCoroutine((IEnumerator) this.DelayOnActivate());
      CameraManager.shakeCamera(0.3f, (float) UnityEngine.Random.Range(0, 360));
    }
    else
      this.cantAfford();
  }

  public void AlreadyBought() => this.gameObject.SetActive(false);

  private void cantAfford()
  {
    AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback", this.gameObject);
    MonoSingleton<Indicator>.Instance.PlayShake();
    if (!((UnityEngine.Object) this.shopKeeperManager.CantAffordBark != (UnityEngine.Object) null))
      return;
    if ((UnityEngine.Object) this.shopKeeperManager.NormalBark != (UnityEngine.Object) null)
      this.shopKeeperManager.NormalBark.SetActive(false);
    this.shopKeeperManager.CantAffordBark.SetActive(true);
  }

  private void boughtItem()
  {
    if (!((UnityEngine.Object) this.shopKeeperManager.BoughtItemBark != (UnityEngine.Object) null))
      return;
    if ((UnityEngine.Object) this.shopKeeperManager.NormalBark != (UnityEngine.Object) null)
      this.shopKeeperManager.NormalBark.SetActive(false);
    this.shopKeeperManager.BoughtItemBark.SetActive(true);
  }

  private IEnumerator DelayOnActivate()
  {
    yield return (object) new WaitForSeconds(0.25f);
    this.Activate();
  }

  private IEnumerator BoughtCustomItem()
  {
    yield return (object) new WaitForSeconds(0.2f);
    this.CallBack?.Invoke();
  }

  protected override void Update()
  {
    base.Update();
    int num = (UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null ? 1 : 0;
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

  private void Activate()
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
      UINewItemOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowNewItemOverlay();
      overlayController.pickedBuilding = this.itemForSale.decorationToBuy;
      overlayController.Show(UINewItemOverlayController.TypeOfCard.Decoration, this.transform.position, false);
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }
    else if (this.itemForSale.TarotCard)
    {
      BiomeConstants.Instance.EmitSmokeExplosionVFX(this.transform.position);
      RumbleManager.Instance.Rumble();
      TarotCustomTarget.Create(this.transform.position, PlayerFarming.Instance.transform.position, 0.0f, this.itemForSale.Card, (System.Action) null);
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }
    else
    {
      if (this.itemForSale.quantity > 1)
      {
        for (int index = 0; index < this.clones.Count; ++index)
        {
          Debug.Log((object) ("Spawn: " + (object) this.itemForSale.itemToBuy));
          InventoryItem.Spawn(this.itemForSale.itemToBuy, 1, this.transform.position);
          UnityEngine.Object.Destroy((UnityEngine.Object) this.clones[index]);
        }
      }
      else
        InventoryItem.Spawn(this.itemForSale.itemToBuy, 1, this.transform.position);
      this.itemForSale.Bought = true;
      if ((UnityEngine.Object) this.shopKeeperManager != (UnityEngine.Object) null)
      {
        foreach (BuyEntry buyEntry in this.shopKeeperManager.shop.itemsForSale)
        {
          if (buyEntry.itemToBuy == this.itemForSale.itemToBuy)
            buyEntry.Bought = true;
        }
        DataManager.Instance.UpdateShop(this.shopKeeperManager.shop);
      }
      this.gameObject.SetActive(false);
    }
  }
}
