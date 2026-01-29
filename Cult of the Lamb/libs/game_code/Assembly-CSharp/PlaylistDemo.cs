// Decompiled with JetBrains decompiler
// Type: PlaylistDemo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class PlaylistDemo : MonoBehaviour
{
  public YoutubeAPIManager youtubeapi;
  public Text searchField;
  public YoutubeVideoUi[] videoListUI;
  public GameObject videoUIResult;
  public GameObject mainUI;

  public void Start()
  {
    this.youtubeapi = UnityEngine.Object.FindObjectOfType<YoutubeAPIManager>();
    if (!((UnityEngine.Object) this.youtubeapi == (UnityEngine.Object) null))
      return;
    this.youtubeapi = this.gameObject.AddComponent<YoutubeAPIManager>();
  }

  public void GetPlaylist()
  {
    this.youtubeapi.GetPlaylistItems(this.searchField.text, 10, new Action<YoutubePlaylistItems[]>(this.OnGetPlaylistDone));
  }

  public void OnGetPlaylistDone(YoutubePlaylistItems[] results)
  {
    this.videoUIResult.SetActive(true);
    this.LoadVideosOnUI(results);
  }

  public void LoadVideosOnUI(YoutubePlaylistItems[] videoList)
  {
    for (int index = 0; index < videoList.Length; ++index)
    {
      this.videoListUI[index].GetComponent<YoutubeVideoUi>().videoName.text = videoList[index].snippet.title;
      this.videoListUI[index].GetComponent<YoutubeVideoUi>().videoId = videoList[index].videoId;
      this.videoListUI[index].GetComponent<YoutubeVideoUi>().thumbUrl = videoList[index].snippet.thumbnails.defaultThumbnail.url;
      this.videoListUI[index].GetComponent<YoutubeVideoUi>().LoadThumbnail();
    }
  }
}
