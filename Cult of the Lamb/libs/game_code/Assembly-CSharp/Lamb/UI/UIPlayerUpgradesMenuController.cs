// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIPlayerUpgradesMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI.Rituals;
using src.UI.InfoCards;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using Unify;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UIPlayerUpgradesMenuController : UIMenuBase
{
  public string kSelectedAnimationState = "Selected";
  public string kCancelSelectionAnimationState = "Cancelled";
  public string kConfirmedSelectionAnimationState = "Confirmed";
  public static System.Action OnDoctrineUnlockSelected;
  public static System.Action OnCrystalDoctrineUnlockSelected;
  [Header("Commandments")]
  [SerializeField]
  public RitualInfoCardController _ritualInfoCardController;
  [SerializeField]
  public RitualItem _ritualItem;
  [SerializeField]
  public GameObject _ritualItemAlert;
  [SerializeField]
  public RitualItem _crystalDoctrineItem;
  [SerializeField]
  public GameObject _crystalDoctrineItemAlert;
  [Header("Abilities")]
  [SerializeField]
  public TextMeshProUGUI _crownAbilityCount;
  [SerializeField]
  public CrownAbilityInfoCardController _crownAbilityInfoCardController;
  [SerializeField]
  public CrownAbilityItemBuyable[] _upgradeShopItems;
  [SerializeField]
  public CrownAbilityItemBuyable[] _upgradeDLCShopItems;
  [SerializeField]
  public GameObject _crownHeader;
  [SerializeField]
  public GameObject _crownContainer;
  [SerializeField]
  public GameObject _crownDLCContainer;
  [Header("Fleeces")]
  [SerializeField]
  public TextMeshProUGUI _fleeceCount;
  [SerializeField]
  public FleeceInfoCardController _fleeceInfoCardController;
  [SerializeField]
  public FleeceItemBuyable[] _fleeceItems;
  [SerializeField]
  public GameObject _fleeceHeader;
  [SerializeField]
  public GameObject _fleeceContainer;
  [SerializeField]
  public GameObject _fleeceDLCContainer;
  [SerializeField]
  public GameObject _fleeceDLCHeader;
  [Header("Misc")]
  [SerializeField]
  public RectTransform _rootContainer;
  [SerializeField]
  public UIHoldInteraction _uiHoldInteraction;
  [SerializeField]
  public UIMenuControlPrompts _controlPrompts;
  [SerializeField]
  public MMScrollRect _scrollRect;
  public GameObject _customiseFleeceButton;
  public bool _didCancel;
  public bool _showingCrystal;
  public bool _showingFleeces;
  public PlayerFleeceManager.FleeceType[] _showFleeces;
  public bool _sequenceRequiresConfirmation = true;
  public UpgradeSystem.Type[] _crownUpgrades = new UpgradeSystem.Type[4]
  {
    UpgradeSystem.Type.Ability_Resurrection,
    UpgradeSystem.Type.Ability_Eat,
    UpgradeSystem.Type.Ability_TeleportHome,
    UpgradeSystem.Type.Ability_BlackHeart
  };
  public UpgradeSystem.Type[] _crownDLCUpgrades = new UpgradeSystem.Type[1]
  {
    UpgradeSystem.Type.Ability_WinterChoice
  };
  public bool startedCoroutine;

  public FleeceItemBuyable GetFleeceItem(int index)
  {
    if (index <= this._fleeceItems.Length && index >= 0)
      return this._fleeceItems[index];
    UnityEngine.Debug.Log((object) ("Failed to get Fleece Item: " + index.ToString()));
    return (FleeceItemBuyable) null;
  }

  public int GetFleeceIndex(int fleece)
  {
    for (int fleeceIndex = 0; fleeceIndex < this._fleeceItems.Length; ++fleeceIndex)
    {
      if (this._fleeceItems[fleeceIndex].Fleece == fleece)
        return fleeceIndex;
    }
    return -1;
  }

  public bool FleeceActive(int index) => this._fleeceItems[index].gameObject.activeSelf;

  public List<int> GetFleeces()
  {
    List<int> fleeces = new List<int>();
    foreach (FleeceItemBuyable fleeceItem in this._fleeceItems)
      fleeces.Add(-1);
    for (int index = 0; index < this._fleeceItems.Length; ++index)
      fleeces[index] = this._fleeceItems[index].Fleece;
    return fleeces;
  }

  public void Start()
  {
    if (!DataManager.Instance.UnlockedUpgrades.Contains(UpgradeSystem.Type.Ritual_HeartsOfTheFaithful1))
      DataManager.Instance.UnlockedUpgrades.Add(UpgradeSystem.Type.Ritual_HeartsOfTheFaithful1);
    if (DataManager.Instance.DoctrineCurrentCount >= 3)
    {
      DataManager.Instance.DoctrineCurrentCount = 0;
      ++DataManager.Instance.CompletedDoctrineStones;
    }
    this._ritualItem.Configure(UpgradeSystem.PrimaryRitual1);
    if (!DoctrineUpgradeSystem.TrySermonsStillAvailable())
      this._ritualItem.SetMaxed();
    this._ritualItem.OnRitualItemSelected += new Action<UpgradeSystem.Type>(this.RitualItemSelected);
    this._crownHeader.gameObject.SetActive(!DataManager.Instance.SurvivalModeActive);
    this._crownContainer.gameObject.SetActive(!DataManager.Instance.SurvivalModeActive);
    this._customiseFleeceButton.gameObject.SetActive(false);
    MonoSingleton<UINavigatorNew>.Instance.OnSelectionChanged += new Action<Selectable, Selectable>(this.OnSelectionChanged);
    if ((double) UpgradeSystem.GetCoolDownNormalised(UpgradeSystem.PrimaryRitual1) <= 0.0 && Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.DOCTRINE_STONE) >= 1 && DoctrineUpgradeSystem.TrySermonsStillAvailable())
      this._ritualItemAlert.SetActive(true);
    else
      this._ritualItemAlert.SetActive(false);
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ritual_CrystalDoctrine))
    {
      this._crystalDoctrineItem.Configure(UpgradeSystem.Type.Ritual_CrystalDoctrine);
      this._crystalDoctrineItem.OnRitualItemSelected += new Action<UpgradeSystem.Type>(this.CrystalDoctrineRitualSelected);
      if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.CRYSTAL_DOCTRINE_STONE) > 0 && DoctrineUpgradeSystem.GetAllRemainingDoctrines().Count > 0 && (double) UpgradeSystem.GetCoolDownNormalised(UpgradeSystem.Type.Ritual_CrystalDoctrine) <= 0.0)
        this._crystalDoctrineItemAlert.SetActive(true);
      else
        this._crystalDoctrineItemAlert.SetActive(false);
    }
    else
      this._crystalDoctrineItem.gameObject.SetActive(false);
    int num1 = 0;
    for (int index = 0; index < this._crownUpgrades.Length; ++index)
    {
      num1 += UpgradeSystem.GetUnlocked(this._crownUpgrades[index]).ToInt();
      this._upgradeShopItems[index].Configure(this._crownUpgrades[index]);
      this._upgradeShopItems[index].OnUpgradeChosen = new Action<CrownAbilityItemBuyable>(this.UpgradeItemSelected);
    }
    this._crownDLCContainer.gameObject.SetActive(false);
    if (DataManager.Instance.BeatenYngya)
    {
      this._crownDLCContainer.gameObject.SetActive(true);
      for (int index = 0; index < this._crownDLCUpgrades.Length; ++index)
      {
        num1 += UpgradeSystem.GetUnlocked(this._crownDLCUpgrades[index]).ToInt();
        this._upgradeDLCShopItems[index].Configure(this._crownDLCUpgrades[index]);
        this._upgradeDLCShopItems[index].OnUpgradeChosen = new Action<CrownAbilityItemBuyable>(this.UpgradeItemSelected);
      }
    }
    this._crownAbilityCount.text = LocalizeIntegration.FormatCurrentMax(num1.ToString(), this._crownUpgrades.Length.ToString()) ?? "";
    int num2 = 0;
    int num3 = 0;
    foreach (FleeceItemBuyable fleeceItem in this._fleeceItems)
    {
      if (PlayerFleeceManager.IS_DLC.Contains((PlayerFleeceManager.FleeceType) fleeceItem.Fleece))
        fleeceItem.transform.SetParent(this._fleeceDLCContainer.transform);
      else
        fleeceItem.transform.SetParent(this._fleeceContainer.transform);
      if (this._fleeceDLCContainer.transform.childCount > 0)
        this._fleeceDLCHeader.gameObject.SetActive(true);
      else
        this._fleeceDLCHeader.gameObject.SetActive(false);
      fleeceItem.Configure(fleeceItem.ForcedFleeceIndex == -1 ? this._fleeceItems.IndexOf<FleeceItemBuyable>(fleeceItem) : fleeceItem.ForcedFleeceIndex);
      fleeceItem.OnFleeceChosen = new Action<int>(this.FleeceItemSelected);
      if (fleeceItem.gameObject.activeSelf)
      {
        ++num3;
        if (fleeceItem.Unlocked)
          ++num2;
      }
    }
    this._fleeceCount.text = LocalizeIntegration.FormatCurrentMax(num2.ToString(), num3.ToString()) ?? "";
    this.TryUnlockFleeceAchievements();
    if (!((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer != (UnityEngine.Object) null))
      return;
    this._fleeceHeader.gameObject.SetActive(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer.isLamb);
    this._fleeceContainer.gameObject.SetActive(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer.isLamb);
  }

  public void OnSelectionChanged(Selectable arg1, Selectable arg2)
  {
    if (!((UnityEngine.Object) arg1.gameObject.GetComponent<FleeceItemBuyable>() == (UnityEngine.Object) null) || !((UnityEngine.Object) this._customiseFleeceButton != (UnityEngine.Object) null))
      return;
    this._customiseFleeceButton.SetActive(false);
  }

  public void ShowCrystalUnlock()
  {
    this._crownAbilityInfoCardController.enabled = false;
    this._fleeceInfoCardController.enabled = false;
    this._sequenceRequiresConfirmation = true;
    this.OverrideDefault((Selectable) null);
    this._showingCrystal = true;
    this.Show();
    this._controlPrompts.HideAcceptButton();
    this._controlPrompts.HideCancelButton();
    this._customiseFleeceButton.gameObject.SetActive(false);
  }

  public void ShowNewFleecesUnlocked(
    PlayerFleeceManager.FleeceType[] fleeceTypes,
    bool requiresConfirmation = false)
  {
    this._showingFleeces = true;
    if (!requiresConfirmation)
      this.OnShownCompleted = this.OnShownCompleted + (System.Action) (() => this._showingFleeces = false);
    this._showFleeces = fleeceTypes;
    this._sequenceRequiresConfirmation = requiresConfirmation;
    this._crownAbilityInfoCardController.enabled = false;
    this._fleeceInfoCardController.enabled = false;
    this.OverrideDefault((Selectable) null);
    this.Show();
    this._controlPrompts.HideAcceptButton();
    this._controlPrompts.HideCancelButton();
    this._customiseFleeceButton.gameObject.SetActive(false);
  }

  public override IEnumerator DoShowAnimation()
  {
    UIPlayerUpgradesMenuController upgradesMenuController = this;
    if (upgradesMenuController._showingCrystal || upgradesMenuController._showingFleeces)
    {
      upgradesMenuController.SetActiveStateForMenu(false);
      foreach (CrownAbilityItemBuyable upgradeShopItem in upgradesMenuController._upgradeShopItems)
        upgradeShopItem.ForceIncognitoState();
      if (upgradesMenuController._showingFleeces)
      {
        foreach (FleeceItemBuyable fleeceItem in upgradesMenuController._fleeceItems)
        {
          if (upgradesMenuController._showFleeces.Contains<PlayerFleeceManager.FleeceType>((PlayerFleeceManager.FleeceType) fleeceItem.Fleece))
            fleeceItem.PrepareForUnlock();
          else
            fleeceItem.ForceIncognitoState();
        }
      }
      upgradesMenuController._ritualItemAlert.SetActive(false);
      upgradesMenuController._ritualItem.ForceIncognitoState();
      if (upgradesMenuController._showingCrystal)
      {
        upgradesMenuController._crystalDoctrineItem.gameObject.SetActive(true);
        upgradesMenuController._crystalDoctrineItem.ForceLockedState();
        upgradesMenuController._crystalDoctrineItemAlert.SetActive(false);
      }
      else if (upgradesMenuController._showingFleeces)
      {
        upgradesMenuController._crystalDoctrineItemAlert.SetActive(false);
        if (DataManager.Instance.OnboardedCrystalDoctrine)
          upgradesMenuController._crystalDoctrineItem.ForceIncognitoState();
      }
      AudioManager.Instance.PlayOneShot("event:/sermon/scroll_sermon_menu", upgradesMenuController.gameObject);
      yield return (object) upgradesMenuController.\u003C\u003En__0();
      yield return (object) new WaitForSecondsRealtime(0.1f);
      if (upgradesMenuController._showingCrystal)
      {
        yield return (object) upgradesMenuController._crystalDoctrineItem.DoUnlock();
        upgradesMenuController._crystalDoctrineItemAlert.SetActive(true);
        Vector3 one = Vector3.one;
        upgradesMenuController._crystalDoctrineItemAlert.transform.localScale = Vector3.zero;
        upgradesMenuController._crystalDoctrineItemAlert.transform.DOScale(one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBounce).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
        upgradesMenuController._crystalDoctrineItemAlert.gameObject.SetActive(true);
        yield return (object) new WaitForSecondsRealtime(0.5f);
      }
      else if (upgradesMenuController._showingFleeces)
      {
        AudioManager.Instance.PlayOneShot("event:/sermon/scroll_sermon_menu", upgradesMenuController.gameObject);
        yield return (object) upgradesMenuController._scrollRect.ScrollToBottom();
        FleeceItemBuyable[] fleeceItemBuyableArray = upgradesMenuController._fleeceItems;
        for (int index = 0; index < fleeceItemBuyableArray.Length; ++index)
        {
          FleeceItemBuyable fleeceItemBuyable = fleeceItemBuyableArray[index];
          if (upgradesMenuController._showFleeces.Contains<PlayerFleeceManager.FleeceType>((PlayerFleeceManager.FleeceType) fleeceItemBuyable.Fleece))
            yield return (object) fleeceItemBuyable.DoUnlock();
        }
        fleeceItemBuyableArray = (FleeceItemBuyable[]) null;
      }
      yield return (object) new WaitForSecondsRealtime(0.1f);
      if (upgradesMenuController._showingCrystal)
      {
        upgradesMenuController._ritualInfoCardController.ShowCardWithParam(UpgradeSystem.Type.Ritual_CrystalDoctrine);
        upgradesMenuController._controlPrompts.ShowAcceptButton();
        while (!InputManager.UI.GetAcceptButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
          yield return (object) null;
        upgradesMenuController.Hide();
      }
      else if (upgradesMenuController._showingFleeces)
      {
        yield return (object) new WaitForSecondsRealtime(0.2f);
        if (upgradesMenuController._sequenceRequiresConfirmation)
        {
          upgradesMenuController._fleeceInfoCardController.ShowCardWithParam((int) upgradesMenuController._showFleeces[0]);
          upgradesMenuController._controlPrompts.ShowAcceptButton();
          upgradesMenuController._customiseFleeceButton.SetActive(false);
          while (!InputManager.UI.GetAcceptButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
            yield return (object) null;
          upgradesMenuController.Hide();
        }
        else
        {
          yield return (object) upgradesMenuController._scrollRect.ScrollToTop();
          upgradesMenuController._ritualItem.AnimateIncognitoOut();
          upgradesMenuController._crystalDoctrineItem.AnimateIncognitoOut();
          foreach (CrownAbilityItemBuyable upgradeShopItem in upgradesMenuController._upgradeShopItems)
            upgradeShopItem.AnimateIncognitoOut();
          foreach (FleeceItemBuyable fleeceItem in upgradesMenuController._fleeceItems)
            fleeceItem.AnimateIncognitoOut();
          yield return (object) new WaitForSecondsRealtime(0.25f);
          upgradesMenuController._crownAbilityInfoCardController.enabled = true;
          upgradesMenuController._fleeceInfoCardController.enabled = true;
          upgradesMenuController._controlPrompts.ShowAcceptButton();
          MonoSingleton<UINavigatorNew>.Instance.Clear();
          upgradesMenuController.SetActiveStateForMenu(true);
        }
      }
    }
    else
      yield return (object) upgradesMenuController.\u003C\u003En__0();
  }

  public IEnumerator FocusCard(RectTransform cardTransform, GameObject redOutline, System.Action andThen)
  {
    UIPlayerUpgradesMenuController upgradesMenuController = this;
    redOutline.gameObject.SetActive(false);
    RectTransform cardContainer = cardTransform.parent as RectTransform;
    upgradesMenuController._ritualInfoCardController.enabled = false;
    upgradesMenuController._fleeceInfoCardController.enabled = false;
    upgradesMenuController._crownAbilityInfoCardController.enabled = false;
    upgradesMenuController.OverrideDefaultOnce(MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable);
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    upgradesMenuController.SetActiveStateForMenu(false);
    upgradesMenuController._controlPrompts.HideAcceptButton();
    upgradesMenuController._uiHoldInteraction.gameObject.SetActive(true);
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
      upgradesMenuController._ritualInfoCardController.enabled = true;
      upgradesMenuController._fleeceInfoCardController.enabled = true;
      upgradesMenuController._crownAbilityInfoCardController.enabled = true;
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

  public void RitualItemSelected(UpgradeSystem.Type ritual)
  {
    System.Action doctrineUnlockSelected = UIPlayerUpgradesMenuController.OnDoctrineUnlockSelected;
    if (doctrineUnlockSelected != null)
      doctrineUnlockSelected();
    this.Hide();
  }

  public void CrystalDoctrineRitualSelected(UpgradeSystem.Type ritual)
  {
    System.Action doctrineUnlockSelected = UIPlayerUpgradesMenuController.OnCrystalDoctrineUnlockSelected;
    if (doctrineUnlockSelected != null)
      doctrineUnlockSelected();
    this.Hide();
  }

  public IEnumerator FocusCardFleece(
    RectTransform cardTransform,
    GameObject redOutline,
    System.Action andThen)
  {
    UIPlayerUpgradesMenuController upgradesMenuController = this;
    AudioManager.Instance.PlayOneShot("event:/ui/confirm_selection");
    upgradesMenuController._customiseFleeceButton.gameObject.SetActive(false);
    redOutline.gameObject.SetActive(false);
    RectTransform cardContainer = cardTransform.parent as RectTransform;
    upgradesMenuController._ritualInfoCardController.enabled = false;
    upgradesMenuController._fleeceInfoCardController.enabled = false;
    upgradesMenuController._crownAbilityInfoCardController.enabled = false;
    upgradesMenuController.OverrideDefaultOnce(MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable);
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    upgradesMenuController.SetActiveStateForMenu(false);
    upgradesMenuController._controlPrompts.HideAcceptButton();
    upgradesMenuController._uiHoldInteraction.gameObject.SetActive(false);
    cardTransform.SetParent((Transform) upgradesMenuController._rootContainer, true);
    cardTransform.DOLocalMove(Vector3.zero, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    upgradesMenuController._animator.Play(upgradesMenuController.kSelectedAnimationState);
    yield return (object) new WaitForSecondsRealtime(1f);
    cardTransform.GetComponentInChildren<FleeceInfoCard>().ShowPrompts();
    bool cancel = false;
    while (!cancel)
    {
      if (InputManager.UI.GetCancelButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
        cancel = true;
      MMVibrate.StopRumble();
      yield return (object) new WaitForEndOfFrame();
    }
    if (cancel)
    {
      AudioManager.Instance.PlayOneShot("event:/ui/close_menu");
      cardTransform.GetComponentInChildren<FleeceInfoCard>().HidePrompts();
      cardTransform.DOLocalMove((Vector3) (Vector2) upgradesMenuController._rootContainer.InverseTransformPoint(cardContainer.TransformPoint(Vector3.zero)), 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => cardTransform.SetParent((Transform) cardContainer, true)));
      upgradesMenuController._animator.Play(upgradesMenuController.kCancelSelectionAnimationState);
      yield return (object) new WaitForSecondsRealtime(1f);
      upgradesMenuController._controlPrompts.ShowAcceptButton();
      upgradesMenuController._customiseFleeceButton.gameObject.SetActive(true);
      upgradesMenuController.SetActiveStateForMenu(true);
      upgradesMenuController._ritualInfoCardController.enabled = true;
      upgradesMenuController._fleeceInfoCardController.enabled = true;
      upgradesMenuController._crownAbilityInfoCardController.enabled = true;
      upgradesMenuController.startedCoroutine = false;
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

  public void FleeceItemSelected(int fleece)
  {
    if (DataManager.Instance.UnlockedFleeces.Contains(fleece))
    {
      if (DataManager.Instance.PlayerFleece == fleece)
        return;
      this.EquipFleece(fleece);
      this.UpdateFleeces();
    }
    else
    {
      int fleeceIndex = fleece;
      for (int index = 0; index < this._fleeceItems.Length; ++index)
      {
        if (this._fleeceItems[index].Fleece == fleece)
        {
          fleeceIndex = index;
          break;
        }
      }
      if (!this._fleeceItems[fleeceIndex].Cost.CanAfford())
        return;
      this.StartCoroutine((IEnumerator) this.FocusCard(this._fleeceInfoCardController.CurrentCard.RectTransform, this._fleeceInfoCardController.CurrentCard._redOutline, (System.Action) (() =>
      {
        int playerFleece = DataManager.Instance.PlayerFleece;
        Inventory.ChangeItemQuantity(this._fleeceItems[fleeceIndex].Cost.CostItem, -this._fleeceItems[fleeceIndex].Cost.CostValue);
        if (!DataManager.Instance.UnlockedFleeces.Contains(fleece))
          DataManager.Instance.UnlockedFleeces.Add(fleece);
        this.EquipFleece(fleece);
        this.UpdateFleeces();
        this.TryUnlockFleeceAchievements();
        this.Hide(true);
        Interaction_TempleAltar.Instance.GetFleeceRoutine(playerFleece, fleece);
      })));
    }
  }

  public void EquipFleece(int fleece)
  {
    FleeceItemBuyable fleeceItemBuyable = (FleeceItemBuyable) null;
    foreach (FleeceItemBuyable fleeceItem in this._fleeceItems)
    {
      if (fleeceItem.ForcedFleeceIndex != -1 && fleeceItem.ForcedFleeceIndex == fleece)
        fleeceItemBuyable = fleeceItem;
    }
    if ((UnityEngine.Object) fleeceItemBuyable != (UnityEngine.Object) null)
      fleeceItemBuyable.Bump();
    else
      this._fleeceItems[fleece].Bump();
    DataManager.Instance.PlayerFleece = fleece;
    bool flag = false;
    ObjectiveManager.CompleteShowFleeceObjective();
    foreach (Vector2 customisedFleeceOption in DataManager.Instance.CustomisedFleeceOptions)
    {
      if ((double) customisedFleeceOption.x == (double) fleece)
      {
        DataManager.Instance.PlayerVisualFleece = (int) customisedFleeceOption.y;
        flag = true;
      }
    }
    if (!flag)
      DataManager.Instance.PlayerVisualFleece = DataManager.Instance.PlayerFleece;
    if (!DataManager.Instance.UnlockedFleeces.Contains(fleece))
      return;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if (player.isLamb)
      {
        if (fleece == 1003)
        {
          player.SetSkin();
        }
        else
        {
          player.IsGoat = false;
          player.simpleSpineAnimator?.SetSkin("Lamb_" + DataManager.Instance.PlayerVisualFleece.ToString());
        }
      }
    }
  }

  public void UpdateFleeces()
  {
    foreach (FleeceItemBuyable fleeceItem in this._fleeceItems)
      fleeceItem.UpdateState();
  }

  public void TryUnlockFleeceAchievements()
  {
    AchievementsWrapper.UnlockAchievement(Achievements.Instance.Lookup("UNLOCK_TUNIC"));
    int count = DataManager.Instance.UnlockedFleeces.Count;
    foreach (int unlockedFleece in DataManager.Instance.UnlockedFleeces)
    {
      if (PlayerFleeceManager.NOT_INCLUDED_IN_ACHIEVEMENT.Contains((PlayerFleeceManager.FleeceType) unlockedFleece))
        --count;
    }
    if (count < 12)
      return;
    AchievementsWrapper.UnlockAchievement(Achievements.Instance.Lookup("UNLOCK_ALL_TUNICS"));
  }

  public void UpgradeItemSelected(CrownAbilityItemBuyable upgradeItem)
  {
    if (UpgradeSystem.GetUnlocked(upgradeItem.Type) || !upgradeItem.Cost.CanAfford())
      return;
    this.StartCoroutine((IEnumerator) this.FocusCard(this._crownAbilityInfoCardController.CurrentCard.RectTransform, this._crownAbilityInfoCardController.CurrentCard._redOutline, (System.Action) (() =>
    {
      Inventory.ChangeItemQuantity(upgradeItem.Cost.CostItem, -upgradeItem.Cost.CostValue);
      UpgradeSystem.UnlockAbility(upgradeItem.Type);
      this.UpdateUpgrades();
      upgradeItem.Bump();
      GameManager.GetInstance().CameraSetOffset(Vector3.zero);
      this.Hide(true);
      Interaction_TempleAltar.Instance.GetAbilityRoutine(upgradeItem.Type);
    })));
  }

  public bool CheckCanAfford(List<StructuresData.ItemCost> cost)
  {
    for (int index = 0; index < cost.Count; ++index)
    {
      if (Inventory.GetItemQuantity(cost[index].CostItem) < cost[index].CostValue)
        return false;
    }
    return true;
  }

  public void UpdateUpgrades()
  {
    foreach (CrownAbilityItemBuyable upgradeShopItem in this._upgradeShopItems)
      upgradeShopItem.UpdateState();
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this._didCancel = true;
    this.Hide();
  }

  public void Update()
  {
    if (this._showingFleeces || this._showingCrystal || DataManager.Instance.UnlockedFleeces.Count <= 1 || !InputManager.UI.GetEditBuildingsButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) || this.startedCoroutine || !this._customiseFleeceButton.activeInHierarchy)
      return;
    this.startedCoroutine = true;
    this.StartCoroutine((IEnumerator) this.FocusCardFleece(this._fleeceInfoCardController.CurrentCard.RectTransform, this._fleeceInfoCardController.CurrentCard._redOutline, (System.Action) null));
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

  [CompilerGenerated]
  public void \u003CShowNewFleecesUnlocked\u003Eb__43_0() => this._showingFleeces = false;

  [CompilerGenerated]
  [DebuggerHidden]
  public IEnumerator \u003C\u003En__0() => base.DoShowAnimation();
}
