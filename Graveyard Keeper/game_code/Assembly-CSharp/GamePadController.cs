// Decompiled with JetBrains decompiler
// Type: GamePadController
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Rewired;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GamePadController : BaseInputController
{
  public Dictionary<GamePadButton, int> _rewired_bindings = new Dictionary<GamePadButton, int>()
  {
    {
      GamePadButton.A,
      2
    },
    {
      GamePadButton.B,
      3
    },
    {
      GamePadButton.X,
      4
    },
    {
      GamePadButton.Y,
      5
    },
    {
      GamePadButton.LB,
      6
    },
    {
      GamePadButton.RB,
      7
    },
    {
      GamePadButton.Back,
      8
    },
    {
      GamePadButton.Start,
      9
    },
    {
      GamePadButton.DUp,
      10
    },
    {
      GamePadButton.DDown,
      11
    },
    {
      GamePadButton.DLeft,
      12
    },
    {
      GamePadButton.DRight,
      13
    },
    {
      GamePadButton.RT,
      19
    },
    {
      GamePadButton.LT,
      20
    }
  };
  public Dictionary<GameKey, GamePadButton> _bindings = KeyBindings.gamepad_bindings;
  public Stick _stick;
  public Stick _stick2;
  public Player _rewired_player;
  public NavigationStick vertical_navigation;
  public NavigationStick horizontal_navigation;
  public bool _no_horizontal_dir;
  public bool _no_vertical_dir;
  public GamePadController.GuiNavigationAxis _last_gui_navigation_axis;
  public float _gui_navigation_delay;
  public GamePadButton _emulated_key;
  public float _emulated_key_time_left;
  public bool _emulated_key_just_pressed;
  public bool _pause_after_emulated_key;
  public static bool cheat_combination_pressed;

  public bool active_at_start => false;

  public GamePadController()
  {
    this._rewired_player = ReInput.players.GetPlayer(0);
    this._stick = new Stick(0, 1);
    this._stick2 = new Stick(21, 22);
    this.vertical_navigation = new NavigationStick(true, this);
    this.horizontal_navigation = new NavigationStick(false, this);
  }

  public override void Update()
  {
    base.Update();
    GamePadController.cheat_combination_pressed = false;
    if (this._rewired_player == null)
    {
      Debug.LogError((object) "Fatal error, _rewired_player is null. Trying to fix...");
      this._rewired_player = ReInput.players.GetPlayer(0);
      if (this._rewired_player == null)
        return;
    }
    this._stick.Update(this._rewired_player);
    this._stick2.Update(this._rewired_player);
    this.dir = this._stick.has_direction ? this._stick.direction : Vector2.zero;
    this.dir2 = this._stick2.has_direction ? this._stick2.direction : Vector2.zero;
    foreach (KeyValuePair<GameKey, GamePadButton> binding in this._bindings)
    {
      int rewiredBinding = this._rewired_bindings[binding.Value];
      if (this._rewired_player.GetButtonDown(rewiredBinding) || this.IsEmulatedPress(binding.Value))
        this.AddPressed(binding.Key);
      if (this._rewired_player.GetButton(rewiredBinding) || this.IsEmulatedHold(binding.Value))
      {
        this.AddHolded(binding.Key);
        if (rewiredBinding >= 10 && rewiredBinding <= 13)
          this.dir = Vector2.zero;
      }
    }
    this.ResetEmulatePressState();
    if (this._emulated_key == GamePadButton.None)
    {
      this.UpdateStickNavigation(this.dir);
    }
    else
    {
      switch (this._emulated_key)
      {
        case GamePadButton.Left:
          this.dir = Vector2.left;
          break;
        case GamePadButton.Right:
          this.dir = Vector2.right;
          break;
        case GamePadButton.Up:
          this.dir = Vector2.up;
          break;
        case GamePadButton.Down:
          this.dir = Vector2.down;
          break;
      }
      this.vertical_navigation.Update(this.dir);
      this.horizontal_navigation.Update(this.dir);
    }
    GamePadController.cheat_combination_pressed = this._rewired_player.GetButton(this._rewired_bindings[GamePadButton.LB]) && this._rewired_player.GetButton(this._rewired_bindings[GamePadButton.LT]) && this._rewired_player.GetButton(this._rewired_bindings[GamePadButton.RB]) && this._rewired_player.GetButton(this._rewired_bindings[GamePadButton.RT]) && (this._rewired_player.GetButtonDown(this._rewired_bindings[GamePadButton.LB]) || this._rewired_player.GetButtonDown(this._rewired_bindings[GamePadButton.LT]) || this._rewired_player.GetButtonDown(this._rewired_bindings[GamePadButton.RB]) || this._rewired_player.GetButtonDown(this._rewired_bindings[GamePadButton.RT]));
    if (this._emulated_key == GamePadButton.None || (double) this._emulated_key_time_left <= 0.0)
      return;
    this._emulated_key_time_left -= Time.deltaTime;
    if ((double) this._emulated_key_time_left > 0.0)
      return;
    this._emulated_key = GamePadButton.None;
    if (!this._pause_after_emulated_key)
      return;
    Time.timeScale = 0.0f;
  }

  public void UpdateStickNavigation(Vector2 gui_navigation)
  {
    if ((double) gui_navigation.magnitude > 0.0)
    {
      GamePadController.GuiNavigationAxis guiNavigationAxis;
      if ((double) Mathf.Abs(gui_navigation.x) > (double) Mathf.Abs(gui_navigation.y))
      {
        gui_navigation.y = 0.0f;
        guiNavigationAxis = GamePadController.GuiNavigationAxis.Horizontal;
      }
      else
      {
        gui_navigation.x = 0.0f;
        guiNavigationAxis = GamePadController.GuiNavigationAxis.Vertical;
      }
      if (this._last_gui_navigation_axis != GamePadController.GuiNavigationAxis.None && this._last_gui_navigation_axis != guiNavigationAxis)
        this._gui_navigation_delay = 0.11f;
      this._last_gui_navigation_axis = guiNavigationAxis;
      if ((double) this._gui_navigation_delay > 0.0)
        gui_navigation = Vector2.zero;
      this._gui_navigation_delay -= Time.deltaTime;
    }
    else
      this._last_gui_navigation_axis = GamePadController.GuiNavigationAxis.None;
    this.vertical_navigation.Update(gui_navigation);
    this.horizontal_navigation.Update(gui_navigation);
  }

  public void AddPressed(GameKey key)
  {
    this.pressed_keys.Add(key);
    if (key == GameKey.Right)
      this.AddPressed(GameKey.SliderInc);
    if (key != GameKey.Left)
      return;
    this.AddPressed(GameKey.SliderDec);
  }

  public void AddHolded(GameKey key)
  {
    if (!this.holded_keys.Contains(key))
      this.holded_keys.Add(key);
    if (key == GameKey.Right)
      this.AddHolded(GameKey.SliderInc);
    if (key != GameKey.Left)
      return;
    this.AddHolded(GameKey.SliderDec);
  }

  public void Vibrate(float value, float duration)
  {
    if (this._rewired_player == null)
      return;
    foreach (Joystick joystick in (IEnumerable<Joystick>) this._rewired_player.controllers.Joysticks)
    {
      if (joystick.supportsVibration)
        joystick.SetVibration(value, value, duration, duration);
    }
  }

  public bool IsEmulatedHold(GamePadButton button) => this._emulated_key == button;

  public bool IsEmulatedPress(GamePadButton button)
  {
    return this._emulated_key == button && this._emulated_key_just_pressed;
  }

  public void EmulateKeyPress(GamePadButton button, float len = 0.2f, bool pause_after_release = true)
  {
    if (pause_after_release)
      Time.timeScale = 1f;
    this._emulated_key = button;
    this._emulated_key_time_left = len;
    this._emulated_key_just_pressed = true;
    this._pause_after_emulated_key = pause_after_release;
  }

  public void ResetEmulatePressState() => this._emulated_key_just_pressed = false;

  public override bool IsActive() => base.IsActive() || this._emulated_key != 0;

  public enum GuiNavigationAxis
  {
    None,
    Vertical,
    Horizontal,
  }
}
