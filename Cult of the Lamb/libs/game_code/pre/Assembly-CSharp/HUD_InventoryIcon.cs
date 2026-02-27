// Decompiled with JetBrains decompiler
// Type: HUD_InventoryIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class HUD_InventoryIcon : BaseMonoBehaviour
{
  public InventoryItem.ITEM_TYPE Type;
  public InventoryItemDisplay inventoryItem;
  public TextMeshProUGUI Text;
  private RectTransform _rectTransform;

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
