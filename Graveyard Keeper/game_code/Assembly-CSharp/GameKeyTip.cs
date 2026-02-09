// Decompiled with JetBrains decompiler
// Type: GameKeyTip
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GameKeyTip
{
  public static Dictionary<GameKey, GamePadButton> _gamepad_bindings = KeyBindings.gamepad_bindings;
  public static Dictionary<GameKey, KeyCode[]> _keyboard_bindings = KeyBindings.keyboard_bindings;
  public static Dictionary<GameKey, GameKey[]> _mouse_bindings = KeyBindings.mouse_bindings;
  public GameKey _key;
  public string _text;
  public bool _active;
  public bool _gamepad_only;
  public bool _translate;
  public bool _prev_newline;
  public static string _prefix = "";

  public static string Get(
    GameKey key,
    string text,
    bool active = true,
    bool gamepad_only = false,
    bool translate = true,
    bool prev_newline = false)
  {
    return new GameKeyTip(key, text, active, gamepad_only, translate, prev_newline).ToString();
  }

  public GameKeyTip(
    GameKey key,
    string text,
    bool active = true,
    bool gamepad_only = true,
    bool translate = true,
    bool prev_newline = false)
  {
    this._key = key;
    this._text = text;
    this._active = active;
    this._gamepad_only = gamepad_only;
    this._translate = translate;
    this._prev_newline = prev_newline;
  }

  public override string ToString()
  {
    if (this._gamepad_only && !LazyInput.gamepad_active)
      return "";
    string icon = GameKeyTip.GetIcon(this._key);
    if (string.IsNullOrEmpty(icon))
      return "";
    string str = icon + (this._translate ? GJL.L(this._text) : this._text);
    return !this._active ? $"[ffffff55]{str}[-]" : str;
  }

  public static void SetPlatformPrefix(string prefix) => GameKeyTip._prefix = prefix;

  public static string GetPlatformPrefix() => GameKeyTip._prefix;

  public static string GetIcon(GameKey key)
  {
    string str = "";
    char ch1 = '(';
    char ch2 = ')';
    if (LazyInput.gamepad_active)
    {
      switch (key)
      {
        case GameKey.Move:
          return "(LS) ";
        case GameKey.AnyQuickslot:
          return $"({GameKeyTip._prefix}DPD)";
        case GameKey.MapCursor:
          return "(RS) ";
        default:
          str = !GameKeyTip._gamepad_bindings.ContainsKey(key) ? "" : GameKeyTip._gamepad_bindings[key].ToString();
          break;
      }
    }
    else
    {
      ch1 = '[';
      ch2 = ']';
      if (key == GameKey.Move)
        return "WASD";
      if (key == GameKey.AnyQuickslot)
        return "1-4";
      if (GameKeyTip._keyboard_bindings.ContainsKey(key))
      {
        KeyCode[] keyboardBinding = GameKeyTip._keyboard_bindings[key];
        str = keyboardBinding.Length == 0 ? "" : keyboardBinding[0].ToString().Replace("Alpha", "");
      }
      if (GameKeyTip._mouse_bindings.ContainsKey(key))
        return $"{ch1.ToString()}{GJL.L(key.ToString())}{ch2.ToString()} ";
    }
    if (string.IsNullOrEmpty(str))
      return "";
    return $"{ch1.ToString()}{GameKeyTip._prefix}{str}{ch2.ToString()} ";
  }

  public static GameKeyTip Back(string text, bool active = true, bool gamepad_only = true, bool translate = true)
  {
    return new GameKeyTip(GameKey.Back, text, active, gamepad_only, translate);
  }

  public static GameKeyTip Back(bool active = true, bool gamepad_only = true, bool translate = true)
  {
    return new GameKeyTip(GameKey.Back, "back", active, gamepad_only, translate);
  }

  public static GameKeyTip Close(bool active = true, bool gamepad_only = true, bool translate = true)
  {
    return new GameKeyTip(GameKey.Back, "close", active, gamepad_only, translate);
  }

  public static GameKeyTip Select(string text, bool active = true, bool gamepad_only = true, bool translate = true)
  {
    return new GameKeyTip(GameKey.Select, text, active, gamepad_only, translate);
  }

  public static GameKeyTip Select(bool active = true, bool gamepad_only = true, bool translate = true)
  {
    return new GameKeyTip(GameKey.Select, "select", active, gamepad_only, translate);
  }

  public static GameKeyTip Option1(string text, bool active = true, bool gamepad_only = true, bool translate = true)
  {
    return new GameKeyTip(GameKey.Option1, text, active, gamepad_only, translate);
  }

  public static GameKeyTip Option2(string text, bool active = true, bool gamepad_only = true, bool translate = true)
  {
    return new GameKeyTip(GameKey.Option2, text, active, gamepad_only, translate);
  }

  public static GameKeyTip LeftStick(bool active = true, bool gamepad_only = true, bool translate = true)
  {
    return new GameKeyTip(GameKey.Move, "move_tip", active, gamepad_only, translate);
  }

  public static GameKeyTip RightStick(bool active = true, bool gamepad_only = true, bool translate = true, string text = "cursor_tip")
  {
    return new GameKeyTip(GameKey.MapCursor, text, active, gamepad_only, translate);
  }
}
