// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MainMenu.LoadMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using src.Extensions;
using src.Managers;
using src.UI;
using src.UINavigator;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.MainMenu;

public class LoadMenu : UISubmenuBase
{
  public System.Action OnDeleteButtonPressed;
  public System.Action OnBackButtonPressed;
  [SerializeField]
  public SaveSlotButton_Load[] _saveSlots;
  [SerializeField]
  public SaveSlotButton_BaseGame[] _baseGameSaveSlots;
  [SerializeField]
  public Button _deleteButton;
  [SerializeField]
  public Button _backButton;
  public SaveSlotButtonBase lastSelectedSlot;

  public SaveSlotButton_Load[] SaveSlots => this._saveSlots;

  public SaveSlotButton_BaseGame[] BaseGameSaveSlots => this._baseGameSaveSlots;

  public void Start()
  {
    this._deleteButton.onClick.AddListener(new UnityAction(this.OnDeleteButtonClicked));
    this._backButton.onClick.AddListener(new UnityAction(this.OnBackButtonClicked));
  }

  public void SetupSlot(int slot) => this._saveSlots[slot].SetupSaveSlot(slot);

  public void SetupSlot(int slot, MetaData metaData)
  {
    if (slot >= 10)
      this._saveSlots[slot - 10].SetupBaseGameSaveSlot(slot, metaData);
    else
      this._saveSlots[slot].SetupSaveSlot(slot, metaData);
  }

  public override void OnShowStarted()
  {
    foreach (SaveSlotButton_Load saveSlot in this._saveSlots)
      saveSlot.OnSaveSlotPressed = saveSlot.OnSaveSlotPressed + new Action<int>(this.OnTryLoadSaveSlot);
    foreach (SaveSlotButton_BaseGame baseGameSaveSlot in this._baseGameSaveSlots)
      baseGameSaveSlot.OnSaveSlotPressed = baseGameSaveSlot.OnSaveSlotPressed + new Action<int>(this.OnTryLoadSaveSlot);
  }

  public override void OnHideStarted()
  {
    foreach (SaveSlotButton_Load saveSlot in this._saveSlots)
      saveSlot.OnSaveSlotPressed = saveSlot.OnSaveSlotPressed - new Action<int>(this.OnTryLoadSaveSlot);
    foreach (SaveSlotButton_BaseGame baseGameSaveSlot in this._baseGameSaveSlots)
      baseGameSaveSlot.OnSaveSlotPressed = baseGameSaveSlot.OnSaveSlotPressed - new Action<int>(this.OnTryLoadSaveSlot);
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    System.Action backButtonPressed = this.OnBackButtonPressed;
    if (backButtonPressed == null)
      return;
    backButtonPressed();
  }

  public void OnTryLoadSaveSlot(int slotIndex)
  {
    MonoSingleton<UINavigatorNew>.Instance.LockInput = MonoSingleton<UINavigatorNew>.Instance.LockNavigation = true;
    this.lastSelectedSlot = slotIndex >= 10 ? (SaveSlotButtonBase) this._baseGameSaveSlots[slotIndex - 10] : (SaveSlotButtonBase) this._saveSlots[slotIndex];
    DeviceLightingManager.StopAll();
    DeviceLightingManager.TransitionLighting(Color.white, Color.black, 1f);
    MetaData? metaData = this.lastSelectedSlot.MetaData;
    if (metaData.HasValue)
    {
      metaData = this.lastSelectedSlot.MetaData;
      if (metaData.Value.ActivatedMajorDLC && !GameManager.AuthenticateMajorDLC())
      {
        UIMenuConfirmationWindow errorWindow = MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate.Instantiate<UIMenuConfirmationWindow>();
        errorWindow.Configure(ScriptLocalization.UI_Popups_Error.GenericHeader, LocalizationManager.GetTranslation("UI/Popups/Error/RequiresMajorDLC"), true);
        errorWindow.Show();
        errorWindow.Canvas.sortingOrder = 1000;
        this.PushInstance<UIMenuConfirmationWindow>(errorWindow);
        errorWindow.OnConfirm += (System.Action) (() => errorWindow.Hide());
        UIMenuConfirmationWindow confirmationWindow = errorWindow;
        confirmationWindow.OnHide = confirmationWindow.OnHide + (System.Action) (() => this.SetActiveStateForMenu(true));
        return;
      }
    }
    if (SaveAndLoad.SaveExist(slotIndex))
      this.ContinueGame(slotIndex);
    else
      this.PlayNewGame(slotIndex);
  }

