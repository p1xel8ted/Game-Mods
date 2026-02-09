// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MainMenu.UIMainMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using MMTools;
using src.Extensions;
using src.Managers;
using src.UI;
using src.UI.InfoCards;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unify;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Lamb.UI.MainMenu;

public class UIMainMenuController : UIMenuBase
{
  public static UIMainMenuController Instance;
  [Header("Menus")]
  [SerializeField]
  public Lamb.UI.MainMenu.MainMenu _mainMenu;
  [SerializeField]
  public LoadMenu _loadMenu;
  [SerializeField]
  public DeleteMenu _deleteMenu;
  [SerializeField]
  public LoadMenu _dlcMenu;
  [Header("Other")]
  [SerializeField]
  public UIMenuControlPrompts _controlPrompts;
  [SerializeField]
  public UINavigatorFollowElement _navigatorHighlight;
  [SerializeField]
  public CanvasGroup ad;
  [SerializeField]
  public RectTransform focus;
  [SerializeField]
  public MainMenuController _menuController;
  [SerializeField]
  public Camera _camera;
  [SerializeField]
  public Camera _comicCamera;
  [SerializeField]
  public SaveInfoCardController _infoCardController;
  [SerializeField]
  public GameObject _keyArt;
  [SerializeField]
  public CameraSubtleMovementOnInput _cameraMovement;
  [Header("Sidebar")]
  [SerializeField]
  public MMButton _discordButton;
  [SerializeField]
  public MMButton _comicButton;
  [SerializeField]
  public GameObject _comicAlert;
  public UIMenuBase _currentMenu;

  public Lamb.UI.MainMenu.MainMenu MainMenu => this._mainMenu;

  public LoadMenu LoadMenu => this._loadMenu;

  public DeleteMenu DeleteMenu => this._deleteMenu;

  public LoadMenu DLCMenu => this._dlcMenu;

  public Camera ComicCamera => this._comicCamera;

  public override void Awake()
  {
    base.Awake();
    UIMainMenuController.Instance = this;
    SaveInfoCardController infoCardController = this._infoCardController;
    infoCardController.OnInfoCardShown = infoCardController.OnInfoCardShown + new Action<SaveInfoCard>(this.OnInfoCardShown);
    this._comicCamera.gameObject.SetActive(false);
    this._controlPrompts.HideCancelButton();
    this._controlPrompts.HideAcceptButton();
    Lamb.UI.MainMenu.MainMenu mainMenu = this._mainMenu;
    mainMenu.OnShow = mainMenu.OnShow + (System.Action) (() => this._controlPrompts.ShowAcceptButton());
    MMConversation.CURRENT_CONVERSATION = (ConversationObject) null;
  }

