// Decompiled with JetBrains decompiler
// Type: TailorItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using Spine.Unity;
using src.UI.Alerts;
using src.UINavigator;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class TailorItem : UIInventoryItem, ISelectHandler, IEventSystemHandler, IDeselectHandler
{
  public Action<TailorItem> OnItemSelected;
  public Action<TailorItem> OnItemHighlighted;
  public Action<TailorItem> OnShakeConfigureCard;
  public Action<bool> OnRemoveQueueSelected;
  [SerializeField]
  public Image _progressRing;
  [SerializeField]
  public Image _canAffordIcon;
  [SerializeField]
  public Image _cantAffordIcon;
  [SerializeField]
  public Image _selectedIcon;
  [SerializeField]
  public TextMeshProUGUI _costText;
  [SerializeField]
  public TextMeshProUGUI _amountCountText;
  [SerializeField]
  public CanvasGroup _cancelCanvasGroup;
  [SerializeField]
  public SkeletonGraphic spine;
  [SerializeField]
  public GameObject _tickMark;
  [SerializeField]
  public Image unavailable;
  [SerializeField]
  public SkeletonGraphic followerSpine;
  [SerializeField]
  public Image defaultRobesIcon;
  [SerializeField]
  public Image DLCRobesIcon;
  [SerializeField]
  public Image _flashIcon;
  [SerializeField]
  public Image lockedIcon;
  [SerializeField]
  public Image lockedTint;
  [SerializeField]
  public ClothingAlertBadge _alert;
  [SerializeField]
  public ClothingCustomiseAlertBadge _alertCustomise;
  [SerializeField]
  public ClothingAssignAlertBadge _alertAssign;
  public TailorItem.InMenu _menuType;
  public Vector2 _anchoredOrigin;
  public bool _canAfford;
  public bool _queued;
  public bool _firstQueuedResource;
  [CompilerGenerated]
  public ClothingData \u003CClothingData\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CVariant\u003Ek__BackingField;
  public bool ShowQuantity;
  public bool hasCrafted;

  public ClothingData ClothingData
  {
    get => this.\u003CClothingData\u003Ek__BackingField;
    set => this.\u003CClothingData\u003Ek__BackingField = value;
  }

  public string Variant
  {
    get => this.\u003CVariant\u003Ek__BackingField;
    set => this.\u003CVariant\u003Ek__BackingField = value;
  }

  public void SetMenu(TailorItem.InMenu menuType)
  {
    this._menuType = menuType;
    if (this._menuType != TailorItem.InMenu.Customise && menuType == TailorItem.InMenu.Craft && this.ClothingData.ClothingType == FollowerClothingType.None)
    {
      this._canAffordIcon.gameObject.SetActive(false);
      this._cantAffordIcon.gameObject.SetActive(true);
    }
    this.UpdateAlerts();
  }

  public TailorItem.InMenu GetMenu() => this._menuType;

  public void Awake()
  {
    this._button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
    this._button.OnSelected += new System.Action(this.OnButtonSelected);
  }

  public void Configure(
    FollowerClothingType data,
    string variant,
    bool queued = false,
    int queuedIndex = 0,
    bool showQuantity = false)
  {
    this.Configure(TailorManager.GetClothingData(data), variant, queued, queuedIndex, showQuantity);
    this._canAffordIcon.gameObject.SetActive(false);
    this._cantAffordIcon.gameObject.SetActive(false);
    this._costText.gameObject.SetActive(false);
  }

  public void Configure(
    ClothingData data,
    string variant,
    bool queued = false,
    int queuedIndex = 0,
    bool showQuantity = false)
  {
    this._alert.Configure(data.ClothingType);
    this._alertCustomise.Configure(data.ClothingType);
    this._alertAssign.Configure(data.ClothingType);
    if (data.IsDLC)
      this.DLCRobesIcon.gameObject.SetActive(true);
    else
      this.DLCRobesIcon.gameObject.SetActive(false);
    this._queued = queued;
    this._anchoredOrigin = this._container.anchoredPosition;
    this._selectedIcon.enabled = false;
    this._cancelCanvasGroup.alpha = 0.0f;
    this.lockedIcon.enabled = false;
    this.lockedTint.enabled = false;
    this.defaultRobesIcon.enabled = false;
    this.ClothingData = data;
    if (this.ClothingData.CanBeCrafted)
      this._button.OnConfirmDenied += new System.Action(((UIInventoryItem) this).Shake);
    else
      this._button.OnConfirmDenied += new System.Action(this.ShakeCard);
    this.Variant = variant;
    this.spine.ConfigureFollowerOutfit(this.ClothingData, DataManager.Instance.GetClothingColour(data.ClothingType), variant);
    StructuresData.ItemCost[] itemCostArray = new StructuresData.ItemCost[this.ClothingData.Cost.Length];
    for (int index = 0; index < itemCostArray.Length; ++index)
      itemCostArray[index] = new StructuresData.ItemCost(this.ClothingData.Cost[index].ItemType, this.ClothingData.Cost[index].Cost);
    this._costText.gameObject.SetActive(!queued);
    this.ShowQuantity = showQuantity;
    if (!showQuantity)
      this._costText.gameObject.SetActive(false);
    this._costText.text = StructuresData.ItemCost.GetCostString(itemCostArray);
    this._firstQueuedResource = queuedIndex == 0;
    this._canAfford = TailorManager.CanAfford(this.ClothingData);
    this._tickMark.gameObject.SetActive(false);
    this._amountCountText.gameObject.SetActive(false);
    if (!this._queued)
      this._button.Confirmable = this._canAfford;
    else
      this._button.Confirmable = true;
    this.UpdateQuantity();
    if (this.ClothingData.ClothingType == FollowerClothingType.None)
      this.defaultRobesIcon.enabled = true;
    this.followerSpine.gameObject.SetActive(false);
    if (!TailorManager.GetClothingAvailable(data.ClothingType) && this.ClothingData.ClothingType != FollowerClothingType.None)
      this.setLocked();
    if (!queued)
      return;
    this.HideAllAlerts();
  }

  public void setLocked()
  {
    this.spine.color = Color.black;
    this.lockedIcon.enabled = true;
    this.lockedTint.enabled = true;
    this._canAffordIcon.gameObject.SetActive(false);
    this._cantAffordIcon.gameObject.SetActive(true);
    this._costText.gameObject.SetActive(false);
  }

  public void SetCount()
  {
    if (this.ClothingData.ClothingType == FollowerClothingType.None || !this.ShowQuantity)
      return;
    this._amountCountText.gameObject.SetActive(true);
    int craftedCount = TailorManager.GetCraftedCount(this.ClothingData.ClothingType);
    if (DataManager.Instance._revealingOutfits.ContainsKey(this.ClothingData.ClothingType))
      craftedCount -= DataManager.Instance._revealingOutfits[this.ClothingData.ClothingType];
    if (craftedCount > 0)
      this._amountCountText.text = craftedCount.ToString();
    else
      this._amountCountText.gameObject.SetActive(false);
  }

  public override void UpdateQuantity()
  {
    this._canAfford = TailorManager.CanAfford(this.ClothingData);
    bool flag = TailorManager.GetClothingAvailable(this.ClothingData.ClothingType) || this.ClothingData.ClothingType == FollowerClothingType.None;
    this._button.Confirmable = this._canAfford & flag;
    StructuresData.ItemCost[] itemCostArray = new StructuresData.ItemCost[this.ClothingData.Cost.Length];
    for (int index = 0; index < itemCostArray.Length; ++index)
      itemCostArray[index] = new StructuresData.ItemCost(this.ClothingData.Cost[index].ItemType, this.ClothingData.Cost[index].Cost);
    this._costText.text = StructuresData.ItemCost.GetCostString(itemCostArray);
    this._tickMark.gameObject.SetActive(false);
    if (!this._queued)
    {
      this._costText.gameObject.SetActive(true);
      this._button.Confirmable = this._canAfford & flag;
      if (this._showQuantity)
      {
        this._icon.color = new Color(this._canAfford ? 1f : 0.0f, 1f, 1f, 1f);
        this._canAffordIcon.color = new Color(this._canAfford ? 1f : 0.0f, 1f, 1f, 1f);
      }
      this._canAffordIcon.gameObject.SetActive(this._canAfford);
      this._cantAffordIcon.gameObject.SetActive(!this._canAfford);
    }
    else
    {
      this._button.Confirmable = true;
      this._canAffordIcon.gameObject.SetActive(true);
      this._cantAffordIcon.gameObject.SetActive(false);
    }
    if (this.ClothingData.SpecialClothing && !this._queued)
    {
      if (TailorManager.IsClothingAvailable(this.ClothingData.ClothingType))
      {
        this._canAffordIcon.gameObject.SetActive(this._canAfford);
        this._cantAffordIcon.gameObject.SetActive(!this._canAfford);
        this._button.Confirmable = this._canAfford & flag;
      }
      else
      {
        this._canAffordIcon.gameObject.SetActive(false);
        this._cantAffordIcon.gameObject.SetActive(true);
        this._button.Confirmable = false;
        this._tickMark.gameObject.SetActive(true);
        this._costText.gameObject.SetActive(false);
      }
      if (!TailorManager.GetClothingAvailable(this.ClothingData.ClothingType))
        this.spine.color = Color.black;
      else
        this.spine.color = Color.white;
    }
    if (this.ClothingData.ClothingType == FollowerClothingType.None && this._menuType == TailorItem.InMenu.Craft)
    {
      this._canAffordIcon.gameObject.SetActive(false);
      this._cantAffordIcon.gameObject.SetActive(true);
    }
    this.SetCount();
    if (TailorManager.GetClothingAvailable(this.ClothingData.ClothingType) || this.ClothingData.ClothingType == FollowerClothingType.None)
      return;
    this.setLocked();
  }

  public void UpdateAlerts()
  {
    this._alert.gameObject.SetActive(this._alert.HasAlert() && this._menuType == TailorItem.InMenu.Craft);
    this._alertCustomise.gameObject.SetActive(this._alertCustomise.HasAlert() && this._menuType == TailorItem.InMenu.Customise);
    this._alertAssign.gameObject.SetActive(this._alertAssign.HasAlert() && this._menuType == TailorItem.InMenu.Assign);
  }

  public void HideAllAlerts()
  {
    this._alert.gameObject.SetActive(false);
    this._alertCustomise.gameObject.SetActive(false);
    this._alertAssign.gameObject.SetActive(false);
  }

  public void UpdateIcon(string variant)
  {
    this.Variant = variant;
    this.spine.ConfigureFollowerOutfit(this.ClothingData, DataManager.Instance.GetClothingColour(this.ClothingData.ClothingType), this.Variant);
  }

  public void OnButtonClicked()
  {
    if (!TailorManager.GetClothingAvailable(this.ClothingData.ClothingType))
      this.Shake();
    else if (this.ClothingData.ClothingType == FollowerClothingType.None && this._menuType == TailorItem.InMenu.Craft)
    {
      this.Shake();
      this.ShakeCard();
    }
    else
    {
      Action<TailorItem> onItemSelected = this.OnItemSelected;
      if (onItemSelected == null)
        return;
      onItemSelected(this);
    }
  }

  public void OnButtonSelected()
  {
    Action<TailorItem> onItemHighlighted = this.OnItemHighlighted;
    if (onItemHighlighted == null)
      return;
    onItemHighlighted(this);
  }

  public void ShakeCard()
  {
    Action<TailorItem> shakeConfigureCard = this.OnShakeConfigureCard;
    if (shakeConfigureCard == null)
      return;
    shakeConfigureCard(this);
  }

  public void UpdateRefiningProgress(float normTime) => this._progressRing.fillAmount = normTime;

  public void OnSelect(BaseEventData eventData)
  {
    if (this._menuType == TailorItem.InMenu.Craft)
      this._alert.TryRemoveAlert();
    else if (this._menuType == TailorItem.InMenu.Customise)
      this._alertCustomise.TryRemoveAlert();
    else
      this._alertAssign.TryRemoveAlert();
    this._selectedIcon.enabled = true;
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.Selected(this.transform.localScale.x, 1.25f));
  }

  public void OnDeselect(BaseEventData eventData)
  {
    this._selectedIcon.enabled = false;
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.DeSelected());
  }

  public IEnumerator Selected(float starting, float target)
  {
    TailorItem tailorItem = this;
    tailorItem._canvasGroup.alpha = 1f;
    tailorItem.transform.localScale = Vector3.one;
    float progress = 0.0f;
    float duration = 0.1f;
    while ((double) (progress += Time.unscaledDeltaTime) < (double) duration)
    {
      float num = Mathf.SmoothStep(starting, target, progress / duration);
      if (tailorItem._queued)
        tailorItem._cancelCanvasGroup.alpha = progress / duration;
      tailorItem.transform.localScale = Vector3.one * num;
      yield return (object) null;
    }
    float num1 = target;
    tailorItem.transform.localScale = Vector3.one * num1;
    if (tailorItem._queued)
    {
      tailorItem._cancelCanvasGroup.alpha = 1f;
      Action<bool> removeQueueSelected = tailorItem.OnRemoveQueueSelected;
      if (removeQueueSelected != null)
        removeQueueSelected(true);
    }
  }

  public IEnumerator DeSelected()
  {
    TailorItem tailorItem = this;
    float progress = 0.0f;
    float duration = 0.3f;
    float startingScale = tailorItem.transform.localScale.x;
    float targetScale = 1f;
    while ((double) (progress += Time.unscaledDeltaTime) < (double) duration)
    {
      float num = Mathf.SmoothStep(startingScale, targetScale, progress / duration);
      tailorItem.transform.localScale = Vector3.one * num;
      if (tailorItem._queued)
        tailorItem._cancelCanvasGroup.alpha = (float) (1.0 - (double) progress / (double) duration);
      yield return (object) null;
    }
    float num1 = targetScale;
    tailorItem.transform.localScale = Vector3.one * num1;
    if (tailorItem._queued)
    {
      IMMSelectable currentSelectable = MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable;
      if (currentSelectable != null)
      {
        TailorItem component = currentSelectable.Selectable.GetComponent<TailorItem>();
        if ((UnityEngine.Object) component == (UnityEngine.Object) null || !component._queued)
        {
          tailorItem._cancelCanvasGroup.alpha = 0.0f;
          Action<bool> removeQueueSelected = tailorItem.OnRemoveQueueSelected;
          if (removeQueueSelected != null)
            removeQueueSelected(false);
        }
      }
      else
      {
        tailorItem._cancelCanvasGroup.alpha = 0.0f;
        Action<bool> removeQueueSelected = tailorItem.OnRemoveQueueSelected;
        if (removeQueueSelected != null)
          removeQueueSelected(false);
      }
    }
  }

  public void ForceDeselect()
  {
    this.StopAllCoroutines();
    this.transform.localScale = Vector3.one;
    if (!this._queued)
      return;
    this._cancelCanvasGroup.alpha = 0.0f;
  }

  public void FadeIngredients(float fade)
  {
    this._costText.DOKill();
    ShortcutExtensionsTMPText.DOFade(this._costText, fade, 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
  }

  public void SetGrey(float fade)
  {
    DOTweenModuleUI.DOFade(this.unavailable, fade, 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
  }

  public void CheckIfCrafted()
  {
    if (this.ClothingData.ClothingType == FollowerClothingType.None)
      return;
    this.hasCrafted = false;
    foreach (FollowerClothingType followerClothingType in DataManager.Instance.clothesCrafted)
    {
      if (followerClothingType == this.ClothingData.ClothingType)
      {
        this.hasCrafted = true;
        return;
      }
    }
    if (!this.hasCrafted)
    {
      this._button.Confirmable = false;
      this.SetGrey(0.5f);
    }
    else
      this._button.Confirmable = true;
  }

  public void ShowTick(bool show)
  {
    this.spine.color = show ? Color.gray : this.spine.color;
    this._canAffordIcon.gameObject.SetActive(!show);
    this._cantAffordIcon.gameObject.SetActive(show);
    this._button.Confirmable = !show;
    this._tickMark.gameObject.SetActive(show);
  }

  public void ShowAssignedFollower(FollowerBrain follower)
  {
    this.followerSpine.gameObject.SetActive(true);
    FollowerBrain.SetFollowerCostume(this.followerSpine.Skeleton, follower._directInfoAccess, forceUpdate: true, setData: false);
  }

  public void HideAssignedFollower()
  {
    this.followerSpine.gameObject.SetActive(false);
    this.SetCount();
  }

  public IEnumerator DoUnlock()
  {
    TailorItem tailorItem = this;
    tailorItem._container.DOScale(Vector3.one * 0.75f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuad).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.125f);
    DataManager.Instance.Alerts.ClothingAssignAlerts.AddOnce(tailorItem.ClothingData.ClothingType);
    if (DataManager.Instance.Alerts.ClothingAssignAlerts.HasAlert(tailorItem.ClothingData.ClothingType))
      tailorItem._alertAssign.gameObject.SetActive(true);
    tailorItem._alert.gameObject.SetActive(false);
    tailorItem._alertCustomise.gameObject.SetActive(false);
    UIManager.PlayAudio("event:/player/layer_clothes");
    tailorItem._container.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    tailorItem._flashIcon.gameObject.SetActive(true);
    DOTweenModuleUI.DOColor(tailorItem._flashIcon, new Color(1f, 1f, 1f, 0.0f), 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    tailorItem.SetCount();
    yield return (object) new WaitForSecondsRealtime(0.1f);
  }

  public enum InMenu
  {
    None,
    Craft,
    Customise,
    Assign,
  }
}
