// Decompiled with JetBrains decompiler
// Type: UIComicMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Beffio.Dithering;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.MainMenu;
using MMTools;
using Rewired;
using src.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

#nullable disable
public class UIComicMenuController : UIMenuBase
{
  public static List<string> LanguageCodes = new List<string>()
  {
    "en",
    "ja",
    "ru",
    "fr",
    "de",
    "es",
    "pt-BR",
    "zh-CN",
    "zh-TW",
    "ko",
    "it",
    "nl",
    "tr",
    "fr-CA",
    "ar"
  };
  [SerializeField]
  public Transform pageContainer;
  [SerializeField]
  public Book frontCover;
  [SerializeField]
  public Book bonusCover;
  [SerializeField]
  public RectTransform defaultParent;
  [SerializeField]
  public RectTransform bonusParent;
  [SerializeField]
  public Palette comicPalette;
  [SerializeField]
  public Material comicMaterial;
  [SerializeField]
  public Material whiteDissolveMaterial;
  [SerializeField]
  public Material blackDissolveMaterial;
  [Space]
  [SerializeField]
  public CanvasGroup controlPromptsGroup;
  [SerializeField]
  public MMControlPrompt nextPrompt;
  [SerializeField]
  public MMControlPrompt previousPrompt;
  [SerializeField]
  public Image[] controlPromptImages;
  [SerializeField]
  public TMP_Text[] controlPromptText;
  [SerializeField]
  public GameObject controlPromptPreviousContainer;
  [SerializeField]
  public TMP_Text pageNumber;
  [SerializeField]
  public GoopFade goopFadeBG;
  [SerializeField]
  public Image normalBG;
  [SerializeField]
  public Sprite emptyPage;
  [SerializeField]
  public MMControlPrompt nextComicPrompt;
  [SerializeField]
  public CanvasGroup comicPromptHolderCanvasGroup;
  [SerializeField]
  public GameObject controlPrompts;
  [SerializeField]
  public CanvasGroup crownCanvasGroup;
  [CompilerGenerated]
  public Camera \u003CCamera\u003Ek__BackingField;
  [SerializeField]
  public UIComicMenu _uiComicMenu;
  [SerializeField]
  public UINormalComicMenuController _simpleComicParent;
  [SerializeField]
  public UINormalComicMenuController _bonusComicParent;
  [SerializeField]
  public Book _simpleComic;
  [SerializeField]
  public Book _bonusComic;
  public List<ComicPageData> comicPages;
  public List<ComicPageData> bonusPages;
  public Vector2Int pageIndex = Vector2Int.zero;
  public Dictionary<Vector2Int, AsyncOperationHandle<GameObject>> loadedPages = new Dictionary<Vector2Int, AsyncOperationHandle<GameObject>>();
  public Stylizer stylizer;
  public Palette cachedPalette;
  public Tween stylizerTween;
  [CompilerGenerated]
  public UIComicPage \u003CCurrentPage\u003Ek__BackingField;
  public List<Color> controlPromptImagesWhiteColours = new List<Color>();
  public List<Color> controlPromptTextWhiteColours = new List<Color>();
  public UIComicMenuController.ComicType currentComicType = UIComicMenuController.ComicType.None;
  public bool isShowingBonus;
  public bool animating;
  [CompilerGenerated]
  public bool \u003CIntroDone\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CBonusComicIndexLoaded\u003Ek__BackingField = -1;
  public static bool ComicActive = false;
  [CompilerGenerated]
  public static bool \u003CIsLoadingPage\u003Ek__BackingField = false;
  [CompilerGenerated]
  public bool \u003CRequiresAnimating\u003Ek__BackingField = true;
  [CompilerGenerated]
  public bool \u003CRequiresCombining\u003Ek__BackingField = true;
  [CompilerGenerated]
  public bool \u003CRequiresLastPanel\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CInvertPromptsColouring\u003Ek__BackingField;
  public static bool isInitialized = false;
  public static bool allowInput = true;
  public static float delayBetweenPress = 1.1f;
  public static float initializeDelay = 2f;
  public const float DELAY_BETWEEN_PRESS = 1.1f;
  public const float INITIALIZE_DELAY = 2f;
  public float lerper;

  public Transform PageContainer => this.pageContainer;

  public Book FrontCover => this.frontCover;

  public Book BonusCover => this.bonusCover;

  public RectTransform DefaultParent => this.defaultParent;

  public RectTransform BonusParent => this.bonusParent;

  public Palette ComicPalette => this.comicPalette;

  public Material ComicMaterial => this.comicMaterial;

  public Material WhiteDissolveMaterial => this.whiteDissolveMaterial;

  public Material BlackDissolveMaterial => this.blackDissolveMaterial;

  public MMControlPrompt NextPromt => this.nextPrompt;

  public Camera Camera
  {
    get => this.\u003CCamera\u003Ek__BackingField;
    set => this.\u003CCamera\u003Ek__BackingField = value;
  }

  public UINormalComicMenuController SimpleComicParent => this._simpleComicParent;

  public UINormalComicMenuController BonusComicParent => this._bonusComicParent;

  public Stylizer Stylizer => this.stylizer;

  public UIComicPage CurrentPage
  {
    get => this.\u003CCurrentPage\u003Ek__BackingField;
    set => this.\u003CCurrentPage\u003Ek__BackingField = value;
  }

  public bool IsShowingBonus => this.isShowingBonus;

  public Vector2Int PageIndex => this.pageIndex;

  public bool IntroDone
  {
    get => this.\u003CIntroDone\u003Ek__BackingField;
    set => this.\u003CIntroDone\u003Ek__BackingField = value;
  }

