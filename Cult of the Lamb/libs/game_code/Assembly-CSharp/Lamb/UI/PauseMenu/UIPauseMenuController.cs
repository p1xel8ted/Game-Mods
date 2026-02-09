// Decompiled with JetBrains decompiler
// Type: Lamb.UI.PauseMenu.UIPauseMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using Lamb.UI.SettingsMenu;
using MMBiomeGeneration;
using MMTools;
using Rewired;
using src.Extensions;
using src.UI;
using src.UINavigator;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UI.Menus.TwitchMenu;
using Unify;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Lamb.UI.PauseMenu;

public class UIPauseMenuController : UIMenuBase
{
  [Header("Buttons")]
  [SerializeField]
  public MMButton _resumeButton;
  [SerializeField]
  public MMButton _saveButton;
  [SerializeField]
  public MMButton _settingsButton;
  [SerializeField]
  public MMButton _twitchSettingsButton;
  [SerializeField]
  public MMButton _coopButton;
  [SerializeField]
  public MMButton _photoModeButton;
  [SerializeField]
  public MMButton _helpButton;
  [SerializeField]
  public MMButton _mainMenuButton;
  [SerializeField]
  public MMButton _quitButton;
  [Header("Sidebar")]
  [SerializeField]
  public MMButton _bugReportButton;
  [SerializeField]
  public MMButton _discordButton;
  [Header("Other")]
  [SerializeField]
  public TextMeshProUGUI _seedText;
  [SerializeField]
  public TextMeshProUGUI _saveButtonText;
  [SerializeField]
  public TextMeshProUGUI _coopButtonText;
  [SerializeField]
  public UINavigatorFollowElement _buttonHighlight;
  [SerializeField]
  public ButtonHighlightController _buttonHighlightController;
  public bool saved;
  public bool isLoading;
  public bool disabledDynamicRes;
  public bool DenyCoop;
  public bool CoopSelected;
  public int previousJoystickCount;
  public bool CoopButtonSelected;

  public override void Awake()
  {
    base.Awake();
    this.SetActiveStateForMenu(false);
  }

  public void Start()
  {
    MMVibrate.StopRumble();
    AudioManager.Instance.PauseActiveLoopsAndSFX();
    this._resumeButton.onClick.AddListener(new UnityAction(this.OnResumeButtonPressed));
    this._saveButton.onClick.AddListener(new UnityAction(this.OnSaveButtonPressed));
    this._settingsButton.onClick.AddListener(new UnityAction(this.OnSettingsButtonPressed));
    this._helpButton.onClick.AddListener(new UnityAction(this.OnHelpButtonPressed));
    this._mainMenuButton.onClick.AddListener(new UnityAction(this.OnMainMenuButtonPressed));
    this._quitButton.onClick.AddListener(new UnityAction(this.OnQuitButtonPressed));
    this._photoModeButton.onClick.AddListener(new UnityAction(this.OnPhotoModePressed));
    this._coopButton.onClick.AddListener(new UnityAction(this.OnCoopButtonPressed));
    this._twitchSettingsButton.onClick.AddListener(new UnityAction(this.OnTwitchSettingsButtonPressed));
    this._twitchSettingsButton.OnSelected += new System.Action(this._buttonHighlightController.SetAsTwitch);
    this._twitchSettingsButton.OnDeselected += new System.Action(this._buttonHighlightController.SetAsRed);
    this._seedText.text = (UnityEngine.Object) BiomeGenerator.Instance == (UnityEngine.Object) null ? "" : BiomeGenerator.Instance.Seed.ToString();
    this._bugReportButton.onClick.AddListener((UnityAction) (() => this.Push<UIBugReportingOverlayController>(MonoSingleton<UIManager>.Instance.BugReportingOverlayTemplate)));
    this._discordButton.onClick.AddListener((UnityAction) (() => Application.OpenURL("https://discord.com/invite/massivemonster")));
    this.StartCoroutine((IEnumerator) this.UpdateLoop());
  }

