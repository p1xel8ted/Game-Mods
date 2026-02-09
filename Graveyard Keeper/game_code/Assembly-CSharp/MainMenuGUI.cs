// Decompiled with JetBrains decompiler
// Type: MainMenuGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Steamworks;
using System;
using UnityEngine;

#nullable disable
public class MainMenuGUI : BaseMenuGUI
{
  public NewsGUI _news_gui;
  public SimpleUITable buttons_table;
  public MenuItemGUI mm_profiles;
  public MenuItemGUI mm_exit;
  public UILabel version_txt;
  public GameObject[] logos_dlc;
  public GamepadNavigationItem _last_focused_item;
  public GameObject pc2PreorderBanner;
  public GameObject pc2AvailableBanner;
  [SerializeField]
  public MainMenuLogoController _main_menu_logo_controller;
  public float shift;

  public override void Init()
  {
    this._news_gui = this.GetComponentInChildren<NewsGUI>();
    if ((UnityEngine.Object) this._news_gui != (UnityEngine.Object) null)
      this._news_gui.Init();
    this.ShiftMenuElementsForSmallResolution();
    base.Init();
  }

  public new void Open(bool switch_music = true)
  {
    Debug.Log((object) ("Open main menu, switch_music = " + switch_music.ToString()));
    GameSettings.me.ApplyVolume();
    MainGame.game_started = false;
    this.version_txt.text = $"ver. {LazyConsts.VERSION:0.000#}".Replace(",", ".");
    UtilityStuff.ProcessVersionLabel(this.version_txt);
    this.Open();
    LazyInput.on_input_changed += new System.Action(((BaseGUI) this).OnInputSourceChanged);
    MainGame.me.world_root.gameObject.SetActive(false);
    this.ShowDLCIcons();
    this.mm_profiles.gameObject.SetActive(false);
    this.mm_exit.gameObject.SetActive(true);
    this.buttons_table.Reposition();
    if (BaseGUI.for_gamepad)
    {
      if ((UnityEngine.Object) this._last_focused_item == (UnityEngine.Object) null)
        this.gamepad_controller.FocusOnFirstActive();
      else
        this.gamepad_controller.SetFocusedItem(this._last_focused_item);
    }
    if (switch_music)
    {
      DarkTonic.MasterAudio.MasterAudio.StopAllPlaylists();
      DarkTonic.MasterAudio.MasterAudio.TriggerPlaylistClip("menu", "menu");
    }
    if ((UnityEngine.Object) this._news_gui != (UnityEngine.Object) null)
      this._news_gui.Open();
    TitleScreen.Show();
    PlatformSpecific.SetGameStatus(GameEvents.GameStatus.InMenu);
    GUIElements.me.CloseAllInGameWindows();
    this.pc2AvailableBanner.gameObject.SetActive(false);
    this.pc2PreorderBanner.gameObject.SetActive(false);
    if (DateTime.Now > new DateTime(2023, 7, 23))
      this.pc2AvailableBanner.gameObject.SetActive(true);
    else
      this.pc2PreorderBanner.gameObject.SetActive(true);
  }

  public static void ShowDLCIcons(GameObject[] dlc_icons)
  {
    int index = 0;
    foreach (GameObject dlcIcon in dlc_icons)
      dlcIcon.SetActive(false);
    if (DLCEngine.IsDLCAvailable(DLCEngine.DLCVersion.BreakingDead))
    {
      index = 1;
      if (DLCEngine.IsDLCAvailable(DLCEngine.DLCVersion.Stories))
      {
        index = 4;
        if (DLCEngine.IsDLCAvailable(DLCEngine.DLCVersion.Refugees))
          index = 7;
      }
      else if (DLCEngine.IsDLCAvailable(DLCEngine.DLCVersion.Refugees))
        index = 5;
    }
    else if (DLCEngine.IsDLCAvailable(DLCEngine.DLCVersion.Stories))
    {
      index = 2;
      if (DLCEngine.IsDLCAvailable(DLCEngine.DLCVersion.Refugees))
        index = 6;
    }
    else if (DLCEngine.IsDLCAvailable(DLCEngine.DLCVersion.Refugees))
      index = 3;
    dlc_icons[index].SetActive(true);
  }