  public void OnInfoCardShown(SaveInfoCard infoCard)
  {
    this.StartCoroutine((IEnumerator) this.FrameWait((System.Action) (() =>
    {
      if ((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance != (UnityEngine.Object) null && MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable != null)
      {
        SaveSlotButtonBase component = MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable.GetComponent<SaveSlotButtonBase>();
        if ((component != null ? (component.SaveIndex >= 10 ? 1 : 0) : 0) != 0)
        {
          infoCard.ShowBackUpPrompt();
          return;
        }
      }
      infoCard.HideBackUpPrompt();
    })));
  }

  public IEnumerator FrameWait(System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void Start()
  {
    this._currentMenu = (UIMenuBase) this._mainMenu;
    UIMenuBase.ActiveMenus.Add((UIMenuBase) this);
    if (CheatConsole.IN_DEMO)
      DemoWatermark.Play();
    this._mainMenu.OnPlayButtonPressed += (System.Action) (() =>
    {
      this.ad.DOFade(0.0f, 0.25f);
      this.SetActiveStateForMenu(this.ad.gameObject, false);
      if (CheatConsole.IN_DEMO)
      {
        SaveAndLoad.SAVE_SLOT = 10;
        AudioManager.Instance.StopCurrentMusic();
        UIManager.PlayAudio("event:/ui/heretics_defeated");
        DifficultyManager.ForceDifficulty(DifficultyManager.Difficulty.Medium);
        DeviceLightingManager.Reset();
        FollowerManager.Reset();
        SimulationManager.Pause();
        StructureManager.Reset();
        TwitchManager.Abort();
        UIDynamicNotificationCenter.Reset();
        GameManager.GoG_Initialised = false;
        MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, "Game Biome Intro", 5f, "", (System.Action) (() =>
        {
          SaveAndLoad.ResetSave(SaveAndLoad.SAVE_SLOT, true);
          DataManager.Instance.SetTutorialVariables();
        }));
        this.SetActiveStateForMenu(false);
        MonoSingleton<UINavigatorNew>.Instance.Clear();
      }
      else
      {
        this.PerformMenuTransition((UISubmenuBase) this._mainMenu, (UISubmenuBase) this._loadMenu);
        this._controlPrompts.ShowCancelButton();
        this._controlPrompts.ShowAcceptButton();
        this._menuController.HideBlueTheme(1f, 0.0f);
      }
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        if (!player.isLamb)
          CoopManager.RemoveCoopPlayerStatic(player, disengagePlayer: true, withDelay: false);
        UnityEngine.Object.Destroy((UnityEngine.Object) player.gameObject);
      }
      PlayerFarming.players.Clear();
      CoopManager.RemovePlayerFromMenu();
      PlayerFarming.playersCount = 0;
      CoopManager.CoopActive = false;
    });
    this._loadMenu.OnBackButtonPressed += (System.Action) (() =>
    {
      this.PerformMenuTransition((UISubmenuBase) this._loadMenu, (UISubmenuBase) this._mainMenu);
      this._controlPrompts.HideCancelButton();
      this.ad.DOFade(1f, 0.25f).SetDelay<TweenerCore<float, float, FloatOptions>>(0.25f);
      this.SetActiveStateForMenu(this.ad.gameObject, true);
      this._controlPrompts.ShowAcceptButton();
      this.StartCoroutine((IEnumerator) this.WaitForDLCCheck());
      this._menuController.ShowBlueTheme(1f, 0.0f);
    });
    this._loadMenu.OnDeleteButtonPressed += (System.Action) (() => this.PerformMenuTransition((UISubmenuBase) this._loadMenu, (UISubmenuBase) this._deleteMenu));
    this._deleteMenu.OnBackButtonPressed += (System.Action) (() => this.PerformMenuTransition((UISubmenuBase) this._deleteMenu, (UISubmenuBase) this._loadMenu));
    this._mainMenu.OnDLCButtonPressed += (System.Action) (() =>
    {
      this.ad.DOFade(0.0f, 0.25f);
      this.SetActiveStateForMenu(this.ad.gameObject, false);
      this.PerformMenuTransition((UISubmenuBase) this._mainMenu, (UISubmenuBase) this._dlcMenu);
      this._controlPrompts.ShowCancelButton();
      this._controlPrompts.ShowAcceptButton();
    });
    this._dlcMenu.OnBackButtonPressed += (System.Action) (() =>
    {
      this.PerformMenuTransition((UISubmenuBase) this._dlcMenu, (UISubmenuBase) this._mainMenu);
      this._controlPrompts.HideCancelButton();
      this._controlPrompts.ShowAcceptButton();
      this.ad.DOFade(1f, 0.25f).SetDelay<TweenerCore<float, float, FloatOptions>>(0.25f);
      this.SetActiveStateForMenu(this.ad.gameObject, true);
      this.StartCoroutine((IEnumerator) this.WaitForDLCCheck());
    });
    this._discordButton.onClick.AddListener((UnityAction) (() => Application.OpenURL("https://discord.com/invite/massivemonster")));
    this._comicButton.onClick.AddListener((UnityAction) (() =>
    {
      this._cameraMovement.Reset();
      this._cameraMovement.enabled = false;
      UIComicMenuController menu = MonoSingleton<UIManager>.Instance.ComicMenuTemplate.Instantiate<UIComicMenuController>();
      this._camera.gameObject.SetActive(false);
      this._comicCamera.gameObject.SetActive(true);
      this._keyArt.gameObject.SetActive(false);
      menu.GetComponent<Canvas>().worldCamera = this._comicCamera;
      menu.ShowComic(false);
      this.PushInstance<UIComicMenuController>(menu);
      PersistenceManager.PersistentData.OpenedComic = true;
      PersistenceManager.Save();
      this._comicButton.TargetGraphics[0].gameObject.SetActive(false);
      UIComicMenuController comicMenuController = menu;
      comicMenuController.OnHide = comicMenuController.OnHide + (System.Action) (() =>
      {
        this._cameraMovement.enabled = true;
        this._camera.gameObject.SetActive(true);
        this._comicCamera.gameObject.SetActive(false);
        this._keyArt.gameObject.SetActive(true);
      });
    }));
    this.StartCoroutine((IEnumerator) this.WaitForDLCCheck());
  }

