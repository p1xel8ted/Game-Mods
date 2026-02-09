// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MainMenu.MainMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI.SettingsMenu;
using MMTools;
using src.Extensions;
using src.Managers;
using src.UI;
using src.UI.Menus.Achievements_Menu;
using src.UINavigator;
using System;
using System.Runtime.CompilerServices;
using Unify;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.MainMenu;

public class MainMenu : UISubmenuBase
{
  public System.Action OnPlayButtonPressed;
  public System.Action OnDLCButtonPressed;
  [Header("Buttons")]
  [SerializeField]
  public Button _playButton;
  [SerializeField]
  public Button _dlcButton;
  [SerializeField]
  public Button _settingsButton;
  [SerializeField]
  public Button _creditsButton;
  [SerializeField]
  public Button _roadmapButton;
  [SerializeField]
  public Button _achievementsButton;
  [SerializeField]
  public Button _quitButton;
  [Header("Other")]
  [SerializeField]
  public UINavigatorFollowElement _buttonHighlight;
  [SerializeField]
  public GameObject _UserPicker;
  public bool WillRequirePostGameReveal;

  public Button PlayButton => this._playButton;

  public void Start()
  {
    this._playButton.onClick.AddListener(new UnityAction(this.OnPlayButtonClicked));
    this._settingsButton.onClick.AddListener(new UnityAction(this.OnSettingsButtonClicked));
    this._creditsButton.onClick.AddListener(new UnityAction(this.OnCreditsButtonClicked));
    this._roadmapButton.onClick.AddListener(new UnityAction(this.OnRoadmapButtonClicked));
    this._achievementsButton.onClick.AddListener(new UnityAction(this.OnAchievementsButtonCLicked));
    this._dlcButton.onClick.AddListener(new UnityAction(this.OnDLCButtonClicked));
    this._quitButton.onClick.AddListener(new UnityAction(this.OnQuitButtonClicked));
    if (CheatConsole.IN_DEMO)
      this.EnableDemo(true);
    SessionManager.OnSessionStart += new SessionManager.SessionEventDelegate(this.ForceSelectPlayOnSessionStart);
  }

  public void ForceSelectPlayOnSessionStart(Guid sessionGuid, User sessionUser)
  {
    this.SetActiveStateForMenu(true);
  }

  public void EnableDemo(bool AlreadyInDemo)
  {
    this._creditsButton.gameObject.SetActive(false);
    this._settingsButton.gameObject.SetActive(false);
    this._quitButton.gameObject.SetActive(false);
    if (AlreadyInDemo)
      return;
    UISettingsMenuController activeMenu = UIManager.GetActiveMenu<UISettingsMenuController>();
    if ((UnityEngine.Object) activeMenu != (UnityEngine.Object) null)
      activeMenu.Hide(true);
    MMTransition.StopCurrentTransition();
    MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade, "Main Menu", 1f, "", (System.Action) (() => MonoSingleton<UINavigatorNew>.Instance.Clear()));
  }

  public override void OnShowCompleted()
  {
    if (!this.WillRequirePostGameReveal)
      return;
    UIMenuConfirmationWindow confirmationWindow = MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate.Instantiate<UIMenuConfirmationWindow>();
    confirmationWindow.Configure(ScriptLocalization.UI_PostGameUnlock.Header, ScriptLocalization.UI_PostGameUnlock.Description, true);
    confirmationWindow.Show();
    this.SetActiveStateForMenu(false);
    confirmationWindow.OnHidden = confirmationWindow.OnHidden + (System.Action) (() =>
    {
      this.WillRequirePostGameReveal = false;
      PersistenceManager.PersistentData.PostGameRevealed = true;
      PersistenceManager.Save();
      this.SetActiveStateForMenu(true);
    });
  }

  public void OnPlayButtonClicked()
  {
    this.OverrideDefaultOnce((Selectable) this._playButton);
    System.Action playButtonPressed = this.OnPlayButtonPressed;
    if (playButtonPressed == null)
      return;
    playButtonPressed();
  }

  public void OnDLCButtonClicked()
  {
    this.OverrideDefaultOnce((Selectable) this._dlcButton);
    System.Action dlcButtonPressed = this.OnDLCButtonPressed;
    if (dlcButtonPressed == null)
      return;
    dlcButtonPressed();
  }

  public void OnSettingsButtonClicked()
  {
    this.Push<UISettingsMenuController>(MonoSingleton<UIManager>.Instance.SettingsMenuControllerTemplate);
  }

  public void OnCreditsButtonClicked()
  {
    this.Push<UICreditsMenuController>(MonoSingleton<UIManager>.Instance.CreditsMenuControllerTemplate);
  }

  public void OnRoadmapButtonClicked()
  {
    this.Push<UIRoadmapOverlayController>(MonoSingleton<UIManager>.Instance.RoadmapOverlayControllerTemplate);
  }

  public void OnAchievementsButtonCLicked()
  {
    this.Push<UIAchievementsMenuController>(MonoSingleton<UIManager>.Instance.AchievementsMenuTemplate);
  }

  public void OnQuitButtonClicked()
  {
    UIMenuConfirmationWindow confirmationWindow = this.Push<UIMenuConfirmationWindow>(MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate);
    confirmationWindow.Configure(ScriptLocalization.UI.Quit_Game, ScriptLocalization.UI.ConfirmQuit);
    confirmationWindow.OnConfirm += (System.Action) (() => Application.Quit());
  }

  public override void OnPush() => this._buttonHighlight.enabled = false;

  public override void OnRelease() => this._buttonHighlight.enabled = true;

  public void Update()
  {
    if (UnifyManager.platform != UnifyManager.Platform.GameCoreConsole && UnifyManager.platform != UnifyManager.Platform.GameCore)
      return;
    if (this._settingsButton.interactable && this._creditsButton.interactable && this._playButton.interactable)
      this._UserPicker.SetActive(true);
    else
      this._UserPicker.SetActive(false);
  }

  [CompilerGenerated]
  public void \u003COnShowCompleted\u003Eb__17_0()
  {
    this.WillRequirePostGameReveal = false;
    PersistenceManager.PersistentData.PostGameRevealed = true;
    PersistenceManager.Save();
    this.SetActiveStateForMenu(true);
  }
}
