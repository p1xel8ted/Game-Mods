// Decompiled with JetBrains decompiler
// Type: LightShaft.Scripts.YoutubePlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Beffio.Dithering;
using Lamb.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

#nullable disable
namespace LightShaft.Scripts;

public class YoutubePlayer : YoutubeSimplifiedRequest
{
  public GameObject Renderer;
  public string musicToPlay;

  public override void Start()
  {
    base.Start();
    if (this.playUsingInternalDevicePlayer)
      return;
    this.events.OnYoutubeUrlAreReady.AddListener(new UnityAction<string>(this.UrlReadyToUse));
    this.events.OnVideoFinished.AddListener(new UnityAction(this.OnVideoPlayerFinished));
    this.events.OnVideoReadyToStart.AddListener(new UnityAction(this.OnVideoLoaded));
  }

  public void UrlReadyToUse(string urlToUse)
  {
    if (!this.loadYoutubeUrlsOnly)
      return;
    Debug.Log((object) "Here you can call your external video player if you want, passing that two variables:");
    if (this.videoQuality != YoutubeSimplifiedRequest.YoutubeVideoQuality.Standard)
    {
      Debug.Log((object) ("Your video Url: " + urlToUse));
      Debug.Log((object) ("Your audio video Url: " + this.audioUrl));
    }
    else
      Debug.Log((object) ("You video Url:" + urlToUse));
  }

  public string GetVideoTitle() => this.videoTitle;

  public void LoadUrl(string url)
  {
    this.Stop();
    this.loadYoutubeUrlsOnly = true;
    this.PlayYoutubeVideo(url);
  }

  public void PreLoadVideo(string url)
  {
    this.Stop();
    this.PrepareVideoToPlayLater = true;
    this.autoPlayOnStart = false;
    this.PlayYoutubeVideo(url);
  }

  public void Play(int startTime)
  {
    this.startFromSecond = true;
    this.startFromSecondTime = startTime;
    this.DisableThumbnailObject();
    this.pauseCalled = false;
    this.events.OnVideoStarted.Invoke();
    if (this.videoQuality == YoutubeSimplifiedRequest.YoutubeVideoQuality.Standard)
    {
      this.videoPlayer.Play();
    }
    else
    {
      this.videoPlayer.Play();
      this.audioPlayer.Play();
    }
    if (!this.startFromSecond)
      return;
    this.StartedFromTime = true;
    if (this.videoQuality == YoutubeSimplifiedRequest.YoutubeVideoQuality.Standard)
      this.videoPlayer.time = (double) this.startFromSecondTime;
    else
      this.audioPlayer.time = (double) this.startFromSecondTime;
  }

  public void Play(string url)
  {
    AudioManager.Instance.StopCurrentMusic();
    AudioManager.Instance.StopCurrentAtmos();
    AudioManager.Instance.StopActiveLoops();
    this.Renderer.gameObject.SetActive(false);
    this.Stop();
    this.PlayYoutubeVideo(url);
    this.PLAYING = true;
  }

  public void Play(string[] playlistUrls)
  {
    this.Stop();
    this.customPlaylist = true;
    this.youtubeUrls = playlistUrls;
    this.PlayYoutubeVideo(playlistUrls[this.CurrentUrlIndex]);
  }

  public override void Play()
  {
    base.Play();
    this.events.OnVideoStarted.Invoke();
    this.DisableThumbnailObject();
    this.pauseCalled = false;
    if (this.videoQuality == YoutubeSimplifiedRequest.YoutubeVideoQuality.Standard)
    {
      this.videoPlayer.Play();
    }
    else
    {
      this.videoPlayer.Play();
      if ((Object) this.controller.volumeSlider != (Object) null)
        this.audioPlayer.GetTargetAudioSource((ushort) 0).volume = this.controller.volumeSlider.value;
      else
        this.audioPlayer.GetTargetAudioSource((ushort) 0).volume = 1f;
      this.StartCoroutine((IEnumerator) this.DelayPlay());
    }
  }

  public void Play(string url, int startFrom)
  {
    this.startFromSecond = true;
    this.startFromSecondTime = startFrom;
    this.Stop();
    this.PlayYoutubeVideo(url);
  }

  public void PlayPause()
  {
    if (!this.YoutubeUrlReady || !this.videoPlayer.isPrepared)
      return;
    if (!this.pauseCalled)
    {
      this.events.OnVideoPaused.Invoke();
      this.Pause();
    }
    else
    {
      this.events.OnVideoResumed.Invoke();
      this.Play();
    }
  }

  public void ToogleFullsScreenMode()
  {
    this.FullscreenModeEnabled = !this.FullscreenModeEnabled;
    if (!this.FullscreenModeEnabled)
    {
      this.videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
      if ((Object) this.videoPlayer.targetCamera == (Object) null)
        this.videoPlayer.targetCamera = this.mainCamera;
      this.mainCamera.GetComponent<Stylizer>().enabled = false;
      this.GetComponentInParent<UIRoadmapOverlayController>().SubCanvasGroup.alpha = 0.0f;
      this.GetComponentInParent<UIRoadmapOverlayController>().YouTubePlayer = this;
    }
    else
    {
      this.GetComponentInParent<UIRoadmapOverlayController>().SubCanvasGroup.alpha = 1f;
      this.videoPlayer.renderMode = VideoRenderMode.MaterialOverride;
    }
  }

