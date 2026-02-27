// Decompiled with JetBrains decompiler
// Type: MMTools.UIInventory.UIInventoryItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
namespace MMTools.UIInventory;

public class UIInventoryItem : BaseMonoBehaviour
{
  public InventoryItemDisplay inventoryItemDisplay;
  public TextMeshProUGUI Name;
  public TextMeshProUGUI Lore;
  public TextMeshProUGUI Description;
  public InventoryItem Item;
  private RectTransform _rectTransform;

  public RectTransform rectTransform
  {
    get
    {
      if ((Object) this._rectTransform == (Object) null)
        this._rectTransform = this.GetComponent<RectTransform>();
      return this._rectTransform;
    }
  }

  public void Init(InventoryItem item)
  {
    if (item == null)
      this.InitEmpty();
    else if (item.type == 0)
    {
      this.InitEmpty();
    }
    else
    {
      this.Item = item;
      this.inventoryItemDisplay.SetImage((InventoryItem.ITEM_TYPE) item.type);
      if ((Object) this.Name != (Object) null)
        this.Name.text = InventoryItem.Name((InventoryItem.ITEM_TYPE) item.type);
      if ((Object) this.Lore != (Object) null)
        this.Lore.text = InventoryItem.Lore((InventoryItem.ITEM_TYPE) item.type);
      if (!((Object) this.Description != (Object) null))
        return;
      this.Description.text = InventoryItem.Description((InventoryItem.ITEM_TYPE) item.type);
    }
  }

  public void InitEmpty()
  {
    this.inventoryItemDisplay.SetImage(InventoryItem.ITEM_TYPE.NONE);
    if ((Object) this.Name != (Object) null)
      this.Name.text = "";
    if ((Object) this.Lore != (Object) null)
      this.Lore.text = "";
    if (!((Object) this.Description != (Object) null))
      return;
    this.Description.text = "";
  }
}
