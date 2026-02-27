// Decompiled with JetBrains decompiler
// Type: CustomizeClothesFollowerDisplay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using TMPro;
using UnityEngine;

#nullable disable
public class CustomizeClothesFollowerDisplay : MonoBehaviour
{
  [SerializeField]
  public SkeletonGraphic _spine;
  [SerializeField]
  public TextMeshProUGUI _headerText;
  [SerializeField]
  public TextMeshProUGUI _descriptionText;

  public void Configure(ClothingData clothingData)
  {
    FollowerBrain.SetFollowerCostume(this._spine.Skeleton, 0, "Pig", 0, FollowerOutfitType.Worker, FollowerHatType.None, clothingData.ClothingType, FollowerCustomisationType.None, FollowerSpecialType.None, InventoryItem.ITEM_TYPE.NONE);
    this._headerText.text = TailorManager.LocalizedName(clothingData.ClothingType);
    this._descriptionText.text = TailorManager.LocalizedDescription(clothingData.ClothingType);
  }
}
