// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeInstructionsScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Flockade;

public class FlockadeInstructionsScreen : UISubmenuBase
{
  public const float _FADE_IN_OUT_DURATION = 0.166666672f;
  public const Ease _FADE_IN_EASING = Ease.OutCubic;
  public const Ease _FADE_OUT_EASING = Ease.InCubic;
  [Header("Instructions Menu")]
  [SerializeField]
  public Localize _description;
  [SerializeField]
  public TextMeshProUGUI _descriptionText;
  [SerializeField]
  public CanvasGroup _contentCanvasGroup;
  [SerializeField]
  public FlockadeInstructionsScreen.Page[] _pages;
  [SerializeField]
  public TextMeshProUGUI _pageCounter;
  [SerializeField]
  public Button _continueButton;
  [Header("Misc")]
  [SerializeField]
  public UINavigatorFollowElement _buttonHighlight;
  public FlockadeInstructionsScreen.Page[] _currentPages;
  public int _currentPageIndex;
  public bool _isAnyShepherdUnlocked;
  public int _numberedPagesTotal;
  public string _pageCounterFormatter;
  [CompilerGenerated]
  public bool \u003CShowOnlyShepherdTutorialNextTime\u003Ek__BackingField;

  public event System.Action ContinueButtonPressed;

  public event System.Action BackButtonPressed;

  public bool ShowOnlyShepherdTutorialNextTime
  {
    get => this.\u003CShowOnlyShepherdTutorialNextTime\u003Ek__BackingField;
    set => this.\u003CShowOnlyShepherdTutorialNextTime\u003Ek__BackingField = value;
  }

  public override void OnShowStarted()
  {
    this._isAnyShepherdUnlocked = FlockadePieceManager.IsAnyPieceOfSameKindUnlocked(FlockadePieceType.Shepherd);
    this._currentPages = ((IEnumerable<FlockadeInstructionsScreen.Page>) this._pages).Where<FlockadeInstructionsScreen.Page>((Func<FlockadeInstructionsScreen.Page, bool>) (page =>
    {
      switch (page.Condition)
      {
        case FlockadeInstructionsScreen.Condition.Always:
          return !this.ShowOnlyShepherdTutorialNextTime;
        case FlockadeInstructionsScreen.Condition.AnyShepherdUnlocked:
          return this._isAnyShepherdUnlocked;
        default:
          throw new ArgumentOutOfRangeException();
      }
    })).ToArray<FlockadeInstructionsScreen.Page>();
    this.ShowOnlyShepherdTutorialNextTime = false;
    this._currentPageIndex = -1;
    this._numberedPagesTotal = ((IEnumerable<FlockadeInstructionsScreen.Page>) this._currentPages).Count<FlockadeInstructionsScreen.Page>((Func<FlockadeInstructionsScreen.Page, bool>) (page => page.Numbered));
    DataManager.Instance.FlockadeTutorialShown = true;
    this.ShowNextPage().Complete(true);
  }

