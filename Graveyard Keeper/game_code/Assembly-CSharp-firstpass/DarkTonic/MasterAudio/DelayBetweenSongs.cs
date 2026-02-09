// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.DelayBetweenSongs
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections;
using UnityEngine;

#nullable disable
namespace DarkTonic.MasterAudio;

public class DelayBetweenSongs : MonoBehaviour
{
  public float minTimeToWait = 1f;
  public float maxTimeToWait = 2f;
  public string playlistControllerName = "PlaylistControllerBass";
  public PlaylistController _controller;

  public void Start()
  {
    this._controller = PlaylistController.InstanceByName(this.playlistControllerName);
    this._controller.SongEnded += new PlaylistController.SongEndedEventHandler(this.SongEnded);
  }

  public void OnDisable()
  {
    this._controller.SongEnded -= new PlaylistController.SongEndedEventHandler(this.SongEnded);
  }

  public void SongEnded(string songName)
  {
    this.StopAllCoroutines();
    this.StartCoroutine(this.PlaySongWithDelay());
  }

  public IEnumerator PlaySongWithDelay()
  {
    DelayBetweenSongs delayBetweenSongs = this;
    float num = Random.Range(delayBetweenSongs.minTimeToWait, delayBetweenSongs.maxTimeToWait);
    if (DarkTonic.MasterAudio.MasterAudio.IgnoreTimeScale)
      yield return (object) delayBetweenSongs.StartCoroutine(CoroutineHelper.WaitForActualSeconds(num));
    else
      yield return (object) new WaitForSeconds(num);
    delayBetweenSongs._controller.PlayNextSong();
  }
}
