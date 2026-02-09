// Decompiled with JetBrains decompiler
// Type: AnimEvents
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class AnimEvents : MonoBehaviour
{
  public GameObject[] game_objects;

  public void PlayMusic(string sound) => DarkTonic.MasterAudio.MasterAudio.TriggerPlaylistClip("music", sound);

  public void PlaySound(string sound)
  {
    string str = sound;
    char[] separator = new char[1]{ ',' };
    foreach (string sType in str.Split(separator, StringSplitOptions.RemoveEmptyEntries))
      DarkTonic.MasterAudio.MasterAudio.PlaySound(sType);
  }

  public void EnableGameObject(int i)
  {
    if (this.game_objects == null || this.game_objects.Length <= i)
      return;
    this.game_objects[i].SetActive(true);
  }

  public void DisableGameObject(int i)
  {
    if (this.game_objects == null || this.game_objects.Length <= i)
      return;
    this.game_objects[i].SetActive(false);
  }
}
