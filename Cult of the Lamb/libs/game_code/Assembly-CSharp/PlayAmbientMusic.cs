// Decompiled with JetBrains decompiler
// Type: PlayAmbientMusic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PlayAmbientMusic : BaseMonoBehaviour
{
  public AudioClip Music;
  public float fadeIn = 1f;

  public void Start() => AmbientMusicController.PlayTrack(this.Music, this.fadeIn);
}
