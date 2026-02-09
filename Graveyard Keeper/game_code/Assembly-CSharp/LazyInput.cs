// Decompiled with JetBrains decompiler
// Type: LazyInput
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class LazyInput : MonoBehaviour
{
  public const float HOLD_PRESS_DELAY = 0.3f;
  public const float DIRECTIONS_DELAY = 0.11f;
  public const float TABS_DELAY = 0.35f;
  public const float SLIDER_DELAY = 0.07f;
  public static GameKey[] toolbar_keys = new GameKey[4]
  {
    GameKey.Toolbar1,
    GameKey.Toolbar2,
    GameKey.Toolbar3,
    GameKey.Toolbar4
  };
  public static Dictionary<GameKey, float> _can_be_holded = new Dictionary<GameKey, float>()
  {
    {
      GameKey.Left,
      0.11f
    },
    {
      GameKey.Right,
      0.11f
    },
    {
      GameKey.Up,
      0.11f
    },
    {
      GameKey.Down,
      0.11f
    },
    {
      GameKey.PrevTab,
      0.35f
    },
    {
      GameKey.NextTab,
      0.35f
    },
    {
      GameKey.PrevSubTab,
      0.35f
    },
    {
      GameKey.NextSubTub,
      0.35f
    },
    {
      GameKey.SliderDec,
      0.07f
    },
    {
      GameKey.SliderInc,
      0.07f
    }
  };
  public static bool _cached;
  public static bool _gamepad_active = true;
  public static LazyInput _me;
  public static GamePadController _gamepad;
  public static KeyboardController _keyboard;
  public List<GameKey> _holded_keys = new List<GameKey>();
  public List<GameKey> _pressed_keys = new List<GameKey>();
  public List<GameKey> _wait_for_release = new List<GameKey>();
  public Vector2 _direction = Vector2.zero;
  public Vector2 _direction2 = Vector2.zero;
  public List<GameKey> _holded_for_press = new List<GameKey>();
  public List<float> _holded_for_press_delays = new List<float>();
  public Dictionary<LazyInput.HoldedGroup, float> _last_releases_in_group = new Dictionary<LazyInput.HoldedGroup, float>();
  public GameKey simulate_hold_key;
  public Vector2 simulate_direction = Vector2.zero;

  public static event System.Action on_input_changed;

  public void PressEmulateLeft() => LazyInput.EmulateGamepadPress(GamePadButton.Left);

  public void PressEmulateRight() => LazyInput.EmulateGamepadPress(GamePadButton.Right);

  public void PressEmulateUp() => LazyInput.EmulateGamepadPress(GamePadButton.Up);

  public void PressEmulateDown() => LazyInput.EmulateGamepadPress(GamePadButton.Down);

  public void ResumeTime() => Time.timeScale = 1f;

  public static bool gamepad_active => LazyInput._gamepad_active;

  public static LazyInput me
  {
    get
    {
      if (!LazyInput._cached)
      {
        LazyInput._me = SingletonGameObjects.FindOrCreate<LazyInput>();
        LazyInput._gamepad = new GamePadController();
        LazyInput._keyboard = new KeyboardController();
        LazyInput._gamepad_active = LazyInput._gamepad.active_at_start;
        LazyInput._cached = true;
      }
      return LazyInput._me;
    }
  }

  public static void Init() => Debug.Log((object) (LazyInput.me.name + " started"));

  public void Update()
  {
    LazyInput._gamepad.Update();
    LazyInput._keyboard.Update();
    this._pressed_keys.Clear();
    this._holded_keys.Clear();
    bool flag = false;
    if (LazyInput._gamepad.IsActive())
    {
      flag = !LazyInput._gamepad_active;
      LazyInput._gamepad_active = true;
    }
    else if (LazyInput._keyboard.IsActive())
    {
      flag = LazyInput._gamepad_active;
      LazyInput._gamepad_active = false;
    }
    if (flag && LazyInput.on_input_changed != null)
      LazyInput.on_input_changed();
    foreach (GameKey key in LazyInput._gamepad_active ? LazyInput._gamepad.pressed_keys : LazyInput._keyboard.pressed_keys)
      this.AddPressed(key);
    foreach (GameKey key in LazyInput._gamepad_active ? LazyInput._gamepad.holded_keys : LazyInput._keyboard.holded_keys)
      this.AddHolded(key);
    if (this.simulate_hold_key != GameKey.None)
      this.AddHolded(this.simulate_hold_key);
    this._direction = LazyInput._gamepad_active ? LazyInput._gamepad.direction : LazyInput._keyboard.direction;
    this._direction2 = LazyInput._gamepad_active ? LazyInput._gamepad.direction2 : LazyInput._keyboard.direction2;
    if ((double) this.simulate_direction.magnitude > 0.0)
      this._direction = this.simulate_direction;
    this.UpdateHolded(Time.deltaTime);
  }

  public static bool GetKey(GameKey key)
  {
    return !LazyInput.me._wait_for_release.Contains(key) && LazyInput.me._holded_keys.Contains(key);
  }

  public static bool AnyKeyDown() => LazyInput.me._pressed_keys.Count > 0;

  public static bool AnyClick()
  {
    return LazyInput.GetKeyDown(GameKey.LeftClick) || LazyInput.GetKeyDown(GameKey.RightClick);
  }

  public static bool GetKeyDown(GameKey key)
  {
    return !LazyInput.me._wait_for_release.Contains(key) && LazyInput.me._pressed_keys.Contains(key);
  }

  public static Vector2 GetDirection() => LazyInput.me._direction;

  public static Vector2 GetDirection2() => LazyInput.me._direction2;

  public static void ClearKey(GameKey key) => LazyInput.me._holded_keys.Remove(key);

  public static void ClearKeyDown(GameKey key) => LazyInput.me._pressed_keys.Remove(key);

  public static void ClearAllKeysDown() => LazyInput.me._pressed_keys.Clear();

  public static void WaitForRelease(GameKey key)
  {
    if (key == GameKey.None || LazyInput.me._wait_for_release.Contains(key))
      return;
    LazyInput.me._wait_for_release.Add(key);
  }

  public static void WaitForReleaseNavigationKeys()
  {
    LazyInput.WaitForRelease(GameKey.Left);
    LazyInput.WaitForRelease(GameKey.Right);
    LazyInput.WaitForRelease(GameKey.Up);
    LazyInput.WaitForRelease(GameKey.Down);
  }

  public static void WaitForReleaseMouseKeys()
  {
    LazyInput.WaitForRelease(GameKey.LeftClick);
    LazyInput.WaitForRelease(GameKey.RightClick);
    foreach (GameKey key in KeyBindings.mouse_bindings[GameKey.LeftClick])
      LazyInput.WaitForRelease(key);
    foreach (GameKey key in KeyBindings.mouse_bindings[GameKey.RightClick])
      LazyInput.WaitForRelease(key);
  }

  public static bool IsNavigationKey(GameKey key)
  {
    return key == GameKey.Up || key == GameKey.Down || key == GameKey.Up || key == GameKey.Up;
  }

  public void AddPressed(GameKey key)
  {
    if (key == GameKey.None || this._pressed_keys.Contains(key) || this._wait_for_release.Contains(key))
      return;
    this._pressed_keys.Add(key);
  }

  public LazyInput.HoldedGroup GetGroup(GameKey key)
  {
    switch (key)
    {
      case GameKey.Left:
      case GameKey.Right:
      case GameKey.Up:
      case GameKey.Down:
        return LazyInput.HoldedGroup.Navigation;
      case GameKey.SliderDec:
      case GameKey.SliderInc:
        return LazyInput.HoldedGroup.Slider;
      case GameKey.PrevTab:
      case GameKey.NextTab:
      case GameKey.PrevSubTab:
      case GameKey.NextSubTub:
        return LazyInput.HoldedGroup.Tabs;
      default:
        return LazyInput.HoldedGroup.None;
    }
  }

  public void AddHolded(GameKey key)
  {
    if (key == GameKey.None || this._holded_keys.Contains(key))
      return;
    this._holded_keys.Add(key);
    if (!LazyInput._can_be_holded.ContainsKey(key) || this._holded_for_press.Contains(key) || this._wait_for_release.Contains(key))
      return;
    this._holded_for_press.Add(key);
    this._holded_for_press_delays.Add(0.3f);
  }

  public void UpdateHolded(float delta_time)
  {
    for (int index = 0; index < this._wait_for_release.Count; ++index)
    {
      if (this._holded_keys.Contains(this._wait_for_release[index]))
      {
        this._holded_keys.Remove(this._wait_for_release[index]);
      }
      else
      {
        this._wait_for_release.RemoveAt(index);
        --index;
      }
    }
    for (int index = 0; index < this._holded_for_press.Count; ++index)
    {
      GameKey key = this._holded_for_press[index];
      if (!this._holded_keys.Contains(key))
      {
        this._holded_for_press.RemoveAt(index);
        this._holded_for_press_delays.RemoveAt(index);
        --index;
        LazyInput.HoldedGroup group = this.GetGroup(key);
        if (group != LazyInput.HoldedGroup.None)
        {
          if (this._last_releases_in_group.ContainsKey(group))
            this._last_releases_in_group[group] = Time.time;
          else
            this._last_releases_in_group.Add(group, Time.time);
        }
      }
      else
      {
        this._holded_for_press_delays[index] -= delta_time;
        if ((double) this._holded_for_press_delays[index] <= 0.0)
        {
          this.AddPressed(key);
          this._holded_for_press_delays[index] = LazyInput._can_be_holded[key];
        }
      }
    }
  }

  public static void Vibrate(float value, float duration)
  {
    LazyInput._gamepad.Vibrate(value, duration);
  }

  public static void EmulateGamepadPress(GamePadButton button)
  {
    LazyInput._gamepad.EmulateKeyPress(button);
  }

  public enum HoldedGroup
  {
    None,
    Navigation,
    Tabs,
    Slider,
  }
}
