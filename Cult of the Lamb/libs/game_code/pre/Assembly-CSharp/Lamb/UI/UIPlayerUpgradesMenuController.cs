// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIPlayerUpgradesMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI.Rituals;
using src.UI.InfoCards;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unify;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class UIPlayerUpgradesMenuController : UIMenuBase
{
  private string kSelectedAnimationState = "Selected";
  private string kCancelSelectionAnimationState = "Cancelled";
  private string kConfirmedSelectionAnimationState = "Confirmed";
  public static System.Action OnDoctrineUnlockSelected;
  [Header("Commandments")]
  [SerializeField]
  private RitualInfoCardController _ritualInfoCardController;
  [SerializeField]
  private RitualItem _ritualItem;
  [SerializeField]
  private GameObject _ritualItemAlert;
  [Header("Abilities")]
  [SerializeField]
  private TextMeshProUGUI _crownAbilityCount;
  [SerializeField]
  private CrownAbilityInfoCardController _crownAbilityInfoCardController;
  [SerializeField]
  private CrownAbilityItemBuyable[] _upgradeShopItems;
  [Header("Fleeces")]
  [SerializeField]
  private TextMeshProUGUI _fleeceCount;
  [SerializeField]
  private FleeceInfoCardController _fleeceInfoCardController;
  [SerializeField]
  private FleeceItemBuyable[] _fleeceItems;
  [Header("Misc")]
  [SerializeField]
  private RectTransform _rootContainer;
  [SerializeField]
  private UIHoldInteraction _uiHoldInteraction;
  [SerializeField]
  private UIMenuControlPrompts _controlPrompts;
  private bool _didCancel;
  private UpgradeSystem.Type[] _crownUpgrades = new UpgradeSystem.Type[4]
  {
    UpgradeSystem.Type.Ability_Resurrection,
    UpgradeSystem.Type.Ability_Eat,
    UpgradeSystem.Type.Ability_TeleportHome,
    UpgradeSystem.Type.Ability_BlackHeart
  };

  private void Start()
  {
    this._ritualItem.Configure(UpgradeSystem.PrimaryRitual1);
    Debug.Log((object) ("DoctrineUpgradeSystem.TrySermonsStillAvailable():" + DoctrineUpgradeSystem.TrySermonsStillAvailable().ToString()));
    if (!DoctrineUpgradeSystem.TrySermonsStillAvailable())
      this._ritualItem.SetMaxed();
    this._ritualItem.OnRitualItemSelected += new Action<UpgradeSystem.Type>(this.RitualItemSelected);
    this._ritualItemAlert.SetActive(Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.DOCTRINE_STONE) >= 1 && (double) UpgradeSystem.GetCoolDownNormalised(UpgradeSystem.PrimaryRitual1) <= 0.0 && DoctrineUpgradeSystem.TrySermonsStillAvailable());
    int num1 = 0;
    for (int index = 0; index < this._crownUpgrades.Length; ++index)
    {
      num1 += UpgradeSystem.GetUnlocked(this._crownUpgrades[index]).ToInt();
      this._upgradeShopItems[index].Configure(this._crownUpgrades[index]);
      this._upgradeShopItems[index].OnUpgradeChosen += new Action<CrownAbilityItemBuyable>(this.UpgradeItemSelected);
    }
    this._crownAbilityCount.text = $"{num1}/{this._crownUpgrades.Length}";
    int num2 = 0;
    foreach (FleeceItemBuyable fleeceItem in this._fleeceItems)
    {
      fleeceItem.Configure(this._fleeceItems.IndexOf<FleeceItemBuyable>(fleeceItem));
      fleeceItem.OnFleeceChosen += new Action<int>(this.FleeceItemSelected);
      num2 += DataManager.Instance.UnlockedFleeces.Contains(fleeceItem.Fleece).ToInt();
    }
    this._fleeceCount.text = $"{num2}/{this._fleeceItems.Length}";
  }

  private IEnumerator FocusCard(RectTransform cardTransform, GameObject redOutline, System.Action andThen)
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
    upgradesMenuController._uiHoldInteraction.Reset();
    cardTransform.SetParent((Transform) upgradesMenuController._rootContainer, true);
    cardTransform.DOLocalMove(Vector3.zero, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    upgradesMenuController._animator.Play(upgradesMenuController.kSelectedAnimationState);
    yield return (object) new WaitForSecondsRealtime(1f);
    bool cancel = false;
    yield return (object) upgradesMenuController._uiHoldInteraction.DoHoldInteraction(new Action<float>(OnHold), new System.Action(OnCancel));
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

    void OnHold(float progress)
    {
      float num = (float) (1.0 + 0.25 * (double) progress);
      cardTransform.localScale = new Vector3(num, num, num);
      cardTransform.localPosition = (Vector3) (UnityEngine.Random.insideUnitCircle * progress * this._uiHoldInteraction.HoldTime * 2f);
      MMVibrate.RumbleContinuous(progress * 0.2f, progress * 0.2f);
      if (redOutline.gameObject.activeSelf != (double) progress > 0.0)
        redOutline.gameObject.SetActive((double) progress > 0.0);
      redOutline.transform.localScale = Vector3.Lerp(new Vector3(1f, 1f, 1f), new Vector3(1.2f, 1.2f, 1.2f), progress);
    }

    void OnCancel()
    {
      cancel = true;
      MMVibrate.StopRumble();
    }
  }

  private void RitualItemSelected(UpgradeSystem.Type ritual)
  {
    System.Action doctrineUnlockSelected = UIPlayerUpgradesMenuController.OnDoctrineUnlockSelected;
    if (doctrineUnlockSelected != null)
      doctrineUnlockSelected();
    this.Hide();
  }

  private void FleeceItemSelected(int fleece)
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
      if (!this._fleeceItems[fleece].Cost.CanAfford())
        return;
      this.StartCoroutine((IEnumerator) this.FocusCard(this._fleeceInfoCardController.CurrentCard.RectTransform, this._fleeceInfoCardController.CurrentCard._redOutline, (System.Action) (() =>
      {
        int playerFleece = DataManager.Instance.PlayerFleece;
        Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.TALISMAN, -1);
        if (!DataManager.Instance.UnlockedFleeces.Contains(fleece))
          DataManager.Instance.UnlockedFleeces.Add(fleece);
        this.EquipFleece(fleece);
        this.UpdateFleeces();
        AchievementsWrapper.UnlockAchievement(Achievements.Instance.Lookup("UNLOCK_TUNIC"));
        if (DataManager.Instance.UnlockedFleeces.Count >= 6)
          AchievementsWrapper.UnlockAchievement(Achievements.Instance.Lookup("UNLOCK_ALL_TUNICS"));
        this.Hide(true);
        Interaction_TempleAltar.Instance.GetFleeceRoutine(playerFleece, fleece);
      })));
    }
  }

  private void EquipFleece(int fleece)
  {
    this._fleeceItems[fleece].Bump();
    DataManager.Instance.PlayerFleece = fleece;
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null))
      return;
    PlayerFarming.Instance.simpleSpineAnimator?.SetSkin("Lamb_" + DataManager.Instance.PlayerFleece.ToString());
  }

  private void UpdateFleeces()
  {
    foreach (FleeceItemBuyable fleeceItem in this._fleeceItems)
      fleeceItem.UpdateState();
  }

  private void UpgradeItemSelected(CrownAbilityItemBuyable upgradeItem)
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

  private bool CheckCanAfford(List<StructuresData.ItemCost> cost)
  {
    for (int index = 0; index < cost.Count; ++index)
    {
      if (Inventory.GetItemQuantity(cost[index].CostItem) < cost[index].CostValue)
        return false;
    }
    return true;
  }

  private void UpdateUpgrades()
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

  protected override void OnHideStarted()
  {
    base.OnHideStarted();
    AudioManager.Instance.PlayOneShot("event:/ui/close_menu");
  }

  protected override void OnHideCompleted()
  {
    if (this._didCancel)
    {
      System.Action onCancel = this.OnCancel;
      if (onCancel != null)
        onCancel();
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }
}