  public void PlayNewGame(int saveSlot)
  {
    SaveAndLoad.SAVE_SLOT = saveSlot;
    if (PersistenceManager.ShowNewGameOptionsMenu())
    {
      MonoSingleton<UINavigatorNew>.Instance.LockInput = MonoSingleton<UINavigatorNew>.Instance.LockNavigation = false;
      bool didBeginGame = false;
      UINewGameOptionsMenuController optionsMenuController = this.Push<UINewGameOptionsMenuController>(MonoSingleton<UIManager>.Instance.NewGameOptionsMenuTemplate);
      optionsMenuController.OnOptionsAccepted += (Action<UINewGameOptionsMenuController.NewGameOptions>) (result =>
      {
        didBeginGame = true;
        Debug.Log((object) "Start a new game with the specified options");
        this.SetActiveStateForMenu(false);
        this.BeginNewGame(result);
      });
      optionsMenuController.OnHide = optionsMenuController.OnHide + (System.Action) (() =>
      {
        if (didBeginGame)
        {
          MonoSingleton<UINavigatorNew>.Instance.LockInput = MonoSingleton<UINavigatorNew>.Instance.LockNavigation = !DataManager.Instance.QuickStartActive;
          this.SetActiveStateForMenu(false);
          MonoSingleton<UINavigatorNew>.Instance.Clear();
        }
        this._canvasGroup.interactable = true;
      });
      this._canvasGroup.interactable = false;
    }
    else
      this.BeginNewGame(new UINewGameOptionsMenuController.NewGameOptions()
      {
        PermadeathMode = false,
        QuickStart = false,
        DifficultyIndex = DifficultyManager.AllAvailableDifficulties().IndexOf<DifficultyManager.Difficulty>(DifficultyManager.Difficulty.Medium)
      });
  }

