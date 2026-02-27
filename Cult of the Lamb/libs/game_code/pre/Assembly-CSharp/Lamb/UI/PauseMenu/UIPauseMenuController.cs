// Decompiled with JetBrains decompiler
// Type: Lamb.UI.PauseMenu.UIPauseMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI.SettingsMenu;
using MMBiomeGeneration;
using MMTools;
using src.Extensions;
using src.UI;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.PauseMenu;

public class UIPauseMenuController : UIMenuBase
{
  [Header("Buttons")]
  [SerializeField]
  private Button _resumeButton;
  [SerializeField]
  private Button _saveButton;
  [SerializeField]
  private Button _settingsButton;
  [SerializeField]
  private Button _helpButton;
  [SerializeField]
  private Button _mainMenuButton;
  [SerializeField]
  private Button _quitButton;
  [Header("Sidebar")]
  [SerializeField]
  private MMButton _bugReportButton;
  [SerializeField]
  private MMButton _discordButton;
  [Header("Other")]
  [SerializeField]
  private TextMeshProUGUI _seedText;
  [SerializeField]
  private TextMeshProUGUI _saveButtonText;
  [SerializeField]
  private UINavigatorFollowElement _buttonHighlight;
  private bool saved;

  public override void Awake()
  {
    base.Awake();
    this.SetActiveStateForMenu(false);
  }

  private void Start()
  {
    MMVibrate.StopRumble();
    AudioManager.Instance.PauseActiveLoops();
    this._resumeButton.onClick.AddListener(new UnityAction(this.OnResumeButtonPressed));
    this._saveButton.onClick.AddListener(new UnityAction(this.OnSaveButtonPressed));
    this._settingsButton.onClick.AddListener(new UnityAction(this.OnSettingsButtonPressed));
    this._helpButton.onClick.AddListener(new UnityAction(this.OnHelpButtonPressed));
    this._mainMenuButton.onClick.AddListener(new UnityAction(this.OnMainMenuButtonPressed));
    this._quitButton.onClick.AddListener(new UnityAction(this.OnQuitButtonPressed));
    this._seedText.text = (UnityEngine.Object) BiomeGenerator.Instance == (UnityEngine.Object) null ? "" : BiomeGenerator.Instance.Seed.ToString();
    this._bugReportButton.onClick.AddListener((UnityAction) (() => this.Push<UIBugReportingOverlayController>(MonoSingleton<UIManager>.Instance.BugReportingOverlayTemplate)));
    this._discordButton.onClick.AddListener((UnityAction) (() => Application.OpenURL("https://discord.com/invite/massivemonster")));
    this.StartCoroutine((IEnumerator) this.UpdateLoop());
  }

  private void OnEnable()
  {
    this.saved = false;
    SaveAndLoad.OnSaveCompleted += new System.Action(this.OnSaveFinalized);
    SaveAndLoad.OnSaveError += new Action<MMReadWriteError>(this.OnSaveError);
  }

  private void OnDisable()
  {
    SaveAndLoad.OnSaveCompleted -= new System.Action(this.OnSaveFinalized);
    SaveAndLoad.OnSaveError -= new Action<MMReadWriteError>(this.OnSaveError);
  }

  private IEnumerator UpdateLoop()
  {
    UIPauseMenuController pauseMenuController = this;
    bool closable = true;
    while (true)
    {
      yield return (object) null;
      if (!closable || !pauseMenuController._canvasGroup.interactable || !InputManager.Gameplay.GetPauseButtonDown())
        closable = UIMenuBase.ActiveMenus.Count == 1;
      else
        break;
    }
    pauseMenuController.OnCancelButtonInput();
  }

  private void Update() => Time.timeScale = 0.0f;

  private void OnResumeButtonPressed() => this.Hide();

  private void OnSaveButtonPressed()
  {
    this._saveButtonText.text = ScriptLocalization.UI.Saving;
    SaveAndLoad.Save();
  }

  private void OnSaveFinalized()
  {
    this._saveButtonText.text = ScriptLocalization.UI.Saved;
    this.saved = true;
  }

  private void OnSaveError(MMReadWriteError error)
  {
    this._saveButtonText.text = ScriptLocalization.UI_SaveError.Title;
    this.saved = true;
    UIMenuConfirmationWindow menu = MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate.Instantiate<UIMenuConfirmationWindow>();
    menu.Configure(ScriptLocalization.UI_SaveError.Title, ScriptLocalization.UI_SaveError.Description.NewLine(), true);
    menu.Show();
    this.PushInstance<UIMenuConfirmationWindow>(menu);
  }

  private void OnSettingsButtonPressed()
  {
    this.Push<UISettingsMenuController>(MonoSingleton<UIManager>.Instance.SettingsMenuControllerTemplate);
  }

  private void OnHelpButtonPressed()
  {
    this.Push<UITutorialMenuController>(MonoSingleton<UIManager>.Instance.TutorialMenuTemplate);
  }

  private void OnMainMenuButtonPressed()
  {
    UIMenuConfirmationWindow confirmationWindow = this.Push<UIMenuConfirmationWindow>(MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate);
    confirmationWindow.Configure(ScriptLocalization.UI_Generic.Quit, !this.saved ? ScriptLocalization.UI.ConfirmQuitSaveWarning : ScriptLocalization.UI.ConfirmQuit);
    confirmationWindow.OnConfirm += new System.Action(this.LoadMainMenu);
  }

  private void LoadMainMenu()
  {
    this.SetActiveStateForMenu(false);
    SimulationManager.Pause();
    KeyboardLightingManager.Reset();
    FollowerManager.Reset();
    StructureManager.Reset();
    UIDynamicNotificationCenter.Reset();
    GameManager.GoG_Initialised = false;
    TwitchManager.Abort();
    MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade, "Main Menu", 1f, "", (System.Action) (() => this.Hide(true)));
  }

  private void OnQuitButtonPressed()
  {
    UIMenuConfirmationWindow confirmationWindow = this.Push<UIMenuConfirmationWindow>(MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate);
    confirmationWindow.Configure(ScriptLocalization.UI_Generic.Quit, !this.saved ? ScriptLocalization.UI.ConfirmQuitSaveWarning : ScriptLocalization.UI.ConfirmQuit);
    confirmationWindow.OnConfirm += (System.Action) (() => Application.Quit());
  }

  protected override void OnShowStarted()
  {
    base.OnShowStarted();
    MMVibrate.StopRumble();
    UIManager.PlayAudio("event:/ui/pause");
  }

  protected override void SetActiveStateForMenu(bool state)
  {
    base.SetActiveStateForMenu(state);
    if (!state)
      return;
    this._saveButton.interactable = (UnityEngine.Object) BiomeGenerator.Instance == (UnityEngine.Object) null;
    this._helpButton.interactable = DataManager.Instance.RevealedTutorialTopics.Count > 0;
  }

  protected override void OnHideStarted()
  {
    AudioManager.Instance.ResumeActiveLoops();
    base.OnHideStarted();
    UIManager.PlayAudio("event:/ui/unpause");
    this._buttonHighlight.enabled = false;
  }

  protected override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  protected override void OnPush() => this._buttonHighlight.enabled = false;

  protected override void OnRelease() => this._buttonHighlight.enabled = true;
}
