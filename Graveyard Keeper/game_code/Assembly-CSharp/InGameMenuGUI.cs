// Decompiled with JetBrains decompiler
// Type: InGameMenuGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DLCRefugees;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class InGameMenuGUI : BaseMenuGUI
{
  public UILabel label_save_and_exit;
  public GamepadNavigationItem _stored_focus;

  public override void Open()
  {
    base.Open();
    MainGame.SetPausedMode(true);
    SmartAudioEngine.me.SetDullMusicMode();
    PlatformSpecific.SetGameStatus(GameEvents.GameStatus.InMenu);
    if (!BaseGUI.for_gamepad)
      return;
    this.gamepad_controller.ReinitItems(false);
    this.gamepad_controller.RestoreFocus();
    if ((Object) this._stored_focus != (Object) null)
      this.gamepad_controller.SetFocusedItem(this._stored_focus);
    this._stored_focus = (GamepadNavigationItem) null;
  }

  public void OnPressedContinue()
  {
    this.OnClosePressed();
    this._stored_focus = (GamepadNavigationItem) null;
  }

  public void OnPressedRestart()
  {
    this.Hide(true);
    MainGame.me.RestartDemoBuild();
  }

  public void OnPressedOptions()
  {
    if (BaseGUI.for_gamepad)
      this._stored_focus = this.gamepad_controller.focused_item;
    this.gameObject.Deactivate();
    GUIElements.me.options.Open();
  }

  public void OnPressedSaveAndExit()
  {
    this._stored_focus = (GamepadNavigationItem) null;
    this.SetControllsActive(false);
    this.OnClosePressed();
    GUIElements.me.dialog.OpenYesNo($"{GJL.L("exit_menu_confirm_txt")}\n\n{GJL.L("exit_menu_confirm")}", (GJCommons.VoidDelegate) (() => LoadingGUI.Show((GJCommons.VoidDelegate) (() => this.ReturnToMainMenu()))), on_hide: (GJCommons.VoidDelegate) (() => this.SetControllsActive(true)));
  }

  public override void OnAboveWindowClosed() => this.Open();

  public override void OnClosePressed()
  {
    this._stored_focus = (GamepadNavigationItem) null;
    base.OnClosePressed();
    SmartAudioEngine.me.SetDullMusicMode(false);
  }

  public override void Hide(bool play_sound = true)
  {
    MainGame.SetPausedMode(false);
    base.Hide(play_sound);
    PlatformSpecific.SetGameStatus(GameEvents.GameStatus.InGame);
  }

  public override bool OnPressedBack()
  {
    this.OnClosePressed();
    return true;
  }

  public void ReturnToMainMenu()
  {
    this._stored_focus = (GamepadNavigationItem) null;
    Debug.Log((object) nameof (ReturnToMainMenu));
    GUIElements.me.saves.StopPlayingGame();
    GUIElements.me.relation.Hide();
    GUIElements.me.main_menu.Open(true);
    LoadingGUI.Hide();
    RefugeesCampEngine.instance.DeInit();
  }

  public void OpenControlsHelpWindow()
  {
    if (BaseGUI.for_gamepad)
      this._stored_focus = this.gamepad_controller.focused_item;
    this.gameObject.Deactivate();
    GUIElements.me.tutorial.Open("controls");
    MainGame.SetPausedMode(true);
  }

  public void OpenTutorialsWindow()
  {
    if (BaseGUI.for_gamepad)
      this._stored_focus = this.gamepad_controller.focused_item;
    this.gameObject.Deactivate();
    GUIElements.me.tutorial_windows_gui.Open();
  }

  [CompilerGenerated]
  public void \u003COnPressedSaveAndExit\u003Eb__6_0()
  {
    LoadingGUI.Show((GJCommons.VoidDelegate) (() => this.ReturnToMainMenu()));
  }

  [CompilerGenerated]
  public void \u003COnPressedSaveAndExit\u003Eb__6_2() => this.ReturnToMainMenu();

  [CompilerGenerated]
  public void \u003COnPressedSaveAndExit\u003Eb__6_1() => this.SetControllsActive(true);
}
