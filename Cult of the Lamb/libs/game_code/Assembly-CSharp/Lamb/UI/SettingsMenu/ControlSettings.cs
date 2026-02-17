// Decompiled with JetBrains decompiler
// Type: Lamb.UI.SettingsMenu.ControlSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Rewired;
using src.UINavigator;
using System;
using UnityEngine;

#nullable disable
namespace Lamb.UI.SettingsMenu;

public class ControlSettings : UISubmenuBase
{
  [Header("General")]
  [SerializeField]
  public MMScrollRect _scrollRect;
  [SerializeField]
  public GameObject _resetBindingPrompt;
  [SerializeField]
  public GameObject _unbindPrompt;
  [Header("Categories")]
  [SerializeField]
  public ControlsScreenBase[] _screens;
  public ControlsScreenBase _currentScreen;
  public InputType _currentInputType;

  public override void Awake()
  {
    base.Awake();
    this._parent.OnHide += new System.Action(((UIMenuBase) this).OnHideStarted);
  }

  public void OnEnable()
  {
    InputManager.General.OnActiveControllerChanged += new Action<Controller>(this.OnActiveControllerChanged);
  }

  public new void OnDisable()
  {
    InputManager.General.OnActiveControllerChanged -= new Action<Controller>(this.OnActiveControllerChanged);
    this._resetBindingPrompt.SetActive(false);
    this._unbindPrompt.SetActive(false);
  }

  public override void OnShowStarted()
  {
    this._scrollRect.normalizedPosition = Vector2.one;
    if ((bool) (UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer)
      this.SetActiveController(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer);
    else
      this.SetActiveController(InputManager.General.GetLastActiveController());
    UIManager.PlayAudio("event:/ui/change_selection");
  }

  public override void OnHideStarted()
  {
    this._resetBindingPrompt.SetActive(false);
    this._unbindPrompt.SetActive(false);
    this._scrollRect.enabled = false;
    foreach (MonoBehaviour screen in this._screens)
      screen.StopAllCoroutines();
  }

  public void OnActiveControllerChanged(Controller controller)
  {
    this.SetActiveController(controller);
  }

  public void SetActiveController(PlayerFarming playerFarming)
  {
    this.SetActiveController(playerFarming.rewiredPlayer.controllers.GetLastActiveController());
  }

  public void SetActiveController(Controller controller)
  {
    this.SetActiveInputType(ControlUtilities.GetCurrentInputType(controller));
  }

  public void SetActiveInputType(InputType inputType)
  {
    if (this._currentInputType != inputType)
    {
      this._scrollRect.enabled = false;
      this._scrollRect.normalizedPosition = Vector2.one;
      foreach (ControlsScreenBase screen in this._screens)
      {
        if (screen.ValidInputType(inputType))
        {
          if ((UnityEngine.Object) screen != (UnityEngine.Object) this._currentScreen)
          {
            this._currentScreen = screen;
            this._currentScreen.Show();
          }
          this._currentScreen.Configure(inputType);
        }
        else if (screen.isActiveAndEnabled)
          screen.Hide();
      }
      this._currentInputType = inputType;
    }
    this._scrollRect.enabled = true;
  }

  public void Update()
  {
    if (!((UnityEngine.Object) this._currentScreen != (UnityEngine.Object) null))
      return;
    this._resetBindingPrompt.SetActive(this._currentScreen.ShowBindingPrompts());
    this._unbindPrompt.SetActive(this._currentScreen.ShowBindingPrompts());
  }

  public void Configure(SettingsData.ControlSettings controlSettings)
  {
    foreach (ControlsScreenBase screen in this._screens)
      screen.Configure(controlSettings);
  }

  public void Reset()
  {
    SettingsManager.Settings.Control = new SettingsData.ControlSettings();
    InputManager.General.ResetBindings();
  }
}
