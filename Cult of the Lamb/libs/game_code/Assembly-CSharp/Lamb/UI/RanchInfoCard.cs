// Decompiled with JetBrains decompiler
// Type: Lamb.UI.RanchInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Spine.Unity;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class RanchInfoCard : UIInfoCardBase<RanchMenuItem>
{
  [Header("General")]
  [SerializeField]
  public RectTransform redOutline;
  [SerializeField]
  public CanvasGroup canvasGroup;
  [Header("Animal")]
  [SerializeField]
  public SkeletonGraphic animalSpine;
  [SerializeField]
  public TextMeshProUGUI followerNameText;
  [SerializeField]
  public TextMeshProUGUI loreText;
  [SerializeField]
  public TextMeshProUGUI ageText;
  [SerializeField]
  public TextMeshProUGUI currentTotalText;
  [SerializeField]
  public TextMeshProUGUI speedText;
  [SerializeField]
  public TextMeshProUGUI harvestItem;
  [SerializeField]
  public TextMeshProUGUI sacrificeItem;
  [SerializeField]
  public TextMeshProUGUI favFood;
  [Header("Buffs")]
  [SerializeField]
  public GameObject overcrowdedBuff;
  [SerializeField]
  public GameObject stinkyBuff;
  [SerializeField]
  public GameObject feralBuff;
  [SerializeField]
  public GameObject ageYoungBuff;
  [SerializeField]
  public GameObject ageOldBuff;
  [SerializeField]
  public GameObject wellfedBuff;
  [SerializeField]
  public GameObject bestFriendBuff;
  [Header("Level Bonus")]
  [SerializeField]
  public GameObject levelBonusObject;
  [SerializeField]
  public TextMeshProUGUI levelBonusText;
  [SerializeField]
  public TextMeshProUGUI levelBonusAmountText;
  [Header("Notifications")]
  [SerializeField]
  public GameObject notificationBox;
  [SerializeField]
  public GameObject harvestRitualNotifcation;
  [SerializeField]
  public TextMeshProUGUI harvestRitualText;
  [SerializeField]
  public GameObject meatRitualNotifcation;
  [SerializeField]
  public TextMeshProUGUI meatRitualText;
  [SerializeField]
  public GameObject overcrowdedNotification;
  [Header("Misc")]
  [SerializeField]
  public LayoutElement iconLayoutElement;
  public RanchMenuItem menuItemConfig;

  public SkeletonGraphic AnimalSpine => this.animalSpine;

  public RectTransform RedOutline => this.redOutline;

  public override void Configure(RanchMenuItem config)
  {
    if ((Object) this.menuItemConfig == (Object) config)
      return;
    this.menuItemConfig = config;
    string translation;
    if (config.AnimalInfo.Age >= 15)
    {
      Debug.Log((object) "It's a old animal");
      translation = LocalizationManager.GetTranslation("UI/RanchAssignMenu/Old");
    }
    else
      translation = config.AnimalInfo.Age >= 2 ? "" : LocalizationManager.GetTranslation("UI/RanchAssignMenu/Baby");
    if (!string.IsNullOrEmpty(translation))
    {
      if (config.AnimalInfo.GivenName != null)
        this.followerNameText.text = string.Format(translation, (object) config.AnimalInfo.GivenName);
      else
        this.followerNameText.text = string.Format(translation, (object) InventoryItem.LocalizedName(config.AnimalInfo.Type));
    }
    else if (config.AnimalInfo.GivenName != null)
      this.followerNameText.text = config.AnimalInfo.GivenName;
    else
      this.followerNameText.text = InventoryItem.LocalizedName(config.AnimalInfo.Type);
    string str1 = string.Join(" - ", this.followerNameText.text, $"{ScriptLocalization.Interactions.Level} {config.AnimalInfo.Level.ToNumeral()}");
    if (config.AnimalInfo.BestFriend)
      this.followerNameText.text = "\uF004 " + str1;
    else
      this.followerNameText.text = str1;
    this.animalSpine.ConfigureAnimal(config.AnimalInfo);
    this.loreText.text = InventoryItem.LocalizedLore(config.AnimalInfo.Type);
    this.ageText.text = string.Format(LocalizationManager.GetTranslation("UI/RanchMenu/AnimalData/Age"), (object) config.AnimalInfo.Age.ToString());
    this.speedText.text = string.Format(LocalizationManager.GetTranslation("UI/RanchMenu/AnimalData/Speed"), (object) config.AnimalInfo.Speed.ToString("0.00"));
    List<InventoryItem> workLoot1 = Interaction_Ranchable.GetWorkLoot(config.AnimalInfo);
    string str2 = CostFormatter.FormatCost((InventoryItem.ITEM_TYPE) workLoot1[0].type, workLoot1[0].quantity, ignoreAffordability: true);
    List<InventoryItem> meatLoot1 = Interaction_Ranchable.GetMeatLoot(config.AnimalInfo);
    this.currentTotalText.text = CostFormatter.FormatCost((InventoryItem.ITEM_TYPE) meatLoot1[0].type, meatLoot1[0].quantity, ignoreAffordability: true) + str2;
    bool flag = false;
    List<InventoryItem> workLoot2 = Interaction_Ranchable.GetWorkLoot(config.AnimalInfo);
    List<InventoryItem> meatLoot2 = Interaction_Ranchable.GetMeatLoot(config.AnimalInfo);
    if (config.AnimalInfo.Level > 1)
    {
      this.levelBonusObject.gameObject.SetActive(true);
      string str3 = config.AnimalInfo.GetLootCountLevelModifier().ToString();
      string str4 = config.AnimalInfo.GetMeatCountLevelModifier().ToString();
      string str5 = string.Format(LocalizationManager.GetTranslation("UI/RanchMenu/LevelBonus"), (object) config.AnimalInfo.Level.ToNumeral());
      this.levelBonusText.text = $"{StaticColors.GreenColorHex}<sprite name=\"icon_GoodTrait\"> {str5}";
      this.levelBonusAmountText.text = $"{StaticColors.GreenColorHex}{FontImageNames.GetIconByType((InventoryItem.ITEM_TYPE) meatLoot2[0].type)}+{str4}{FontImageNames.GetIconByType((InventoryItem.ITEM_TYPE) workLoot2[0].type)}+{str3} ";
    }
    else
      this.levelBonusObject.gameObject.SetActive(false);
    if (config.AnimalInfo.IsFavouriteFoodRevealed)
    {
      this.favFood.gameObject.SetActive(true);
      this.favFood.text = "\uF004 " + FontImageNames.GetIconByType(config.AnimalInfo.FavouriteFood);
    }
    else
      this.favFood.gameObject.SetActive(false);
    if (config.AnimalInfo.State == Interaction_Ranchable.State.Overcrowded)
      this.overcrowdedBuff.gameObject.SetActive(true);
    else
      this.overcrowdedBuff.gameObject.SetActive(false);
    if (config.AnimalInfo.Ailment == Interaction_Ranchable.Ailment.Stinky)
      this.stinkyBuff.gameObject.SetActive(true);
    else
      this.stinkyBuff.gameObject.SetActive(false);
    if (config.AnimalInfo.Ailment == Interaction_Ranchable.Ailment.Feral)
      this.feralBuff.gameObject.SetActive(true);
    else
      this.feralBuff.gameObject.SetActive(false);
    if (config.AnimalInfo.Age < 2)
      this.ageYoungBuff.gameObject.SetActive(true);
    else
      this.ageYoungBuff.gameObject.SetActive(false);
    if (config.AnimalInfo.Age >= 15)
      this.ageOldBuff.gameObject.SetActive(true);
    else
      this.ageOldBuff.gameObject.SetActive(false);
    if (config.AnimalInfo.GrowthStage >= 6)
      this.wellfedBuff.gameObject.SetActive(true);
    else
      this.wellfedBuff.gameObject.SetActive(false);
    if (config.AnimalInfo.BestFriend)
      this.bestFriendBuff.gameObject.SetActive(true);
    else
      this.bestFriendBuff.gameObject.SetActive(false);
    if (FollowerBrainStats.IsRanchHarvest)
    {
      this.harvestRitualNotifcation.SetActive(true);
      this.levelBonusText.gameObject.SetActive(true);
      this.harvestRitualText.text = $"{StaticColors.GreenColorHex}<sprite name=\"icon_GoodTrait\">{LocalizationManager.GetTranslation("UI/RanchMenu/RanchRitualHarvestActive")}+{(workLoot2[0].quantity / 2).ToString()}";
      flag = true;
    }
    else
      this.harvestRitualNotifcation.SetActive(false);
    if (FollowerBrainStats.IsRanchMeat)
    {
      this.meatRitualNotifcation.SetActive(true);
      this.levelBonusText.gameObject.SetActive(true);
      this.meatRitualText.text = $"{StaticColors.GreenColorHex}<sprite name=\"icon_GoodTrait\">{LocalizationManager.GetTranslation("UI/RanchMenu/RanchRitualMeatActive")}+{(meatLoot2[0].quantity / 2).ToString()}";
      flag = true;
    }
    else
      this.meatRitualNotifcation.SetActive(false);
    if (config.AnimalInfo.State == Interaction_Ranchable.State.Overcrowded)
    {
      this.overcrowdedNotification.SetActive(true);
      this.levelBonusText.gameObject.SetActive(true);
      flag = true;
    }
    else
      this.overcrowdedNotification.SetActive(false);
    this.notificationBox.gameObject.SetActive(flag);
    if (config.AnimalInfo.Age > 2)
    {
      switch (config.AnimalInfo.Type)
      {
        case InventoryItem.ITEM_TYPE.ANIMAL_GOAT:
        case InventoryItem.ITEM_TYPE.ANIMAL_TURTLE:
        case InventoryItem.ITEM_TYPE.ANIMAL_CRAB:
        case InventoryItem.ITEM_TYPE.ANIMAL_SPIDER:
        case InventoryItem.ITEM_TYPE.ANIMAL_COW:
          this.iconLayoutElement.preferredHeight = 160f;
          break;
        case InventoryItem.ITEM_TYPE.ANIMAL_SNAIL:
          this.iconLayoutElement.preferredHeight = 170f;
          break;
        case InventoryItem.ITEM_TYPE.ANIMAL_LLAMA:
          this.iconLayoutElement.preferredHeight = 270f;
          break;
      }
    }
    else
      this.iconLayoutElement.preferredHeight = 160f;
  }
}
