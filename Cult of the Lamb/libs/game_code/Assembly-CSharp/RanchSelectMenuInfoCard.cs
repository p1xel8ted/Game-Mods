// Decompiled with JetBrains decompiler
// Type: RanchSelectMenuInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class RanchSelectMenuInfoCard : UIInfoCardBase<RanchSelectItem>
{
  [Header("Prison Info Card")]
  [SerializeField]
  public TextMeshProUGUI _followerNameText;
  [SerializeField]
  public SkeletonGraphic _followerSpine;
  [SerializeField]
  public RectTransform _redOutline;
  [SerializeField]
  public TextMeshProUGUI _descriptionTextMeshProUGUI;
  [SerializeField]
  public CanvasGroup _canvasGroup;
  [Header("Misc")]
  [SerializeField]
  public LayoutElement iconLayoutElement;
  public RanchSelectItem _followerInfo;

  public SkeletonGraphic FollowerSpine => this._followerSpine;

  public RectTransform RedOutline => this._redOutline;

  public override void Configure(RanchSelectItem config)
  {
    if ((Object) this._followerInfo == (Object) config)
      return;
    this._followerInfo = config;
    if (!string.IsNullOrEmpty(config.AnimalInfo.GivenName))
      this._followerNameText.text = config.AnimalInfo.GivenName;
    else
      this._followerNameText.text = InventoryItem.LocalizedName(config.AnimalInfo.Type);
    this._followerSpine.ConfigureAnimal(config.AnimalInfo);
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
