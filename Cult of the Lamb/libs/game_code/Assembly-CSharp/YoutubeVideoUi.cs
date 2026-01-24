// Decompiled with JetBrains decompiler
// Type: YoutubeVideoUi
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using LightShaft.Scripts;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;

#nullable disable
public class YoutubeVideoUi : MonoBehaviour
{
  public Text videoName;
  public string videoId;
  public string thumbUrl;
  public Image videoThumb;
  public GameObject mainUI;

  public void PlayYoutubeVideo()
  {
    if ((Object) Object.FindObjectOfType<YoutubePlayer>() != (Object) null)
    {
      Object.FindObjectOfType<YoutubePlayer>().Play("https://youtube.com/watch?v=" + this.videoId);
      Object.FindObjectOfType<YoutubePlayer>().videoPlayer.loopPointReached += new VideoPlayer.EventHandler(this.VideoFinished);
    }
    if ((Object) Object.FindObjectOfType<VideoSearchDemo>() != (Object) null)
      this.mainUI = Object.FindObjectOfType<VideoSearchDemo>().mainUI;
    else if ((Object) Object.FindObjectOfType<ChannelSearchDemo>() != (Object) null)
      this.mainUI = Object.FindObjectOfType<ChannelSearchDemo>().mainUI;
    else if ((Object) Object.FindObjectOfType<PlaylistDemo>() != (Object) null)
      this.mainUI = Object.FindObjectOfType<PlaylistDemo>().mainUI;
    this.mainUI.SetActive(false);
  }

  public void VideoFinished(VideoPlayer vPlayer)
  {
    if ((Object) Object.FindObjectOfType<YoutubePlayer>() != (Object) null)
      Object.FindObjectOfType<YoutubePlayer>().videoPlayer.loopPointReached -= new VideoPlayer.EventHandler(this.VideoFinished);
    Debug.Log((object) "Video Finished");
    this.mainUI.SetActive(true);
  }

  public IEnumerator PlayVideo(string url)
  {
    yield return (object) true;
    Debug.Log((object) "below this line will run when the video is finished");
    this.VideoFinished();
  }

  public void LoadThumbnail() => this.StartCoroutine((IEnumerator) this.DownloadThumb());

  public IEnumerator DownloadThumb()
  {
    UnityWebRequest request = UnityWebRequestTexture.GetTexture(this.thumbUrl);
    yield return (object) request.SendWebRequest();
    Texture2D content = DownloadHandlerTexture.GetContent(request);
    this.videoThumb.sprite = Sprite.Create(content, new Rect(0.0f, 0.0f, (float) content.width, (float) content.height), new Vector2(0.5f, 0.5f), 100f);
  }

  public void VideoFinished() => Debug.Log((object) "Finished!");
}