  public void BeginNewGame(
    UINewGameOptionsMenuController.NewGameOptions newGameOptions)
  {
    GameManager.CurrentDungeonLayer = 1;
    GameManager.CurrentDungeonFloor = 1;
    GameManager.DungeonUseAllLayers = false;
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    AudioManager.Instance.StopCurrentMusic();
    UIManager.PlayAudio("event:/ui/heretics_defeated");
    if (newGameOptions.QuickStart)
    {
      SaveAndLoad.ResetSave(SaveAndLoad.SAVE_SLOT, true);
      DataManager.Instance.PermadeDeathActive = newGameOptions.PermadeathMode;
      DataManager.Instance.MetaData.Difficulty = newGameOptions.DifficultyIndex;
      DataManager.Instance.QuickStartActive = true;
      DataManager.Instance.OnboardingFinished = true;
      if (newGameOptions.WinterMode)
      {
        PersistenceManager.PersistentData.PlayedWinterMode = true;
        DataManager.Instance.WinterModeActive = true;
        DataManager.Instance.MAJOR_DLC = true;
        DataManager.Instance.RemovedStoryMomentsActive = true;
      }
      if (newGameOptions.SurvivalMode)
      {
        PersistenceManager.PersistentData.PlayedSurvivalMode = true;
        PersistenceManager.Save();
        DataManager.Instance.SurvivalModeActive = true;
        DataManager.Instance.RemovedStoryMomentsActive = true;
      }
      DataManager.Instance.SetTutorialVariables();
      DifficultyManager.ForceDifficulty(DataManager.Instance.MetaData.Difficulty);
      MonoSingleton<UINavigatorNew>.Instance.LockInput = MonoSingleton<UINavigatorNew>.Instance.LockNavigation = false;
      MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, "Base Biome 1", 5f, "", (System.Action) null);
    }
    else
    {
      QuoteScreenController.Init(new List<QuoteScreenController.QuoteTypes>()
      {
        QuoteScreenController.QuoteTypes.IntroQuote,
        QuoteScreenController.QuoteTypes.IntroQuote2
      }, (System.Action) (() => MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, "Game Biome Intro", 1f, "", (System.Action) null)), (System.Action) (() => MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, "Game Biome Intro", 5f, "", (System.Action) null)));
      MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, "QuoteScreen", 5f, "", (System.Action) (() =>
      {
        SaveAndLoad.ResetSave(SaveAndLoad.SAVE_SLOT, true);
        DataManager.Instance.SetTutorialVariables();
        DataManager.Instance.PermadeDeathActive = newGameOptions.PermadeathMode;
        DataManager.Instance.MetaData.Difficulty = newGameOptions.DifficultyIndex;
        DifficultyManager.ForceDifficulty(DataManager.Instance.MetaData.Difficulty);
        MonoSingleton<UINavigatorNew>.Instance.LockInput = MonoSingleton<UINavigatorNew>.Instance.LockNavigation = false;
      }));
    }
  }

  public void ContinueGame(int saveSlot)
  {
    this.SetActiveStateForMenu(false);
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    UIManager.PlayAudio("event:/ui/heretics_defeated");
    SaveAndLoad.SAVE_SLOT = saveSlot;
    AudioManager.Instance.StopCurrentMusic();
    MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, "Base Biome 1", 3f, "", new System.Action(this.ContinueGameCallback));
  }

  public void ContinueGameCallback()
  {
    AudioManager.Instance.StopCurrentMusic();
    SaveAndLoad.Load(SaveAndLoad.SAVE_SLOT);
    MonoSingleton<UINavigatorNew>.Instance.LockInput = MonoSingleton<UINavigatorNew>.Instance.LockNavigation = false;
  }

  public void OnLoadSuccess()
  {
    Debug.Log((object) "Load success!");
    AudioManager.Instance.StopCurrentMusic();
    SaveAndLoad.OnLoadComplete -= new System.Action(this.OnLoadSuccess);
    SaveAndLoad.OnLoadError -= new Action<MMReadWriteError>(this.OnLoadError);
  }

  public void OnLoadError(MMReadWriteError error)
  {
    Time.timeScale = 0.0f;
    this.SetActiveStateForMenu(false);
    MonoSingleton<UINavigatorNew>.Instance.LockInput = false;
    UIMenuConfirmationWindow errorWindow = UIMenuBase.ActiveMenus.LastElement<UIMenuBase>().Push<UIMenuConfirmationWindow>(MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate);
    errorWindow.Configure(ScriptLocalization.UI_Popups_Error.GenericHeader, ScriptLocalization.UI_Popups_Error.SaveCorrupted, true);
    errorWindow.Show();
    errorWindow.Canvas.sortingOrder = 1000;
    this.PushInstance<UIMenuConfirmationWindow>(errorWindow);
    MMTransition.StopCurrentTransition();
    errorWindow.OnConfirm += (System.Action) (() =>
    {
      MonoSingleton<UINavigatorNew>.Instance.Clear();
      UIMenuBase.ActiveMenus.Clear();
      MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade, "Main Menu", 3f, "", (System.Action) (() => { }));
      errorWindow.Hide(true);
    });
    SaveAndLoad.OnLoadComplete -= new System.Action(this.OnLoadSuccess);
    SaveAndLoad.OnLoadError -= new Action<MMReadWriteError>(this.OnLoadError);
  }

  public void OnDeleteButtonClicked()
  {
    this.OverrideDefaultOnce((Selectable) this._deleteButton);
    System.Action deleteButtonPressed = this.OnDeleteButtonPressed;
    if (deleteButtonPressed == null)
      return;
    deleteButtonPressed();
  }

  public void OnBackButtonClicked()
  {
    System.Action backButtonPressed = this.OnBackButtonPressed;
    if (backButtonPressed == null)
      return;
    backButtonPressed();
  }

  [CompilerGenerated]
  public void \u003COnTryLoadSaveSlot\u003Eb__17_1() => this.SetActiveStateForMenu(true);
}
