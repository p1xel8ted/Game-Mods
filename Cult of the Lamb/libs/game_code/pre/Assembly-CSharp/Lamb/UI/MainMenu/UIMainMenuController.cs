// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MainMenu.UIMainMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using src.Extensions;
using src.UI;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Lamb.UI.MainMenu;

public class UIMainMenuController : UIMenuBase
{
  [Header("Menus")]
  [SerializeField]
  private Lamb.UI.MainMenu.MainMenu _mainMenu;
  [SerializeField]
  private LoadMenu _loadMenu;
  [SerializeField]
  private DeleteMenu _deleteMenu;
  [Header("Other")]
  [SerializeField]
  private UIMenuControlPrompts _controlPrompts;
  [SerializeField]
  private UINavigatorFollowElement _navigatorHighlight;
  [Header("Sidebar")]
  [SerializeField]
  private MMButton _discordButton;
  private UIMenuBase _currentMenu;

  public Lamb.UI.MainMenu.MainMenu MainMenu => this._mainMenu;

  public LoadMenu LoadMenu => this._loadMenu;

  public DeleteMenu DeleteMenu => this._deleteMenu;

  public override void Awake()
  {
    base.Awake();
    this._controlPrompts.HideCancelButton();
    this._controlPrompts.HideAcceptButton();
    Lamb.UI.MainMenu.MainMenu mainMenu = this._mainMenu;
    mainMenu.OnShow = mainMenu.OnShow + (System.Action) (() => this._controlPrompts.ShowAcceptButton());
  }

  public void Start()
  {
    this._currentMenu = (UIMenuBase) this._mainMenu;
    UIMenuBase.ActiveMenus.Add((UIMenuBase) this);
    if (CheatConsole.IN_DEMO)
      DemoWatermark.Play();
    this._mainMenu.OnPlayButtonPressed += (System.Action) (() =>
    {
      if (CheatConsole.IN_DEMO)
      {
        SaveAndLoad.SAVE_SLOT = 10;
        AudioManager.Instance.StopCurrentMusic();
        UIManager.PlayAudio("event:/ui/heretics_defeated");
        DifficultyManager.ForceDifficulty(DifficultyManager.Difficulty.Medium);
        KeyboardLightingManager.Reset();
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
      }
    });
    this._loadMenu.OnBackButtonPressed += (System.Action) (() =>
    {
      this.PerformMenuTransition((UISubmenuBase) this._loadMenu, (UISubmenuBase) this._mainMenu);
      this._controlPrompts.HideCancelButton();
      this._controlPrompts.ShowAcceptButton();
    });
    this._loadMenu.OnDeleteButtonPressed += (System.Action) (() => this.PerformMenuTransition((UISubmenuBase) this._loadMenu, (UISubmenuBase) this._deleteMenu));
    this._deleteMenu.OnBackButtonPressed += (System.Action) (() => this.PerformMenuTransition((UISubmenuBase) this._deleteMenu, (UISubmenuBase) this._loadMenu));
    this._discordButton.onClick.AddListener((UnityAction) (() => Application.OpenURL("https://discord.com/invite/massivemonster")));
  }

  private void PerformMenuTransition(UISubmenuBase from, UISubmenuBase to)
  {
    this._currentMenu = (UIMenuBase) to;
    from.Hide();
    to.Show();
  }

  protected override void OnPush() => this._navigatorHighlight.enabled = false;

  protected override void OnRelease() => this._navigatorHighlight.enabled = true;

  protected override void OnDestroy()
  {
    base.OnDestroy();
    UIMenuBase.ActiveMenus.Remove((UIMenuBase) this);
  }

  public IEnumerator PreloadMetadata()
  {
    COTLDataReadWriter<MetaData> metaDataPreloader = new COTLDataReadWriter<MetaData>();
    COTLDataReadWriter<DataManager> saveDataLoader = new COTLDataReadWriter<DataManager>();
    List<int> metaErrors = new List<int>();
    for (int slot = 0; slot < 3; ++slot)
    {
      int currentSlot = slot;
      string filename = SaveAndLoad.MakeMetaSlot(currentSlot);
      if (SaveAndLoad.SaveExist(currentSlot))
      {
        bool loaded = false;
        MetaData mData = new MetaData();
        if (metaDataPreloader.FileExists(filename))
        {
          COTLDataReadWriter<MetaData> cotlDataReadWriter1 = metaDataPreloader;
          cotlDataReadWriter1.OnReadCompleted = cotlDataReadWriter1.OnReadCompleted + new Action<MetaData>(MetaDataLoaded);
          COTLDataReadWriter<MetaData> cotlDataReadWriter2 = metaDataPreloader;
          cotlDataReadWriter2.OnReadError = cotlDataReadWriter2.OnReadError + new Action<MMReadWriteError>(MetaDataReadError);
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
          this._loadMenu.SetupSlot(currentSlot, mData);
          this._deleteMenu.SetupSlot(currentSlot, mData);
        }
        else
        {
          this._loadMenu.SetupSlot(currentSlot);
          this._deleteMenu.SetupSlot(currentSlot);
        }
        metaDataPreloader.OnWriteCompleted = (System.Action) null;
        metaDataPreloader.OnReadCompleted = (Action<MetaData>) null;
        metaDataPreloader.OnReadError = (Action<MMReadWriteError>) null;
        saveDataLoader.OnReadCompleted = (Action<DataManager>) null;

        void MetaDataLoaded(MetaData metaData)
        {
          mData = metaData;
          loaded = true;
        }

        void MetaDataReadError(MMReadWriteError readWriteError)
        {
          metaErrors.Add(currentSlot);
          loaded = true;
        }
      }
      else
      {
        this._loadMenu.SetupSlot(currentSlot);
        this._deleteMenu.SetupSlot(currentSlot);
      }
    }
    if (metaErrors.Count > 0)
      yield return (object) this.RestoreFiles(metaErrors, metaDataPreloader, saveDataLoader);
  }

  private IEnumerator RestoreFiles(
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
  }
}
