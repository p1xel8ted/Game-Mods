// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIRelicMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using src.Extensions;
using src.UI.InfoCards;
using src.UI.Items;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UIRelicMenuController : UIMenuBase
{
  public const string kUnlockAnimationState = "Unlock";
  public const string kShowUnlockAnimationState = "Show Unlock";
  [SerializeField]
  public MMScrollRect _scrollRect;
  [SerializeField]
  public RectTransform _contentContainer;
  [SerializeField]
  public RectTransform _DLCHeader;
  [SerializeField]
  public RectTransform _DLCcontentContainer;
  [SerializeField]
  public RectTransform _CoOpHeader;
  [SerializeField]
  public RectTransform _CoOpcontentContainer;
  [SerializeField]
  public TMP_Text _collectedText;
  [SerializeField]
  public UIMenuControlPrompts _controlPrompts;
  [SerializeField]
  public RelicInfoCardController _infoCardController;
  [Header("Reveal Sequence")]
  [SerializeField]
  public CanvasGroup _unlockHeaderCanvasGroup;
  [SerializeField]
  public RectTransform _front;
  [SerializeField]
  public GameObject _soloTitleHeader;
  [SerializeField]
  public GameObject _coopTitleHeader;
  public List<RelicItem> _items = new List<RelicItem>();
  public RelicData _showRelic;
  public List<RelicData> _showRelics;
  public int _relicCount;
  public PlayerFarming playerFarming;

  public void SetCoopTitleHeader()
  {
    this._soloTitleHeader.SetActive(false);
    this._coopTitleHeader.SetActive(true);
  }

  public int relicsTotalCount
  {
    get
    {
      return !DataManager.Instance.PlayerFoundRelics.Contains(RelicType.DamageEye_Coop) ? EquipmentManager.RelicData.Length - EquipmentManager.CoopRelics.Count : EquipmentManager.RelicData.Length;
    }
  }

  public void Show(RelicType relicType, PlayerFarming playerFarming, bool instant = false)
  {
    this.playerFarming = playerFarming;
    this._showRelic = EquipmentManager.GetRelicData(relicType);
    this._relicCount = Mathf.Clamp(DataManager.Instance.PlayerFoundRelics.Count - 1, 0, int.MaxValue);
    this.Show(instant);
  }

  public void Show(List<RelicType> relicTypes, bool instant = false)
  {
    this.Show(EquipmentManager.GetRelicData(relicTypes), instant);
  }

  public void Show(List<RelicData> relicDatas, bool instant = false)
  {
    this._showRelics = relicDatas;
    this._relicCount = Mathf.Clamp(DataManager.Instance.PlayerFoundRelics.Count - this._showRelics.Count, 0, int.MaxValue);
    this.Show(instant);
  }

  public override void OnShowStarted()
  {
    this._unlockHeaderCanvasGroup.alpha = 0.0f;
    UIManager.PlayAudio("event:/ui/open_menu");
    this._controlPrompts.HideAcceptButton();
    this._scrollRect.normalizedPosition = Vector2.one;
    if (!CoopManager.CoopActive)
      this._CoOpHeader.gameObject.SetActive(false);
    List<RelicData> relicDataList = new List<RelicData>((IEnumerable<RelicData>) EquipmentManager.RelicData);
    for (int index = relicDataList.Count - 1; index >= 0; --index)
    {
      if (!DataManager.Instance.PlayerFoundRelics.Contains(relicDataList[index].RelicType) && !CoopManager.CoopActive && EquipmentManager.CoopRelics.Contains(relicDataList[index].RelicType))
        relicDataList.RemoveAt(index);
    }
    foreach (RelicData data in relicDataList)
    {
      RelicItem relicItem = !RelicData.GetRelicDLC(data.RelicType) ? (data.RelicSubType != RelicSubType.Coop ? MonoSingleton<UIManager>.Instance.RelicItemTemplate.Instantiate<RelicItem>((Transform) this._contentContainer) : MonoSingleton<UIManager>.Instance.RelicItemTemplate.Instantiate<RelicItem>((Transform) this._CoOpcontentContainer)) : MonoSingleton<UIManager>.Instance.RelicItemTemplate.Instantiate<RelicItem>((Transform) this._DLCcontentContainer);
      relicItem.Configure(data);
      this._items.Add(relicItem);
    }
    if ((UnityEngine.Object) this._showRelic == (UnityEngine.Object) null && this._showRelics == null)
    {
      this._relicCount = Mathf.Clamp(DataManager.Instance.PlayerFoundRelics.Count, 0, int.MaxValue);
      this.OverrideDefault((Selectable) this._items[0].Selectable);
      this.ActivateNavigation();
    }
    this._collectedText.text = string.Format(LocalizationManager.GetTranslation("UI/Collected"), (object) (LocalizeIntegration.FormatCurrentMax(this._relicCount.ToString(), this.relicsTotalCount.ToString()) ?? ""));
    if (GameManager.AuthenticateMajorDLC())
      return;
    this._DLCHeader.gameObject.SetActive(false);
    this._DLCcontentContainer.gameObject.SetActive(false);
  }

  public override IEnumerator DoShowAnimation()
  {
    if ((UnityEngine.Object) this._showRelic != (UnityEngine.Object) null)
      yield return (object) this.DoUnlockAnimationSingle();
    else if (this._showRelics != null && this._showRelics.Count > 0)
      yield return (object) this.DoUnlockAnimationMulti();
    else
      yield return (object) this.\u003C\u003En__0();
  }

  public IEnumerator DoUnlockAnimationSingle()
  {
    UIRelicMenuController relicMenuController = this;
    relicMenuController._canvasGroup.interactable = false;
    relicMenuController._scrollRect.vertical = false;
    relicMenuController._controlPrompts.HideCancelButton();
    RelicItem target = relicMenuController._items[0];
    foreach (RelicItem relicItem in relicMenuController._items)
    {
      if (relicItem.Data.RelicType == relicMenuController._showRelic.RelicType)
      {
        target = relicItem;
        target.ForceLockedState();
        target.Alert.TryRemoveAlert();
      }
      else
        relicItem.ForceIncognitoState();
    }
    relicMenuController._infoCardController.enabled = false;
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    relicMenuController.SetActiveStateForMenu(false);
    RelicInfoCard infoCard = relicMenuController._infoCardController.Card1;
    infoCard.Configure(relicMenuController._showRelic, relicMenuController.playerFarming);
    infoCard.Hide(true);
    infoCard.Animator.enabled = false;
    infoCard.CanvasGroup.alpha = 0.0f;
    infoCard.RectTransform.SetParent((Transform) relicMenuController._front);
    infoCard.RectTransform.anchoredPosition = Vector2.zero;
    Transform originalParent = infoCard.IconContainer.parent;
    RectTransform iconContainer = infoCard.IconContainer;
    CanvasGroup iconCanvasGroup = infoCard.IconCanvasGroup;
    Vector2 originalScale = (Vector2) iconContainer.localScale;
    Vector2 originalPosition = (Vector2) iconContainer.localPosition;
    iconContainer.SetParent((Transform) relicMenuController._front);
    iconCanvasGroup.alpha = 0.0f;
    iconContainer.localScale = (Vector3) (originalScale * 3f);
    iconContainer.localPosition = (Vector3) Vector2.zero;
    yield return (object) relicMenuController._animator.YieldForAnimation("Unlock");
    yield return (object) new WaitForSecondsRealtime(0.15f);
    iconContainer.DOScale((Vector3) (originalScale * 5f), 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    iconCanvasGroup.DOFade(1f, 0.5f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutBack).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.75f);
    iconContainer.DOLocalMove((Vector3) (Vector2) relicMenuController._front.InverseTransformPoint(originalParent.TransformPoint((Vector3) originalPosition)), 0.65f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
    iconContainer.DOScale((Vector3) originalScale, 0.65f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
    yield return (object) new WaitForSecondsRealtime(0.15f);
    infoCard.CanvasGroup.DOFade(1f, 0.75f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.55f);
    iconContainer.SetParent(originalParent);
    relicMenuController._controlPrompts.ShowAcceptButton();
    while (!InputManager.UI.GetAcceptButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
      yield return (object) null;
    relicMenuController._controlPrompts.HideAcceptButton();
    infoCard.RectTransform.SetParent(relicMenuController._infoCardController.transform, true);
    infoCard.RectTransform.DOAnchorPos(Vector2.zero, 0.66f).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
    relicMenuController._unlockHeaderCanvasGroup.DOFade(0.0f, 0.66f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    AudioManager.Instance.PlayOneShot("event:/sermon/scroll_sermon_menu", relicMenuController.gameObject);
    yield return (object) relicMenuController._animator.YieldForAnimation("Show Unlock");
    yield return (object) new WaitForSecondsRealtime(0.25f);
    relicMenuController.OverrideDefaultOnce((Selectable) target.Selectable);
    AudioManager.Instance.PlayOneShot("event:/sermon/scroll_sermon_menu", relicMenuController.gameObject);
    yield return (object) relicMenuController._scrollRect.DoScrollTo(target.RectTransform);
    ++relicMenuController._relicCount;
    UIManager.PlayAudio("event:/player/new_item_sequence_close");
    yield return (object) target.DoUnlock();
    relicMenuController._collectedText.text = string.Format(LocalizationManager.GetTranslation("UI/Collected"), (object) (LocalizeIntegration.FormatCurrentMax(relicMenuController._relicCount.ToString(), relicMenuController.relicsTotalCount.ToString()) ?? ""));
    target.Alert.gameObject.SetActive(true);
    target.Selectable.OnDeselected += (System.Action) (() => target.Alert.gameObject.SetActive(false));
    infoCard.Animator.enabled = true;
    infoCard.Show(true);
    relicMenuController._infoCardController.ForceCurrentCard(infoCard, relicMenuController._showRelic);
    relicMenuController._infoCardController.enabled = true;
    relicMenuController.SetActiveStateForMenu(true);
    relicMenuController._controlPrompts.ShowCancelButton();
    relicMenuController._scrollRect.vertical = true;
    relicMenuController._canvasGroup.interactable = true;
  }

  public override void OnHideStarted()
  {
    base.OnHideStarted();
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    UIManager.PlayAudio("event:/ui/close_menu");
  }

  public IEnumerator DoUnlockAnimationMulti()
  {
    UIRelicMenuController relicMenuController = this;
    relicMenuController._canvasGroup.interactable = false;
    relicMenuController._infoCardController.enabled = false;
    relicMenuController._scrollRect.ScrollSpeedModifier = 2f;
    relicMenuController._controlPrompts.HideCancelButton();
    List<RelicItem> relicItems = new List<RelicItem>();
    foreach (RelicItem relicItem in relicMenuController._items)
    {
      RelicItem item = relicItem;
      item.Selectable.Interactable = false;
      if (relicMenuController._showRelics.Contains(item.Data))
      {
        relicItems.Add(item);
        item.ForceLockedState();
      }
      else
        item.ForceIncognitoState();
      item.Selectable.OnDeselected += (System.Action) (() => item.Alert.gameObject.SetActive(false));
    }
    relicItems.Sort((Comparison<RelicItem>) ((a, b) => a.RectTransform.GetSiblingIndex().CompareTo(b.RectTransform.GetSiblingIndex())));
    yield return (object) relicMenuController._animator.YieldForAnimation("Show");
    yield return (object) new WaitForSecondsRealtime(0.1f);
    for (int i = 0; i < relicItems.Count; ++i)
    {
      float num = (float) (2.0 + (double) Mathf.Floor((float) i / 3f) * 0.30000001192092896);
      relicMenuController._scrollRect.ScrollSpeedModifier = num;
      ++relicMenuController._relicCount;
      yield return (object) relicMenuController._scrollRect.DoScrollTo(relicItems[i].RectTransform);
      relicMenuController._collectedText.text = string.Format(LocalizationManager.GetTranslation("UI/Collected"), (object) (LocalizeIntegration.FormatCurrentMax(relicMenuController._relicCount.ToString(), relicMenuController.relicsTotalCount.ToString()) ?? ""));
      yield return (object) relicItems[i].Flash();
    }
    for (int index = 0; index < relicItems.Count; ++index)
      relicMenuController.StartCoroutine((IEnumerator) relicItems[index].ShowAlert());
    yield return (object) new WaitForSecondsRealtime(0.1f);
    relicMenuController._scrollRect.ScrollSpeedModifier = 1f;
    relicMenuController.OverrideDefault((Selectable) relicItems.LastElement<RelicItem>().Selectable);
    relicMenuController._infoCardController.enabled = true;
    relicMenuController.SetActiveStateForMenu(true);
    relicMenuController._controlPrompts.ShowCancelButton();
    relicMenuController._canvasGroup.interactable = true;
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  public override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  [CompilerGenerated]
  [DebuggerHidden]
  public IEnumerator \u003C\u003En__0() => base.DoShowAnimation();
}
