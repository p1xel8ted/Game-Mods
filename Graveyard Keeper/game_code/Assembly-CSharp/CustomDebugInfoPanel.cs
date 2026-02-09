// Decompiled with JetBrains decompiler
// Type: CustomDebugInfoPanel
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class CustomDebugInfoPanel
{
  public static void Update()
  {
    if ((Object) GUIElements.me == (Object) null || !Input.GetKeyDown(KeyCode.F6) || !Input.GetKey(KeyCode.RightShift))
      return;
    GUIElements.me.dialog.OpenOK($"Res: {Screen.width}x{Screen.height}, {Screen.fullScreenMode}\n" + $"Cam sizes: W={MainGame.me.world_cam.orthographicSize}; G={MainGame.me.gui_cam.orthographicSize}; GH={MainGame.me.ui_root.manualHeight}");
  }
}
