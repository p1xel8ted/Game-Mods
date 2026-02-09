// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UICultUpgradesMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI.Rituals;
using src.UINavigator;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using Unify;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class UICultUpgradesMenuController : UIMenuBase
{
  public static System.Action OnCultUpgraded;
  public string kSelectedAnimationState = "Selected";
  public string kCancelSelectionAnimationState = "Cancelled";
  public string kConfirmedSelectionAnimationState = "Confirmed";
  [Header("Cult Upgrade")]
  [SerializeField]
  public CultUpgradeInfoCardController _cultUpgradeInfoCardController;
  [SerializeField]
  public CultUpgradeItem _cultUpgradeItem;
  [SerializeField]
  public TextMeshProUGUI _cultUpgradeItemLevel;
  [SerializeField]
  public MMScrollRect scrollRect;
  [SerializeField]
  public GameObject _ritualItemAlert;
  [Header("Borders")]
  [SerializeField]
  public CultUpgradeItem[] _upgradeShopItems;
  [Header("Misc")]
  [SerializeField]
  public RectTransform _rootContainer;
  [SerializeField]
  public UIHoldInteraction _uiHoldInteraction;
  [SerializeField]
  public UIMenuControlPrompts _controlPrompts;
  [SerializeField]
  public UICultUpgradeProgress _progressUI;
  [SerializeField]
  public GameObject[] _hiddenBeforeFirstCultUpgrade;
  [SerializeField]
  public GameObject[] _hiddenBeforeFirstBorderUpgrade;
  [Header("DLC")]
  [SerializeField]
  public RectTransform _dlcHeader;
  [SerializeField]
  public RectTransform _dlcContent;
  [SerializeField]
  public CultUpgradeItem[] _dlcUpgradeShopItems;
  public int templeLevel;
  public bool _didCancel;
  public bool preventCloseUntilRevealComplete;
  [CompilerGenerated]
  public CultUpgradeData.TYPE \u003CRevealing\u003Ek__BackingField;

  public CultUpgradeData.TYPE Revealing
  {
    get => this.\u003CRevealing\u003Ek__BackingField;
    set => this.\u003CRevealing\u003Ek__BackingField = value;
  }

  public void Start()
  {
    this._dlcHeader.gameObject.SetActive(SeasonsManager.Active);
    this._dlcContent.gameObject.SetActive(SeasonsManager.Active);
    this.templeLevel = DataManager.Instance.TempleLevel;
    this._cultUpgradeItem.Configure((CultUpgradeData.TYPE) (1 + Mathf.Max(0, this.templeLevel)));
    for (int index = 0; index < this._upgradeShopItems.Length; ++index)
      this._upgradeShopItems[index].Configure((CultUpgradeData.TYPE) (100 + index));
    for (int index = 0; index < this._dlcUpgradeShopItems.Length; ++index)
      this._dlcUpgradeShopItems[index].Configure((CultUpgradeData.TYPE) (104 + index));
    this.Init();
    for (int index = 0; index < this._upgradeShopItems.Length; ++index)
      this._upgradeShopItems[index].OnCultUpgradeItemSelected += new Action<CultUpgradeData.TYPE>(this.BorderItemSelected);
    for (int index = 0; index < this._dlcUpgradeShopItems.Length; ++index)
      this._dlcUpgradeShopItems[index].OnCultUpgradeItemSelected += new Action<CultUpgradeData.TYPE>(this.BorderItemSelected);
    if (CultUpgradeData.IsUpgradeMaxed())
    {
      this._cultUpgradeItemLevel.text = this.templeLevel.ToNumeral();
      this._cultUpgradeItem.selectionAlwaysInvalid = true;
    }
    else
    {
      this._cultUpgradeItemLevel.text = Mathf.Max(1, DataManager.Instance.TempleLevel + 1).ToNumeral();
      this._cultUpgradeItem.OnCultUpgradeItemSelected += new Action<CultUpgradeData.TYPE>(this.CultUpgradeMainPurchaseSelected);
      Debug.Log((object) ("Ritual alert present? " + this._ritualItemAlert.name));
      this._ritualItemAlert.SetActive(!CultUpgradeData.IsUpgradeMaxed() && CultUpgradeData.UserCanAffordUpgrade((CultUpgradeData.TYPE) (1 + Mathf.Max(0, DataManager.Instance.TempleLevel) + 1)));
      Debug.Log((object) ("Ritual alert active? " + this._ritualItemAlert.activeInHierarchy.ToString()));
    }
    if (!CultUpgradeData.IsUpgradeMaxed())
      return;
    AchievementsWrapper.UnlockAchievement(Achievements.Instance.Lookup("FULLY_UPGRADE_RANKING"));
  }

  public override void OnShowStarted()
  {
    base.OnShowStarted();
    if (this.Revealing == CultUpgradeData.TYPE.None)
      return;
    this._cultUpgradeInfoCardController.gameObject.SetActive(false);
  }

  public override void OnShowCompleted()
  {
    base.OnShowCompleted();
    if (this.Revealing == CultUpgradeData.TYPE.None)
      return;
    this.StartCoroutine((IEnumerator) this.RevealSequenceIE());
  }

  public IEnumerator RevealSequenceIE()
  {
    UICultUpgradesMenuController upgradesMenuController = this;
    upgradesMenuController.SetActiveStateForMenu(false);
    CultUpgradeItem item = (CultUpgradeItem) null;
    foreach (CultUpgradeItem dlcUpgradeShopItem in upgradesMenuController._dlcUpgradeShopItems)
    {
      if (dlcUpgradeShopItem.CultUpgradeType == upgradesMenuController.Revealing)
        item = dlcUpgradeShopItem;
    }
    upgradesMenuController._controlPrompts.gameObject.SetActive(false);
    yield return (object) upgradesMenuController.StartCoroutine((IEnumerator) upgradesMenuController.scrollRect.DoScrollTo(item.transform as RectTransform));
    yield return (object) new WaitForSecondsRealtime(1f);
    yield return (object) upgradesMenuController.StartCoroutine((IEnumerator) item.DoUnlock(upgradesMenuController.Revealing, 0.0f));
    if (upgradesMenuController.Revealing == CultUpgradeData.TYPE.Border5)
      DataManager.Instance.TempleUnlockedBorder5 = true;
    else if (upgradesMenuController.Revealing == CultUpgradeData.TYPE.Border6)
      DataManager.Instance.TempleUnlockedBorder6 = true;
    upgradesMenuController._cultUpgradeInfoCardController.gameObject.SetActive(true);
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) item.Button);
    upgradesMenuController._cultUpgradeInfoCardController.CurrentCard?.Show(upgradesMenuController.Revealing, false);
    yield return (object) new WaitForSecondsRealtime(3f);
    upgradesMenuController.preventCloseUntilRevealComplete = false;
    upgradesMenuController.Hide();
  }

  public void Init()
  {
    for (int index = 0; index < this._upgradeShopItems.Length; ++index)
      this._upgradeShopItems[index].Init((CultUpgradeData.TYPE) (100 + index), true);
    for (int index = 0; index < this._dlcUpgradeShopItems.Length; ++index)
      this._dlcUpgradeShopItems[index].Init((CultUpgradeData.TYPE) (104 + index));
    this.RefreshHiddenItems();
  }

  public void RefreshHiddenItems()
  {
    for (int index = 0; index < this._hiddenBeforeFirstCultUpgrade.Length; ++index)
      this._hiddenBeforeFirstCultUpgrade[index].gameObject.SetActive(this.templeLevel >= 1);
    for (int index = 0; index < this._hiddenBeforeFirstBorderUpgrade.Length; ++index)
      this._hiddenBeforeFirstBorderUpgrade[index].gameObject.SetActive(this.templeLevel >= 3);
    Debug.Log((object) ("Refreshed hidden items? " + this.templeLevel.ToString()));
  }

  public void CultUpgradeMainPurchaseSelected(CultUpgradeData.TYPE type)
  {
    this.StartCoroutine((IEnumerator) this.FocusCard(this._cultUpgradeInfoCardController.CurrentCard.RectTransform, this._cultUpgradeInfoCardController.CurrentCard._redOutline, (System.Action) (() =>
    {
      DataManager.Instance.TempleLevel = (int) type;
      this._cultUpgradeItem.StartCoroutine((IEnumerator) this._cultUpgradeItem.DoUnlock((CultUpgradeData.TYPE) (1 + Mathf.Max(0, DataManager.Instance.TempleLevel))));
      UIManager.PlayAudio("event:/ui/level_node_beat_level");
      foreach (StructuresData.ItemCost itemCost in CultUpgradeData.GetCost(type))
        Inventory.ChangeItemQuantity(itemCost.CostItem, -itemCost.CostValue);
      this.RefreshHiddenItems();
      switch (DataManager.Instance.TempleLevel)
      {
        case 1:
          this._upgradeShopItems[0].StartCoroutine((IEnumerator) this._upgradeShopItems[0].DoUnlock(this._upgradeShopItems[0].CultUpgradeType));
          DataManager.Instance.TempleBorder = 100;
          break;
        case 3:
          this._upgradeShopItems[1].StartCoroutine((IEnumerator) this._upgradeShopItems[1].DoUnlock(this._upgradeShopItems[1].CultUpgradeType));
          break;
        case 6:
          this._upgradeShopItems[2].StartCoroutine((IEnumerator) this._upgradeShopItems[2].DoUnlock(this._upgradeShopItems[2].CultUpgradeType));
          break;
        case 9:
          this._upgradeShopItems[3].StartCoroutine((IEnumerator) this._upgradeShopItems[3].DoUnlock(this._upgradeShopItems[3].CultUpgradeType));
          break;
      }
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.SpendSinInTemple);
      Debug.Log((object) "CULT UPGRADE INVOKED");
      System.Action onCultUpgraded = UICultUpgradesMenuController.OnCultUpgraded;
      if (onCultUpgraded != null)
        onCultUpgraded();
      this.StartCoroutine((IEnumerator) this.HideAndRevealInfoCard());
      if (!CultUpgradeData.IsUpgradeMaxed())
        return;
      AchievementsWrapper.UnlockAchievement(Achievements.Instance.Lookup("FULLY_UPGRADE_RANKING"));
    })));
    this._cultUpgradeItemLevel.transform.DOKill();
    this._cultUpgradeItemLevel.transform.DOShakePosition(1f, new Vector3(0.0f, -2f, 0.0f)).SetUpdate<Tweener>(true);
    if (CultUpgradeData.IsUpgradeMaxed())
      this._cultUpgradeItemLevel.text = DataManager.Instance.TempleLevel.ToNumeral();
    else
      this._cultUpgradeItemLevel.text = (DataManager.Instance.TempleLevel + 1).ToNumeral();
    Debug.Log((object) ("Current temple level is " + DataManager.Instance.TempleLevel.ToString()));
  }

  public IEnumerator HideAndRevealInfoCard()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    UICultUpgradesMenuController upgradesMenuController = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      upgradesMenuController._cultUpgradeInfoCardController.CurrentCard.Configure((CultUpgradeData.TYPE) (1 + Mathf.Max(0, DataManager.Instance.TempleLevel)));
      upgradesMenuController._cultUpgradeInfoCardController.CurrentCard.Show();
      upgradesMenuController._canvasGroup.alpha = 1f;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    upgradesMenuController._canvasGroup.alpha = 0.99f;
    upgradesMenuController._cultUpgradeInfoCardController.CurrentCard.Hide();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void BorderItemSelected(CultUpgradeData.TYPE type)
  {
    DataManager.Instance.TempleBorder = (int) type;
    System.Action onCultUpgraded = UICultUpgradesMenuController.OnCultUpgraded;
    if (onCultUpgraded != null)
      onCultUpgraded();
    this._cultUpgradeItem.StartCoroutine((IEnumerator) this._cultUpgradeItem.DoUnlock((CultUpgradeData.TYPE) (1 + Mathf.Max(0, DataManager.Instance.TempleLevel)), 0.01f));
    this._cultUpgradeInfoCardController.CurrentCard.Show(true);
    this.Init();
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable || this.preventCloseUntilRevealComplete)
      return;
    this._didCancel = true;
    this.Hide();
  }

  public override void OnHideStarted()
  {
    base.OnHideStarted();
    AudioManager.Instance.PlayOneShot("event:/ui/close_menu");
  }

  public override void OnHideCompleted()
  {
    if (this._didCancel)
    {
      System.Action onCancel = this.OnCancel;
      if (onCancel != null)
        onCancel();
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public IEnumerator FocusCard(RectTransform cardTransform, GameObject redOutline, System.Action andThen)
  {
    UICultUpgradesMenuController upgradesMenuController = this;
    redOutline.gameObject.SetActive(false);
    RectTransform cardContainer = cardTransform.parent as RectTransform;
    upgradesMenuController._cultUpgradeInfoCardController.enabled = false;
    upgradesMenuController.OverrideDefaultOnce(MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable);
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    upgradesMenuController.SetActiveStateForMenu(false);
    upgradesMenuController._controlPrompts.HideAcceptButton();
    upgradesMenuController._uiHoldInteraction.Reset();
    cardTransform.SetParent((Transform) upgradesMenuController._rootContainer, true);
    cardTransform.DOLocalMove(Vector3.zero, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    upgradesMenuController._animator.Play(upgradesMenuController.kSelectedAnimationState);
    yield return (object) new WaitForSecondsRealtime(1f);
    bool cancel = false;
    yield return (object) upgradesMenuController._uiHoldInteraction.DoHoldInteraction((Action<float>) (progress =>
    {
      float num = (float) (1.0 + 0.25 * (double) progress);
      cardTransform.localScale = new Vector3(num, num, num);
      cardTransform.localPosition = (Vector3) (UnityEngine.Random.insideUnitCircle * progress * this._uiHoldInteraction.HoldTime * 2f);
      MMVibrate.RumbleContinuous(progress * 0.2f, progress * 0.2f, MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer);
      if (redOutline.gameObject.activeSelf != (double) progress > 0.0)
        redOutline.gameObject.SetActive((double) progress > 0.0);
      redOutline.transform.localScale = Vector3.Lerp(new Vector3(1f, 1f, 1f), new Vector3(1.2f, 1.2f, 1.2f), progress);
    }), (System.Action) (() =>
    {
      cancel = true;
      MMVibrate.StopRumble();
    }));
    MMVibrate.StopRumble();
    if (cancel)
    {
      cardTransform.DOLocalMove((Vector3) (Vector2) upgradesMenuController._rootContainer.InverseTransformPoint(cardContainer.TransformPoint(Vector3.zero)), 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => cardTransform.SetParent((Transform) cardContainer, true)));
      upgradesMenuController._animator.Play(upgradesMenuController.kCancelSelectionAnimationState);
      yield return (object) new WaitForSecondsRealtime(1f);
      upgradesMenuController._controlPrompts.ShowAcceptButton();
      upgradesMenuController.SetActiveStateForMenu(true);
      upgradesMenuController._cultUpgradeInfoCardController.enabled = true;
    }
    else
    {
      upgradesMenuController._controlPrompts.HideCancelButton();
      cardTransform.DOScale(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      yield return (object) upgradesMenuController._animator.YieldForAnimation(upgradesMenuController.kConfirmedSelectionAnimationState);
      System.Action action = andThen;
      if (action != null)
        action();
      upgradesMenuController.Hide(true);
    }
  }
}
