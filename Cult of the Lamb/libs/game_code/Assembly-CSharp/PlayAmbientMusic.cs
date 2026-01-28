// Decompiled with JetBrains decompiler
// Type: PlayAmbientMusic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PlayAmbientMusic : BaseMonoBehaviour
{
  public AudioClip Music;
  public float fadeIn = 1f;

  public void Start() => AmbientMusicController.PlayTrack(this.Music, this.fadeIn);
}
