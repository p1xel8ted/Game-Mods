// Decompiled with JetBrains decompiler
// Type: SingleChoiceRewardOption
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class SingleChoiceRewardOption : BaseMonoBehaviour
{
  public UnityEvent Callback;
  public bool StartHidden;
  public TextMeshPro QuantityText;
  [SerializeField]
  private Vector2 randomOffset = new Vector2(-2f, 2f);
  [SerializeField]
  private InventoryItemDisplay itemDisplay;
  [SerializeField]
  private Interaction_PickUpLoot interaction;
  [Space]
  [SerializeField]
  private SingleChoiceRewardOption[] otherOptions;
  [SerializeField]
  public List<BuyEntry> itemOptions;
  public bool AllowDecorationAndSkin;
  public bool AllowDuplicateFood;

  public BuyEntry Option { get; private set; }

  private void Start()
  {
    this.SetOption();
    if (!this.StartHidden)
      return;
    this.Hide();
  }

  public void Reveal()
  {
    this.gameObject.SetActive(true);
    this.transform.localScale = Vector3.zero;
    this.transform.DOScale(Vector3.one * 1.5f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
  }

  private void Hide() => this.gameObject.SetActive(false);

  public virtual List<BuyEntry> GetOptions() => this.itemOptions;

  private void SetOption()
  {
    List<BuyEntry> options = this.GetOptions();
    for (int index = options.Count - 1; index >= 0; --index)
    {
      if (options[index].itemToBuy == InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION)
      {
        if (!DataManager.GetDecorationsAvailableCategory(PlayerFarming.Location))
          options.Remove(options[index]);
      }
      else if (options[index].itemToBuy == InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN)
      {
        if (!DataManager.CheckIfThereAreSkinsAvailable())
          options.Remove(options[index]);
      }
      else if (options[index].itemToBuy == InventoryItem.ITEM_TYPE.DOCTRINE_STONE)
      {
        if (!DoctrineUpgradeSystem.TrySermonsStillAvailable() || !DoctrineUpgradeSystem.TryGetStillDoctrineStone())
          options.Remove(options[index]);
      }
      else if (options[index].itemToBuy == InventoryItem.ITEM_TYPE.TRINKET_CARD && DataManager.Instance.PlayerFleece == 4)
        options.Remove(options[index]);
      else if (!DataManager.Instance.ShowLoyaltyBars && (options[index].itemToBuy == InventoryItem.ITEM_TYPE.GIFT_SMALL || options[index].itemToBuy == InventoryItem.ITEM_TYPE.GIFT_MEDIUM))
        options.Remove(options[index]);
    }
    foreach (SingleChoiceRewardOption otherOption in this.otherOptions)
    {
      if (otherOption.Option != null)
      {
        for (int index = options.Count - 1; index >= 0; --index)
        {
          if (options[index].itemToBuy == otherOption.Option.itemToBuy)
            options.RemoveAt(index);
          else if (!this.AllowDuplicateFood && InventoryItem.IsFood(options[index].itemToBuy) && InventoryItem.IsFood(otherOption.Option.itemToBuy))
            options.RemoveAt(index);
          else if (InventoryItem.IsFish(options[index].itemToBuy) && InventoryItem.IsFish(otherOption.Option.itemToBuy))
            options.RemoveAt(index);
          else if (InventoryItem.IsGiftOrNecklace(options[index].itemToBuy) && InventoryItem.IsGiftOrNecklace(otherOption.Option.itemToBuy))
            options.RemoveAt(index);
          else if (options[index].GroupID != -1 && options[index].GroupID == otherOption.Option.GroupID)
            options.RemoveAt(index);
          else if (!this.AllowDecorationAndSkin && (options[index].itemToBuy == InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN && otherOption.Option.itemToBuy == InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION || options[index].itemToBuy == InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION && otherOption.Option.itemToBuy == InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN))
            options.RemoveAt(index);
        }
      }
    }
    if (options.Count <= 0)
    {
      this.gameObject.SetActive(false);
    }
    else
    {
      this.Option = options[Random.Range(0, options.Count)];
      if (this.StartHidden && this.Option.itemToBuy == InventoryItem.ITEM_TYPE.BERRY)
      {
        switch (PlayerFarming.Location)
        {
          case FollowerLocation.Dungeon1_1:
            this.Option.itemToBuy = InventoryItem.ITEM_TYPE.BERRY;
            break;
          case FollowerLocation.Dungeon1_2:
            this.Option.itemToBuy = InventoryItem.ITEM_TYPE.PUMPKIN;
            break;
          case FollowerLocation.Dungeon1_3:
            this.Option.itemToBuy = InventoryItem.ITEM_TYPE.CAULIFLOWER;
            break;
          case FollowerLocation.Dungeon1_4:
            this.Option.itemToBuy = InventoryItem.ITEM_TYPE.BEETROOT;
            break;
        }
      }
      if (this.StartHidden && this.Option.itemToBuy == InventoryItem.ITEM_TYPE.SEED)
      {
        switch (PlayerFarming.Location)
        {
          case FollowerLocation.Dungeon1_1:
            this.Option.itemToBuy = InventoryItem.ITEM_TYPE.SEED;
            break;
          case FollowerLocation.Dungeon1_2:
            this.Option.itemToBuy = InventoryItem.ITEM_TYPE.SEED_PUMPKIN;
            break;
          case FollowerLocation.Dungeon1_3:
            this.Option.itemToBuy = InventoryItem.ITEM_TYPE.SEED_CAULIFLOWER;
            break;
          case FollowerLocation.Dungeon1_4:
            this.Option.itemToBuy = InventoryItem.ITEM_TYPE.SEED_BEETROOT;
            break;
        }
      }
      if (!this.Option.SingleQuantityItem)
        this.Option.quantity = Mathf.Clamp(this.Option.quantity + Random.Range((int) this.randomOffset.x, (int) this.randomOffset.y), 1, int.MaxValue);
      this.itemDisplay.SetImage(this.Option.itemToBuy);
      if (this.Option.quantity > 1)
        this.QuantityText.text = "x" + (object) this.Option.quantity;
      else
        this.QuantityText.text = "";
      this.interaction.Init(this.Option.itemToBuy, this.Option.quantity);
      this.interaction.OnInteraction += new Interaction.InteractionEvent(this.OnInteraction);
      Debug.Log((object) ("Option.itemToBuy: " + (object) this.Option.itemToBuy).Colour(Color.green));
    }
  }

  private void OnInteraction(StateMachine state)
  {
    this.ChoiceDisabled();
    foreach (SingleChoiceRewardOption otherOption in this.otherOptions)
      otherOption.ChoiceDisabled();
    this.Callback?.Invoke();
  }

  public void ChoiceDisabled()
  {
    this.itemDisplay.gameObject.SetActive(false);
    this.QuantityText.gameObject.SetActive(false);
  }
}
