// Decompiled with JetBrains decompiler
// Type: KeyboardController
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class KeyboardController : BaseInputController
{
  public const float DOUBLE_CLICK_DELAY = 0.5f;
  public float _last_click_time;

  public override void Update()
  {
    base.Update();
    foreach (KeyValuePair<GameKey, KeyCode[]> keyboardBinding in KeyBindings.keyboard_bindings)
    {
      GameKey key1 = keyboardBinding.Key;
      foreach (int key2 in keyboardBinding.Value)
      {
        if (Input.GetKeyDown((KeyCode) key2) && !this.pressed_keys.Contains(key1))
          this.pressed_keys.Add(key1);
        if (Input.GetKey((KeyCode) key2) && !this.holded_keys.Contains(key1))
          this.holded_keys.Add(key1);
      }
    }
    if (Input.GetMouseButtonDown(0))
    {
      this.pressed_keys.Add(GameKey.LeftClick);
      if ((double) Time.time - (double) this._last_click_time <= 0.5)
      {
        this.pressed_keys.Add(GameKey.DoubleClick);
        this._last_click_time = 0.0f;
      }
      else
        this._last_click_time = Time.time;
      if (Input.GetKey(KeyCode.LeftShift))
        this.pressed_keys.Add(GameKey.MoveAllStack);
      foreach (GameKey gameKey in KeyBindings.mouse_bindings[GameKey.LeftClick])
        this.pressed_keys.Add(gameKey);
    }
    if (Input.GetMouseButton(0))
    {
      this.holded_keys.Add(GameKey.LeftClick);
      foreach (GameKey gameKey in KeyBindings.mouse_bindings[GameKey.LeftClick])
        this.holded_keys.Add(gameKey);
    }
    if (Input.GetMouseButtonDown(1))
    {
      this.pressed_keys.Add(GameKey.RightClick);
      if (Input.GetKey(KeyCode.LeftShift))
        this.pressed_keys.Add(GameKey.MoveAllStack);
      foreach (GameKey gameKey in KeyBindings.mouse_bindings[GameKey.RightClick])
        this.pressed_keys.Add(gameKey);
    }
    if (Input.GetMouseButton(1))
    {
      this.holded_keys.Add(GameKey.RightClick);
      foreach (GameKey gameKey in KeyBindings.mouse_bindings[GameKey.RightClick])
        this.holded_keys.Add(gameKey);
    }
    this.dir = Vector2.zero;
    if (this.holded_keys.Contains(GameKey.Left))
      --this.dir.x;
    if (this.holded_keys.Contains(GameKey.Right))
      ++this.dir.x;
    if (this.holded_keys.Contains(GameKey.Up))
      ++this.dir.y;
    if (!this.holded_keys.Contains(GameKey.Down))
      return;
    --this.dir.y;
  }
}
