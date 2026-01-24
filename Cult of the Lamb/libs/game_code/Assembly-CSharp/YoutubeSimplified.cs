// Decompiled with JetBrains decompiler
// Type: YoutubeSimplified
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using LightShaft.Scripts;
using UnityEngine;
using UnityEngine.Video;

#nullable disable
public class YoutubeSimplified : MonoBehaviour
{
  public YoutubePlayer player;
  public string url;
  public bool autoPlay = true;
  public bool fullscreen = true;
  public VideoPlayer videoPlayer;

  public void Awake()
  {
    this.videoPlayer = this.GetComponentInChildren<VideoPlayer>();
    this.player = this.GetComponentInChildren<YoutubePlayer>();
    this.player.videoPlayer = this.videoPlayer;
  }

  public void Start() => this.Play();

  public void Play()
  {
    if (this.fullscreen)
    {
      this.videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
      this.videoPlayer.aspectRatio = VideoAspectRatio.FitInside;
      this.videoPlayer.targetCamera = this.player.mainCamera;
    }
    this.player.autoPlayOnStart = this.autoPlay;
    this.player.videoQuality = YoutubeSimplifiedRequest.YoutubeVideoQuality.Standard;
    if (!this.autoPlay)
      return;
    this.player.Play(this.url);
  }
}
