// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MainMenu.LoadMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using src.Extensions;
using src.UI;
using src.UINavigator;
using System;
using System.Collections.Generic;
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
  private SaveSlotButtonBase[] _saveSlots;
  [SerializeField]
  private Button _deleteButton;
  [SerializeField]
  private Button _backButton;

  private void Start()
  {
    this._deleteButton.onClick.AddListener(new UnityAction(this.OnDeleteButtonClicked));
    this._backButton.onClick.AddListener(new UnityAction(this.OnBackButtonClicked));
  }

  public void SetupSlot(int slot) => this._saveSlots[slot].SetupSaveSlot(slot);

  public void SetupSlot(int slot, MetaData metaData)
  {
    this._saveSlots[slot].SetupSaveSlot(slot, metaData);
  }

  protected override void OnShowStarted()
  {
    foreach (SaveSlotButtonBase saveSlot in this._saveSlots)
      saveSlot.OnSaveSlotPressed += new Action<int>(this.OnTryLoadSaveSlot);
  }

  protected override void OnHideStarted()
  {
    foreach (SaveSlotButtonBase saveSlot in this._saveSlots)
      saveSlot.OnSaveSlotPressed -= new Action<int>(this.OnTryLoadSaveSlot);
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

  private void OnTryLoadSaveSlot(int slotIndex)
  {
    KeyboardLightingManager.StopAll();
    KeyboardLightingManager.TransitionAllKeys(Color.white, Color.black, 1f);
    if (SaveAndLoad.SaveExist(slotIndex))
      this.ContinueGame(slotIndex);
    else
      this.PlayNewGame(slotIndex);
  }

  private void PlayNewGame(int saveSlot)
  {
    SaveAndLoad.SAVE_SLOT = saveSlot;
    GameManager.CurrentDungeonLayer = 1;
    GameManager.CurrentDungeonFloor = 1;
    GameManager.DungeonUseAllLayers = false;
    AudioManager.Instance.StopCurrentMusic();
    UIManager.PlayAudio("event:/ui/heretics_defeated");
    QuoteScreenController.Init(new List<QuoteScreenController.QuoteTypes>()
    {
      QuoteScreenController.QuoteTypes.IntroQuote,
      QuoteScreenController.QuoteTypes.IntroQuote2
    }, (System.Action) (() => MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, "Game Biome Intro", 1f, "", (System.Action) null)), (System.Action) (() => MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, "Game Biome Intro", 5f, "", (System.Action) null)));
    MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, "QuoteScreen", 5f, "", (System.Action) (() =>
    {
      SaveAndLoad.ResetSave(saveSlot, true);
      DataManager.Instance.SetTutorialVariables();
      DifficultyManager.ForceDifficulty(DataManager.Instance.MetaData.Difficulty);
    }));
    this.SetActiveStateForMenu(false);
    MonoSingleton<UINavigatorNew>.Instance.Clear();
  }

  private void ContinueGame(int saveSlot)
  {
    UIManager.PlayAudio("event:/ui/heretics_defeated");
    SaveAndLoad.SAVE_SLOT = saveSlot;
    MMTransition.Play(MMTransition.TransitionType.FadeAndCallBack, MMTransition.Effect.BlackFade, "Base Biome 1", 3f, "", new System.Action(this.ContinueGameCallback));
    this.OverrideDefault((Selectable) this._saveSlots[saveSlot].Button);
    this.SetActiveStateForMenu(false);
    MonoSingleton<UINavigatorNew>.Instance.Clear();
  }

  private void ContinueGameCallback()
  {
    MMTransition.IsPlaying = false;
    SaveAndLoad.OnLoadComplete += new System.Action(this.OnLoadSuccess);
    SaveAndLoad.OnLoadError += new Action<MMReadWriteError>(this.OnLoadError);
    SaveAndLoad.Load(SaveAndLoad.SAVE_SLOT);
  }

  private void OnLoadSuccess()
  {
    Debug.Log((object) "Load success!");
    AudioManager.Instance.StopCurrentMusic();
    MMTransition.Play(MMTransition.TransitionType.LoadAndFadeOut, MMTransition.Effect.BlackFade, "Base Biome 1", 3f, string.Empty, (System.Action) null);
    SaveAndLoad.OnLoadComplete -= new System.Action(this.OnLoadSuccess);
    SaveAndLoad.OnLoadError -= new Action<MMReadWriteError>(this.OnLoadError);
  }

  private void OnLoadError(MMReadWriteError error)
  {
    UIMenuConfirmationWindow errorWindow = MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate.Instantiate<UIMenuConfirmationWindow>();
    errorWindow.Configure(ScriptLocalization.UI_Popups_Error.GenericHeader, ScriptLocalization.UI_Popups_Error.SaveCorrupted, true);
    errorWindow.Show();
    errorWindow.Canvas.sortingOrder = 1000;
    this.PushInstance<UIMenuConfirmationWindow>(errorWindow);
    errorWindow.OnConfirm += (System.Action) (() => errorWindow.Hide());
    UIMenuConfirmationWindow confirmationWindow = errorWindow;
    confirmationWindow.OnHide = confirmationWindow.OnHide + (System.Action) (() =>
    {
      MonoSingleton<UINavigatorNew>.Instance.Clear();
      MMTransition.IsPlaying = true;
      MMTransition.ResumePlay((System.Action) (() => this.SetActiveStateForMenu(true)));
    });
    SaveAndLoad.OnLoadComplete -= new System.Action(this.OnLoadSuccess);
    SaveAndLoad.OnLoadError -= new Action<MMReadWriteError>(this.OnLoadError);
  }

  private void OnDeleteButtonClicked()
  {
    this.OverrideDefaultOnce((Selectable) this._deleteButton);
    System.Action deleteButtonPressed = this.OnDeleteButtonPressed;
    if (deleteButtonPressed == null)
      return;
    deleteButtonPressed();
  }

  private void OnBackButtonClicked()
  {
    System.Action backButtonPressed = this.OnBackButtonPressed;
    if (backButtonPressed == null)
      return;
    backButtonPressed();
  }
}
