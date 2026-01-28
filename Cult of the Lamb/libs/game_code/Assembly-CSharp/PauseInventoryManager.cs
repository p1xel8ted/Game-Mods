// Decompiled with JetBrains decompiler
// Type: PauseInventoryManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class PauseInventoryManager : BaseMonoBehaviour
{
  public UI_NavigatorSimple UINav;
  public GameObject InventoryItemObject;
  public GameObject InventoryItemParent;
  public TextMeshProUGUI Title;
  public Image FeaturedIcon;
  public TextMeshProUGUI Description;
  public TextMeshProUGUI Lore;
  public InventoryItemDisplay ItemDisplayReference;
  public CanvasGroup canvas;
  public List<InventoryItem.ITEM_TYPE> Blacklist = new List<InventoryItem.ITEM_TYPE>()
  {
    InventoryItem.ITEM_TYPE.SEEDS,
    InventoryItem.ITEM_TYPE.INGREDIENTS,
    InventoryItem.ITEM_TYPE.MEALS
  };
  public List<GameObject> Icons = new List<GameObject>();
  public bool WhiteListItem;
  public bool CanvasDisabled;

  public void OnChangeSelectionUnity(Selectable NewSelectable, Selectable PrevSelectable)
  {
    if (!((Object) NewSelectable != (Object) null))
      return;
    if (NewSelectable.GetComponent<PauseInventoryItem>().Type != InventoryItem.ITEM_TYPE.NONE)
    {
      InventoryItem.ITEM_TYPE type = NewSelectable.GetComponent<PauseInventoryItem>().Type;
      this.FeaturedIcon.enabled = true;
      this.Title.text = InventoryItem.LocalizedName(type);
      this.Description.text = InventoryItem.LocalizedDescription(type);
      this.Lore.text = InventoryItem.LocalizedLore(type);
      this.FeaturedIcon.sprite = this.ItemDisplayReference.GetImage(type);
    }
    else
    {
      this.Title.text = "No inventory items";
      this.Description.text = "";
      this.Lore.text = "";
      this.FeaturedIcon.enabled = false;
    }
  }

  public void NoItems()
  {
    this.Title.text = "No inventory items";
    this.Description.text = "";
    this.Lore.text = "";
    this.FeaturedIcon.enabled = false;
  }

  public bool CheckOnBlacklist(InventoryItem.ITEM_TYPE type)
  {
    bool flag = false;
    foreach (InventoryItem.ITEM_TYPE itemType in this.Blacklist)
    {
      if (type == itemType)
        flag = true;
    }
    return flag;
  }

  public void OnEnable()
  {
    foreach (Object icon in this.Icons)
      Object.Destroy(icon);
    this.InventoryItemObject.SetActive(false);
    this.UINav.OnChangeSelection += new UI_NavigatorSimple.ChangeSelection(this.OnChangeSelectionUnity);
    this.Icons.Clear();
    this.WhiteListItem = true;
    List<InventoryItem> items = Inventory.items;
    if (items.Count == 0)
    {
      this.NoItems();
    }
    else
    {
      foreach (InventoryItem inventoryItem in items)
      {
        if (!this.CheckOnBlacklist((InventoryItem.ITEM_TYPE) inventoryItem.type))
        {
          GameObject gameObject = Object.Instantiate<GameObject>(this.InventoryItemObject, this.InventoryItemParent.transform);
          gameObject.SetActive(true);
          gameObject.GetComponent<PauseInventoryItem>().Init((InventoryItem.ITEM_TYPE) inventoryItem.type, inventoryItem.quantity);
          this.Icons.Add(gameObject);
        }
      }
      while (this.Icons.Count < 21)
      {
        GameObject gameObject = Object.Instantiate<GameObject>(this.InventoryItemObject, this.InventoryItemParent.transform);
        gameObject.SetActive(true);
        gameObject.GetComponent<PauseInventoryItem>().Init(InventoryItem.ITEM_TYPE.NONE, 0);
        this.Icons.Add(gameObject);
      }
      Debug.Log((object) this.Icons[0]);
      this.Icons[0].GetComponent<Selectable>().Select();
      this.OnChangeSelectionUnity(this.Icons[0].GetComponent<Selectable>(), (Selectable) null);
    }
  }

  public void OnDisable()
  {
    this.UINav.OnChangeSelection -= new UI_NavigatorSimple.ChangeSelection(this.OnChangeSelectionUnity);
  }

  public void Start()
  {
  }

  public void SetDefault()
  {
    if (this.Icons.Count <= 0)
      return;
    Debug.Log((object) ("Set Default: " + ((object) this.Icons[0])?.ToString()));
    this.UINav.selectable = this.Icons[0].GetComponent<Selectable>();
    this.Icons[0].GetComponent<Selectable>().Select();
  }

  public void Update()
  {
    if (!this.canvas.interactable && !this.CanvasDisabled)
    {
      this.CanvasDisabled = true;
    }
    else
    {
      if (!this.CanvasDisabled || !this.canvas.interactable)
        return;
      this.SetDefault();
      this.CanvasDisabled = false;
    }
  }
}
