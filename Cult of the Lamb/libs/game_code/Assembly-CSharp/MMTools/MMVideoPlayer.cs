// Decompiled with JetBrains decompiler
// Type: MMTools.MMVideoPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using UnityEngine;
using UnityEngine.Video;

#nullable disable
namespace MMTools;

public class MMVideoPlayer : MonoBehaviour
{
  public static VideoPlayer videoPlayer;
  public static System.Action Callback;
  public static GameObject Instance;
  public MMVideoPlayer.Options Skippable;
  public MMVideoPlayer.Options FastForward;
  public bool HideOnCompete;
  public static MMVideoPlayer mmVideoPlayer;
  public bool completed;
  public bool wasDisabled;
  public GameObject controlprompt;
  public GameObject skipPrompt;

  public bool IsPlaying
  {
    get
    {
      return (UnityEngine.Object) MMVideoPlayer.videoPlayer != (UnityEngine.Object) null && MMVideoPlayer.videoPlayer.isPlaying;
    }
  }

  public void Start() => UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this.gameObject);

  public static void Hide()
  {
    if (!((UnityEngine.Object) MMVideoPlayer.Instance != (UnityEngine.Object) null))
      return;
    MMVideoPlayer.Instance.SetActive(false);
  }

  public void OnDisable()
  {
    if (!this.wasDisabled || !((UnityEngine.Object) WeatherSystemController.Instance != (UnityEngine.Object) null))
      return;
    this.wasDisabled = false;
    WeatherSystemController.Instance.EnableWeatherEffect();
  }

  public static void Play(
    string _FileName,
    System.Action _CallBack,
    MMVideoPlayer.Options Skippable,
    MMVideoPlayer.Options FastForward,
    bool HideOnCompete = true,
    bool ForceStreaming = false)
  {
    if ((UnityEngine.Object) MMVideoPlayer.Instance == (UnityEngine.Object) null)
    {
      MMVideoPlayer.Instance = UnityEngine.Object.Instantiate(Resources.Load("MMVideoPlayer/Video Player")) as GameObject;
      MMVideoPlayer.mmVideoPlayer = MMVideoPlayer.Instance.GetComponent<MMVideoPlayer>();
    }
    else
      MMVideoPlayer.Instance.SetActive(true);
    if ((UnityEngine.Object) WeatherSystemController.Instance != (UnityEngine.Object) null)
    {
      MMVideoPlayer.mmVideoPlayer.wasDisabled = true;
      WeatherSystemController.Instance.DisableWeatherEffect();
    }
    DeviceLightingManager.PlayVideo();
    if ((UnityEngine.Object) MonoSingleton<UIManager>.Instance != (UnityEngine.Object) null)
      MonoSingleton<UIManager>.Instance.ForceBlockMenus = true;
    if ((UnityEngine.Object) MMVideoPlayer.mmVideoPlayer.controlprompt != (UnityEngine.Object) null)
    {
      if (_FileName == "Trailer")
        MMVideoPlayer.mmVideoPlayer.controlprompt.SetActive(true);
      else
        MMVideoPlayer.mmVideoPlayer.controlprompt.SetActive(false);
    }
    MMVideoPlayer.mmVideoPlayer.Skippable = Skippable;
    MMVideoPlayer.mmVideoPlayer.FastForward = FastForward;
    MMVideoPlayer.mmVideoPlayer.HideOnCompete = HideOnCompete;
    MMVideoPlayer.mmVideoPlayer.completed = false;
    MMVideoPlayer.videoPlayer = MMVideoPlayer.Instance.GetComponent<VideoPlayer>();
    MMVideoPlayer.videoPlayer.source = VideoSource.VideoClip;
    MMVideoPlayer.videoPlayer.errorReceived += new VideoPlayer.ErrorEventHandler(MMVideoPlayer.HandleVideoPlayerError);
    if (ForceStreaming)
      MMVideoPlayer.videoPlayer.url = $"{Application.streamingAssetsPath}/MMVideoPlayer/Videos/{_FileName}.mp4";
    else
      MMVideoPlayer.videoPlayer.clip = Resources.Load("MMVideoPlayer/Videos/" + _FileName) as VideoClip;
    MMVideoPlayer.videoPlayer.Play();
    MMVideoPlayer.videoPlayer.loopPointReached += new VideoPlayer.EventHandler(MMVideoPlayer.EndReached);
    MMVideoPlayer.Callback = _CallBack;
    MMVideoPlayer.mmVideoPlayer.skipPrompt.SetActive(Skippable == MMVideoPlayer.Options.ENABLE);
  }

  public static void HandleVideoPlayerError(VideoPlayer source, string message)
  {
    Debug.Log((object) ("MMVideoPlayer - ERROR - " + message));
    MMVideoPlayer.EndReached(source);
  }

  public void Update()
  {
    if ((UnityEngine.Object) MMVideoPlayer.videoPlayer != (UnityEngine.Object) null && MMVideoPlayer.videoPlayer.isPlaying)
    {
      if (this.FastForward == MMVideoPlayer.Options.ENABLE)
      {
        if (Input.anyKeyDown)
          MMVideoPlayer.videoPlayer.playbackSpeed = 2f;
        if (!Input.anyKey)
          MMVideoPlayer.videoPlayer.playbackSpeed = 1f;
      }
      if (!MMTransition.IsPlaying && this.Skippable == MMVideoPlayer.Options.ENABLE && InputManager.UI.GetCancelButtonDown())
      {
        this.completed = true;
        MMVideoPlayer.EndReached((VideoPlayer) null);
      }
    }
    if (!((UnityEngine.Object) MMVideoPlayer.videoPlayer != (UnityEngine.Object) null) || this.completed || MMVideoPlayer.videoPlayer.frame <= 0L || MMVideoPlayer.videoPlayer.isPlaying)
      return;
    this.completed = true;
    MMVideoPlayer.EndReached(MMVideoPlayer.videoPlayer);
  }

  public static void EndReached(VideoPlayer vp)
  {
    DeviceLightingManager.StopVideo();
    if (MMVideoPlayer.Callback != null)
      MMVideoPlayer.Callback();
    MMVideoPlayer.videoPlayer.loopPointReached -= new VideoPlayer.EventHandler(MMVideoPlayer.EndReached);
    MMVideoPlayer.videoPlayer.errorReceived -= new VideoPlayer.ErrorEventHandler(MMVideoPlayer.HandleVideoPlayerError);
    if ((UnityEngine.Object) MMVideoPlayer.Instance != (UnityEngine.Object) null)
      MMVideoPlayer.Instance.SetActive(!MMVideoPlayer.mmVideoPlayer.HideOnCompete);
    if (!((UnityEngine.Object) MonoSingleton<UIManager>.Instance != (UnityEngine.Object) null))
      return;
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
  }

  public static void ForceStopVideo()
  {
    if (!((UnityEngine.Object) MMVideoPlayer.videoPlayer != (UnityEngine.Object) null))
      return;
    MMVideoPlayer.videoPlayer.loopPointReached -= new VideoPlayer.EventHandler(MMVideoPlayer.EndReached);
    MMVideoPlayer.videoPlayer.errorReceived -= new VideoPlayer.ErrorEventHandler(MMVideoPlayer.HandleVideoPlayerError);
    MMVideoPlayer.videoPlayer.Stop();
    if ((UnityEngine.Object) MonoSingleton<UIManager>.Instance != (UnityEngine.Object) null)
      MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
    if ((UnityEngine.Object) MMVideoPlayer.Instance != (UnityEngine.Object) null)
      MMVideoPlayer.Instance.SetActive(false);
    UnityEngine.Object.Destroy((UnityEngine.Object) MMVideoPlayer.Instance);
  }

  public enum Options
  {
    ENABLE,
    DISABLE,
  }
}