  public void OnVideoPlayerFinished()
  {
    if (this.FinishedCalled)
      return;
    Debug.Log((object) "video finished...");
    this.FinishedCalled = true;
    this.StartCoroutine((IEnumerator) this.PreventFinishToBeCalledTwoTimes());
    if (!this.loadYoutubeUrlsOnly)
    {
      if (!this.videoPlayer.isPrepared)
        return;
      if (this.debug)
        Debug.Log((object) "Finished");
      if (this.videoPlayer.isLooping)
      {
        this.videoPlayer.time = 0.0;
        this.videoPlayer.frame = 0L;
        this.audioPlayer.time = 0.0;
        this.audioPlayer.frame = 0L;
        this.videoPlayer.Play();
        this.audioPlayer.Play();
      }
      this.events.OnVideoFinished.Invoke();
      if (!this.customPlaylist || !this.autoPlayNextVideo)
        return;
      Debug.Log((object) "Calling next video of playlist");
      this.CallNextUrl();
    }
    else
    {
      if (!this.playUsingInternalDevicePlayer)
        return;
      this.events.OnVideoFinished.Invoke();
    }
  }

  public void PlayMusic(string musicPath) => this.musicToPlay = musicPath;

  public void OnVideoLoaded()
  {
    AudioManager.Instance.PlayMusic(this.musicToPlay);
    if (this.controller.useSliderToProgressVideo)
    {
      if ((Object) this.controller.playbackSlider == (Object) null)
        this.controller.showPlayerControl = false;
      else
        this.controller.playbackSlider.maxValue = this.videoQuality != YoutubeSimplifiedRequest.YoutubeVideoQuality.Standard ? (float) Mathf.RoundToInt((float) this.audioPlayer.frameCount / this.audioPlayer.frameRate) : (float) Mathf.RoundToInt((float) this.videoPlayer.frameCount / this.videoPlayer.frameRate);
    }
    if ((Object) this.events != (Object) null && this.events.videoTimeEvents.Length != 0)
    {
      foreach (YoutubeTimedEvent videoTimeEvent in this.events.videoTimeEvents)
        videoTimeEvent.Called = false;
    }
    Debug.Log((object) "The video is ready to play");
  }

  public void CallNextUrl()
  {
    if (!this.customPlaylist)
      return;
    if (this.CurrentUrlIndex + 1 < this.youtubeUrls.Length)
      ++this.CurrentUrlIndex;
    else
      this.CurrentUrlIndex = 0;
    this.PlayYoutubeVideo(this.youtubeUrls[this.CurrentUrlIndex]);
  }

  public void CallPreviousUrl()
  {
    if (!this.customPlaylist)
      return;
    if (this.CurrentUrlIndex - 1 > 0)
      --this.CurrentUrlIndex;
    else
      this.CurrentUrlIndex = 0;
    this.PlayYoutubeVideo(this.youtubeUrls[this.CurrentUrlIndex]);
  }

  public void OnApplicationPause(bool pause)
  {
    if (this.playUsingInternalDevicePlayer || this.loadYoutubeUrlsOnly || !this.videoPlayer.isPrepared)
      return;
    if ((Object) this.audioPlayer != (Object) null)
      this.audioPlayer.Pause();
    this.videoPlayer.Pause();
  }

  public void OnApplicationQuit()
  {
    if ((Object) this.videoPlayer != (Object) null && (Object) this.videoPlayer.targetTexture != (Object) null)
      this.videoPlayer.targetTexture.Release();
    if (this.playUsingInternalDevicePlayer)
      return;
    this.events.OnYoutubeUrlAreReady.RemoveListener(new UnityAction<string>(this.UrlReadyToUse));
    this.events.OnVideoFinished.RemoveListener(new UnityAction(this.OnVideoPlayerFinished));
    this.events.OnVideoReadyToStart.RemoveListener(new UnityAction(this.OnVideoLoaded));
  }

  public void OnEnable()
  {
    if (!this.autoPlayOnEnable || this.pauseCalled)
      return;
    this.StartCoroutine((IEnumerator) this.WaitThingsGetDone());
  }

  public IEnumerator WaitThingsGetDone()
  {
    YoutubePlayer youtubePlayer = this;
    yield return (object) new WaitForSeconds(1f);
    if (youtubePlayer.YoutubeUrlReady && youtubePlayer.videoPlayer.isPrepared)
      youtubePlayer.Play();
    else if (!youtubePlayer.YoutubeUrlReady)
      youtubePlayer.Play(youtubePlayer.youtubeUrl);
  }
}
