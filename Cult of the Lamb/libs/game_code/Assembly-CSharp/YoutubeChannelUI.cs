// Decompiled with JetBrains decompiler
// Type: YoutubeChannelUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

#nullable disable
public class YoutubeChannelUI : MonoBehaviour
{
  public Text videoName;
  public string videoId;
  public string thumbUrl;
  public Image videoThumb;

  public void LoadChannel()
  {
    Object.FindObjectOfType<ChannelSearchDemo>().LoadChannelResult(this.videoId);
  }

  public void LoadThumbnail() => this.StartCoroutine((IEnumerator) this.DownloadThumb());

  public IEnumerator DownloadThumb()
  {
    UnityWebRequest request = UnityWebRequestTexture.GetTexture(this.thumbUrl);
    yield return (object) request.SendWebRequest();
    Texture2D content = DownloadHandlerTexture.GetContent(request);
    this.videoThumb.sprite = Sprite.Create(content, new Rect(0.0f, 0.0f, (float) content.width, (float) content.height), new Vector2(0.5f, 0.5f), 100f);
  }
}