  public void OnEnable()
  {
    SessionManager.OnSessionEnd += new SessionManager.SessionEventDelegate(this.OnSessionEnd);
    this.saved = false;
    SaveAndLoad.OnSaveCompleted += new System.Action(this.OnSaveFinalized);
    SaveAndLoad.OnSaveError += new Action<MMReadWriteError>(this.OnSaveError);
    if (CoopManager.CoopActive)
      this._coopButtonText.text = ScriptLocalization.UI.CoopMenu_Remove;
    else
      this._coopButtonText.text = ScriptLocalization.UI.CoopMenu_Add;
    if ((bool) (UnityEngine.Object) MonoSingleton<UIManager>.Instance)
      MonoSingleton<UIManager>.Instance.SetCurrentCursor(0);
    ReInput.ControllerConnectedEvent += (Action<ControllerStatusChangedEventArgs>) new Action<ControllerStatusChangedEventArgs>(this.OnControllerConnected);
    this._coopButton.OnSelected += new System.Action(this.CoopButtonOnSelected);
    this._coopButton.OnDeselected += new System.Action(this.CoopButtonOnDeselected);
  }

  public new void OnDisable()
  {
    SessionManager.OnSessionEnd -= new SessionManager.SessionEventDelegate(this.OnSessionEnd);
    SaveAndLoad.OnSaveCompleted -= new System.Action(this.OnSaveFinalized);
    SaveAndLoad.OnSaveError -= new Action<MMReadWriteError>(this.OnSaveError);
    if ((bool) (UnityEngine.Object) MonoSingleton<UIManager>.Instance)
      MonoSingleton<UIManager>.Instance.SetPreviousCursor();
    if ((bool) (UnityEngine.Object) MonoSingleton<UIManager>.Instance)
      MonoSingleton<UIManager>.Instance.ResetPreviousCursor();
    ReInput.ControllerConnectedEvent -= (Action<ControllerStatusChangedEventArgs>) new Action<ControllerStatusChangedEventArgs>(this.OnControllerConnected);
    this._coopButton.OnSelected -= new System.Action(this.CoopButtonOnSelected);
    this._coopButton.OnDeselected -= new System.Action(this.CoopButtonOnDeselected);
  }

  public void OnControllerConnected(ControllerStatusChangedEventArgs args)
  {
    this.RefreshCoopText();
  }

