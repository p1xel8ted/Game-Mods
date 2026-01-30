// Decompiled with JetBrains decompiler
// Type: UIUnlockCurseIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class UIUnlockCurseIcon : 
  BaseMonoBehaviour,
  ISelectHandler,
  IEventSystemHandler,
  IDeselectHandler
{
  public TarotCards.Card Type;
  public Image Image;
  public Image SelectedIcon;
  public Image WhiteFlash;
  public Image OwnThisImage;
  public Material NormalMaterial;
  public Material BWMaterial;
  public Material redOutline;
  public Material greenOutline;
  public TMP_Text upgradeLevel;
  public TMP_Text upgradeLevelBackground;
  public GameObject costParent;
  public TMP_Text costText;
  public List<UIUnlockCurseIcon.TypeAndImage> Icons = new List<UIUnlockCurseIcon.TypeAndImage>();
  [CompilerGenerated]
  public bool \u003CLocked\u003Ek__BackingField = true;
  public InventoryItem[] cost;

  public bool Locked
  {
    get => this.\u003CLocked\u003Ek__BackingField;
    set => this.\u003CLocked\u003Ek__BackingField = value;
  }

  public void Init(TarotCards.Card Type)
  {
    this.Type = Type;
    foreach (UIUnlockCurseIcon.TypeAndImage icon in this.Icons)
    {
      if (icon.Type == this.Type)
        this.Image.sprite = icon.IconSprite;
    }
    this.SelectedIcon.enabled = false;
    this.WhiteFlash.color = new Color(1f, 1f, 1f, 0.0f);
    this.OwnThisImage.enabled = true;
    this.SelectedIcon.material = this.redOutline;
    this.Image.material = this.BWMaterial;
    this.costText.text = "";
    this.costParent.SetActive(false);
    foreach (InventoryItem inventoryItem in this.cost)
    {
      string str = Inventory.GetItemQuantity(inventoryItem.type) >= inventoryItem.quantity ? "<color=#f4ecd3>" : "<color=red>";
      this.costText.text += $"<size=30>{FontImageNames.GetIconByType((InventoryItem.ITEM_TYPE) inventoryItem.type)}</size> {str}{Inventory.GetItemQuantity(inventoryItem.type)}</color>/{inventoryItem.quantity}";
    }
  }

  public void SetUnlocked()
  {
    this.OwnThisImage.enabled = false;
    this.SelectedIcon.material = this.greenOutline;
    this.Image.material = this.NormalMaterial;
  }

  public void OnSelect(BaseEventData eventData)
  {
    this.SelectedIcon.enabled = true;
    this.transform.DOScale(Vector3.one * 1.1f, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
  }

  public void OnDeselect(BaseEventData eventData)
  {
    this.SelectedIcon.enabled = false;
    this.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    this.costParent.SetActive(false);
  }

  [Serializable]
  public class TypeAndImage
  {
    public TarotCards.Card Type;
    public Sprite IconSprite;
  }
}
