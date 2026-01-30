// Decompiled with JetBrains decompiler
// Type: Lamb.UI.RanchAssignInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Spine.Unity;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class RanchAssignInfoCard : UIInfoCardBase<RanchAssignMenuItem>
{
  [Header("General")]
  [SerializeField]
  public CanvasGroup canvasGroup;
  [SerializeField]
  public RectTransform redOutline;
  [Header("Animal")]
  [SerializeField]
  public TextMeshProUGUI animalName;
  [SerializeField]
  public SkeletonGraphic animalSpine;
  [SerializeField]
  public TextMeshProUGUI loreTextMeshProUGUI;
  [SerializeField]
  public TextMeshProUGUI harvestItemIcon;
  [SerializeField]
  public TextMeshProUGUI harvestItemAmountText;
  [SerializeField]
  public TextMeshProUGUI sacrificeItemIcon;
  [SerializeField]
  public TextMeshProUGUI sacrificeItemAmountText;
  [SerializeField]
  public RectTransform lockedObject;
  [Header("Notifcations")]
  [SerializeField]
  public RectTransform collectBeforeAssign;
  public RanchAssignMenuItem assignMenuConfig;

  public SkeletonGraphic FollowerSpine => this.animalSpine;

  public RectTransform RedOutline => this.redOutline;

  public override void Configure(RanchAssignMenuItem config)
  {
    StructuresData.Ranchable_Animal dummyAnimal = config.GetDummyAnimal();
    this.animalSpine.ConfigureAnimal(dummyAnimal);
    if ((Object) this.assignMenuConfig == (Object) config)
      return;
    this.assignMenuConfig = config;
    this.animalName.text = InventoryItem.LocalizedName(dummyAnimal.Type);
    this.loreTextMeshProUGUI.text = InventoryItem.LocalizedLore(dummyAnimal.Type);
    List<InventoryItem> workLoot = Interaction_Ranchable.GetWorkLoot(dummyAnimal);
    string iconByType1 = FontImageNames.GetIconByType((InventoryItem.ITEM_TYPE) workLoot[0].type);
    List<InventoryItem> meatLoot = Interaction_Ranchable.GetMeatLoot(dummyAnimal);
    string iconByType2 = FontImageNames.GetIconByType((InventoryItem.ITEM_TYPE) meatLoot[0].type);
    this.harvestItemIcon.text = iconByType1;
    this.harvestItemAmountText.text = "x" + (workLoot.Count > 1 ? workLoot.Count : workLoot[0].quantity).ToString();
    this.sacrificeItemIcon.text = iconByType2;
    this.sacrificeItemAmountText.text = "x" + (meatLoot.Count > 1 ? meatLoot.Count : meatLoot[0].quantity).ToString();
    int type = Interaction_Ranchable.GetWorkLoot(dummyAnimal)[0].type;
    string str = $"{LocalizationManager.GetTranslation("UI/RanchAssignMenu/Prodcues")} {FontImageNames.GetIconByType((InventoryItem.ITEM_TYPE) type)}";
    this.lockedObject.gameObject.SetActive(!AnimalData.HasDiscoveredAnimal(config.Type));
    if (Inventory.GetItemQuantity(config.Type) <= 0)
      this.collectBeforeAssign.gameObject.SetActive(true);
    else
      this.collectBeforeAssign.gameObject.SetActive(false);
  }
}