  public IEnumerator UpdateLoop()
  {
    UIPauseMenuController pauseMenuController = this;
    bool closable = true;
    while (true)
    {
      yield return (object) null;
      if (!closable || !pauseMenuController._canvasGroup.interactable || !InputManager.Gameplay.GetPauseButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
        closable = UIMenuBase.ActiveMenus.Count == 1;
      else
        break;
    }
    pauseMenuController.OnCancelButtonInput();
  }

  public void Update()
  {
    Time.timeScale = 0.0f;
    if (ReInput.controllers == null || ReInput.controllers.joystickCount == this.previousJoystickCount || this.IsHiding)
      return;
    this.RefreshCoopText();
  }

  public void OnSessionEnd(Guid sessionGuid, User sessionUser) => this.Hide(true);

  public void OnResumeButtonPressed() => this.Hide();

  public void OnSaveButtonPressed()
  {
    this._saveButtonText.text = ScriptLocalization.UI.Saving;
    SaveAndLoad.Save();
  }

  public void OnSaveFinalized()
  {
    this._saveButtonText.text = ScriptLocalization.UI.Saved;
    this.saved = true;
  }

  public void OnSaveError(MMReadWriteError error)
  {
    this._saveButtonText.text = ScriptLocalization.UI_SaveError.Title;
    this.saved = true;
    UIMenuConfirmationWindow menu = MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate.Instantiate<UIMenuConfirmationWindow>();
    menu.Configure(ScriptLocalization.UI_SaveError.Title, ScriptLocalization.UI_SaveError.Description.NewLine(), true);
    menu.Show();
    this.PushInstance<UIMenuConfirmationWindow>(menu);
  }

  public void OnPhotoModePressed()
  {
    if (this.isLoading)
      return;
    this.StartCoroutine((IEnumerator) this.LoadPhotoMode());
  }

  public IEnumerator LoadPhotoMode()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    UIPauseMenuController pauseMenuController = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      pauseMenuController.isLoading = false;
      pauseMenuController.Hide(true);
      PhotoModeManager.EnablePhotoMode();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    pauseMenuController.isLoading = true;
    System.Threading.Tasks.Task task = MonoSingleton<UIManager>.Instance.LoadPhotomodeAssets();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitUntil((Func<bool>) (() => task.IsCompleted));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void OnCoopButtonPressed()
  {
    if (!CoopManager.CoopActive)
    {
      if (this.DenyCoop)
      {
        this.Shake(this._coopButton.transform);
      }
      else
      {
        this.Hide(true);
        CoopManager.AddPlayerFromMenu();
      }
    }
    else
    {
      this.OnHidden = this.OnHidden + (System.Action) (() => CoopManager.RemovePlayerFromMenu());
      this.Hide();
    }
  }

  public void Shake(Transform _iconContainer)
  {
    _iconContainer.DOKill();
    _iconContainer.DOShakePosition(1f, new Vector3(10f, 0.0f)).SetUpdate<Tweener>(true);
  }

  public void OnSettingsButtonPressed()
  {
    this.Push<UISettingsMenuController>(MonoSingleton<UIManager>.Instance.SettingsMenuControllerTemplate);
  }

  public void OnTwitchSettingsButtonPressed()
  {
    this.Push<UITwitchSettingsMenuController>(MonoSingleton<UIManager>.Instance.TwitchSettingsMenuController);
  }

  public void OnHelpButtonPressed()
  {
    this.Push<UITutorialMenuController>(MonoSingleton<UIManager>.Instance.TutorialMenuTemplate);
  }

  public void OnMainMenuButtonPressed()
  {
    UIMenuConfirmationWindow confirmationWindow = this.Push<UIMenuConfirmationWindow>(MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate);
    confirmationWindow.Configure(ScriptLocalization.UI_Generic.Quit, !this.saved ? ScriptLocalization.UI.ConfirmQuitSaveWarning : ScriptLocalization.UI.ConfirmQuit);
    confirmationWindow.OnConfirm += new System.Action(this.LoadMainMenu);
  }

  public void LoadMainMenu()
  {
    this.SetActiveStateForMenu(false);
    if (CoopManager.CoopActive)
      CoopManager.RemovePlayerFromMenu();
    SimulationManager.Pause();
    DeviceLightingManager.Reset();
    FollowerManager.Reset();
    StructureManager.Reset();
    UIDynamicNotificationCenter.Reset();
    MonoSingleton<UIManager>.Instance.ResetPreviousCursor();
    TwitchManager.Abort();
    MMTransition.ForceShowIcon = true;
    MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade, "Main Menu", 1f, "", (System.Action) (() => this.Hide(true)));
  }

  public void OnQuitButtonPressed()
  {
    UIMenuConfirmationWindow confirmationWindow = this.Push<UIMenuConfirmationWindow>(MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate);
    confirmationWindow.Configure(ScriptLocalization.UI_Generic.Quit, !this.saved ? ScriptLocalization.UI.ConfirmQuitSaveWarning : ScriptLocalization.UI.ConfirmQuit);
    confirmationWindow.OnConfirm += (System.Action) (() => Application.Quit());
  }

  public override void OnShowStarted()
  {
    base.OnShowStarted();
    MMVibrate.StopRumble();
    UIManager.PlayAudio("event:/ui/pause");
  }

  public override void SetActiveStateForMenu(bool state)
  {
    base.SetActiveStateForMenu(state);
    this.previousJoystickCount = ReInput.controllers.joystickCount;
    if (!state)
      return;
    this.RefreshCoopText();
    this._saveButton.interactable = (UnityEngine.Object) BiomeGenerator.Instance == (UnityEngine.Object) null && !MonoSingleton<UIManager>.Instance.ForceDisableSaving;
    this._helpButton.interactable = DataManager.Instance.RevealedTutorialTopics.Count > 0;
  }

