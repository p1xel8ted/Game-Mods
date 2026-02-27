// Decompiled with JetBrains decompiler
// Type: Lamb.UI.SettingsMenu.ControlSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Rewired;
using System;
using UnityEngine;

#nullable disable
namespace Lamb.UI.SettingsMenu;

public class ControlSettings : UISubmenuBase
{
  [Header("General")]
  [SerializeField]
  private MMScrollRect _scrollRect;
  [SerializeField]
  private GameObject _resetBindingPrompt;
  [SerializeField]
  private GameObject _unbindPrompt;
  [Header("Categories")]
  [SerializeField]
  private ControlsScreenBase[] _screens;
  private ControlsScreenBase _currentScreen;
  private InputType _currentInputType;

  public override void Awake()
  {
    base.Awake();
    this._parent.OnHide += new System.Action(((UIMenuBase) this).OnHideStarted);
  }

  private void OnEnable()
  {
    InputManager.General.OnActiveControllerChanged += new Action<Controller>(this.OnActiveControllerChanged);
  }

  private void OnDisable()
  {
    InputManager.General.OnActiveControllerChanged -= new Action<Controller>(this.OnActiveControllerChanged);
    this._resetBindingPrompt.SetActive(false);
    this._unbindPrompt.SetActive(false);
  }

  protected override void OnShowStarted()
  {
    this._scrollRect.normalizedPosition = Vector2.one;
    this.SetActiveController(InputManager.General.GetLastActiveController());
    UIManager.PlayAudio("event:/ui/change_selection");
  }

  protected override void OnHideStarted()
  {
    this._resetBindingPrompt.SetActive(false);
    this._unbindPrompt.SetActive(false);
    this._scrollRect.enabled = false;
  }

  private void OnActiveControllerChanged(Controller controller)
  {
    this.SetActiveController(controller);
  }

  private void SetActiveController(Controller controller)
  {
    this.SetActiveInputType(ControlUtilities.GetCurrentInputType(controller));
  }

  private void SetActiveInputType(InputType inputType)
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

  private void Update()
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