  public int BonusComicIndexLoaded
  {
    get => this.\u003CBonusComicIndexLoaded\u003Ek__BackingField;
    set => this.\u003CBonusComicIndexLoaded\u003Ek__BackingField = value;
  }

  public static bool IsLoadingPage
  {
    set => UIComicMenuController.\u003CIsLoadingPage\u003Ek__BackingField = value;
    get => UIComicMenuController.\u003CIsLoadingPage\u003Ek__BackingField;
  }

  public bool RequiresAnimating
  {
    get => this.\u003CRequiresAnimating\u003Ek__BackingField;
    set => this.\u003CRequiresAnimating\u003Ek__BackingField = value;
  }

  public bool RequiresCombining
  {
    get => this.\u003CRequiresCombining\u003Ek__BackingField;
    set => this.\u003CRequiresCombining\u003Ek__BackingField = value;
  }

  public bool RequiresLastPanel
  {
    get => this.\u003CRequiresLastPanel\u003Ek__BackingField;
    set => this.\u003CRequiresLastPanel\u003Ek__BackingField = value;
  }

  public bool InvertPromptsColouring
  {
    get => this.\u003CInvertPromptsColouring\u003Ek__BackingField;
    set => this.\u003CInvertPromptsColouring\u003Ek__BackingField = value;
  }

  public bool IsMenuOpen
  {
    get => (UnityEngine.Object) this._uiComicMenu != (UnityEngine.Object) null && this._uiComicMenu.gameObject.activeSelf;
  }

  public override void Awake()
  {
    base.Awake();
    UIComicMenuController.InitializeDelay();
    UIComicMenuController.ComicActive = true;
    this.comicPages = ((IEnumerable<ComicPageData>) UnityEngine.Resources.LoadAll<ComicPageData>("Data/Comic Data")).ToList<ComicPageData>();
    this.bonusPages = ((IEnumerable<ComicPageData>) UnityEngine.Resources.LoadAll<ComicPageData>("Data/Comic Bonus Data")).ToList<ComicPageData>();
    this.Camera = UIMainMenuController.Instance.ComicCamera;
    this.GetComponent<Canvas>().worldCamera = this.Camera;
    this.controlPromptsGroup.alpha = 0.0f;
    foreach (Graphic controlPromptImage in this.controlPromptImages)
      this.controlPromptImagesWhiteColours.Add(controlPromptImage.color);
    foreach (Graphic graphic in this.controlPromptText)
      this.controlPromptTextWhiteColours.Add(graphic.color);
    InputManager.General.OnActiveControllerChanged += new Action<Controller>(this.OnActiveControllerChanged);
    this.bonusCover.enabled = false;
    this.UpdateInputPrompts();
    this.comicPromptHolderCanvasGroup.alpha = 0.0f;
    this.CheckUnlockedComicButtonPrompts(true);
    this.frontCover.OnIntroComplete.AddListener(new UnityAction(this.DoneIntro));
    this.frontCover.OnIntroStart.AddListener(new UnityAction(this.GoopFadeIn));
    this.bonusCover.gameObject.SetActive(false);
    AudioManager.Instance.PlayMusic("event:/music/comic/comic");
  }

  public static bool IsInitialized => UIComicMenuController.isInitialized;

  public static bool AllowInput
  {
    get => UIComicMenuController.allowInput && UIComicMenuController.isInitialized;
  }

  public static void InitializeDelay()
  {
    UIComicMenuController.allowInput = false;
    UIComicMenuController.delayBetweenPress = 1.1f;
    UIComicMenuController.isInitialized = false;
    UIComicMenuController.initializeDelay = 2f;
  }

  public static void ButtonPressed()
  {
    if (!UIComicMenuController.AllowInput)
      return;
    UIComicMenuController.allowInput = false;
    UIComicMenuController.delayBetweenPress = 1.1f;
  }

  public void Update()
  {
    if (!UIComicMenuController.allowInput && (double) (UIComicMenuController.delayBetweenPress -= Time.deltaTime) <= 0.0)
      UIComicMenuController.allowInput = true;
    if (!UIComicMenuController.isInitialized && (double) (UIComicMenuController.initializeDelay -= Time.deltaTime) <= 0.0)
      UIComicMenuController.isInitialized = true;
    if ((UnityEngine.Object) this.CurrentPage != (UnityEngine.Object) null)
    {
      if ((double) this.crownCanvasGroup.alpha > 0.0 && (this.CurrentPage.Animating || this.animating))
        this.lerper -= Time.deltaTime * 4f;
      else if ((double) this.crownCanvasGroup.alpha < 1.0 && !this.CurrentPage.Animating && !this.animating)
        this.lerper += Time.deltaTime * 4f;
      this.crownCanvasGroup.alpha = Mathf.Lerp(0.0f, 1f, this.lerper);
    }
    else
      this.crownCanvasGroup.alpha = 0.0f;
  }

  public void GoopFadeIn() => this.goopFadeBG.FadeIn(1f);

  public void CheckUnlockedComicButtonPrompts(bool isMainComic)
  {
    this.isShowingBonus = !isMainComic;
    if (!PersistenceManager.PersistentData.UnlockedBonusComicPages)
    {
      Debug.Log((object) "Comic not unlocked");
      this.nextComicPrompt.gameObject.SetActive(false);
      this.bonusParent.gameObject.SetActive(false);
    }
    else
      this.nextComicPrompt.gameObject.SetActive(true);
  }

  public void DoneIntro()
  {
    Debug.Log((object) "Done Intro");
    this._uiComicMenu.Show();
    this.comicPromptHolderCanvasGroup.DOFade(1f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    Vector3 position = this.bonusCover.gameObject.transform.position;
    this.bonusCover.enabled = false;
    this.bonusCover.gameObject.SetActive(PersistenceManager.PersistentData.UnlockedBonusComicPages);
    this.bonusCover.transform.position = this.frontCover.transform.position;
    this.bonusCover.transform.DOMove(position, 1f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.IntroDone = true));
  }

