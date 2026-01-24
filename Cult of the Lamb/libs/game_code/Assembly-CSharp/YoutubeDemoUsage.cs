// Decompiled with JetBrains decompiler
// Type: YoutubeDemoUsage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using LightShaft.Scripts;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

#nullable disable
public class YoutubeDemoUsage : MonoBehaviour
{
  public GameObject mainUI;
  public Text videoUrlInput;

  public void DemoPlayback()
  {
    if ((Object) Object.FindObjectOfType<YoutubePlayer>() != (Object) null)
    {
      Object.FindObjectOfType<YoutubePlayer>().Play("bc0sJvtKrRM");
      Object.FindObjectOfType<YoutubePlayer>().videoPlayer.loopPointReached += new VideoPlayer.EventHandler(this.OnVideoFinished);
    }
    this.mainUI.SetActive(false);
  }

  public void PlayFromInput()
  {
    if ((Object) Object.FindObjectOfType<YoutubePlayer>() != (Object) null)
    {
      Object.FindObjectOfType<YoutubePlayer>().Play(this.videoUrlInput.text);
      Object.FindObjectOfType<YoutubePlayer>().videoPlayer.loopPointReached += new VideoPlayer.EventHandler(this.OnVideoFinished);
    }
    this.mainUI.SetActive(false);
  }

  public void OnVideoFinished(VideoPlayer vPlayer)
  {
    if ((Object) Object.FindObjectOfType<YoutubePlayer>() != (Object) null)
      Object.FindObjectOfType<YoutubePlayer>().videoPlayer.loopPointReached -= new VideoPlayer.EventHandler(this.OnVideoFinished);
    this.mainUI.SetActive(true);
  }
}
