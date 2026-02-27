// Decompiled with JetBrains decompiler
// Type: MMTools.MMVideoPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI;
using UnityEngine;
using UnityEngine.Video;

#nullable disable
namespace MMTools;

public class MMVideoPlayer : MonoBehaviour
{
  private static Unify.VideoPlayer unifyVideoPlayer;
  private static bool isPlayingVideo;
  private static bool finished;
  private static bool loaded;
  private static bool unifyVideoIsLooping;
  private static bool unifyVideoPlayerPrepared;
  private static float inputBufferTime = 1f;
  private static float videoPlayStartTime;
  private static UnityEngine.Video.VideoPlayer videoPlayer;
  private static System.Action Callback;
  public static GameObject Instance;
  private MMVideoPlayer.Options Skippable;
  private MMVideoPlayer.Options FastForward;
  public bool HideOnCompete;
  private static MMVideoPlayer mmVideoPlayer;
  private bool completed;
  public GameObject controlprompt;

  private void Start() => UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this.gameObject);

  public static void Hide()
  {
    if (!((UnityEngine.Object) MMVideoPlayer.Instance != (UnityEngine.Object) null))
      return;
    MMVideoPlayer.Instance.SetActive(false);
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
    KeyboardLightingManager.PlayVideo();
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
    MMVideoPlayer.videoPlayer = MMVideoPlayer.Instance.GetComponent<UnityEngine.Video.VideoPlayer>();
    MMVideoPlayer.videoPlayer.source = VideoSource.VideoClip;
    MMVideoPlayer.videoPlayer.errorReceived += new UnityEngine.Video.VideoPlayer.ErrorEventHandler(MMVideoPlayer.HandleVideoPlayerError);
    if (ForceStreaming)
      MMVideoPlayer.videoPlayer.url = $"{Application.streamingAssetsPath}/MMVideoPlayer/Videos/{_FileName}.mp4";
    else
      MMVideoPlayer.videoPlayer.clip = Resources.Load("MMVideoPlayer/Videos/" + _FileName) as VideoClip;
    MMVideoPlayer.videoPlayer.Play();
    MMVideoPlayer.videoPlayer.loopPointReached += new UnityEngine.Video.VideoPlayer.EventHandler(MMVideoPlayer.EndReached);
    MMVideoPlayer.Callback = _CallBack;
  }

  private static void HandleVideoPlayerError(UnityEngine.Video.VideoPlayer source, string message)
  {
    Debug.Log((object) ("MMVideoPlayer - ERROR - " + message));
    MMVideoPlayer.EndReached(source);
  }

  private void Update()
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
      if (this.Skippable == MMVideoPlayer.Options.ENABLE && (Input.GetKeyUp(KeyCode.Escape) || InputManager.UI.GetAcceptButtonUp()))
        MMVideoPlayer.EndReached((UnityEngine.Video.VideoPlayer) null);
    }
    if (!((UnityEngine.Object) MMVideoPlayer.videoPlayer != (UnityEngine.Object) null) || this.completed || MMVideoPlayer.videoPlayer.frame <= 0L || MMVideoPlayer.videoPlayer.isPlaying)
      return;
    this.completed = true;
    MMVideoPlayer.EndReached(MMVideoPlayer.videoPlayer);
  }

  private static void EndReached(UnityEngine.Video.VideoPlayer vp)
  {
    if (MMVideoPlayer.Callback != null)
      MMVideoPlayer.Callback();
    MMVideoPlayer.videoPlayer.loopPointReached -= new UnityEngine.Video.VideoPlayer.EventHandler(MMVideoPlayer.EndReached);
    MMVideoPlayer.videoPlayer.errorReceived -= new UnityEngine.Video.VideoPlayer.ErrorEventHandler(MMVideoPlayer.HandleVideoPlayerError);
    KeyboardLightingManager.StopVideo();
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
    MMVideoPlayer.videoPlayer.loopPointReached -= new UnityEngine.Video.VideoPlayer.EventHandler(MMVideoPlayer.EndReached);
    MMVideoPlayer.videoPlayer.errorReceived -= new UnityEngine.Video.VideoPlayer.ErrorEventHandler(MMVideoPlayer.HandleVideoPlayerError);
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