  public override IEnumerator DoShowAnimation()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    FlockadeInstructionsScreen instructionsScreen = this;
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
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) instructionsScreen._canvasGroup.DOFade(1f, 0.5f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InCubic).WaitForCompletion();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public override void SetActiveStateForMenu(bool state)
  {
    this._buttonHighlight.enabled = state;
    base.SetActiveStateForMenu(state);
  }

  public override void Awake()
  {
    base.Awake();
    this._pageCounterFormatter = this._pageCounter.text;
  }

  public virtual void Start()
  {
    this._continueButton.onClick.AddListener(new UnityAction(this.OnContinueButtonInput));
  }

  public void OnContinueButtonInput()
  {
    DG.Tweening.Sequence t = this.ShowNextPage();
    if (t == null)
    {
      System.Action continueButtonPressed = this.ContinueButtonPressed;
      if (continueButtonPressed == null)
        return;
      continueButtonPressed();
    }
    else
    {
      this._continueButton.interactable = false;
      t.OnComplete<DG.Tweening.Sequence>((TweenCallback) (() => this._continueButton.interactable = true));
    }
  }

  public DG.Tweening.Sequence ShowNextPage()
  {
    if (this._currentPageIndex > this._currentPages.Length - 1)
      return (DG.Tweening.Sequence) null;
    FlockadeInstructionsScreen.Page currentPage = this._currentPageIndex >= 0 ? this._currentPages[this._currentPageIndex] : (FlockadeInstructionsScreen.Page) null;
    if (this._currentPageIndex == this._currentPages.Length - 1)
    {
      currentPage?.GameObject.SetActive(false);
      return (DG.Tweening.Sequence) null;
    }
    FlockadeInstructionsScreen.Page nextPage = this._currentPages[++this._currentPageIndex];
    if (nextPage.Condition == FlockadeInstructionsScreen.Condition.AnyShepherdUnlocked)
      DataManager.Instance.FlockadeShepherdsTutorialShown = true;
    return DOTween.Sequence().Append((Tween) ShortcutExtensionsTMPText.DOFade(this._descriptionText, 0.0f, 0.166666672f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.InCubic)).Join((Tween) this._contentCanvasGroup.DOFade(0.0f, 0.166666672f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InCubic)).Join((Tween) ShortcutExtensionsTMPText.DOFade(this._pageCounter, 0.0f, 0.166666672f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.InCubic)).AppendCallback((TweenCallback) (() =>
    {
      if (string.IsNullOrEmpty(nextPage.Description))
      {
        this._description.Term = (string) null;
        this._descriptionText.text = string.Empty;
      }
      else
        this._description.Term = nextPage.Description;
      currentPage?.GameObject.SetActive(false);
      nextPage.GameObject.SetActive(true);
      if (nextPage.Numbered)
        this._pageCounter.text = string.Format(this._pageCounterFormatter, (object) ((IEnumerable<FlockadeInstructionsScreen.Page>) this._currentPages).Take<FlockadeInstructionsScreen.Page>(this._currentPageIndex + 1).Count<FlockadeInstructionsScreen.Page>((Func<FlockadeInstructionsScreen.Page, bool>) (page => page.Numbered)), (object) this._numberedPagesTotal);
      else
        this._pageCounter.text = string.Empty;
    })).Append((Tween) ShortcutExtensionsTMPText.DOFade(this._descriptionText, 1f, 0.166666672f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutCubic)).Join((Tween) this._contentCanvasGroup.DOFade(1f, 0.166666672f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutCubic)).Join((Tween) ShortcutExtensionsTMPText.DOFade(this._pageCounter, 1f, 0.166666672f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutCubic));
  }

  public override void OnCancelButtonInput()
  {
    if (!this._parent.CanvasGroup.interactable || !this._canvasGroup.interactable)
      return;
    this._currentPages[this._currentPageIndex].GameObject.SetActive(false);
    System.Action backButtonPressed = this.BackButtonPressed;
    if (backButtonPressed == null)
      return;
    backButtonPressed();
  }

  public override IEnumerator DoHideAnimation()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    FlockadeInstructionsScreen instructionsScreen = this;
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
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) instructionsScreen._canvasGroup.DOFade(0.0f, 0.5f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutCubic).WaitForCompletion();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  [CompilerGenerated]
  public bool \u003COnShowStarted\u003Eb__27_0(FlockadeInstructionsScreen.Page page)
  {
    switch (page.Condition)
    {
      case FlockadeInstructionsScreen.Condition.Always:
        return !this.ShowOnlyShepherdTutorialNextTime;
      case FlockadeInstructionsScreen.Condition.AnyShepherdUnlocked:
        return this._isAnyShepherdUnlocked;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  [CompilerGenerated]
  public void \u003COnContinueButtonInput\u003Eb__32_0()
  {
    this._continueButton.interactable = true;
  }

  public enum Condition
  {
    Always,
    AnyShepherdUnlocked,
  }

  [Serializable]
  public class Page
  {
    [TermsPopup("")]
    public string Description;
    public GameObject GameObject;
    public FlockadeInstructionsScreen.Condition Condition;
    public bool Numbered = true;
  }
}
