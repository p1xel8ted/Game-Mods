// Decompiled with JetBrains decompiler
// Type: Lamb.UI.necklaceFormItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
namespace Lamb.UI;

public class necklaceFormItem : 
  IndoctrinationCharacterItem<necklaceFormItem>,
  ISelectHandler,
  IEventSystemHandler
{
  [SerializeField]
  public InventoryItemDisplay _itemDisplay;
  [SerializeField]
  public TextMeshProUGUI _itemCount;
  [SerializeField]
  public InventoryItem.ITEM_TYPE _necklace;
  public int _count;

  public InventoryItem.ITEM_TYPE Necklace => this._necklace;

  public virtual void Configure(
    WorshipperData.SkinAndData skinAndData,
    InventoryItem.ITEM_TYPE necklaceType,
    bool isEquipped)
  {
    this._count = Inventory.GetItemQuantity(necklaceType);
    if (isEquipped)
      ++this._count;
    if ((bool) (UnityEngine.Object) this._itemCount)
    {
      this._itemCount.text = this._count.ToString();
      if (this._count == 0)
      {
        this._itemCount.color = StaticColors.RedColor;
        this._itemDisplay.image.color = Color.grey;
      }
      else
        this._itemCount.color = StaticColors.GreenColor;
      this._itemDisplay.SetImage(necklaceType);
    }
    this._locked = !DataManager.Instance.FoundItems.Contains(necklaceType) && this._count <= 0;
    this.Button.Confirmable = !this._locked;
    if (this._locked)
    {
      this._itemCount.text = "";
      this._itemDisplay.image.color = Color.black;
    }
    this.UpdateState();
    this._necklace = necklaceType;
  }

  public InventoryItem.ITEM_TYPE ReturnNecklace() => this._necklace;

  public override void OnButtonClickedImpl()
  {
    if (this._count <= 0 && this._necklace != InventoryItem.ITEM_TYPE.NONE)
      return;
    Action<necklaceFormItem> onItemSelected = this.OnItemSelected;
    if (onItemSelected == null)
      return;
    onItemSelected(this);
  }

  public void OnSelect(BaseEventData eventData)
  {
  }

  public override void OnButtonHighlightedImpl()
  {
    Action<necklaceFormItem> onItemHighlighted = this.OnItemHighlighted;
    if (onItemHighlighted == null)
      return;
    onItemHighlighted(this);
  }
}
