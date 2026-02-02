// Decompiled with JetBrains decompiler
// Type: IndividualVideoDataDemo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

#nullable disable
public class IndividualVideoDataDemo : MonoBehaviour
{
  public YoutubeAPIManager youtubeapi;
  public Text videoIdInput;
  public Text UI_title;
  public Text UI_description;
  public Text UI_duration;
  public Text UI_likes;
  public Text UI_dislikes;
  public Text UI_favorites;
  public Text UI_comments;
  public Text UI_views;
  public Image UI_thumbnail;

  public void Start()
  {
    this.youtubeapi = UnityEngine.Object.FindObjectOfType<YoutubeAPIManager>();
    if (!((UnityEngine.Object) this.youtubeapi == (UnityEngine.Object) null))
      return;
    this.youtubeapi = this.gameObject.AddComponent<YoutubeAPIManager>();
  }

  public void GetVideoData()
  {
    this.youtubeapi.GetVideoData(this.videoIdInput.text, new Action<YoutubeData>(this.OnFinishLoadingData));
  }

  public void OnFinishLoadingData(YoutubeData result)
  {
    this.UI_title.text = result.snippet.title;
    this.UI_description.text = result.snippet.description;
    this.UI_duration.text = "Duration: " + result.contentDetails.duration.Replace("PT", "");
    this.UI_likes.text = "Likes: " + result.statistics.likeCount;
    this.UI_dislikes.text = "Dislikes: " + result.statistics.dislikeCount;
    this.UI_favorites.text = "Favs: " + result.statistics.favoriteCount;
    this.UI_comments.text = "Comments: " + result.statistics.commentCount;
    this.UI_views.text = "Views: " + result.statistics.viewCount;
    this.LoadThumbnail(result.snippet.thumbnails.defaultThumbnail.url);
  }

  public void LoadThumbnail(string url)
  {
    this.StartCoroutine((IEnumerator) this.DownloadThumb(url));
  }

  public IEnumerator DownloadThumb(string url)
  {
    UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
    yield return (object) request.SendWebRequest();
    Texture2D content = DownloadHandlerTexture.GetContent(request);
    this.UI_thumbnail.sprite = Sprite.Create(content, new Rect(0.0f, 0.0f, (float) content.width, (float) content.height), new Vector2(0.5f, 0.5f), 100f);
  }
}
