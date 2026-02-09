// Decompiled with JetBrains decompiler
// Type: LogoVolume
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class LogoVolume : MonoBehaviour
{
  public void Awake()
  {
    AudioSource component = this.GetComponent<AudioSource>();
    if (!((Object) component != (Object) null))
      return;
    component.volume = (float) (GameSettings.me.volume_master * GameSettings.me.volume_sfx) / 100f;
  }
}