  public IEnumerator WaitForDLCCheck()
  {
    yield return (object) new WaitUntil((Func<bool>) (() => (UnityEngine.Object) SessionManager.instance != (UnityEngine.Object) null && SessionManager.instance.HasStarted));
    yield return (object) GameManager.WaitForTime(0.02f, (System.Action) null);
    this._comicAlert.gameObject.SetActive(PersistenceManager.PersistentData.RevealedComic && !PersistenceManager.PersistentData.OpenedComic);
    this._comicButton.gameObject.SetActive(GameManager.AuthenticatePilgrimDLC());
  }

  public void PerformMenuTransition(UISubmenuBase from, UISubmenuBase to)
  {
    this._currentMenu = (UIMenuBase) to;
    from.Hide();
    to.Show();
  }

  public override void OnPush() => this._navigatorHighlight.enabled = false;

  public override void OnRelease() => this._navigatorHighlight.enabled = true;

  public override void OnDestroy()
  {
    base.OnDestroy();
    UIMainMenuController.Instance = (UIMainMenuController) null;
    SaveInfoCardController infoCardController = this._infoCardController;
    infoCardController.OnInfoCardShown = infoCardController.OnInfoCardShown - new Action<SaveInfoCard>(this.OnInfoCardShown);
    UIMenuBase.ActiveMenus.Remove((UIMenuBase) this);
  }

  public IEnumerator PreloadMetadata(bool backUpSaves = false)
  {
    UIMainMenuController mainMenuController = this;
    COTLDataReadWriter<MetaData> metaDataPreloader = new COTLDataReadWriter<MetaData>();
    COTLDataReadWriter<DataManager> saveDataLoader = new COTLDataReadWriter<DataManager>();
    List<int> metaErrors = new List<int>();
    int slot = 0;
    while (slot < 5)
    {
      int currentSlot = backUpSaves ? slot + 10 : slot;
      string filename = SaveAndLoad.MakeMetaSlot(currentSlot);
      if (SaveAndLoad.SaveExist(currentSlot))
      {
        bool loaded = false;
        MetaData mData = new MetaData();
        if (metaDataPreloader.FileExists(filename))
        {
          COTLDataReadWriter<MetaData> cotlDataReadWriter1 = metaDataPreloader;
          cotlDataReadWriter1.OnReadCompleted = cotlDataReadWriter1.OnReadCompleted + (Action<MetaData>) (metaData =>
          {
            mData = metaData;
            loaded = true;
          });
          COTLDataReadWriter<MetaData> cotlDataReadWriter2 = metaDataPreloader;
          cotlDataReadWriter2.OnReadError = cotlDataReadWriter2.OnReadError + (Action<MMReadWriteError>) (readWriteError =>
          {
            metaErrors.Add(currentSlot);
            loaded = true;
          });
          metaDataPreloader.Read(filename);
        }
        else
        {
          metaErrors.Add(slot);
          loaded = true;
        }
        while (!loaded)
          yield return (object) null;
        if (!metaErrors.Contains(currentSlot))
        {
          mainMenuController._loadMenu.SetupSlot(currentSlot, mData);
          if (!backUpSaves)
            mainMenuController._deleteMenu.SetupSlot(currentSlot, mData);
        }
        else if (!backUpSaves)
        {
          mainMenuController._loadMenu.SetupSlot(currentSlot);
          mainMenuController._deleteMenu.SetupSlot(currentSlot);
        }
        metaDataPreloader.OnWriteCompleted = (System.Action) null;
        metaDataPreloader.OnReadCompleted = (Action<MetaData>) null;
        metaDataPreloader.OnReadError = (Action<MMReadWriteError>) null;
        saveDataLoader.OnReadCompleted = (Action<DataManager>) null;
      }
      else if (!backUpSaves)
      {
        mainMenuController._loadMenu.SetupSlot(currentSlot);
        mainMenuController._deleteMenu.SetupSlot(currentSlot);
      }
      ++slot;
    }
    if (metaErrors.Count > 0)
      yield return (object) mainMenuController.RestoreFiles(metaErrors, metaDataPreloader, saveDataLoader);
    if (!backUpSaves)
      yield return (object) mainMenuController.StartCoroutine((IEnumerator) mainMenuController.PreloadMetadata(true));
  }

