// Decompiled with JetBrains decompiler
// Type: Lamb.UI.IngredientItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI.Alerts;
using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
namespace Lamb.UI;

public class IngredientItem : UIInventoryItem, ISelectHandler, IEventSystemHandler
{
  [SerializeField]
  protected InventoryAlert _alert;
  [SerializeField]
  private GameObject[] _starFills;
  [SerializeField]
  private GameObject _removeIcon;

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

  private int GetStarRating()
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
