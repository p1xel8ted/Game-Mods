// Decompiled with JetBrains decompiler
// Type: LightShaft.Scripts.YoutubeVideoController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

#nullable disable
namespace LightShaft.Scripts;

public class YoutubeVideoController : MonoBehaviour
{
  public YoutubePlayer _player;
  public bool showPlayerControl;
  public Slider playbackSlider;
  public Image progressRectangle;
  public Slider speedSlider;
  public Slider volumeSlider;
  public Text currentTime;
  public Text totalTime;
  public GameObject loading;
  public Button nextVideoButton;
  public Button previousVideoButton;
  public bool showingVolume;
  public bool showingSpeed;
  [SerializeField]
  public GameObject controllerMainUI;
  [Tooltip("Time to auto hide the controller. 0 will not hide the controller.")]
  [SerializeField]
  public int hideScreenControlTime;
  [Header("If you want to use a sprite rectangle instead of slider disable this")]
  public bool useSliderToProgressVideo = true;

  public void Awake()
  {
    this._player = this.GetComponent<YoutubePlayer>();
    if (!this.showPlayerControl)
    {
      if (!((Object) this.controllerMainUI != (Object) null))
        return;
      this.controllerMainUI.SetActive(false);
    }
    else
    {
      if (!this._player.customPlaylist)
      {
        if ((Object) this.previousVideoButton != (Object) null && (Object) this.nextVideoButton != (Object) null)
        {
          this.previousVideoButton.gameObject.SetActive(false);
          this.nextVideoButton.gameObject.SetActive(false);
        }
      }
      else if ((Object) this.previousVideoButton != (Object) null && (Object) this.nextVideoButton != (Object) null)
      {
        this.previousVideoButton.gameObject.SetActive(true);
        this.nextVideoButton.gameObject.SetActive(true);
      }
      if (this.showPlayerControl)
      {
        if ((Object) this.speedSlider == (Object) null)
          Debug.LogWarning((object) "Drag the playback speed slider to the speedSlider field.");
        if ((Object) this.volumeSlider == (Object) null)
          Debug.LogWarning((object) "Drag the volume eslider to the volumeSlider field.");
        if ((Object) this.playbackSlider == (Object) null)
          Debug.LogWarning((object) "Drag the playback slider to the playbackSlider field, this is necessary to change the video progress.");
      }
      this.speedSlider.maxValue = 3f;
      if (this.useSliderToProgressVideo)
      {
        this.progressRectangle.gameObject.SetActive(false);
        this.playbackSlider.gameObject.SetActive(true);
      }
      else
      {
        this.playbackSlider.gameObject.SetActive(false);
        this.progressRectangle.gameObject.SetActive(true);
      }
    }
  }

  public void Play() => this._player.Play();

  public void Pause() => this._player.Pause();

  public void PlayToggle() => this._player.PlayPause();

  public void ChangeVolume(float volume)
  {
    switch (this._player.videoPlayer.audioOutputMode)
    {
      case VideoAudioOutputMode.AudioSource:
        this._player.videoPlayer.GetComponent<AudioSource>().volume = volume;
        this._player.videoPlayer.SetDirectAudioVolume((ushort) 0, volume);
        break;
      case VideoAudioOutputMode.Direct:
        this._player.audioPlayer.SetDirectAudioVolume((ushort) 0, volume);
        this._player.videoPlayer.SetDirectAudioVolume((ushort) 0, volume);
        break;
      default:
        this._player.videoPlayer.GetComponent<AudioSource>().volume = volume;
        this._player.videoPlayer.SetDirectAudioVolume((ushort) 0, volume);
        break;
    }
  }

  public void ChangePlaybackSpeed(float speed)
  {
    if (!this._player.videoPlayer.canSetPlaybackSpeed)
      return;
    if ((double) speed <= 0.0)
    {
      this._player.videoPlayer.playbackSpeed = 0.5f;
      this._player.audioPlayer.playbackSpeed = 0.5f;
    }
    else
    {
      this._player.videoPlayer.playbackSpeed = speed;
      this._player.audioPlayer.playbackSpeed = speed;
    }
  }

  public void PlayNextVideo()
  {
    if (this.NextVideo())
      return;
    Debug.Log((object) "Cannot play the next video.");
  }

  public void PlayPreviousVideo()
  {
    if (this.PreviousVideo())
      return;
    Debug.Log((object) "Cannot play the previous video.");
  }

  public bool NextVideo()
  {
    if (!this._player.customPlaylist)
      return false;
    this._player.CallNextUrl();
    return true;
  }

  public bool PreviousVideo()
  {
    if (!this._player.customPlaylist)
      return false;
    this._player.CallPreviousUrl();
    return true;
  }

  public void ChangeVideoTime(float value)
  {
    this._player.SkipToPercent((float) (Mathf.RoundToInt(value) * 100) / this.playbackSlider.maxValue * 0.01f);
    this._player.progressStartDrag = false;
  }

  public void PlaybackSliderStartDrag() => this._player.progressStartDrag = true;

  public void ToggleFullScreen() => this._player.ToogleFullsScreenMode();

  public void HideControllers()
  {
    if (!((Object) this.controllerMainUI != (Object) null))
      return;
    this.controllerMainUI.SetActive(false);
    this.showingVolume = false;
    this.showingSpeed = false;
    this.volumeSlider.gameObject.SetActive(false);
    this.speedSlider.gameObject.SetActive(false);
  }

  public void ToggleVolumeSlider()
  {
    if (this.showingVolume)
    {
      this.showingVolume = false;
      this.volumeSlider.gameObject.SetActive(false);
    }
    else
    {
      this.showingVolume = true;
      this.volumeSlider.gameObject.SetActive(true);
    }
  }

  public void ToggleSpeedSlider()
  {
    if (this.showingSpeed)
    {
      this.showingSpeed = false;
      this.speedSlider.gameObject.SetActive(false);
    }
    else
    {
      this.showingSpeed = true;
      this.speedSlider.gameObject.SetActive(true);
    }
  }
}
