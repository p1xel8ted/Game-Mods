// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UISandboxMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine.Unity;
using src.Extensions;
using src.UINavigator;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UISandboxMenuController : UIMenuBase
{
  public Action<ScenarioData> OnScenarioChosen;
  [Header("Main Content")]
  [SerializeField]
  public RectTransform _lambContainer;
  [SerializeField]
  public SkeletonGraphic _lambGraphic;
  [SerializeField]
  public SandboxCategory[] _categories;
  [SerializeField]
  public Image _backgroundImage;
  [SerializeField]
  public UIMenuControlPrompts _controlPrompts;
  [SerializeField]
  public Image _background;
  [Header("Buttons")]
  [SerializeField]
  public MMButton _leftTab;
  [SerializeField]
  public GameObject _leftTabAlert;
  [SerializeField]
  public MMButton _rightTab;
  [SerializeField]
  public GameObject _rightTabAlert;
  [Header("XP Bar")]
  [SerializeField]
  public RectTransform _xpBarTransform;
  [Header("Incognito")]
  [SerializeField]
  public CanvasGroup _incognitoLower;
  [SerializeField]
  public CanvasGroup _incognitoUpper;
  public SandboxCategory _currentMenu;
  public bool _bossRushAvailable;
  public bool _onboardingBossRush;
  public Material _backgroundMaterial;

  public override void Awake()
  {
    base.Awake();
    this._onboardingBossRush = DungeonSandboxManager.GetCompletedRunCount() > 0 && !DataManager.Instance.OnboardedBossRush;
    this._incognitoLower.alpha = 0.0f;
    this._incognitoUpper.alpha = 0.0f;
    this._backgroundMaterial = new Material(this._background.material);
    this._background.material = this._backgroundMaterial;
    int selectedFleece = 0;
    foreach (SandboxCategory category in this._categories)
    {
      if (this._onboardingBossRush || DataManager.Instance.OnboardedBossRush)
        category.ShowDots();
      else
        category.HideDots();
      category.OnFleeceSelected += (Action<PlayerFleeceManager.FleeceType>) (fleece =>
      {
        selectedFleece = (int) fleece;
        if (DataManager.Instance.UnlockedFleeces.Contains((int) fleece))
        {
          this._controlPrompts.ShowAcceptButton();
          this._lambGraphic.Skeleton.SetSkin($"Lamb_{(int) fleece}");
        }
        else
        {
          this._lambGraphic.Skeleton.SetSkin("Lamb_0");
          this._controlPrompts.HideAcceptButton();
        }
      });
      category.OnScenarioChosen += (Action<ScenarioData>) (scenario =>
      {
        scenario.FleeceType = selectedFleece;
        Action<ScenarioData> onScenarioChosen = this.OnScenarioChosen;
        if (onScenarioChosen != null)
          onScenarioChosen(scenario);
        this.Hide();
      });
    }
    this._currentMenu = this._categories[0];
    this._canvasGroup.alpha = 0.0f;
    this._rightTab.interactable = DataManager.Instance.OnboardedBossRush;
    this._leftTab.interactable = DataManager.Instance.OnboardedBossRush;
    this._leftTab.gameObject.SetActive(DataManager.Instance.OnboardedBossRush);
    this._rightTab.gameObject.SetActive(DataManager.Instance.OnboardedBossRush);
    this._rightTab.onClick.AddListener(new UnityAction(this.NavigatePageRight));
    this._leftTab.onClick.AddListener(new UnityAction(this.NavigatePageLeft));
    this._rightTabAlert.SetActive(false);
    this._leftTabAlert.SetActive(false);
  }

  public void OnEnable()
  {
    MonoSingleton<UINavigatorNew>.Instance.OnPageNavigateLeft += new System.Action(this.NavigatePageLeft);
    MonoSingleton<UINavigatorNew>.Instance.OnPageNavigateRight += new System.Action(this.NavigatePageRight);
    this.CheckTabsShouldBeVisible();
    if (!SettingsManager.Settings.Accessibility.DyslexicFont)
      return;
    this._xpBarTransform.anchoredPosition = new Vector2(0.0f, -50f);
  }

  public new void OnDisable()
  {
    MonoSingleton<UINavigatorNew>.Instance.OnPageNavigateLeft -= new System.Action(this.NavigatePageLeft);
    MonoSingleton<UINavigatorNew>.Instance.OnPageNavigateRight -= new System.Action(this.NavigatePageRight);
  }

  public override IEnumerator DoShowAnimation()
  {
    UISandboxMenuController sandboxMenuController = this;
    sandboxMenuController._canvasGroup.DOFade(1f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    Vector3 localScale = sandboxMenuController._lambContainer.localScale;
    sandboxMenuController._lambContainer.localScale = localScale * 1.5f;
    sandboxMenuController._lambContainer.DOScale(localScale, 0.5f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    sandboxMenuController._lambGraphic.SetAnimation("sermons/doctrine-loop", true);
    sandboxMenuController._currentMenu.Show();
    if (sandboxMenuController._onboardingBossRush)
    {
      yield return (object) null;
      sandboxMenuController.SetActiveStateForMenu(false);
      MonoSingleton<UINavigatorNew>.Instance.Clear();
      sandboxMenuController._controlPrompts.HideAcceptButton();
      sandboxMenuController._controlPrompts.HideCancelButton();
      Vector2 rightOrigin = (Vector2) sandboxMenuController._rightTab.transform.localPosition;
      Vector2 leftOrigin = (Vector2) sandboxMenuController._leftTab.transform.localPosition;
      sandboxMenuController._rightTab.transform.localPosition += new Vector3(200f, 0.0f, 0.0f);
      sandboxMenuController._leftTab.transform.localPosition -= new Vector3(200f, 0.0f, 0.0f);
      foreach (SandboxCategory category in sandboxMenuController._categories)
        category.SetIncognitoMode();
      sandboxMenuController._incognitoUpper.alpha = 0.65f;
      yield return (object) sandboxMenuController._currentMenu.YieldUntilShown();
      yield return (object) new WaitForSecondsRealtime(0.25f);
      sandboxMenuController._rightTab.gameObject.SetActive(true);
      sandboxMenuController._rightTab.transform.DOLocalMove((Vector3) rightOrigin, 0.5f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      yield return (object) new WaitForSecondsRealtime(0.5f);
      yield return (object) new WaitForSecondsRealtime(0.1f);
      sandboxMenuController._rightTabAlert.transform.localScale = Vector3.zero;
      sandboxMenuController._rightTabAlert.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBounce).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      sandboxMenuController._rightTabAlert.gameObject.SetActive(true);
      UIManager.PlayAudio("event:/ui/glass_ball_ding");
      yield return (object) new WaitForSecondsRealtime(0.5f);
      sandboxMenuController.SetActiveStateForMenu(sandboxMenuController._rightTab.gameObject, true);
      float timer = 0.0f;
      do
      {
        timer += Time.unscaledDeltaTime;
        if ((double) timer <= 1.0)
          yield return (object) null;
        else
          break;
      }
      while (!InputManager.UI.GetPageNavigateRightDown());
      sandboxMenuController._rightTab.interactable = false;
      sandboxMenuController._rightTab.gameObject.SetActive(false);
      sandboxMenuController._rightTabAlert.SetActive(false);
      sandboxMenuController.DoPageNavigateRight();
      yield return (object) sandboxMenuController._currentMenu.YieldUntilShown();
      sandboxMenuController._leftTab.gameObject.SetActive(true);
      sandboxMenuController._leftTab.transform.DOLocalMove((Vector3) leftOrigin, 0.5f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      yield return (object) new WaitForSecondsRealtime(0.5f);
      yield return (object) new WaitForSecondsRealtime(0.1f);
      sandboxMenuController._leftTabAlert.transform.localScale = Vector3.zero;
      sandboxMenuController._leftTabAlert.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBounce).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      sandboxMenuController._leftTabAlert.gameObject.SetActive(true);
      UIManager.PlayAudio("event:/ui/glass_ball_ding");
      yield return (object) new WaitForSecondsRealtime(0.5f);
      foreach (SandboxCategory category in sandboxMenuController._categories)
        category.RemoveIncognitoMode();
      sandboxMenuController._incognitoUpper.DOFade(0.0f, 0.25f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
      yield return (object) new WaitForSecondsRealtime(0.25f);
      sandboxMenuController._leftTab.interactable = true;
      sandboxMenuController._rightTab.interactable = true;
      sandboxMenuController._onboardingBossRush = false;
      sandboxMenuController._controlPrompts.ShowAcceptButton();
      sandboxMenuController._controlPrompts.ShowCancelButton();
      sandboxMenuController._currentMenu.SetCurrentSelectable((PlayerFleeceManager.FleeceType) DataManager.Instance.PlayerFleece);
      sandboxMenuController._currentMenu.Activate();
      sandboxMenuController.SetActiveStateForMenu(true);
      DataManager.Instance.OnboardedBossRush = true;
      rightOrigin = new Vector2();
      leftOrigin = new Vector2();
    }
    else
      sandboxMenuController._currentMenu.SetCurrentSelectable((PlayerFleeceManager.FleeceType) DataManager.Instance.PlayerFleece);
    yield return (object) new WaitForSecondsRealtime(0.5f);
  }

  public void CheckTabsShouldBeVisible()
  {
    if (!DataManager.Instance.OnboardedBossRush)
      return;
    this._leftTab.gameObject.SetActive(true);
    this._rightTab.gameObject.SetActive(true);
    if (this._categories.IndexOf<SandboxCategory>(this._currentMenu) == 0)
      this._leftTab.gameObject.SetActive(false);
    if (this._categories.IndexOf<SandboxCategory>(this._currentMenu) != this._categories.Length - 1)
      return;
    this._rightTab.gameObject.SetActive(false);
  }

  public void NavigatePageLeft()
  {
    if (!this._leftTab.interactable)
      return;
    this.DoNavigatePageLeft();
  }

  public void DoNavigatePageLeft()
  {
    this._leftTabAlert.SetActive(false);
    this._leftTab.gameObject.transform.DOKill();
    this._leftTab.gameObject.transform.DOShakePosition(1f, new Vector3(10f, 0.0f));
    if (this._categories.IndexOf<SandboxCategory>(this._currentMenu) == 0)
    {
      UIManager.PlayAudio("event:/ui/negative_feedback");
    }
    else
    {
      UIManager.PlayAudio("event:/ui/conversation_change_page");
      this.PerformMenuTransition(this._currentMenu, this._categories[this._categories.IndexOf<SandboxCategory>(this._currentMenu) - 1], SandboxCategory.TransitionType.MoveLeft);
    }
    this.CheckTabsShouldBeVisible();
  }

  public void NavigatePageRight()
  {
    if (!this._rightTab.interactable)
      return;
    this.DoPageNavigateRight();
  }

  public void DoPageNavigateRight()
  {
    this._rightTabAlert.SetActive(false);
    this._rightTab.gameObject.transform.DOKill();
    this._rightTab.gameObject.transform.DOShakePosition(1f, new Vector3(10f, 0.0f));
    if (this._categories.IndexOf<SandboxCategory>(this._currentMenu) == this._categories.Length - 1)
    {
      UIManager.PlayAudio("event:/ui/negative_feedback");
    }
    else
    {
      this.PerformMenuTransition(this._currentMenu, this._categories[this._categories.IndexOf<SandboxCategory>(this._currentMenu) + 1], SandboxCategory.TransitionType.MoveRight);
      UIManager.PlayAudio("event:/ui/conversation_change_page");
    }
    this.CheckTabsShouldBeVisible();
  }

  public void PerformMenuTransition(
    SandboxCategory from,
    SandboxCategory to,
    SandboxCategory.TransitionType transitionType)
  {
    this._currentMenu = to;
    if (MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable != null)
      to.SetCurrentSelectable(MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable.GetComponent<SandboxFleeceItem>());
    from.Hide(transitionType);
    to.Show(transitionType);
    this._backgroundMaterial.DOKill();
    this._backgroundImage.DOKill();
    this._backgroundMaterial.DOColor(to._backgroundColor, "_Color1", 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    DOTweenModuleUI.DOColor(this._backgroundImage, to._backgroundColor, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
  }

  public void EndSelection() => this.Hide();

  public override IEnumerator DoHideAnimation()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    UISandboxMenuController sandboxMenuController = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    sandboxMenuController._canvasGroup.DOFade(0.0f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(1f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  public override void OnHideCompleted()
  {
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    MonoSingleton<UIManager>.Instance.UnloadSandboxMenuAssets();
  }
}
