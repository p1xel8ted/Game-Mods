// Decompiled with JetBrains decompiler
// Type: UIComicMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using src.Managers;
using src.UINavigator;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class UIComicMenu : UIMenuBase
{
  [SerializeField]
  public TextMeshProUGUI _comicTitle;
  [SerializeField]
  public MMButton _playButton;
  [SerializeField]
  public MMButton _resumeButton;
  [SerializeField]
  public TextMeshProUGUI _resumeButtonText;
  [SerializeField]
  public MMButton _exitButton;
  [SerializeField]
  public GameObject _accessibleVersionContainer;
  [SerializeField]
  public MMToggle _accessibleVersionButton;
  [SerializeField]
  public TextMeshProUGUI _accessibleVersionText;
  [SerializeField]
  public UINavigatorFollowElement buttonHighlighter;
  [SerializeField]
  public Vector2[] positions;
  [SerializeField]
  public UIComicMenuController _uiComicMenuController;
  public bool showingMainComic = true;
  public bool simpleVersion;
  public bool animating;

  public override void Awake()
  {
    base.Awake();
    this._accessibleVersionButton.Value = false;
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.OnLocalizationUpdated);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.OnLocalizationUpdated);
    this._accessibleVersionButton.OnValueChanged -= new Action<bool>(this.OnValueChanged);
  }

  public void OnEnable()
  {
    this.animating = false;
    this._playButton.onClick.AddListener(new UnityAction(this.OnPlayButtonPressed));
    this._resumeButton.onClick.AddListener(new UnityAction(this.OnResumeButtonPressed));
    this._exitButton.onClick.AddListener(new UnityAction(this.OnExitButtonPressed));
    this._accessibleVersionButton.OnValueChanged += new Action<bool>(this.OnValueChanged);
  }

  public new void OnDisable()
  {
    this._playButton.onClick.RemoveListener(new UnityAction(this.OnPlayButtonPressed));
    this._resumeButton.onClick.RemoveListener(new UnityAction(this.OnResumeButtonPressed));
    this._exitButton.onClick.RemoveListener(new UnityAction(this.OnExitButtonPressed));
    this._accessibleVersionButton.OnValueChanged -= new Action<bool>(this.OnValueChanged);
    this.StopAllCoroutines();
  }

  public void Start()
  {
    this.OnLocalizationUpdated();
    this._accessibleVersionText.text = $"({this._accessibleVersionText.text})";
  }

  public bool IsMenuReady => !this.animating && this._uiComicMenuController.IntroDone;

  public void Update()
  {
    if (!UIComicMenuController.AllowInput || this.animating || !InputManager.UI.GetPageNavigateRightDown() || !this._uiComicMenuController.IntroDone)
      return;
    this.StartCoroutine((IEnumerator) this.SwapComics());
    UIComicMenuController.ButtonPressed();
  }

  public void SwapComic()
  {
    if (this.animating || !this._uiComicMenuController.IntroDone || !UIComicMenuController.AllowInput)
      return;
    this.StartCoroutine((IEnumerator) this.SwapComics());
    UIComicMenuController.ButtonPressed();
  }

  public IEnumerator SwapComics()
  {
    if (PersistenceManager.PersistentData.UnlockedBonusComicPages)
    {
      this.animating = true;
      this._uiComicMenuController.FrontCover.DoIntro = false;
      this._uiComicMenuController.BonusCover.DoIntro = false;
      if (this.showingMainComic)
        this.ChangeComic((RectTransform) this._uiComicMenuController.BonusParent.transform, this._uiComicMenuController.BonusCover, (RectTransform) this._uiComicMenuController.DefaultParent.transform, this._uiComicMenuController.FrontCover);
      else
        this.ChangeComic((RectTransform) this._uiComicMenuController.DefaultParent.transform, this._uiComicMenuController.FrontCover, (RectTransform) this._uiComicMenuController.BonusParent.transform, this._uiComicMenuController.BonusCover);
      this.showingMainComic = !this.showingMainComic;
      this.OnComicChanged(this.showingMainComic);
      yield return (object) new WaitForSeconds(1f);
      this.animating = false;
    }
  }

  public void ChangeComic(
    RectTransform focusContainer,
    Book focusBook,
    RectTransform nonFocusContainer,
    Book nonFocusBook)
  {
    nonFocusContainer.DOAnchorPos(this.positions[1], 1f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutBack);
    nonFocusContainer.DOScale(Vector3.one * 0.75f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    nonFocusBook.enabled = false;
    nonFocusBook.RightNext.color = Color.grey;
    focusContainer.DOAnchorPos(this.positions[0], 1f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutBack);
    focusContainer.DOScale(Vector3.one, 1f);
    focusBook.enabled = true;
    focusBook.RightNext.color = Color.white;
  }

  public override void OnShowCompleted()
  {
    if (PersistenceManager.PersistentData.ComicPageIndex.x == 0 || this._accessibleVersionButton.Value)
    {
      this._resumeButton.gameObject.SetActive(false);
      MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this._playButton);
    }
    else
      MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this._resumeButton);
    this.OnComicChanged(this.showingMainComic);
    this.OnLocalizationUpdated();
  }

  public void OnLocalizationUpdated()
  {
    float num = Mathf.Clamp((float) ((double) PersistenceManager.PersistentData.ComicPageIndex.x / 50.0 * 100.0), 1f, float.MaxValue);
    this._resumeButtonText.text = $"{LocalizationManager.GetTranslation("UI/Resume")}<br><size=20><i> ( {string.Format(LocalizationManager.GetTranslation("Comic/Page"), (object) num)} )";
  }

  public void OnComicChanged(bool isMainComic)
  {
    if (isMainComic)
      this._comicTitle.text = $"<uppercase>{LocalizationManager.GetTranslation("Comic/Page8_Bubble_4")}</uppercase>";
    else
      this._comicTitle.text = $"<uppercase>{LocalizationManager.GetTranslation("Comic/Bonus_Title")}</uppercase>";
    this.showingMainComic = isMainComic;
    this._accessibleVersionContainer.gameObject.SetActive(isMainComic);
    this._resumeButton.gameObject.SetActive(!this._accessibleVersionButton.Value & isMainComic && PersistenceManager.PersistentData.ComicPageIndex.x != 0);
    this.buttonHighlighter.DoMoveButton(MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable);
    this._uiComicMenuController.CheckUnlockedComicButtonPrompts(this.showingMainComic);
    if (PersistenceManager.PersistentData.ComicPageIndex.x == 0 || !isMainComic || isMainComic && this._accessibleVersionButton.Value)
      MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this._playButton);
    else
      MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this._resumeButton);
  }

  public void OnResumeButtonPressed()
  {
    if (!UIComicMenuController.AllowInput)
      return;
    UIComicMenuController.ButtonPressed();
    this._uiComicMenuController.BookOpened(this.simpleVersion ? UIComicMenuController.ComicType.Simple : UIComicMenuController.ComicType.Default);
    this.Hide();
  }

  public void OnPlayButtonPressed()
  {
    if (!UIComicMenuController.AllowInput)
      return;
    UIComicMenuController.ButtonPressed();
    UIManager.PlayAudio("event:/Stings/church_bell");
    MMVibrate.Haptic(MMVibrate.HapticTypes.Selection);
    DeviceLightingManager.FlashColor(Color.white);
    if (!this._accessibleVersionButton.Value && !this._uiComicMenuController.IsShowingBonus)
    {
      PersistenceManager.PersistentData.ComicPageIndex = Vector2Int.zero;
      PersistenceManager.PersistentData.ComicPanelIndex = 0;
      PersistenceManager.PersistentData.ComicExploredPages.Clear();
      PersistenceManager.Save();
    }
    this._uiComicMenuController.BookOpened(this.simpleVersion ? UIComicMenuController.ComicType.Simple : UIComicMenuController.ComicType.Default);
    this.Hide();
  }

  public override void OnHideStarted() => base.OnHideStarted();

  public void OnValueChanged(bool obj)
  {
    this.simpleVersion = obj;
    this._resumeButton.gameObject.SetActive(!this.simpleVersion && this.showingMainComic && PersistenceManager.PersistentData.ComicPageIndex.x != 0);
    this.buttonHighlighter.DoMoveButton(MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable);
  }

  public void OnExitButtonPressed()
  {
    if (!UIComicMenuController.AllowInput)
      return;
    UIComicMenuController.ButtonPressed();
    this._uiComicMenuController.OnCancelButtonInput();
  }
}
