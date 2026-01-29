// Decompiled with JetBrains decompiler
// Type: Unity.VideoHelper.VideoController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

#nullable disable
namespace Unity.VideoHelper;

public class VideoController : MonoBehaviour
{
  [SerializeField]
  public RawImage screen;
  [SerializeField]
  public bool startAfterPreparation = true;
  [Header("Optional")]
  [SerializeField]
  public VideoPlayer videoPlayer;
  [SerializeField]
  public AudioSource audioSource;
  [Header("Events")]
  [SerializeField]
  public UnityEvent onPrepared = new UnityEvent();
  [SerializeField]
  public UnityEvent onStartedPlaying = new UnityEvent();
  [SerializeField]
  public UnityEvent onFinishedPlaying = new UnityEvent();

  public bool StartAfterPreparation
  {
    get => this.startAfterPreparation;
    set => this.startAfterPreparation = value;
  }

  public float NormalizedTime => (float) this.videoPlayer.time / (float) this.Duration;

  public ulong Duration => this.videoPlayer.frameCount / (ulong) this.videoPlayer.frameRate;

  public ulong Time => (ulong) this.videoPlayer.time;

  public bool IsPrepared => this.videoPlayer.isPrepared;

  public bool IsPlaying => this.videoPlayer.isPlaying;

  public float Volume
  {
    get
    {
      return !((Object) this.audioSource == (Object) null) ? this.audioSource.volume : this.videoPlayer.GetDirectAudioVolume((ushort) 0);
    }
    set
    {
      if ((Object) this.audioSource == (Object) null)
        this.videoPlayer.SetDirectAudioVolume((ushort) 0, value);
      else
        this.audioSource.volume = value;
    }
  }

  public RawImage Screen
  {
    get => this.screen;
    set => this.screen = value;
  }

  public UnityEvent OnPrepared => this.onPrepared;

  public UnityEvent OnStartedPlaying => this.onStartedPlaying;

  public UnityEvent OnFinishedPlaying => this.onFinishedPlaying;

  public void OnEnable() => this.SubscribeToVideoPlayerEvents();

  public void OnDisable() => this.UnsubscribeFromVideoPlayerEvents();

  public void Start()
  {
    if ((Object) this.videoPlayer == (Object) null)
    {
      this.videoPlayer = this.gameObject.GetOrAddComponent<VideoPlayer>();
      this.SubscribeToVideoPlayerEvents();
    }
    this.videoPlayer.playOnAwake = false;
  }

  public void PrepareForUrl(string url)
  {
    Debug.Log((object) "SENT THE URL!");
    this.videoPlayer.source = VideoSource.Url;
    this.videoPlayer.url = url;
    this.videoPlayer.Prepare();
  }

  public void PrepareForClip(VideoClip clip)
  {
    this.videoPlayer.source = VideoSource.VideoClip;
    this.videoPlayer.clip = clip;
    this.videoPlayer.Prepare();
  }

  public void Play()
  {
    if (!this.IsPrepared)
      this.videoPlayer.Prepare();
    else
      this.videoPlayer.Play();
  }

  public void Pause() => this.videoPlayer.Pause();

  public void TogglePlayPause()
  {
    if (this.IsPlaying)
      this.Pause();
    else
      this.Play();
  }

  public void SetPlaybackSpeed(float speed) => this.videoPlayer.playbackSpeed = speed;

  public void Seek(float time)
  {
    time = Mathf.Clamp(time, 0.0f, 1f);
    this.videoPlayer.time = (double) time * (double) this.Duration;
  }

  public void OnStarted(VideoPlayer source) => this.onStartedPlaying.Invoke();

  public void OnFinished(VideoPlayer source) => this.onFinishedPlaying.Invoke();

  public void OnPrepareCompleted(VideoPlayer source)
  {
    this.onPrepared.Invoke();
    this.screen.texture = this.videoPlayer.texture;
    this.SetupAudio();
    this.SetupScreenAspectRatio();
    if (!this.StartAfterPreparation)
      return;
    this.Play();
  }

  public void SetupScreenAspectRatio()
  {
    AspectRatioFitter orAddComponent = this.screen.gameObject.GetOrAddComponent<AspectRatioFitter>();
    orAddComponent.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
    orAddComponent.aspectRatio = (float) this.videoPlayer.texture.width / (float) this.videoPlayer.texture.height;
  }

  public void SetupAudio()
  {
    if (this.videoPlayer.audioTrackCount <= (ushort) 0)
      return;
    if ((Object) this.audioSource == (Object) null && this.videoPlayer.canSetDirectAudioVolume)
    {
      this.videoPlayer.audioOutputMode = VideoAudioOutputMode.Direct;
    }
    else
    {
      this.videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
      this.videoPlayer.SetTargetAudioSource((ushort) 0, this.audioSource);
    }
    this.videoPlayer.controlledAudioTrackCount = (ushort) 1;
    this.videoPlayer.EnableAudioTrack((ushort) 0, true);
  }

  public void OnError(VideoPlayer source, string message)
  {
  }

  public void SubscribeToVideoPlayerEvents()
  {
    if ((Object) this.videoPlayer == (Object) null)
      return;
    this.videoPlayer.errorReceived += new VideoPlayer.ErrorEventHandler(this.OnError);
    this.videoPlayer.prepareCompleted += new VideoPlayer.EventHandler(this.OnPrepareCompleted);
    this.videoPlayer.started += new VideoPlayer.EventHandler(this.OnStarted);
    this.videoPlayer.loopPointReached += new VideoPlayer.EventHandler(this.OnFinished);
  }

  public void UnsubscribeFromVideoPlayerEvents()
  {
    if ((Object) this.videoPlayer == (Object) null)
      return;
    this.videoPlayer.errorReceived -= new VideoPlayer.ErrorEventHandler(this.OnError);
    this.videoPlayer.prepareCompleted -= new VideoPlayer.EventHandler(this.OnPrepareCompleted);
    this.videoPlayer.started -= new VideoPlayer.EventHandler(this.OnStarted);
    this.videoPlayer.loopPointReached -= new VideoPlayer.EventHandler(this.OnFinished);
  }
}