  public IEnumerator RestoreFiles(
    List<int> markedForRestoration,
    COTLDataReadWriter<MetaData> metaDataPreloader,
    COTLDataReadWriter<DataManager> saveDataLoader)
  {
    UIMainMenuController mainMenuController = this;
    UIMenuConfirmationWindow menu1 = MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate.Instantiate<UIMenuConfirmationWindow>();
    menu1.Configure(ScriptLocalization.UI_Popups_Error.GenericHeader, ScriptLocalization.UI_Popups_Error.LoadingErrorDescription, true);
    menu1.Show();
    mainMenuController.PushInstance<UIMenuConfirmationWindow>(menu1);
    yield return (object) menu1.YieldUntilHide();
    UILoadingOverlayController loadingOverlayController = mainMenuController.Push<UILoadingOverlayController>(MonoSingleton<UIManager>.Instance.LoadingOverlayControllerTemplate);
    yield return (object) loadingOverlayController.YieldUntilShown();
    int filesNotRestored = 0;
    bool loaded = true;
    foreach (int slot in markedForRestoration)
    {
      loaded = false;
      loadingOverlayController.Message = string.Format(ScriptLocalization.UI_Popups_SaveRestore.RestoringProgress, (object) (slot + 1), (object) markedForRestoration.Count, (object) 1, (object) 4);
      yield return (object) new WaitForSecondsRealtime(0.05f);
      string metaSlot = SaveAndLoad.MakeMetaSlot(slot);
      if (metaDataPreloader.FileExists(metaSlot))
        metaDataPreloader.Delete(metaSlot);
      COTLDataReadWriter<MetaData> cotlDataReadWriter1 = metaDataPreloader;
      cotlDataReadWriter1.OnWriteCompleted = cotlDataReadWriter1.OnWriteCompleted + (System.Action) (() => loaded = true);
      yield return (object) new WaitForSecondsRealtime(0.25f);
      string saveSlot = SaveAndLoad.MakeSaveSlot(slot);
      DataManager saveFile = (DataManager) null;
      COTLDataReadWriter<DataManager> cotlDataReadWriter2 = saveDataLoader;
      cotlDataReadWriter2.OnReadCompleted = cotlDataReadWriter2.OnReadCompleted + (Action<DataManager>) (datamanager =>
      {
        loaded = true;
        saveFile = datamanager;
      });
      COTLDataReadWriter<DataManager> cotlDataReadWriter3 = saveDataLoader;
      cotlDataReadWriter3.OnReadError = cotlDataReadWriter3.OnReadError + (Action<MMReadWriteError>) (error =>
      {
        loaded = true;
        ++filesNotRestored;
        saveDataLoader.Delete(saveSlot);
      });
      loadingOverlayController.Message = string.Format(ScriptLocalization.UI_Popups_SaveRestore.RestoringProgress, (object) slot, (object) markedForRestoration.Count, (object) 2, (object) 4);
      yield return (object) new WaitForSecondsRealtime(0.05f);
      saveDataLoader.Read(saveSlot);
      while (!loaded)
        yield return (object) null;
      yield return (object) new WaitForSecondsRealtime(0.25f);
      if (saveFile != null)
      {
        loadingOverlayController.Message = string.Format(ScriptLocalization.UI_Popups_SaveRestore.RestoringProgress, (object) slot, (object) markedForRestoration.Count, (object) 3, (object) 4);
        yield return (object) new WaitForSecondsRealtime(0.05f);
        MetaData metaData = MetaData.Default(saveFile);
        mainMenuController._loadMenu.SetupSlot(slot, metaData);
        mainMenuController._deleteMenu.SetupSlot(slot, metaData);
        metaDataPreloader.Write(metaData, metaSlot, true, false);
      }
      while (!loaded)
        yield return (object) null;
      loadingOverlayController.Message = string.Format(ScriptLocalization.UI_Popups_SaveRestore.RestoringProgress, (object) slot, (object) markedForRestoration.Count, (object) 4, (object) 4);
      yield return (object) new WaitForSecondsRealtime(0.25f);
      metaDataPreloader.OnWriteCompleted = (System.Action) null;
      metaDataPreloader.OnReadCompleted = (Action<MetaData>) null;
      metaDataPreloader.OnReadError = (Action<MMReadWriteError>) null;
      saveDataLoader.OnReadCompleted = (Action<DataManager>) null;
      metaSlot = (string) null;
    }
    loadingOverlayController.Hide();
    yield return (object) loadingOverlayController.YieldUntilHidden();
    if (filesNotRestored > 0)
    {
      UIMenuConfirmationWindow menu2 = MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate.Instantiate<UIMenuConfirmationWindow>();
      menu2.Configure(ScriptLocalization.UI_Popups_Error.Restoration, ScriptLocalization.UI_Popups_Error.RestorationErrorDescription, true);
      menu2.Show();
      mainMenuController.PushInstance<UIMenuConfirmationWindow>(menu2);
      yield return (object) menu2.YieldUntilHidden();
    }
    else
    {
      UIMenuConfirmationWindow menu3 = MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate.Instantiate<UIMenuConfirmationWindow>();
      menu3.Configure(ScriptLocalization.UI_Popups_Success.RestorationComplete, ScriptLocalization.UI_Popups_Success.RestorationCompleteDescription, true);
      menu3.Show();
      mainMenuController.PushInstance<UIMenuConfirmationWindow>(menu3);
      yield return (object) menu3.YieldUntilHidden();
    }
    mainMenuController.SetActiveStateForMenu(true);
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) (mainMenuController.MainMenu.PlayButton as MMButton));
  }

  public void FocusOnComicButton() => this.FocusOnObject(this._comicButton);

  public void FocusOnObject(MMButton target)
  {
    this.StartCoroutine((IEnumerator) this.FocusOnObjectIE(target));
  }

  public IEnumerator FocusOnObjectIE(MMButton target)
  {
    this._menuController.doIntroGlitch = false;
    this._cameraMovement.Reset(0.5f);
    this._cameraMovement.Reset();
    this._cameraMovement.enabled = false;
    yield return (object) new WaitForSeconds(0.5f);
    while (UIMenuBase.ActiveMenus.Count > 1)
    {
      MonoSingleton<UINavigatorNew>.Instance.LockNavigation = MonoSingleton<UINavigatorNew>.Instance.LockInput = true;
      yield return (object) null;
    }
    this.focus.gameObject.SetActive(true);
    this.focus.localScale = Vector3.one * 100f;
    Time.timeScale = 0.0f;
    this.focus.DOScale(7f, 1.5f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this.focus.DOMove(target.TargetGraphics[1].transform.position + new Vector3(-0.01f, 0.0f, 0.0f), 1.5f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    MonoSingleton<UINavigatorNew>.Instance.LockNavigation = MonoSingleton<UINavigatorNew>.Instance.LockInput = true;
    yield return (object) new WaitForSecondsRealtime(2f);
    target.transform.DOPunchScale(Vector3.one * 0.2f, 0.5f).SetUpdate<Tweener>(true);
    target.TargetGraphics[0].gameObject.SetActive(true);
    yield return (object) new WaitForSecondsRealtime(2f);
    this.focus.DOScale(100f, 1f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this.focus.DOAnchorPos((Vector2) Vector3.zero, 1f).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
    Time.timeScale = 1f;
    MonoSingleton<UINavigatorNew>.Instance.LockNavigation = MonoSingleton<UINavigatorNew>.Instance.LockInput = false;
    yield return (object) new WaitForSeconds(1f);
    this.focus.gameObject.SetActive(false);
    this._cameraMovement.enabled = true;
    this._menuController.doIntroGlitch = true;
  }

  public void Update()
  {
    this.ad.gameObject.SetActive(LocalizationManager.CurrentLanguageCode == "en");
  }

  [CompilerGenerated]
  public void \u003CAwake\u003Eb__29_0() => this._controlPrompts.ShowAcceptButton();

  [CompilerGenerated]
  public void \u003CStart\u003Eb__32_2()
  {
    this.ad.DOFade(0.0f, 0.25f);
    this.SetActiveStateForMenu(this.ad.gameObject, false);
    if (CheatConsole.IN_DEMO)
    {
      SaveAndLoad.SAVE_SLOT = 10;
      AudioManager.Instance.StopCurrentMusic();
      UIManager.PlayAudio("event:/ui/heretics_defeated");
      DifficultyManager.ForceDifficulty(DifficultyManager.Difficulty.Medium);
      DeviceLightingManager.Reset();
      FollowerManager.Reset();
      SimulationManager.Pause();
      StructureManager.Reset();
      TwitchManager.Abort();
      UIDynamicNotificationCenter.Reset();
      GameManager.GoG_Initialised = false;
      MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, "Game Biome Intro", 5f, "", (System.Action) (() =>
      {
        SaveAndLoad.ResetSave(SaveAndLoad.SAVE_SLOT, true);
        DataManager.Instance.SetTutorialVariables();
      }));
      this.SetActiveStateForMenu(false);
      MonoSingleton<UINavigatorNew>.Instance.Clear();
    }
    else
    {
      this.PerformMenuTransition((UISubmenuBase) this._mainMenu, (UISubmenuBase) this._loadMenu);
      this._controlPrompts.ShowCancelButton();
      this._controlPrompts.ShowAcceptButton();
      this._menuController.HideBlueTheme(1f, 0.0f);
    }
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if (!player.isLamb)
        CoopManager.RemoveCoopPlayerStatic(player, disengagePlayer: true, withDelay: false);
      UnityEngine.Object.Destroy((UnityEngine.Object) player.gameObject);
    }
    PlayerFarming.players.Clear();
    CoopManager.RemovePlayerFromMenu();
    PlayerFarming.playersCount = 0;
    CoopManager.CoopActive = false;
  }

  [CompilerGenerated]
  public void \u003CStart\u003Eb__32_4()
  {
    this.PerformMenuTransition((UISubmenuBase) this._loadMenu, (UISubmenuBase) this._mainMenu);
    this._controlPrompts.HideCancelButton();
    this.ad.DOFade(1f, 0.25f).SetDelay<TweenerCore<float, float, FloatOptions>>(0.25f);
    this.SetActiveStateForMenu(this.ad.gameObject, true);
    this._controlPrompts.ShowAcceptButton();
    this.StartCoroutine((IEnumerator) this.WaitForDLCCheck());
    this._menuController.ShowBlueTheme(1f, 0.0f);
  }

  [CompilerGenerated]
  public void \u003CStart\u003Eb__32_5()
  {
    this.PerformMenuTransition((UISubmenuBase) this._loadMenu, (UISubmenuBase) this._deleteMenu);
  }

  [CompilerGenerated]
  public void \u003CStart\u003Eb__32_6()
  {
    this.PerformMenuTransition((UISubmenuBase) this._deleteMenu, (UISubmenuBase) this._loadMenu);
  }

  [CompilerGenerated]
  public void \u003CStart\u003Eb__32_7()
  {
    this.ad.DOFade(0.0f, 0.25f);
    this.SetActiveStateForMenu(this.ad.gameObject, false);
    this.PerformMenuTransition((UISubmenuBase) this._mainMenu, (UISubmenuBase) this._dlcMenu);
    this._controlPrompts.ShowCancelButton();
    this._controlPrompts.ShowAcceptButton();
  }

  [CompilerGenerated]
  public void \u003CStart\u003Eb__32_8()
  {
    this.PerformMenuTransition((UISubmenuBase) this._dlcMenu, (UISubmenuBase) this._mainMenu);
    this._controlPrompts.HideCancelButton();
    this._controlPrompts.ShowAcceptButton();
    this.ad.DOFade(1f, 0.25f).SetDelay<TweenerCore<float, float, FloatOptions>>(0.25f);
    this.SetActiveStateForMenu(this.ad.gameObject, true);
    this.StartCoroutine((IEnumerator) this.WaitForDLCCheck());
  }

  [CompilerGenerated]
  public void \u003CStart\u003Eb__32_1()
  {
    this._cameraMovement.Reset();
    this._cameraMovement.enabled = false;
    UIComicMenuController menu = MonoSingleton<UIManager>.Instance.ComicMenuTemplate.Instantiate<UIComicMenuController>();
    this._camera.gameObject.SetActive(false);
    this._comicCamera.gameObject.SetActive(true);
    this._keyArt.gameObject.SetActive(false);
    menu.GetComponent<Canvas>().worldCamera = this._comicCamera;
    menu.ShowComic(false);
    this.PushInstance<UIComicMenuController>(menu);
    PersistenceManager.PersistentData.OpenedComic = true;
    PersistenceManager.Save();
    this._comicButton.TargetGraphics[0].gameObject.SetActive(false);
    UIComicMenuController comicMenuController = menu;
    comicMenuController.OnHide = comicMenuController.OnHide + (System.Action) (() =>
    {
      this._cameraMovement.enabled = true;
      this._camera.gameObject.SetActive(true);
      this._comicCamera.gameObject.SetActive(false);
      this._keyArt.gameObject.SetActive(true);
    });
  }

  [CompilerGenerated]
  public void \u003CStart\u003Eb__32_9()
  {
    this._cameraMovement.enabled = true;
    this._camera.gameObject.SetActive(true);
    this._comicCamera.gameObject.SetActive(false);
    this._keyArt.gameObject.SetActive(true);
  }
}
