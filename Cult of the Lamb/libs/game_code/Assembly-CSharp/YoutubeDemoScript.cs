// Decompiled with JetBrains decompiler
// Type: YoutubeDemoScript
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using LightShaft.Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class YoutubeDemoScript : MonoBehaviour
{
  [SerializeField]
  public YoutubePlayer player;
  [SerializeField]
  public DemoCustomPlayerScript customPlayer;
  [SerializeField]
  public InputField urlInput;

  public void Reset()
  {
    this.player.loadYoutubeUrlsOnly = false;
    this.player.events.OnYoutubeUrlAreReady.RemoveListener(new UnityAction<string>(this.customPlayer.Play));
  }

  public void PreLoadTheVideoOnly()
  {
    this.Reset();
    this.player.PreLoadVideo("https://www.youtube.com/watch?v=GEPJZwFt2DM");
  }

  public void LoadUrlForACustomPlayer()
  {
    this.Reset();
    this.player.events.OnYoutubeUrlAreReady.AddListener(new UnityAction<string>(this.customPlayer.Play));
    this.player.LoadUrl("https://www.youtube.com/watch?v=GEPJZwFt2DM");
  }

  public void PlayFromUrlField()
  {
    this.Reset();
    this.player.Play(this.urlInput.text);
  }

  public void PlayFromUrlFieldStartingAt()
  {
    this.Reset();
    this.player.Play(this.urlInput.text, 10);
  }

  public void PlayCustomPlaylist()
  {
    this.Reset();
    string[] playlistUrls = new string[2]
    {
      "https://www.youtube.com/watch?v=GEPJZwFt2DM",
      "https://www.youtube.com/watch?v=pg5P69Hzsbg"
    };
    this.player.autoPlayNextVideo = true;
    this.player.Play(playlistUrls);
  }
}
