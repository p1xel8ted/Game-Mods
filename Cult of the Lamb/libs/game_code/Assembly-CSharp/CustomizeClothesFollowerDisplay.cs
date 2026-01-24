// Decompiled with JetBrains decompiler
// Type: CustomizeClothesFollowerDisplay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
