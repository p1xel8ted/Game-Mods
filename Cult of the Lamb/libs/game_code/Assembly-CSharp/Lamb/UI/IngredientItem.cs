// Decompiled with JetBrains decompiler
// Type: Lamb.UI.IngredientItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.Alerts;
using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
namespace Lamb.UI;

public class IngredientItem : UIInventoryItem, ISelectHandler, IEventSystemHandler
{
  [SerializeField]
  public InventoryAlert _alert;
  [SerializeField]
  public GameObject[] _starFills;
  [SerializeField]
  public GameObject _removeIcon;

  public void Configure(InventoryItem.ITEM_TYPE type, bool queued, bool showQuantity = true)
  {
    this.Configure(type, showQuantity);
    if ((Object) this._alert != (Object) null)
      this._alert.Configure(type);
    int starRating = this.GetStarRating();
    for (int index = 0; index < this._starFills.Length; ++index)
      this._starFills[index].SetActive(starRating >= index + 1);
    this._removeIcon.SetActive(queued);
  }

  public void OnSelect(BaseEventData eventData)
  {
    if (!((Object) this._alert != (Object) null))
      return;
    this._alert.TryRemoveAlert();
  }

  public int GetStarRating()
  {
    switch (CookingData.GetIngredientType(this.Type))
    {
      case CookingData.IngredientType.VEGETABLE_MEDIUM:
      case CookingData.IngredientType.MEAT_MEDIUM:
      case CookingData.IngredientType.FISH_MEDIUM:
      case CookingData.IngredientType.SPECIAL_FOLLOWER_MEAT:
        return 2;
      case CookingData.IngredientType.VEGETABLE_HIGH:
      case CookingData.IngredientType.MEAT_HIGH:
      case CookingData.IngredientType.FISH_HIGH:
        return 3;
      default:
        return 1;
    }
  }
}
