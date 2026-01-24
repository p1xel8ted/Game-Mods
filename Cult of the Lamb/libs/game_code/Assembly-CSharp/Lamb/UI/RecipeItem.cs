// Decompiled with JetBrains decompiler
// Type: Lamb.UI.RecipeItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class RecipeItem : UIInventoryItem, ISelectHandler, IEventSystemHandler
{
  public Action<RecipeItem> OnRecipeChosen;
  [Header("Progress")]
  [SerializeField]
  public Image _alert;
  [SerializeField]
  public GameObject _hungerContainer;
  [SerializeField]
  public Image _hungerFill;
  [SerializeField]
  public GameObject _starContainer;
  [SerializeField]
  public GameObject[] _starFills;
  [SerializeField]
  public GameObject _removeIcon;
  public bool _isQueued;
  public bool _discovered;
  public bool _canAfford;

  public void Configure(InventoryItem.ITEM_TYPE type, bool showQuantity = true, bool isQueued = false)
  {
    this._discovered = CookingData.HasRecipeDiscovered(type);
    this.Configure(type, showQuantity);
    if ((UnityEngine.Object) this._button != (UnityEngine.Object) null)
    {
      this._button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
      this._button.OnConfirmDenied += new System.Action(((UIInventoryItem) this).Shake);
    }
    float tummyRating = CookingData.GetTummyRating(this.Type);
    this._hungerFill.fillAmount = tummyRating;
    if ((double) tummyRating <= 0.25)
      this._hungerFill.color = StaticColors.RedColor;
    else if ((double) tummyRating <= 0.5)
      this._hungerFill.color = StaticColors.OrangeColor;
    else
      this._hungerFill.color = StaticColors.GreenColor;
    this._hungerContainer.gameObject.SetActive(!CookingData.GetAllDrinks().Contains<InventoryItem.ITEM_TYPE>(this.Type) && this._discovered);
    if (!showQuantity)
    {
      int satationLevel = CookingData.GetSatationLevel(type);
      for (int index = 0; index < this._starFills.Length; ++index)
        this._starFills[index].SetActive(satationLevel >= index + 1);
    }
    else
      this._starContainer.SetActive(false);
    if ((UnityEngine.Object) this._alert != (UnityEngine.Object) null)
      this._alert.enabled = false;
    this._removeIcon.SetActive(isQueued);
  }

  public void OnSelect(BaseEventData eventData)
  {
  }

  public void OnButtonClicked()
  {
    Action<RecipeItem> onRecipeChosen = this.OnRecipeChosen;
    if (onRecipeChosen == null)
      return;
    onRecipeChosen(this);
  }

  public override void UpdateQuantity()
  {
    if (!this._showQuantity && !this._isQueued)
      return;
    this._canAfford = CookingData.CanMakeMeal(this.Type);
    if ((UnityEngine.Object) this._button != (UnityEngine.Object) null)
      this._button.Confirmable = this._discovered && this._canAfford;
    if (this._showQuantity && !this._isQueued)
    {
      this._quantity = CookingData.GetCookableRecipeAmount(this.Type, Inventory.items);
      this._amountText.text = this._quantity.ToString();
      this._amountText.color = this._quantity <= 0 ? StaticColors.RedColor : StaticColors.OffWhiteColor;
      if (this._quantity <= 0)
      {
        this._icon.color = this._discovered ? new Color(0.0f, 1f, 1f, 1f) : Color.black;
        if (this._discovered)
          return;
        this._hungerContainer.gameObject.SetActive(false);
        this._amountText.text = "";
      }
      else
        this._icon.color = new Color(1f, 1f, 1f, 1f);
    }
    else
    {
      if (!this._isQueued && !this._canAfford)
      {
        if (this._discovered)
          this._icon.color = new Color(0.0f, 1f, 1f, 1f);
        else
          this._icon.color = Color.black;
      }
      this._amountText.text = "";
    }
  }
}
