// Decompiled with JetBrains decompiler
// Type: ToolbarGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ToolbarGUI : MonoBehaviour
{
  public ToolbarSetGUI keyboard;
  public ToolbarSetGUI gamepad;

  public void Init()
  {
    this.keyboard.Init(true);
    this.gamepad.Init(false);
  }

  public void Redraw()
  {
    this.Activate<ToolbarGUI>();
    bool gamepadActive = LazyInput.gamepad_active;
    this.keyboard.SetActive(!gamepadActive);
    this.gamepad.SetActive(gamepadActive);
    (gamepadActive ? this.gamepad : this.keyboard).Redraw();
  }
}