  public void ShowComic(bool isShowingBonus)
  {
    this.isShowingBonus = isShowingBonus;
    this.Show();
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    UIComicMenuController.ComicActive = false;
    this.comicPages.Clear();
    this.comicPages = (List<ComicPageData>) null;
    this.bonusPages.Clear();
    this.bonusPages = (List<ComicPageData>) null;
    if (this.stylizerTween != null)
    {
      this.stylizerTween.Kill();
      this.stylizerTween = (Tween) null;
    }
    this.CleanUpBook();
    if ((UnityEngine.Object) this.frontCover != (UnityEngine.Object) null)
    {
      this.frontCover.OnIntroComplete.RemoveAllListeners();
      this.frontCover.OnIntroStart.RemoveAllListeners();
    }
    InputManager.General.OnActiveControllerChanged -= new Action<Controller>(this.OnActiveControllerChanged);
  }

  public void CleanUpBook()
  {
    foreach (KeyValuePair<Vector2Int, AsyncOperationHandle<GameObject>> loadedPage in this.loadedPages)
      Addressables.Release<GameObject>(loadedPage.Value);
    this.loadedPages.Clear();
    this.CurrentPage = (UIComicPage) null;
  }

  public override void OnShowStarted()
  {
    MMVibrate.StopRumble();
    UIManager.PlayAudio("event:/ui/pause");
    this.stylizer = this.Camera.GetComponent<Stylizer>();
    this.DisableStylizer();
    this.frontCover.gameObject.SetActive(true);
    this.goopFadeBG.FadeIn(1f);
    this.normalBG.DOKill();
    DOTweenModuleUI.DOFade(this.normalBG, 1f, 2f).SetDelay<TweenerCore<Color, Color, ColorOptions>>(1f);
    PersistenceManager.PersistentData.ComicPanelIndex = 0;
    if (InputManager.General.InputIsController())
    {
      this.ShowPrompts();
      this.UpdatePrompts();
      this.frontCover.IsMouse = false;
    }
    base.OnShowStarted();
  }

