// Decompiled with JetBrains decompiler
// Type: SingleChoiceRewardOption
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class SingleChoiceRewardOption : BaseMonoBehaviour
{
  public UnityEvent Callback;
  public bool StartHidden;
  public TextMeshPro QuantityText;
  public Material materialPresetText;
  [SerializeField]
  public Vector2 randomOffset = new Vector2(-2f, 2f);
  [SerializeField]
  public InventoryItemDisplay itemDisplay;
  [SerializeField]
  public Interaction_PickUpLoot interaction;
  [Space]
  [SerializeField]
  public SingleChoiceRewardOption[] otherOptions;
  [SerializeField]
  public List<BuyEntry> itemOptions;
  [CompilerGenerated]
  public BuyEntry \u003COption\u003Ek__BackingField;
  public bool allowRareDLCRewards;
  public bool AllowDecorationAndSkin;
  public bool AllowDuplicateFood;

  public BuyEntry Option
  {
    get => this.\u003COption\u003Ek__BackingField;
    set => this.\u003COption\u003Ek__BackingField = value;
  }

  public void Start()
  {
    this.SetOption();
    if (!this.StartHidden)
      return;
    this.Hide();
  }

  public void Reveal()
  {
    if (this.Option == null)
      return;
    this.gameObject.SetActive(true);
    this.transform.localScale = Vector3.zero;
    this.transform.DOScale(Vector3.one * 1.5f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    if (!((Object) this.materialPresetText != (Object) null))
      return;
    this.QuantityText.fontSharedMaterial = this.materialPresetText;
  }

  public void Hide() => this.gameObject.SetActive(false);

  public virtual List<BuyEntry> GetOptions()
  {
    if ((!this.allowRareDLCRewards || (double) Random.value >= 0.20000000298023224 ? 0 : (PlayerFarming.Location == FollowerLocation.Dungeon1_5 ? 1 : (PlayerFarming.Location == FollowerLocation.Dungeon1_6 ? 1 : 0))) == 0)
      return this.itemOptions;
    List<BuyEntry> options = new List<BuyEntry>();
    if (DataManager.Instance.BlizzardOfferingsCompleted <= 4)
    {
      int offeringsCompleted = DataManager.Instance.BlizzardOfferingsCompleted;
      options.Add(new BuyEntry()
      {
        itemToBuy = InventoryItem.Necklaces_DLC[offeringsCompleted],
        quantity = 1,
        SingleQuantityItem = true
      });
    }
    else if ((double) Random.value < 0.5)
      options.Add(new BuyEntry()
      {
        itemToBuy = InventoryItem.Necklaces_DLC[Random.Range(0, InventoryItem.Necklaces_DLC.Count)],
        quantity = 1,
        SingleQuantityItem = true
      });
    if (DataManager.Instance.OnboardedWool && (double) Random.value < 0.10000000149011612)
    {
      List<InventoryItem.ITEM_TYPE> animalsWeightedList = AnimalMarketplaceManager.GetUnlockedAnimalsWeightedList();
      options.Add(new BuyEntry()
      {
        itemToBuy = animalsWeightedList[Random.Range(0, animalsWeightedList.Count)],
        quantity = 1,
        SingleQuantityItem = true
      });
    }
    if (DataManager.Instance.ShowLoyaltyBars)
    {
      InventoryItem.ITEM_TYPE[] itemTypeArray = new InventoryItem.ITEM_TYPE[2]
      {
        InventoryItem.ITEM_TYPE.GIFT_SMALL,
        InventoryItem.ITEM_TYPE.GIFT_MEDIUM
      };
      options.Add(new BuyEntry()
      {
        itemToBuy = itemTypeArray[Random.Range(0, itemTypeArray.Length)],
        quantity = 1,
        SingleQuantityItem = true
      });
    }
    if ((double) Random.value < 0.25)
    {
      if (DataManager.CheckIfThereAreSkinsAvailable())
        options.Add(new BuyEntry()
        {
          itemToBuy = InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN,
          quantity = 1,
          SingleQuantityItem = true
        });
      if (DataManager.Instance.GetDecorationListFromLocation(PlayerFarming.Location).Count > 0)
        options.Add(new BuyEntry()
        {
          itemToBuy = InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION,
          quantity = 1,
          SingleQuantityItem = true
        });
    }
    InventoryItem.ITEM_TYPE[] itemTypeArray1 = new InventoryItem.ITEM_TYPE[3]
    {
      InventoryItem.ITEM_TYPE.GOLD_REFINED,
      InventoryItem.ITEM_TYPE.LOG_REFINED,
      InventoryItem.ITEM_TYPE.STONE_REFINED
    };
    options.Add(new BuyEntry()
    {
      itemToBuy = itemTypeArray1[Random.Range(0, itemTypeArray1.Length)],
      quantity = 1,
      SingleQuantityItem = true
    });
    return options;
  }

  public void SetOption()
  {
    List<BuyEntry> options = this.GetOptions();
    for (int index = options.Count - 1; index >= 0; --index)
    {
      if (options[index].itemToBuy == InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION)
      {
        if (DataManager.Instance.GetDecorationListFromLocation(PlayerFarming.Location).Count <= 0)
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
      else if (options[index].itemToBuy == InventoryItem.ITEM_TYPE.TRINKET_CARD && PlayerFleeceManager.FleecePreventTarotCards())
        options.Remove(options[index]);
      else if (!DataManager.Instance.ShowLoyaltyBars && (options[index].itemToBuy == InventoryItem.ITEM_TYPE.GIFT_SMALL || options[index].itemToBuy == InventoryItem.ITEM_TYPE.GIFT_MEDIUM))
        options.Remove(options[index]);
      else if (InventoryItem.IsHeart(options[index].itemToBuy) && PlayerFleeceManager.FleecePreventsHealthPickups())
        options.Remove(options[index]);
      else if (options[index].itemToBuy == InventoryItem.ITEM_TYPE.YEW_CURSED && DataManager.Instance.CollectedYewMutated)
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
          case FollowerLocation.Dungeon1_6:
            this.Option.itemToBuy = InventoryItem.ITEM_TYPE.CHILLI;
            this.Option.quantity = Random.Range(6, 14);
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
          case FollowerLocation.Dungeon1_5:
          case FollowerLocation.Dungeon1_6:
            if (DataManager.Instance.OnboardedWool)
            {
              this.Option.itemToBuy = InventoryItem.ITEM_TYPE.WOOL;
              this.Option.quantity = Random.Range(2, 6);
              break;
            }
            break;
        }
        if (DataManager.Instance.PleasureEnabled && (double) Random.value < 0.20000000298023224)
          this.Option.itemToBuy = (double) Random.value >= 0.5 ? InventoryItem.ITEM_TYPE.SEED_GRAPES : InventoryItem.ITEM_TYPE.SEED_HOPS;
        if (DataManager.Instance.TailorEnabled && (double) Random.value < 0.20000000298023224)
          this.Option.itemToBuy = InventoryItem.ITEM_TYPE.SEED_COTTON;
        if (SeasonsManager.Active && (double) Random.value < 0.20000000298023224)
          this.Option.itemToBuy = InventoryItem.ITEM_TYPE.SEED_CHILLI;
      }
      if ((this.Option.itemToBuy != InventoryItem.ITEM_TYPE.YEW_CURSED || DataManager.Instance.CollectedYewMutated) && (this.Option.itemToBuy != InventoryItem.ITEM_TYPE.LIGHTNING_SHARD || DataManager.Instance.CollectedLightningShards) && (PlayerFarming.Location != FollowerLocation.Dungeon1_5 || DataManager.Instance.TotalShrineGhostJuice > 4))
      {
        if (this.StartHidden && DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.ViolentExtremist) && ((double) Random.value < 0.15000000596046448 || !DataManager.Instance.ViolentExtremistFirstTime))
        {
          bool flag = true;
          foreach (SingleChoiceRewardOption otherOption in this.otherOptions)
          {
            if ((Object) otherOption != (Object) null && otherOption.Option != null && otherOption.Option.itemToBuy == InventoryItem.ITEM_TYPE.PLEASURE_POINT)
              flag = false;
          }
          if (flag)
          {
            this.Option.quantity = 1;
            this.Option.SingleQuantityItem = true;
            this.Option.itemToBuy = InventoryItem.ITEM_TYPE.PLEASURE_POINT;
            DataManager.Instance.ViolentExtremistFirstTime = true;
          }
        }
        if (!this.Option.SingleQuantityItem)
          this.Option.quantity = Mathf.Clamp(this.Option.quantity + Random.Range((int) this.randomOffset.x, (int) this.randomOffset.y), 1, int.MaxValue);
      }
      this.itemDisplay.SetImage(this.Option.itemToBuy);
      if (this.Option.quantity > 1 && !this.Option.SingleQuantityItem)
        this.QuantityText.text = "x" + this.Option.quantity.ToString();
      else
        this.QuantityText.text = "";
      this.interaction.Init(this.Option.itemToBuy, this.Option.quantity);
      this.interaction.OnInteraction += new Interaction.InteractionEvent(this.OnInteraction);
      Debug.Log((object) ("Option.itemToBuy: " + this.Option.itemToBuy.ToString()).Colour(Color.green));
    }
  }

  public void OnInteraction(StateMachine state)
  {
    if (this.Option.itemToBuy != InventoryItem.ITEM_TYPE.PLEASURE_POINT)
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
