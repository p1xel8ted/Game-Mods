// Decompiled with JetBrains decompiler
// Type: HUD_InventoryIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class HUD_InventoryIcon : BaseMonoBehaviour
{
  public InventoryItem.ITEM_TYPE Type;
  public InventoryItemDisplay inventoryItem;
  public TextMeshProUGUI Text;
  public RectTransform _rectTransform;

  public RectTransform rectTransform
  {
    get
    {
      if ((Object) this._rectTransform == (Object) null)
        this._rectTransform = this.GetComponent<RectTransform>();
      return this._rectTransform;
    }
    set => this._rectTransform = value;
  }

  public void InitFromType(InventoryItem.ITEM_TYPE Type)
  {
    this.Type = Type;
    this.InitFromType();
  }

  public void InitFromType()
  {
    InventoryItem inventoryItem = Inventory.GetItemByType((int) this.Type);
    if (inventoryItem == null)
    {
      inventoryItem = new InventoryItem();
      inventoryItem.Init((int) this.Type, 0);
    }
    this.Init(inventoryItem);
  }

  public void Init(InventoryItem item)
  {
    this.Type = (InventoryItem.ITEM_TYPE) item.type;
    this.inventoryItem.SetImage((InventoryItem.ITEM_TYPE) item.type);
    int quantity = item.quantity;
    this.Text.text = quantity < 2 ? "" : item.quantity.ToString();
    Inventory.GetItemByType((int) this.Type);
    if (item.type == 0)
      return;
    this.Text.text = quantity.ToString();
  }
}
