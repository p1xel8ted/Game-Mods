// Decompiled with JetBrains decompiler
// Type: PlayAmbientMusic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PlayAmbientMusic : BaseMonoBehaviour
{
  public AudioClip Music;
  public float fadeIn = 1f;

  public void Start() => AmbientMusicController.PlayTrack(this.Music, this.fadeIn);
}
