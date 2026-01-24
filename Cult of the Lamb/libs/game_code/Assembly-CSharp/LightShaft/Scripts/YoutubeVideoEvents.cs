// Decompiled with JetBrains decompiler
// Type: LightShaft.Scripts.YoutubeVideoEvents
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace LightShaft.Scripts;

public class YoutubeVideoEvents : MonoBehaviour
{
  public YoutubePlayer _player;
  [Header("Custom Events To use with video player only")]
  [Tooltip("When the url's are loaded")]
  public UrlLoadEvent OnYoutubeUrlAreReady;
  [Tooltip("When the videos are ready to play")]
  public UnityEvent OnVideoReadyToStart;
  [Tooltip("When the video start playing")]
  public UnityEvent OnVideoStarted;
  [Tooltip("When the video resume playing")]
  public UnityEvent OnVideoResumed;
  [Tooltip("When the video pause")]
  public UnityEvent OnVideoPaused;
  [Tooltip("When the video finish")]
  public UnityEvent OnVideoFinished;
  [Header("Event called on desired video time")]
  [Tooltip("If you want to call a custom function on desired video time.")]
  public YoutubeTimedEvent[] videoTimeEvents;

  public void Awake() => this._player = this.GetComponent<YoutubePlayer>();

  public void FixedUpdate()
  {
    if (this._player.playUsingInternalDevicePlayer || this._player.loadYoutubeUrlsOnly || this._player.videoPlayer.frame <= 0L || !this._player.videoPlayer.isPlaying)
      return;
    foreach (YoutubeTimedEvent videoTimeEvent in this.videoTimeEvents)
    {
      if (!videoTimeEvent.Called && (double) videoTimeEvent.time <= this._player.videoPlayer.time && (double) videoTimeEvent.time > this._player.videoPlayer.time - 2.0)
      {
        videoTimeEvent.Called = true;
        if (videoTimeEvent.pauseVideo)
          this._player.Pause();
        videoTimeEvent.timeEvent.Invoke();
      }
    }
  }
}
