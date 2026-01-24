// Decompiled with JetBrains decompiler
// Type: ChannelSearchDemo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ChannelSearchDemo : MonoBehaviour
{
  public YoutubeAPIManager youtubeapi;
  public Text searchField;
  public Dropdown mainFilters;
  public GameObject[] channelListUI;
  public GameObject[] videoListUI;
  public GameObject videoUIResult;
  public GameObject channelUIResult;
  public GameObject mainUI;

  public void Start()
  {
    this.youtubeapi = UnityEngine.Object.FindObjectOfType<YoutubeAPIManager>();
    if (!((UnityEngine.Object) this.youtubeapi == (UnityEngine.Object) null))
      return;
    this.youtubeapi = this.gameObject.AddComponent<YoutubeAPIManager>();
  }

  public void SearchChannel()
  {
    YoutubeAPIManager.YoutubeSearchOrderFilter order = YoutubeAPIManager.YoutubeSearchOrderFilter.none;
    switch (this.mainFilters.value)
    {
      case 0:
        order = YoutubeAPIManager.YoutubeSearchOrderFilter.none;
        break;
      case 1:
        order = YoutubeAPIManager.YoutubeSearchOrderFilter.date;
        break;
      case 2:
        order = YoutubeAPIManager.YoutubeSearchOrderFilter.rating;
        break;
      case 3:
        order = YoutubeAPIManager.YoutubeSearchOrderFilter.relevance;
        break;
      case 4:
        order = YoutubeAPIManager.YoutubeSearchOrderFilter.title;
        break;
      case 5:
        order = YoutubeAPIManager.YoutubeSearchOrderFilter.videoCount;
        break;
      case 6:
        order = YoutubeAPIManager.YoutubeSearchOrderFilter.viewCount;
        break;
    }
    this.youtubeapi.SearchForChannels(this.searchField.text, 10, order, YoutubeAPIManager.YoutubeSafeSearchFilter.none, new Action<YoutubeChannel[]>(this.OnSearchDone));
  }

  public void OnSearchDone(YoutubeChannel[] results)
  {
    this.channelUIResult.SetActive(true);
    this.LoadChannelsOnUI(results);
  }

  public void LoadChannelsOnUI(YoutubeChannel[] videoList)
  {
    for (int index = 0; index < videoList.Length; ++index)
    {
      this.channelListUI[index].GetComponent<YoutubeChannelUI>().videoName.text = videoList[index].title;
      this.channelListUI[index].GetComponent<YoutubeChannelUI>().videoId = videoList[index].id;
      this.channelListUI[index].GetComponent<YoutubeChannelUI>().thumbUrl = videoList[index].thumbnail;
      this.channelListUI[index].GetComponent<YoutubeChannelUI>().LoadThumbnail();
    }
  }

  public void LoadChannelResult(string channelId)
  {
    this.youtubeapi.GetChannelVideos(channelId, 10, new Action<YoutubeData[]>(this.OnChannelResultLoaded));
  }

  public void OnChannelResultLoaded(YoutubeData[] videoList)
  {
    this.channelUIResult.SetActive(false);
    this.videoUIResult.SetActive(true);
    for (int index = 0; index < videoList.Length; ++index)
    {
      this.videoListUI[index].GetComponent<YoutubeVideoUi>().videoName.text = videoList[index].snippet.title;
      this.videoListUI[index].GetComponent<YoutubeVideoUi>().videoId = videoList[index].id;
      this.videoListUI[index].GetComponent<YoutubeVideoUi>().thumbUrl = videoList[index].snippet.thumbnails.defaultThumbnail.url;
      this.videoListUI[index].GetComponent<YoutubeVideoUi>().LoadThumbnail();
    }
  }
}
