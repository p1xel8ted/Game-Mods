// Decompiled with JetBrains decompiler
// Type: SkullIconContainerGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SkullIconContainerGUI : MonoBehaviour
{
  [SerializeField]
  public GameObject skull_obj;

  public void SetSkullActive(bool is_active)
  {
    if (!((Object) this.skull_obj != (Object) null))
      return;
    this.skull_obj.SetActive(is_active);
  }
}
