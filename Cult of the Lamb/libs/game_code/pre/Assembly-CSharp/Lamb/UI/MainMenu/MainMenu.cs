// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MainMenu.MainMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI.SettingsMenu;
using MMTools;
using src.UI;
using src.UINavigator;
using Unify;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.MainMenu;

public class MainMenu : UISubmenuBase
{
  public System.Action OnPlayButtonPressed;
  [Header("Buttons")]
  [SerializeField]
  private Button _playButton;
  [SerializeField]
  private Button _settingsButton;
  [SerializeField]
  private Button _creditsButton;
  [SerializeField]
  private Button _roadmapButton;
  [SerializeField]
  private Button _quitButton;
  [Header("Other")]
  [SerializeField]
  private UINavigatorFollowElement _buttonHighlight;
  [SerializeField]
  private GameObject _UserPicker;

  public void Start()
  {
    this._playButton.onClick.AddListener(new UnityAction(this.OnPlayButtonClicked));
    this._settingsButton.onClick.AddListener(new UnityAction(this.OnSettingsButtonClicked));
    this._creditsButton.onClick.AddListener(new UnityAction(this.OnCreditsButtonClicked));
    this._roadmapButton.onClick.AddListener(new UnityAction(this.OnRoadmapButtonClicked));
    this._quitButton.onClick.AddListener(new UnityAction(this.OnQuitButtonClicked));
    if (!CheatConsole.IN_DEMO)
      return;
    this.EnableDemo(true);
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

  private void OnPlayButtonClicked()
  {
    this.OverrideDefaultOnce((Selectable) this._playButton);
    System.Action playButtonPressed = this.OnPlayButtonPressed;
    if (playButtonPressed == null)
      return;
    playButtonPressed();
  }

  private void OnSettingsButtonClicked()
  {
    this.Push<UISettingsMenuController>(MonoSingleton<UIManager>.Instance.SettingsMenuControllerTemplate);
  }

  private void OnCreditsButtonClicked()
  {
    this.Push<UICreditsMenuController>(MonoSingleton<UIManager>.Instance.CreditsMenuControllerTemplate);
  }

  private void OnRoadmapButtonClicked()
  {
    this.Push<UIRoadmapOverlayController>(MonoSingleton<UIManager>.Instance.RoadmapOverlayControllerTemplate);
  }

  private void OnQuitButtonClicked()
  {
    UIMenuConfirmationWindow confirmationWindow = this.Push<UIMenuConfirmationWindow>(MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate);
    confirmationWindow.Configure(ScriptLocalization.UI.Quit_Game, ScriptLocalization.UI.ConfirmQuit);
    confirmationWindow.OnConfirm += (System.Action) (() => Application.Quit());
  }

  protected override void OnPush() => this._buttonHighlight.enabled = false;

  protected override void OnRelease() => this._buttonHighlight.enabled = true;

  private void Update()
  {
    if (UnifyManager.platform != UnifyManager.Platform.GameCoreConsole && UnifyManager.platform != UnifyManager.Platform.GameCore)
      return;
    if (this._settingsButton.interactable && this._creditsButton.interactable && this._playButton.interactable)
      this._UserPicker.SetActive(true);
    else
      this._UserPicker.SetActive(false);
  }
}