  public void ShowDLCIcons() => this._main_menu_logo_controller.ShowLogos();

  public void OnPressedPlay() => GUIElements.me.saves.Open();

  public void OnPressedContinue() => GUIElements.me.saves.Open();

  public void OnPressedProfile() => PlatformSpecific.OnProfileSelect();

  public void OnPressedOptions()
  {
    GUIElements.me.main_menu.Hide(true);
    GUIElements.me.options.Open();
  }

  public void OnPressedLiveStreaming()
  {
    GUIElements.me.main_menu.Hide(true);
    GUIElements.me.live_streaming.Open();
  }

  public void OnPressedCredits()
  {
    GUIElements.me.main_menu.Hide(true);
    GUIElements.me.credits.Open();
    GUIElements.me.hud.Hide();
  }

  public void OnBackFromCredits()
  {
    GUIElements.me.main_menu.Open(false);
    GUIElements.me.credits.Hide(true);
  }

  public void OnPressedExit() => Application.Quit();

  public override void Hide(bool play_sound = true)
  {
    if ((UnityEngine.Object) this._news_gui != (UnityEngine.Object) null)
      this._news_gui.Hide();
    LazyInput.on_input_changed -= new System.Action(((BaseGUI) this).OnInputSourceChanged);
    if (play_sound && BaseGUI.for_gamepad)
      this._last_focused_item = this.gamepad_controller.focused_item;
    base.Hide(play_sound);
  }

  public override void OnInputSourceChanged()
  {
    if (!this.is_shown_and_top)
      return;
    this.UpdateSourceType(true);
    this.InitPlatformDependentStuff();
    if (!this.is_shown || !BaseGUI.for_gamepad)
      return;
    LazyInput.ClearAllKeysDown();
    LazyInput.WaitForReleaseNavigationKeys();
    this.gamepad_controller.ReinitItems(true);
  }

  public override void UpdatTip(bool select_active)
  {
  }

  public void OnLeaveFeedbackButtonPressed()
  {
    Application.OpenURL("https://steamcommunity.com/app/599140/discussions/1/");
  }

  public void OnStrangerSinsBannerPressed()
  {
    PlatformSpecific.OpenStoreLink(DLCEngine.DLCVersion.Stories);
  }

  public void OnRefugeesBannerPressed()
  {
    PlatformSpecific.OpenStoreLink(DLCEngine.DLCVersion.Refugees);
  }

  public void OnSoulsBannerPressed() => PlatformSpecific.OpenStoreLink(DLCEngine.DLCVersion.Souls);

  public void OnPC2BannerPressed()
  {
    string str = "https://store.steampowered.com/app/1161590/Punch_Club_2_Fast_Forward/";
    if (SteamManager.Initialized)
    {
      Debug.Log((object) "SteamManager is initialized");
      SteamFriends.ActivateGameOverlayToWebPage(str);
    }
    else
    {
      Debug.Log((object) "SteamManager isn't initialized");
      Application.OpenURL(str);
    }
  }

  public void ShiftMenuElementsForSmallResolution()
  {
    double num = (double) Screen.width / (double) Screen.height;
    float y = 0.0f;
    if (num > 1.5)
    {
      if (Screen.width <= 1366)
      {
        y = -55f - this.shift;
        this.shift = -55f;
      }
      else if (Screen.width <= 1600 && DLCEngine.DLCAvailableCount() == 4)
      {
        y = -50f - this.shift;
        this.shift = -50f;
      }
      else if (Screen.width <= 1920 && DLCEngine.DLCAvailableCount() == 4)
      {
        y = -50f - this.shift;
        this.shift = -50f;
      }
      else
      {
        y = -this.shift;
        this.shift = 0.0f;
      }
    }
    Vector3 vector3 = new Vector3(0.0f, y, 0.0f);
    this.buttons_table.gameObject.transform.localPosition += vector3;
    this.version_txt.gameObject.transform.localPosition += vector3;
    for (int index = 0; index < this.logos_dlc.Length; ++index)
      this.logos_dlc[index].gameObject.transform.localPosition += vector3;
    this._main_menu_logo_controller.gameObject.transform.localPosition += vector3;
  }
}
