// Decompiled with JetBrains decompiler
// Type: UIUnlockCurseIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
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
  private InventoryItem[] cost;

  public bool Locked { get; private set; } = true;

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
