// Decompiled with JetBrains decompiler
// Type: Unity.VideoHelper.VideoPresenter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Unity.VideoHelper;

[RequireComponent(typeof (VideoController))]
public class VideoPresenter : MonoBehaviour, ITimelineProvider
{
  public const string MinutesFormat = "{0:00}:{1:00}";
  public const string HoursFormat = "{0:00}:{1:00}:{2:00}";
  public IDisplayController display;
  public VideoController controller;
  public float previousVolume;
  [Header("Controls")]
  public Transform Screen;
  public Transform ControlsPanel;
  public Transform LoadingIndicator;
  public Transform Thumbnail;
  public Timeline Timeline;
  public Slider Volume;
  public Image PlayPause;
  public Image MuteUnmute;
  public Image NormalFullscreen;
  public Text Current;
  public Text Duration;
  [SerializeField]
  public int targetDisplay;
  [Header("Input")]
  public KeyCode FullscreenKey = KeyCode.F;
  public KeyCode WindowedKey = KeyCode.Escape;
  public KeyCode TogglePlayKey = KeyCode.Space;
  [Space(10f)]
  public bool ToggleScreenOnDoubleClick = true;
  public bool TogglePlayPauseOnClick = true;
  [Header("Content")]
  public Sprite Play;
  public Sprite Pause;
  public Sprite Normal;
  public Sprite Fullscreen;
  public VolumeInfo[] Volumes = new VolumeInfo[0];

  public int TargetDisplay
  {
    get => this.targetDisplay;
    set
    {
      this.targetDisplay = value;
      this.display = DisplayController.ForDisplay(this.targetDisplay);
    }
  }

  public void Start()
  {
    this.controller = this.GetComponent<VideoController>();
    if ((UnityEngine.Object) this.controller == (UnityEngine.Object) null)
    {
      Debug.Log((object) "There is no video controller attached.");
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this);
    }
    else
    {
      this.controller.OnStartedPlaying.AddListener(new UnityAction(this.OnStartedPlaying));
      this.Volume.onValueChanged.AddListener(new UnityAction<float>(this.OnVolumeChanged));
      this.Thumbnail.OnClick(new UnityAction(this.Prepare));
      this.MuteUnmute.OnClick(new UnityAction(this.ToggleMute));
      this.PlayPause.OnClick(new UnityAction(this.ToggleIsPlaying));
      this.NormalFullscreen.OnClick(new UnityAction(this.ToggleFullscreen));
      this.Screen.OnDoubleClick(new UnityAction(this.ToggleFullscreen));
      this.Screen.OnClick(new UnityAction(this.ToggleIsPlaying));
      this.ControlsPanel.SetGameObjectActive(false);
      this.LoadingIndicator.SetGameObjectActive(false);
      this.Thumbnail.SetGameObjectActive(true);
      this.TargetDisplay = this.targetDisplay;
      Array.Sort<VolumeInfo>(this.Volumes, (Comparison<VolumeInfo>) ((v1, v2) =>
      {
        if ((double) v1.Minimum > (double) v2.Minimum)
          return 1;
        return (double) v1.Minimum == (double) v2.Minimum ? 0 : -1;
      }));
    }
  }

  public void Update()
  {
    this.CheckKeys();
    if (!this.controller.IsPlaying)
      return;
    this.Timeline.Position = this.controller.NormalizedTime;
  }

  public string GetFormattedPosition(float time)
  {
    return this.PrettyTimeFormat(TimeSpan.FromSeconds((double) time * (double) this.controller.Duration));
  }

  public void ToggleMute()
  {
    if ((double) this.Volume.value == 0.0)
    {
      this.Volume.value = this.previousVolume;
    }
    else
    {
      this.previousVolume = this.Volume.value;
      this.Volume.value = 0.0f;
    }
  }

  public void Prepare()
  {
    this.Thumbnail.SetGameObjectActive(false);
    this.LoadingIndicator.SetGameObjectActive(true);
  }

  public void CheckKeys()
  {
    if (Input.GetKeyDown(this.FullscreenKey))
      this.display.ToFullscreen(this.gameObject.transform as RectTransform);
    if (Input.GetKeyDown(this.WindowedKey))
      this.display.ToNormal();
    if (!Input.GetKeyDown(this.TogglePlayKey))
      return;
    this.ToggleIsPlaying();
  }

  public void ToggleIsPlaying()
  {
    if (this.controller.IsPlaying)
    {
      this.controller.Pause();
      this.PlayPause.sprite = this.Play;
    }
    else
    {
      this.controller.Play();
      this.PlayPause.sprite = this.Pause;
    }
  }

  public void OnStartedPlaying()
  {
    this.Screen.SetGameObjectActive(true);
    this.ControlsPanel.SetGameObjectActive(true);
    this.LoadingIndicator.SetGameObjectActive(false);
    if ((UnityEngine.Object) this.Duration != (UnityEngine.Object) null)
      this.Duration.text = this.PrettyTimeFormat(TimeSpan.FromSeconds((double) this.controller.Duration));
    this.StartCoroutine((IEnumerator) this.SetCurrentPosition());
    this.Volume.value = this.controller.Volume;
    this.NormalFullscreen.sprite = this.Fullscreen;
    this.PlayPause.sprite = this.Pause;
  }

  public void OnVolumeChanged(float volume)
  {
    this.controller.Volume = volume;
    for (int index = 0; index < this.Volumes.Length; ++index)
    {
      VolumeInfo volume1 = this.Volumes[index];
      float num = this.Volumes.Length - 1 >= index + 1 ? this.Volumes[index + 1].Minimum : 2f;
      if ((double) volume1.Minimum <= (double) volume && (double) num > (double) volume)
        this.MuteUnmute.sprite = volume1.Sprite;
    }
  }

  public void ToggleFullscreen()
  {
    if (this.display.IsFullscreen)
    {
      this.display.ToNormal();
      this.NormalFullscreen.sprite = this.Fullscreen;
    }
    else
    {
      this.display.ToFullscreen(this.gameObject.transform as RectTransform);
      this.NormalFullscreen.sprite = this.Normal;
    }
  }

  public IEnumerator SetCurrentPosition()
  {
    while (this.controller.IsPlaying)
    {
      if ((UnityEngine.Object) this.Current != (UnityEngine.Object) null)
        this.Current.text = this.PrettyTimeFormat(TimeSpan.FromSeconds((double) this.controller.Time));
      yield return (object) new WaitForSeconds(1f);
    }
  }

  public string PrettyTimeFormat(TimeSpan time)
  {
    return time.TotalHours <= 1.0 ? $"{time.Minutes:00}:{time.Seconds:00}" : $"{time.Hours:00}:{time.Minutes:00}:{time.Seconds:00}";
  }
}
