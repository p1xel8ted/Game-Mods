// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIUpgradeTreeMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using src.Extensions;
using src.UINavigator;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UIUpgradeTreeMenuController : 
  UIUpgradeTreeMenuBase<UIUpgradeUnlockOverlayController>,
  ITreeMenuController
{
  [Header("0 Points Colours")]
  [SerializeField]
  public Color _zeroPointsTextColor = StaticColors.OffWhiteColor;
  [SerializeField]
  public Color _zeroPointsCrownImageColor = Color.red;
  [SerializeField]
  public Image _crownImage;
  [SerializeField]
  public Color _zeroPointsCrownOutlineColor = Color.white;
  [SerializeField]
  public Image _crownOutline;
  [SerializeField]
  public Color _zeroPointsDecorationImageColor = Color.white;
  [SerializeField]
  public Image _decorationImageLeft;
  [SerializeField]
  public Image _decorationImageRight;
  [SerializeField]
  public Color _zeroPointsDivineInspirationTextColor = Color.white;
  [SerializeField]
  public TextMeshProUGUI _divineInspirationText;
  [SerializeField]
  [ColorUsage(true, true)]
  public Color _zeroPointsGoopColor = Color.white;
  [SerializeField]
  [ColorUsage(true, true)]
  public Color _availablePointsGoopColor = Color.white;
  [SerializeField]
  public Image _pointsGoop;
  [SerializeField]
  public GameObject _controlPromptsObj;
  [SerializeField]
  public GameObject _pointsContainerObj;
  [SerializeField]
  public GameObject _dlcPrompt;
  [SerializeField]
  public GameObject _dlcLock;
  [SerializeField]
  public GameObject _dlcAlert;
  public bool changingTrees;
  public bool canSwapPages;
  [CompilerGenerated]
  public float \u003CDelayTimer\u003Ek__BackingField;
  public bool resetFail;

  public float DelayTimer
  {
    get => this.\u003CDelayTimer\u003Ek__BackingField;
    set => this.\u003CDelayTimer\u003Ek__BackingField = value;
  }

  public void Start()
  {
    this.canSwapPages = UpgradeSystem.GetUnlocked(UpgradeSystem.Type.WinterSystem);
    this._dlcLock.gameObject.SetActive(!this.canSwapPages);
    this._dlcAlert.gameObject.SetActive(false);
    if (!BuildingShrine.ShowingDLCTree && this._configuration.NumUnlockedUpgrades() < this._configuration.AllUpgrades.Count || this is UIDLCUpgradeTreeMenuController || !DataManager.Instance.MAJOR_DLC || !this.canSwapPages)
      return;
    this.ShowDLCTree(false, BuildingShrine.AnimateSnowDLCTree);
  }

  public bool isDLCTreeMenuController => this is UIDLCUpgradeTreeMenuController;

  public override void OnDestroy() => base.OnDestroy();

  public override void OnShowCompleted()
  {
    base.OnShowCompleted();
    if (this.revealType == UpgradeSystem.Type.WinterSystem)
      this.StartCoroutine((IEnumerator) this.DoDLCRevealAnimation());
    else if (BuildingShrine.ShowingDLCTree && !(this is UIDLCUpgradeTreeMenuController) && DataManager.Instance.MAJOR_DLC)
      this._canvasGroup.interactable = false;
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.WinterSystem))
      return;
    this._dlcPrompt.GetComponent<MMButton>().Interactable = false;
  }

  public void EnableControlPrompts()
  {
    this._controlPromptsObj.gameObject.SetActive(true);
    this._pointsContainerObj.gameObject.SetActive(true);
    if (!((UnityEngine.Object) this._pointsGoop != (UnityEngine.Object) null))
      return;
    this._pointsGoop.gameObject.SetActive(true);
  }

  public void DisableControlPrompts()
  {
    this._controlPromptsObj.gameObject.SetActive(false);
    this._pointsContainerObj.gameObject.SetActive(false);
    if (!((UnityEngine.Object) this._pointsGoop != (UnityEngine.Object) null))
      return;
    this._pointsGoop.gameObject.SetActive(false);
  }

  public override Selectable GetDefaultSelectable()
  {
    foreach (UpgradeTreeNode treeNode in this._treeNodes)
    {
      if (treeNode.Upgrade == DataManager.Instance.MostRecentTreeUpgrade)
        return (Selectable) treeNode.Button;
    }
    return (Selectable) null;
  }

  public override int UpgradePoints() => UpgradeSystem.AbilityPoints;

  public override string GetPointsText() => UpgradeSystem.AbilityPoints.ToString();

  public override void UpdatePointsText()
  {
    base.UpdatePointsText();
    if (this.UpgradePoints() <= 0)
    {
      if ((UnityEngine.Object) this._pointsText != (UnityEngine.Object) null)
        this._pointsText.color = this._zeroPointsTextColor;
      if ((UnityEngine.Object) this._crownImage != (UnityEngine.Object) null)
        this._crownImage.color = this._zeroPointsCrownImageColor;
      if ((UnityEngine.Object) this._crownOutline != (UnityEngine.Object) null)
        this._crownOutline.color = this._zeroPointsCrownOutlineColor;
      if ((UnityEngine.Object) this._decorationImageLeft != (UnityEngine.Object) null)
        this._decorationImageLeft.color = this._zeroPointsDecorationImageColor;
      if ((UnityEngine.Object) this._decorationImageRight != (UnityEngine.Object) null)
        this._decorationImageRight.color = this._zeroPointsDecorationImageColor;
      if ((UnityEngine.Object) this._divineInspirationText != (UnityEngine.Object) null)
        this._divineInspirationText.color = this._zeroPointsDivineInspirationTextColor;
      if (!((UnityEngine.Object) this._pointsGoop != (UnityEngine.Object) null))
        return;
      if ((UnityEngine.Object) this._pointsGoop.material != (UnityEngine.Object) null)
        this._pointsGoop.material.SetColor("_TintCOlor", this._zeroPointsGoopColor);
      else
        this._pointsGoop.color = this._zeroPointsGoopColor;
    }
    else
    {
      if (!((UnityEngine.Object) this._pointsGoop != (UnityEngine.Object) null))
        return;
      if ((UnityEngine.Object) this._pointsGoop.material != (UnityEngine.Object) null)
        this._pointsGoop.material.SetColor("_TintCOlor", this._availablePointsGoopColor);
      else
        this._pointsGoop.color = this._availablePointsGoopColor;
    }
  }

  public new virtual void Update()
  {
    this.ZoomUpdate();
    this.DelayTimer += Time.unscaledDeltaTime;
    if (DataManager.Instance.MAJOR_DLC && (double) this.DelayTimer > 1.0 && InputManager.UI.GetPageNavigateRightDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) && !this.changingTrees && this.canSwapPages && !this.IsHiding && this.CanvasGroup.interactable)
      this.ShowDLCTree(false, false);
    else if (this._dlcLock.activeSelf && InputManager.UI.GetPageNavigateRightDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) && !this.resetFail)
    {
      this._dlcLock.transform.DOComplete();
      this._dlcLock.transform.DOPunchPosition(Vector3.one * 1.2f, 0.33f).SetUpdate<Tweener>(true);
      UIManager.PlayAudio("event:/ui/negative_feedback");
      MMVibrate.Haptic(MMVibrate.HapticTypes.Failure);
      this.resetFail = true;
    }
    else
      this.resetFail = false;
  }

  public void ShowDLCTree() => this.ShowDLCTree(false, false);

  public void ShowDLCTree(bool forced, bool animateSnow)
  {
    BuildingShrine.ShowingDLCTree = true;
    this._canvasGroup.interactable = false;
    this.changingTrees = true;
    UIDLCUpgradeTreeMenuController treeMenuController1 = MonoSingleton<UIManager>.Instance.DLCUpgradeTreeMenuTemplate.Instantiate<UIDLCUpgradeTreeMenuController>();
    treeMenuController1.Show(this, animateSnow);
    UIDLCUpgradeTreeMenuController treeMenuController2 = treeMenuController1;
    treeMenuController2.OnShownCompleted = treeMenuController2.OnShownCompleted + (System.Action) (() =>
    {
      this.gameObject.SetActive(false);
      this.changingTrees = false;
    });
    if (!forced)
      return;
    treeMenuController1.DelayTimer = 0.5f;
  }

  public override void DoRelease()
  {
  }

  public override void DoUnlock(UpgradeSystem.Type upgrade)
  {
    UpgradeSystem.UnlockAbility(upgrade);
    --UpgradeSystem.AbilityPoints;
    DataManager.Instance.MostRecentTreeUpgrade = upgrade;
  }

  public override UpgradeTreeNode.TreeTier TreeTier()
  {
    return DataManager.Instance.CurrentUpgradeTreeTier;
  }

  public override void UpdateTier(UpgradeTreeNode.TreeTier tier)
  {
    DataManager.Instance.CurrentUpgradeTreeTier = tier;
  }

  public override void OnUnlockAnimationCompleted()
  {
    base.OnUnlockAnimationCompleted();
    this._cursor.LockPosition = false;
  }

  public List<NodeConnectionLine> GetConnectionLines() => this.NodeConnections;

  public virtual IEnumerator DoDLCRevealAnimation()
  {
    UIUpgradeTreeMenuController treeMenuController = this;
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    treeMenuController.SetActiveStateForMenu(false);
    treeMenuController._cursor.enabled = false;
    treeMenuController._cursor.CanvasGroup.DOFade(0.0f, 0.1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    treeMenuController._scrollRect.enabled = false;
    yield return (object) new WaitForSecondsRealtime(1f);
    treeMenuController._dlcLock.gameObject.SetActive(false);
    treeMenuController._dlcAlert.gameObject.SetActive(true);
    treeMenuController._dlcAlert.transform.DOPunchScale(treeMenuController._dlcAlert.transform.localScale * 0.5f, 0.5f).SetUpdate<Tweener>(true);
    treeMenuController._dlcPrompt.GetComponent<MMButton>().SetInteractionState(true);
    treeMenuController._dlcPrompt.gameObject.SetActive(true);
    treeMenuController._dlcPrompt.transform.DOPunchScale(Vector3.one * 0.25f, 0.25f).SetUpdate<Tweener>(true);
    AudioManager.Instance.PlayOneShot("event:/dlc/ui/divinetree/woolhaven_tab_unlock");
    yield return (object) new WaitForSecondsRealtime(1f);
    treeMenuController.canSwapPages = true;
    treeMenuController.CanvasGroup.interactable = true;
  }

  public override void SetActiveStateForMenu(bool state)
  {
    base.SetActiveStateForMenu(state);
    this.canSwapPages = UpgradeSystem.GetUnlocked(UpgradeSystem.Type.WinterSystem);
    if (this.canSwapPages)
      return;
    this._dlcLock.gameObject.SetActive(true);
    this._dlcAlert.gameObject.SetActive(false);
    this._dlcPrompt.GetComponent<MMButton>().Interactable = false;
  }

  [CompilerGenerated]
  public void \u003CShowDLCTree\u003Eb__38_0()
  {
    this.gameObject.SetActive(false);
    this.changingTrees = false;
  }
}