  public void RefreshCoopText()
  {
    if (CoopManager.CoopActive)
      this._coopButtonText.text = ScriptLocalization.UI.CoopMenu_Remove;
    else
      this._coopButtonText.text = ScriptLocalization.UI.CoopMenu_Add;
    this.DenyCoop = false;
    this._coopButton.Interactable = true;
    if (ReInput.controllers.joystickCount < 1)
    {
      this.DenyCoop = true;
      this._coopButton.Interactable = false;
      this._coopButtonText.text = ScriptLocalization.UI.CoopDisabled_Gamepad;
    }
    else if (!CoopManager.CoopActive && PlayerFarming.playersCount < 1)
    {
      this.DenyCoop = true;
      this._coopButton.Interactable = false;
      this._coopButtonText.text = ScriptLocalization.UI.CoopDisabled_Intro;
    }
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if (player.isLamb && (player.state.CURRENT_STATE == StateMachine.State.InActive || player.state.CURRENT_STATE == StateMachine.State.CustomAnimation || player.GoToAndStopping))
      {
        this.DenyCoop = true;
        this._coopButton.Interactable = false;
        this._coopButtonText.text = ScriptLocalization.UI.CoopDisabled_Interacting;
      }
    }
    if ((bool) (UnityEngine.Object) DeathCatRoomManager.Instance && (bool) (UnityEngine.Object) DeathCatRoomManager.Instance.gameObject && DeathCatRoomManager.Instance.gameObject.activeInHierarchy)
    {
      this.DenyCoop = true;
      this._coopButton.Interactable = false;
      this._coopButtonText.text = ScriptLocalization.UI.CoopDisabled_RespawnRoom;
    }
    if ((bool) (UnityEngine.Object) RespawnRoomManager.Instance && (bool) (UnityEngine.Object) RespawnRoomManager.Instance.gameObject && RespawnRoomManager.Instance.gameObject.activeInHierarchy)
    {
      this.DenyCoop = true;
      this._coopButton.Interactable = false;
      this._coopButtonText.text = ScriptLocalization.UI.CoopDisabled_RespawnRoom;
    }
    if ((UnityEngine.Object) CoopManager.Instance != (UnityEngine.Object) null && CoopManager.Instance.LockedAddRemoveCoopPlayer)
    {
      this.DenyCoop = true;
      this._coopButton.Interactable = false;
      this._coopButtonText.text = ScriptLocalization.UI.CoopDisabled_Intro;
    }
    if ((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null && BiomeGenerator.Instance.CurrentRoom != null && !BiomeGenerator.Instance.CurrentRoom.Active)
    {
      this.DenyCoop = true;
      this._coopButton.Interactable = false;
      this._coopButtonText.text = ScriptLocalization.UI.CoopDisabled_Generic;
    }
    if (!this.DenyCoop || !this.CoopButtonSelected)
      return;
    Debug.Log((object) "Leaving coop button on gamepad removed if it's selected");
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this._photoModeButton);
  }

  public void CoopButtonOnSelected()
  {
    Debug.Log((object) "Coop is selected");
    this.CoopButtonSelected = true;
  }

  public void CoopButtonOnDeselected()
  {
    Debug.Log((object) "Coop is deselected");
    this.CoopButtonSelected = false;
  }

  public override void OnHideStarted()
  {
    base.OnHideStarted();
    UIManager.PlayAudio("event:/ui/unpause");
    this._buttonHighlight.enabled = false;
  }

  public override void OnHideCompleted()
  {
    AudioManager.Instance.ResumePausedLoopsAndSFX();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable || HideSaveicon.IsSaving())
      return;
    this.Hide();
  }

  public override void OnPush() => this._buttonHighlight.enabled = false;

  public override void OnRelease() => this._buttonHighlight.enabled = true;

  [CompilerGenerated]
  public void \u003CStart\u003Eb__18_0()
  {
    this.Push<UIBugReportingOverlayController>(MonoSingleton<UIManager>.Instance.BugReportingOverlayTemplate);
  }

  [CompilerGenerated]
  public void \u003CLoadMainMenu\u003Eb__38_0() => this.Hide(true);
}