  public override void OnCancelButtonInput()
  {
    if (!UIComicMenuController.AllowInput || !UIComicMenuController.IsInitialized || !this.IntroDone || this.animating)
      return;
    UIComicMenuController.ButtonPressed();
    base.OnCancelButtonInput();
    UIManager.PlayAudio("event:/ui/close_menu");
    AudioManager.Instance.StopCurrentAtmos();
    AudioManager.Instance.StopCurrentMusic();
    AudioManager.Instance.PlayMusic("event:/music/comic/comic");
    AudioManager.Instance.SetMusicParam("comic_id", 0.0f);
    this.pageIndex = Vector2Int.zero;
    this.BonusComicIndexLoaded = -1;
    this.HidePrompts();
    if (this.currentComicType == UIComicMenuController.ComicType.Simple)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      UIComicMenuController.\u003C\u003Ec__DisplayClass134_0 displayClass1340 = new UIComicMenuController.\u003C\u003Ec__DisplayClass134_0()
      {
        \u003C\u003E4__this = this,
        autoFlip = this._simpleComic.GetComponent<AutoFlip>()
      };
      // ISSUE: reference to a compiler-generated field
      displayClass1340.autoFlip.Mode = FlipMode.LeftToRight;
      if (this._simpleComic.currentPage == 0)
      {
        // ISSUE: reference to a compiler-generated method
        displayClass1340.\u003COnCancelButtonInput\u003Eg__Callback\u007C0();
      }
      else
      {
        this._simpleComicParent.ControlPromptsGroup.DOFade(0.0f, 1f);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        displayClass1340.autoFlip.StartFlipping(new System.Action(displayClass1340.\u003COnCancelButtonInput\u003Eg__Callback\u007C0));
      }
    }
    else if (this.currentComicType == UIComicMenuController.ComicType.Bonus)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      UIComicMenuController.\u003C\u003Ec__DisplayClass134_1 displayClass1341 = new UIComicMenuController.\u003C\u003Ec__DisplayClass134_1()
      {
        \u003C\u003E4__this = this,
        autoFlip = this._bonusComic.GetComponent<AutoFlip>()
      };
      // ISSUE: reference to a compiler-generated field
      displayClass1341.autoFlip.Mode = FlipMode.LeftToRight;
      if (this._bonusComic.currentPage == 0)
      {
        // ISSUE: reference to a compiler-generated method
        displayClass1341.\u003COnCancelButtonInput\u003Eg__Callback\u007C1();
      }
      else
      {
        this._bonusComicParent.ControlPromptsGroup.DOFade(0.0f, 1f);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        displayClass1341.autoFlip.StartFlipping(new System.Action(displayClass1341.\u003COnCancelButtonInput\u003Eg__Callback\u007C1));
      }
    }
    else if (this.currentComicType == UIComicMenuController.ComicType.Default)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      UIComicMenuController.\u003C\u003Ec__DisplayClass134_2 displayClass1342 = new UIComicMenuController.\u003C\u003Ec__DisplayClass134_2();
      // ISSUE: reference to a compiler-generated field
      displayClass1342.\u003C\u003E4__this = this;
      this.bonusCover.gameObject.SetActive(PersistenceManager.PersistentData.UnlockedBonusComicPages);
      this.controlPromptsGroup.alpha = 0.0f;
      if ((UnityEngine.Object) this.CurrentPage != (UnityEngine.Object) null)
        this.CurrentPage.ResetStylizer();
      if ((UnityEngine.Object) this.CurrentPage == (UnityEngine.Object) null && this.frontCover.gameObject.activeSelf)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        UIComicMenuController.\u003C\u003Ec__DisplayClass134_3 displayClass1343 = new UIComicMenuController.\u003C\u003Ec__DisplayClass134_3()
        {
          CS\u0024\u003C\u003E8__locals1 = displayClass1342,
          autoFlip = this.frontCover.GetComponent<AutoFlip>()
        };
        // ISSUE: reference to a compiler-generated field
        displayClass1343.autoFlip.Mode = FlipMode.LeftToRight;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        displayClass1343.autoFlip.StartFlipping(new System.Action(displayClass1343.\u003COnCancelButtonInput\u003Eb__3));
        this.currentComicType = UIComicMenuController.ComicType.None;
        return;
      }
      // ISSUE: reference to a compiler-generated field
      displayClass1342.temp = this.frontCover.bookPages[1];
      if ((UnityEngine.Object) this.CurrentPage != (UnityEngine.Object) null)
      {
        Sprite leftSprite;
        Sprite rightSprite;
        this.CurrentPage.SplitPages(this.CurrentPage.SquishPage(), out leftSprite, out rightSprite);
        this.frontCover.bookPages[1] = leftSprite;
        this.frontCover.RightNext.sprite = rightSprite;
        this.HidePrompts();
        UnityEngine.Object.Destroy((UnityEngine.Object) this.CurrentPage.gameObject);
      }
      this.frontCover.DOKill();
      ((RectTransform) this.frontCover.transform).anchoredPosition = new Vector2(0.0f, 0.0f);
      ((RectTransform) this.frontCover.transform).DOAnchorPos(new Vector2(-280f, 0.0f), 1f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutCirc);
      this.frontCover.transform.localScale = new Vector3(1.7225f, 1.25f, 1.425f);
      this.frontCover.transform.DOScale(Vector3.one, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutCirc);
      this.frontCover.gameObject.SetActive(true);
      this.frontCover.FlipToEnd();
      // ISSUE: reference to a compiler-generated method
      this.StartCoroutine((IEnumerator) this.WaitFor(1f, new System.Action(displayClass1342.\u003COnCancelButtonInput\u003Eb__2)));
    }
    else
    {
      this.Hide();
      if ((UnityEngine.Object) this.frontCover != (UnityEngine.Object) null)
        this.frontCover.OnFlip.RemoveListener(new UnityAction(this.OnFrontCoverOpened));
      this.CleanUpBook();
      UnityEngine.Resources.UnloadUnusedAssets();
    }
    this.currentComicType = UIComicMenuController.ComicType.None;
  }

  public override void OnHideStarted()
  {
    base.OnHideStarted();
    this.goopFadeBG.FadeOut(1.5f, 0.5f);
    DOTweenModuleUI.DOFade(this.normalBG, 0.0f, 0.5f);
  }

  public override void OnHideCompleted()
  {
    base.OnHideCompleted();
    AudioManager.Instance.PlayMusic("event:/music/menu/menu_title");
    AudioManager.Instance.StopCurrentAtmos();
    this.EnableStylizer();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void OnFrontCoverOpened()
  {
    bool openingFromFirstPage = PersistenceManager.PersistentData.ComicPageIndex == Vector2Int.zero;
    System.Action callback = (System.Action) (() =>
    {
      this.RequiresAnimating = openingFromFirstPage;
      this.ShowPage(PersistenceManager.PersistentData.ComicPageIndex, (Action<UIComicPage>) (page =>
      {
        CanvasGroup canvasGroup = this.frontCover.GetComponent<CanvasGroup>();
        canvasGroup.DOFade(0.0f, 0.25f).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
        {
          canvasGroup.alpha = 1f;
          this.frontCover.gameObject.SetActive(false);
          this.animating = false;
          this.UpdatePrompts();
        }));
      }));
    });
    if (openingFromFirstPage)
    {
      ((RectTransform) this.frontCover.transform).DOAnchorPos(new Vector2(-970f, -920f), 1f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutCirc);
      this.frontCover.transform.DOScale(new Vector3(3.5f, 3.4f, 3.5f), 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutCirc).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => callback()));
    }
    else
      callback();
  }

  public void BookOpened(UIComicMenuController.ComicType comicType)
  {
    if (this.IsShowingBonus)
      comicType = UIComicMenuController.ComicType.Bonus;
    this.currentComicType = comicType;
    this.animating = true;
    this.comicPromptHolderCanvasGroup.DOKill();
    this.comicPromptHolderCanvasGroup.alpha = 0.0f;
    if (this.IsShowingBonus)
      this.controlPromptsGroup.transform.parent.gameObject.SetActive(false);
    else
      this.controlPromptsGroup.transform.parent.gameObject.SetActive(true);
    this._uiComicMenu.Hide();
    this.isShowingBonus = false;
    switch (comicType)
    {
      case UIComicMenuController.ComicType.Default:
        this.frontCover.OnFlip.AddListener(new UnityAction(this.OnFrontCoverOpened));
        this.frontCover.Animate = true;
        this.pageIndex = PersistenceManager.PersistentData.ComicPageIndex;
        if (PersistenceManager.PersistentData.ComicPageIndex.x != 0)
        {
          this.frontCover.GetBookPage(2);
          this.frontCover.Animate = false;
          this.frontCover.gameObject.SetActive(false);
          this.controlPrompts.SetActive(false);
          this.RequiresAnimating = false;
          this.RequiresCombining = false;
          this.ShowPage(PersistenceManager.PersistentData.ComicPageIndex, (Action<UIComicPage>) (page =>
          {
            this._uiComicMenu.CanvasGroup.alpha = 0.0f;
            Sprite temp1 = this.frontCover.bookPages[1];
            Sprite temp2 = this.frontCover.bookPages[2];
            Sprite leftSprite;
            Sprite rightSprite;
            this.CurrentPage.SplitPages(this.CurrentPage.SquishPage(), out leftSprite, out rightSprite);
            this.frontCover.bookPages[1] = leftSprite;
            this.frontCover.bookPages[2] = rightSprite;
            page.StopAllCoroutines();
            UnityEngine.Object.Destroy((UnityEngine.Object) page.gameObject);
            this._uiComicMenu.CanvasGroup.alpha = 1f;
            this.frontCover.gameObject.SetActive(true);
            this.frontCover.ResetPosition((System.Action) (() =>
            {
              ((RectTransform) this.frontCover.transform).DOAnchorPos((Vector2) Vector2Int.zero, 1f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutCirc);
              this.frontCover.transform.DOScale(new Vector3(1.72f, 1.26f, 0.5f), 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutCirc).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.5f);
              this.frontCover.FlipRightPage();
              this.StartCoroutine((IEnumerator) this.WaitFor(1.5f, (System.Action) (() =>
              {
                this.frontCover.bookPages[1] = temp1;
                this.frontCover.bookPages[2] = temp2;
              })));
            }));
          }));
          break;
        }
        this.frontCover.ResetPosition((System.Action) (() =>
        {
          this.frontCover.Animate = false;
          this.frontCover.FlipRightPage();
        }));
        break;
      case UIComicMenuController.ComicType.Simple:
        this.bonusCover.gameObject.SetActive(false);
        this.frontCover.ResetPosition((System.Action) (() =>
        {
          this.frontCover.gameObject.SetActive(false);
          this._simpleComicParent.Show(true);
          this._simpleComic.IntroDone();
          this._simpleComic.transform.DOScale(Vector3.one * 1.15f, 1f);
          this.animating = false;
        }));
        break;
      case UIComicMenuController.ComicType.Bonus:
        this.frontCover.gameObject.SetActive(false);
        this.isShowingBonus = true;
        this._bonusComicParent.IsShowingBonus = true;
        this._bonusComic.transform.DOScale(Vector3.one * 1.15f, 1f);
        this.bonusCover.ResetPosition((System.Action) (() =>
        {
          this.bonusCover.gameObject.SetActive(false);
          this._bonusComicParent.Show(true);
          this._bonusComic.IntroDone();
          this.animating = false;
        }));
        break;
    }
  }

  public void RightPage()
  {
    if (this.IsShowingBonus)
    {
      ++this.pageIndex.x;
      if (this.BonusComicIndexLoaded >= this.pageIndex.x)
        return;
    }
    else
    {
      this.pageIndex.x = Mathf.Clamp(PersistenceManager.PersistentData.ComicPageIndex.x + 1, 0, this.comicPages.Count - 1);
      this.pageIndex.y = PersistenceManager.PersistentData.ComicPageIndex.y;
      PersistenceManager.PersistentData.ComicPanelIndex = 0;
      PersistenceManager.PersistentData.ComicExploredHistory = this.pageIndex.x >= PersistenceManager.PersistentData.ComicExploredHistory.x ? this.pageIndex : PersistenceManager.PersistentData.ComicExploredHistory;
    }
    if ((UnityEngine.Object) this.GetPage(this.pageIndex) != (UnityEngine.Object) null)
    {
      if (this.currentComicType == UIComicMenuController.ComicType.Default && this.pageIndex.x >= 50)
        this.StartCoroutine((IEnumerator) this.ComicBookCompleted());
      else
        this.ShowPage(this.pageIndex);
    }
    else
    {
      if (this.IsShowingBonus)
        return;
      this.pageIndex.y = 0;
      for (int index = 0; index < 10; ++index)
      {
        ++PersistenceManager.PersistentData.ComicPageIndex.x;
        this.pageIndex.x = PersistenceManager.PersistentData.ComicPageIndex.x;
        if ((UnityEngine.Object) this.GetPage(this.pageIndex) != (UnityEngine.Object) null)
        {
          this.ShowPage(this.pageIndex);
          break;
        }
      }
    }
  }

  public void LeftPage()
  {
    int index1 = PersistenceManager.PersistentData.ComicExploredPages.IndexOf(this.pageIndex);
    PersistenceManager.PersistentData.ComicExploredPages.RemoveRange(index1, PersistenceManager.PersistentData.ComicExploredPages.Count - index1);
    this.pageIndex = PersistenceManager.PersistentData.ComicExploredPages[PersistenceManager.PersistentData.ComicExploredPages.Count - 1];
    this.RequiresLastPanel = true;
    if ((UnityEngine.Object) this.GetPage(this.pageIndex) != (UnityEngine.Object) null)
    {
      this.ShowPage(this.pageIndex, (Action<UIComicPage>) (page => { }));
    }
    else
    {
      this.pageIndex.y = 0;
      for (int index2 = 0; index2 < 10; ++index2)
      {
        --PersistenceManager.PersistentData.ComicPageIndex.x;
        this.pageIndex.x = PersistenceManager.PersistentData.ComicPageIndex.x;
        if ((UnityEngine.Object) this.GetPage(this.pageIndex) != (UnityEngine.Object) null)
        {
          this.ShowPage(this.pageIndex);
          break;
        }
      }
    }
  }

  public IEnumerator ComicBookCompleted()
  {
    UIComicMenuController comicMenuController = this;
    yield return (object) new WaitForEndOfFrame();
    comicMenuController.bonusCover.gameObject.SetActive(false);
    comicMenuController.frontCover.OnFlip.RemoveListener(new UnityAction(comicMenuController.OnFrontCoverOpened));
    Sprite rightSprite = (Sprite) null;
    Sprite leftSprite;
    comicMenuController.CurrentPage.SplitPages(comicMenuController.CurrentPage.SquishPage(), out leftSprite, out rightSprite);
    comicMenuController.CurrentPage.gameObject.SetActive(false);
    Sprite temp1 = comicMenuController.frontCover.bookPages[1];
    Sprite temp2 = comicMenuController.frontCover.bookPages[2];
    comicMenuController.frontCover.LeftNext.sprite = leftSprite;
    comicMenuController.frontCover.RightNext.sprite = rightSprite;
    comicMenuController.frontCover.bookPages[1] = leftSprite;
    comicMenuController.frontCover.bookPages[2] = rightSprite;
    comicMenuController.frontCover.gameObject.SetActive(true);
    UnityEngine.Object.Destroy((UnityEngine.Object) comicMenuController.CurrentPage.gameObject);
    comicMenuController.CurrentPage = (UIComicPage) null;
    yield return (object) new WaitForEndOfFrame();
    UIComicPage page = (UIComicPage) null;
    comicMenuController.ShowPage(comicMenuController.PageIndex, (Action<UIComicPage>) (p => page = p));
    while ((UnityEngine.Object) page == (UnityEngine.Object) null)
      yield return (object) null;
    PersistenceManager.PersistentData.ComicPageIndex = Vector2Int.zero;
    PersistenceManager.PersistentData.ComicPanelIndex = 0;
    PersistenceManager.PersistentData.ComicExploredPages.Clear();
    PersistenceManager.Save();
    comicMenuController.controlPrompts.gameObject.SetActive(false);
    comicMenuController.frontCover.gameObject.SetActive(false);
    RectTransform transform = page.Panels[0].Panel.Segments[0].Segment.transform as RectTransform;
    UIComicPage uiComicPage = page;
    Rect rect = transform.rect;
    int width = (int) ((double) rect.width / 2.0);
    rect = transform.rect;
    int height = (int) rect.height;
    Sprite sprite = uiComicPage.SquishPage(width, height);
    comicMenuController.frontCover.bookPages[3] = sprite;
    comicMenuController.frontCover.bookPages[4] = comicMenuController.emptyPage;
    comicMenuController.previousPrompt.transform.parent.gameObject.SetActive(false);
    comicMenuController.controlPrompts.gameObject.SetActive(true);
    comicMenuController.frontCover.gameObject.SetActive(true);
    comicMenuController.frontCover.FlipRightPage();
    comicMenuController.frontCover.bookPages[1] = temp1;
    comicMenuController.frontCover.bookPages[2] = temp2;
    ((RectTransform) comicMenuController.frontCover.transform).DOAnchorPos(new Vector2(280f, 0.0f), 1f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutSine);
    comicMenuController.frontCover.transform.DOScale(Vector3.one, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    UnityEngine.Object.Destroy((UnityEngine.Object) page.gameObject);
    yield return (object) new WaitForSeconds(2f);
    comicMenuController.animating = false;
    bool flag;
    while (true)
    {
      flag = InputManager.General.InputIsController() ? InputManager.UI.GetPageNavigateRightDown() : InputManager.UI.GetAcceptButtonDown() || InputManager.Gameplay.GetInteractButtonDown();
      if (!(InputManager.UI.GetCancelButtonDown() | flag) || !UIComicMenuController.AllowInput)
        yield return (object) null;
      else
        break;
    }
    if (flag)
    {
      comicMenuController.OnCancelButtonInput();
      UIComicMenuController.ButtonPressed();
    }
  }

  public void ShowPage(Vector2Int index, Action<UIComicPage> callback = null)
  {
    if (UIComicMenuController.IsLoadingPage)
      return;
    if (!this.IsShowingBonus)
    {
      PersistenceManager.PersistentData.ComicPageIndex = index;
      if (!PersistenceManager.PersistentData.ComicExploredPages.Contains(index))
        PersistenceManager.PersistentData.ComicExploredPages.Add(index);
      if (index.x >= 15)
        PersistenceManager.PersistentData.RevealedJalalasBag = true;
      PersistenceManager.Save();
    }
    else
      --index.x;
    int num1 = Mathf.RoundToInt((float) ((double) index.x / (this.IsShowingBonus ? 15.0 : 50.0) * 100.0));
    this.pageNumber.text = string.Format(LocalizationManager.GetTranslation("Comic/Page"), (object) num1);
    UIComicMenuController.IsLoadingPage = true;
    this.LoadPage(this.GetPage(index), index, (Action<GameObject>) (r =>
    {
      UIComicMenuController.IsLoadingPage = false;
      for (int index1 = this.pageContainer.childCount - 1; index1 >= 0; --index1)
      {
        UIComicPage component = this.pageContainer.GetChild(index1).GetComponent<UIComicPage>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          component.gameObject.SetActive(false);
      }
      this.UnloadPage(index + Vector2Int.left * 2);
      this.UnloadPage(index + Vector2Int.right * 2);
      int num2 = 3;
      for (int y = 0; y < num2; ++y)
      {
        Vector2Int pageIndex1 = new Vector2Int(index.x - 2, y);
        Vector2Int pageIndex2 = new Vector2Int(index.x + 2, y);
        this.UnloadPage(pageIndex1);
        this.UnloadPage(pageIndex2);
      }
      UIComicPage component1 = UnityEngine.Object.Instantiate<GameObject>(r, this.pageContainer).GetComponent<UIComicPage>();
      component1.transform.SetAsFirstSibling();
      if (this.IsShowingBonus)
      {
        Sprite rightSprite = (Sprite) null;
        int num3 = index.x * 2;
        RectTransform transform = component1.Panels[0].Panel.Segments[0].Segment.transform as RectTransform;
        UIComicPage uiComicPage = component1;
        Rect rect = transform.rect;
        int width = (int) rect.width;
        rect = transform.rect;
        int height = (int) rect.height;
        Sprite sprite = uiComicPage.SquishPage(width, height);
        this._bonusComic.bookPages[this._bonusComic.bookPages.Length - 1] = sprite;
        Sprite leftSprite;
        component1.SplitPages(sprite, out leftSprite, out rightSprite);
        this._bonusComic.Right.sprite = leftSprite;
        this._bonusComic.RightNext.sprite = rightSprite;
        this._bonusComic.bookPages[num3 + 1] = leftSprite;
        this._bonusComic.bookPages[num3 + 2] = rightSprite;
        component1.gameObject.SetActive(false);
        this.BonusComicIndexLoaded = index.x;
      }
      for (int index2 = this.pageContainer.childCount - 1; index2 >= 0; --index2)
      {
        UIComicPage p = this.pageContainer.GetChild(index2).GetComponent<UIComicPage>();
        if ((UnityEngine.Object) p != (UnityEngine.Object) null && (UnityEngine.Object) p != (UnityEngine.Object) component1)
        {
          if (!this.IsShowingBonus)
          {
            p.gameObject.SetActive(true);
            p.FadePanels();
            p.CanvasGroup.DOFade(0.0f, 0.2f).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => UnityEngine.Object.Destroy((UnityEngine.Object) p.gameObject)));
          }
          else
            UnityEngine.Object.Destroy((UnityEngine.Object) p.gameObject);
        }
      }
      Action<UIComicPage> action = callback;
      if (action == null)
        return;
      action(component1);
    }));
    if (index.x - 1 > 0)
      this.LoadPage(this.GetPage(new Vector2Int(index.x - 1, 0)), new Vector2Int(index.x - 1, 0), (Action<GameObject>) null);
    if ((this.isShowingBonus ? (index.x + 1 < this.bonusPages.Count ? 1 : 0) : (index.x + 1 < this.comicPages.Count ? 1 : 0)) == 0)
      return;
    this.LoadPage(this.GetPage(new Vector2Int(index.x + 1, 0)), new Vector2Int(index.x + 1, 0), (Action<GameObject>) null);
  }

  public void LoadPage(ComicPageData pageData, Vector2Int pageIndex, Action<GameObject> callback)
  {
    if ((UnityEngine.Object) pageData == (UnityEngine.Object) null)
      return;
    ComicPageData page = this.GetPage(pageIndex + Vector2Int.up);
    if ((UnityEngine.Object) page != (UnityEngine.Object) null && !this.loadedPages.ContainsKey(pageIndex + Vector2Int.up))
      this.LoadPage(page, pageIndex + Vector2Int.up, (Action<GameObject>) null);
    if (!this.loadedPages.ContainsKey(pageIndex))
    {
      AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>((object) pageData.Page);
      asyncOperationHandle.Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        Action<GameObject> action = callback;
        if (action != null)
          action(obj.Result);
        this.loadedPages.Add(pageIndex, obj);
      });
      asyncOperationHandle.WaitForCompletion();
    }
    else
    {
      Action<GameObject> action = callback;
      if (action == null)
        return;
      action(this.loadedPages[pageIndex].Result);
    }
  }

  public void UnloadPage(Vector2Int pageIndex)
  {
    if (!this.loadedPages.ContainsKey(pageIndex))
      return;
    Addressables.Release<GameObject>(this.loadedPages[pageIndex]);
    this.loadedPages.Remove(pageIndex);
  }

  public ComicPageData GetPage(Vector2Int pageIndex)
  {
    foreach (ComicPageData page in new List<ComicPageData>(this.IsShowingBonus ? (IEnumerable<ComicPageData>) this.bonusPages : (IEnumerable<ComicPageData>) this.comicPages))
    {
      string oldValue = this.IsShowingBonus ? "Bonus Comic Page " : "Comic Page ";
      string s = page.name.Replace(oldValue, "");
      int num = 0;
      for (int index = 0; index < s.Length; ++index)
      {
        if (s[index] == '-')
        {
          num = int.Parse(s.Substring(index + 1, s.Length - (index + 1)));
          s = s.Remove(index, s.Length - index);
        }
      }
      if (int.Parse(s) == pageIndex.x + 1 && num == pageIndex.y)
        return page;
    }
    return (ComicPageData) null;
  }

  public void ShowPrompts()
  {
    if (this.IsShowingBonus)
    {
      this.controlPromptsGroup.gameObject.SetActive(false);
    }
    else
    {
      this.controlPromptsGroup.gameObject.SetActive(true);
      if (!SettingsManager.Settings.Accessibility.HighContrastText)
        this.controlPromptsGroup.alpha = 0.5f;
      else
        this.controlPromptsGroup.alpha = 1f;
    }
  }

  public void HidePrompts() => this.controlPromptsGroup.gameObject.SetActive(false);

  public void UpdatePrompts()
  {
    for (int index = 0; index < this.controlPromptImages.Length; ++index)
    {
      Image controlPromptImage = this.controlPromptImages[index];
      Color imagesWhiteColour = this.controlPromptImagesWhiteColours[index];
      if ((UnityEngine.Object) this.CurrentPage != (UnityEngine.Object) null)
      {
        if (this.CurrentPage.BackgroundIsLight)
          controlPromptImage.color = this.InvertPromptsColouring ? imagesWhiteColour : new Color(1f - imagesWhiteColour.r, 1f - imagesWhiteColour.g, 1f - imagesWhiteColour.b, controlPromptImage.color.a);
        else
          controlPromptImage.color = this.InvertPromptsColouring ? new Color(1f - imagesWhiteColour.r, 1f - imagesWhiteColour.g, 1f - imagesWhiteColour.b, controlPromptImage.color.a) : imagesWhiteColour;
      }
    }
    for (int index = 0; index < this.controlPromptText.Length; ++index)
    {
      TMP_Text tmpText = this.controlPromptText[index];
      Color promptTextWhiteColour = this.controlPromptTextWhiteColours[index];
      if ((UnityEngine.Object) this.CurrentPage != (UnityEngine.Object) null)
      {
        if (this.CurrentPage.BackgroundIsLight)
          tmpText.color = this.InvertPromptsColouring ? promptTextWhiteColour : new Color(1f - promptTextWhiteColour.r, 1f - promptTextWhiteColour.g, 1f - promptTextWhiteColour.b, tmpText.color.a);
        else
          tmpText.color = this.InvertPromptsColouring ? new Color(1f - promptTextWhiteColour.r, 1f - promptTextWhiteColour.g, 1f - promptTextWhiteColour.b, tmpText.color.a) : promptTextWhiteColour;
      }
    }
    this.controlPromptPreviousContainer.gameObject.SetActive(PersistenceManager.PersistentData.ComicPageIndex.x > 0);
  }

  public void DisableStylizer()
  {
    float f = 1f;
    this.stylizerTween = (Tween) DOTween.To((DOGetter<float>) (() => this.stylizer.EffectIntensity), (DOSetter<float>) (x => f = x), 0.0f, 2f).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => this.stylizer.EffectIntensity = f)).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
  }

  public void EnableStylizer()
  {
    float f = 0.0f;
    this.stylizerTween = (Tween) DOTween.To((DOGetter<float>) (() => this.stylizer.EffectIntensity), (DOSetter<float>) (x => f = x), 1f, 2f).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => this.stylizer.EffectIntensity = f)).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
  }

  public new void OnDisable()
  {
    this.stylizer.EffectIntensity = 1f;
    this.StopAllCoroutines();
  }

  public void OnActiveControllerChanged(Controller controller) => this.UpdateInputPrompts();

  public void UpdateInputPrompts()
  {
    if (InputManager.General.InputIsController())
    {
      this.nextPrompt.Category = 1;
      this.nextPrompt.Action = 44;
      this.previousPrompt.Category = 1;
      this.previousPrompt.Action = 43;
    }
    else
    {
      this.nextPrompt.Category = 0;
      this.nextPrompt.Action = 2;
      this.previousPrompt.Category = 0;
      this.previousPrompt.Action = 68;
    }
    this.nextPrompt.ForceUpdate();
    this.previousPrompt.ForceUpdate();
  }

  public IEnumerator WaitFor(float duration, System.Action callback)
  {
    yield return (object) new WaitForSeconds(duration);
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void IncreasePageIndex() => ++this.pageIndex.x;

  public void DecreasePageIndex() => --this.pageIndex.x;

  [CompilerGenerated]
  public void \u003CDoneIntro\u003Eb__129_0() => this.IntroDone = true;

  [CompilerGenerated]
  public void \u003CBookOpened\u003Eb__138_3(UIComicPage page)
  {
    this._uiComicMenu.CanvasGroup.alpha = 0.0f;
    Sprite temp1 = this.frontCover.bookPages[1];
    Sprite temp2 = this.frontCover.bookPages[2];
    Sprite leftSprite;
    Sprite rightSprite;
    this.CurrentPage.SplitPages(this.CurrentPage.SquishPage(), out leftSprite, out rightSprite);
    this.frontCover.bookPages[1] = leftSprite;
    this.frontCover.bookPages[2] = rightSprite;
    page.StopAllCoroutines();
    UnityEngine.Object.Destroy((UnityEngine.Object) page.gameObject);
    this._uiComicMenu.CanvasGroup.alpha = 1f;
    this.frontCover.gameObject.SetActive(true);
    this.frontCover.ResetPosition((System.Action) (() =>
    {
      ((RectTransform) this.frontCover.transform).DOAnchorPos((Vector2) Vector2Int.zero, 1f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutCirc);
      this.frontCover.transform.DOScale(new Vector3(1.72f, 1.26f, 0.5f), 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutCirc).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.5f);
      this.frontCover.FlipRightPage();
      this.StartCoroutine((IEnumerator) this.WaitFor(1.5f, (System.Action) (() =>
      {
        this.frontCover.bookPages[1] = temp1;
        this.frontCover.bookPages[2] = temp2;
      })));
    }));
  }

  [CompilerGenerated]
  public void \u003CBookOpened\u003Eb__138_0()
  {
    this.frontCover.Animate = false;
    this.frontCover.FlipRightPage();
  }

  [CompilerGenerated]
  public void \u003CBookOpened\u003Eb__138_1()
  {
    this.frontCover.gameObject.SetActive(false);
    this._simpleComicParent.Show(true);
    this._simpleComic.IntroDone();
    this._simpleComic.transform.DOScale(Vector3.one * 1.15f, 1f);
    this.animating = false;
  }

  [CompilerGenerated]
  public void \u003CBookOpened\u003Eb__138_2()
  {
    this.bonusCover.gameObject.SetActive(false);
    this._bonusComicParent.Show(true);
    this._bonusComic.IntroDone();
    this.animating = false;
  }

  public enum ComicType
  {
    Default,
    Simple,
    Bonus,
    None,
  }

  public enum Language
  {
    English,
    Japanese,
    Russian,
    French,
    German,
    Spanish,
    Portuguese,
    Chinese_Simplified,
    Chinese_Traditional,
    Korean,
    Italian,
    Dutch,
    Turkish,
    FrenchCanadian,
    Arabic,
  }
}
