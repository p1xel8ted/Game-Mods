// Decompiled with JetBrains decompiler
// Type: Lamb.UI.RanchAssignMenuItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI.Alerts;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class RanchAssignMenuItem : UIInventoryItem
{
  public Action<RanchAssignMenuItem> OnHighlighted;
  public Action<RanchAssignMenuItem> OnSelected;
  [CompilerGenerated]
  public InventoryItem.ITEM_TYPE \u003CAnimalType\u003Ek__BackingField;
  [SerializeField]
  public Image outline;
  [SerializeField]
  public Image background;
  [SerializeField]
  public Image greenOutline;
  [SerializeField]
  public Image lockObject;
  [Header("Alert")]
  [SerializeField]
  public RecipeAlert _alert;
  public StructuresData.Ranchable_Animal dummyAnimal;

  public StructuresData.Ranchable_Animal DummyAnimal => this.dummyAnimal;

  public InventoryItem.ITEM_TYPE AnimalType
  {
    get => this.\u003CAnimalType\u003Ek__BackingField;
    set => this.\u003CAnimalType\u003Ek__BackingField = value;
  }

  public void Awake()
  {
    if (!((UnityEngine.Object) this.Button != (UnityEngine.Object) null))
      return;
    this.Button.onClick.AddListener(new UnityAction(this.OnItemSelected));
    this.Button.OnSelected += new System.Action(this.OnItemHighlighted);
    this.Button.OnDeselected += new System.Action(this.OnItemUnhighlighted);
  }

  public void OnDestroy()
  {
    if (!((UnityEngine.Object) this.Button != (UnityEngine.Object) null))
      return;
    this.Button.onClick.RemoveAllListeners();
    this.Button.OnSelected -= new System.Action(this.OnItemHighlighted);
    this.Button.OnDeselected -= new System.Action(this.OnItemUnhighlighted);
  }

  public override void Configure(InventoryItem.ITEM_TYPE animalType, bool disabled = false)
  {
    this._alert.Configure(animalType);
    int itemQuantity = Inventory.GetItemQuantity(animalType);
    base.Configure(animalType, !disabled || itemQuantity > 0);
    this.Button.Interactable = true;
    this.AnimalType = animalType;
    this.outline.transform.DOScale(0.95f, 0.0f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    if (disabled && itemQuantity <= 0)
    {
      this._icon.color = Color.black;
      this.background.color = Color.grey;
      this.lockObject.gameObject.SetActive(true);
      this.greenOutline.gameObject.SetActive(false);
      this._amountText.text = "";
    }
    else
    {
      this._icon.color = Color.white;
      this.lockObject.gameObject.SetActive(false);
      if (itemQuantity > 0)
      {
        this.background.color = Color.white;
        this._amountText.color = Color.white;
        this._amountText.text = Inventory.GetItemQuantity(animalType).ToString();
        AnimalData.TryDiscoverAnimal(animalType);
      }
      else
      {
        this.greenOutline.gameObject.SetActive(false);
        this.background.color = Color.grey;
        this.outline.color = Color.black;
        this._amountText.color = Color.red;
        this._amountText.text = $"<color=#FF0000>{Inventory.GetItemQuantity(animalType).ToString()}</color>";
      }
    }
  }

  public void OnItemSelected()
  {
    if (Inventory.GetItemQuantity(this.AnimalType) <= 0)
    {
      this.Shake();
    }
    else
    {
      Action<RanchAssignMenuItem> onSelected = this.OnSelected;
      if (onSelected == null)
        return;
      onSelected(this);
    }
  }

  public void OnItemUnhighlighted()
  {
    this.outline.transform.DOKill();
    this.outline.transform.DOScale(0.95f, 0.3f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart);
    this.outline.color = Color.gray;
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.DeSelected());
  }

  public void OnItemHighlighted()
  {
    this._alert.TryRemoveAlert();
    this.outline.transform.DOKill();
    this.outline.transform.DOScale(1.1f, 0.3f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart);
    this.background.color = Color.white;
    Action<RanchAssignMenuItem> onHighlighted = this.OnHighlighted;
    if (onHighlighted != null)
      onHighlighted(this);
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.Selected(this.transform.localScale.x, 1.2f));
  }

  public IEnumerator Selected(float starting, float target)
  {
    RanchAssignMenuItem ranchAssignMenuItem = this;
    ranchAssignMenuItem.transform.localScale = Vector3.one;
    float progress = 0.0f;
    float duration = 0.1f;
    while ((double) (progress += Time.unscaledDeltaTime) < (double) duration)
    {
      float num = Mathf.SmoothStep(starting, target, progress / duration);
      ranchAssignMenuItem.transform.localScale = Vector3.one * num;
      yield return (object) null;
    }
    float num1 = target;
    ranchAssignMenuItem.transform.localScale = Vector3.one * num1;
  }

  public IEnumerator DeSelected()
  {
    RanchAssignMenuItem ranchAssignMenuItem = this;
    float progress = 0.0f;
    float duration = 0.3f;
    float startingScale = ranchAssignMenuItem.transform.localScale.x;
    float targetScale = 1f;
    while ((double) (progress += Time.unscaledDeltaTime) < (double) duration)
    {
      float num = Mathf.SmoothStep(startingScale, targetScale, progress / duration);
      ranchAssignMenuItem.transform.localScale = Vector3.one * num;
      yield return (object) null;
    }
    float num1 = targetScale;
    ranchAssignMenuItem.transform.localScale = Vector3.one * num1;
  }

  public StructuresData.Ranchable_Animal GetDummyAnimal()
  {
    StructuresData.Ranchable_Animal dummyAnimal = new StructuresData.Ranchable_Animal()
    {
      Type = this.AnimalType,
      Ears = UnityEngine.Random.Range(1, 6),
      Head = UnityEngine.Random.Range(1, 6),
      Horns = UnityEngine.Random.Range(1, 6),
      Colour = UnityEngine.Random.Range(0, 10)
    };
    this.dummyAnimal = dummyAnimal;
    return dummyAnimal;
  }
}
