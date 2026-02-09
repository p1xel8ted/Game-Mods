// Decompiled with JetBrains decompiler
// Type: PlatformDependentElement
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PlatformDependentElement : MonoBehaviour
{
  public bool ignore;
  [Space]
  [Space]
  [Header("Show when:")]
  public bool controller_gamepad = true;
  public bool controller_mouse = true;
  [Space]
  public bool platform_pc = true;
  public bool platform_any_console = true;
  public bool platform_xbox = true;
  public bool platform_switch = true;
  public bool platform_ps = true;

  public void Init(bool for_gamepad)
  {
    if (this.ignore)
      return;
    bool flag = true & this.platform_pc;
    this.gameObject.SetActive(!for_gamepad ? flag & this.controller_mouse : flag & this.controller_gamepad);
  }
}
